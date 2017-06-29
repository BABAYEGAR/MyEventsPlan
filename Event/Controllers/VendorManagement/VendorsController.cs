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
        private readonly EventDataContext _databaseConnection = new EventDataContext();

        // GET: Vendors
        [SessionExpire]
        public ActionResult Index()
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var vendors =
                _databaseConnection.Vendors.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId).Include(v => v.VendorService);
            ViewBag.VendorServiceId = new SelectList(_databaseConnection.VendorServices, "VendorServiceId", "ServiceName");
            return View(vendors.ToList());
        }

        // GET: Vendors/Profile
        public ActionResult Profile()
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                var vendor =
                    _databaseConnection.Vendors.Find(loggedinuser.VendorId);
                    ViewBag.VendorServiceId = new SelectList(_databaseConnection.VendorServices, "VendorServiceId", "ServiceName",
                        vendor.VendorServiceId);
                    ViewBag.LocationId = new SelectList(_databaseConnection.Locations, "LocationId", "Name", vendor.LocationId);
                
                return View(vendor);
        }

        // GET: Vendors/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var vendor = _databaseConnection.Vendors.Find(id);
            if (vendor == null)
                return HttpNotFound();
            return View(vendor);
        }

        // GET: Vendors/ListOfVendors/SearchParameters
        public ActionResult ListOfVendors(long? categoryId, FormCollection collectedValues)
        {
            //initialize values
            long? serviceId = null;
            long? locationId = null;
            if (collectedValues["VendorServiceId"] != "")
            {
                serviceId = Convert.ToInt64(collectedValues["VendorServiceId"]);
            }
            if (collectedValues["VendorServiceId"] == "")
            {
                collectedValues["VendorServiceId"] = null;
            }
            if (collectedValues["LocationId"] != "")
            {
                locationId = Convert.ToInt64(collectedValues["LocationId"]);
            }
            if (collectedValues["LocationId"] == "")
            {
                collectedValues["LocationId"] = null;
            }
            var ratingOne = 0;
            var ratingTwo = 0;
            var ratingThree = 0;
            var ratingFour = 0;
            var ratingFive = 0;

            //initialize prices
            long? minimumPrice = null;
            long? maximumPrice = null;

            //collect price string values
                var min = collectedValues["MinimumPrice"];
                var max = collectedValues["MaximumPrice"];

            //check and convert price to long
            if (collectedValues["MinimumPrice"] != "")
            {
                minimumPrice = Convert.ToInt64(min);
            }
            if (collectedValues["MinimumPrice"] == "")
            {
                
                collectedValues["MinimumPrice"] = null;
            }
            if (collectedValues["MaximumPrice"] != "")
            {
                maximumPrice = Convert.ToInt64(max);
            }
            if (collectedValues["MaximumPrice"] == "")
            {
                collectedValues["MaximumPrice"] = null;
            }
            ViewBag.MinimumPrice = minimumPrice;
            ViewBag.maximumPrice = maximumPrice;

            //check and compute star rating
            if (collectedValues["checkbox"] != null)
            {
                var start = collectedValues["checkbox"];
                if (start.Contains("1"))
                    ratingOne = 1;
                if (start.Contains("2"))
                    ratingTwo = 2;
                if (start.Contains("3"))
                    ratingThree = 3;
                if (start.Contains("4"))
                    ratingFour = 4;
                if (start.Contains("5"))
                    ratingFive = 5;
            }
            if (collectedValues["checkbox"] == "")
            {
                collectedValues["checkbox"] = null;
            }
            if (categoryId != null && collectedValues["VendorServiceId"] == null && collectedValues["LocationId"] == null&& collectedValues["checkbox"] == null)
                ViewBag.vendors = _databaseConnection.Vendors.Where(n => n.VendorServiceId == categoryId &&
                                                        n.EventId == null).ToList();
            if (collectedValues["VendorServiceId"] != null && collectedValues["LocationId"] == null &&
              collectedValues["checkbox"] == null)
                ViewBag.vendors = _databaseConnection.Vendors.Where(n => n.VendorServiceId == serviceId &&
                                                        n.EventId == null).ToList();
            if (collectedValues["VendorServiceId"] == null && collectedValues["LocationId"] != null &&
                collectedValues["checkbox"] == null)
                ViewBag.vendors = _databaseConnection.Vendors.Where(n => n.LocationId == locationId &&
                                                        n.EventId == null).ToList();
            if (collectedValues["VendorServiceId"] != null && collectedValues["LocationId"] != null &&
                 collectedValues["checkbox"] == null && collectedValues["MinimumPrice"] == null && collectedValues["MaximumPrice"] == null)
                ViewBag.vendors = _databaseConnection.Vendors.Where(n => n.LocationId == locationId && n.VendorServiceId == serviceId &&
                                                        n.EventId == null).ToList();
            if (collectedValues["VendorServiceId"] != null && collectedValues["LocationId"] != null &&
                collectedValues["checkbox"] == null && collectedValues["MinimumPrice"] != null && collectedValues["MaximumPrice"] == null)
                ViewBag.vendors = _databaseConnection.Vendors.Where(n => n.LocationId == locationId && n.VendorServiceId == serviceId &&
                                                        n.EventId == null && n.MinimumPrice >= minimumPrice).ToList();
            if (collectedValues["VendorServiceId"] != null && collectedValues["LocationId"] != null &&
                collectedValues["checkbox"] == null && collectedValues["MinimumPrice"] == null && collectedValues["MaximumPrice"] != null)
                ViewBag.vendors = _databaseConnection.Vendors.Where(n => n.LocationId == locationId && n.VendorServiceId == serviceId &&
                                                        n.EventId == null && n.MaximumPrice <= maximumPrice).ToList();
            if (collectedValues["VendorServiceId"] != null && collectedValues["LocationId"] != null &&
                collectedValues["checkbox"] != null && collectedValues["MinimumPrice"] != null && collectedValues["MaximumPrice"] != null)
                ViewBag.vendors = _databaseConnection.Vendors.Where(n => n.LocationId == locationId && n.VendorServiceId == serviceId &&
                                                        n.EventId == null && n.MinimumPrice >= minimumPrice &&
                                                        n.MaximumPrice >= maximumPrice &&
                                                        (n.AverageRating == ratingOne || n.AverageRating == ratingTwo ||
                                                         n.AverageRating == ratingThree
                                                         || n.AverageRating == ratingFour ||
                                                         n.AverageRating == ratingFive) && n.MinimumPrice >= minimumPrice && n.MaximumPrice <= maximumPrice).ToList();
            if (collectedValues["VendorServiceId"] == null && collectedValues["LocationId"] == null &&
                collectedValues["checkbox"] == null && collectedValues["MinimumPrice"] == null && collectedValues["MaximumPrice"] == null)
            {
                ViewBag.vendors = _databaseConnection.Vendors.Where(n => n.EventId == null).ToList();
                ViewBag.VendorServiceId = new SelectList(_databaseConnection.VendorServices, "VendorServiceId", "ServiceName");
                ViewBag.LocationId = new SelectList(_databaseConnection.Locations, "LocationId", "Name");
            }

            if (serviceId != null)
                ViewBag.VendorServiceId = new SelectList(_databaseConnection.VendorServices, "VendorServiceId", "ServiceName", serviceId);
            if (categoryId != null)
                ViewBag.VendorServiceId = new SelectList(_databaseConnection.VendorServices, "VendorServiceId", "ServiceName",
                    categoryId);
            ViewBag.LocationId = new SelectList(_databaseConnection.Locations, "LocationId", "Name", locationId);
            ViewBag.rate1 = ratingOne;
            ViewBag.rate2 = ratingTwo;
            ViewBag.rate3 = ratingThree;
            ViewBag.rate4 = ratingFour;
            ViewBag.rate5 = ratingFive;
            return View();
        }

        // GET: Vendors/ListOfVendors/SearchParameters
        [SessionExpire]
        public ActionResult EventVendors(FormCollection collectedValues)
        {
            long? serviceId = null;
            long? locationId = null;
            if (collectedValues["VendorServiceId"] != "")
                serviceId = Convert.ToInt64(collectedValues["VendorServiceId"]);
            if (collectedValues["VendorServiceId"] != "")
                locationId = Convert.ToInt64(collectedValues["LocationId"]);
            ViewBag.VendorServiceId = new SelectList(_databaseConnection.VendorServices, "VendorServiceId", "ServiceName", serviceId);
            ViewBag.LocationId = new SelectList(_databaseConnection.Locations, "LocationId", "Name", locationId);
            if (locationId == 0 && serviceId == 0)
            {
                ViewBag.vendors = _databaseConnection.Vendors.ToList();
                return View();
            }
            ViewBag.vendors = _databaseConnection.Vendors.Where(n => n.LocationId == locationId && n.VendorServiceId == serviceId)
                .ToList();
            return View();
        }

        // GET: Vendors/Create
        public ActionResult Create()
        {
            ViewBag.VendorServiceId = new SelectList(_databaseConnection.VendorServices, "VendorServiceId", "ServiceName");
            ViewBag.LocationId = new SelectList(_databaseConnection.Locations, "LocationId", "Name");
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
            var allUsers = _databaseConnection.AppUsers;


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

            _databaseConnection.Vendors.Add(vendor);
            _databaseConnection.SaveChanges();

            var mapping = new EventVendorMapping();
            if (events != null) mapping.EventId = events.EventId;
            mapping.EventPlannerId = loggedinuser.EventPlannerId;
            mapping.VendorId = vendor.VendorId;
            mapping.DateCreated = DateTime.Now;
            mapping.DateLastModified = DateTime.Now;
            mapping.LastModifiedBy = loggedinuser.AppUserId;
            mapping.CreatedBy = loggedinuser.AppUserId;

            _databaseConnection.EventVendorMappings.Add(mapping);
            _databaseConnection.SaveChanges();
            TempData["display"] = "You have successfully added a personal vendor to your event!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("ListOfVendors");
        }

        [SessionExpire]
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
                user = _databaseConnection.AppUsers.Find(loggedinuser.AppUserId);
                if (user != null)
                {
                    user.Password = new Hashing().HashPassword(vendor.ConfirmPassword);
                    vendor.Password = null;
                    vendor.ConfirmPassword = null;

                    _databaseConnection.Entry(user).State = EntityState.Modified;
                    _databaseConnection.Entry(vendor).State = EntityState.Modified;
                }
            }
            _databaseConnection.SaveChanges();
            loggedinuser = user;
            return View("Profile");
        }

        // GET: Vendors/Create
        public ActionResult Register(long? id)
        {
            ViewBag.VendorServiceId = new SelectList(_databaseConnection.VendorServices, "VendorServiceId", "ServiceName");
            ViewBag.LocationId = new SelectList(_databaseConnection.Locations, "LocationId", "Name");
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
            var role = _databaseConnection.Roles.FirstOrDefault(m => m.Name == "Vendor");
            var packageId = Convert.ToInt64(collectedValues["packageId"]);
            var pacakge = _databaseConnection.VendorPackages.Find(packageId);
            if (ModelState.IsValid)
            {
                vendor.DateCreated = DateTime.Now;
                vendor.DateLastModified = DateTime.Now;
                if (_databaseConnection.AppUsers.Any(n => n.Email == vendor.Email))
                {
                    TempData["display"] = "The email entered already exist! Try another one!";
                    TempData["notificationtype"] = NotificationType.Error.ToString();
                    ViewBag.VendorServiceId = new SelectList(_databaseConnection.VendorServices, "VendorServiceId", "ServiceName");
                    ViewBag.LocationId = new SelectList(_databaseConnection.Locations, "LocationId", "Name");
                    ViewBag.packageId = packageId;
                    return View(vendor);
                }
                vendor.LastModifiedBy = 1;
                vendor.CreatedBy = 1;
                vendor.EventPlannerId = null;
                vendor.EventId = null;
                vendor.Password = new Hashing().HashPassword(vendor.ConfirmPassword);
                vendor.ConfirmPassword = vendor.Password;
                if (logo != null && logo.FileName != "")
                    vendor.Logo = new FileUploader().UploadFile(logo, UploadType.VendorLogo);

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
                appUser.Status = UserAccountStatus.Enabled.ToString();
                if (role != null) appUser.RoleId = role.RoleId;
                appUser.Password = vendor.Password;


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

                ViewBag.VendorServiceId = new SelectList(_databaseConnection.VendorServices, "VendorServiceId", "ServiceName",
                    vendor.VendorServiceId);
                ViewBag.LocationId = new SelectList(_databaseConnection.Locations, "LocationId", "Name", vendor.LocationId);
                return RedirectToAction("Invoice", "VendorPackages", new {id = packageSetting.VendorPackageId});
            }
            ViewBag.VendorServiceId = new SelectList(_databaseConnection.VendorServices, "VendorServiceId", "ServiceName",
                vendor.VendorServiceId);
            ViewBag.LocationId = new SelectList(_databaseConnection.Locations, "LocationId", "Name", vendor.LocationId);
            return View(vendor);
        }

        [HttpGet]
        [AllowAnonymous]
        // GET: EventPlanners/Pricing
        public ActionResult Pricing()
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var packages = _databaseConnection.VendorPackageSettings.Include(n => n.VendorPackage);

            var packageSubscribed =
                packages.SingleOrDefault(
                    n =>
                        n.VendorId == loggedinuser.VendorId &&
                        n.Status == PackageStatusEnum.Active.ToString());
            if (packageSubscribed != null)
                Session["subscribe"] = packageSubscribed;
            return View(_databaseConnection.VendorPackages.ToList());
        }

        [HttpGet]
        [AllowAnonymous]
        // GET: EventPlanners/Invoice
        public ActionResult Invoice(long id)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var selectedPackage = _databaseConnection.VendorPackages.Find(id);
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
            var packageSetting = _databaseConnection.VendorPackageSettings.Include(n => n.VendorPackage);
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
                _databaseConnection.Entry(packageSubscribed).State = EntityState.Modified;
                _databaseConnection.SaveChanges();

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
                _databaseConnection.VendorPackageSettings.Add(packageToSubscribed);
                _databaseConnection.SaveChanges();

                //commit invoice to database
                if (invoice != null) _databaseConnection.SubscriptionInvoices.Add(invoice);
                _databaseConnection.SaveChanges();
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
            _databaseConnection.VendorPackageSettings.Add(packageToSubscribed);
            _databaseConnection.SaveChanges();
            if (invoice != null) _databaseConnection.SubscriptionInvoices.Add(invoice);
            _databaseConnection.SaveChanges();

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
            if (vendor != null) _databaseConnection.Vendors.Add(vendor);
            _databaseConnection.SaveChanges();

            var appUser = Session["vendoruser"] as AppUser;
            if (vendor != null)
                if (appUser != null)
                {
                    appUser.VendorId = vendor.VendorId;
                    _databaseConnection.AppUsers.Add(appUser);
                    _databaseConnection.SaveChanges();

                    var packageSetting = Session["vendorpackage"] as VendorPackageSetting;
                    if (packageSetting != null)
                    {
                        packageSetting.AppUserId = appUser.AppUserId;
                        packageSetting.VendorId = vendor.VendorId;
                        _databaseConnection.VendorPackageSettings.Add(packageSetting);
                    }
                    var vendorImage = Session["vendorimage"] as VendorImage;
                    if (vendorImage != null)
                    {
                        vendorImage.VendorId = vendor.VendorId;
                        _databaseConnection.VendorImages.Add(vendorImage);
                        _databaseConnection.SaveChanges();
                    }
                    _databaseConnection.SaveChanges();
                    new MailerDaemon().NewVendor(vendor, appUser.AppUserId);
                    var invoice = Session["invoice"] as SubscriptionInvoice;
                    if (invoice != null)
                        invoice.AppUserId = appUser.AppUserId;
                    if (invoice != null)
                    {
                        invoice.VendorId = vendor.VendorId;
                        invoice.CreatedBy = appUser.AppUserId;
                        invoice.LastModifiedBy = appUser.AppUserId;

                        _databaseConnection.SubscriptionInvoices.Add(invoice);
                    }
                    _databaseConnection.SaveChanges();
                    TempData["login"] =
                        "You have successfully registered as a vendor! Enter credentials to login";
                    TempData["notificationtype"] = NotificationType.Success.ToString();
                    return RedirectToAction("Login", "Account");
                }
            TempData["display"] =
                "There might have been an ssue while trying to create the account, Try again!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Pricing", "Vendors");
        }

        // GET: Vendors/Edit/5
        [SessionExpire]
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var vendor = _databaseConnection.Vendors.Find(id);
            if (vendor == null)
                return HttpNotFound();
            ViewBag.VendorServiceId = new SelectList(_databaseConnection.VendorServices, "VendorServiceId", "ServiceName",
                vendor.VendorServiceId);
            return View(vendor);
        }

        // POST: Vendors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
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
                var vendorUser = _databaseConnection.AppUsers.SingleOrDefault(n => n.VendorId == vendor.VendorId);
                vendor.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    vendor.LastModifiedBy = loggedinuser.AppUserId;
                    if (logo != null && logo.FileName != "")
                        vendor.Logo = new FileUploader().UploadFile(logo, UploadType.VendorLogo);
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                _databaseConnection.Entry(vendor).State = EntityState.Modified;
                _databaseConnection.SaveChanges();
                if (vendorUser != null)
                {
                    vendorUser.DateLastModified = DateTime.Now;
                    vendorUser.Email = vendor.Email;
                    vendorUser.Firstname = vendor.Name;
                    vendorUser.Lastname = vendor.Name;
                    vendorUser.Mobile = vendor.Mobile;
                }
                _databaseConnection.Entry(vendorUser).State = EntityState.Modified;
                _databaseConnection.SaveChanges();
                TempData["vendor"] = "You have successfully modified a vendor!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Profile");
            }
            ViewBag.VendorServiceId = new SelectList(_databaseConnection.VendorServices, "VendorServiceId", "ServiceName",
                vendor.VendorServiceId);
            ViewBag.LocationId = new SelectList(_databaseConnection.Locations, "LocationId", "Name", vendor.LocationId);
            return View(vendor);
        }

        // GET: Vendors/Delete/5
        [SessionExpire]
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var vendor = _databaseConnection.Vendors.Find(id);
            if (vendor == null)
                return HttpNotFound();
            return View(vendor);
        }

        // POST: Vendors/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult DeleteConfirmed(long id)
        {
            var vendor = _databaseConnection.Vendors.Find(id);
            var user = _databaseConnection.AppUsers.SingleOrDefault(n => n.VendorId == vendor.VendorId);
            var eventMapping = _databaseConnection.EventVendorMappings.Where(n => n.VendorId == vendor.VendorId);
            var reviews = _databaseConnection.VendorReviews.Where(n => n.VendorId == vendor.VendorId);
            var subscription = _databaseConnection.SubscriptionInvoices.Where(n => n.VendorId == vendor.VendorId);
            var images = _databaseConnection.VendorImages.Where(n => n.VendorId == vendor.VendorId);
            var enquiries = _databaseConnection.VendorEnquiries.Where(n => n.VendorId == vendor.VendorId);
            var settings = _databaseConnection.VendorPackageSettings.Where(n => n.VendorId == vendor.VendorId);
            if (user != null) _databaseConnection.AppUsers.Remove(user);
            foreach (var item in eventMapping)
                _databaseConnection.EventVendorMappings.Remove(item);
            foreach (var item in reviews)
                _databaseConnection.VendorReviews.Remove(item);
            foreach (var item in subscription)
                _databaseConnection.SubscriptionInvoices.Remove(item);
            foreach (var item in images)
                _databaseConnection.VendorImages.Remove(item);
            foreach (var item in enquiries)
                _databaseConnection.VendorEnquiries.Remove(item);
            foreach (var item in settings)
                _databaseConnection.VendorPackageSettings.Remove(item);
            _databaseConnection.SaveChanges();
            _databaseConnection.Vendors.Remove(vendor);
            _databaseConnection.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _databaseConnection.Dispose();
            base.Dispose(disposing);
        }
    }
}