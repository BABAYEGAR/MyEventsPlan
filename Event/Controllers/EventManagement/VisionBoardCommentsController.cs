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
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.EventManagement
{
    public class VisionBoardCommentsController : Controller
    {
        private EventDataContext db = new EventDataContext();

        // GET: VisionBoardComments
        public ActionResult Index()
        {
            var visionBoardComments = db.VisionBoardComments.Include(v => v.VisionBoard);
            return View(visionBoardComments.ToList());
        }

        // GET: VisionBoardComments/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VisionBoardComment visionBoardComment = db.VisionBoardComments.Find(id);
            if (visionBoardComment == null)
            {
                return HttpNotFound();
            }
            return View(visionBoardComment);
        }

        // GET: VisionBoardComments/Create
        public ActionResult Create()
        {
            ViewBag.VisionBoardId = new SelectList(db.VisionBoards, "VisionBoardId", "File");
            return View();
        }

        // POST: VisionBoardComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VisionBoardCommentId,Comment,VisionBoardId,CreatedBy,DateCreated,DateLastModified,LastModifiedBy")] VisionBoardComment visionBoardComment)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                if (loggedinuser != null)
                {
                    visionBoardComment.CreatedBy = loggedinuser.AppUserId;
                    visionBoardComment.LastModifiedBy = loggedinuser.AppUserId;
                }
                visionBoardComment.DateCreated = DateTime.Now;
                visionBoardComment.DateLastModified = DateTime.Now;
            db.VisionBoardComments.Add(visionBoardComment);
                db.SaveChanges();
                TempData["display"] = "You have successfully added a comment!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Details","VisionBoards",new{id = visionBoardComment.VisionBoardId});
            }

            ViewBag.VisionBoardId = new SelectList(db.VisionBoards, "VisionBoardId", "File", visionBoardComment.VisionBoardId);
            return View(visionBoardComment);
        }

        // GET: VisionBoardComments/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VisionBoardComment visionBoardComment = db.VisionBoardComments.Find(id);
            if (visionBoardComment == null)
            {
                return HttpNotFound();
            }
            ViewBag.VisionBoardId = new SelectList(db.VisionBoards, "VisionBoardId", "File", visionBoardComment.VisionBoardId);
            return View(visionBoardComment);
        }

        // POST: VisionBoardComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VisionBoardCommentId,Comment,VisionBoardId,CreatedBy,DateCreated")] VisionBoardComment visionBoardComment)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                if (loggedinuser != null)
                {
                    visionBoardComment.LastModifiedBy = loggedinuser.AppUserId;
                }
                visionBoardComment.DateLastModified = DateTime.Now;
                db.Entry(visionBoardComment).State = EntityState.Modified;
                db.SaveChanges();
                TempData["display"] = "You have successfully modified the comment!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            ViewBag.VisionBoardId = new SelectList(db.VisionBoards, "VisionBoardId", "File", visionBoardComment.VisionBoardId);
            return View(visionBoardComment);
        }
        // POST: VisionBoardComments/Delete/5
        [HttpGet, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            VisionBoardComment visionBoardComment = db.VisionBoardComments.Find(id);
            long boardId = visionBoardComment.VisionBoardId;
            db.VisionBoardComments.Remove(visionBoardComment);
            db.SaveChanges();
            TempData["display"] = "You have successfully deleted the comment!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Details","VisionBoards",new{id = boardId});
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
