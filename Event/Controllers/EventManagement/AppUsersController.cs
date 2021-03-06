﻿using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.AuthenticationManagement;
using MyEventPlan.Data.Service.Enum;
using MyEventPlan.Data.Service.FileUploader;

namespace MyEventPlan.Controllers.EventManagement
{
    public class AppUsersController : Controller
    {
        private readonly EventDataContext _databaseConnection = new EventDataContext();

        // GET: AppUsers
        [SessionExpire]
        public ActionResult Index()
        {
            var appUsers = _databaseConnection.AppUsers.Include(a => a.EventPlanner).Include(a => a.Role);
            return View(appUsers.ToList());
        }

        // GET: EnableUser
        [SessionExpire]
        public ActionResult EnableUser(long? id)
        {
            var appUser = _databaseConnection.AppUsers.Find(id);
            appUser.Status = UserAccountStatus.Enabled.ToString();
            _databaseConnection.Entry(appUser).State = EntityState.Modified;
            _databaseConnection.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: DisableUser
        [SessionExpire]
        public ActionResult DisableUser(long? id)
        {
            var appUser = _databaseConnection.AppUsers.Find(id);
            appUser.Status = UserAccountStatus.Disabled.ToString();
            _databaseConnection.Entry(appUser).State = EntityState.Modified;
            _databaseConnection.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: AppUsers/BackgroundColor
        [SessionExpire]
        public ActionResult BackgroundColor()
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            return View(loggedinuser);
        }

        // POST: AppUsers/BackgroundColor
        [HttpPost]
        [SessionExpire]
        public ActionResult BackgroundColor(FormCollection collectedValues)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            AppUser user = null;
            if (loggedinuser != null)
            {
                user = _databaseConnection.AppUsers.Find(loggedinuser.AppUserId);
                var bgColor = typeof(BackgroundColor).GetEnumName(int.Parse(collectedValues["BackgroundColor"]));
                user.BackgroundColor = bgColor;
                _databaseConnection.Entry(user).State = EntityState.Modified;
            }
            _databaseConnection.SaveChanges();
            Session["myeventplanloggedinuser"] = user;
            TempData["display"] = "You have successfully changed your background color!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Setting", "Account");
        }

        // GET: AppUsers/ImageUpload
        [SessionExpire]
        public ActionResult ImageUpload()
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            return View(loggedinuser);
        }

        // POST: AppUsers/BackgroundColor
        [HttpPost]
        [SessionExpire]
        public ActionResult ImageUpload(FormCollection collectedValues)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var plannerUser = _databaseConnection.EventPlanners.SingleOrDefault(n => n.EventPlannerId == loggedinuser.EventPlannerId);
            EventPlanner user = null;
            var image = Request.Files["image_file"];
            if (loggedinuser != null)
            {
                user = _databaseConnection.EventPlanners.Find(loggedinuser.EventPlannerId);
                if (user != null)
                {
                    user.Logo = new FileUploader().UploadFile(image, UploadType.EventPlannerLogo);
                    _databaseConnection.Entry(user).State = EntityState.Modified;
                }
            }
            _databaseConnection.SaveChanges();
            Session["eventplanner"] = user;
            TempData["display"] = "You have successfully changed your logo/image!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Setting", "Account");
        }

        // GET: AppUsers/Details/5
        [SessionExpire]
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var appUser = _databaseConnection.AppUsers.Find(id);
            if (appUser == null)
                return HttpNotFound();
            return View(appUser);
        }

        // GET: AppUsers/Create
        [SessionExpire]
        public ActionResult Create()
        {
            ViewBag.EventPlannerId = new SelectList(_databaseConnection.EventPlanners, "EventPlannerId", "Firstname");
            ViewBag.RoleId = new SelectList(_databaseConnection.Roles, "RoleId", "Name");
            return View();
        }

        // POST: AppUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Create(
            [Bind(
                Include =
                    "AppUserId,Firstname,Lastname,Email,Mobile,Password,RoleId,BackgroundColor" +
                    ",EventPlannerId,CreatedBy,DateCreated,DateLastModified,LastModifiedBy"
            )] AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                _databaseConnection.AppUsers.Add(appUser);
                _databaseConnection.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EventPlannerId = new SelectList(_databaseConnection.EventPlanners, "EventPlannerId", "Firstname",
                appUser.EventPlannerId);
            ViewBag.RoleId = new SelectList(_databaseConnection.Roles, "RoleId", "Name", appUser.RoleId);
            return View(appUser);
        }

        // GET: AppUsers/Edit/5
        [SessionExpire]
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var appUser = _databaseConnection.AppUsers.Find(id);
            if (appUser == null)
                return HttpNotFound();
            ViewBag.EventPlannerId = new SelectList(_databaseConnection.EventPlanners, "EventPlannerId", "Firstname",
                appUser.EventPlannerId);
            ViewBag.RoleId = new SelectList(_databaseConnection.Roles, "RoleId", "Name", appUser.RoleId);
            return View(appUser);
        }

        // POST: AppUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Edit(
            [Bind(
                Include =
                    "AppUserId,Firstname,Lastname,Email,Mobile,Password,RoleId,EventPlannerId,ProfileImage,CreatedBy,DateCreated,DateLastModified,LastModifiedBy"
            )] AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                _databaseConnection.Entry(appUser).State = EntityState.Modified;
                _databaseConnection.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EventPlannerId = new SelectList(_databaseConnection.EventPlanners, "EventPlannerId", "Firstname",
                appUser.EventPlannerId);
            ViewBag.RoleId = new SelectList(_databaseConnection.Roles, "RoleId", "Name", appUser.RoleId);
            return View(appUser);
        }

        // POST: AppUsers/UpdateProfile/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult UpdateProfile(
            [Bind(
                Include =
                    "AppUserId,Firstname,Lastname,Email,Mobile,Status,Password,BackgroundColor" +
                    ",RoleId,VendorId,ClientId,EventPlannerId,CreatedBy,DateCreated"
            )] AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                appUser.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    if (loggedinuser.EventPlannerId != null)
                    {
                        var planner = _databaseConnection.EventPlanners.Find(loggedinuser.EventPlannerId);
                        if (planner != null)
                        {
                            planner.Name = appUser.Firstname + " " + appUser.Lastname;
                            planner.Email = appUser.Email;
                            planner.Mobile = appUser.Mobile;

                            _databaseConnection.Entry(planner).State = EntityState.Modified;
                        }
                        _databaseConnection.SaveChanges();
                    }
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                _databaseConnection.Entry(appUser).State = EntityState.Modified;
                _databaseConnection.SaveChanges();
                Session["myeventplanloggedinuser"] = appUser;
                TempData["display"] = "You have successfully updated your profile!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("UserProfile", "Account");
            }
            return RedirectToAction("Dashboard", "Home");
        }

        // GET: AppUsers/Delete/5
        [SessionExpire]
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var appUser = _databaseConnection.AppUsers.Find(id);
            if (appUser == null)
                return HttpNotFound();
            return View(appUser);
        }

        // POST: AppUsers/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult DeleteConfirmed(long id)
        {
            var appUser = _databaseConnection.AppUsers.Find(id);

            var subscriptionSetting = _databaseConnection.EventPlannerPackageSettings.Where(n => n.AppUserId == id);
            foreach (var item in subscriptionSetting)
                _databaseConnection.EventPlannerPackageSettings.Remove(item);
            _databaseConnection.AppUsers.Remove(appUser);
            _databaseConnection.SaveChanges();
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