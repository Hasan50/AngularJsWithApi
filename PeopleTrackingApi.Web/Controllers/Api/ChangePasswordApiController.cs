using PeopleTrackingApi.Common;
using PeopleTrackingApi.Web.Models;
using System.Web.Http;
using PeopleTrackingApi.BusinessDomain;
using PeopleTrackingApi.BusinessDomain.Interfaces;

namespace PeopleTrackingApi.Web.Controllers.Api
{
    public class ChangePasswordApiController : BaseApiController
    {

        [HttpPost]
        public IHttpActionResult Post(LocalPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var response = RTUnityMapper.GetInstance<IUserCredential>().ChangePassword(model.UserName, CryptographyHelper.CreateMD5Hash(model.ConfirmPassword));
                return Ok(response);
            }
            return Ok(new { Success = false, Message = "Oops!try again." });
        }
    }
}
