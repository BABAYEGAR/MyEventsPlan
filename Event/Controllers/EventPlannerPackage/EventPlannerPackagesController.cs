using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.EventPlannerPackage
{
    public class EventPlannerPackagesController : Controller
    {
        private readonly EventPlannerPackageDataContext db = new EventPlannerPackageDataContext();
        private readonly EventDataContext _dbd = new EventDataContext();
        private readonly SubscriptionInvoiceDataContext _dbe = new SubscriptionInvoiceDataContext();
        private readonly EventPlannerPackageSettingDataContext _dbf = new EventPlannerPackageSettingDataContext();

        // GET: EventPlannerPackages
        public ActionResult Index()
        {
            return View(db.EventPlannerPackages.ToList());
        }
        [HttpGet]
        [AllowAnonymous]
        // GET: EventPlanners/Pricing
        public ActionResult Pricing()
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var packages = db.EventPlannerPackageSettings.Include(n => n.EventPlannerPackage);

            var packageSubscribed =
                packages.SingleOrDefault(
                    n =>
                        (n.EventPlannerId == loggedinuser.EventPlannerId) &&
                        (n.Status == PackageStatusEnum.Active.ToString()));
            if (packageSubscribed != null)
                Session["subscribe"] = packageSubscribed;
            return View(db.EventPlannerPackages.ToList());
        }
        [HttpGet]
        [AllowAnonymous]
        // GET: EventPlanners/Invoice
        public ActionResult Invoice(long id)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var selectedPackage = db.EventPlannerPackages.Find(id);
            var subscriptionInvoice = new SubscriptionInvoice();

            //random number
            var generator = new Random();
            var randomNumber = generator.Next(0, 1000000).ToString("D6");

            if (loggedinuser != null)
            {
                subscriptionInvoice.AppUserId = loggedinuser.AppUserId;
                if (loggedinuser.EventPlannerId != null)
                    subscriptionInvoice.EventPlannerId = (long)loggedinuser.EventPlannerId;
                subscriptionInvoice.DateCreated = DateTime.Now;
                subscriptionInvoice.DateLastModified = DateTime.Now;
                subscriptionInvoice.CreatedBy = loggedinuser.AppUserId;
                subscriptionInvoice.LastModifiedBy = loggedinuser.AppUserId;
            }
            subscriptionInvoice.InvoiceNumber = "#" + randomNumber;
            if (selectedPackage != null)
            {
                subscriptionInvoice.PackageId = selectedPackage.EventPlannerPackageId;

                Session["package"] = selectedPackage;
            }
            Session["invoice"] = subscriptionInvoice;
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        // GET: EventPlanners/Pricing
        public ActionResult ConfirmPayment()
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var package = Session["package"] as Event.Data.Objects.Entities.EventPlannerPackage;
            var invoice = Session["invoice"] as SubscriptionInvoice;
            var packageToSubscribed = new EventPlannerPackageSetting();
            var packages = _dbd.EventPlannerPackageSettings.Include(n => n.EventPlannerPackage);
            //current subscription
            var packageSubscribed =
                packages.SingleOrDefault(
                    n =>
                        (n.EventPlannerId == loggedinuser.EventPlannerId) &&
                        (n.Status == PackageStatusEnum.Active.ToString()));


            if (packageSubscribed != null)
            {
                //make the current package inactive
                packageSubscribed.Status = PackageStatusEnum.Inactive.ToString();
                packageSubscribed.DateLastModified = DateTime.Now;
                _dbf.Entry(packageSubscribed).State = EntityState.Modified;
                _dbf.SaveChanges();

                //populate new package
                if ((loggedinuser != null) && (loggedinuser.EventPlannerId != null))
                    packageToSubscribed.EventPlannerId = (long)loggedinuser.EventPlannerId;
                if (loggedinuser != null)
                {
                    packageToSubscribed.CreatedBy = loggedinuser.AppUserId;
                    packageToSubscribed.LastModifiedBy = loggedinuser.AppUserId;
                    packageToSubscribed.AppUserId = loggedinuser.AppUserId;
                }

                packageToSubscribed.DateCreated = DateTime.Now;
                packageToSubscribed.DateLastModified = DateTime.Now;
                packageToSubscribed.Status = PackageStatusEnum.Active.ToString();
                packageToSubscribed.SubscribedEvent = 0;

                //package data
                if (package != null)
                {
                    packageToSubscribed.EventPlannerPackageId = package.EventPlannerPackageId;
                    packageToSubscribed.AllowedEvent = package.MaximumEvents;
                }
                //commit package to database
                _dbf.EventPlannerPackageSettings.Add(packageToSubscribed);
                _dbf.SaveChanges();

                //commit invoice to database
                if (invoice != null) _dbe.SubscriptionInvoices.Add(invoice);
                _dbe.SaveChanges();
                Session["package"] = null;
                Session["invoice"] = null;
                Session["subscribe"] = packageSubscribed;
                //display notification
                TempData["display"] = "You have successfully subscribed to the package!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Setting", "Account");
            }
            if (loggedinuser != null)
            {
                if (loggedinuser.EventPlannerId != null)
                    packageToSubscribed.EventPlannerId = (long)loggedinuser.EventPlannerId;
                packageToSubscribed.CreatedBy = loggedinuser.AppUserId;
                packageToSubscribed.LastModifiedBy = loggedinuser.AppUserId;
                packageToSubscribed.AppUserId = loggedinuser.AppUserId;
            }
            packageToSubscribed.DateCreated = DateTime.Now;
            packageToSubscribed.DateLastModified = DateTime.Now;
            packageToSubscribed.Status = PackageStatusEnum.Active.ToString();
            packageToSubscribed.SubscribedEvent = 0;


            //package data
            if (package != null)
            {
                packageToSubscribed.EventPlannerPackageId = package.EventPlannerPackageId;
                packageToSubscribed.AllowedEvent = package.MaximumEvents;
            }
            _dbf.EventPlannerPackageSettings.Add(packageToSubscribed);
            _dbf.SaveChanges();
            if (invoice != null) _dbe.SubscriptionInvoices.Add(invoice);
            _dbe.SaveChanges();

            Session["package"] = null;
            Session["invoice"] = null;
            Session["subscribe"] = packageToSubscribed;

            //display notification
            TempData["display"] = "You have successfully subscribed to the package!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Dashboard", "Home");
        }
        // GET: EventPlannerPackages/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventPlannerPackage = db.EventPlannerPackages.Find(id);
            if (eventPlannerPackage == null)
                return HttpNotFound();
            return View(eventPlannerPackage);
        }

        // GET: EventPlannerPackages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EventPlannerPackages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "EventPlannerPackageId,PackageName,Amount,MaximumEvents,Description,PackageGrade")]
            Event.Data.Objects.Entities.EventPlannerPackage eventPlannerPackage, FormCollection collectedValues)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                eventPlannerPackage.DateCreated = DateTime.Now;
                eventPlannerPackage.DateLastModified = DateTime.Now;
                eventPlannerPackage.PackageGrade =
                    typeof(EventPlannerPackageEnum).GetEnumName(int.Parse(collectedValues["PackageGrade"]));
                if (loggedinuser != null)
                {
                    eventPlannerPackage.LastModifiedBy = loggedinuser.AppUserId;
                    eventPlannerPackage.CreatedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                if (db.EventPlannerPackages.Any(n => n.PackageGrade == eventPlannerPackage.PackageGrade))
                {
                    TempData["display"] = "A package already exist with this package grade, Try again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Index");
                }
                db.EventPlannerPackages.Add(eventPlannerPackage);
                db.SaveChanges();
                TempData["display"] = "You have successfully added a package!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }

            return View(eventPlannerPackage);
        }

        // GET: EventPlannerPackages/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventPlannerPackage = db.EventPlannerPackages.Find(id);
            if (eventPlannerPackage == null)
                return HttpNotFound();
            return View(eventPlannerPackage);
        }

        // POST: EventPlannerPackages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include =
                "EventPlannerPackageId,PackageGrade,PackageName,Description,Amount,MaximumEvents,CreatedBy,DateCreated")]
            Event.Data.Objects.Entities.EventPlannerPackage eventPlannerPackage)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                eventPlannerPackage.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    eventPlannerPackage.LastModifiedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                db.Entry(eventPlannerPackage).State = EntityState.Modified;
                db.SaveChanges();
                TempData["display"] = "You have successfully modified the pacakge!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            return View(eventPlannerPackage);
        }

        // GET: EventPlannerPackages/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventPlannerPackage = db.EventPlannerPackages.Find(id);
            if (eventPlannerPackage == null)
                return HttpNotFound();
            return View(eventPlannerPackage);
        }

        // POST: EventPlannerPackages/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            var eventPlannerPackage = db.EventPlannerPackages.Find(id);
            db.EventPlannerPackages.Remove(eventPlannerPackage);
            db.SaveChanges();
            TempData["display"] = "You have successfully deleted the!";
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