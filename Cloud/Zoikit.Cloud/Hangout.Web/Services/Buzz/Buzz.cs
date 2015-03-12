using Hangout.Web.Core;
using Hangout.Web.Core.Follow;
using Hangout.Web.Core.Buzz;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;

namespace Hangout.Web.Services.Buzz
{
    public class Buzz
    {

        public Web.Services.Objects.Buzz.Buzz GetLastBuzz(Guid userId,Guid meId,string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(meId,zat))
                {

                    Web.Model.Buzz hangout = Web.Core.Buzz.Buzz.GetLastBuzz(userId);

                    if (hangout == null)
                    {
                        return null;
                    }

                    Web.Model.UserProfile user = Web.Core.Accounts.User.GetUserProfile(hangout.UserID);

                    if(hangout.ScreenshotLocationID!=null&&hangout.ScreenshotLocationID!=new Guid())
                    {
                        Services.Objects.Locations.Location loc = Services.Location.Location.ConvertLocation(Core.Location.Location.GetLocation(hangout.ScreenshotLocationID));
                        return new Services.Objects.Buzz.Buzz { Text = hangout.Text, BuzzID = hangout.BuzzID, User = new Objects.Accounts.CompactUser { UserID = hangout.UserID, Name = user.Name, ProfilePicURL = user.ProfilePicURL }, Posted = hangout.DateTimeStamp, City=Services.Location.Location.ConvertCity(Core.Location.City.GetCityById(hangout.CityID)) }; //a light service object returned. 
                    }

                    return new Services.Objects.Buzz.Buzz { Text = hangout.Text, BuzzID = hangout.BuzzID, User = new Objects.Accounts.CompactUser { UserID = hangout.UserID, Name = user.Name, ProfilePicURL = user.ProfilePicURL }, Posted = hangout.DateTimeStamp, City = Services.Location.Location.ConvertCity(Core.Location.City.GetCityById(hangout.CityID)) }; //a light service object returned. 
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


        public List<Services.Objects.Buzz.Buzz> GetUserLatestBuzz(Guid userId, int take, List<Guid> skipList)
        {
            try
            {

                List<Web.Model.BuzzByUser> hangouts = Web.Core.Buzz.Buzz.GetUserLatestBuzzs(userId, take, skipList);
                return hangouts.Select(o => new Services.Objects.Buzz.Buzz { Text = o.Text, BuzzID = o.BuzzID, User = new Objects.Accounts.CompactUser { UserID = o.UserID }, Posted = o.DateTimeStamp }).ToList();

            }
            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return null;
            }
        }

