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

namespace MyEventPlan.Controllers.EventManagement
{
    public class EventPlannersController : Controller
    {
        private readonly EventDataContext _databaseConnection = new EventDataContext();

        // GET: EventPlanners
        [SessionExpire]
        public ActionResult Index()
        {
            var eventPlanners = _databaseConnection.EventPlanners.Include(e => e.Role);
            return View(eventPlanners.ToList());
        }

        // GET: EventPlanners/Details/5
        public ActionResult EventPlannerDetails(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventPlanner = _databaseConnection.EventPlanners.Find(id);
            if (eventPlanner == null)
                return HttpNotFound();
            return View(eventPlanner);
        }

        // GET: EventPlanners/Details/5
        [SessionExpire]
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventPlanner = _databaseConnection.EventPlanners.Find(id);
            if (eventPlanner == null)
                return HttpNotFound();
            return View(eventPlanner);
        }

        // GET: EventPlanners/Create
        public ActionResult Create(string type)
        {
            ViewBag.type = type;
            ViewBag.LocationId = new SelectList(_databaseConnection.Locations, "LocationId", "Name");
            return View();
        }

        // POST: EventPlanners/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include =
                "EventPlannerId,Name,LocationId,Email,Mobile,ConfirmPassword,FacebookPage,TwitterPage,InstagramPage,Website,PricingDetails" +
                ",YoutubePage,GooglePlusPage,MaximumPrice,MinimumPrice,About,AverageRating,Password,Type,RoleId")]
            EventPlanner
                eventPlanner, FormCollection collectedValues)
        {
            if (ModelState.IsValid)
            {
                var password = new Hashing().HashPassword(collectedValues["ConfirmPassword"]);
                var role = _databaseConnection.Roles.FirstOrDefault(m => m.Name == "Event Planner");
                var logo = Request.Files["logo"];
                eventPlanner.RoleId = role?.RoleId;
                eventPlanner.Password = password;
                eventPlanner.ConfirmPassword = password;
                eventPlanner.Type = collectedValues["Type"];
                if (logo != null && logo.FileName != "")
                {
                    eventPlanner.Logo = new FileUploader().UploadFile(logo, UploadType.EventPlannerLogo);
                }
                if (_databaseConnection.AppUsers.Any(n => n.Email == eventPlanner.Email))
                {
                    TempData["display"] = "The email entered already exist! Try another one!";
                    TempData["notificationtype"] = NotificationType.Error.ToString();
                    return View(eventPlanner);
                }
                //create app user
                var appuser = new AppUser();

                appuser.Email = eventPlanner.Email;
                appuser.Password = password;
                appuser.Firstname = eventPlanner.Name;
                appuser.Lastname = eventPlanner.Name;
                appuser.Mobile = eventPlanner.Mobile;
                appuser.DateCreated = DateTime.Now;
                appuser.DateLastModified = DateTime.Now;
                appuser.LastModifiedBy = null;
                appuser.CreatedBy = null;
                appuser.BackgroundColor = BackgroundColor.Default.ToString();
                appuser.Status = UserAccountStatus.Enabled.ToString();

                var checkeventPlanner = _databaseConnection.EventPlanners.SingleOrDefault(n => n.Email == eventPlanner.Email);
                if (checkeventPlanner != null)
                {
                    TempData["planner"] = "The email already exist, try another email!";
                    TempData["notificationtype"] = NotificationType.Error.ToString();
                    return RedirectToAction("Create", eventPlanner);
                }
                _databaseConnection.EventPlanners.Add(eventPlanner);
                _databaseConnection.SaveChanges();


                appuser.EventPlannerId = eventPlanner.EventPlannerId;
                appuser.RoleId = eventPlanner.RoleId;


                _databaseConnection.AppUsers.Add(appuser);
                _databaseConnection.SaveChanges();

                var eventPlannerSetting = new EventPlannerPackageSetting();
                eventPlannerSetting.EventPlannerId = eventPlanner.EventPlannerId;
                eventPlannerSetting.AppUserId = appuser.AppUserId;
                eventPlannerSetting.Status = PackageStatusEnum.Active.ToString();
                eventPlannerSetting.SubscribedEvent = 0;
                eventPlannerSetting.AllowedEvent = 10;
                eventPlannerSetting.EventPlannerPackageId = null;
                eventPlannerSetting.CreatedBy = null;
                eventPlannerSetting.LastModifiedBy = null;
                eventPlannerSetting.DateCreated = DateTime.Now;
                eventPlannerSetting.DateLastModified = DateTime.Now;

                _databaseConnection.EventPlannerPackageSettings.Add(eventPlannerSetting);
                _databaseConnection.SaveChanges();
                new MailerDaemon().NewEventPlanner(eventPlanner, appuser.AppUserId);
                TempData["login"] = "You have successfully signed up to MyEventsPlan!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Login", "Account");
            }

            return View(eventPlanner);
        }

        // GET: EventPlanners/Edit/5
        [SessionExpire]
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventPlanner = _databaseConnection.EventPlanners.Find(id);
            if (eventPlanner == null)
                return HttpNotFound();
            ViewBag.RoleId = new SelectList(_databaseConnection.Roles, "RoleId", "Name", eventPlanner.RoleId);
            return View(eventPlanner);
        }

        // POST: EventPlanners/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Edit(
            [Bind(Include =
                "EventPlannerId,Name,LocationId,Email,Mobile,ConfirmPassword,FacebookPage,TwitterPage,InstagramPage,Website,PricingDetails" +
                ",YoutubePage,GooglePlusPage,MaximumPrice,MinimumPrice,About,AverageRating,Password,Type,RoleId")]
            EventPlanner
                eventPlanner, FormCollection collectedValues)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                //create app user
                if (loggedinuser != null)
                {
                    var appuser = _databaseConnection.AppUsers.Find(loggedinuser.AppUserId);

                    if (appuser != null)
                    {
                        appuser.Email = eventPlanner.Email;
                        appuser.Firstname = eventPlanner.Name;
                        appuser.Lastname = eventPlanner.Name;
                        appuser.Mobile = eventPlanner.Mobile;
                        appuser.DateLastModified = DateTime.Now;
                        appuser.LastModifiedBy = null;

                        _databaseConnection.Entry(eventPlanner).State = EntityState.Modified;
                        _databaseConnection.SaveChanges();
                        _databaseConnection.Entry(appuser).State = EntityState.Modified;
                        _databaseConnection.SaveChanges();

                        Session["eventplanner"] = eventPlanner;
                        Session["myeventplanloggedinuser"] = appuser;
                    }
                }


                TempData["display"] = "You have successfully modified your profile!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Setting", "Account");
            }
            TempData["display"] = " Unable to update profile!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Setting", "Account");
        }


        // GET: EventPlanners/Delete/5
        [SessionExpire]
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventPlanner = _databaseConnection.EventPlanners.Find(id);
            if (eventPlanner == null)
                return HttpNotFound();
            return View(eventPlanner);
        }

        // POST: EventPlanners/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult DeleteConfirmed(long id)
        {
            var eventPlanner = _databaseConnection.EventPlanners.Find(id);
            var user = _databaseConnection.AppUsers.SingleOrDefault(n => n.EventPlannerId == id);
            var events = _databaseConnection.Event.Where(n => n.EventPlannerId == id).ToList();
            var prospects = _databaseConnection.Prospects.Where(n => n.EventPlannerId == id).ToList();
            var subscription = _databaseConnection.SubscriptionInvoices.Where(n => n.EventPlannerId == id).ToList();
            var settings = _databaseConnection.EventPlannerPackageSettings.Where(n => n.EventPlannerId == id).ToList();
            var resources = _databaseConnection.Resources.Where(n => n.EventPlannerId == id).ToList();
            var contacts = _databaseConnection.Contacts.Where(n => n.EventPlannerId == id).ToList();
            var reviews = _databaseConnection.EventPlannerReviews.Where(n => n.EventPlannerId == id).ToList();
            var enquiry = _databaseConnection.EventPlannerEnquiries.Where(n => n.EventPlannerId == id).ToList();


            foreach (var item in prospects)
                _databaseConnection.Prospects.Remove(item);
            foreach (var items in events)
            {
                var clients = _databaseConnection.Clients.Where(n => n.EventId == items.EventId);
                var vendors = _databaseConnection.Vendors.Where(n => n.EventId == items.EventId);
                var resourceMapping = _databaseConnection.EventResourceMapping.Where(n => n.EventId == items.EventId);
                var staffMapping = _databaseConnection.StaffEventMapping.Where(n => n.EventId == items.EventId);
                var budget = _databaseConnection.Budgets.Where(n => n.EventId == items.EventId);
                var checkList = _databaseConnection.CheckLists.Where(n => n.EventId == items.EventId);
                var listItems = _databaseConnection.CheckListItems.Where(n => n.EventId == items.EventId);
                var invoice = _databaseConnection.Invoices.Where(n => n.EventId == items.EventId);
                var geustList = _databaseConnection.GuestLists.Where(n => n.EventId == items.EventId);
                var guests = _databaseConnection.Guests.Where(n => n.EventId == items.EventId);
                var appointment = _databaseConnection.Appointments.Where(n => n.EventId == items.EventId);
                var vendorMapping = _databaseConnection.EventVendorMappings.Where(n => n.EventId == items.EventId);
                var notes = _databaseConnection.Notes.Where(n => n.EventId == items.EventId);
                var tasks = _databaseConnection.Tasks.Where(n => n.EventId == items.EventId);
                foreach (var item in clients)
                {
                    //get clients with their login details and their messages
                    var users = _databaseConnection.AppUsers.Where(n => n.ClientId == item.ClientId);
                    foreach (var itemsss in users)
                    {
                        var messages = _databaseConnection.Messages.Where(n => n.AppUserId == itemsss.AppUserId);
                        var groupMembers = _databaseConnection.MessageGroupMembers.Where(n => n.AppUserId == itemsss.AppUserId);
                        var notifications = _databaseConnection.Notifications.Where(n => n.AppUserId == itemsss.AppUserId);
                        var checklist = _databaseConnection.PersonalCheckLists.Where(n => n.AppUserId == itemsss.AppUserId);
                        foreach (var itemss in messages)
                            _databaseConnection.Messages.Remove(itemss);
                        foreach (var member in groupMembers)
                            _databaseConnection.MessageGroupMembers.Remove(member);
                        foreach (var notification in notifications)
                            _databaseConnection.Notifications.Remove(notification);
                        foreach (var list in checklist)
                        {
                            var checklistItems =
                                _databaseConnection.PersonalCheckListItems.Where(
                                    n => n.PersonalCheckListId == list.PersonalCheckListId);
                            foreach (var listItem in checklistItems)
                                _databaseConnection.PersonalCheckListItems.Remove(listItem);
                            _databaseConnection.PersonalCheckLists.Remove(list);
                        }
                        _databaseConnection.AppUsers.Remove(itemsss);
                    }
                    _databaseConnection.Clients.Remove(item);
                }
                foreach (var item in vendors)
                    _databaseConnection.Vendors.Remove(item);
                foreach (var item in resourceMapping)
                    _databaseConnection.EventResourceMapping.Remove(item);
                foreach (var item in staffMapping)
                    _databaseConnection.StaffEventMapping.Remove(item);
                foreach (var item in budget)
                    _databaseConnection.Budgets.Remove(item);
                foreach (var item in checkList)
                    _databaseConnection.CheckLists.Remove(item);
                foreach (var item in listItems)
                    _databaseConnection.CheckListItems.Remove(item);
                //get event invoive and delete the payments and items
                foreach (var item in invoice)
                {
                    var payments = _databaseConnection.InvoicePayments.Where(n => n.InvoiceId == item.InvoiceId);
                    var invoiceItems = _databaseConnection.InvoiceItems.Where(n => n.InvoiceId == item.InvoiceId);

                    foreach (var itemss in payments)
                        _databaseConnection.InvoicePayments.Remove(itemss);
                    foreach (var itemss in invoiceItems)
                        _databaseConnection.InvoiceItems.Remove(itemss);
                    _databaseConnection.Invoices.Remove(item);
                }
                foreach (var item in geustList)
                    _databaseConnection.GuestLists.Remove(item);
                foreach (var item in guests)
                    _databaseConnection.Guests.Remove(item);
                foreach (var item in appointment)
                    _databaseConnection.Appointments.Remove(item);
                foreach (var item in vendorMapping)
                    _databaseConnection.EventVendorMappings.Remove(item);
                foreach (var item in notes)
                    _databaseConnection.Notes.Remove(item);
                foreach (var item in tasks)
                    _databaseConnection.Tasks.Remove(item);
                _databaseConnection.Event.Remove(items);
            }
            foreach (var item in subscription)
                _databaseConnection.SubscriptionInvoices.Remove(item);
            foreach (var item in resources)
                _databaseConnection.Resources.Remove(item);
            foreach (var item in contacts)
                _databaseConnection.Contacts.Remove(item);

            foreach (var item in settings)
                _databaseConnection.EventPlannerPackageSettings.Remove(item);
            foreach (var item in reviews)
                _databaseConnection.EventPlannerReviews.Remove(item);
            foreach (var item in enquiry)
                _databaseConnection.EventPlannerEnquiries.Remove(item);
            if (user != null)
                _databaseConnection.AppUsers.Remove(user);

            _databaseConnection.EventPlanners.Remove(eventPlanner);
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