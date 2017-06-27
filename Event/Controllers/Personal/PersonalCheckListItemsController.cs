using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.AuthenticationManagement;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.Personal
{
    public class PersonalCheckListItemsController : Controller
    {
        private readonly EventDataContext _databaseConnection = new EventDataContext();

        // GET: PersonalCheckListItems
        [SessionExpire]
        public ActionResult Index(long? checkListId)
        {
            var personalCheckListItems = _databaseConnection.PersonalCheckListItems.Where(n => n.PersonalCheckListId == checkListId)
                .Include(p => p.PersonalCheckList);
            ViewBag.checkListId = checkListId;
            return View(personalCheckListItems.ToList());
        }

        // GET: PersonalCheckListItems/Details/5
        [SessionExpire]
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var personalCheckListItem = _databaseConnection.PersonalCheckListItems.Find(id);
            if (personalCheckListItem == null)
                return HttpNotFound();
            return View(personalCheckListItem);
        }

        // GET: CheckItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult CheckItem(int[] table_records, FormCollection collectedValues)
        {
            var allMappings = _databaseConnection.PersonalCheckListItems.ToList();
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
                                n.PersonalCheckListItemId == id &&
                                n.PersonalCheckListId == checkListId && n.Checked))
                    {
                    }
                    else
                    {
                        var item = _databaseConnection.PersonalCheckListItems.Find(id);
                        item.Checked = true;
                        item.DateLastModified = DateTime.Now;
                        if (loggedinuser != null) item.LastModifiedBy = loggedinuser.AppUserId;
                        _databaseConnection.Entry(item).State = EntityState.Modified;
                        _databaseConnection.SaveChanges();

                        var allItems = _databaseConnection.PersonalCheckListItems.Where(n => n.PersonalCheckListId == checkListId);
                        var checkList = _databaseConnection.PersonalCheckLists.Find(checkListId);
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

        // GET: PersonalCheckListItems/Create
        [SessionExpire]
        public ActionResult Create()
        {
            return View();
        }

        // POST: PersonalCheckListItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Create(
            [Bind(Include = "PersonalCheckListItemId,Name,Checked,PersonalCheckListId")]
            PersonalCheckListItem personalCheckListItem)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                personalCheckListItem.DateCreated = DateTime.Now;
                personalCheckListItem.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    personalCheckListItem.LastModifiedBy = loggedinuser.AppUserId;
                    personalCheckListItem.CreatedBy = loggedinuser.AppUserId;
                    personalCheckListItem.Checked = false;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                _databaseConnection.PersonalCheckListItems.Add(personalCheckListItem);
                _databaseConnection.SaveChanges();
                TempData["display"] = "Your have successfully created a new item!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index", new {checkListId = personalCheckListItem.PersonalCheckListId});
            }
            return View(personalCheckListItem);
        }

        // GET: PersonalCheckListItems/Edit/5
        [SessionExpire]
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var personalCheckListItem = _databaseConnection.PersonalCheckListItems.Find(id);
            if (personalCheckListItem == null)
                return HttpNotFound();
            ViewBag.PersonalCheckListId = new SelectList(_databaseConnection.PersonalCheckLists, "PersonalCheckListId", "Name",
                personalCheckListItem.PersonalCheckListId);
            return View(personalCheckListItem);
        }

        // POST: PersonalCheckListItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Edit(
            [Bind(Include = "PersonalCheckListItemId,Name,Checked,PersonalCheckListId,CreatedBy,DateCreated")]
            PersonalCheckListItem personalCheckListItem)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                personalCheckListItem.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    personalCheckListItem.LastModifiedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                _databaseConnection.Entry(personalCheckListItem).State = EntityState.Modified;
                _databaseConnection.SaveChanges();
                TempData["display"] = "Your have successfully modified the item!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index", new {checkListId = personalCheckListItem.PersonalCheckListId});
            }
            return View(personalCheckListItem);
        }

        // GET: PersonalCheckListItems/Delete/5
        [SessionExpire]
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var personalCheckListItem = _databaseConnection.PersonalCheckListItems.Find(id);
            if (personalCheckListItem == null)
                return HttpNotFound();
            return View(personalCheckListItem);
        }

        // POST: PersonalCheckListItems/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult DeleteConfirmed(long id)
        {
            var personalCheckListItem = _databaseConnection.PersonalCheckListItems.Find(id);
            _databaseConnection.PersonalCheckListItems.Remove(personalCheckListItem);
            _databaseConnection.SaveChanges();
            TempData["display"] = "Your have successfully deleted the item!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Index", new {checkListId = personalCheckListItem.PersonalCheckListId});
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _databaseConnection.Dispose();
            base.Dispose(disposing);
        }
    }
}