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
    public class CheckListsController : Controller
    {
        private readonly CheckListDataContext db = new CheckListDataContext();

        // GET: CheckLists
        [SessionExpire]
        public ActionResult Index(long? eventId)
        {
            var checkLists = db.CheckLists.Where(n => n.EventId == eventId).Include(c => c.Event);
            ViewBag.eventId = eventId;
            return View(checkLists.ToList());
        }

        // GET: CheckLists/Details/5
        [SessionExpire]
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var checkList = db.CheckLists.Find(id);
            if (checkList == null)
                return HttpNotFound();
            return View(checkList);
        }

        // GET: CheckLists/Create
        [SessionExpire]
        public ActionResult Create()
        {
            return View();
        }

        // POST: CheckLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Create([Bind(Include = "CheckListId,Name,EventId")] CheckList checkList)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                checkList.DateCreated = DateTime.Now;
                checkList.DateLastModified = DateTime.Now;
                var listExist = db.CheckLists.Where(m => m.EventId == checkList.EventId && m.Name == checkList.Name)
                    .ToList();
                if (loggedinuser != null)
                {
                    if (listExist.Count > 0)
                    {
                        TempData["display"] = "A check-list with the same name exist, try another name!";
                        TempData["notificationtype"] = NotificationType.Error.ToString();
                        return RedirectToAction("Index", new {eventId = checkList.EventId});
                    }
                    checkList.LastModifiedBy = loggedinuser.AppUserId;
                    checkList.CreatedBy = loggedinuser.AppUserId;
                    checkList.Status = ChecklistStatusEnum.Incomplete.ToString();
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                db.CheckLists.Add(checkList);
                db.SaveChanges();
                TempData["display"] = "Your have successfully created a new list!";
                TempData["notificationtype"] = NotificationType.Info.ToString();
                return RedirectToAction("Index", new {eventId = checkList.EventId});
            }
            return View(checkList);
        }

        // GET: CheckLists/Edit/5
        [SessionExpire]
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var checkList = db.CheckLists.Find(id);
            if (checkList == null)
                return HttpNotFound();
            return View(checkList);
        }

        // POST: CheckLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Edit(
            [Bind(Include = "CheckListId,Status,Name,EventId,CreatedBy,DateCreated")] CheckList checkList)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                checkList.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    checkList.LastModifiedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                db.Entry(checkList).State = EntityState.Modified;
                db.SaveChanges();
                TempData["display"] = "Your have successfully modified the list!";
                TempData["notificationtype"] = NotificationType.Info.ToString();
                return RedirectToAction("Index", new {eventId = checkList.EventId});
            }
            return View(checkList);
        }

        // GET: CheckLists/Delete/5
        [SessionExpire]
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var checkList = db.CheckLists.Find(id);
            if (checkList == null)
                return HttpNotFound();
            return View(checkList);
        }

        // POST: CheckLists/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult DeleteConfirmed(long id)
        {
            var checkList = db.CheckLists.Find(id);
            var eventId = checkList.EventId;
            db.CheckLists.Remove(checkList);
            db.SaveChanges();
            TempData["display"] = "Your have successfully deleted the list!";
            TempData["notificationtype"] = NotificationType.Info.ToString();
            return RedirectToAction("Index", new {eventId});
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}