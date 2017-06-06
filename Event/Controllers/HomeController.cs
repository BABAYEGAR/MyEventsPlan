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
        private readonly EventDataContext _dbEvent = new EventDataContext();

       
        public ActionResult Index()
        {
            ViewBag.VendorServiceId = new SelectList(_dbEvent.VendorServices, "VendorServiceId", "ServiceName");
            ViewBag.LocationId = new SelectList(_dbEvent.Locations, "LocationId", "Name");
            return View();
        }

       
        public ActionResult Dashboard()
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;


            ViewBag.upComingvents = _dbEvent.Event
                .Where(n => n.EventPlannerId == loggedinuser.EventPlannerId && n.EventDate > DateTime.Now)
                .OrderByDescending(n => n.EventDate).ToList();
            ViewBag.checkList = _dbEvent.PersonalCheckLists.Where(n => n.AppUserId == loggedinuser.AppUserId);

            //recent event details
            ViewBag.events = _dbEvent.Event.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId).ToList();
            ViewBag.prospects = _dbEvent.Prospects.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId).ToList();
            ViewBag.contacts = _dbEvent.Contacts.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId).ToList();
            ViewBag.resources = _dbEvent.Resources.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId).ToList();
            ViewBag.appointments = _dbEvent.Appointments.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId)
                .ToList();
            ViewBag.invoice = _dbEvent.Invoices.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId).ToList();
            ViewBag.staff = _dbEvent.Staff.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId).ToList();
            ViewBag.vendors = _dbEvent.Vendors.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId).ToList();
            ViewBag.vendors = _dbEvent.Vendors.ToList();
            ViewBag.planners = _dbEvent.EventPlanners.ToList();
            ViewBag.vendorpackage = _dbEvent.VendorPackages.ToList();
            ViewBag.plannerpackage = _dbEvent.EventPlannerPackages.ToList();
            return View();
        }

       
        public ActionResult ViewProfile()
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