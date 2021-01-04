using PeopleTrackingApi.Common;
using PeopleTrackingApi.Web.Models;
using System.Web.Http;
using PeopleTrackingApi.BusinessDomain;
using PeopleTrackingApi.BusinessDomain.Interfaces;

namespace PeopleTrackingApi.Web.Controllers.Api
{
    public class TokenApiController : ApiController
    {
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult CreateToken([FromBody] LoginModel model)
        {
            var password = CryptographyHelper.CreateMD5Hash(model.Password);
            var user = RTUnityMapper.GetInstance<IUserCredential>().Get(model.LoginID, password);
            if (user == null)
            {
                return BadRequest();
            }
            var token = TokenManager.GenerateToken(model.LoginID,user.Id);
            return Ok(new { Success = true, Token = TokenManager.GenerateToken(model.LoginID,user.Id), UserKey = user.DoctorId });
        }
    }
}
