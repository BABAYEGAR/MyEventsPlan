using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.AuthenticationManagement;
using MyEventPlan.Data.Service.EmailService;
using MyEventPlan.Data.Service.Enum;
using MyEventPlan.Data.Service.FileUploader;

namespace MyEventPlan.Controllers.VendorManagement
{
    public class VendorsController : Controller
    {
        private readonly VendorDataContext db = new VendorDataContext();
        private readonly EventDataContext dbc = new EventDataContext();
        private readonly VendorPackageSettingDataContext dbd = new VendorPackageSettingDataContext();

        // GET: Vendors
        public ActionResult Index()
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var vendors =
                db.Vendors.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId).Include(v => v.VendorService);
            ViewBag.VendorServiceId = new SelectList(db.VendorService, "VendorServiceId", "ServiceName");
            return View(vendors.ToList());
        }

        // GET: Vendors/Profile
        public ActionResult Profile()
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var vendor =
                db.Vendors.Find(loggedinuser.VendorId);
            if (vendor != null)
            {
                ViewBag.VendorServiceId = new SelectList(db.VendorService, "VendorServiceId", "ServiceName",
                    vendor.VendorServiceId);
                ViewBag.LocationId = new SelectList(dbc.Locations, "LocationId", "Name", vendor.LocationId);
            }
            return View(vendor);
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

        // GET: Vendors/ListOfVendors/SearchParameters
        public ActionResult ListOfVendors(FormCollection collectedValues)
        {
            long? serviceId = null;
            long? locationId = null;
            if (collectedValues["VendorServiceId"] != "")
                serviceId = Convert.ToInt64(collectedValues["VendorServiceId"]);
            if (collectedValues["VendorServiceId"] != "")
                locationId = Convert.ToInt64(collectedValues["LocationId"]);
            ViewBag.VendorServiceId = new SelectList(db.VendorService, "VendorServiceId", "ServiceName", serviceId);
            ViewBag.LocationId = new SelectList(dbc.Locations, "LocationId", "Name", locationId);
            ViewBag.vendors = db.Vendors.Where(n => n.LocationId == locationId && n.VendorServiceId == serviceId &&
                                                    n.EventId == null)
                .ToList();
            return View();
        }

        // GET: Vendors/ListOfVendors/SearchParameters
        public ActionResult EventVendors(FormCollection collectedValues)
        {
            long? serviceId = null;
            long? locationId = null;
            if (collectedValues["VendorServiceId"] != "")
                serviceId = Convert.ToInt64(collectedValues["VendorServiceId"]);
            if (collectedValues["VendorServiceId"] != "")
                locationId = Convert.ToInt64(collectedValues["LocationId"]);
            ViewBag.VendorServiceId = new SelectList(db.VendorService, "VendorServiceId", "ServiceName", serviceId);
            ViewBag.LocationId = new SelectList(dbc.Locations, "LocationId", "Name", locationId);
            if (locationId == 0 && serviceId == 0)
            {
                ViewBag.vendors = db.Vendors.ToList();
                return View();
            }
            ViewBag.vendors = db.Vendors.Where(n => n.LocationId == locationId && n.VendorServiceId == serviceId)
                .ToList();
            return View();
        }

        // GET: Vendors/Create
        public ActionResult Create()
        {
            ViewBag.VendorServiceId = new SelectList(db.VendorService, "VendorServiceId", "ServiceName");
            ViewBag.LocationId = new SelectList(dbc.Locations, "LocationId", "Name");
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
                    "VendorId,Name,About,Address,Email,FacebookPage,TwitterPage,InstagramPage,Website,PricingDetails,YoutubePage,GooglePlusPage,AveragePrice,LocationId,ConfirmPassword,Password,Mobile,VendorServiceId,EventPlannerId"
            )] Vendor vendor, FormCollection collectedValues)
        {
            var allUsers = dbc.AppUsers;


            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var events = Session["event"] as Event.Data.Objects.Entities.Event;
            vendor.DateCreated = DateTime.Now;
            vendor.DateLastModified = DateTime.Now;
            if (loggedinuser != null)
            {
                var userExist = allUsers.Any(n => n.Email == vendor.Email);
                if (userExist)
                {
                    TempData["display"] = "The email entered already exist! Try another one!";
                    TempData["notificationtype"] = NotificationType.Error.ToString();
                }
                else
                {
                    vendor.LastModifiedBy = loggedinuser.AppUserId;
                    vendor.CreatedBy = loggedinuser.AppUserId;
                    vendor.EventPlannerId = loggedinuser.EventPlannerId;
                    if (events != null) vendor.EventId = events.EventId;
                    vendor.Password = new Hashing().HashPassword("password");
                    vendor.ConfirmPassword = vendor.Password;
                }
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
            TempData["display"] = "You have successfully added a personal vendor to your event!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("ListOfVendors");
        }

        public ActionResult ChangePassword(
            [Bind(
                Include =
                    "VendorId,Name,About,Address,Email,FacebookPage,TwitterPage,InstagramPage,Website,PricingDetails,YoutubePage,GooglePlusPage,AveragePrice,LocationId,ConfirmPassword,Password,Mobile,VendorServiceId,EventPlannerId"
            )] Vendor vendor, FormCollection collectedValues)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            AppUser user = null;
            if (loggedinuser != null)
            {
                user = dbc.AppUsers.Find(loggedinuser.AppUserId);
                if (user != null)
                {
                    user.Password = new Hashing().HashPassword(vendor.ConfirmPassword);
                    vendor.Password = null;
                    vendor.ConfirmPassword = null;

                    dbc.Entry(user).State = EntityState.Modified;
                    dbc.Entry(vendor).State = EntityState.Modified;
                }
            }
            dbc.SaveChanges();
            loggedinuser = user;
            return View("Profile");
        }

        // GET: Vendors/Create
        public ActionResult Register(long? id)
        {
            ViewBag.VendorServiceId = new SelectList(db.VendorService, "VendorServiceId", "ServiceName");
            ViewBag.LocationId = new SelectList(dbc.Locations, "LocationId", "Name");
            ViewBag.packageId = id;
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
                    "VendorId,Name,About,Address,Email,FacebookPage,TwitterPage,InstagramPage,Website,PricingDetails,YoutubePage,GooglePlusPage,AveragePrice,LocationId,ConfirmPassword,Password,Mobile,VendorServiceId,EventPlannerId"
            )] Vendor vendor, FormCollection collectedValues)
        {
            var logo = Request.Files["logo"];
            var role = dbc.Roles.FirstOrDefault(m => m.Name == "Vendor");
            var packageId = Convert.ToInt64(collectedValues["packageId"]);
            var pacakge = dbd.VendorPackages.Find(packageId);
            if (ModelState.IsValid)
            {
                vendor.DateCreated = DateTime.Now;
                vendor.DateLastModified = DateTime.Now;
                if (dbc.AppUsers.Any(n => n.Email == vendor.Email))
                {
                    TempData["outterform"] = "The email entered already exist! Try another one!";
                    TempData["notificationtype"] = NotificationType.Error.ToString();
                    return View(vendor);
                }
                vendor.LastModifiedBy = 1;
                vendor.CreatedBy = 1;
                vendor.EventPlannerId = null;
                vendor.EventId = null;
                if (logo != null && logo.FileName != "")
                    vendor.Logo = new FileUploader().UploadFile(logo, UploadType.vendorLogo);

                Session["vendor"] = vendor;


                //vendor user account details
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
                if (role != null) appUser.RoleId = role.RoleId;
                appUser.Password = new Hashing().HashPassword(vendor.Password);


                Session["vendoruser"] = appUser;
                var packageSetting = new VendorPackageSetting
                {
                    AppUserId = appUser.AppUserId,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(1),
                    Status = PackageStatusEnum.Active.ToString(),
                    VendorId = vendor.VendorId,
                    VendorPackageId = packageId,
                    DateCreated = DateTime.Now,
                    DateLastModified = DateTime.Now,
                    CreatedBy = 1,
                    LastModifiedBy = 1,
                    Amount = (long) pacakge.Amount
                };
                Session["vendorpackage"] = packageSetting;
                ViewBag.VendorServiceId = new SelectList(db.VendorService, "VendorServiceId", "ServiceName",
                    vendor.VendorServiceId);
                ViewBag.LocationId = new SelectList(dbc.Locations, "LocationId", "Name", vendor.LocationId);
                return View(vendor);
            }
            ViewBag.VendorServiceId = new SelectList(db.VendorService, "VendorServiceId", "ServiceName",
                vendor.VendorServiceId);
            ViewBag.LocationId = new SelectList(dbc.Locations, "LocationId", "Name", vendor.LocationId);
            return View(vendor);
        }

        [HttpGet]
        public ActionResult ConfirmPayment()
        {
            var vendor = Session["vendor"] as Vendor;
            if (vendor != null) db.Vendors.Add(vendor);
            db.SaveChanges();

            var appUser = Session["vendoruser"] as AppUser;
            if (appUser != null) dbc.AppUsers.Add(appUser);
            dbc.SaveChanges();

            var packageSetting = Session["vendorpackage"] as VendorPackageSetting;
            if (packageSetting != null) dbd.VendorPackageSetting.Add(packageSetting);
            dbd.SaveChanges();

            new MailerDaemon().NewVendor(vendor, appUser.AppUserId);
            TempData["login"] =
                "You have successfully registered as a vendor! Verify access in your email to login";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Login", "Account");
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
                    "VendorId,Name,About,Address,Email,FacebookPage,TwitterPage,InstagramPage,Website,PricingDetails,YoutubePage,GooglePlusPage" +
                    ",AveragePrice,LocationId,ConfirmPassword,Password,Mobile,VendorServiceId,EventPlannerId,CreatedBy,DateCreated"
            )] Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                var logo = Request.Files["logo"];
                vendor.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    vendor.LastModifiedBy = loggedinuser.AppUserId;
                    if (logo != null && logo.FileName != "")
                        vendor.Logo = new FileUploader().UploadFile(logo, UploadType.vendorLogo);
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
                return RedirectToAction("Profile");
            }
            ViewBag.VendorServiceId = new SelectList(db.VendorService, "VendorServiceId", "ServiceName",
                vendor.VendorServiceId);
            ViewBag.LocationId = new SelectList(dbc.Locations, "LocationId", "Name", vendor.LocationId);
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