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
        private readonly AppUserDataContext _dbd = new AppUserDataContext();
        private readonly SubscriptionInvoiceDataContext _dbe = new SubscriptionInvoiceDataContext();
        private readonly VendorDataContext db = new VendorDataContext();
        private readonly EventDataContext dbc = new EventDataContext();
        private readonly VendorPackageSettingDataContext dbd = new VendorPackageSettingDataContext();
        private readonly VendorImageDataContext dbf = new VendorImageDataContext();

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

        // GET: Vendors/ListOfVendors/SearchParameters
        public ActionResult ListOfVendors(long? categoryId,FormCollection collectedValues)
        {
            long? serviceId = null;
            long? locationId = null;
            if (collectedValues["VendorServiceId"] != "")
                serviceId = Convert.ToInt64(collectedValues["VendorServiceId"]);
            if (collectedValues["LocationId"] != "")
                locationId = Convert.ToInt64(collectedValues["LocationId"]);
            long minimumPrice = 0;
            long maximumPrice = 0;
            int ratingOne = 0;
            int ratingTwo = 0;
            int ratingThree = 0;
            int ratingFour = 0;
            int ratingFive = 0;
            if (collectedValues["Price"] != null)
            {
                var price = collectedValues["Price"];
                var index = price.IndexOf("-", StringComparison.Ordinal);
                var min = (index > 0 ? price.Substring(0, index) : "").Replace("N", "");
                var max = price.Substring(price.LastIndexOf('-') + 1).Replace("N", "");

                minimumPrice = Convert.ToInt64(min);
                maximumPrice = Convert.ToInt64(max);
            }
            if (collectedValues["checkbox"] != null)
            {
                var start = collectedValues["checkbox"];
                if (start.Contains("1"))
                {
                    ratingOne = 1;
                }
                if (start.Contains("2"))
                {
                    ratingTwo = 2;
                }
                if (start.Contains("3"))
                {
                    ratingThree = 3;
                }
                if (start.Contains("4"))
                {
                    ratingFour = 4;
                }
                if (start.Contains("5"))
                {
                    ratingFive = 5;
                }

            }
            if (categoryId != null && collectedValues["VendorServiceId"] == "" && collectedValues["LocationId"] == "" &&
                collectedValues["Price"] == null && collectedValues["checkbox"] == null)
            {
                ViewBag.vendors = db.Vendors.Where(n => n.VendorServiceId == categoryId &&
                                                        n.EventId == null).ToList();
            }
            if (collectedValues["VendorServiceId"] != "" && collectedValues["LocationId"] == "" &&
                collectedValues["Price"] == null && collectedValues["checkbox"] == null)
            {
                ViewBag.vendors = db.Vendors.Where(n => n.VendorServiceId == serviceId &&
                                                        n.EventId == null).ToList();
            }
            if (collectedValues["VendorServiceId"] == "" && collectedValues["LocationId"] != "" &&
                collectedValues["Price"] == null && collectedValues["checkbox"] == null)
            {
                ViewBag.vendors = db.Vendors.Where(n => n.LocationId == locationId &&
                                                        n.EventId == null).ToList();
            }
            if (collectedValues["VendorServiceId"] != "" && collectedValues["LocationId"] != "" &&
                collectedValues["Price"] == null && collectedValues["checkbox"] == null)
            {
                ViewBag.vendors = db.Vendors.Where(n => n.LocationId == locationId && n.VendorServiceId == serviceId &&
                                                        n.EventId == null).ToList();
            }
            if (collectedValues["VendorServiceId"] != "" && collectedValues["LocationId"] != "" &&
                collectedValues["Price"] != null && collectedValues["checkbox"] == null)
            {
                ViewBag.vendors = db.Vendors.Where(n => n.LocationId == locationId && n.VendorServiceId == serviceId &&
                                                        n.EventId == null && n.MinimumPrice >= minimumPrice &&
                                                        n.MaximumPrice >= maximumPrice).ToList();
            }
            if (collectedValues["VendorServiceId"] != "" && collectedValues["LocationId"] != "" &&
                collectedValues["Price"] != null && collectedValues["checkbox"] != null)
            {

                ViewBag.vendors = db.Vendors.Where(n => n.LocationId == locationId && n.VendorServiceId == serviceId &&
                                                        n.EventId == null && n.MinimumPrice >= minimumPrice &&
                                                        n.MaximumPrice >= maximumPrice && (n.AverageRating == ratingOne || n.AverageRating == ratingTwo || n.AverageRating == ratingThree
                                                        || n.AverageRating == ratingFour || n.AverageRating == ratingFive)).ToList();
            }
            if (collectedValues["VendorServiceId"] == "" && collectedValues["LocationId"] == "" &&
                collectedValues["Price"] == null && collectedValues["checkbox"] == null)
            {
                ViewBag.vendors = db.Vendors.Where(n=> n.EventId == null).ToList();
                ViewBag.VendorServiceId = new SelectList(db.VendorService, "VendorServiceId", "ServiceName");
                ViewBag.LocationId = new SelectList(dbc.Locations, "LocationId", "Name");
            }
            if (serviceId != null)
            {
                ViewBag.VendorServiceId = new SelectList(db.VendorService, "VendorServiceId", "ServiceName", serviceId);
            }
            if (categoryId != null)
            {
                ViewBag.VendorServiceId = new SelectList(db.VendorService, "VendorServiceId", "ServiceName", categoryId);
            }
            ViewBag.LocationId = new SelectList(dbc.Locations, "LocationId", "Name", locationId);
            ViewBag.rate1 = ratingOne;
            ViewBag.rate2 = ratingTwo;
            ViewBag.rate3 = ratingThree;
            ViewBag.rate4 = ratingFour;
            ViewBag.rate5 = ratingFive;
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
                    "VendorId,Name,About,Address,Email,FacebookPage,TwitterPage,InstagramPage,Website,PricingDetails" +
                    ",YoutubePage,GooglePlusPage,MaximumPrice,MinimumPrice,LocationId,ConfirmPassword,Password,Mobile,VendorServiceId,EventPlannerId"
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
                    "VendorId,Name,About,Address,Email,FacebookPage,TwitterPage" +
                    ",InstagramPage,Website,PricingDetails,YoutubePage,GooglePlusPage,MaximumPrice,MinimumPrice" +
                    ",Logo,LocationId,ConfirmPassword,Password,Mobile,VendorServiceId,EventPlannerId"
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
                    "VendorId,Name,About,Address,Email,FacebookPage,TwitterPage,InstagramPage,Website," +
                    "PricingDetails,YoutubePage,GooglePlusPage,MaximumPrice,MinimumPrice,LocationId,ConfirmPassword," +
                    "Password,Mobile,VendorServiceId,EventPlannerId"
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
                appUser.Verified = true;
                if (role != null) appUser.RoleId = role.RoleId;
                appUser.Password = new Hashing().HashPassword(vendor.Password);


                Session["vendoruser"] = appUser;
                var packageSetting = new VendorPackageSetting
                {
                    AppUserId = appUser.AppUserId,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(1),
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
                var vendorImage = new VendorImage
                {
                    ImageOne = null,
                    ImageTwo = null,
                    ImageThree = null,
                    ImageFour = null,
                    ImageFive = null,
                    ImageSix = null,
                    ImageSeven = null,
                    ImageEight = null,
                    ImageNine = null,
                    ImageTen = null,
                    DateLastModified = DateTime.Now,
                    DateCreated = DateTime.Now,
                    CreatedBy = 1,
                    LastModifiedBy = 1
                };
                Session["vendorimage"] = vendorImage;
               
                ViewBag.VendorServiceId = new SelectList(db.VendorService, "VendorServiceId", "ServiceName",
                    vendor.VendorServiceId);
                ViewBag.LocationId = new SelectList(dbc.Locations, "LocationId", "Name", vendor.LocationId);
                return RedirectToAction("Invoice", "VendorPackages", new {id = packageSetting.VendorPackageId});
            }
            ViewBag.VendorServiceId = new SelectList(db.VendorService, "VendorServiceId", "ServiceName",
                vendor.VendorServiceId);
            ViewBag.LocationId = new SelectList(dbc.Locations, "LocationId", "Name", vendor.LocationId);
            return View(vendor);
        }

        [HttpGet]
        [AllowAnonymous]
        // GET: EventPlanners/Pricing
        public ActionResult Pricing()
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var packages = db.VendorPackageSetting.Include(n => n.VendorPackage);

            var packageSubscribed =
                packages.SingleOrDefault(
                    n =>
                        n.VendorId == loggedinuser.VendorId &&
                        n.Status == PackageStatusEnum.Active.ToString());
            if (packageSubscribed != null)
                Session["subscribe"] = packageSubscribed;
            return View(dbc.VendorPackages.ToList());
        }

        [HttpGet]
        [AllowAnonymous]
        // GET: EventPlanners/Invoice
        public ActionResult Invoice(long id)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var selectedPackage = dbd.VendorPackages.Find(id);
            var subscriptionInvoice = new SubscriptionInvoice();

            //random number
            var generator = new Random();
            var randomNumber = generator.Next(0, 1000000).ToString("D6");

            if (loggedinuser != null)
            {
                subscriptionInvoice.AppUserId = loggedinuser.AppUserId;
                if (loggedinuser.VendorId != null)
                    subscriptionInvoice.VendorId = (long) loggedinuser.VendorId;
                subscriptionInvoice.DateCreated = DateTime.Now;
                subscriptionInvoice.DateLastModified = DateTime.Now;
                subscriptionInvoice.CreatedBy = loggedinuser.AppUserId;
                subscriptionInvoice.LastModifiedBy = loggedinuser.AppUserId;
            }
            subscriptionInvoice.InvoiceNumber = "#" + randomNumber;
            if (selectedPackage != null)
            {
                subscriptionInvoice.PackageId = selectedPackage.VendorPackageId;

                Session["package"] = selectedPackage;
            }
            Session["invoice"] = subscriptionInvoice;
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ConfirmPostAccountPayment()
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var package = Session["package"] as Event.Data.Objects.Entities.VendorPackage;
            var invoice = Session["invoice"] as SubscriptionInvoice;
            var packageToSubscribed = new VendorPackageSetting();
            var packageSetting = dbd.VendorPackageSetting.Include(n => n.VendorPackage);
            //current subscription
            var packageSubscribed =
                packageSetting.SingleOrDefault(
                    n =>
                        n.VendorId == loggedinuser.VendorId &&
                        n.Status == PackageStatusEnum.Active.ToString());

            if (packageSubscribed != null)
            {
                //make the current package inactive
                packageSubscribed.Status = PackageStatusEnum.Inactive.ToString();
                packageSubscribed.DateLastModified = DateTime.Now;
                dbd.Entry(packageSubscribed).State = EntityState.Modified;
                dbd.SaveChanges();

                //populate new package
                if (loggedinuser != null && loggedinuser.VendorId != null)
                    packageToSubscribed.VendorId = (long) loggedinuser.VendorId;
                if (loggedinuser != null)
                {
                    packageToSubscribed.CreatedBy = loggedinuser.AppUserId;
                    packageToSubscribed.LastModifiedBy = loggedinuser.AppUserId;
                    packageToSubscribed.AppUserId = loggedinuser.AppUserId;
                }

                packageToSubscribed.DateCreated = DateTime.Now;
                packageToSubscribed.DateLastModified = DateTime.Now;
                packageToSubscribed.Status = PackageStatusEnum.Active.ToString();
                packageToSubscribed.StartDate = packageSubscribed.DateCreated;
                packageToSubscribed.EndDate = packageSubscribed.DateCreated.AddMonths(1);

                //package data
                if (package != null)
                {
                    packageToSubscribed.VendorPackageId = package.VendorPackageId;
                    if (package.Amount != null) packageToSubscribed.Amount = (long) package.Amount;
                }
                //commit package to database
                dbd.VendorPackageSetting.Add(packageToSubscribed);
                dbd.SaveChanges();

                //commit invoice to database
                if (invoice != null) _dbe.SubscriptionInvoices.Add(invoice);
                _dbe.SaveChanges();
                Session["package"] = null;
                Session["invoice"] = null;
                //display notification
                TempData["display"] = "You have successfully subscribed to the package!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Setting", "Account");
            }
            if (loggedinuser != null)
            {
                if (loggedinuser.VendorId != null)
                    packageToSubscribed.VendorId = (long) loggedinuser.VendorId;
                packageToSubscribed.CreatedBy = loggedinuser.AppUserId;
                packageToSubscribed.LastModifiedBy = loggedinuser.AppUserId;
                packageToSubscribed.AppUserId = loggedinuser.AppUserId;
            }
            packageToSubscribed.DateCreated = DateTime.Now;
            packageToSubscribed.DateLastModified = DateTime.Now;
            packageToSubscribed.Status = PackageStatusEnum.Active.ToString();
            packageToSubscribed.StartDate = DateTime.Now;
            packageToSubscribed.EndDate = DateTime.Now.AddMonths(1);


            //package data
            if (package != null)
            {
                packageToSubscribed.VendorPackageId = package.VendorPackageId;
                if (package.Amount != null) packageToSubscribed.Amount = (long) package.Amount;
            }
            dbd.VendorPackageSetting.Add(packageToSubscribed);
            dbd.SaveChanges();
            if (invoice != null) _dbe.SubscriptionInvoices.Add(invoice);
            _dbe.SaveChanges();

            Session["package"] = null;
            Session["invoice"] = null;

            //display notification
            TempData["display"] = "You have successfully topped up your account with this package!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Profile", "Vendors");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ConfirmPayment()
        {
            var vendor = Session["vendor"] as Vendor;
            if (vendor != null) db.Vendors.Add(vendor);
            db.SaveChanges();

            var appUser = Session["vendoruser"] as AppUser;
            if (vendor != null)
                if (appUser != null)
                {
                    appUser.VendorId = vendor.VendorId;
                    _dbd.AppUsers.Add(appUser);
                    _dbd.SaveChanges();

                    var packageSetting = Session["vendorpackage"] as VendorPackageSetting;
                    if (packageSetting != null)
                    {
                        packageSetting.AppUserId = appUser.AppUserId;
                        packageSetting.VendorId = vendor.VendorId;
                        dbd.VendorPackageSetting.Add(packageSetting);
                    }
                    var vendorImage = Session["vendorimage"] as VendorImage;
                    if (vendorImage != null)
                    {
                        vendorImage.VendorId = vendor.VendorId;
                        dbf.VendorImages.Add(vendorImage);
                        dbf.SaveChanges();
                    }
                    dbd.SaveChanges();
                    new MailerDaemon().NewVendor(vendor, appUser.AppUserId);
                    TempData["login"] =
                        "You have successfully registered as a vendor! Verify access in your email to login";
                    TempData["notificationtype"] = NotificationType.Success.ToString();
                    return RedirectToAction("Login", "Account");
                }
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
                    ",MaximumPrice,MinimumPrice,LocationId,ConfirmPassword,Logo,Password,Mobile,VendorServiceId,EventPlannerId,CreatedBy,DateCreated"
            )] Vendor vendor, FormCollection collectedValues)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                var logo = Request.Files["logo"];
                var vendorUser = _dbd.AppUsers.SingleOrDefault(n => n.VendorId == vendor.VendorId);
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
                if (vendorUser != null)
                {
                    vendorUser.DateLastModified = DateTime.Now;
                    vendorUser.Email = vendor.Email;
                    vendorUser.Firstname = vendor.Name;
                    vendorUser.Lastname = vendor.Name;
                    vendorUser.Mobile = vendor.Mobile;
                    vendorUser.ProfileImage = vendor.Logo;
                }
                _dbd.Entry(vendorUser).State = EntityState.Modified;
                _dbd.SaveChanges();
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
            var user = dbc.AppUsers.SingleOrDefault(n => n.VendorId == vendor.VendorId);
            var eventMapping = dbc.EventVendorMappings.Where(n => n.VendorId == vendor.VendorId);
            var reviews = dbc.VendorReviews.Where(n => n.VendorId == vendor.VendorId);
            var subscription = dbc.SubscriptionInvoices.Where(n => n.VendorId == vendor.VendorId);
            var images = dbc.VendorImages.Where(n => n.VendorId == vendor.VendorId);
            var enquiries = dbc.VendorEnquiries.Where(n => n.VendorId == vendor.VendorId);
            var settings = dbc.VendorPackageSettings.Where(n => n.VendorId == vendor.VendorId);
            if (user != null) dbc.AppUsers.Remove(user);
            foreach (var item in eventMapping)
            {
                dbc.EventVendorMappings.Remove(item);
            }
            foreach (var item in reviews)
            {
                dbc.VendorReviews.Remove(item);
            }
            foreach (var item in subscription)
            {
                dbc.SubscriptionInvoices.Remove(item);
            }
            foreach (var item in images)
            {
                dbc.VendorImages.Remove(item);
            }
            foreach (var item in enquiries)
            {
                dbc.VendorEnquiries.Remove(item);
            }
            foreach (var item in settings)
            {
                dbc.VendorPackageSettings.Remove(item);
            }
            dbc.SaveChanges();
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