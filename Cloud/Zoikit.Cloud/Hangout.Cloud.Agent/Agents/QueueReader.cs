using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using System.Threading.Tasks;


namespace Hangout.Cloud.Agent.Agents
{
    class QueueReader 
    {

        public void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            // initialize the account information

           
            try
            {





                //CloudStorageAccount.((configName, configSetter) =>
                //{
                //    configSetter(RoleEnvironment.GetConfigurationSettingValue(configName)); // Error and error message is "error"

                //    RoleEnvironment.Changed += (sender, arg) =>
                //    {
                //        if (arg.Changes.OfType<RoleEnvironmentConfigurationSettingChange>()
                //          .Any((change) => (change.ConfigurationSettingName == configName)))
                //        {
                //            if (!configSetter(RoleEnvironment.GetConfigurationSettingValue(configName)))
                //            {
                //                RoleEnvironment.RequestRecycle();
                //            }
                //        }
                //    };
                //});


            }
            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(Guid.Empty, Web.Core.ClientType.WindowsAzure, ex);

            }

                var storageAccount = CloudStorageAccount.Parse(Settings.CloudStorageString);

                // retrieve a reference to the messages queue
                var queueClient = storageAccount.CreateCloudQueueClient();
                var queue = queueClient.GetQueueReference("hangoutqueue");

                queue.CreateIfNotExists();
                
           

                

