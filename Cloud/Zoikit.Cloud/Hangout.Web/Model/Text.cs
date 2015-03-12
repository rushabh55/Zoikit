using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;
using System.ServiceModel.Channels;

namespace Hangout.Web.Model
{
    public class Text : TableEntity
    {

        public Text()
        {
        }

        public Text(Guid fromuserId, Guid touserId, string text)
        {

            this.RowKey = (DateTime.MaxValue.Ticks - DateTime.Now.Ticks).ToString();
            TextID = Guid.NewGuid();
            this.PartitionKey = Model.Text.GetPartitionKey(fromuserId, touserId);
            UserFrom = fromuserId;
            UserTo = touserId;
            TextMessage = text;
            MarkAsRead = false;
            DateTimeStamp = DateTime.Now;
        }

        public static string GetPartitionKey(Guid fromuserId, Guid touserId)
        {
            if (String.Compare(fromuserId.ToString(), touserId.ToString()) < 0)
            {
                return fromuserId.ToString() + "_" + touserId.ToString();
            }
            else
            {
                return touserId.ToString()+"_"+fromuserId.ToString();
            }
        }





        public DateTime DateTimeStamp { get; set; }

        public bool MarkAsRead { get; set; }

        public string TextMessage { get; set; }

        public Guid UserTo { get; set; }

        public Guid UserFrom { get; set; }

        public Guid TextID { get; set; }
    }


}