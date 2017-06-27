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

namespace MyEventPlan.Controllers.EventManagement
{
    public class ClientsController : Controller
    {
        private readonly EventDataContext _databaseConnection = new EventDataContext();

        // GET: Clients
        [SessionExpire]
        public ActionResult Index()
        {
            var events = Session["event"] as Event.Data.Objects.Entities.Event;
            var clients =
                _databaseConnection.Clients.Where(n => n.EventId == events.EventId).Include(c => c.Event).Include(c => c.EventPlanner);
            return View(clients.ToList());
        }

        // GET: Clients
        [SessionExpire]
        public ActionResult AllMyClients()
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var clients =
                _databaseConnection.Clients.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId).Include(c => c.Event)
                    .Include(c => c.EventPlanner);
            return View(clients.ToList());
        }

        // GET: Clients/Details/5
        [SessionExpire]
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var client = _databaseConnection.Clients.Find(id);
            if (client == null)
                return HttpNotFound();
            return View(client);
        }

        // GET: Clients/Details/5
        [SessionExpire]
        public ActionResult CreateLoginAccessForClient(long? id)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var role = _databaseConnection.Roles.SingleOrDefault(n => n.Name == "Client");
            var events = Session["event"] as Event.Data.Objects.Entities.Event;
            var client = _databaseConnection.Clients.Find(id);
            var appUser = new AppUser();
            appUser.Firstname = client.Name;
            appUser.Lastname = client.Name;
            appUser.Email = client.Email;
            appUser.Mobile = client.Mobile;
            if (role != null) appUser.RoleId = role.RoleId;
            appUser.DateLastModified = DateTime.Now;
            appUser.DateCreated = DateTime.Now;
            appUser.Password = new Hashing().HashPassword("Password");
            appUser.Status = UserAccountStatus.Enabled.ToString();
            if (loggedinuser != null)
            {
                appUser.CreatedBy = loggedinuser.AppUserId;
                appUser.LastModifiedBy = loggedinuser.AppUserId;
                appUser.ClientId = id;
            }
            _databaseConnection.AppUsers.Add(appUser);
            _databaseConnection.SaveChanges();
            if (events != null) new MailerDaemon().NewClientLogin(client, appUser.AppUserId, events.Name);
            TempData["display"] = "The login acces link has been successfully sent to the clients email!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Index", new {id = client.EventId});
        }

        // GET: Clients/Create
        [SessionExpire]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Create([Bind(Include = "ClientId,Name,Password,Email,Mobile")] Client client)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                var events = Session["event"] as Event.Data.Objects.Entities.Event;
                client.DateCreated = DateTime.Now;
                client.DateLastModified = DateTime.Now;
                if (events != null) client.EventId = events.EventId;
                var clientExist = _databaseConnection.Clients
                    .Where(m => m.Email == client.Email && m.EventPlannerId == loggedinuser.EventPlannerId).ToList();
                if (loggedinuser != null)
                {
                    if (clientExist.Count > 0)
                    {
                        TempData["display"] = "A client with the same email exist, try another email!";
                        TempData["notificationtype"] = NotificationType.Error.ToString();
                        return RedirectToAction("Index", new {id = client.EventId});
                    }
                    client.LastModifiedBy = loggedinuser.AppUserId;
                    client.CreatedBy = loggedinuser.AppUserId;
                    client.EventPlannerId = loggedinuser.EventPlannerId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                _databaseConnection.Clients.Add(client);
                _databaseConnection.SaveChanges();
                TempData["display"] = "You have successfully added a new client!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index", new {id = client.EventId});
            }
            return View(client);
        }

        // GET: Clients/Edit/5
        [SessionExpire]
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var client = _databaseConnection.Clients.Find(id);
            if (client == null)
                return HttpNotFound();
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Edit(
            [Bind(Include = "ClientId,Name,Password,Email,Mobile,EventPlannerId,EventId,CreatedBy,DateCreated")] Client
                client)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                client.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    client.LastModifiedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                _databaseConnection.Entry(client).State = EntityState.Modified;
                _databaseConnection.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EventId = new SelectList(_databaseConnection.Event, "EventId", "Name", client.EventId);
            ViewBag.EventPlannerId = new SelectList(_databaseConnection.EventPlanners, "EventPlannerId", "Firstname",
                client.EventPlannerId);
            return View(client);
        }

        // GET: Clients/Delete/5
        [SessionExpire]
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var client = _databaseConnection.Clients.Find(id);
            if (client == null)
                return HttpNotFound();
            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult DeleteConfirmed(long id)
        {
            var client = _databaseConnection.Clients.Find(id);
            _databaseConnection.Clients.Remove(client);
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