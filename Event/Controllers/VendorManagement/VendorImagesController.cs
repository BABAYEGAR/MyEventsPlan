﻿using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.AuthenticationManagement;
using MyEventPlan.Data.Service.Enum;
using MyEventPlan.Data.Service.FileUploader;

namespace MyEventPlan.Controllers.VendorManagement
{
    public class VendorImagesController : Controller
    {
        private readonly EventDataContext _databaseConnection = new EventDataContext();

        // GET: VendorImages
        [SessionExpire]
        public ActionResult Index()
        {
            var vendorImages = _databaseConnection.VendorImages.Include(v => v.Vendor);
            return View(vendorImages.ToList());
        }

        // GET: VendorImages/Details/5
        [SessionExpire]
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var vendorImage = _databaseConnection.VendorImages.Find(id);
            if (vendorImage == null)
                return HttpNotFound();
            return View(vendorImage);
        }

        // GET: VendorImages/Create
        [SessionExpire]
        public ActionResult Create()
        {
            ViewBag.VendorId = new SelectList(_databaseConnection.Vendors, "VendorId", "Name");
            return View();
        }

        // POST: VendorImages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Create(
            [Bind(Include =
                "VendorImageId,ImageOne,ImageTwo,ImageThree,ImageFour,ImageFive,ImageSix,ImageSeven,ImageEight,ImageNine,ImageTen,VendorId,CreatedBy,DateCreated,DateLastModified,LastModifiedBy")]
            VendorImage vendorImage)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                vendorImage.DateCreated = DateTime.Now;
                vendorImage.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    var imageOne = Request.Files["imageOne"];
                    var imageTwo = Request.Files["imageTwo"];
                    var imageThree = Request.Files["imageThree"];
                    var imageFour = Request.Files["imageFour"];
                    var imageFive = Request.Files["imageFive"];
                    var imageSix = Request.Files["imageSix"];
                    var imageSeven = Request.Files["imageSeven"];
                    var imageEight = Request.Files["imageEight"];
                    var imageNine = Request.Files["imageNine"];
                    var imageTen = Request.Files["imageTen"];
                    if (imageOne != null && imageOne.FileName != "")
                        vendorImage.ImageOne = new FileUploader().UploadFile(imageOne, UploadType.VendorImage);
                    if (imageTwo != null && imageTwo.FileName != "")
                        vendorImage.ImageTwo = new FileUploader().UploadFile(imageTwo, UploadType.VendorImage);
                    if (imageThree != null && imageThree.FileName != "")
                        vendorImage.ImageThree = new FileUploader().UploadFile(imageThree, UploadType.VendorImage);
                    if (imageFour != null && imageFour.FileName != "")
                        vendorImage.ImageFour = new FileUploader().UploadFile(imageFour, UploadType.VendorImage);
                    if (imageFive != null && imageFive.FileName != "")
                        vendorImage.ImageFive = new FileUploader().UploadFile(imageFive, UploadType.VendorImage);
                    if (imageSix != null && imageSix.FileName != "")
                        vendorImage.ImageSix = new FileUploader().UploadFile(imageSix, UploadType.VendorImage);
                    if (imageSeven != null && imageSeven.FileName != "")
                        vendorImage.ImageSeven = new FileUploader().UploadFile(imageSeven, UploadType.VendorImage);
                    if (imageEight != null && imageEight.FileName != "")
                        vendorImage.ImageEight = new FileUploader().UploadFile(imageEight, UploadType.VendorImage);
                    if (imageNine != null && imageNine.FileName != "")
                        vendorImage.ImageNine = new FileUploader().UploadFile(imageNine, UploadType.VendorImage);
                    if (imageTen != null && imageTen.FileName != "")
                        vendorImage.ImageTen = new FileUploader().UploadFile(imageTen, UploadType.VendorImage);

                    vendorImage.LastModifiedBy = loggedinuser.AppUserId;
                    vendorImage.CreatedBy = loggedinuser.AppUserId;
                    vendorImage.VendorId = loggedinuser.VendorId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }

                _databaseConnection.VendorImages.Add(vendorImage);
                _databaseConnection.SaveChanges();
                TempData["display"] = "You have successfully added Image(s)!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Profile", "Vendors");
            }

            ViewBag.VendorId = new SelectList(_databaseConnection.Vendors, "VendorId", "Name", vendorImage.VendorId);
            return View(vendorImage);
        }

        // GET: VendorImages/Edit/5
        [SessionExpire]
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var vendorImage = _databaseConnection.VendorImages.Find(id);
            if (vendorImage == null)
                return HttpNotFound();
            ViewBag.VendorId = new SelectList(_databaseConnection.Vendors, "VendorId", "Name", vendorImage.VendorId);
            return View(vendorImage);
        }

        // POST: VendorImages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Edit(
            [Bind(Include =
                "VendorImageId,ImageOne,ImageTwo,ImageThree,ImageFour,ImageFive,ImageSix,ImageSeven,ImageEight,ImageNine,ImageTen,VendorId,CreatedBy,DateCreated,DateLastModified,LastModifiedBy")]
            VendorImage vendorImage)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                vendorImage.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    var imageOne = Request.Files["imageOne"];
                    var imageTwo = Request.Files["imageTwo"];
                    var imageThree = Request.Files["imageThree"];
                    var imageFour = Request.Files["imageFour"];
                    var imageFive = Request.Files["imageFive"];
                    var imageSix = Request.Files["imageSix"];
                    var imageSeven = Request.Files["imageSeven"];
                    var imageEight = Request.Files["imageEight"];
                    var imageNine = Request.Files["imageNine"];
                    var imageTen = Request.Files["imageTen"];
                    if (imageOne != null && imageOne.FileName != "")
                        vendorImage.ImageOne = new FileUploader().UploadFile(imageOne, UploadType.VendorImage);
                    if (imageTwo != null && imageTwo.FileName != "")
                        vendorImage.ImageTwo = new FileUploader().UploadFile(imageTwo, UploadType.VendorImage);
                    if (imageThree != null && imageThree.FileName != "")
                        vendorImage.ImageThree = new FileUploader().UploadFile(imageThree, UploadType.VendorImage);
                    if (imageFour != null && imageFour.FileName != "")
                        vendorImage.ImageFour = new FileUploader().UploadFile(imageFour, UploadType.VendorImage);
                    if (imageFive != null && imageFive.FileName != "")
                        vendorImage.ImageFive = new FileUploader().UploadFile(imageFive, UploadType.VendorImage);
                    if (imageSix != null && imageSix.FileName != "")
                        vendorImage.ImageSix = new FileUploader().UploadFile(imageSix, UploadType.VendorImage);
                    if (imageSeven != null && imageSeven.FileName != "")
                        vendorImage.ImageSeven = new FileUploader().UploadFile(imageSeven, UploadType.VendorImage);
                    if (imageEight != null && imageEight.FileName != "")
                        vendorImage.ImageEight = new FileUploader().UploadFile(imageEight, UploadType.VendorImage);
                    if (imageNine != null && imageNine.FileName != "")
                        vendorImage.ImageNine = new FileUploader().UploadFile(imageNine, UploadType.VendorImage);
                    if (imageTen != null && imageTen.FileName != "")
                        vendorImage.ImageTen = new FileUploader().UploadFile(imageTen, UploadType.VendorImage);

                    vendorImage.LastModifiedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                _databaseConnection.Entry(vendorImage).State = EntityState.Modified;
                _databaseConnection.SaveChanges();
                return RedirectToAction("Profile", "Vendors");
            }
            ViewBag.VendorId = new SelectList(_databaseConnection.Vendors, "VendorId", "Name", vendorImage.VendorId);
            return View(vendorImage);
        }

        // GET: VendorImages/Delete/5
        [SessionExpire]
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var vendorImage = _databaseConnection.VendorImages.Find(id);
            if (vendorImage == null)
                return HttpNotFound();
            return View(vendorImage);
        }

        // POST: VendorImages/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult DeleteConfirmed(long id)
        {
            var vendorImage = _databaseConnection.VendorImages.Find(id);
            _databaseConnection.VendorImages.Remove(vendorImage);
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