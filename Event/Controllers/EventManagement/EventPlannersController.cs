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
        private readonly EventPlannerDataContext db = new EventPlannerDataContext();
        private readonly AppUserDataContext dbc = new AppUserDataContext();
        private readonly EventDataContext dbd = new EventDataContext();

        // GET: EventPlanners
        [SessionExpire]
        public ActionResult Index()
        {
            var eventPlanners = db.EventPlanners.Include(e => e.Role);
            return View(eventPlanners.ToList());
        }

        // GET: EventPlanners/Details/5
        public ActionResult EventPlannerDetails(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventPlanner = db.EventPlanners.Find(id);
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
            var eventPlanner = db.EventPlanners.Find(id);
            if (eventPlanner == null)
                return HttpNotFound();
            return View(eventPlanner);
        }

        // GET: EventPlanners/Create
        public ActionResult Create(string type)
        {
            ViewBag.type = type;
            ViewBag.LocationId = new SelectList(dbd.Locations, "LocationId", "Name");
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
                var role = db.Roles.FirstOrDefault(m => m.Name == "Event Planner");
                var logo = Request.Files["logo"];
                eventPlanner.RoleId = role?.RoleId;
                eventPlanner.Password = password;
                eventPlanner.ConfirmPassword = password;
                eventPlanner.Type = collectedValues["Type"];
                if (logo != null && logo.FileName != "")
                {
                    eventPlanner.Logo = new FileUploader().UploadFile(logo, UploadType.EventPlannerLogo);
                }
                if (dbc.AppUsers.Any(n => n.Email == eventPlanner.Email))
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

                var checkeventPlanner = db.EventPlanners.SingleOrDefault(n => n.Email == eventPlanner.Email);
                if (checkeventPlanner != null)
                {
                    TempData["planner"] = "The email already exist, try another email!";
                    TempData["notificationtype"] = NotificationType.Error.ToString();
                    return RedirectToAction("Create", eventPlanner);
                }
                db.EventPlanners.Add(eventPlanner);
                db.SaveChanges();


                appuser.EventPlannerId = eventPlanner.EventPlannerId;
                appuser.RoleId = eventPlanner.RoleId;


                dbc.AppUsers.Add(appuser);
                dbc.SaveChanges();

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

                dbd.EventPlannerPackageSettings.Add(eventPlannerSetting);
                dbd.SaveChanges();
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
            var eventPlanner = db.EventPlanners.Find(id);
            if (eventPlanner == null)
                return HttpNotFound();
            ViewBag.RoleId = new SelectList(db.Roles, "RoleId", "Name", eventPlanner.RoleId);
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
                    var appuser = dbc.AppUsers.Find(loggedinuser.AppUserId);

                    if (appuser != null)
                    {
                        appuser.Email = eventPlanner.Email;
                        appuser.Firstname = eventPlanner.Name;
                        appuser.Lastname = eventPlanner.Name;
                        appuser.Mobile = eventPlanner.Mobile;
                        appuser.DateLastModified = DateTime.Now;
                        appuser.LastModifiedBy = null;

                        db.Entry(eventPlanner).State = EntityState.Modified;
                        db.SaveChanges();
                        dbc.Entry(appuser).State = EntityState.Modified;
                        dbc.SaveChanges();

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
            var eventPlanner = db.EventPlanners.Find(id);
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
            var eventPlanner = dbd.EventPlanners.Find(id);
            var user = dbd.AppUsers.SingleOrDefault(n => n.EventPlannerId == id);
            var events = dbd.Event.Where(n => n.EventPlannerId == id).ToList();
            var prospects = dbd.Prospects.Where(n => n.EventPlannerId == id).ToList();
            var subscription = dbd.SubscriptionInvoices.Where(n => n.EventPlannerId == id).ToList();
            var settings = dbd.EventPlannerPackageSettings.Where(n => n.EventPlannerId == id).ToList();
            var resources = dbd.Resources.Where(n => n.EventPlannerId == id).ToList();
            var contacts = dbd.Contacts.Where(n => n.EventPlannerId == id).ToList();
            var reviews = dbd.EventPlannerReviews.Where(n => n.EventPlannerId == id).ToList();
            var enquiry = dbd.EventPlannerEnquiries.Where(n => n.EventPlannerId == id).ToList();


            foreach (var item in prospects)
                dbd.Prospects.Remove(item);
            foreach (var items in events)
            {
                var clients = dbd.Clients.Where(n => n.EventId == items.EventId);
                var vendors = dbd.Vendors.Where(n => n.EventId == items.EventId);
                var resourceMapping = dbd.EventResourceMapping.Where(n => n.EventId == items.EventId);
                var staffMapping = dbd.StaffEventMapping.Where(n => n.EventId == items.EventId);
                var budget = dbd.Budgets.Where(n => n.EventId == items.EventId);
                var checkList = dbd.CheckLists.Where(n => n.EventId == items.EventId);
                var listItems = dbd.CheckListItems.Where(n => n.EventId == items.EventId);
                var invoice = dbd.Invoices.Where(n => n.EventId == items.EventId);
                var geustList = dbd.GuestLists.Where(n => n.EventId == items.EventId);
                var guests = dbd.Guests.Where(n => n.EventId == items.EventId);
                var appointment = dbd.Appointments.Where(n => n.EventId == items.EventId);
                var vendorMapping = dbd.EventVendorMappings.Where(n => n.EventId == items.EventId);
                var notes = dbd.Notes.Where(n => n.EventId == items.EventId);
                var tasks = dbd.Tasks.Where(n => n.EventId == items.EventId);
                foreach (var item in clients)
                {
                    //get clients with their login details and their messages
                    var users = dbd.AppUsers.Where(n => n.ClientId == item.ClientId);
                    foreach (var itemsss in users)
                    {
                        var messages = dbd.Messages.Where(n => n.AppUserId == itemsss.AppUserId);
                        var groupMembers = dbd.MessageGroupMembers.Where(n => n.AppUserId == itemsss.AppUserId);
                        var notifications = dbd.Notifications.Where(n => n.AppUserId == itemsss.AppUserId);
                        var checklist = dbd.PersonalCheckLists.Where(n => n.AppUserId == itemsss.AppUserId);
                        foreach (var itemss in messages)
                            dbd.Messages.Remove(itemss);
                        foreach (var member in groupMembers)
                            dbd.MessageGroupMembers.Remove(member);
                        foreach (var notification in notifications)
                            dbd.Notifications.Remove(notification);
                        foreach (var list in checklist)
                        {
                            var checklistItems =
                                dbd.PersonalCheckListItems.Where(
                                    n => n.PersonalCheckListId == list.PersonalCheckListId);
                            foreach (var listItem in checklistItems)
                                dbd.PersonalCheckListItems.Remove(listItem);
                            dbd.PersonalCheckLists.Remove(list);
                        }
                        dbd.AppUsers.Remove(itemsss);
                    }
                    dbd.Clients.Remove(item);
                }
                foreach (var item in vendors)
                    dbd.Vendors.Remove(item);
                foreach (var item in resourceMapping)
                    dbd.EventResourceMapping.Remove(item);
                foreach (var item in staffMapping)
                    dbd.StaffEventMapping.Remove(item);
                foreach (var item in budget)
                    dbd.Budgets.Remove(item);
                foreach (var item in checkList)
                    dbd.CheckLists.Remove(item);
                foreach (var item in listItems)
                    dbd.CheckListItems.Remove(item);
                //get event invoive and delete the payments and items
                foreach (var item in invoice)
                {
                    var payments = dbd.InvoicePayments.Where(n => n.InvoiceId == item.InvoiceId);
                    var invoiceItems = dbd.InvoiceItems.Where(n => n.InvoiceId == item.InvoiceId);

                    foreach (var itemss in payments)
                        dbd.InvoicePayments.Remove(itemss);
                    foreach (var itemss in invoiceItems)
                        dbd.InvoiceItems.Remove(itemss);
                    dbd.Invoices.Remove(item);
                }
                foreach (var item in geustList)
                    dbd.GuestLists.Remove(item);
                foreach (var item in guests)
                    dbd.Guests.Remove(item);
                foreach (var item in appointment)
                    dbd.Appointments.Remove(item);
                foreach (var item in vendorMapping)
                    dbd.EventVendorMappings.Remove(item);
                foreach (var item in notes)
                    dbd.Notes.Remove(item);
                foreach (var item in tasks)
                    dbd.Tasks.Remove(item);
                dbd.Event.Remove(items);
            }
            foreach (var item in subscription)
                dbd.SubscriptionInvoices.Remove(item);
            foreach (var item in resources)
                dbd.Resources.Remove(item);
            foreach (var item in contacts)
                dbd.Contacts.Remove(item);

            foreach (var item in settings)
                dbd.EventPlannerPackageSettings.Remove(item);
            foreach (var item in reviews)
                dbd.EventPlannerReviews.Remove(item);
            foreach (var item in enquiry)
                dbd.EventPlannerEnquiries.Remove(item);
            if (user != null)
                dbd.AppUsers.Remove(user);

            dbd.EventPlanners.Remove(eventPlanner);
            dbd.SaveChanges();

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