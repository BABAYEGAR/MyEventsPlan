using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.AuthenticationManagement;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.EventManagement
{
    public class ContactsController : Controller
    {
        private readonly EventDataContext _databaseConnection = new EventDataContext();

        // GET: Contacts
        [SessionExpire]
        public ActionResult Index()
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var contact =
                _databaseConnection.Contacts.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId).Include(c => c.EventPlanner);
            return View(contact.ToList());
        }

        // GET: Contacts/Details/5
        [SessionExpire]
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var contact = _databaseConnection.Contacts.Find(id);
            if (contact == null)
                return HttpNotFound();
            return View(contact);
        }

        // GET: Contacts/Create
        [SessionExpire]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Create([Bind(Include = "ContactId,Title,Firstname,Lastname,Email,Mobile")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                if (loggedinuser != null) contact.EventPlannerId = loggedinuser.EventPlannerId;
                var contactExist = _databaseConnection.Contacts
                    .Where(m => m.Email == contact.Email && m.EventPlannerId == loggedinuser.EventPlannerId).ToList();
                if (loggedinuser != null)
                {
                    if (contactExist.Count > 0)
                    {
                        TempData["display"] = "A contact with the same email exist, try another email!";
                        TempData["notificationtype"] = NotificationType.Error.ToString();
                        return RedirectToAction("Index");
                    }
                    contact.CreatedBy = loggedinuser.AppUserId;
                    contact.DateCreated = DateTime.Now;
                    contact.DateLastModified = DateTime.Now;
                    contact.LastModifiedBy = loggedinuser.AppUserId;
                }
                _databaseConnection.Contacts.Add(contact);
                _databaseConnection.SaveChanges();
                TempData["display"] = "You have successfully added a contact!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            return View(contact);
        }

        // GET: Contacts/Edit/5
        [SessionExpire]
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var contact = _databaseConnection.Contacts.Find(id);
            if (contact == null)
                return HttpNotFound();
            return View(contact);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Edit(
            [Bind(Include = "ContactId,Title,Firstname,Lastname,Email,Mobile,EventPlannerId,CreatedBy,DateCreated")]
            Contact contact)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                if (loggedinuser != null)
                {
                    contact.DateLastModified = DateTime.Now;
                    contact.LastModifiedBy = loggedinuser.AppUserId;
                }
                _databaseConnection.Contacts.Add(contact);
                _databaseConnection.SaveChanges();
                _databaseConnection.Entry(contact).State = EntityState.Modified;
                _databaseConnection.SaveChanges();
                TempData["display"] = "You have successfully modified the contact!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            return View(contact);
        }

        // GET: Contacts/Delete/5
        [SessionExpire]
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var contact = _databaseConnection.Contacts.Find(id);
            if (contact == null)
                return HttpNotFound();
            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult DeleteConfirmed(long id)
        {
            var contact = _databaseConnection.Contacts.Find(id);
            _databaseConnection.Contacts.Remove(contact);
            _databaseConnection.SaveChanges();
            TempData["display"] = "You have successfully deleted the contact!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _databaseConnection.Dispose();
            base.Dispose(disposing);
        }
    }
}