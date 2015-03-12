using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Hangout.Client.Core.User
{
    public class User
    {
        public static bool ValidateUserProfile()
        {
            try
            {


                AccountService.UserData userData = Settings.Settings.UserData;
                if (userData == null)
                {
                    return false;
                }
                if (userData.UserID!=0)
                {


                    if (String.IsNullOrWhiteSpace(userData.Name))
                    {

                        return false;
                    }

                    if (userData.Age==0)
                    {

                        return false;
                    }
                    if (userData.DateTimeStamp==null)
                    {
                        return false;
                    }

                    if (userData.DateTimeUpdated == null)
                    {
                        return false;
                    }


                    if (String.IsNullOrWhiteSpace(userData.DefaultLengthUnits))
                    {

                        return false;
                    }


                    if (String.IsNullOrWhiteSpace(userData.RelationshipStatus))
                    {
                        return false;
                    }

                    if (String.IsNullOrWhiteSpace(userData.ZAT))
                    {
                        return false;
                    }

                    if (userData.Birthday == null)
                    {

                        return false;
                    }

                    if (userData.Gender == null)
                    {
                        return false;
                    }



                    if (String.IsNullOrWhiteSpace(userData.Bio))
                    {

                        return false;
                    }


                   

                    if (String.IsNullOrWhiteSpace(userData.DefaultLengthUnits))
                    {
                        return false;
                    }

                }

                return true;
            }
            catch (Exception ex)
            {
                Core.Exceptions.ExceptionReporting.Report(ex);
                MessageBox.Show(ErrorText.GenericErrorText, ErrorText.GenericErrorCaption, MessageBoxButton.OK);
                return false;
            }
        }

        public static int UserID
        {
            get
            {
                if (Settings.Settings.UserData == null)
                {
                    return - 1;
                }

                return Settings.Settings.UserData.UserID;

            }
        }

        public static string ZAT
        {
            get
            {
                if (Settings.Settings.UserData == null)
                {
                    return "";
                }

                return Settings.Settings.UserData.ZAT;

            }
        }
    }
}
