using Hangout.Web.Core;
using Hangout.Web.Core.Follow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Services.Users
{
    public class User
    {


       

        public Web.Services.Objects.Accounts.User GetUser(Guid userId, Guid getuserId, string zat)
        {
            

            try
            {
                if (Web.Core.Accounts.User.IsValid(userId,zat))
                {
                    return Convert(userId, Core.Accounts.User.GetUser(getuserId));
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

        public List<Web.Services.Objects.Accounts.User> GetUsersFollowed(Guid userId, int take, List<Guid> skipList, string zat)
        {
            List<Guid> ids = GetuserIdFollowed(userId,take,skipList, zat);
            List<Web.Services.Objects.Accounts.User> users = new List<Web.Services.Objects.Accounts.User>();

            foreach (Guid id in ids)
            {
                Web.Services.Objects.Accounts.User user = Convert(userId, Web.Core.Accounts.User.GetUser(id));

                users.Add(user);
            }


            return users;
        }

        public List<Web.Services.Objects.Accounts.User> GetUsersFollowed(Guid meid, Guid userId, int take, List<Guid> skipList, string zat)
        {
            return Convert(meid,Core.Users.Follow.GetFollowingUserList(userId,take,skipList));
        }



        public List<Web.Services.Objects.Accounts.User> GetUsersFollowing(Guid userId, int take, List<Guid> skipList, string zat)
        {
            List<Guid> ids = GetuserIdFollowing(userId,take,skipList, zat);
            List<Web.Services.Objects.Accounts.User> users = new List<Web.Services.Objects.Accounts.User>();

            foreach (Guid id in ids)
            {
                Web.Services.Objects.Accounts.User user = Convert(userId, Web.Core.Accounts.User.GetUser(id));
                users.Add(user);
            }


            return users;
        }

        public List<Web.Services.Objects.Accounts.User> GetUsersFollowing(Guid meid, Guid userId, int take, List<Guid> skipList, string zat)
        {
            return Convert(meid, Core.Users.Follow.GetFollowersUserList(userId,take,skipList));
           
        }

        public List<Guid> GetuserIdFollowed(Guid userId, int take, List<Guid> skipList, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(zat))
                {
                    return Core.Users.Follow.GetFollowingList(userId, take,skipList);
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

        public List<Services.Objects.Accounts.User> GetLocalFollowersByTag(Guid userId, Guid tagId, Guid cityId, int take, List<Guid> skipList, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(zat))
                {
                    
                    return Convert(userId, Core.Tags.Follow.GetLocalFollowersByTag( tagId, cityId,take, skipList));
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

        private List<Objects.Accounts.User> Convert(Guid userId, List<Guid> list)
        {
            List<Objects.Accounts.User> cu = new List<Objects.Accounts.User>();

            foreach (Guid user in list)
            {
                cu.Add(Convert(userId,user));
            }

            return cu;
        }

        private Objects.Accounts.User Convert(Guid userId, Guid profileId)
        {
            Objects.Accounts.User x = new Objects.Accounts.User();
            Model.UserProfile profile = Core.Accounts.User.GetUserProfile(profileId);

            if (profile == null)
            {
                return null;
            }


            x.IsFollowing = (bool)Core.Users.Follow.IsFollowing(userId, profileId);
            //x.CommonItems = Core.Users.Users.GetNoOfCommonItems(userId, user.UserID);

            x.AboutUs = profile.Bio;
            x.Name = profile.Name;
            x.ProfilePicURL = profile.ProfilePicURL;
            x.UserID = profile.UserID;

            return x;
        }


        public List<Guid> GetuserIdFollowing(Guid userId, int take, List<Guid> skipList, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(zat))
                {
                    return Core.Users.Follow.GetFollowersList(userId,take,skipList);
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

        public Web.Core.Follow.FollowResult FollowUser(Guid userId, Guid followuserId, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId,zat))
                {
                    if (userId != followuserId)
                    {
                        Web.Core.Follow.FollowResult res = Core.Users.Follow.FollowUser(userId, followuserId);

                        if (res == FollowResult.Following)
                        {
                            Core.Cloud.Queue.AddMessage("UserFollow:" + userId + ":User:" + followuserId);
                           
                            
                        }

                        return res;
                    }
                    else
                    {
                        return FollowResult.Error;
                    }
                }

                else
                {
                    throw new UnauthorizedAccessException();
                }
            }

            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return FollowResult.Error;
            }
        }

        public Web.Core.Follow.FollowResult UnfollowUser(Guid userId, Guid unFollowuserId, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(zat))
                {
                    if (userId != unFollowuserId)
                    {
                        Web.Core.Follow.FollowResult res = Core.Users.Follow.UnFollowUser(userId, unFollowuserId);

                      
                        if(res==FollowResult.Unfollowed)
                        {
                            Core.Users.Follow.DecreaseFollowCount(unFollowuserId);
                            Core.Users.Follow.DecreaseFollowingCount(userId);
                        }

                        return res;
                    }
                    else
                    {
                        return FollowResult.Error;
                    }

                }

                else
                {
                    throw new UnauthorizedAccessException();
                }
            }

            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return FollowResult.Error;
            }
        }

        public List<Objects.Accounts.User> GetPeopleAroundYou(Guid userId, int count,List<Guid> skipList,string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId,zat))
                {
                    List<Model.User> users = Core.Users.Users.GetPeopleAroundYou(userId, count,skipList);

                    List<Objects.Accounts.User> userList = new List<Objects.Accounts.User>();

                    foreach(Model.User user  in users)
                    {
                        Objects.Accounts.User u = Convert(userId, user);
                        if (u == null)
                        {
                            continue;
                        }
                        userList.Add(u);
                    }

                    return userList;
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

        public Objects.Accounts.User Convert(Guid userId, Model.User user)
        {
            Objects.Accounts.User x = new Objects.Accounts.User();
            Model.UserProfile profile = Core.Accounts.User.GetUserProfile(user.UserID);

            if (profile == null)
            {
                return null;
            }
           

            x.IsFollowing = (bool)Core.Users.Follow.IsFollowing(userId, user.UserID);
            //x.CommonItems = Core.Users.Users.GetNoOfCommonItems(userId, user.UserID);
           
            x.AboutUs = profile.Bio;
            x.Name = profile.Name;
            x.ProfilePicURL = profile.ProfilePicURL;
            x.UserID = profile.UserID;
           
            return x;
        }

        private static List<Objects.Accounts.CompactUser> Convert(List<Model.User> list)
        {
            List<Objects.Accounts.CompactUser> cu = new List<Objects.Accounts.CompactUser>();

            foreach (Model.User user in list)
            {
                cu.Add(Convert(user));
            }

            return cu;
        }

        private static Objects.Accounts.CompactUser Convert(Model.User user)
        {

            if(user==null)
            {
                return null;
            }

          

            Objects.Accounts.CompactUser cu = new Objects.Accounts.CompactUser();
            cu.UserID = user.UserID;
            Model.UserProfile profile = Core.Accounts.User.GetUserProfile(user.UserID);

            if (profile == null)
            {
                return null;
            }
            cu.Name = profile.Name;
            cu.ProfilePicURL = profile.ProfilePicURL;

            return cu;
        }





        internal List<Objects.Accounts.User> Convert(Guid userId, List<Model.User> list)
        {
            List<Objects.Accounts.User> users = new List<Objects.Accounts.User>();

            foreach (Model.User u in list)
            {
                Objects.Accounts.User obj = Convert(userId, u);

                if (obj == null)
                {
                    continue;
                }

                users.Add(obj);
            }

            return users;
        }

        internal Objects.Accounts.CompactUser Convert(Core.Accounts.UserData userData)
        {
            return new Objects.Accounts.CompactUser { Name = userData.Profile.Name, ProfilePicURL = userData.Profile.ProfilePicURL, UserID = userData.User.UserID };
        }

        public List<Objects.Accounts.User> GetLocalUsersByCategoryFollowing(Guid userId, Guid categoryId, int take, List<Guid> skipList, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(zat))
                {
                    return Convert(userId, Core.Users.Users.GetLocalUsersByCategoryFollowing(userId, categoryId, take, skipList));
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



        public List<Objects.Accounts.User> GetPeopleWhoFollowBuzz(Guid userId, Guid buzzId, int take, List<Guid> skipList, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(zat))
                {
                    return Convert(userId, Core.Users.Users.GetPeoplewhoFollowBuzz(buzzId, take, skipList));
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

        internal Objects.Accounts.CompactUser Convert(Guid userId)
        {
            return Convert(Core.Accounts.User.GetUser(userId));
        }

        public List<Objects.Accounts.User> GetPeopleWhoFollowPlace(Guid userId, Guid placeId, int take, List<Guid> skipList, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(zat))
                {
                    return Convert(userId, Core.Users.Users.GetPeoplewhoFollowPlace(placeId, take, skipList));
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

        public List<Objects.Accounts.User> PeopleNearPlace(Guid userId, Guid placeId, int take, List<Guid> skipList, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(zat))
                {
                    return Convert(userId, Core.Users.Users.GetPeopleNearPlace(placeId, take, skipList));
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

        public List<Objects.Accounts.User> GetPeopleCheckedIn(Guid userId, Guid placeId, int take, List<Guid> skipList, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(zat))
                {
                    return Convert(userId, Core.Location.Place.GetPeopleCheckedIn(placeId, take, skipList));
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

        public Web.Services.Objects.User.CompleteUserProfile GetCompleteUserProfile(Guid userId, Guid getprofile, string zat)
        {
           

            try
            {
                if (Web.Core.Accounts.User.IsValid(zat))
                {
                    Web.Services.Objects.User.CompleteUserProfile x = new Objects.User.CompleteUserProfile();

                    Model.User user = Core.Accounts.User.GetUser(getprofile);
                    Model.UserProfile profile = Core.Accounts.User.GetUserProfile(getprofile);

                    if (profile == null)
                    {
                        return null;
                    }

                    Model.Location loc = Core.Location.UserLocation.GetLastLocation(getprofile);
                    x.IsFollowing = (bool)Core.Users.Follow.IsFollowing(userId, getprofile);
                    x.AboutUs = profile.Bio;
                    x.Name = profile.Name;
                    x.ProfilePicURL = profile.ProfilePicURL;
                    x.UserID = profile.UserID;
                    x.BuzzCount = profile.BuzzCount;
                    x.FollowersCount = profile.FollowersCount;
                    x.FollowingCount = profile.FollowingCount;
                   
                    if(loc!=null)
                    {
                        x.City = Services.Location.Location.ConvertCity(Core.Location.City.GetCityById(loc.CityID));
                    }
                       
                  
                    return x;
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

        public Objects.User.Rep GetUserRep(Guid userId, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(zat))
                {


                    Objects.User.Rep rep = new Objects.User.Rep();
                   
                    

                    return rep;
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

        public List<Objects.Accounts.User> SearchPeopleAroundYou(Guid userId,string text, int count, List<Guid> skipList, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId, zat))
                {
                    //List<Model.User> users = Core.Users.Users.SearchPeopleAroundYou(userId,text, count, skipList);
                    List<Model.User> users = new List<Model.User>();


                    List<Objects.Accounts.User> userList = new List<Objects.Accounts.User>();

                    foreach (Model.User user in users)
                    {
                        Objects.Accounts.User u = Convert(userId, user);
                        if (u == null)
                        {
                            continue;
                        }
                        userList.Add(u);
                    }

                    return userList;
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



        internal Objects.Accounts.User ConvertUserByID(Guid userId, Guid id)
        {
            Objects.Accounts.User x = new Objects.Accounts.User();
            Model.UserProfile profile = Core.Accounts.User.GetUserProfile(id);

            if (profile == null)
            {
                return null;
            }


            x.IsFollowing = (bool)Core.Users.Follow.IsFollowing(userId, id);
            //x.CommonItems = Core.Users.Users.GetNoOfCommonItems(userId, user.UserID);

            x.AboutUs = profile.Bio;
            x.Name = profile.Name;
            x.ProfilePicURL = profile.ProfilePicURL;
            x.UserID = profile.UserID;

            return x;
        }

        internal Objects.Accounts.CompactUser Convert(Model.UserProfile userProfile)
        {
            Objects.Accounts.CompactUser user = new Objects.Accounts.CompactUser();
            user.Name = userProfile.Name;
            user.ProfilePicURL = userProfile.ProfilePicURL;
            user.UserID = userProfile.UserID;

            return user;
        }

        public List<Objects.Accounts.User> SearchUsers(Guid userId, string searchtext, int take, List<Guid> skipList, double lat, double lon, Guid cityId, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId, zat))
                {
                    List<Model.User> users = Core.Users.Users.SearchPeopleAroundYou(userId, searchtext, take, skipList,lat,lon,cityId);

                    List<Objects.Accounts.User> userList = new List<Objects.Accounts.User>();

                    foreach (Model.User user in users)
                    {
                        Objects.Accounts.User u = Convert(userId, user);
                        if (u == null)
                        {
                            continue;
                        }
                        userList.Add(u);
                    }

                    return userList;
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
    }
}