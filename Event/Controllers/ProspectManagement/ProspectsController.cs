using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;

namespace MyEventPlan.Controllers.ProspectManagement
{
    public class ProspectsController : Controller
    {
        private ProspectDataContext db = new ProspectDataContext();

        // GET: Prospects
        public ActionResult Index()
        {
            var prospects = db.Prospects.Include(p => p.EventType);
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
            if (ModelState.IsValid)
            {
                prospect.DateCreated = DateTime.Now;
                prospect.DateLastModified = DateTime.Now;
                prospect.CreatedBy = null;
                prospect.LastModifiedBy = null;
                db.Prospects.Add(prospect);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EventTypeId = new SelectList(db.EventTypes, "EventTypeId", "Name", prospect.EventTypeId);
            return View(prospect);
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
        public ActionResult Edit([Bind(Include = "ProspectId,Name,Color,EventTypeId,TargetBudget,StartDate,StartTime,EndDate,EndTime,CreatedBy,DateCreated")] Prospect prospect)
        {
            if (ModelState.IsValid)
            {
                prospect.DateLastModified = DateTime.Now;
                prospect.LastModifiedBy = null;
                db.Entry(prospect).State = EntityState.Modified;
                db.SaveChanges();
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
