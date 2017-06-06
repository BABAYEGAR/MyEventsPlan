using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.AuthenticationManagement;
using MyEventPlan.Data.Service.Encryption;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.EventManagement
{
    public class StaffsController : Controller
    {
        private readonly StaffDataContext db = new StaffDataContext();

        // GET: Staffs
        [SessionExpire]
        public ActionResult Index()
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var staff = db.Staff.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId).Include(s => s.Role);
            return View(staff.ToList());
        }

        [SessionExpire]
        public ActionResult ActivateStaff(long? id)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var staff =
                db.Staff.SingleOrDefault(n => n.EventPlannerId == loggedinuser.EventPlannerId && n.StaffId == id);
            if (staff != null)
                staff.Status = StaffStatus.Activated.ToString();
            TempData["display"] = "You have successfully activated the staff!";
            TempData["notificationtype"] = NotificationType.Success.ToString();

            db.Entry(staff).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [SessionExpire]
        public ActionResult DeActivateStaff(long? id)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var staff =
                db.Staff.SingleOrDefault(n => n.EventPlannerId == loggedinuser.EventPlannerId && n.StaffId == id);
            if (staff != null)
                staff.Status = StaffStatus.Deactivated.ToString();
            TempData["display"] = "You have successfully activated the staff!";
            TempData["notificationtype"] = NotificationType.Success.ToString();

            db.Entry(staff).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Staffs/Details/5
        [SessionExpire]
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var staff = db.Staff.Find(id);
            if (staff == null)
                return HttpNotFound();
            return View(staff);
        }

        // GET: Staffs/Create
        [SessionExpire]
        public ActionResult Create()
        {
            //ViewBag.RoleId = new SelectList(db.Roles, "RoleId", "Name");
            return View();
        }

        // POST: Staffs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Create([Bind(Include = "StaffId,Firstname,Lastname,Email,Mobile")] Staff staff)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var role = db.Roles.SingleOrDefault(n => n.Name == "Staff");
            var listExist = db.Staff.Where(m => m.EventPlannerId == staff.EventPlannerId && m.Email == staff.Email)
                .ToList();
            if (ModelState.IsValid)
            {
                if (loggedinuser != null)
                {
                    if (listExist.Count > 0)
                    {
                        TempData["display"] = "A staff with the same email exist, try another email!";
                        TempData["notificationtype"] = NotificationType.Error.ToString();
                        return RedirectToAction("Index");
                    }
                    staff.CreatedBy = loggedinuser.AppUserId;
                    staff.DateCreated = DateTime.Now;
                    staff.DateLastModified = DateTime.Now;
                    staff.LastModifiedBy = loggedinuser.AppUserId;
                    staff.RoleId = role.RoleId;
                    staff.EventPlannerId = loggedinuser.EventPlannerId;
                    staff.Password = new Md5Ecryption().ConvertStringToMd5Hash(Membership.GeneratePassword(6, 0));
                    staff.Status = StaffStatus.Activated.ToString();
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                db.Staff.Add(staff);
                db.SaveChanges();
                TempData["display"] = "You have successfully added a staff!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }

            // ViewBag.RoleId = new SelectList(db.Roles, "RoleId", "Name", staff.RoleId);
            return View(staff);
        }

        // GET: Staffs/Edit/5
        [SessionExpire]
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var staff = db.Staff.Find(id);
            if (staff == null)
                return HttpNotFound();
            //ViewBag.RoleId = new SelectList(db.Roles, "RoleId", "Name", staff.RoleId);
            return View(staff);
        }

        // POST: Staffs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Edit(
            [Bind(
                Include =
                    "StaffId,Firstname,Lastname,Status,Email,Mobile,Password,RoleId,CreatedBy,EventPlannerId,DateCreated"
            )] Staff staff)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            if (ModelState.IsValid)
            {
                staff.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    staff.LastModifiedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                db.Entry(staff).State = EntityState.Modified;
                db.SaveChanges();
                TempData["display"] = "You have successfully modified a staff!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            //ViewBag.RoleId = new SelectList(db.Roles, "RoleId", "Name", staff.RoleId);
            return View(staff);
        }

        // GET: Staffs/Delete/5
        [SessionExpire]
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var staff = db.Staff.Find(id);
            if (staff == null)
                return HttpNotFound();
            return View(staff);
        }

        // POST: Staffs/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult DeleteConfirmed(long id)
        {
            var staff = db.Staff.Find(id);
            db.Staff.Remove(staff);
            db.SaveChanges();
            TempData["display"] = "You have successfully deleted a staff!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
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