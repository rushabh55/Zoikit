using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Model
{
    public class AmplifyBuzz : TableEntity
    {
        public AmplifyBuzz()
        {

        }

        public AmplifyBuzz(Guid userId,Guid buzzId, bool amplify)
        {
            this.PartitionKey = buzzId.ToString();
            this.RowKey = userId.ToString();
            BuzzID = buzzId;
            UserID=userId;
            DateTimeStamp = DateTime.Now;
            Amplify = amplify;
           
        }

       
        public Guid UserID { get; set; }
        public  Guid BuzzID { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public  bool Amplify { get; set; }
        
        public static string GetPartitionKey(Guid buzzId)
        {
            return buzzId.ToString();
        }

    
}
}