﻿using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Event;
using Event.Data.Objects.Entities;
using Event.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.AuthenticationManagement;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        private readonly EventDataContext _databaseConnection = new EventDataContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get => _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            private set => _signInManager = value;
        }

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
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
            return View(packageSubscribed);
        }


        [HttpGet]
        [AllowAnonymous]
     
        public ActionResult VerifyEmail()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        [SessionExpire]
     
        public ActionResult Setting()
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            return View(loggedinuser);
        }

        [HttpGet]
        [AllowAnonymous]

        [SessionExpire]
        public ActionResult UserProfile()
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            return View(loggedinuser);
        }

        [HttpPost]
        [AllowAnonymous]
     
        public ActionResult VerifyEmail(FormCollection collectedValues)
        {
            var email = collectedValues["Email"];
            var user = _databaseConnection.AppUsers.SingleOrDefault(n => n.Email == email);

            Session["user"] = user;
            TempData["display"] = "Your email has been successfully verified. Change your password!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("PasswordReset", "Account");
        }

        [HttpGet]
        [AllowAnonymous]
     
        public ActionResult VerifyAccount(long id)
        {
            var user = _databaseConnection.AppUsers.Find(id);
            _databaseConnection.Entry(user).State = EntityState.Modified;
            _databaseConnection.SaveChanges();
            Session["myeventplanloggedinuser"] = user;
            return RedirectToAction("Dashboard", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
     
        public ActionResult ChangeClientPassword(long? id)
        {
            var user = _databaseConnection.AppUsers.Find(id);
            Session["user"] = user;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
     
        public ActionResult ChangeClientPassword(FormCollection collectedValues)
        {
            var loggedinuser = Session["user"] as AppUser;
            var password = collectedValues["ConfirmPassword"];
            if (loggedinuser != null)
            {
                var user = _databaseConnection.AppUsers.Find(loggedinuser.AppUserId);
                if (user != null)
                {
                    user.Password = new Hashing().HashPassword(password);
                    _databaseConnection.Entry(user).State = EntityState.Modified;
                }
            }
            _databaseConnection.SaveChanges();
            Session["user"] = null;
            TempData["login"] = "You have successfully changed your password!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        [AllowAnonymous]
     
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
     
        public ActionResult ChangePassword(FormCollection collectedValues)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var password = collectedValues["ConfirmPassword"];
            if (loggedinuser != null)
            {
                var user = _databaseConnection.AppUsers.Find(loggedinuser.AppUserId);
                if (user != null)
                {
                    user.Password = new Hashing().HashPassword(password);
                    _databaseConnection.Entry(user).State = EntityState.Modified;
                }
            }
            _databaseConnection.SaveChanges();
            Session["myeventplanloggedinuser"] = loggedinuser;
            TempData["display"] = "You have successfully changed your password!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Setting", "Account");
        }

        [HttpGet]
        [AllowAnonymous]
     
        public ActionResult PasswordReset()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
     
        public ActionResult PasswordReset(FormCollection collectedValues)
        {
            var loggedinuser = Session["user"] as AppUser;

            var reset = new PasswordReset();
            var password = collectedValues["ConfirmPassword"];
            if (loggedinuser != null)
            {
                var user = _databaseConnection.AppUsers.Find(loggedinuser.AppUserId);
                if (user != null)
                {
                    user.Password = new Hashing().HashPassword(password);
                    _databaseConnection.Entry(user).State = EntityState.Modified;
                }
            }
            var generator = new Random();
            var number = Convert.ToInt64(generator.Next(0, 1000000).ToString("D6"));
            reset.Password = password;
            reset.ConfirmPassword = new Hashing().HashPassword(password);
            reset.Date = DateTime.Now;
            reset.Code = number;
            if (loggedinuser != null) reset.Email = loggedinuser.Email;
            //_databaseConnection.PasswordResets.Add(reset);
            _databaseConnection.SaveChanges();
            //_dbc.SaveChanges();

            Session["user"] = null;
            TempData["login"] = "Welcome Back! You have successfully changed your password.. Login to continue!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
     
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
     
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(model);
            var user = new AuthenticationFactory().AuthenticateAppUserLogin(model.Email, model.Password);
            if (user != null)
            {
                if (user.Status != UserAccountStatus.Disabled.ToString())
                {
                    Session["myeventplanloggedinuser"] = user;
                    Session["role"] = user.Role;

                    //package setting
                    var packageSubscribed =
                        _databaseConnection.EventPlannerPackageSettings.SingleOrDefault(
                            n =>
                                n.EventPlannerId == user.EventPlannerId &&
                                n.Status == PackageStatusEnum.Active.ToString());
                    if (packageSubscribed != null)
                        Session["subscribe"] = packageSubscribed;
                    if (user.EventPlannerId != null)
                    {
                        var eventPlanner = _databaseConnection.EventPlanners.Find(user.EventPlannerId);
                        Session["eventplanner"] = eventPlanner;
                        //new Gmail().GetAllGmailContacts("MyeventsPlan", "salxsaa@gmail.com", "Brigada95");
                        return RedirectToAction("Dashboard", "Home");
                    }
                    if (user.VendorId != null)
                    {
                        var vendor = _databaseConnection.Vendors.Find(user.VendorId);
                        Session["vendor"] = vendor;
                        return RedirectToAction("Profile", "Vendors");
                    }
                    return RedirectToAction("Dashboard", "Home");
                }
                TempData["login"] = "Your account has been disabled, Contact the support team";
                TempData["notificationtype"] = NotificationType.Info.ToString();
                return View(model);
            }
            TempData["login"] = "Inavlid email/password, Try again!";
            TempData["notificationtype"] = NotificationType.Error.ToString();
            return View(model);
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
                return View("Error");
            return View(new VerifyCodeViewModel {Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe});
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result =
                await
                    SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, model.RememberMe,
                        model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
     
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser {UserName = model.Email, Email = model.Email};
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, false, false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
                return View("Error");
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
     
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !await UserManager.IsEmailConfirmedAsync(user.Id))
                    return View("ForgotPasswordConfirmation");

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
     
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
     
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code.ToString(), model.Password);
            if (result.Succeeded)
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
     
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
     
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider,
                Url.Action("ExternalLoginCallback", "Account", new {ReturnUrl = returnUrl}));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
                return View("Error");
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions =
                userFactors.Select(purpose => new SelectListItem {Text = purpose, Value = purpose}).ToList();
            return
                View(new SendCodeViewModel {Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe});
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
                return View();

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
                return View("Error");
            return RedirectToAction("VerifyCode",
                new {Provider = model.SelectedProvider, model.ReturnUrl, model.RememberMe});
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
                return RedirectToAction("Login");

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new {ReturnUrl = returnUrl, RememberMe = false});
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation",
                        new ExternalLoginConfirmationViewModel {Email = loginInfo.Email});
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model,
            string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Manage");

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                    return View("ExternalLoginFailure");
                var user = new ApplicationUser {UserName = model.Email, Email = model.Email};
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, false, false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        // POST: /Account/LogOff
        [HttpGet]
        public ActionResult LogOff()
        {
            Session["myeventplanloggedinuser"] = null;
            Session["role"] = null;
            Session["event"] = null;
            Session["vendor"] = null;
            Session["eventplanner"] = null;
            Session["subscribe"] = null;

            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
     
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties {RedirectUri = RedirectUri};
                if (UserId != null)
                    properties.Dictionary[XsrfKey] = UserId;
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        #endregion
    }
}