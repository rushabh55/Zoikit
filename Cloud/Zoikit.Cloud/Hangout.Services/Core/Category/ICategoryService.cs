using Hangout.Web.Core;
using Hangout.Web.Core.Follow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Hangout.Services.Core.Category
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICategoryService" in both code and config file together.
    [ServiceContract]
    public interface ICategoryService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetCategoriesFollowed?UserID={userId}&ZAT={zat}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<Web.Services.Objects.Category.UserCategory> GetCategoriesFollowed(Guid userId, string zat);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetCategoryIDFollowed?UserID={userId}&ZAT={zat}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<Guid> GetCategoryIDFollowed(Guid userId, string zat);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "FollowCategory?UserID={userId}&ZAT={zat}&CategoryID={categoryId}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        FollowResult FollowCategory(Guid userId,Guid categoryId, string zat);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "UnfollowCategory?UserID={userId}&ZAT={zat}&CategoryID={categoryId}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        FollowResult UnfollowCategory(Guid userId,Guid categoryId, string zat);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetAllCategories?UserID={userId}&ZAT={zat}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<Web.Services.Objects.Category.UserCategory> GetAllCategories(Guid userId,string zat);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetCategoriesFollowedByUser?MeID={meId}&ZAT={zat}&UserID={userId}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<Web.Services.Objects.Category.UserCategory> GetCategoriesFollowedByUser(Guid meId, Guid userId, string zat);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetCategoryByID?UserID={userId}&ZAT={zat}&CategoryID={categoryId}", BodyStyle = WebMessageBodyStyle.Wrapped)]
        Hangout.Web.Services.Objects.Category.UserCategory GetCategoyrByID(Guid userId, Guid categoryId, string zat);

    }
}
