﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.MappingManagement
{
    public class StaffEventMappingsController : Controller
    {
        private StaffEventMappingDataContext db = new StaffEventMappingDataContext();

        // GET: StaffEventMappings
        public ActionResult Index(long? id)
        {
            ViewBag.Event = id;
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var mappings = db.StaffEventMapping.Where(n => n.EventId == id && n.EventPlannerId == loggedinuser.EventPlannerId);
            var vedors =
                from a in db.Staff
                join b in mappings on a.StaffId equals b.StaffId
                where a.EventPlannerId == loggedinuser.EventPlannerId
                select a;

            ViewBag.StaffId = new SelectList(db.Staff.Except(vedors), "StaffId", "Firstname");
            var staffEventMapping = db.StaffEventMapping.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId).Include(e => e.Event).Include(e => e.Staff);
            return View(staffEventMapping.Where(n => n.EventId == id).ToList());
        }

        // GET: StaffEventMappings/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffEventMapping staffEventMapping = db.StaffEventMapping.Find(id);
            if (staffEventMapping == null)
            {
                return HttpNotFound();
            }
            return View(staffEventMapping);
        }

        // GET: StaffEventMappings/Create
        public ActionResult Create()
        {
            ViewBag.EventId = new SelectList(db.Event, "EventId", "Name");
            ViewBag.EventPlannerId = new SelectList(db.EventPlanner, "EventPlannerId", "Firstname");
            ViewBag.StaffId = new SelectList(db.Staff, "StaffId", "Firstname");
            return View();
        }

        // POST: StaffEventMappings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StaffEventMappingId,EventId,StaffId")] StaffEventMapping staffEventMapping)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var eventId = ViewBag.Event;
            if (ModelState.IsValid)
            {
                staffEventMapping.DateCreated = DateTime.Now;
                staffEventMapping.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    staffEventMapping.CreatedBy = loggedinuser.AppUserId;
                    staffEventMapping.LastModifiedBy = loggedinuser.AppUserId;
                    staffEventMapping.EventPlannerId = loggedinuser.EventPlannerId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                var id = staffEventMapping.EventId;
                db.StaffEventMapping.Add(staffEventMapping);
                db.SaveChanges();
                TempData["mapping"] = "You have successfully assigned the staff to the event!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                ViewBag.StaffId = new SelectList(db.Staff, "StaffId", "Firstname", staffEventMapping.StaffId);
                return View("Index", db.StaffEventMapping.Where(n => n.EventId == id).ToList());
            }

            ViewBag.EventId = new SelectList(db.Event, "EventId", "Name", staffEventMapping.EventId);
            ViewBag.EventPlannerId = new SelectList(db.EventPlanner, "EventPlannerId", "Firstname", staffEventMapping.EventPlannerId);
            ViewBag.StaffId = new SelectList(db.Staff, "StaffId", "Firstname", staffEventMapping.StaffId);
            return View(staffEventMapping);
        }

        // GET: StaffEventMappings/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffEventMapping staffEventMapping = db.StaffEventMapping.Find(id);
            if (staffEventMapping == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventId = new SelectList(db.Event, "EventId", "Name", staffEventMapping.EventId);
            ViewBag.EventPlannerId = new SelectList(db.EventPlanner, "EventPlannerId", "Firstname", staffEventMapping.EventPlannerId);
            ViewBag.StaffId = new SelectList(db.Staff, "StaffId", "Firstname", staffEventMapping.StaffId);
            return View(staffEventMapping);
        }

        // POST: StaffEventMappings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StaffEventMappingId,EventId,StaffId,EventPlannerId,CreatedBy,DateCreated,DateLastModified,LastModifiedBy")] StaffEventMapping staffEventMapping)
        {
            if (ModelState.IsValid)
            {
                db.Entry(staffEventMapping).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EventId = new SelectList(db.Event, "EventId", "Name", staffEventMapping.EventId);
            ViewBag.EventPlannerId = new SelectList(db.EventPlanner, "EventPlannerId", "Firstname", staffEventMapping.EventPlannerId);
            ViewBag.StaffId = new SelectList(db.Staff, "StaffId", "Firstname", staffEventMapping.StaffId);
            return View(staffEventMapping);
        }

        // GET: StaffEventMappings/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffEventMapping staffEventMapping = db.StaffEventMapping.Find(id);
            if (staffEventMapping == null)
            {
                return HttpNotFound();
            }
            return View(staffEventMapping);
        }

        // POST: StaffEventMappings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            StaffEventMapping staffEventMapping = db.StaffEventMapping.Find(id);
            db.StaffEventMapping.Remove(staffEventMapping);
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