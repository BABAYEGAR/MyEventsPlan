﻿using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.AuthenticationManagement;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.MappingManagement
{
    public class StaffEventMappingsController : Controller
    {
        private readonly EventDataContext _databaseConnection = new EventDataContext();

        // GET: StaffEventMappings
        [SessionExpire]
        public ActionResult Index(long? id)
        {
            ViewBag.Event = id;
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var mappings =
                _databaseConnection.StaffEventMapping.Where(n => n.EventId == id && n.EventPlannerId == loggedinuser.EventPlannerId);
            var vedors =
                from a in _databaseConnection.Staff
                join b in mappings on a.StaffId equals b.StaffId
                where a.EventPlannerId == loggedinuser.EventPlannerId
                select a;

            ViewBag.StaffId = new SelectList(_databaseConnection.Staff.Except(vedors), "StaffId", "DisplayName");
            var staffEventMapping =
                _databaseConnection.StaffEventMapping.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId)
                    .Include(e => e.Event)
                    .Include(e => e.Staff);
            return View(staffEventMapping.Where(n => n.EventId == id).ToList());
        }

        // GET: StaffEventMappings/Details/5
        [SessionExpire]
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var staffEventMapping = _databaseConnection.StaffEventMapping.Find(id);
            if (staffEventMapping == null)
                return HttpNotFound();
            return View(staffEventMapping);
        }

        // GET: StaffEventMappings/Create
        [SessionExpire]
        public ActionResult Create()
        {
            ViewBag.EventId = new SelectList(_databaseConnection.Event, "EventId", "Name");
            ViewBag.EventPlannerId = new SelectList(_databaseConnection.EventPlanners, "EventPlannerId", "Firstname");
            ViewBag.StaffId = new SelectList(_databaseConnection.Staff, "StaffId", "Firstname");
            return View();
        }

        // POST: StaffEventMappings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Create(
            [Bind(Include = "StaffEventMappingId,EventId,StaffId")] StaffEventMapping staffEventMapping,
            FormCollection collectedValues)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var eventId = Convert.ToInt64(collectedValues["EventId"]);
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
                _databaseConnection.StaffEventMapping.Add(staffEventMapping);
                _databaseConnection.SaveChanges();
                TempData["mapping"] = "You have successfully assigned the staff to the event!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index", new {id = eventId});
            }
            var mappings =
                _databaseConnection.StaffEventMapping.Where(
                    n => n.EventId == eventId && n.EventPlannerId == loggedinuser.EventPlannerId);
            var vedors =
                from a in _databaseConnection.Staff
                join b in mappings on a.StaffId equals b.StaffId
                where a.EventPlannerId == loggedinuser.EventPlannerId
                select a;

            ViewBag.StaffId = new SelectList(_databaseConnection.Staff.Except(vedors), "StaffId", "Firstname");
            return View(staffEventMapping);
        }

        // GET: StaffEventMappings/Edit/5
        [SessionExpire]
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var staffEventMapping = _databaseConnection.StaffEventMapping.Find(id);
            if (staffEventMapping == null)
                return HttpNotFound();
            ViewBag.EventId = new SelectList(_databaseConnection.Event, "EventId", "Name", staffEventMapping.EventId);
            ViewBag.EventPlannerId = new SelectList(_databaseConnection.EventPlanners, "EventPlannerId", "Firstname",
                staffEventMapping.EventPlannerId);
            ViewBag.StaffId = new SelectList(_databaseConnection.Staff, "StaffId", "Firstname", staffEventMapping.StaffId);
            return View(staffEventMapping);
        }

        // POST: StaffEventMappings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Edit(
            [Bind(
                Include =
                    "StaffEventMappingId,EventId,StaffId,EventPlannerId,CreatedBy,DateCreated,DateLastModified,LastModifiedBy"
            )] StaffEventMapping staffEventMapping)
        {
            if (ModelState.IsValid)
            {
                _databaseConnection.Entry(staffEventMapping).State = EntityState.Modified;
                _databaseConnection.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EventId = new SelectList(_databaseConnection.Event, "EventId", "Name", staffEventMapping.EventId);
            ViewBag.EventPlannerId = new SelectList(_databaseConnection.EventPlanners, "EventPlannerId", "Firstname",
                staffEventMapping.EventPlannerId);
            ViewBag.StaffId = new SelectList(_databaseConnection.Staff, "StaffId", "Firstname", staffEventMapping.StaffId);
            return View(staffEventMapping);
        }

        // GET: StaffEventMappings/Delete/5
        [SessionExpire]
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var staffEventMapping = _databaseConnection.StaffEventMapping.Find(id);
            if (staffEventMapping == null)
                return HttpNotFound();
            return View(staffEventMapping);
        }

        // POST: StaffEventMappings/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult DeleteConfirmed(long id)
        {
            var staffEventMapping = _databaseConnection.StaffEventMapping.Find(id);
            var eventId = staffEventMapping.EventId;
            _databaseConnection.StaffEventMapping.Remove(staffEventMapping);
            _databaseConnection.SaveChanges();
            return RedirectToAction("Index", new {id = eventId});
        }

        // POST: StaffEventMappings/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult UnassignStaff(long id)
        {
            var staffEventMapping = _databaseConnection.StaffEventMapping.Find(id);
            var eventId = staffEventMapping.EventId;
            _databaseConnection.StaffEventMapping.Remove(staffEventMapping);
            _databaseConnection.SaveChanges();
            return RedirectToAction("Index", new {id = eventId});
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _databaseConnection.Dispose();
            base.Dispose(disposing);
        }
    }
}