                while (true)
                {
                    try
                    {
                        Trace.WriteLine("QR ENTERED");
                       

                        if (queue.Exists())
                        {  
                            var msg= queue.GetMessage();
                            try
                            {
                                if (msg != null)
                                {

                                    

                                    string text = msg.AsString;


                                    if (text.Contains("BuzzAdded"))
                                    {
                                       
                                        Guid buzzid = new Guid(text.Split(':')[1]);
                                        Guid userId = new Guid(text.Split(':')[3]);
                                        Task.Factory.StartNew(() => Web.Core.Buzz.UserBuzzInteract.AddInteract(buzzid,userId), TaskCreationOptions.None);
                                        Task.Factory.StartNew(() => Web.Core.Buzz.Buzz.IncreaseBuzzCount(userId), TaskCreationOptions.None);
                                        Task.Factory.StartNew(() => Web.Core.Buzz.Buzz.NotifyBuzzAdded(userId, buzzid), TaskCreationOptions.None);
                                    }

                                    if (text.Contains("PeopleNearUser"))
                                    {
                                        Guid userId = new Guid(text.Split(':')[1]);
                                       Task.Factory.StartNew(() =>  Web.Core.Users.Users.NotifyInterestedPeopleNearUser(userId), TaskCreationOptions.None);
                                    }

                                    if (text.Contains("CityChange"))
                                    {
                                       Guid userId = new Guid(text.Split(':')[1]);
                                       Guid oldCityId = new Guid(text.Split(':')[2]);
                                       Guid newCityId = new Guid(text.Split(':')[3]);
                                       Task.Factory.StartNew(() =>  Web.Core.Location.City.ChangeUserCity(userId,oldCityId,newCityId), TaskCreationOptions.None);
                                    }

                                   

                                    if (text.Contains("UserFollow"))
                                    {
                                        Guid userId = new Guid(text.Split(':')[1]);
                                        Guid followUserId = new Guid(text.Split(':')[3]);
                                        Task.Factory.StartNew(() => Web.Core.Users.Follow.IncreaseFollowCount(followUserId), TaskCreationOptions.None);
                                        Task.Factory.StartNew(() => Web.Core.Users.Follow.IncreaseFollowingCount(userId), TaskCreationOptions.None);
                                        Task.Factory.StartNew(() => Web.Core.Users.Follow.NotifyUserFollow(userId, followUserId), TaskCreationOptions.None);
                                    }

                                    if (text.Contains("CategoryFollow"))
                                    {
                                        Guid categoryId = new Guid(text.Split(':')[1]);
                                        Guid userId = new Guid(text.Split(':')[3]);
                                        Task.Factory.StartNew(() => Web.Core.Category.Follow.NotifyCategoryFollow(userId, categoryId), TaskCreationOptions.None);
                                    }

                                    if (text.Contains("PlaceFollow"))
                                    {
                                        Guid venueId = new Guid(text.Split(':')[1]);
                                        Guid userId = new Guid(text.Split(':')[3]);
                                        Task.Factory.StartNew(() => Web.Core.Location.UserPlaceFollow.NotifyPlaceFollow(venueId, userId), TaskCreationOptions.None);
                                    }

                                    if (text.Contains("PlaceCheckIn"))
                                    {
                                        Guid venueId = new Guid(text.Split(':')[1]);
                                        Guid userId = new Guid(text.Split(':')[3]);
                                        Task.Factory.StartNew(() => Web.Core.Location.Place.NotifyPlaceCheckIn(venueId, userId), TaskCreationOptions.None);
                                    }


                                    if (text.Contains("BuzzFollow"))
                                    {
                                        Guid buzzid = new Guid(text.Split(':')[1]);
                                        Guid userId = new Guid(text.Split(':')[3]);
                                        try
                                        {
                                            Task.Factory.StartNew(() => Web.Core.Buzz.UserBuzzInteract.AddInteract(buzzid, userId), TaskCreationOptions.None);
                                            Task.Factory.StartNew(() => Web.Core.Buzz.Follow.IncreaseLikeCount(buzzid), TaskCreationOptions.None);
                                        }
                                        catch { }

                                        try
                                        {
                                            Task.Factory.StartNew(() => Web.Core.Buzz.Follow.NotifyLikeBuzz(userId, buzzid), TaskCreationOptions.None);
                                        }
                                        catch { }
                                    }

                                    if (text.Contains("BuzzUnfollow"))
                                    {
                                        Guid buzzid = new Guid(text.Split(':')[1]);
                                        Guid userId = new Guid(text.Split(':')[3]);
                                        Task.Factory.StartNew(() => Web.Core.Buzz.UserBuzzInteract.RemoveInteract(buzzid, userId), TaskCreationOptions.None);
                                        Task.Factory.StartNew(() => Web.Core.Buzz.Follow.DecreaseLikeCount(buzzid), TaskCreationOptions.None);
                                       
                                    }

                                    

                                    if (text.Contains("TagFollowed"))
                                    {
                                        Guid tagid = new Guid(text.Split(':')[1]);
                                        Guid userId = new Guid(text.Split(':')[3]);
                                       
                                        Task.Factory.StartNew(() => Web.Core.Tags.Follow.NotifyTagFollowed(userId, tagid), TaskCreationOptions.None);
                                    }


                                    if (text.Contains("Amplified"))
                                    {
                                       
                                        Guid buzzId = new Guid(text.Split(':')[1]);
                                        Guid userId = new Guid(text.Split(':')[2]);
                                        try
                                        {
                                            Task.Factory.StartNew(() => Web.Core.Buzz.UserBuzzInteract.AddInteract(buzzId, userId), TaskCreationOptions.None);
                                            Task.Factory.StartNew(() => Web.Core.Buzz.Amplify.NotifyAmplifyBuzz(userId, buzzId), TaskCreationOptions.None);
                                        }
                                        catch
                                        {

                                        }

                                        try { 
                                                     Task.Factory.StartNew(() => Web.Core.Buzz.Amplify.IncreaseAmplificationCount(buzzId), TaskCreationOptions.None);
                                            }
                                        catch
                                        {

                                        }

                                    }


                                    if (text.Contains("DeAmplified"))
                                    {
                                        Guid buzzId = new Guid(text.Split(':')[1]);
                                        Guid userId = new Guid(text.Split(':')[2]);
                                        Task.Factory.StartNew(() => Web.Core.Buzz.UserBuzzInteract.RemoveInteract(buzzId, userId), TaskCreationOptions.None);
                                        Task.Factory.StartNew(() => Web.Core.Buzz.Amplify.IncreaseDeamplificationCount(buzzId), TaskCreationOptions.None);

                                    }


                                    if (text.Contains("DeAmplificationUndo"))
                                    {
                                        Guid buzzId = new Guid(text.Split(':')[1]);
                                        Guid userId = new Guid(text.Split(':')[2]);

                                        Task.Factory.StartNew(() => Web.Core.Buzz.Amplify.ReduceDeamplificationCount(buzzId), TaskCreationOptions.None);

                                    }

                                    if (text.Contains("AmplificationUndo"))
                                    {
                                        Guid buzzId = new Guid(text.Split(':')[1]);
                                        Guid userId = new Guid(text.Split(':')[2]);
                                        Task.Factory.StartNew(() => Web.Core.Buzz.UserBuzzInteract.RemoveInteract(buzzId, userId), TaskCreationOptions.None);
                                        Task.Factory.StartNew(() => Web.Core.Buzz.Amplify.ReduceAmplificationCount(buzzId), TaskCreationOptions.None);

                                    }

                                    if (text.Contains("BuzzComment"))
                                    {
                                        Guid buzzid = new Guid(text.Split(':')[1]);
                                        Guid userId = new Guid(text.Split(':')[3]);
                                        string comment = (text.Split(':')[5]);
                                        Task.Factory.StartNew(() => Web.Core.Buzz.UserBuzzInteract.AddInteract(buzzid, userId), TaskCreationOptions.None);
                                        Task.Factory.StartNew(() => Web.Core.Buzz.Buzz.NotifyBuzzComment(userId, buzzid, comment), TaskCreationOptions.None);
                                    }

                                    if (text.Contains("BuzzUpdated"))
                                    {
                                        Guid buzzid = new Guid(text.Split(':')[1]);
                                        Guid userId = new Guid(text.Split(':')[2]);
                                        Task.Factory.StartNew(() => Web.Core.Buzz.Buzz.NotifyBuzzUpdate(userId, buzzid), TaskCreationOptions.None);
                                    }

                                    if (text.Contains("Trophy"))
                                    {
                                        Guid userId = new Guid(text.Split(':')[1]);
                                        Task.Factory.StartNew(() => Core.Trophy.UpdateTrophies(userId), TaskCreationOptions.None);
                                    }



                                    if (text.Contains("RefreshLikes"))
                                    {
                                        Guid userId = new Guid(text.Split(':')[1]);
                                        Task.Factory.StartNew(() => Web.Core.Tags.FacebookTag.RefreshTags(userId), TaskCreationOptions.None);

                                    }

                                    if (text.Contains("Compatibility"))
                                    {
                                        Guid userId = new Guid(text.Split(':')[1]);
                                        Task.Factory.StartNew(() => Agents.CompatibilityAgent.UpdateCompatibility(userId), TaskCreationOptions.None);//update compatibility
                                    }

                                   

                                    if (text.Contains("HangoutAdded"))
                                    {
                                        Guid hangoutid = new Guid(text.Split(':')[1]);
                                        Guid userId = new Guid(text.Split(':')[3]);
                                        Task.Factory.StartNew(() => Core.Buzz.BuzzAdded(hangoutid, userId), TaskCreationOptions.None);
                                    }

                                    if (text.Contains("FacebookService"))
                                    {
                                        Guid userId = new Guid(text.Split(':')[1]);
                                        Task.Factory.StartNew(() => Core.Facebook.Run(userId), TaskCreationOptions.None);
                                    }

                                    if (text.Contains("TwitterService"))
                                    {
                                        Guid userId = new Guid(text.Split(':')[1]);
                                        Task.Factory.StartNew(() => Core.Twitter.Run(userId), TaskCreationOptions.None);
                                    }

                                    if (text.Contains("FoursquareService"))
                                    {
                                        Guid userId = new Guid(text.Split(':')[1]);
                                        Task.Factory.StartNew(() => Core.Foursquare.Run(userId), TaskCreationOptions.None);
                                    }

                                    if (text.Contains("IncrementCommentCount"))
                                    {
                                        Guid buzzId = new Guid(text.Split(':')[1]);
                                        Task.Factory.StartNew(() => Web.Core.Buzz.Buzz.IncreaseBuzzComment(buzzId), TaskCreationOptions.None);
                                    }

                                    queue.DeleteMessage(msg);


                                }
                                else
                                {
                                    
                                    Thread.Sleep(new TimeSpan(0, 0, 1)); //start after 30 seconds :)
                                }
                            }
                            catch(Exception e)
                            {
                                Web.Core.Exceptions.ExceptionReporting.AddAnException(Guid.Empty, Web.Core.ClientType.WindowsAzure, e);
                                queue.DeleteMessage(msg); //some bad message. Delete :)
                            }
                           
                        }
                        else
                        {
                            
                            Thread.Sleep(new TimeSpan(0, 0, 5)); //start after 30 seconds :)
                        }
                    }
                    catch(Exception ex)
                    {
                        Web.Core.Exceptions.ExceptionReporting.AddAnException(Guid.Empty,Web.Core.ClientType.WindowsAzure,ex);
                    }

                }

            }
           
        }


    }

