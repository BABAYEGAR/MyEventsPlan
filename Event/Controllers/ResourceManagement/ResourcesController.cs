using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.AuthenticationManagement;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.ResourceManagement
{
    public class ResourcesController : Controller
    {
        private readonly EventDataContext _databaseConnection = new EventDataContext();

        // GET: Resources
        [SessionExpire]
        public ActionResult Index()
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var resources = _databaseConnection.Resources.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId);
            return View(resources.ToList());
        }
        [SessionExpire]
        public ActionResult ViewEvents(long id)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var resources = _databaseConnection.EventResourceMapping.Where(n => n.ResourceId == id);
            return View(resources.ToList());
        }
        // GET: Resources/Details/5
        [SessionExpire]
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var resource = _databaseConnection.Resources.Find(id);
            if (resource == null)
                return HttpNotFound();
            return View(resource);
        }

        // GET: Resources/Create
        [SessionExpire]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Resources/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Create([Bind(Include = "ResourceId,Name,Quantity")] Resource resource)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            if (ModelState.IsValid)
            {
                resource.DateCreated = DateTime.Now;
                resource.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    resource.CreatedBy = loggedinuser.AppUserId;
                    resource.LastModifiedBy = loggedinuser.AppUserId;
                    if (loggedinuser.EventPlannerId != null)
                        resource.EventPlannerId = (long) loggedinuser.EventPlannerId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                _databaseConnection.Resources.Add(resource);
                _databaseConnection.SaveChanges();
                TempData["display"] = "You have successfully added an item to your inventory!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            return View(resource);
        }

        // GET: Resources/Edit/5
        [SessionExpire]
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var resource = _databaseConnection.Resources.Find(id);
            if (resource == null)
                return HttpNotFound();
            ;
            return View(resource);
        }

        // POST: Resources/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Edit(
            [Bind(Include = "ResourceId,Name,Quantity,EventPlannerId,CreatedBy,DateCreated")] Resource resource)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            if (ModelState.IsValid)
            {
                resource.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    resource.LastModifiedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                _databaseConnection.Entry(resource).State = EntityState.Modified;
                _databaseConnection.SaveChanges();
                TempData["display"] = "You have successfully modified the item in your inventory!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            return View(resource);
        }

        // GET: Resources/Delete/5
        [SessionExpire]
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var resource = _databaseConnection.Resources.Find(id);
            if (resource == null)
                return HttpNotFound();
            return View(resource);
        }

        // POST: Resources/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult DeleteConfirmed(long id)
        {
            var resource = _databaseConnection.Resources.Find(id);
            _databaseConnection.Resources.Remove(resource);
            _databaseConnection.SaveChanges();
            TempData["display"] = "You have successfully deleted the item in your inventory!";
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