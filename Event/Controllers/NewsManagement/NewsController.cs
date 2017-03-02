using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.Enum;
using MyEventPlan.Data.Service.FileUploader;

namespace MyEventPlan.Controllers.NewsManagement
{
    public class NewsController : Controller
    {
        private NewsDataContext db = new NewsDataContext();

        // GET: News
        public ActionResult Index()
        {
            var newses = db.Newses.Include(n => n.EventPlanner);
            return View(newses.ToList());
        }
        // GET: News
        public ActionResult MyNewsFeeds()
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var newses = db.Newses.Where(n=>n.EventPlannerId == loggedinuser.EventPlannerId).Include(n => n.EventPlanner);
            ViewBag.EventId = new SelectList(db.Event.Where(n=>n.EventPlannerId == loggedinuser.EventPlannerId), "EventId", "Name");
            return View("Index",newses.ToList());
        }
        // GET: News/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.Newses.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // GET: News/Create
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
        public ActionResult Create([Bind(Include = "NewsId,Title,Content,EventId")] News news)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            HttpPostedFileBase image = Request.Files["image"];
            if (ModelState.IsValid)
            {
                
                news.DateCreated = DateTime.Now;
                news.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    news.LastModifiedBy = loggedinuser.AppUserId;
                    news.CreatedBy = loggedinuser.AppUserId;
                    if (loggedinuser.EventPlannerId != null) news.EventPlannerId = (long) loggedinuser.EventPlannerId;
                    news.NewsImage = image != null && image.FileName != "" ? new FileUploader().UploadFile(image, UploadType.NewsImage) : null;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                db.Newses.Add(news);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EventId = new SelectList(db.Event.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId), "EventId", "Name");
            return View(news);
        }

        // GET: News/Edit/5
        public ActionResult Edit(long? id)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.Newses.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventId = new SelectList(db.Event.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId), "EventId", "Name");
            return View(news);
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NewsId,Title,Content,NewsImage,EventPlannerId,EventId,CreatedBy,DateCreated")] News news)
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
                    if (loggedinuser.EventPlannerId != null) news.EventPlannerId = (long)loggedinuser.EventPlannerId;
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
            ViewBag.EventId = new SelectList(db.Event.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId), "EventId", "Name");
            return View(news);
        }

        // GET: News/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.Newses.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            News news = db.Newses.Find(id);
            db.Newses.Remove(news);
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
