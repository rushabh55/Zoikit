using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace Hangout.Web.Model
{
    public class User : TableEntity
    {

        public User()
        {
            this.RowKey = Guid.NewGuid().ToString();
            this.PartitionKey = RowKey.Substring(0, 2);
            UserID = new Guid(this.RowKey);
            DateTimeStamp = DateTime.Now;
        }

        public User(string email)
        {
            
            this.RowKey = Guid.NewGuid().ToString();
            this.PartitionKey = RowKey.Substring(0, 2);

            UserID = new Guid(this.RowKey);
            DateTimeStamp = DateTime.Now;
            Email = email;


        }


       

        public Guid UserID { get; set; }

        public DateTime DateTimeStamp { get; set; }
        public DateTime DateTimeUpdated { get; set; }

        public string Username { get; set; }
        public string Email { get; set; }
        public Guid RoleID { get; set; }

        public bool Activated { get; set; }
        public bool Blocked { get; set; }
       

        public string SessionKey { get; set; }
        public string ZAT { get; set; }
        public string Password { get; set; }
        public int FailedLoginCounts { get; set; }
        public bool AccountBlockedByFailedLogin { get; set; }
        public bool EmailConfirmed { get; set; }
        public string EmailConfirmationCode { get; set; }






        internal static string GetPartitionKey(Guid userId)
        {
            return userId.ToString().Substring(0, 2);
        }

        internal static string GetRowKey(Guid userId)
        {
            return userId.ToString();
        }
    }


}