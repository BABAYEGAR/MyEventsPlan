﻿using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.AuthenticationManagement;
using MyEventPlan.Data.Service.Calender;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.EventManagement
{
    public class CheckListItemsController : Controller
    {
        private readonly EventDataContext _databaseConnection = new EventDataContext();

        // GET: CheckListItems
        [SessionExpire]
        public ActionResult Index(long? checkListId)
        {
            var checkListItems =
                _databaseConnection.CheckListItems.Where(n => n.CheckListId == checkListId)
                    .Include(c => c.CheckList)
                    .Include(c => c.Event);
            ViewBag.checkListId = checkListId;
            return View(checkListItems.ToList());
        }
        [HttpGet]
        public ActionResult ReloadItems(long? checkListId)
        {
            var checkListItems =
                _databaseConnection.CheckListItems.Where(n => n.CheckListId == checkListId)
                    .Include(c => c.CheckList)
                    .Include(c => c.Event);
            ViewBag.checkListId = checkListId;
            return PartialView("EventPlanning/CheckListItems", checkListItems.ToList());
        }
        // GET: CheckListItems/Details/5
        [SessionExpire]
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var checkListItem = _databaseConnection.CheckListItems.Find(id);
            if (checkListItem == null)
                return HttpNotFound();
            return View(checkListItem);
        }

        // GET: CheckItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult CheckItem(int[] table_records, FormCollection collectedValues)
        {
            var allMappings = _databaseConnection.CheckListItems.ToList();
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var checkListId = Convert.ToInt64(collectedValues["checkListId"]);
            if (table_records != null)
            {
                var length = table_records.Length;
                for (var i = 0; i < length; i++)
                {
                    var id = table_records[i];
                    if (
                        allMappings.Any(
                            n =>
                                n.CheckListItemId == id &&
                                n.CheckListId == checkListId && n.Checked))
                    {
                    }
                    else
                    {
                        var item = _databaseConnection.CheckListItems.Find(id);
                        item.Checked = true;
                        item.DateLastModified = DateTime.Now;
                        if (loggedinuser != null) item.LastModifiedBy = loggedinuser.AppUserId;
                        _databaseConnection.Entry(item).State = EntityState.Modified;
                        _databaseConnection.SaveChanges();

                        var allItems = _databaseConnection.CheckListItems.Where(n => n.CheckListId == checkListId);
                        var checkList = _databaseConnection.CheckLists.Find(checkListId);
                        if (allItems.All(n => n.Checked))
                        {
                            checkList.Status = ChecklistStatusEnum.Completed.ToString();
                            _databaseConnection.Entry(checkList).State = EntityState.Modified;
                            _databaseConnection.SaveChanges();
                        }

                        TempData["display"] = "you have succesfully checked the item(s)!";
                        TempData["notificationtype"] = NotificationType.Success.ToString();
                    }
                }
            }
            else
            {
                TempData["display"] = "no item has been selected!";
                TempData["notificationtype"] = NotificationType.Error.ToString();
                return RedirectToAction("Index", new {checkListId});
            }
            return RedirectToAction("Index", new {checkListId});
        }

        // GET: CheckListItems/Create
        [SessionExpire]
        public ActionResult Create()
        {
            return View();
        }

        // POST: CheckListItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Create(
            [Bind(Include = "CheckListItemId,Name,Checked,EventId,CheckListId")] CheckListItem checkListItem)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                checkListItem.DateCreated = DateTime.Now;
                checkListItem.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    checkListItem.LastModifiedBy = loggedinuser.AppUserId;
                    checkListItem.CreatedBy = loggedinuser.AppUserId;
                    checkListItem.Checked = false;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                _databaseConnection.CheckListItems.Add(checkListItem);
                _databaseConnection.SaveChanges();
                TempData["display"] = "Your have successfully created a new item!";
                TempData["notificationtype"] = NotificationType.Info.ToString();
                return RedirectToAction("Index", new {checkListId = checkListItem.CheckListId});
            }
            return View(checkListItem);
        }
        public bool CreateMoreItems(string Name, string EventId, string CheckListId)
        {
            try
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                var items = new CheckListItem();
                items.Name = Name;
                items.CheckListId = Convert.ToInt64(CheckListId);
                items.EventId = Convert.ToInt64(EventId);
                items.DateCreated = DateTime.Now;
                items.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    items.CreatedBy = loggedinuser.AppUserId;
                    items.LastModifiedBy = loggedinuser.AppUserId;
                }

                _databaseConnection.CheckListItems.Add(items);
                _databaseConnection.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // GET: CheckListItems/UncheckItem/5
        [SessionExpire]
        public ActionResult UncheckItem(long? id)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var checkListItem = _databaseConnection.CheckListItems.Find(id);
            if (checkListItem == null)
                return HttpNotFound();
            checkListItem.Checked = false;
            checkListItem.DateLastModified = DateTime.Now;
            if (loggedinuser != null) checkListItem.LastModifiedBy = loggedinuser.AppUserId;
            _databaseConnection.Entry(checkListItem).State = EntityState.Modified;
            _databaseConnection.SaveChanges();
            return RedirectToAction("Index", new {checkListId = checkListItem.CheckListId});
        }

        // GET: CheckListItems/Edit/5
        [SessionExpire]
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var checkListItem = _databaseConnection.CheckListItems.Find(id);
            if (checkListItem == null)
                return HttpNotFound();
            return View(checkListItem);
        }

        // POST: CheckListItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Edit(
            [Bind(Include = "CheckListItemId,Name,Checked,EventId,CheckListId,CreatedBy,DateCreated")] CheckListItem
                checkListItem)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                checkListItem.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    checkListItem.LastModifiedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                _databaseConnection.Entry(checkListItem).State = EntityState.Modified;
                _databaseConnection.SaveChanges();
                TempData["display"] = "Your have successfully modified the item!";
                TempData["notificationtype"] = NotificationType.Info.ToString();
                return RedirectToAction("Index", new {checkListId = checkListItem.CheckListId});
            }
            return View(checkListItem);
        }

        // GET: CheckListItems/Delete/5
        [SessionExpire]
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var checkListItem = _databaseConnection.CheckListItems.Find(id);
            if (checkListItem == null)
                return HttpNotFound();
            return View(checkListItem);
        }

        // POST: CheckListItems/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult DeleteConfirmed(long id)
        {
            var checkListItem = _databaseConnection.CheckListItems.Find(id);
            var checkListId = checkListItem.CheckListId;
            _databaseConnection.CheckListItems.Remove(checkListItem);
            _databaseConnection.SaveChanges();
            TempData["display"] = "Your have successfully deleted the item!";
            TempData["notificationtype"] = NotificationType.Info.ToString();
            return RedirectToAction("Index", new {checkListId});
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _databaseConnection.Dispose();
            base.Dispose(disposing);
        }
    }
}