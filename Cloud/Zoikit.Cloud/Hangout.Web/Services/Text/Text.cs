using Hangout.Web.Services.Objects.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Services.Text
{
    public class Text
    {
        

        public Objects.Text.UserText GetUserText(Guid fromId, Guid toId, List<Guid> skipList, int take, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(fromId,zat) || Web.Core.Accounts.User.IsValid(toId,zat))
                {
                    List<Model.Text> texts = Core.Text.Text.GetUserText(fromId, toId, skipList, take);

                    UserText txt = new UserText();
                    Services.Users.User usr = new Users.User();
                    txt.User = usr.Convert(Core.Accounts.User.GetUserData(toId));

                    foreach (Model.Text t in texts)
                    {
                        txt.Texts = Convert(texts);
                    }

                    return txt;

                    
                }

                else
                {
                    throw new UnauthorizedAccessException();
                }
            }

            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(fromId, Web.Core.ClientType.WindowsAzure, ex);
                return null;
            }

            

        }

        private List<Objects.Text.Text> Convert(List<Model.Text> texts)
        {
            List<Objects.Text.Text> list = new List<Objects.Text.Text>();

            foreach (Model.Text t in texts)
            {
                Objects.Text.Text a = new Objects.Text.Text();
                a.DateTimeStamp = t.DateTimeStamp;
                a.MarkAsRead = t.MarkAsRead;
                a.TextMessage = t.TextMessage;
                a.TextId = t.TextID;
                Services.Users.User usr = new Users.User();
                a.User = usr.Convert(Core.Accounts.User.GetUserData(t.UserFrom));

                list.Add(a);
            }

            return list;
        }

        private List<Objects.Text.Text> Convert(Guid meId,List<Model.Text> texts)
        {
            List<Objects.Text.Text> list = new List<Objects.Text.Text>();

            foreach (Model.Text t in texts)
            {
                Objects.Text.Text a = new Objects.Text.Text();
                a.DateTimeStamp = t.DateTimeStamp;
                if (meId == t.UserFrom)
                {
                    a.MarkAsRead = true;
                }
                else
                {
                    a.MarkAsRead = t.MarkAsRead;
                }
                a.TextMessage = t.TextMessage;
                a.TextId = t.TextID;
                Services.Users.User usr = new Users.User();
                if (t.UserTo == meId)
                {
                    a.User = usr.Convert(Core.Accounts.User.GetUserData(t.UserFrom));
                }
                else
                {
                    a.User = usr.Convert(Core.Accounts.User.GetUserData(t.UserTo));
                }

                list.Add(a);
            }

            return list;
        }

        public List<Objects.Text.Text> GetText(Guid userId, List<Guid> skipList, int take, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId, zat))
                {
                    return Convert(userId, Core.Text.Text.GetText(userId, skipList, take));
                }
                 else
                {
                    throw new UnauthorizedAccessException();
                }
            }
        

            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return null;
            }

        }

        public Core.Text.TextSentStatus SendText(Guid fromId, Guid toId, string text, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(fromId, zat))
                {
                    Core.Text.TextData data = Core.Text.Text.SendText(fromId, toId, text);
                    if (data.Status == Core.Text.TextSentStatus.Saved)
                    {
                        Core.Text.Text.SendTextNotifications(toId,data.Text);
                    }

                    return data.Status;
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }


            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(fromId, Web.Core.ClientType.WindowsAzure, ex);
                return Core.Text.TextSentStatus.Error;
            }
           
        }

        public Web.Services.Objects.Service.Result MarkAsRead(Guid fromId, Guid toId, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(toId, zat))
                {
                    Core.Text.Text.MarkAsRead(fromId, toId);
                    Core.PushNotifications.PushNotifications.SendNoOfUnreadNotification(toId);
                    Core.Text.Text.SendTextMainNotification(toId);
                    return Objects.Service.Result.OK;
                }
                else
                {
                    return Objects.Service.Result.Error;
                }
            }


            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(fromId, Web.Core.ClientType.WindowsAzure, ex);
                return Objects.Service.Result.Error;
            }
           
        }

        public Web.Services.Objects.Service.Result MarkAllAsRead(Guid userId, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId, zat))
                {
                    Core.Text.Text.MarkAllAsRead(userId);
                    Core.PushNotifications.PushNotifications.SendNoOfUnreadNotification(userId);
                    Core.Text.Text.SendTextMainNotification(userId);
                    return Objects.Service.Result.OK;
                }
                else
                {
                    return Objects.Service.Result.Error;
                }
            }


            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return Objects.Service.Result.Error;
            }
            
        }

       public List<Objects.Text.Text> GetTextsAfter(Guid fromId, Guid toId, Guid textId, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(fromId, zat) || Web.Core.Accounts.User.IsValid(toId, zat))
                {
                    return Convert(Core.Text.Text.GetTextsAfter(fromId, toId, textId));
                }

                else
                {
                    throw new UnauthorizedAccessException();
                }
            }

            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(fromId, Web.Core.ClientType.WindowsAzure, ex);
                return null;
            }
            
        }

       public int GetUnreadMessagesCount(Guid userId, string zat)
       {
           try
           {
               if (Web.Core.Accounts.User.IsValid(userId, zat))
               {
                   return Core.Text.Text.GetNumberOfUnreadMessages(userId);
               }

               else
               {
                   throw new UnauthorizedAccessException();
               }
           }

           catch (System.Exception ex)
           {
               Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
               return 0;
           }

       }
    }
}