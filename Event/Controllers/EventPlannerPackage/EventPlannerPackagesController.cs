using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.AuthenticationManagement;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.EventPlannerPackage
{
    public class EventPlannerPackagesController : Controller
    {
        private readonly EventDataContext _databaseConnection = new EventDataContext();

        // GET: EventPlannerPackages
        [SessionExpire]
        public ActionResult Index()
        {
            return View(_databaseConnection.EventPlannerPackages.ToList());
        }

        [HttpGet]
        [AllowAnonymous]
        // GET: EventPlanners/Pricing
        public ActionResult Pricing()
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var packages = _databaseConnection.EventPlannerPackageSettings.Include(n => n.EventPlannerPackage);

            var packageSubscribed =
                packages.SingleOrDefault(
                    n =>
                        n.EventPlannerId == loggedinuser.EventPlannerId &&
                        n.Status == PackageStatusEnum.Active.ToString());
            if (packageSubscribed != null)
                Session["subscribe"] = packageSubscribed;
            return View(_databaseConnection.EventPlannerPackages.ToList());
        }

        [HttpGet]
        [AllowAnonymous]
        // GET: EventPlanners/Invoice
        public ActionResult Invoice(long id)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var selectedPackage = _databaseConnection.EventPlannerPackages.Find(id);
            var subscriptionInvoice = new SubscriptionInvoice();

            //random number
            var generator = new Random();
            var randomNumber = generator.Next(0, 1000000).ToString("D6");

            if (loggedinuser != null)
            {
                subscriptionInvoice.AppUserId = loggedinuser.AppUserId;
                if (loggedinuser.EventPlannerId != null)
                    subscriptionInvoice.EventPlannerId = (long) loggedinuser.EventPlannerId;
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
            var packages = _databaseConnection.EventPlannerPackageSettings.Include(n => n.EventPlannerPackage);
            //current subscription
            var packageSubscribed =
                packages.SingleOrDefault(
                    n =>
                        n.EventPlannerId == loggedinuser.EventPlannerId &&
                        n.Status == PackageStatusEnum.Active.ToString());


            if (packageSubscribed != null)
            {
                //make the current package inactive
                packageSubscribed.Status = PackageStatusEnum.Inactive.ToString();
                packageSubscribed.DateLastModified = DateTime.Now;
                _databaseConnection.Entry(packageSubscribed).State = EntityState.Modified;
                _databaseConnection.SaveChanges();

                //populate new package
                if (loggedinuser != null && loggedinuser.EventPlannerId != null)
                    packageToSubscribed.EventPlannerId = (long) loggedinuser.EventPlannerId;
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
                _databaseConnection.EventPlannerPackageSettings.Add(packageToSubscribed);
                _databaseConnection.SaveChanges();

                //commit invoice to database
                if (invoice != null) _databaseConnection.SubscriptionInvoices.Add(invoice);
                _databaseConnection.SaveChanges();
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
                    packageToSubscribed.EventPlannerId = (long) loggedinuser.EventPlannerId;
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
            _databaseConnection.EventPlannerPackageSettings.Add(packageToSubscribed);
            _databaseConnection.SaveChanges();
            if (invoice != null) _databaseConnection.SubscriptionInvoices.Add(invoice);
            _databaseConnection.SaveChanges();

            Session["package"] = null;
            Session["invoice"] = null;
            Session["subscribe"] = packageToSubscribed;

            //display notification
            TempData["display"] = "You have successfully subscribed to the package!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Dashboard", "Home");
        }

        // GET: EventPlannerPackages/Details/5
        [SessionExpire]
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventPlannerPackage = _databaseConnection.EventPlannerPackages.Find(id);
            if (eventPlannerPackage == null)
                return HttpNotFound();
            return View(eventPlannerPackage);
        }

        // GET: EventPlannerPackages/Create
        [SessionExpire]
        public ActionResult Create()
        {
            return View();
        }

        // POST: EventPlannerPackages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
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
                if (_databaseConnection.EventPlannerPackages.Any(n => n.PackageGrade == eventPlannerPackage.PackageGrade))
                {
                    TempData["display"] = "A package already exist with this package grade, Try again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Index");
                }
                _databaseConnection.EventPlannerPackages.Add(eventPlannerPackage);
                _databaseConnection.SaveChanges();
                TempData["display"] = "You have successfully added a package!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }

            return View(eventPlannerPackage);
        }

        // GET: EventPlannerPackages/Edit/5
        [SessionExpire]
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventPlannerPackage = _databaseConnection.EventPlannerPackages.Find(id);
            if (eventPlannerPackage == null)
                return HttpNotFound();
            return View(eventPlannerPackage);
        }

        // POST: EventPlannerPackages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
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
                _databaseConnection.Entry(eventPlannerPackage).State = EntityState.Modified;
                _databaseConnection.SaveChanges();
                TempData["display"] = "You have successfully modified the pacakge!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            return View(eventPlannerPackage);
        }

        // GET: EventPlannerPackages/Delete/5
        [SessionExpire]
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventPlannerPackage = _databaseConnection.EventPlannerPackages.Find(id);
            if (eventPlannerPackage == null)
                return HttpNotFound();
            return View(eventPlannerPackage);
        }

        // POST: EventPlannerPackages/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult DeleteConfirmed(long id)
        {
            var eventPlannerPackage = _databaseConnection.EventPlannerPackages.Find(id);
            _databaseConnection.EventPlannerPackages.Remove(eventPlannerPackage);
            _databaseConnection.SaveChanges();
            TempData["display"] = "You have successfully deleted the!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
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