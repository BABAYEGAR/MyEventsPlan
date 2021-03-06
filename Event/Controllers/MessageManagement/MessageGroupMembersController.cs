﻿using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.AuthenticationManagement;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.MessageManagement
{
    public class MessageGroupMembersController : Controller
    {
        private readonly EventDataContext _databaseConnection = new EventDataContext();

        // GET: MessageGroupMembers
        [SessionExpire]
        public ActionResult Index()
        {
            var messageGroupMembers = _databaseConnection.MessageGroupMembers.Include(m => m.AppUser).Include(m => m.MessageGroup);
            return View(messageGroupMembers.ToList());
        }

        // GET: GroupMembers
        [SessionExpire]
        public ActionResult GroupMember(long? id)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            ViewBag.groupId = id;
            IQueryable<AppUser> users;
            if (loggedinuser != null && loggedinuser.EventPlannerId != null)
            {
                users = _databaseConnection.AppUsers.Where(n => n.CreatedBy == loggedinuser.AppUserId);
                return View(users.ToList());
            }

            return View((IQueryable<AppUser>) null);
        }

        // GET: AttendeeList
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult GroupMember(int[] table_records, FormCollection collectedValues)
        {
            var allMappings = _databaseConnection.MessageGroupMembers.ToList();
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var groupId = Convert.ToInt64(collectedValues["MessageGroupId"]);
            if (table_records != null)
            {
                var length = table_records.Length;
                for (var i = 0; i < length; i++)
                {
                    var id = table_records[i];
                    if (
                        allMappings.Any(
                            n =>
                                n.MessageGroupId == groupId && n.AppUserId == id))
                    {
                    }
                    else
                    {
                        if (loggedinuser != null)
                        {
                            var groupMembers = new MessageGroupMember
                            {
                                MessageGroupId = groupId,
                                AppUserId = id,
                                DateCreated = DateTime.Now,
                                DateLastModified = DateTime.Now,
                                LastModifiedBy = loggedinuser.AppUserId,
                                CreatedBy = loggedinuser.AppUserId
                            };

                            _databaseConnection.MessageGroupMembers.Add(groupMembers);
                            _databaseConnection.SaveChanges();

                            TempData["display"] = "you have succesfully added the users(s) to the group!";
                            TempData["notificationtype"] = NotificationType.Success.ToString();
                        }
                        else
                        {
                            TempData["login"] = "Session has expired, Login and try again!";
                            TempData["notificationtype"] = NotificationType.Success.ToString();
                            return RedirectToAction("Login", "Account");
                        }
                    }
                }
            }
            else
            {
                TempData["display"] = "no user has been selected!";
                TempData["notificationtype"] = NotificationType.Error.ToString();
                return RedirectToAction("GroupMember", new {id = groupId});
            }
            return RedirectToAction("GroupMember", new {id = groupId});
        }

        // GET: MessageGroupMembers/Details/5
        [SessionExpire]
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var messageGroupMember = _databaseConnection.MessageGroupMembers.Find(id);
            if (messageGroupMember == null)
                return HttpNotFound();
            return View(messageGroupMember);
        }

        // GET: MessageGroupMembers/Create
        [SessionExpire]
        public ActionResult Create()
        {
            ViewBag.AppUserId = new SelectList(_databaseConnection.AppUsers, "AppUserId", "Firstname");
            ViewBag.MessageGroupId = new SelectList(_databaseConnection.MessageGroups, "MessageGroupId", "Name");
            return View();
        }

        // POST: MessageGroupMembers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Create(
            [Bind(
                Include =
                    "MessageGroupMemberId,MessageGroupId,AppUserId,CreatedBy,DateCreated,DateLastModified,LastModifiedBy"
            )] MessageGroupMember messageGroupMember)
        {
            if (ModelState.IsValid)
            {
                _databaseConnection.MessageGroupMembers.Add(messageGroupMember);
                _databaseConnection.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AppUserId = new SelectList(_databaseConnection.AppUsers, "AppUserId", "Firstname", messageGroupMember.AppUserId);
            ViewBag.MessageGroupId = new SelectList(_databaseConnection.MessageGroups, "MessageGroupId", "Name",
                messageGroupMember.MessageGroupId);
            return View(messageGroupMember);
        }

        // GET: MessageGroupMembers/Edit/5
        [SessionExpire]
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var messageGroupMember = _databaseConnection.MessageGroupMembers.Find(id);
            if (messageGroupMember == null)
                return HttpNotFound();
            ViewBag.AppUserId = new SelectList(_databaseConnection.AppUsers, "AppUserId", "Firstname", messageGroupMember.AppUserId);
            ViewBag.MessageGroupId = new SelectList(_databaseConnection.MessageGroups, "MessageGroupId", "Name",
                messageGroupMember.MessageGroupId);
            return View(messageGroupMember);
        }

        // POST: MessageGroupMembers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Edit(
            [Bind(
                Include =
                    "MessageGroupMemberId,MessageGroupId,AppUserId,CreatedBy,DateCreated,DateLastModified,LastModifiedBy"
            )] MessageGroupMember messageGroupMember)
        {
            if (ModelState.IsValid)
            {
                _databaseConnection.Entry(messageGroupMember).State = EntityState.Modified;
                _databaseConnection.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AppUserId = new SelectList(_databaseConnection.AppUsers, "AppUserId", "Firstname", messageGroupMember.AppUserId);
            ViewBag.MessageGroupId = new SelectList(_databaseConnection.MessageGroups, "MessageGroupId", "Name",
                messageGroupMember.MessageGroupId);
            return View(messageGroupMember);
        }

        // GET: MessageGroupMembers/Delete/5
        [SessionExpire]
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var messageGroupMember = _databaseConnection.MessageGroupMembers.Find(id);
            if (messageGroupMember == null)
                return HttpNotFound();
            return View(messageGroupMember);
        }

        // POST: MessageGroupMembers/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult DeleteConfirmed(long id)
        {
            var messageGroupMember = _databaseConnection.MessageGroupMembers.Find(id);
            _databaseConnection.MessageGroupMembers.Remove(messageGroupMember);
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