﻿using System;
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
        public void NewUser(AppUser user)
        {
            var message = new MailMessage
            {
                From = new MailAddress(Config.SupportEmailAddress),
                Subject = "New User Details",
                Priority = MailPriority.High,
                SubjectEncoding = Encoding.UTF8,
                Body = GetEmailBody_NewUserCreated(user),
                IsBodyHtml = true
            };
            //message.To.Add(Config.DevEmailAddress);
            message.To.Add(user.Email);
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
        /// <param name="user"></param>
        /// <returns></returns>
        private static string GetEmailBody_NewUserCreated(AppUser user)
        {
            return
                new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplates/NewUserCreated.html")).ReadToEnd()
                    .Replace("DISPLAYNAME", user.Firstname)
                    .Replace("USERNAME", user.Email)
                    .Replace("PASSWORD", user.Password)
                    .Replace("URL", "http://10.10.15.77/bhuinfo/Account/Login")
                    .Replace("ROLE", user.Firstname)
                    .Replace("FROM", Config.SupportEmailAddress);
        }

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
        public void NewVendor(Vendor vendor)
        {
            var message = new MailMessage
            {
                From = new MailAddress(Config.SupportEmailAddress),
                Subject = "New Vendor",
                Priority = MailPriority.High,
                SubjectEncoding = Encoding.UTF8,
                Body = GetEmailBody_NewVendorCreated(vendor),
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
        /// <returns></returns>
        private static string GetEmailBody_NewVendorCreated(Vendor vendor)
        {
            return
                new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplates/NewUserCreated.html")).ReadToEnd();
        }

        /// <summary>
        ///     This method sends an email containing a username and password to a newly created user
        /// </summary>
        /// <param name="eventPlanner"></param>
        public void NewEventPlanner(EventPlanner eventPlanner)
        {
            var message = new MailMessage
            {
                From = new MailAddress(Config.SupportEmailAddress),
                Subject = "New Event Planner",
                Priority = MailPriority.High,
                SubjectEncoding = Encoding.UTF8,
                Body = GetEmailBody_NewEventPlannerCreated(eventPlanner),
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
        /// <returns></returns>
        private static string GetEmailBody_NewEventPlannerCreated(EventPlanner eventPlanner)
        {
            return
                new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplates/NewUserCreated.html")).ReadToEnd();
        }

        /// <summary>
        ///     This method sends an email containing a username and password to a newly created user
        /// </summary>
        /// <param name="guest"></param>
        public void NewVendor(Guest guest)
        {
            var message = new MailMessage
            {
                From = new MailAddress(Config.SupportEmailAddress),
                Subject = "Event Invitation",
                Priority = MailPriority.High,
                SubjectEncoding = Encoding.UTF8,
                Body = GetEmailBody_GuestStatusCreated(guest),
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
        /// <returns></returns>
        private static string GetEmailBody_GuestStatusCreated(Guest guest)
        {
            return
                new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplates/NewUserCreated.html")).ReadToEnd();
        }

        /// <summary>
        ///     This method sends an email containing a username and password to a newly created user
        /// </summary>
        /// <param name="client"></param>
        public void NewClientLogin(Client client)
        {
            var message = new MailMessage
            {
                From = new MailAddress(Config.SupportEmailAddress),
                Subject = "Client Login-Access",
                Priority = MailPriority.High,
                SubjectEncoding = Encoding.UTF8,
                Body = GetEmailBody_ClientLoginCreated(client),
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
        /// <param name="client"></param>
        /// <returns></returns>
        private static string GetEmailBody_ClientLoginCreated(Client client)
        {
            return
                new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplates/NewUserCreated.html")).ReadToEnd();
        }
    }
}