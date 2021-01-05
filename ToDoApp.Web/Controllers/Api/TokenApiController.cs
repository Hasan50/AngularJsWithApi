using ToDoApp.Common;
using ToDoApp.Web.Models;
using System.Web.Http;
using ToDoApp.BusinessDomain;
using ToDoApp.BusinessDomain.Interfaces;

namespace ToDoApp.Web.Controllers.Api
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
