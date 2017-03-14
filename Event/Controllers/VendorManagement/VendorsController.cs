using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.AuthenticationManagement;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.VendorManagement
{
    public class VendorsController : Controller
    {
        private readonly VendorDataContext db = new VendorDataContext();
        private readonly EventDataContext dbc = new EventDataContext();

        // GET: Vendors
        public ActionResult Index()
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var vendors =
                db.Vendors.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId).Include(v => v.VendorService);
            ViewBag.VendorServiceId = new SelectList(db.VendorService, "VendorServiceId", "ServiceName");
            return View(vendors.ToList());
        }

        // GET: Vendors/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var vendor = db.Vendors.Find(id);
            if (vendor == null)
                return HttpNotFound();
            return View(vendor);
        }

        // GET: Vendors/Details/5
        public ActionResult VendorDetails(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var vendor = db.Vendors.Find(id);
            if (vendor == null)
                return HttpNotFound();
            return View(vendor);
        }

        // GET: Vendors/ListOfVendors
        public ActionResult ListOfVendors()
        {
            ViewBag.VendorServiceId = new SelectList(db.VendorService, "VendorServiceId", "ServiceName");
            return View(db.Vendors.Include(n => n.Location));
        }

        // GET: Vendors/ListOfVendorsByLocation
        public ActionResult ListOfVendorsByLocation(long? locationId)
        {
            ViewBag.VendorServiceId = new SelectList(db.VendorService, "VendorServiceId", "ServiceName");
            return View("ListOfVendors",
                db.Vendors.Where(n => n.LocationId == locationId).Include(n => n.Location).Include(n => n.VendorService));
        }

        // GET: Vendors/ListOfVendorsByCategory
        public ActionResult ListOfVendorsByCategory(long? categoryId)
        {
            ViewBag.VendorServiceId = new SelectList(db.VendorService, "VendorServiceId", "ServiceName");
            return View("ListOfVendors",
                db.Vendors.Where(n => n.VendorServiceId == categoryId)
                    .Include(n => n.Location)
                    .Include(n => n.VendorService));
        }

        // GET: Vendors/Create
        public ActionResult Create()
        {
            ViewBag.VendorServiceId = new SelectList(db.VendorService, "VendorServiceId", "ServiceName");
            ViewBag.VendorServiceId = new SelectList(dbc.Locations, "LocationId", "Name");
            return View();
        }

        // POST: Vendors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(
                 Include =
                     "VendorId,Name,Address,Email,Mobile,LocationId,About,VendorServiceId,BusinessName,BusinessContact"
             )] Vendor vendor)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var events = Session["event"] as Event.Data.Objects.Entities.Event;
            vendor.DateCreated = DateTime.Now;
            vendor.DateLastModified = DateTime.Now;
            vendor.ImageOne = "131329580750710796.jpg";
            vendor.ImageTwo = "131329580750710796.jpg";
            vendor.ImageThree = "131329580750710796.jpg";
            if (loggedinuser != null)
            {
                vendor.LastModifiedBy = loggedinuser.AppUserId;
                vendor.CreatedBy = loggedinuser.AppUserId;
                vendor.EventPlannerId = loggedinuser.EventPlannerId;
                if (events != null) vendor.EventId = events.EventId;
                vendor.Password = new Hashing().HashPassword("password");
                vendor.ConfirmPassword = new Hashing().HashPassword("password");
                
            }
            else
            {
                TempData["login"] = "Your session has expired, Login again!";
                TempData["notificationtype"] = NotificationType.Info.ToString();
                return RedirectToAction("Login", "Account");
            }

            db.Vendors.Add(vendor);
            db.SaveChanges();

            var mapping = new EventVendorMapping();
            if (events != null) mapping.EventId = events.EventId;
            mapping.EventPlannerId = loggedinuser.EventPlannerId;
            mapping.VendorId = vendor.VendorId;
            mapping.DateCreated = DateTime.Now;
            mapping.DateLastModified = DateTime.Now;
            mapping.LastModifiedBy = loggedinuser.AppUserId;
            mapping.CreatedBy = loggedinuser.AppUserId;
            
            dbc.EventVendorMappings.Add(mapping);
            dbc.SaveChanges();
            TempData["vendor"] = "You have successfully added a vendor to your event!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("ListOfVendors");
        }


        // GET: Vendors/Create
        public ActionResult Register()
        {
            ViewBag.VendorServiceId = new SelectList(db.VendorService, "VendorServiceId", "ServiceName");
            ViewBag.LocationId = new SelectList(dbc.Locations, "LocationId", "Name");
            return View();
        }

        // POST: Vendors/Register
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(
            [Bind(
                 Include =
                     "VendorId,Name,Address,Email,LocationId,ConfirmPassword,Password,Mobile,VendorServiceId,EventPlannerId,BusinessName,BusinessContact"
             )] Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                vendor.DateCreated = DateTime.Now;
                vendor.DateLastModified = DateTime.Now;

                if (db.Vendors.Any(n => n.Email == vendor.Email))
                {
                    TempData["outterform"] = "A vendor exist with the same email, Try again!";
                    TempData["notificationtype"] = NotificationType.Error.ToString();
                    return View(vendor);
                }
                vendor.LastModifiedBy = 1;
                vendor.CreatedBy = 1;
                vendor.EventPlannerId = null;
                vendor.EventId = null;

                db.Vendors.Add(vendor);
                db.SaveChanges();
                var appUser = new AppUser();
                appUser.EventPlannerId = null;
                appUser.Email = vendor.Email;
                appUser.Firstname = vendor.Name;
                appUser.Lastname = vendor.Name;
                appUser.CreatedBy = 1;
                appUser.LastModifiedBy = 1;
                appUser.DateCreated = DateTime.Now;
                appUser.DateLastModified = DateTime.Now;
                appUser.VendorId = vendor.VendorId;
                appUser.Mobile = vendor.Mobile;
                appUser.Verified = false;
                appUser.Password = new Hashing().HashPassword(vendor.Password);
                dbc.AppUsers.Add(appUser);
                dbc.SaveChanges();
                TempData["login"] = "You have successfully registered as a vendor! Verify access in your email to login";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Login", "Account");
            }
            ViewBag.VendorServiceId = new SelectList(db.VendorService, "VendorServiceId", "ServiceName");
            ViewBag.LocationId = new SelectList(dbc.Locations, "LocationId", "Name");
            return View(vendor);
        }

        // GET: Vendors/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var vendor = db.Vendors.Find(id);
            if (vendor == null)
                return HttpNotFound();
            ViewBag.VendorServiceId = new SelectList(db.VendorService, "VendorServiceId", "ServiceName",
                vendor.VendorServiceId);
            return View(vendor);
        }

        // POST: Vendors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(
                 Include =
                     "VendorId,Name,Address,Email,Mobile,LocationId,ConfirmPassword,Password,VendorServiceId,EventPlannerId,BusinessName,BusinessContact"
             )] Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                vendor.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    vendor.LastModifiedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                db.Entry(vendor).State = EntityState.Modified;
                db.SaveChanges();
                TempData["vendor"] = "You have successfully modified a vendor!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                ViewBag.VendorServiceId = new SelectList(db.VendorService, "VendorServiceId", "ServiceName");
                return RedirectToAction("Index");
            }
            ViewBag.VendorServiceId = new SelectList(db.VendorService, "VendorServiceId", "ServiceName",
                vendor.VendorServiceId);
            return View(vendor);
        }

        // GET: Vendors/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var vendor = db.Vendors.Find(id);
            if (vendor == null)
                return HttpNotFound();
            return View(vendor);
        }

        // POST: Vendors/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            var vendor = db.Vendors.Find(id);
            db.Vendors.Remove(vendor);
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