using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.AuthenticationManagement;
using MyEventPlan.Data.Service.EmailService;
using MyEventPlan.Data.Service.Encryption;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.EventManagement
{
    public class EventPlannersController : Controller
    {
        private readonly EventPlannerDataContext db = new EventPlannerDataContext();
        private readonly AppUserDataContext dbc = new AppUserDataContext();
        private readonly EventDataContext dbd = new EventDataContext();

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
        public ActionResult Create(string type)
        {
            ViewBag.type = type;
            ViewBag.LocationId = new SelectList(dbd.Locations, "LocationId", "Name");
            return View();
        }

        // POST: EventPlanners/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "EventPlannerId,Name,LocationId,Email,Mobile,ConfirmPassword,Password")] EventPlanner
                eventPlanner, FormCollection collectedValues)
        {
            if (ModelState.IsValid)
            {
                var password = new Md5Ecryption().ConvertStringToMd5Hash(collectedValues["ConfirmPassword"]);
                var role = db.Roles.FirstOrDefault(m => m.Name == "Event Planner");
                eventPlanner.RoleId = role?.RoleId;
                eventPlanner.Password = password;
                eventPlanner.ConfirmPassword = password;
                eventPlanner.Type = collectedValues["Type"];
                if (dbc.AppUsers.Any(n => n.Email == eventPlanner.Email))
                {
                    TempData["display"] = "The email entered already exist! Try another one!";
                    TempData["notificationtype"] = NotificationType.Error.ToString();
                    return View(eventPlanner);
                }
                //create app user
                var appuser = new AppUser();

                appuser.Email = eventPlanner.Email;
                appuser.Password = new Hashing().HashPassword(eventPlanner.Password);
                appuser.Firstname = eventPlanner.Name;
                appuser.Lastname = eventPlanner.Name;
                appuser.Mobile = eventPlanner.Mobile;
                appuser.DateCreated = DateTime.Now;
                appuser.DateLastModified = DateTime.Now;
                appuser.LastModifiedBy = null;
                appuser.CreatedBy = null;
                appuser.Verified = false;

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

                var eventPlannerSetting = new EventPlannerPackageSetting();
                eventPlannerSetting.EventPlannerId = eventPlanner.EventPlannerId;
                eventPlannerSetting.AppUserId = appuser.AppUserId;
                eventPlannerSetting.Status = PackageStatusEnum.Active.ToString();
                eventPlannerSetting.SubscribedEvent = 0;
                eventPlannerSetting.AllowedEvent = 10;
                eventPlannerSetting.EventPlannerPackageId = null;
                eventPlannerSetting.CreatedBy = null;
                eventPlannerSetting.LastModifiedBy = null;
                eventPlannerSetting.DateCreated = DateTime.Now;
                eventPlannerSetting.DateLastModified = DateTime.Now;

                dbd.EventPlannerPackageSettings.Add(eventPlannerSetting);
                dbd.SaveChanges();
                new MailerDaemon().NewEventPlanner(eventPlanner, appuser.AppUserId);
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
            [Bind(Include = "EventPlannerId,Firstname,Lastname,Email,Mobile,BusinessName,BusinessContact,Type,RoleId")] EventPlanner eventPlanner)
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