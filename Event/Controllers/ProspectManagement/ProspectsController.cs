using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.ProspectManagement
{
    public class ProspectsController : Controller
    {
        private ProspectDataContext db = new ProspectDataContext();

        // GET: Prospects
        public ActionResult Index()
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var prospects = db.Prospects.OrderByDescending(n=>n.StartDate).Include(p => p.EventType).Where(n=>n.EventPlannerId == loggedinuser.EventPlannerId);
            return View(prospects.ToList());
        }

        // GET: Prospects/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prospect prospect = db.Prospects.Find(id);
            if (prospect == null)
            {
                return HttpNotFound();
            }
            return View(prospect);
        }

        // GET: Prospects/Create
        public ActionResult Create()
        {
            ViewBag.EventTypeId = new SelectList(db.EventTypes, "EventTypeId", "Name");
            return View();
        }

        // POST: Prospects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProspectId,Name,Color,EventTypeId,TargetBudget,StartDate,StartTime,EndDate,EndTime")] Prospect prospect)
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
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                db.Prospects.Add(prospect);
                db.SaveChanges();
                TempData["display"] = "You have successfully added a prospect!";
                TempData["notificationtype"] = NotificationType.Success.ToString(); 
                return RedirectToAction("Index");
            }

            ViewBag.EventTypeId = new SelectList(db.EventTypes, "EventTypeId", "Name", prospect.EventTypeId);
            return View(prospect);
        }
        public ActionResult CancelProspect(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prospect prospect = db.Prospects.Find(id);
            prospect.Status = ProspectStausEnum.Cancelled.ToString();
            db.Entry(prospect).State = EntityState.Modified;
            db.SaveChanges();
            TempData["display"] = "You have successfully cancelled the prospect!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Index");
        }

        // GET: Prospects/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prospect prospect = db.Prospects.Find(id);
            if (prospect == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventTypeId = new SelectList(db.EventTypes, "EventTypeId", "Name", prospect.EventTypeId);
            return View(prospect);
        }

        // POST: Prospects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProspectId,Name,Color,Status,EventTypeId,TargetBudget,EventPlannerId,StartDate,StartTime,EndDate,EndTime,CreatedBy,DateCreated")] Prospect prospect)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            if (ModelState.IsValid)
            {
                prospect.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    prospect.LastModifiedBy = loggedinuser.AppUserId;

                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                db.Entry(prospect).State = EntityState.Modified;
                db.SaveChanges();
                TempData["display"] = "You have successfully modified the prospect!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            ViewBag.EventTypeId = new SelectList(db.EventTypes, "EventTypeId", "Name", prospect.EventTypeId);
            return View(prospect);
        }

        // GET: Prospects/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prospect prospect = db.Prospects.Find(id);
            if (prospect == null)
            {
                return HttpNotFound();
            }
            return View(prospect);
        }

        // POST: Prospects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Prospect prospect = db.Prospects.Find(id);
            db.Prospects.Remove(prospect);
            db.SaveChanges();
            TempData["display"] = "You have deleted the prospect!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
