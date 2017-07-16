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

namespace MyEventPlan.Controllers.EventManagement
{
    public class VisionBoardsController : Controller
    {
        private EventDataContext db = new EventDataContext();

        // GET: VisionBoards
        public ActionResult Index()
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            return View(db.VisionBoards.Where(n=>n.CreatedBy == loggedinuser.AppUserId).ToList());
        }

        // GET: VisionBoards/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VisionBoard visionBoard = db.VisionBoards.Find(id);
            ViewBag.boardId = id;
            if (visionBoard == null)
            {
                return HttpNotFound();
            }
            return View(visionBoard);
        }

        // GET: VisionBoards/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VisionBoards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VisionBoardId,File,Title,Description")] VisionBoard visionBoard)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            HttpPostedFileBase file = Request.Files["File"];
            if (ModelState.IsValid)
            {
                if (loggedinuser != null)
                {
                    visionBoard.CreatedBy = loggedinuser.AppUserId;
                    visionBoard.LastModifiedBy = loggedinuser.AppUserId;
                    visionBoard.File = new FileUploader().UploadFile(file,UploadType.VisionBoard);
                }
                visionBoard.DateCreated = DateTime.Now;
                visionBoard.DateLastModified = DateTime.Now;
                db.VisionBoards.Add(visionBoard);
                db.SaveChanges();
                TempData["display"] = "You have successfully added a vision board!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }

            return View(visionBoard);
        }

        // GET: VisionBoards/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VisionBoard visionBoard = db.VisionBoards.Find(id);
           
            if (visionBoard == null)
            {
                return HttpNotFound();
            }
            return View(visionBoard);
        }

        // POST: VisionBoards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VisionBoardId,File,Title,Description,CreatedBy,DateCreated")] VisionBoard visionBoard)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                if (loggedinuser != null) visionBoard.LastModifiedBy = loggedinuser.AppUserId;
                visionBoard.DateLastModified = DateTime.Now;
                db.Entry(visionBoard).State = EntityState.Modified;
                db.SaveChanges();
                TempData["display"] = "You have successfully modified the vision board!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            return View(visionBoard);
        }

        // GET: VisionBoards/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VisionBoard visionBoard = db.VisionBoards.Find(id);
            if (visionBoard == null)
            {
                return HttpNotFound();
            }
            return View(visionBoard);
        }

        // POST: VisionBoards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            VisionBoard visionBoard = db.VisionBoards.Find(id);
            
            db.VisionBoards.Remove(visionBoard);
            db.SaveChanges();
            TempData["display"] = "You have successfully deleted the vision board!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
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
