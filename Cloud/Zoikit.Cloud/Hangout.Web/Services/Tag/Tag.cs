using Hangout.Web.Core;
using Hangout.Web.Core.Follow;
using Hangout.Web.Services.Objects.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangout.Web.Services.Tag
{
    public class Tag
    {

        

        public List<Web.Services.Objects.Tag.UserTag> GetTagsFollowed(Guid userId,int take,Guid cityId,List<Guid> skipList, string zat)
        {
            List<Guid> ids = GetTagIDFollowed(userId, take,skipList,cityId, zat);
            List<Web.Services.Objects.Tag.UserTag> tokens = new List<Web.Services.Objects.Tag.UserTag>();

            foreach (Guid id in ids)
            {
                tokens.Add(ConvertUserTag(userId, id,cityId));
               
            }


            return tokens;
        }

        public Web.Services.Objects.Tag.UserTag ConvertUserTag(Guid userId,  Guid tagId,Guid cityId)
        {
            Model.Tag token = Core.Tags.Tags.GetTagByID(tagId);
            Web.Services.Objects.Tag.UserTag tkn = new Objects.Tag.UserTag();
            tkn.Tag = Services.Tag.Tag.Convert(token);
            tkn.Following = (bool)Core.Tags.Follow.IsFollowing(userId, tagId,cityId);
            tkn.NoOfLocalPeopleFollowing = Core.Tags.Follow.GetNoOfPeopleFollowingByUserCity(userId, tagId);


            return tkn;
        }

        private List<Web.Services.Objects.Tag.UserTag> ConvertUserTag(Guid userId,Guid cityId, List<Model.Tag> tokens)
        {
            List<Web.Services.Objects.Tag.UserTag> userTags = new List<Objects.Tag.UserTag>();

            foreach (Model.Tag token in tokens)
            {
                userTags.Add(ConvertUserTag(userId, token.TagID,cityId));
            }

            return userTags;
        }

        public List<Guid> GetTagIDFollowed(Guid userId,int take,List<Guid> skipList,Guid cityId, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(zat))
                {
                    return Core.Tags.Follow.GetTagIdFollowing(userId,take,skipList);
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

        public Web.Core.Follow.FollowResult FollowTag(Guid userId, Guid tagId, Guid cityId, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(zat))
                {
                    Web.Core.Follow.FollowResult res= Core.Tags.Follow.FollowTag(userId, tagId,cityId, true);

                    if (res == FollowResult.Following)
                    {
                        Core.Cloud.Queue.AddMessage("TagFollowed:" + tagId + ":User:" + userId);
                       
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

        public Web.Core.Follow.FollowResult UnfollowTag(Guid userId, Guid tagId, Guid cityId, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(zat))
                {
                    Web.Core.Follow.FollowResult res= Core.Tags.Follow.UnFollowTag(userId, tagId,cityId);

                   
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

        public static List<Objects.Tag.Tag> ConvertTag(List<Model.Tag> tokens)
        {

            List<Objects.Tag.Tag> list = new List<Objects.Tag.Tag>();

            foreach (Model.Tag t in tokens)
            {
                Objects.Tag.Tag obj = new Objects.Tag.Tag();
                obj.Id = t.TagID;
                if (t.Name.StartsWith("#"))
                {
                    obj.Name = Core.Tags.Tags.NormalizeTagName(t.Name);
                }
                else
                {
                    obj.Name = "#" + Core.Tags.Tags.NormalizeTagName(t.Name);
                }
                

                list.Add(obj);
                  
            }


            return list;

        }


        public static Objects.Tag.Tag Convert(Model.Tag token)
        {
            if (token == null)
            {
                return null;
            }


                Objects.Tag.Tag obj = new Objects.Tag.Tag();
                obj.Id = token.TagID;
                if (token.Name.StartsWith("#"))
                {
                    obj.Name = Core.Tags.Tags.NormalizeTagName(token.Name);
                }
                else
                {
                    obj.Name = "#" + Core.Tags.Tags.NormalizeTagName(token.Name);
                }
                

                return obj;

        }



        internal static List<Objects.Tag.Tag> Convert(List<Model.Tag> list)
        {
            List<Objects.Tag.Tag> newList = new List<Objects.Tag.Tag>();

            foreach (Model.Tag tok in list)
            {
                newList.Add(Convert(tok));
            }

            return newList;
        }

        internal static List<Objects.Tag.UserTag> Convert(Guid userId,Guid cityId, List<Model.Tag> list)
        {
            List<Objects.Tag.UserTag> newList = new List<Objects.Tag.UserTag>();

            foreach (Model.Tag tok in list)
            {
                UserTag token = new UserTag();
                token.Tag = Convert(tok);
                token.NoOfLocalPeopleFollowing = Core.Tags.Follow.GetNoOfPeopleFollowingByUserCity(userId, tok.TagID);
                token.Following = (bool)Core.Tags.Follow.IsFollowing(userId, tok.TagID,cityId);

                newList.Add(token);
            }

            return newList;
        }

        public List<Web.Services.Objects.Tag.UserTag> GetTagsFollowed(Guid meId,Guid userId, Guid cityId,  int take,List<Guid> skipList, string zat)
        {
            List<Guid> ids = GetTagIDFollowed(userId,take,skipList,cityId,zat);
            List<Web.Services.Objects.Tag.UserTag> tokens = new List<Web.Services.Objects.Tag.UserTag>();

            foreach (Guid id in ids)
            {
                Model.Tag token = Core.Tags.Tags.GetTagByID(id);
                Web.Services.Objects.Tag.UserTag tkn = new Objects.Tag.UserTag();
                tkn.Tag = Services.Tag.Tag.Convert(token);
                tkn.Following = (bool)Core.Tags.Follow.IsFollowing(meId, cityId, id);
                tkn.NoOfLocalPeopleFollowing = Core.Tags.Follow.GetNoOfPeopleFollowingByUserCity(meId, id);
                tokens.Add(tkn);

            }


            return tokens;
        }

        

        

        public Objects.Tag.UserTag GetTagByID(Guid userId, Guid tagId, Guid cityId, string zat)
        {

           
            try
            {
                if (Web.Core.Accounts.User.IsValid(zat))
                {
                    return ConvertUserTag(userId, tagId,cityId);
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

        public List<Objects.Tag.UserTag> GetTagsInBuzz(Guid userId, Guid buzzId, Guid cityId,  string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(zat))
                {
                    return ConvertUserTag(userId,cityId, Core.Tags.Tags.GetTagsInBuzz(buzzId));
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

        public FollowResult FollowTag(Guid userId, string tagname,Guid cityId, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(zat))
                {
                    Guid tagId = Core.Tags.Tags.InsertTagIfNotExists(tagname).TagID;
                    FollowResult res= Core.Tags.Follow.FollowTag(userId, tagId, cityId, true);

                    if (res == FollowResult.Following)
                    {
                        Core.Cloud.Queue.AddMessage("TagFollowed:" + tagId + ":User:" + userId);
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

        public UserTag GetTagByName(Guid userId, string name, Guid cityId, string zat)
        {
            try
            {
                if (Web.Core.Accounts.User.IsValid(userId,zat))
                {
                    UserTag t = new UserTag();
                    Objects.Tag.Tag ta = new Objects.Tag.Tag();
                    ta.Name = name;
                    ta.Id=Core.Tags.Tags.GetTagIdByName(name);

                    t.Tag = ta;

                    t.NoOfLocalPeopleFollowing = Core.Tags.Follow.GetLocalTagFollowCount(ta.Id, cityId);

                    //get user current city

                    Model.Location loc = Core.Location.UserLocation.GetLastLocation(userId);

                    t.Following = (bool)Core.Tags.Follow.IsFollowing(userId, ta.Id, loc.CityID);

                   
                    return t;
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