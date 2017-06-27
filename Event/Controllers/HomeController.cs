using System;
using System.Linq;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.AuthenticationManagement;

namespace MyEventPlan.Controllers
{
    public class HomeController : Controller
    {
        private readonly EventDataContext _databaseConnection = new EventDataContext();


        public ActionResult Index()
        {
            ViewBag.VendorServiceId = new SelectList(_databaseConnection.VendorServices, "VendorServiceId", "ServiceName");
            ViewBag.LocationId = new SelectList(_databaseConnection.Locations, "LocationId", "Name");
            return View();
        }

        [SessionExpire]
        public ActionResult Dashboard()
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;


            ViewBag.upComingvents = _databaseConnection.Event
                .Where(n => n.EventPlannerId == loggedinuser.EventPlannerId && n.EventDate > DateTime.Now)
                .OrderByDescending(n => n.EventDate).ToList();
            ViewBag.checkList = _databaseConnection.PersonalCheckLists.Where(n => n.AppUserId == loggedinuser.AppUserId);

            //recent event details
            ViewBag.events = _databaseConnection.Event.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId).ToList();
            ViewBag.prospects = _databaseConnection.Prospects.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId).ToList();
            ViewBag.contacts = _databaseConnection.Contacts.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId).ToList();
            ViewBag.resources = _databaseConnection.Resources.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId).ToList();
            ViewBag.appointments = _databaseConnection.Appointments.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId)
                .ToList();
            ViewBag.invoice = _databaseConnection.Invoices.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId).ToList();
            ViewBag.staff = _databaseConnection.Staff.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId).ToList();
            ViewBag.vendors = _databaseConnection.Vendors.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId).ToList();
            ViewBag.vendors = _databaseConnection.Vendors.ToList();
            ViewBag.planners = _databaseConnection.EventPlanners.ToList();
            ViewBag.vendorpackage = _databaseConnection.VendorPackages.ToList();
            ViewBag.plannerpackage = _databaseConnection.EventPlannerPackages.ToList();
            return View();
        }

        [SessionExpire]
        public ActionResult ViewProfile()
        {
            return View();
        }

        public ActionResult Pricing()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

       
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}