using PeopleTrackingApi.Common;
using PeopleTrackingApi.Web.Models;
using System.Web.Http;
using PeopleTrackingApi.BusinessDomain;
using PeopleTrackingApi.BusinessDomain.Interfaces;
using System.Linq;

namespace PeopleTrackingApi.Web.Controllers.Api
{
    /// <summary>
    /// this api using jwt authentication.login method return jwt token.after login jwt bearer token required for any api calling
    /// </summary>
    public class AccountApiController : BaseApiController
    {
        private readonly IUserCredential _userCredential;

        public AccountApiController()
        {
            _userCredential = RTUnityMapper.GetInstance<IUserCredential>();
        }

        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult Register([FromBody]BusinessDomainRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var userModel = _userCredential.GetByLoginID(model.PhoneNumber, UserType.Admin);
                if (userModel != null)
                    return BadRequest("This mobile number already exists.");
                var password = CryptographyHelper.CreateMD5Hash(model.Password);
                var response = _userCredential.Save(new Common.Models.UserCredentialModel
                {
                    FullName = model.UserFullName,
                    UserTypeId = (int)UserType.Admin,
                    Email = model.Email,
                    ContactNo = model.PhoneNumber,
                    LoginID = model.PhoneNumber,
                    IsActive = true,
                    Password = password,
                    CompanyName=model.CompanyName
                });

                return Ok(response);
            }

            return BadRequest("Invalid model.");
        }

        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult Login([FromBody]BusinessDomainLoginModel model)
        {
            if (ModelState.IsValid)
            {
                var password = CryptographyHelper.CreateMD5Hash(model.Password);
                var user = _userCredential.Get(model.UserName, password);
                if (user == null)
                    return Ok(new { Success = false, message = "Invalid userid/password" });

                return Ok(new
                {
                    Success = true,
                    Token = TokenManager.GenerateToken(model.UserName,user.Id,user.CompanyId),
                    UserKey = user.Id,
                    UserName = user.LoginID,
                    FullName= user.FullName,
                    IsSuperAdmin= user.UserTypeId == (int)UserType.SuperAdmin,
                    IsAdmin = user.UserTypeId == (int)UserType.Admin,
                    IsEmployee = user.UserTypeId == (int)UserType.User,
                    CompanyId= user.CompanyId
                });
            }

            return BadRequest();
        }

        /// <summary>
        /// the api for admin login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult LoginAdmin([FromBody]BusinessDomainLoginModel model)
        {
            if (ModelState.IsValid)
            {
                var password = CryptographyHelper.CreateMD5Hash(model.Password);
                var user = _userCredential.Get(model.UserName, password);
                if (user == null)
                    return Ok(new { Success = false, message = "Invalid userid/password" });

                if(user.UserTypeId == (int)UserType.User)
                    return Ok(new { Success = false, message = "Invalid userid/password" });

                return Ok(new
                {
                    Success = true,
                    Token = TokenManager.GenerateToken(model.UserName,user.Id,user.CompanyId),
                    UserKey = user.Id,
                    UserName = user.LoginID,
                    FullName= user.FullName,
                    IsAdmin = user.UserTypeId == (int)UserType.Admin,
                    IsEmployee = user.UserTypeId == (int)UserType.User,
                    CompanyId = user.CompanyId
                });
            }


            return BadRequest();
        }

        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult LoginUser([FromBody]BusinessDomainLoginModel model)
        {
            if (ModelState.IsValid)
            {
                var password = CryptographyHelper.CreateMD5Hash(model.Password);
                var user = _userCredential.Get(model.UserName, password);
                if (user == null)
                    return Ok(new { Success = false, message = "Invalid userid/password" });

                return Ok(new
                {
                    Success = true,
                    Token = TokenManager.GenerateToken(model.UserName,user.Id,user.CompanyId),
                    UserKey = user.Id,
                    UserName = user.LoginID,
                    FullName= user.FullName,
                    IsAdmin = user.UserTypeId == (int)UserType.Admin,
                    IsEmployee = user.UserTypeId == (int)UserType.User,
                    CompanyId = user.CompanyId
                });
            }

            return BadRequest();
        }

        [HttpGet]
        public BusinessDomainRegisterModel GetUserClaims(string userKey)
        {
            var dd = _userCredential.GetProfileDetails(userKey);
            BusinessDomainRegisterModel model = new BusinessDomainRegisterModel()
            {
                Id = dd.Id,
                UserName = dd.LoginID,
                PhoneNumber = dd.ContactNo,
                Email = dd.Email,
                Gender = "Male",
                UserFullName = dd.FullName,
                UserType = dd.UserTypeId == (int)UserType.Admin ? "admin" : "user",
                UserTypeId = dd.UserTypeId
            };
            return model;
        }
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult CheckExistPhone(string phoneno)
        {
            var userModel = _userCredential.GetByLoginID(phoneno, UserType.Admin);
            if (userModel != null)
                return BadRequest("This mobile number already exists.");
            return  Ok(new { Success = true});

        }
        [HttpPost]
        public IHttpActionResult UpdateUser([FromBody]BusinessDomainRegisterModel model)
        {
                var userModel = _userCredential.GetByLoginID(model.UserName);
                if (userModel == null)
                    return BadRequest("User not found.");
                var response = _userCredential.Update(new Common.Models.UserCredentialModel
                {
                    Id=model.Id,
                    FullName = model.UserFullName,
                    UserTypeId = model.UserTypeId,
                    Email = model.Email,
                    ContactNo = model.PhoneNumber,
                    IsActive = true,
                    CompanyName = model.CompanyName
                });

                return Ok(response);
        }
        [HttpPost]
        public IHttpActionResult ChangePassword(LocalPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var userdetail = _userCredential.GetProfileDetails(model.UserName);
                var password = CryptographyHelper.CreateMD5Hash(model.OldPassword);
                var user = _userCredential.Get(userdetail.LoginID, password); 
                if (user == null)
                    return Ok(new { Success = false, Message = "Invalid userid/password." });

                var response = _userCredential.ChangePassword(user.Id, CryptographyHelper.CreateMD5Hash(model.ConfirmPassword));
                return Ok(response);
            }
            return Ok(new { Success = false, Message = "Oops!try again." });
        }

    }
}
