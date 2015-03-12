using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using SendGridMail;
using SendGridMail.Transport;
using System.Net;

namespace Hangout.Web.Core.Mail
{

    public static class Mail
    {

       

        public delegate void SendMailDelegate(string to, string subject, string body);

        public static SendMailDelegate send;

        public static void Send(string to,string subject,string body)
        {
            if (send == null)
                send = new SendMailDelegate(SendMail);

            send.BeginInvoke(to, subject, body,null,null); //creates a new thread and sends it Async

        }

        private static void SendMail(string to, string subject, string body)
        {
            try
            {


               
                // Create credentials, specifying your user name and password.
             

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtpout.secureserver.net");

                mail.From = new MailAddress("911@zoikit.com", "Zoik it! Support");
                mail.To.Add(to);
                mail.Subject = subject;
                mail.Body = body;

                SmtpServer.Port = 25;
                SmtpServer.Credentials = new System.Net.NetworkCredential("911@zoikit.com", "Macmein141!");
                SmtpServer.EnableSsl = false;

                SmtpServer.Send(mail);

            }
            catch (Exception ex)
            {
                Exceptions.ExceptionReporting.AddAnException(Guid.Empty, ClientType.WindowsAzure, ex);

            }
        }


        public static void Send(List<string> to, string subject, string body)
        {
            foreach (String s in to)
            {
                Send(s, subject, body);
            }
        }


      

        internal static void SendRegistrationMail(string username, string email)
        {
            try
            {
                Guid user = Core.Accounts.User.GetIfUserExists(email);

                string code = Core.Accounts.User.GetUser(user).EmailConfirmationCode;

                string link = "http://www.zoikit.com/Pages/ActivateEmail.aspx?user=" + user.ToString() + "&code=" + code;

                string body = MailTexts.RegistrationMailBody.Replace("<USER>", username);

                body = body.Replace("<LINK>", link);

                Send(email, MailTexts.RegistrationMailSubject, body);
            }
            catch
            {

            }


        }

        internal static void MailNewPassword(string username, string email, string newPass)
        {
            string body = MailTexts.ForgotPasswordBody.Replace("<USER>", username).Replace("<PASSWORD>",newPass);

            Send(email, MailTexts.ForgotPasswordSubject, body);
        }

        

        internal static void SendPasswordChangeMail(string username, string email)
        {
            string body = MailTexts.PasswordChangeBody.Replace("<USER>", username);

            Send(email, MailTexts.PasswordChangeSubject, body);
        }
    }
}