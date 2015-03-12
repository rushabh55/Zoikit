using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Core.Accounts
{
    public class HangoutApp
    {
        public static AccountStatus Register(string username,string email,string password)
        {
            Core.Cloud.TableStorage.InitializeUsersTable();
            Core.Cloud.TableStorage.InitializeUserProfileTable();
            //check if email already exists
            if (Registered(email))
            {
                return AccountStatus.AlreadyRegistered;
            }

            if (Core.Accounts.User.DoesUserExists(username))
            {
                return AccountStatus.UsernameInvalid;
            }

            Guid userId=Core.Accounts.User.GetIfUserExists(email);

            if (userId == null||userId==Guid.Empty)
            {
                //continue to insert new user 
                Model.User user = new Model.User();
                user.Email = email.ToLower();
                user.EmailConfirmed = false;
                user.EmailConfirmationCode = GetEmailConfrimationCode();
                user.Username = username.ToLower();
                user.FailedLoginCounts = 0;
                user.Blocked = false;
                user.Activated = true;
                user.Password = Crypt.Encrypt(password);
                user.DateTimeStamp = DateTime.Now;
                user.DateTimeUpdated = DateTime.Now;
                user.AccountBlockedByFailedLogin = false;
                user.ZAT = Guid.NewGuid().ToString();

                Model.Table.Users.Execute(TableOperation.Insert(user));

                

                //MAIL USER ABOUT ACCOUNT CREATION

                Model.UserProfile profile = new Model.UserProfile(user.UserID);
                profile.UserID = user.UserID;
                profile.Birthday = new DateTime(1990, 1, 1);
                profile.DateTimeUpdated = DateTime.Now;
                profile.ProfilePicURL = Core.Accounts.User.GetRandomAvatar();
                profile.Name = "";
                profile.DateTimeStamp = DateTime.Now;
                Model.Table.UserProfile.Execute(TableOperation.Insert(profile));

                return AccountStatus.AccountCreated;

            }
            else
            {
                return Register(userId,username, email, password);
            }
        }

        public static AccountStatus Register(Guid userId, string username, string email, string password)
        {

            Model.User user = Core.Accounts.User.GetUser(userId);
            user.Email = email.ToLower();
            user.EmailConfirmed = false;
            user.EmailConfirmationCode = GetEmailConfrimationCode();
            user.Username = username.ToLower();
            user.FailedLoginCounts = 0;
            user.Blocked = false;
            user.Activated = true;
            user.Password = Crypt.Encrypt(password);
            user.DateTimeStamp = DateTime.Now;
            user.DateTimeUpdated = DateTime.Now;
            user.AccountBlockedByFailedLogin = false;
            user.ZAT = Guid.NewGuid().ToString();

            Model.Table.Users.Execute(TableOperation.Replace(user));

            //MAIL USER ABOUT ACCOUNT CREATION



            return AccountStatus.Updated;
        }

        public static AccountStatus Login(string username, string password)
        {

            Core.Cloud.TableStorage.InitializeUsersTable();

            TableQuery<Model.User> userQuery = new TableQuery<Model.User>().Where(TableQuery.CombineFilters(
            TableQuery.GenerateFilterCondition("Username", QueryComparisons.Equal, username),TableOperators.Or,TableQuery.GenerateFilterCondition("Email", QueryComparisons.Equal, username)));



            Model.User user = Model.Table.Users.ExecuteQuery(userQuery).FirstOrDefault(); 

            if (user == null)
            {
                //username invalid 
                return AccountStatus.UsernameInvalid;

            }

            if (Crypt.Encrypt(password) == user.Password)
            {
                //O.K
                user.FailedLoginCounts = 0;
                Model.Table.Users.Execute(TableOperation.Replace(user));
                return AccountStatus.LoggedIn;
            }
            else
            {
                if (user.FailedLoginCounts == 0)
                {
                    user.FailedLoginCounts=1;

                }
                else
                {
                    user.FailedLoginCounts++;
                }

                Model.Table.Users.Execute(TableOperation.Replace(user));

                if (user.FailedLoginCounts >= 5)
                {
                    //MAIL USER - POSSIBLE ACCOUNT HIJACK!
                }

                return AccountStatus.LogInFailed;

            }

        }

        public static bool Registered(string email)
        {

           



            if(GetUserByEmail(email)!=null)
            {
                return true;
            }

            return false;
        }

        private static Model.User GetUserByEmail(string email)
        {

            Core.Cloud.TableStorage.InitializeUsersTable();

            TableQuery<Model.User> userQuery = new TableQuery<Model.User>().Where(
            TableQuery.GenerateFilterCondition("Email", QueryComparisons.Equal, email));



            IEnumerable<Model.User> users = Model.Table.Users.ExecuteQuery(userQuery);

            if (users == null)
            {
                return null;
            }

            else
            {
                return users.FirstOrDefault();
            }

        }

        public static bool Registered(Guid userId)
        {
            Core.Cloud.TableStorage.InitializeUsersTable();

            TableQuery<Model.User> userQuery = new TableQuery<Model.User>().Where(
            TableQuery.CombineFilters(
            TableQuery.CombineFilters(
            TableQuery.CombineFilters(
            TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, Model.User.GetRowKey(userId)), 
            TableOperators.And,
            TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.User.GetPartitionKey(userId))), 
            TableOperators.And,
            TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, userId)), 
            TableOperators.And,
            TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("Username", QueryComparisons.NotEqual, ""), 
            TableOperators.And, 
            TableQuery.GenerateFilterCondition("Password", QueryComparisons.NotEqual, ""))));




            if (Model.Table.Users.ExecuteQuery(userQuery).FirstOrDefault() != null)
            {
                return true;
            }

            return false;
        }


        public static string GetEmailConfrimationCode()
        {
            Random r = new Random();
            return r.Next(1000, 9999).ToString();

        }

        public static bool ConfirmEmail(Guid userId, string code)
        {


            Core.Cloud.TableStorage.InitializeUsersTable();

            TableQuery<Model.User> userQuery = new TableQuery<Model.User>().Where(
            TableQuery.CombineFilters(
            TableQuery.CombineFilters(
            TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Model.User.GetPartitionKey(userId)), TableOperators.And,
            TableQuery.CombineFilters(
            TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, Model.User.GetRowKey(userId)), TableOperators.And,
            TableQuery.GenerateFilterConditionForGuid("UserID", QueryComparisons.Equal, userId))), TableOperators.And, TableQuery.GenerateFilterCondition("EmailConfirmationCode", QueryComparisons.Equal, code)));


            Model.User user=Model.Table.Users.ExecuteQuery(userQuery).FirstOrDefault();
            if ( user == null)
            {
                return false;
            }

            user.EmailConfirmed = true;

            Model.Table.Users.Execute(TableOperation.Replace(user));

            //MAIL USER ABOUT EMAIL CONFIRMATION :)

            return true;
        }


        public static bool ResetPassword(string email)
        {


            if (Registered(email))
            {
                Core.Cloud.TableStorage.InitializeUsersTable();

                TableQuery<Model.User> userQuery = new TableQuery<Model.User>().Where(
            TableQuery.GenerateFilterCondition("Email", QueryComparisons.Equal, email));


                Model.User user = Model.Table.Users.ExecuteQuery(userQuery).FirstOrDefault();

               
                if (user != null)
                {
                    string newPass=GenerateRandomPassword();
                    user.Password = Crypt.Encrypt(newPass);
                    user.FailedLoginCounts = 0;

                    Model.Table.Users.Execute(TableOperation.Replace(user));
                    //MAIL USER NEW PASSWORD!
                    Core.Mail.Mail.MailNewPassword(user.Username, email, newPass);
                    return true;
                }
            }

            return false;
        }

        public static string GenerateRandomPassword()
        {
            return new Random().Next(10000, 99999).ToString();
        }

        public static bool ChangePassword(Guid userId, string oldPass, string newPass)
        {
            Core.Cloud.TableStorage.InitializeUsersTable();

            if (Registered(userId))
            {

                Model.User user = Core.Accounts.User.GetUser(userId);

                if (user != null)
                {
                    if (Crypt.Encrypt(oldPass) == user.Password)
                    {
                        user.Password = Crypt.Encrypt(newPass);
                        user.FailedLoginCounts = 0;
                        

                        //MAIL USER ABOUT PASSWORD CHANGE NOTIFICATION :)

                        Model.Table.Users.Execute(TableOperation.Replace(user));

                        Mail.Mail.SendPasswordChangeMail(user.Username,user.Email);

                        return true;
                    }
                    
                }
            }

            return false;
        }

        

        public static UserData? GetUserData(string username)
        {
            Guid id=Core.Accounts.User.GetuserId(username);
            if(id==null||id==Guid.Empty)
            {
                return null;
            }
            return Core.Accounts.User.GetUserData(id);
        }
        
    }
}