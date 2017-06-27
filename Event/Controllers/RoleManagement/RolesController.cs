using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.AuthenticationManagement;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.RoleManagement
{
    public class RolesController : Controller
    {
        private readonly EventDataContext _databaseConnectiondb = new EventDataContext();

        // GET: Roles
        [SessionExpire]
        public ActionResult Index()
        {
            return View(_databaseConnectiondb.Roles.ToList());
        }

        // GET: Roles/Details/5
        [SessionExpire]
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var role = _databaseConnectiondb.Roles.Find(id);
            if (role == null)
                return HttpNotFound();
            return View(role);
        }

        // GET: Roles/Create
        [SessionExpire]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Create(
            [Bind(
                Include =
                    "RoleId,Name,ManageApplicationUser,ManageRoles,ManageEvents,ManageEventType,ManageProspects,ManageInvoices,ManageGuestList,ManageBudgets,ManagePackages,ManageLocations,"+
                "ManageVendors,ManageEventVendors,ManageVendorServices,ManageCalendar,ManageStaff,ManageResources,ManageCheckList,ManageContacts,ManageNotes,ManageTasks"
            )] Role role)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            if (ModelState.IsValid)
            {
                role.DateCreated = DateTime.Now;
                role.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    role.CreatedBy = loggedinuser.AppUserId;
                    role.LastModifiedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                _databaseConnectiondb.Roles.Add(role);
                _databaseConnectiondb.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(role);
        }

        // GET: Roles/Edit/5
        [SessionExpire]
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var role = _databaseConnectiondb.Roles.Find(id);
            if (role == null)
                return HttpNotFound();
            return View(role);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Edit(
            [Bind(
                Include =
                    "RoleId,Name,ManageApplicationUser,ManageRoles,ManageEvents,ManageEventType,ManageProspects,ManageInvoices,ManageGuestList,ManageBudgets,ManagePackages,ManageLocations,"+
                    "ManageVendors,ManageEventVendors,ManageVendorServices,ManageCalendar,ManageStaff,ManageResources,ManageCheckList,ManageContacts,ManageNotes,ManageTasks,DateCreated,CreatedBy"
            )] Role role)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            if (ModelState.IsValid)
            {
                role.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    role.LastModifiedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                _databaseConnectiondb.Entry(role).State = EntityState.Modified;
                _databaseConnectiondb.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(role);
        }

        // GET: Roles/Delete/5
        [SessionExpire]
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var role = _databaseConnectiondb.Roles.Find(id);
            if (role == null)
                return HttpNotFound();
            return View(role);
        }

        // POST: Roles/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult DeleteConfirmed(long id)
        {
            var role = _databaseConnectiondb.Roles.Find(id);
            _databaseConnectiondb.Roles.Remove(role);
            _databaseConnectiondb.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _databaseConnectiondb.Dispose();
            base.Dispose(disposing);
        }
    }
}