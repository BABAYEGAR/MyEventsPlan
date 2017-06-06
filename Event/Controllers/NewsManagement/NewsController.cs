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

namespace MyEventPlan.Controllers.NewsManagement
{
    public class NewsController : Controller
    {
        private readonly NewsDataContext db = new NewsDataContext();
        private readonly EventDataContext dbc = new EventDataContext();

        // GET: News
        [SessionExpire]
        public ActionResult Index()
        {
            var newses = db.Newses.Include(n => n.EventPlanner);
            return View(newses.ToList());
        }

        // GET: News
        [SessionExpire]
        public ActionResult MyNewsFeeds()
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var newses =
                db.Newses.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId).Include(n => n.EventPlanner);
            ViewBag.EventId = new SelectList(db.Event.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId),
                "EventId", "Name");
            return View("Index", newses.ToList());
        }

        // GET: News/Details/5
        [SessionExpire]
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var news = db.Newses.Find(id);
            if (news == null)
                return HttpNotFound();
            return View(news);
        }

        // GET: News/Create
        [SessionExpire]
        public ActionResult Create()
        {
            ViewBag.EventId = new SelectList(db.Event, "EventId", "Name");
            return View();
        }

        // POST: News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Create([Bind(Include = "NewsId,Content")] News news)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var image = Request.Files["image"];
            if (ModelState.IsValid)
            {
                news.DateCreated = DateTime.Now;
                news.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    news.LastModifiedBy = loggedinuser.AppUserId;
                    news.CreatedBy = loggedinuser.AppUserId;
                    if (loggedinuser.EventPlannerId != null) news.EventPlannerId = (long) loggedinuser.EventPlannerId;
                    news.NewsImage = image != null && image.FileName != ""
                        ? new FileUploader().UploadFile(image, UploadType.NewsImage)
                        : null;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                db.Newses.Add(news);
                db.SaveChanges();
                return RedirectToAction("MyNewsFeeds");
            }

            ViewBag.EventId = new SelectList(db.Event.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId),
                "EventId", "Name");
            return View(news);
        }

        // GET: News/Edit/5
        [SessionExpire]
        public ActionResult Edit(long? id)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var news = db.Newses.Find(id);
            if (news == null)
                return HttpNotFound();
            ViewBag.EventId = new SelectList(db.Event.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId),
                "EventId", "Name");
            return View(news);
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Edit(
            [Bind(Include = "NewsId,Title,Content,NewsImage,EventPlannerId,EventId,CreatedBy,DateCreated")] News news)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            if (ModelState.IsValid)
            {
                news.DateCreated = DateTime.Now;
                news.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    news.LastModifiedBy = loggedinuser.AppUserId;
                    news.CreatedBy = loggedinuser.AppUserId;
                    if (loggedinuser.EventPlannerId != null) news.EventPlannerId = (long) loggedinuser.EventPlannerId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                db.Entry(news).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EventId = new SelectList(db.Event.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId),
                "EventId", "Name");
            return View(news);
        }

        // GET: News/Delete/5
        [SessionExpire]
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var news = db.Newses.Find(id);
            if (news == null)
                return HttpNotFound();
            return View(news);
        }

        // POST: News/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult DeleteConfirmed(long id)
        {
            var news = db.Newses.Find(id);
            db.Newses.Remove(news);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [SessionExpire]
        public ActionResult LikeOrDislikeANews(long? id, string like, string dislike)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var news = db.Newses.Find(id);
            var newsAction = new NewsAction();
            var actionLikeCheck = dbc.NewsActions.SingleOrDefault(n => n.AppUserId == loggedinuser.AppUserId
                                                                       && n.NewsId == news.NewsId &&
                                                                       n.Action == NewsActionEnum.Like.ToString());

            var actionDisLikeCheck = dbc.NewsActions.SingleOrDefault(n => n.AppUserId == loggedinuser.AppUserId
                                                                          && n.NewsId == news.NewsId &&
                                                                          n.Action == NewsActionEnum.Dislike
                                                                              .ToString());
            if (loggedinuser != null)
            {
                newsAction.NewsId = news.NewsId;
                newsAction.AppUserId = loggedinuser.AppUserId;
                newsAction.CreatedBy = loggedinuser.AppUserId;
                newsAction.DateLastModified = DateTime.Now;
                newsAction.DateCreated = DateTime.Now;
                newsAction.LastModifiedBy = loggedinuser.AppUserId;
            }
            if (like != null)
            {
                news.Likes = news.Likes + 1;
                newsAction.Action = NewsActionEnum.Like.ToString();
                dislike = null;
            }
            if (dislike != null)
            {
                news.Dislike = news.Dislike + 1;
                newsAction.Action = NewsActionEnum.Dislike.ToString();
            }
            if (actionLikeCheck != null && like != null)
                return PartialView("_LikeOrDislikePartial", news);
            if (actionDisLikeCheck != null && dislike != null)
                return PartialView("_LikeOrDislikePartial", news);
            dbc.Entry(news).State = EntityState.Modified;
            dbc.NewsActions.Add(newsAction);
            dbc.SaveChanges();
            return PartialView("_LikeOrDislikePartial", news);
        }

        [HttpGet]
        [SessionExpire]
        public ActionResult ReloadCompleteView(long newsId)
        {
            var news = db.Newses.Find(newsId);
            return PartialView("_LikeOrDislikePartial", news);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}