        public Objects.Buzz.BuzzData GetBuzz(Guid buzzId, string accesstoken)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(accesstoken))
                {
                    Web.Model.BuzzByID data = Web.Core.Buzz.Buzz.GetBuzzByID(buzzId);

                    return new Objects.Buzz.BuzzData { Buzz = new Objects.Buzz.Buzz { Posted = data.DateTimeStamp, BuzzID = data.BuzzID, Text = data.Text, User = new Objects.Accounts.CompactUser { UserID = data.UserID } } };
                }
                else { throw new Web.Core.Exceptions.Account.UnAuthorisedAccess(); }
            }
            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(Guid.Empty, Web.Core.ClientType.WindowsAzure, ex);
                return null;
            }
        }

        public List<Services.Objects.Accounts.User> GetUsersInBuzz(Guid userId,int take,List<Guid> skipList, Guid buzzId, string accesstoken)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId,accesstoken))
                {
                    Services.Users.User obj = new Users.User();

                    List<Guid> usersId = Web.Core.Buzz.Follow.GetUsersWhoLikesBuzz(buzzId,take,skipList).ToList();


                    List<Model.User> users = Core.Accounts.User.GetUsersById(usersId);

                    return obj.Convert(userId, users);

                }
                else { throw new Web.Core.Exceptions.Account.UnAuthorisedAccess(); }
            }
            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(Guid.Empty, Web.Core.ClientType.WindowsAzure, ex);
                return null;
            }
        }





       

       

        public List<Services.Objects.Buzz.Buzz> GetUserBuzz(Guid userId, List<Guid> skipList, int take,string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(zat))
                {

                    List<Guid> buzz = Web.Core.Buzz.Buzz.GetUserBuzzInteract(userId,take,skipList);

                    return Convert(userId, buzz);
                }
                else
                {
                    throw new Web.Core.Exceptions.Account.UnAuthorisedAccess();
                }


            }
            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return null;
            }
        }

        public List<Services.Objects.Buzz.Buzz> GetLatestBuzz(Guid userId, int take, List<Guid> skipList, string accessTag)
        {
            try
            {

                if (Web.Core.Accounts.User.IsValid(userId, accessTag))
                {
                    return Convert(userId,Web.Core.Buzz.Buzz.GetUserLatestBuzzs(userId, take, skipList));
                }
                else { throw new Web.Core.Exceptions.Account.UnAuthorisedAccess(); }
            }
            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return null;

            }
        }

       

        public List<Web.Services.Objects.Buzz.Buzz> GetBuzz(Guid userId, int take,List<Guid> skipList, double? lat, double? lon, Guid? cityId,string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId,zat))
                {
                    return Convert(userId, Core.Buzz.Buzz.GetBuzz(userId, take,skipList, lat,lon, cityId));
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

        public List<Web.Services.Objects.Buzz.Buzz> GetBuzzBefore(Guid buzzId, Guid userId, double? lat, double? lon, Guid? cityId, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId, zat))
                {
                    return Convert(userId, Core.Buzz.Buzz.GetBuzzBefore(buzzId,userId,lat,lon,cityId));
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

        public List<Web.Services.Objects.Buzz.Buzz> GetBuzzAfter(Guid buzzId, Guid userId, int take, Guid venueId, Guid cityId, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId, zat))
                {
                    return Convert(userId, Core.Buzz.Buzz.GetBuzzAfter(buzzId, userId,take, venueId, cityId));
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

        public List<Web.Services.Objects.Buzz.Buzz> GetBuzzFollowed(Guid userId,int take,List<Guid> skipList, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId,zat))
                {
                    List<Guid> ids = GetBuzzIDFollowed(userId,take,skipList, zat);
                    List<Web.Services.Objects.Buzz.Buzz> buzz = new List<Web.Services.Objects.Buzz.Buzz>();

                    foreach (Guid id in ids)
                    {
                        Model.BuzzByID hangout = Core.Buzz.Buzz.GetBuzzByID(id);
                        Web.Services.Objects.Buzz.Buzz bz = new Objects.Buzz.Buzz();
                        bz = Convert(userId,hangout);



                        buzz.Add(bz);

                    }


                    return buzz;
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

       


        public List<Web.Services.Objects.Buzz.Buzz> GetBuzzFollowed(Guid meId,Guid userId,int take, List<Guid> skipList, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(zat))
                {
                    List<Guid> ids = GetBuzzIDFollowed(userId,take,skipList,zat);
                    List<Web.Services.Objects.Buzz.Buzz> buzz = new List<Web.Services.Objects.Buzz.Buzz>();

                    foreach (Guid id in ids)
                    {
                        Model.BuzzByID hangout = Core.Buzz.Buzz.GetBuzzByID(id);
                        

                        Objects.Buzz.Buzz obj = new Objects.Buzz.Buzz();

                        obj.BuzzID = hangout.BuzzID;

                        if (obj.City != null)
                        {

                            obj.City = Services.Location.Location.ConvertCity(Core.Location.City.GetCityById(hangout.CityID));
                        }

                        obj.Posted = hangout.DateTimeStamp;

                        if (meId != null)
                        {
                            obj.Liked = (bool)Core.Buzz.Follow.Liked(meId, hangout.BuzzID);
                        }
                        else
                        {
                            obj.Liked = false;
                        }

                        obj.LikeCount = Core.Buzz.Follow.GetNoOfLikes(hangout.BuzzID);

                        obj.Posted = hangout.DateTimeStamp;

                       

                        obj.Text = hangout.Text;

                        Services.Users.User userObj = new Users.User();

                        obj.User = userObj.Convert(Core.Accounts.User.GetUserData(hangout.UserID));

                        Services.Location.Place venueObj = new Location.Place();
                        
                        buzz.Add(obj);

                    }


                    return buzz;
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

       
        public List<Guid> GetBuzzIDFollowed(Guid userId,int take, List<Guid> skipList, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId,zat))
                {
                    return Core.Buzz.Follow.GetBuzzIdLiked(userId,take,skipList);
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

        public Web.Core.Follow.FollowResult FollowBuzz(Guid userId, Guid buzzId, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(zat))
                {
                    Web.Core.Follow.FollowResult res=Core.Buzz.Follow.LikeBuzz(userId, buzzId);
                    if (res == FollowResult.Following)
                    {
                        Core.Cloud.Queue.AddMessage("BuzzFollow:" + buzzId + ":User:" + userId);
                        //Core.Cloud.Queue.AddMessage("Trophy:" + userId);
                        //Guid x = Core.Buzz.Buzz.GetBuzzByID(buzzId).UserID;
                        //Core.Cloud.Queue.AddMessage("Trophy:" + x);
                        //Core.Score.UserScore.IncreaseUserScore(userId, 10);
                        //Core.Score.UserScore.IncreaseUserScore(x, 20);
                    }

                    return res;

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

        public Web.Core.Follow.FollowResult UnfollowBuzz(Guid userId, Guid buzzId, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId,zat))
                {
                     Web.Core.Follow.FollowResult res= Core.Buzz.Follow.UnLikeBuzz(userId, buzzId);
                     if (res == FollowResult.Unfollowed)
                     {
                         Core.Cloud.Queue.AddMessage("BuzzUnfollow:" + buzzId + ":User:" + userId);
                         //Core.Cloud.Queue.AddMessage("Trophy:" + userId);
                         //Guid x = Core.Buzz.Buzz.GetBuzzByID(buzzId).UserID;
                         //Core.Cloud.Queue.AddMessage("Trophy:" + x);
                         //Core.Score.UserScore.IncreaseUserScore(userId, 10);
                         //Core.Score.UserScore.IncreaseUserScore(x, 20);
                     }

                     return res;

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

        public  List<Objects.Buzz.Buzz> Convert(Guid userId,List<Model.BuzzByID> hangouts)
        {

            List<Objects.Buzz.Buzz> list = new List<Objects.Buzz.Buzz>();
            if (hangouts != null)
            {
                foreach (Model.BuzzByID t in hangouts)
                {
                    list.Add(Convert(userId, t));

                }
            }


            return list;

        }

        public List<Objects.Buzz.Buzz> Convert(Guid userId, List<Model.Buzz> hangouts)
        {

            List<Objects.Buzz.Buzz> list = new List<Objects.Buzz.Buzz>();
            if (hangouts != null)
            {
                foreach (Model.Buzz t in hangouts)
                {
                    list.Add(Convert(userId, t));

                }
            }


            return list;

        }


        public List<Objects.Buzz.Buzz> Convert(Guid userId, List<Model.BuzzByUser> hangouts)
        {

            List<Objects.Buzz.Buzz> list = new List<Objects.Buzz.Buzz>();
            if (hangouts != null)
            {
                foreach (Model.BuzzByUser t in hangouts)
                {
                    list.Add(Convert(userId, t));

                }
            }


            return list;

        }


        public  Objects.Buzz.Buzz Convert(Guid userId,Model.BuzzByID hangout)
        {
            if (hangout != null)
            {

                Objects.Buzz.Buzz obj = new Objects.Buzz.Buzz();

                obj.BuzzID = hangout.BuzzID;

                obj.City = Services.Location.Location.ConvertCity(Core.Location.City.GetCityById(hangout.CityID));

                obj.Posted = hangout.DateTimeStamp;

                if (userId != null)
                {
                    obj.Liked = (bool)Core.Buzz.Follow.Liked(userId, hangout.BuzzID);
                }
                else
                {
                    obj.Liked = false;

                }

               

                obj.LikeCount = Core.Buzz.Follow.GetNoOfLikes(hangout.BuzzID);

                obj.Posted = hangout.DateTimeStamp;

                Model.AmplifyBuzz amp = Core.Buzz.Amplify.GetAmplification(userId, hangout.BuzzID);

               if(amp!=null)
               {
                   if(amp.Amplify)
                   {
                       obj.Amplified = true;
                   }
                   else
                   {
                       obj.Deamplified = true;
                   }
               }

                obj.Text = hangout.Text;

                Services.Users.User userObj = new Users.User();

                obj.User = userObj.Convert(Core.Accounts.User.GetUserData(hangout.UserID));
                if(hangout.ScreenshotLocationID!=null)
                {
                    obj.Location = Location.Location.ConvertLocation(Core.Location.Location.GetLocation(hangout.ScreenshotLocationID));

                }

                obj.AmplifyCount = hangout.AmplifyCount;
                obj.CommentCount = hangout.CommentCount;
                obj.BuzzComments = ConvertBuzzComment(userId, Core.Buzz.Buzz.GetBuzzComments(userId,hangout.BuzzID,3,null));
                
                if(hangout.ScreenshotLocationID!=new Guid()&&hangout.ScreenshotLocationID!=null)
                {
                    Model.Location loc = Core.Location.UserLocation.GetLastLocation(userId);
                    Model.Location buzzLocation = Core.Location.Location.GetLocation(hangout.ScreenshotLocationID);
                    
                    obj.Distance = Core.Location.Distance.CalculateDistance(loc.Latitude, loc.Longitude, buzzLocation.Latitude, buzzLocation.Longitude);
                   
                }
                else
                {
                    obj.Distance = -1;
                }

                obj.ImageURL = hangout.ImageURL;
                



                
                
                return obj;
            }
            return null;


            

        }

        public Objects.Buzz.Buzz Convert(Guid userId, Model.Buzz hangout)
        {
            if (hangout != null)
            {

                Objects.Buzz.Buzz obj = new Objects.Buzz.Buzz();

                obj.BuzzID = hangout.BuzzID;

                obj.City = Services.Location.Location.ConvertCity(Core.Location.City.GetCityById(hangout.CityID));

                obj.Posted = hangout.DateTimeStamp;

                if (userId != null)
                {
                    obj.Liked = (bool)Core.Buzz.Follow.Liked(userId, hangout.BuzzID);
                }
                else
                {
                    obj.Liked = false;

                }



                obj.LikeCount = hangout.LikeCount;

                obj.Posted = hangout.DateTimeStamp;

                Model.AmplifyBuzz amp = Core.Buzz.Amplify.GetAmplification(userId, hangout.BuzzID);

                if (amp != null)
                {
                    if (amp.Amplify)
                    {
                        obj.Amplified = true;
                    }
                    else
                    {
                        obj.Deamplified = true;
                    }
                }

                obj.Text = hangout.Text;

                Services.Users.User userObj = new Users.User();

                obj.User = userObj.Convert(Core.Accounts.User.GetUserProfile(hangout.UserID));
                if (hangout.ScreenshotLocationID != null)
                {
                    obj.Location = Location.Location.ConvertLocation(Core.Location.Location.GetLocation(hangout.ScreenshotLocationID));

                }

                obj.AmplifyCount = hangout.AmplifyCount;
                obj.CommentCount = hangout.CommentCount;
                obj.BuzzComments = ConvertBuzzComment(userId, Core.Buzz.Buzz.GetBuzzComments(userId, hangout.BuzzID, 3, null));

                if (hangout.ScreenshotLocationID != new Guid() && hangout.ScreenshotLocationID != null)
                {
                    Model.Location loc = Core.Location.UserLocation.GetLastLocation(userId);
                    Model.Location buzzLocation = Core.Location.Location.GetLocation(hangout.ScreenshotLocationID,hangout.CityID);

                    obj.Distance = Core.Location.Distance.CalculateDistance(loc.Latitude, loc.Longitude, buzzLocation.Latitude, buzzLocation.Longitude);

                }
                else
                {
                    obj.Distance = -1;
                }

                obj.ImageURL = hangout.ImageURL;






                return obj;
            }
            return null;




        }

        public Objects.Buzz.Buzz Convert(Guid userId, Model.BuzzByUser hangout)
        {
            if (hangout != null)
            {

                Objects.Buzz.Buzz obj = new Objects.Buzz.Buzz();

                obj.BuzzID = hangout.BuzzID;

                obj.City = Services.Location.Location.ConvertCity(Core.Location.City.GetCityById(hangout.CityID));

                obj.Posted = hangout.DateTimeStamp;

                if (userId != null)
                {
                    obj.Liked = (bool)Core.Buzz.Follow.Liked(userId, hangout.BuzzID);
                }
                else
                {
                    obj.Liked = false;

                }

                

                obj.LikeCount = Core.Buzz.Follow.GetNoOfLikes(hangout.BuzzID);

                obj.Posted = hangout.DateTimeStamp;

                Model.AmplifyBuzz amp = Core.Buzz.Amplify.GetAmplification(userId, hangout.BuzzID);

                if (amp != null)
                {
                    if (amp.Amplify)
                    {
                        obj.Amplified = true;
                    }
                    else
                    {
                        obj.Deamplified = true;
                    }
                }

                obj.Text = hangout.Text;

                Services.Users.User userObj = new Users.User();

                obj.User = userObj.Convert(Core.Accounts.User.GetUserData(hangout.UserID));
                if (hangout.ScreenshotLocationID != null)
                {
                    obj.Location = Location.Location.ConvertLocation(Core.Location.Location.GetLocation(hangout.ScreenshotLocationID));

                }

                obj.AmplifyCount = hangout.AmplifyCount;
                obj.CommentCount = hangout.CommentCount;
                obj.BuzzComments = ConvertBuzzComment(userId, Core.Buzz.Buzz.GetBuzzComments(userId, hangout.BuzzID, 3, null));

                if (hangout.ScreenshotLocationID != new Guid() && hangout.ScreenshotLocationID != null)
                {
                    Model.Location loc = Core.Location.UserLocation.GetLastLocation(userId);
                    Model.Location buzzLocation = Core.Location.Location.GetLocation(hangout.ScreenshotLocationID);

                    obj.Distance = Core.Location.Distance.CalculateDistance(loc.Latitude, loc.Longitude, buzzLocation.Latitude, buzzLocation.Longitude);

                }
                else
                {
                    obj.Distance = -1;
                }

                obj.ImageURL = hangout.ImageURL;






                return obj;
            }
            return null;




        }



        public List<Objects.Buzz.Buzz> GetLocalBuzzByCategory(Guid userId, Guid categoryId, int take, List<Guid> skipList, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(zat))
                {
                    return Convert(userId, Core.Buzz.Buzz.GetLocalBuzzByCategory(userId, categoryId, take, skipList));
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







        public Objects.Buzz.Buzz GetBuzzByID(Guid userId, Guid buzzId, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId,zat))
                {
                    return Convert(userId, Core.Buzz.Buzz.GetBuzzByID(buzzId));
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

        public List<Objects.Buzz.BuzzComment> GetBuzzComments(Guid userId, Guid buzzId, int take, List<Guid> skipList, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId,zat))
                {
                    return ConvertBuzzComment(userId, Core.Buzz.Buzz.GetBuzzComments(userId, buzzId,take,skipList));
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

        private List<Objects.Buzz.BuzzComment> ConvertBuzzComment(Guid userId, List<Model.BuzzComment> list)
        {
            List<Objects.Buzz.BuzzComment> newList = new List<Objects.Buzz.BuzzComment>();
            Services.Users.User x=new Users.User(); //conversion obj. 
            foreach (Model.BuzzComment comment in list)
            {
                Objects.Buzz.BuzzComment obj = new Objects.Buzz.BuzzComment();
                obj.Comment = comment.CommentText;
                obj.DatePosted = comment.DateTimeStamp;
                obj.CommentID = comment.BuzzCommentID;
                obj.User = x.Convert(comment.UserID);


                newList.Add(obj);
            }

            return newList;
        }

      

        public List<Objects.Buzz.BuzzComment> AddBuzzComment(Guid userId, Guid buzzId, string comment,Guid lastCommentId, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId,zat))
                {

                   List<Objects.Buzz.BuzzComment> comments= ConvertBuzzComment(userId,Core.Buzz.Buzz.AddBuzzComment(userId, buzzId,comment,lastCommentId));

                    //List<Model.BuzzComment> comList=new List<Model.BuzzComment>();
                    //comList.Add(com);

                    //List<Objects.Buzz.BuzzComment> res = ConvertBuzzComment(userId,comList);

                    //if (res.Count > 0)
                    //{
                    //    Core.Buzz.Follow.LikeBuzz(userId, buzzId); //follow the buzz :)
                    //}

                    //Core.Buzz.Buzz.PushBuzzCommentUpdate(buzzId);

                    
                    Core.Cloud.Queue.AddMessage("BuzzComment:" + buzzId + ":User:" + userId + ":Comment:" + comment); //send notifications to user. :)

                    return comments;
                    //return res.FirstOrDefault();
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

        public Core.Buzz.BuzzSaveStatus InsertBuzz(Guid userId, string text, double? lat, double? lon,  Guid? cityId, string imageUri, string accesstoken)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId, accesstoken))
                {
                    BuzzTag res= Web.Core.Buzz.Buzz.InsertBuzz(userId, text,lat,lon,cityId,imageUri);
                    if (res != null)
                    {
                        if (res.SaveStatus == BuzzSaveStatus.Saved)
                        {
                            Core.Cloud.Queue.AddMessage("BuzzAdded:" + res.BuzzID + ":User:" + userId);
                        }
                    }

                    return res.SaveStatus;

                    
                }
                else { throw new Web.Core.Exceptions.Account.UnAuthorisedAccess(); }
            }
            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return BuzzSaveStatus.Error;
            }


        }

        public BuzzSaveStatus UpdateBuzz(Guid userId, Guid buzzId, string text, double? lat, double? lon,  Guid? cityId, string imageUri,string accesstoken)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId, accesstoken))
                {
                    BuzzSaveStatus res =  Web.Core.Buzz.Buzz.UpdateBuzz(buzzId, text, lat, lon ,cityId,imageUri);


                    if (res == BuzzSaveStatus.Saved)
                    {
                        //Notify Users
                        Core.Cloud.Queue.AddMessage("Buzz:" + buzzId + ":User:" + userId); //send notifications to user. :)
                    }


                    return res;

                    //Web.Core.Score.UserScore.IncreaseUserScore(userId, 8); //Increase the user score to 8
                    //Web.Core.Buzz.Buzz.JoinBuzz(userId, hangout.buzzId, clientDateTime); //join in hangout :)
                    //Web.Core.Buzz.Buzz.AddBuzzComment(userId, hangout.buzzId, " created this Buzz", clientDateTime);//add a hangout comment
                    //Web.Core.Tags.Tags.IncreaseTagScore(Web.Core.Text.TextManipulation.GetAllTags(text), 6, userId); //increase token score by 6
                    //send a notification to those compatible
                    //Core.Cloud.Queue.AddCloudBuzzInsertedMessage(userId, hangout.buzzId);
                }
                else { throw new Web.Core.Exceptions.Account.UnAuthorisedAccess(); }
            }
            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return BuzzSaveStatus.Error;
            }
        }

        public List<Objects.Buzz.Buzz> SearchBuzz(Guid userId, string searchtext, int take, List<Guid> skipList, double? lat, double? lon, Guid? cityId, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId, zat))
                {
                    return Convert(userId, Core.Buzz.Buzz.SearchBuzz(userId, searchtext,take, skipList, lat,lon,cityId));
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


        public  int GetAmplifyCount(Guid buzzId,Guid userId, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId, zat))
                {
                    return Web.Core.Buzz.Amplify.GetAmplifyCount(buzzId);
                }

                return -1;
            }
            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return -1;
            }
        }

        public  int GetDeAmplifyCount(Guid buzzId, Guid userId, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId, zat))
                {
                    return Web.Core.Buzz.Amplify.GetDeAmplifyCount(buzzId);
                }

                return -1;
            }
            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return -1;
            }

        
        }

        public  AmplifyStatus UndoAmplification(Guid buzzId, Guid userId, bool amplify,string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId, zat))
                {
                    AmplifyStatus status= Web.Core.Buzz.Amplify.Undo(buzzId, userId, amplify);

                    if(status==AmplifyStatus.AmplificationUndoed)
                    {
                        Core.Cloud.Queue.AddMessage("AmplificationUndo:" + buzzId + ":" + userId);
                    }

                    if(status==AmplifyStatus.DeAmplificationUndoed)
                    {
                        Core.Cloud.Queue.AddMessage("DeAmplificationUndo:" + buzzId + ":" + userId);
                    }

                }

                return AmplifyStatus.InvalidZAT;
            }
           
            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return AmplifyStatus.InvalidZAT; 
            }
        }

        public  AmplifyStatus Amplify(Guid buzzId, Guid userId, bool amplify,string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId, zat))
                {
                   AmplifyStatus status=  Web.Core.Buzz.Amplify.AmplifyBuzz(buzzId, userId, amplify);
                    if(status==AmplifyStatus.Amplified)
                    {
                        Core.Cloud.Queue.AddMessage("Amplified:" + buzzId+":"+userId);
                    }
                    if(status==AmplifyStatus.DeAmplified)
                    {
                        Core.Cloud.Queue.AddMessage("DeAmplified:" + buzzId+":"+userId);
                    }
                    if(status==AmplifyStatus.AmplificationUndoed)
                    {
                        Core.Cloud.Queue.AddMessage("AmplificationUndo:" + buzzId + ":" + userId);
                    }

                    if (status == AmplifyStatus.DeAmplificationUndoed)
                    {
                        Core.Cloud.Queue.AddMessage("DeAmplificationUndo:" + buzzId + ":" + userId);
                    }

                }

                return AmplifyStatus.InvalidZAT;
            }

            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return AmplifyStatus.InvalidZAT;
            }




        }

        public  List<Objects.Accounts.User> GetUsersWhoAmplify(Guid buzzId, int take, List<Guid> skipList, bool amplify,Guid userid, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userid, zat))
                {

                    Services.Users.User obj = new Users.User();

                    List<Guid> usersId = Web.Core.Buzz.Amplify.GetUsersWhoAmplify(buzzId,take,skipList, amplify);


                    List<Model.User> users = Core.Accounts.User.GetUsersById(usersId);

                    return obj.Convert(userid, users);

                }

                return null;
            }

            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userid, Web.Core.ClientType.WindowsAzure, ex);
                return null;
            }
        }


        public string UploadImage(Guid userid, string zat, byte[] image)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userid, zat))
                {
                    return Core.Buzz.Buzz.SaveBuzzImage(image);
                }

                return "";
            }

            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userid, Web.Core.ClientType.WindowsAzure, ex);
                return "";
            }
        }

       

        public List<Objects.Buzz.BuzzComment> GetBuzzCommentsBefore(Guid userId, Guid buzzId, Guid lastCommentId, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId, zat))
                {
                    return ConvertBuzzComment(userId,Core.Buzz.Buzz.GetBuzzCommentsBefore(buzzId, lastCommentId));
                }

                return null;
            }

            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return null;
            }
        }

        public List<Objects.Buzz.Buzz> GetLocalBuzzByTag(Guid userId, Guid tagId, Guid cityId, int take, List<Guid> skipList, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId, zat))
                {

                    return Convert(userId, Core.Tags.Follow.GetLocalBuzzByTag(userId, tagId, take,cityId, skipList)).OrderByDescending(o=>o.Posted).ToList();
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

        private List<Objects.Buzz.Buzz> Convert(Guid userId, List<Guid> hangouts)
        {
            List<Objects.Buzz.Buzz> list = new List<Objects.Buzz.Buzz>();
            if (hangouts != null)
            {
                foreach (Guid t in hangouts)
                {
                    list.Add(Convert(userId, Core.Buzz.Buzz.GetBuzzByID(t)));

                }
            }


            return list;
        }

        public bool HasNewBuzz(Guid userId, Guid cityId, Guid lastBuzzId, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId, zat))
                {
                    return Core.Buzz.Buzz.HasNewBuzz(userId, cityId, lastBuzzId, zat);
                }

                else
                {
                    throw new UnauthorizedAccessException();
                }
            }

            catch (System.Exception ex)
            {
                Web.Core.Exceptions.ExceptionReporting.AddAnException(userId, Web.Core.ClientType.WindowsAzure, ex);
                return false ;
            }
        }
    }
}