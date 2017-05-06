using System;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Web;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.Service.Configuration;

namespace MyEventPlan.Data.Service.EmailService
{
    public class MailerDaemon
    {
        /// <summary>
        ///     This method sends an email containing a username and password to a newly created user
        /// </summary>
        /// <param name="user"></param>
        public void ResetUserPassword(AppUser user)
        {
            var message = new MailMessage();

            message.From = new MailAddress(Config.SupportEmailAddress);
            message.To.Add(user.Email);
            message.Subject = "New Password";
            message.Priority = MailPriority.High;
            message.SubjectEncoding = Encoding.UTF8;
            message.Body = GetEmailBody_NewPasswordCreated(user);
            message.IsBodyHtml = true;
            try
            {
                new SmtpClient().Send(message);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        ///     This contains the html and passed values for the password reset emails
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private static string GetEmailBody_NewPasswordCreated(AppUser user)
        {
            return
                new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplates/ResetPassword.html"))
                    .ReadToEnd()
                    .Replace("FROM", Config.SupportEmailAddress)
                    .Replace("DISPLAYNAME", user.Firstname)
                    .Replace("URL", "http://10.10.15.77/bhuinfo/Account/ResetPassword/" + user.AppUserId);
        }

        /// <summary>
        ///     This method sends emails to the support of the bhuinfo application
        /// </summary>
        /// <param name="senderName"></param>
        /// <param name="senderMessage"></param>
        /// <param name="email"></param>
        public void ContactUs(string senderName, string senderMessage, string email)
        {
            var message = new MailMessage();
            message.From = new MailAddress(email);
            message.To.Add(Config.SupportEmailAddress);
            message.Subject = "New Contact";
            message.Priority = MailPriority.High;
            message.SubjectEncoding = Encoding.UTF8;
            message.Body = GetEmailBody_ContactUs(senderName, senderMessage);
            message.IsBodyHtml = true;
            try
            {
                new SmtpClient().Send(message);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        ///     This method contains the html and passed values for the contact us email
        /// </summary>
        /// <param name="senderName"></param>
        /// <param name="senderMessage"></param>
        /// <returns></returns>
        private static string GetEmailBody_ContactUs(string senderName, string senderMessage)
        {
            return
                new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplates/ContactUs.html"))
                    .ReadToEnd()
                    .Replace("DISPLAYNAME", senderName)
                    .Replace("URL", "http://10.10.15.77/bhuinfo")
                    .Replace("MESSAGE", senderMessage);
        }

        /// <summary>
        ///     This method sends an email containing a username and password to a newly created user
        /// </summary>
        /// <param name="vendor"></param>
        /// <param name="userId"></param>
        public void NewVendor(Vendor vendor,long userId)
        {
            var message = new MailMessage
            {
                From = new MailAddress(Config.SupportEmailAddress),
                Subject = "New Vendor",
                Priority = MailPriority.High,
                SubjectEncoding = Encoding.UTF8,
                Body = GetEmailBody_NewVendorCreated(vendor,userId),
                IsBodyHtml = true
            };
            //message.To.Add(Config.DevEmailAddress);
            message.To.Add(vendor.Email);
            try
            {
                new SmtpClient().Send(message);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        ///     Html page content for the new user email
        /// </summary>
        /// <param name="vendor"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private static string GetEmailBody_NewVendorCreated(Vendor vendor,long userId)
        {
            return
           new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplates/NewVendor.html")).ReadToEnd()
                    .Replace("DISPLAYNAME", vendor.Name)
                    .Replace("AppUserId", userId.ToString());
        }

        /// <summary>
        ///     This method sends an email containing a username and password to a newly created user
        /// </summary>
        /// <param name="eventPlanner"></param>
        /// <param name="userId"></param>
        public void NewEventPlanner(EventPlanner eventPlanner,long userId)
        {
            var message = new MailMessage
            {
                From = new MailAddress(Config.SupportEmailAddress),
                Subject = "New Event Planner",
                Priority = MailPriority.High,
                SubjectEncoding = Encoding.UTF8,
                Body = GetEmailBody_NewEventPlannerCreated(eventPlanner,userId),
                IsBodyHtml = true
            };
            //message.To.Add(Config.DevEmailAddress);
            message.To.Add(eventPlanner.Email);
            try
            {
                new SmtpClient().Send(message);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        ///     Html page content for the new user email
        /// </summary>
        /// <param name="eventPlanner"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private static string GetEmailBody_NewEventPlannerCreated(EventPlanner eventPlanner,long userId)
        {
            return
                new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplates/NewEventPlanner.html")).ReadToEnd()
                         .Replace("DISPLAYNAME", eventPlanner.Firstname + " "+ eventPlanner.Lastname)
                    .Replace("AppUserId", userId.ToString());
        }

        /// <summary>
        ///     This method sends an email containing a username and password to a newly created user
        /// </summary>
        /// <param name="guest"></param>
        /// <param name="eventName"></param>
        public void NewGuest(Guest guest,string eventName)
        {
            var message = new MailMessage
            {
                From = new MailAddress(Config.SupportEmailAddress),
                Subject = "Event Invitation",
                Priority = MailPriority.High,
                SubjectEncoding = Encoding.UTF8,
                Body = GetEmailBody_GuestStatusCreated(guest,eventName),
                IsBodyHtml = true
            };
            //message.To.Add(Config.DevEmailAddress);
            message.To.Add(guest.Email);
            try
            {
                new SmtpClient().Send(message);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        ///     Html page content for the new user email
        /// </summary>
        /// <param name="guest"></param>
        /// <param name="eventName"></param>
        /// <returns></returns>
        private static string GetEmailBody_GuestStatusCreated(Guest guest,string eventName)
        {
            return
                new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplates/GuestStatus.html")).ReadToEnd()
                    .Replace("EventName", eventName)
                    .Replace("GuestId", guest.GuestId.ToString());
        }

        /// <summary>
        ///     This method sends an email containing a username and password to a newly created user
        /// </summary>
        /// <param name="client"></param>
        /// <param name="userId"></param>
        /// <param name="eventName"></param>
        public void NewClientLogin(Client client,long userId, string eventName)
        {
            var message = new MailMessage
            {
                From = new MailAddress(Config.SupportEmailAddress),
                Subject = "Client Login-Access",
                Priority = MailPriority.High,
                SubjectEncoding = Encoding.UTF8,
                Body = GetEmailBody_ClientLoginCreated(userId, eventName),
                IsBodyHtml = true
            };
            //message.To.Add(Config.DevEmailAddress);
            message.To.Add(client.Email);
            try
            {
                new SmtpClient().Send(message);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        ///     Html page content for the new user email
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="eventName"></param>
        /// <returns></returns>
        private static string GetEmailBody_ClientLoginCreated(long userId,string eventName)
        {
            return
                new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplates/ClientLogin.html")).ReadToEnd()
                .Replace("EventName", eventName)
                    .Replace("AppUserId", userId.ToString());
        }
    }
}