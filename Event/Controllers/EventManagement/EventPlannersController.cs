using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.Encryption;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.EventManagement
{
    public class EventPlannersController : Controller
    {
        private readonly EventPlannerDataContext db = new EventPlannerDataContext();
        private readonly AppUserDataContext dbc = new AppUserDataContext();

        // GET: EventPlanners
        public ActionResult Index()
        {
            var eventPlanners = db.EventPlanners.Include(e => e.Role);
            return View(eventPlanners.ToList());
        }

        // GET: EventPlanners/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventPlanner = db.EventPlanners.Find(id);
            if (eventPlanner == null)
                return HttpNotFound();
            return View(eventPlanner);
        }

        // GET: EventPlanners/Create
        public ActionResult Create()
        {
            ViewBag.RoleId = new SelectList(db.Roles, "RoleId", "Name");
            return View();
        }

        // POST: EventPlanners/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "EventPlannerId,Firstname,Lastname,Email,Mobile,ConfirmPassword,Password")] EventPlanner
                eventPlanner, FormCollection collectedValues)
        {
            if (ModelState.IsValid)
            {
                var password = new Md5Ecryption().ConvertStringToMd5Hash(collectedValues["ConfirmPassword"]);
                var role = db.Roles.FirstOrDefault(m => m.Name == "Event Planner");
                eventPlanner.RoleId = role?.RoleId;
                eventPlanner.Password = password;
                eventPlanner.ConfirmPassword = password;

                //create app user
                var appuser = new AppUser();

                appuser.Email = eventPlanner.Email;
                appuser.Password = eventPlanner.Password;
                appuser.Firstname = eventPlanner.Firstname;
                appuser.Lastname = eventPlanner.Lastname;
                appuser.Mobile = eventPlanner.Mobile;
                appuser.DateCreated = DateTime.Now;
                appuser.DateLastModified = DateTime.Now;
                appuser.LastModifiedBy = null;
                appuser.CreatedBy = null;

                var checkeventPlanner = db.EventPlanners.SingleOrDefault(n => n.Email == eventPlanner.Email);
                if (checkeventPlanner != null)
                {
                    TempData["planner"] = "The email already exist, try another email!";
                    TempData["notificationtype"] = NotificationType.Error.ToString();
                    return RedirectToAction("Create", eventPlanner);
                }
                db.EventPlanners.Add(eventPlanner);
                db.SaveChanges();


                appuser.EventPlannerId = eventPlanner.EventPlannerId;
                appuser.RoleId = eventPlanner.RoleId;


                dbc.AppUsers.Add(appuser);
                dbc.SaveChanges();
                TempData["login"] = "You have successfully signed up to PlanMyLeave type!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Login", "Account");
            }

            return View(eventPlanner);
        }

        // GET: EventPlanners/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventPlanner = db.EventPlanners.Find(id);
            if (eventPlanner == null)
                return HttpNotFound();
            ViewBag.RoleId = new SelectList(db.Roles, "RoleId", "Name", eventPlanner.RoleId);
            return View(eventPlanner);
        }

        // POST: EventPlanners/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "EventPlannerId,Firstname,Lastname,Email,Mobile,RoleId")] EventPlanner eventPlanner)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eventPlanner).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(eventPlanner);
        }

        // GET: EventPlanners/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventPlanner = db.EventPlanners.Find(id);
            if (eventPlanner == null)
                return HttpNotFound();
            return View(eventPlanner);
        }

        // POST: EventPlanners/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            var eventPlanner = db.EventPlanners.Find(id);
            db.EventPlanners.Remove(eventPlanner);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}