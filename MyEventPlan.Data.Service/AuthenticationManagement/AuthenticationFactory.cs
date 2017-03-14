using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Security;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.Encryption;

namespace MyEventPlan.Data.Service.AuthenticationManagement
{
    public class AuthenticationFactory
    {
        private readonly AppUserDataContext _db = new AppUserDataContext();

        /// <summary>
        ///     This ,ethod is used to authenticate a users login
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public AppUser AuthenticateAppUserLogin(string email, string password)
        {
            AppUser user = null;
            user = _db.AppUsers.SingleOrDefault(n => n.Email == email);
            var hashPassword = user != null && new Hashing().ValidatePassword(password,user.Password);
            if (hashPassword)
            {
                return user;
            }
            else
            {
                user = null;
            }
            
            return user;
        }
        /// <summary>
        ///     This method is used to send a forgot password request to fetch the user
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public AppUser ForgotPasswordRequest(string email)
        {
            email = email.Trim();
            var user = _db.AppUsers.SingleOrDefault(m=>m.Email == email);
            if (user != null)
            {
                var appuser = _db.AppUsers.Find(user.AppUserId);
                var newPassword = Membership.GeneratePassword(8, 1);
                appuser.Password = newPassword;
                _db.Entry(appuser).State = EntityState.Modified;
            }
            _db.SaveChanges();
            //new MailerDaemon().ResetUserPassword(appuser);
            return user;
        }

        /// <summary>
        ///     This method is used to reset a user password
        /// </summary>
        /// <param name="newPassword"></param>
        /// <param name="userId"></param>
        public void ResetUserPassword(string newPassword, int userId)
        {
            var user = _db.AppUsers.Find(userId);
            user.Password = newPassword;
            var hashPasword = new Md5Ecryption().ConvertStringToMd5Hash(newPassword);
            _db.Entry(user).State = EntityState.Modified;
            user.Password = hashPasword;
            _db.SaveChanges();
        }

        /// <summary>
        ///     This method generates a password hash from a clear password using MD5
        /// </summary>
        /// <param name="clearPassword">The clear password to be hashed</param>
        /// <returns>The hashed password</returns>
        public string GetPasswordHash(string clearPassword)
        {
            return new Md5Ecryption().ConvertStringToMd5Hash(clearPassword);
        }

        /// <summary>
        ///     This ,ethod enables a user to change their password
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public bool ChangeUserPassword(long userId, string oldPassword, string newPassword)
        {
            var encryptedOldPassword = GetPasswordHash(oldPassword);
            if (encryptedOldPassword == null) throw new ArgumentNullException(nameof(encryptedOldPassword));
            var encryptedNewPassword = GetPasswordHash(newPassword);
            if (encryptedNewPassword == null) throw new ArgumentNullException(nameof(encryptedNewPassword));
            bool isPasswordChanged;
            var user = _db.AppUsers.Find(userId);
            if (user.Password == encryptedOldPassword)
            {
                user.Password = encryptedNewPassword;
                _db.Entry(user).State = EntityState.Modified;
                _db.SaveChanges();
                isPasswordChanged = true;
            }
            else
            {
                isPasswordChanged = false;
            }
            return isPasswordChanged;
        }
    }
}