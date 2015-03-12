using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Core.Exceptions
{
    public class ExceptionReporting
    {
        public static void AddAnException(Guid userId,ClientType type,System.Exception e)
        {
            System.Exception temp = e;
            Stack<System.Exception> st = new Stack<Exception>();
            st.Push(e);
            while (temp.InnerException != null)
            {
                st.Push(e.InnerException);
                temp = temp.InnerException;
            }

            Guid LastInserted = Guid.Empty;

            while (st.Count>0)
            {
                string clienttype = "";
                if (type == ClientType.WindowsAzure)
                {
                    clienttype = "Windows Azure";
                }

                if (type == ClientType.WindowsPhone7)
                {
                    clienttype = "Windows Phone 7";
                }

                if (type == ClientType.Windows8)
                {
                    clienttype = "Windows 8";
                }

                if (type == ClientType.WindowsPhone8)
                {
                    clienttype = "Windows Phone 8";
                }



                LastInserted = InsertException(userId, clienttype, st.Pop(), LastInserted);
                
                
            }

        }


        public static  Guid AddAnException(Guid userId, string appId,string message,string stackTrace)
        {

            Core.Cloud.TableStorage.InitializeExceptionsTable();

            if (userId == null || userId == Guid.Empty)
            {
                userId = Guid.Empty;
            }

            Model.Exceptions exception = new Model.Exceptions(userId);
            if (userId == null ||userId==Guid.Empty)
            {
                exception.UserID = Guid.Empty;
            }
            else
            {
                exception.UserID=userId;
            }
            exception.ExceptionMessage = message;
            exception.StackTrace = stackTrace;
            string clienttype = appId;
            exception.DateTimeStamp = DateTime.Now;
            exception.InnerException = Guid.Empty;
            exception.ClientType = clienttype; 

            Model.Table.Exceptions.Execute(TableOperation.Insert(exception));

            return exception.ExceptionID;
        }

        private static Guid InsertException(Guid userId, string clientType, System.Exception e,Guid InnerExceptionId)
        {

            try
            {
                Model.Exceptions exception = new Model.Exceptions();
                if (userId == null||userId ==Guid.Empty)
                {
                    exception.UserID = Guid.Empty;
                }
                else
                {
                    exception.UserID=userId;
                }
                exception.ExceptionMessage = e.Message;
                exception.StackTrace = e.StackTrace;
                exception.ClientType = clientType;
                exception.DateTimeStamp = DateTime.Now;
                if (InnerExceptionId != null)
                {
                    exception.InnerException = InnerExceptionId;
                }

                Model.Table.Exceptions.Execute(TableOperation.Insert(exception));

                return exception.ExceptionID;
            }
            catch
            {
                return Guid.Empty;
            }

        }
    }
}