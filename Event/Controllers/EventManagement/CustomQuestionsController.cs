using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.AuthenticationManagement;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.EventManagement
{
    public class CustomQuestionsController : Controller
    {
        private EventDataContext db = new EventDataContext();

        // GET: CustomQuestions
        [SessionExpire]
        public ActionResult Index()
        {
            var customQuestions = db.CustomQuestions;
            return View(customQuestions.ToList());
        }

        // GET: CustomQuestions/Details/5
        [SessionExpire]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomQuestion customQuestion = db.CustomQuestions.Find(id);
            if (customQuestion == null)
            {
                return HttpNotFound();
            }
            return View(customQuestion);
        }

        // GET: CustomQuestions/Create
        [SessionExpire]
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomQuestions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Create([Bind(Include = "CustomQuestionId,Question,Answer,GuestListId")] CustomQuestion customQuestion)
        {
            if (ModelState.IsValid)
            {
                db.CustomQuestions.Add(customQuestion);
                db.SaveChanges();
                TempData["display"] = "You have successfully added a new custom question!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Details","Guests",new{id = customQuestion.GuestId});
            }
            return View(customQuestion);
        }

        // GET: CustomQuestions/Edit/5
        [SessionExpire]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomQuestion customQuestion = db.CustomQuestions.Find(id);
            if (customQuestion == null)
            {
                return HttpNotFound();
            }
            return View(customQuestion);
        }

        // POST: CustomQuestions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Edit([Bind(Include = "CustomQuestionId,Question,Answer,GuestListId")] CustomQuestion customQuestion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customQuestion).State = EntityState.Modified;
                db.SaveChanges();
                TempData["display"] = "You have successfully modified the custom question!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Details", "Guests", new { id = customQuestion.GuestId });
            }
            //ViewBag.GuestListId = new SelectList(db.GuestLists, "GuestListId", "Name", customQuestion.GuestListId);
            return View(customQuestion);
        }

        // GET: CustomQuestions/Delete/5
        [SessionExpire]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomQuestion customQuestion = db.CustomQuestions.Find(id);
            if (customQuestion == null)
            {
                return HttpNotFound();
            }
            return View(customQuestion);
        }

        // POST: CustomQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult DeleteConfirmed(long id)
        {
            CustomQuestion customQuestion = db.CustomQuestions.Find(id);
            long listId = id;
            db.CustomQuestions.Remove(customQuestion);
            db.SaveChanges();
            TempData["display"] = "You have successfully deleted the custom question!";
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
