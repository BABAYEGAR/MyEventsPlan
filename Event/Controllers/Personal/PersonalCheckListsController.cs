using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.AuthenticationManagement;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.Personal
{
    public class PersonalCheckListsController : Controller
    {
        private readonly EventDataContext _databaseConnection = new EventDataContext();

        // GET: PersonalCheckLists
        [SessionExpire]
        public ActionResult Index()
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var personalCheckLists = _databaseConnection.PersonalCheckLists.Where(n => n.AppUserId == loggedinuser.AppUserId)
                .Include(p => p.AppUser);
            return View(personalCheckLists.ToList());
        }

        // GET: PersonalCheckLists/Details/5
        [SessionExpire]
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var personalCheckList = _databaseConnection.PersonalCheckLists.Find(id);
            if (personalCheckList == null)
                return HttpNotFound();
            return View(personalCheckList);
        }

        // GET: PersonalCheckLists/Create
        [SessionExpire]
        public ActionResult Create()
        {
            ViewBag.AppUserId = new SelectList(_databaseConnection.AppUsers, "AppUserId", "Firstname");
            return View();
        }

        // POST: PersonalCheckLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Create(
            [Bind(Include = "PersonalCheckListId,Name,Status,AppUserId")] PersonalCheckList personalCheckList)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                personalCheckList.DateCreated = DateTime.Now;
                personalCheckList.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    personalCheckList.LastModifiedBy = loggedinuser.AppUserId;
                    personalCheckList.CreatedBy = loggedinuser.AppUserId;
                    personalCheckList.Status = ChecklistStatusEnum.Incomplete.ToString();
                    personalCheckList.AppUserId = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                _databaseConnection.PersonalCheckLists.Add(personalCheckList);
                _databaseConnection.SaveChanges();
                TempData["display"] = "Your have successfully created a new list!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }

            ViewBag.AppUserId = new SelectList(_databaseConnection.AppUsers, "AppUserId", "Firstname", personalCheckList.AppUserId);
            return View(personalCheckList);
        }

        // GET: PersonalCheckLists/Edit/5
        [SessionExpire]
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var personalCheckList = _databaseConnection.PersonalCheckLists.Find(id);
            if (personalCheckList == null)
                return HttpNotFound();
            ViewBag.AppUserId = new SelectList(_databaseConnection.AppUsers, "AppUserId", "Firstname", personalCheckList.AppUserId);
            return View(personalCheckList);
        }

        // POST: PersonalCheckLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Edit(
            [Bind(Include = "PersonalCheckListId,Name,Status,AppUserId,CreatedBy,DateCreated")]
            PersonalCheckList personalCheckList)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                personalCheckList.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    personalCheckList.LastModifiedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                _databaseConnection.Entry(personalCheckList).State = EntityState.Modified;
                _databaseConnection.SaveChanges();
                TempData["display"] = "Your have successfully modified the list!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            return View(personalCheckList);
        }

        // GET: PersonalCheckLists/Delete/5
        [SessionExpire]
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var personalCheckList = _databaseConnection.PersonalCheckLists.Find(id);
            if (personalCheckList == null)
                return HttpNotFound();
            return View(personalCheckList);
        }

        // POST: PersonalCheckLists/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult DeleteConfirmed(long id)
        {
            var personalCheckList = _databaseConnection.PersonalCheckLists.Find(id);
            _databaseConnection.PersonalCheckLists.Remove(personalCheckList);
            _databaseConnection.SaveChanges();
            TempData["display"] = "Your have successfully deleted the list!";
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