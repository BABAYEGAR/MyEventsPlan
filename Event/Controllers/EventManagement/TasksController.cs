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
    public class TasksController : Controller
    {
        private readonly EventDataContext _databaseConnection = new EventDataContext();

        // GET: Tasks
        [SessionExpire]
        public ActionResult Index(long? eventId)
        {
            var tasks = _databaseConnection.Tasks.Where(n => n.EventId == eventId).Include(t => t.Event).Include(t => t.Staff);
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            ViewBag.eventId = eventId;
            ViewBag.StaffId = new SelectList(_databaseConnection.Staff.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId),
                "StaffId", "DisplayName");
            return View(tasks.ToList());
        }

        // GET: Tasks/Details/5
        [SessionExpire]
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var task = _databaseConnection.Tasks.Find(id);
            if (task == null)
                return HttpNotFound();
            return View(task);
        }

        // GET: Tasks/Create
        [SessionExpire]
        public ActionResult Create()
        {
            ViewBag.EventId = new SelectList(_databaseConnection.Event, "EventId", "Name");
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Create([Bind(Include = "TaskId,Name,Description,EventId,DueDate,StaffId")] Task task)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                task.DateCreated = DateTime.Now;
                task.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    task.LastModifiedBy = loggedinuser.AppUserId;
                    task.CreatedBy = loggedinuser.AppUserId;
                    task.Status = TaskStausEnum.InProgress.ToString();
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                _databaseConnection.Tasks.Add(task);
                _databaseConnection.SaveChanges();
                TempData["task"] = "Your have successfully added a new task!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index", new {eventId = task.EventId});
            }
            return View(task);
        }

        // GET: Tasks/Edit/5
        [SessionExpire]
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var task = _databaseConnection.Tasks.Find(id);
            if (task == null)
                return HttpNotFound();
            return View(task);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Edit(
            [Bind(Include = "TaskId,Name,Description,EventId,CreatedBy,DateCreated,DueDate,Status,StaffId")] Task task)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                task.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    task.LastModifiedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                _databaseConnection.Entry(task).State = EntityState.Modified;
                _databaseConnection.SaveChanges();
                TempData["task"] = "Your have successfully added a new task!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index", new {eventId = task.EventId});
            }
            return View(task);
        }

        // GET: Tasks/Delete/5
        [SessionExpire]
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var task = _databaseConnection.Tasks.Find(id);
            if (task == null)
                return HttpNotFound();
            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult DeleteConfirmed(long id)
        {
            var task = _databaseConnection.Tasks.Find(id);
            _databaseConnection.Tasks.Remove(task);
            _databaseConnection.SaveChanges();
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