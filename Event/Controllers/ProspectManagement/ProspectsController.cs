using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.AuthenticationManagement;
using MyEventPlan.Data.Service.EmailService;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.ProspectManagement
{
    public class ProspectsController : Controller
    {
        private readonly EventDataContext _databaseConnection = new EventDataContext();

        // GET: Prospects
        [SessionExpire]
        public ActionResult Index()
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            ViewBag.EventTypeId = new SelectList(_databaseConnection.EventTypes, "EventTypeId", "Name");
            var prospects =
                _databaseConnection.Prospects.OrderByDescending(n => n.StartDate)
                    .Include(p => p.EventType)
                    .Where(n => n.EventPlannerId == loggedinuser.EventPlannerId);
            return View(prospects.ToList());
        }

        [SessionExpire]
        public ActionResult FollowUp(FormCollection collectedValues)
        {
            var prospectId = Convert.ToInt64(collectedValues["id"]);
            var prospect = _databaseConnection.Prospects.Find(prospectId);
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var message = collectedValues["Message"];
            if (new MailerDaemon().FolowUpProspect(prospect, loggedinuser, message))
            {
                TempData["display"] = "You have successfully sent a follow up email to the prospect!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Details", "Prospects", new {id = prospectId});
            }
            return RedirectToAction("Details", "Prospects", new {id = prospectId});
        }

        // GET: Prospects/Details/5
        [SessionExpire]
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var prospect = _databaseConnection.Prospects.Find(id);

            if (prospect == null)
                return HttpNotFound();
            ViewBag.EventTypeId = new SelectList(_databaseConnection.EventTypes, "EventTypeId", "Name", prospect.EventTypeId);
            return View(prospect);
        }

        // GET: Prospects/Create
        [SessionExpire]
        public ActionResult Create()
        {
            ViewBag.EventTypeId = new SelectList(_databaseConnection.EventTypes, "EventTypeId", "Name");
            return View();
        }

        // POST: Prospects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Create(
            [Bind(Include =
                "ProspectId,Name,Color,EventTypeId,EventDate,TargetBudget,StartDate,EndDate,Email,PhoneNumber")]
            Prospect prospect, FormCollection collectedValues)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            if (ModelState.IsValid)
            {
                prospect.DateCreated = DateTime.Now;
                prospect.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    prospect.CreatedBy = loggedinuser.AppUserId;
                    prospect.LastModifiedBy = loggedinuser.AppUserId;
                    prospect.EventPlannerId = loggedinuser.EventPlannerId;
                    prospect.Status = ProspectStausEnum.Active.ToString();
                    prospect.TargetBudget = prospect.TargetBudget.Replace(",", "");
                    prospect.StartTime = Convert.ToDateTime(collectedValues["StartDate"]).ToShortTimeString();
                    prospect.EndTime = Convert.ToDateTime(collectedValues["EndDate"]).ToShortTimeString();
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                _databaseConnection.Prospects.Add(prospect);
                _databaseConnection.SaveChanges();

                TempData["display"] = "You have successfully added a prospect!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }

            ViewBag.EventTypeId = new SelectList(_databaseConnection.EventTypes, "EventTypeId", "Name", prospect.EventTypeId);
            return View(prospect);
        }

        // GET: Prospects/Details/5
        [SessionExpire]
        public ActionResult ConvertProspectToEvent(long? id)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var prospect = _databaseConnection.Prospects.Find(id);
            if (prospect == null)
                return HttpNotFound();
            var events = new Event.Data.Objects.Entities.Event();
            events.EventPlannerId = prospect.EventPlannerId;
            events.Name = prospect.Name;
            events.Color = prospect.Color;
            events.StartDate = prospect.StartDate;
            events.EndDate = prospect.EndDate;
            events.StartTime = prospect.StartTime;
            events.EndTime = prospect.EndTime;
            events.TargetBudget = prospect.TargetBudget;
            events.EventDate = prospect.EventDate;
            if (loggedinuser != null)
            {
                events.CreatedBy = loggedinuser.AppUserId;
                events.LastModifiedBy = loggedinuser.AppUserId;
            }
            events.DateCreated = DateTime.Now;
            events.DateLastModified = DateTime.Now;
            events.Status = EventStausEnum.New.ToString();
            events.EventTypeId = prospect.EventTypeId;
            _databaseConnection.Event.Add(events);
            _databaseConnection.SaveChanges();

            var prospectForDelete = _databaseConnection.Prospects.Find(id);
            _databaseConnection.Prospects.Remove(prospectForDelete);
            _databaseConnection.SaveChanges();

            TempData["display"] = "You have successfully converted the prospect to an event!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Index", "Events");
        }

        [SessionExpire]
        public ActionResult CancelProspect(long? id)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var prospect = _databaseConnection.Prospects.Find(id);
            prospect.Status = ProspectStausEnum.Cancelled.ToString();
            prospect.DateLastModified = DateTime.Now;
            if (loggedinuser != null) prospect.LastModifiedBy = loggedinuser.AppUserId;
            _databaseConnection.Entry(prospect).State = EntityState.Modified;
            _databaseConnection.SaveChanges();
            TempData["display"] = "You have successfully cancelled the prospect!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Index");
        }

        // GET: Prospects/Edit/5
        [SessionExpire]
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var prospect = _databaseConnection.Prospects.Find(id);
            if (prospect == null)
                return HttpNotFound();
            ViewBag.EventTypeId = new SelectList(_databaseConnection.EventTypes, "EventTypeId", "Name", prospect.EventTypeId);
            return View(prospect);
        }

        // POST: Prospects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Edit(
            [Bind(
                Include =
                    "ProspectId,Name,Color,Status,EventTypeId,TargetBudget,EventDate,EventPlannerId,StartDate,EndDate,CreatedBy,DateCreated,Email,PhoneNumber"
            )] Prospect prospect, FormCollection collectedValues)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            if (ModelState.IsValid)
            {
                prospect.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    prospect.LastModifiedBy = loggedinuser.AppUserId;
                    prospect.StartTime = Convert.ToDateTime(collectedValues["StartDate"]).ToShortTimeString();
                    prospect.EndTime = Convert.ToDateTime(collectedValues["EndDate"]).ToShortTimeString();
                    prospect.TargetBudget = prospect.TargetBudget.Replace(",", "");
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                _databaseConnection.Entry(prospect).State = EntityState.Modified;
                _databaseConnection.SaveChanges();
                TempData["display"] = "You have successfully modified the prospect!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            ViewBag.EventTypeId = new SelectList(_databaseConnection.EventTypes, "EventTypeId", "Name", prospect.EventTypeId);
            return View(prospect);
        }

        // GET: Prospects/Delete/5
        [SessionExpire]
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var prospect = _databaseConnection.Prospects.Find(id);
            if (prospect == null)
                return HttpNotFound();
            return View(prospect);
        }

        // POST: Prospects/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult DeleteConfirmed(long id)
        {
            var prospect = _databaseConnection.Prospects.Find(id);
            _databaseConnection.Prospects.Remove(prospect);
            _databaseConnection.SaveChanges();
            TempData["display"] = "You have deleted the prospect!";
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