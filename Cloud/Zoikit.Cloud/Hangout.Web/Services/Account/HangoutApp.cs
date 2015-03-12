using Hangout.Web.Core.Accounts;
using Hangout.Web.Services.Objects.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Services.Account
{
    public class HangoutApp
    {
        

        public AppAuthenticationTag Register(string username, string email, string password)
        {
            AppAuthenticationTag status = new AppAuthenticationTag();
            status.Status=  Core.Accounts.HangoutApp.Register(username,email,password);
            if (status.Status == AccountStatus.AccountCreated || status.Status == AccountStatus.AlreadyRegistered || status.Status == AccountStatus.Updated)
            {
              

                //get the zat
                status.Tag = Core.Accounts.User.GetZAT(username);
            }

            if (status.Status == AccountStatus.AccountCreated)
            {
                Core.Mail.Mail.SendRegistrationMail(username,email);
            }

            

            return status;
        }

        public AppAuthenticationTag Register(Guid userId, string username, string email, string password)
        {
            AppAuthenticationTag status = new AppAuthenticationTag();
            status.Status = Core.Accounts.HangoutApp.Register(userId, username, email, password);
            if (status.Status == AccountStatus.AccountCreated || status.Status == AccountStatus.AlreadyRegistered || status.Status == AccountStatus.Updated)
            {
                //get the zat
                status.Tag = Core.Accounts.User.GetZAT(username);
            }

            return status;
        }

        public AppAuthenticationTag Login(string username, string password)
        {
            AppAuthenticationTag status = new AppAuthenticationTag();

            status.Status = Core.Accounts.HangoutApp.Login(username, password);
            if (status.Status == AccountStatus.LoggedIn)
            {
                //get the zat
                status.Tag = Core.Accounts.User.GetZAT(username);
            }

            return status;
        }

        public bool ConfirmEmail(Guid userId, string code, string zat)
        {
            try
            {
                if (Core.Accounts.User.IsValid(zat))
                {
                    return Core.Accounts.HangoutApp.ConfirmEmail(userId, code);
                }
                throw new UnauthorizedAccessException();
            }
            catch
            {
                //report :)

                return false;
            }

        }

        public bool ChangePassword(Guid userId, string oldPass, string newPass)
        {
            return Core.Accounts.HangoutApp.ChangePassword(userId, oldPass, newPass);
        }


        public bool ResetPassword(string email)
        {
            return Core.Accounts.HangoutApp.ResetPassword(email);
        }
      

    }
}