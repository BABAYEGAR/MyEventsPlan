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
using MyEventPlan.Data.Service.AuthenticationManagement;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.EventManagement
{
    public class MealChoicesController : Controller
    {
        private EventDataContext db = new EventDataContext();

        // GET: MealChoices
        [SessionExpire]
        public ActionResult Index()
        {
            var mealChoices = db.MealChoices.Include(m => m.GuestList);
            return View(mealChoices.ToList());
        }

        // GET: MealChoices/Details/5
        [SessionExpire]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MealChoice mealChoice = db.MealChoices.Find(id);
            if (mealChoice == null)
            {
                return HttpNotFound();
            }
            return View(mealChoice);
        }

        // GET: MealChoices/Create
        [SessionExpire]
        public ActionResult Create()
        {
            ViewBag.GuestListId = new SelectList(db.GuestLists, "GuestListId", "Name");
            return View();
        }

        // POST: MealChoices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Create([Bind(Include = "MealChoiceId,Choice,GuestListId")] MealChoice mealChoice)
        {
            if (ModelState.IsValid)
            {

                db.MealChoices.Add(mealChoice);
                db.SaveChanges();
                TempData["display"] = "You have successfully added a new meal choice!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Details","GuestLists",new{id = mealChoice.GuestListId});
            }

            ViewBag.GuestListId = new SelectList(db.GuestLists, "GuestListId", "Name", mealChoice.GuestListId);
            return View(mealChoice);
        }

        // GET: MealChoices/Edit/5
        [SessionExpire]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MealChoice mealChoice = db.MealChoices.Find(id);
            if (mealChoice == null)
            {
                return HttpNotFound();
            }
            ViewBag.GuestListId = new SelectList(db.GuestLists, "GuestListId", "Name", mealChoice.GuestListId);
            return View(mealChoice);
        }

        // POST: MealChoices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Edit([Bind(Include = "MealChoiceId,Choice,GuestListId")] MealChoice mealChoice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mealChoice).State = EntityState.Modified;
                db.SaveChanges();
                TempData["display"] = "You have successfully modified the meal choice!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Details", "GuestLists", new { id = mealChoice.GuestListId });
            }
            ViewBag.GuestListId = new SelectList(db.GuestLists, "GuestListId", "Name", mealChoice.GuestListId);
            return View(mealChoice);
        }

        // GET: MealChoices/Delete/5
        [SessionExpire]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MealChoice mealChoice = db.MealChoices.Find(id);
            if (mealChoice == null)
            {
                return HttpNotFound();
            }
            return View(mealChoice);
        }

        // POST: MealChoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult DeleteConfirmed(long id)
        {
            MealChoice mealChoice = db.MealChoices.Find(id);
            long listId = id;
            db.MealChoices.Remove(mealChoice);
            db.SaveChanges();
            TempData["display"] = "You have successfully deleted the meal choice!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Details", "GuestLists", new { id = listId });
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
