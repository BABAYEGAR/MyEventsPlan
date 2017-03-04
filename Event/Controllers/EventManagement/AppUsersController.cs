using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.EventManagement
{
    public class AppUsersController : Controller
    {
        private readonly AppUserDataContext db = new AppUserDataContext();

        // GET: AppUsers
        public ActionResult Index()
        {
            var appUsers = db.AppUsers.Include(a => a.EventPlanner).Include(a => a.Role);
            return View(appUsers.ToList());
        }

        // GET: EnableUser
        public ActionResult EnableUser(long? id)
        {
            var appUser = db.AppUsers.Find(id);
            appUser.Status = UserAccountStatus.Enabled.ToString();
            db.Entry(appUser).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: DisableUser
        public ActionResult DisableUser(long? id)
        {
            var appUser = db.AppUsers.Find(id);
            appUser.Status = UserAccountStatus.Disabled.ToString();
            db.Entry(appUser).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: AppUsers/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var appUser = db.AppUsers.Find(id);
            if (appUser == null)
                return HttpNotFound();
            return View(appUser);
        }

        // GET: AppUsers/Create
        public ActionResult Create()
        {
            ViewBag.EventPlannerId = new SelectList(db.EventPlanners, "EventPlannerId", "Firstname");
            ViewBag.RoleId = new SelectList(db.Roles, "RoleId", "Name");
            return View();
        }

        // POST: AppUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(
                 Include =
                     "AppUserId,Firstname,Lastname,Email,Mobile,Password,RoleId,EventPlannerId,ProfileImage,CreatedBy,DateCreated,DateLastModified,LastModifiedBy"
             )] AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                db.AppUsers.Add(appUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EventPlannerId = new SelectList(db.EventPlanners, "EventPlannerId", "Firstname",
                appUser.EventPlannerId);
            ViewBag.RoleId = new SelectList(db.Roles, "RoleId", "Name", appUser.RoleId);
            return View(appUser);
        }

        // GET: AppUsers/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var appUser = db.AppUsers.Find(id);
            if (appUser == null)
                return HttpNotFound();
            ViewBag.EventPlannerId = new SelectList(db.EventPlanners, "EventPlannerId", "Firstname",
                appUser.EventPlannerId);
            ViewBag.RoleId = new SelectList(db.Roles, "RoleId", "Name", appUser.RoleId);
            return View(appUser);
        }

        // POST: AppUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(
                 Include =
                     "AppUserId,Firstname,Lastname,Email,Mobile,Password,RoleId,EventPlannerId,ProfileImage,CreatedBy,DateCreated,DateLastModified,LastModifiedBy"
             )] AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EventPlannerId = new SelectList(db.EventPlanners, "EventPlannerId", "Firstname",
                appUser.EventPlannerId);
            ViewBag.RoleId = new SelectList(db.Roles, "RoleId", "Name", appUser.RoleId);
            return View(appUser);
        }

        // GET: AppUsers/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var appUser = db.AppUsers.Find(id);
            if (appUser == null)
                return HttpNotFound();
            return View(appUser);
        }

        // POST: AppUsers/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            var appUser = db.AppUsers.Find(id);
            db.AppUsers.Remove(appUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}