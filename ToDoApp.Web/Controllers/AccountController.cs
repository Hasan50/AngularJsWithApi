using ToDoApp.BusinessDomain;
using ToDoApp.BusinessDomain.Interfaces;
using ToDoApp.Common;
using ToDoApp.Common.Models;
using ToDoApp.Web.Models;
using System.Web.Mvc;
using System.Web.Security;

namespace ToDoApp.Web.Controllers
{
    public class AccountController : BaseReportController
    {
        private readonly IUserCredential _userCredential;

        public AccountController()
        {
            _userCredential = RTUnityMapper.GetInstance<IUserCredential>();
        }

        [HttpGet]
        public ActionResult Login()
        {

            return View(new LoginModel());
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = GetUser(model);
                if (user == null)
                {
                    ViewData["ErrorTxt"] = "Login was unsuccessful.";
                    return View(model);
                }

                FormsAuthentication.SetAuthCookie(model.LoginID, model.RememberMe);

                System.Web.HttpContext.Current.Session[Constants.CurrentUser] = new UserSessionModel
                {
                    Id = user.Id,
                    UserTypeId = user.UserTypeId,
                    CompanyId = user.CompanyId,
                    FullName = user.FullName,
                    UserInitial = model.LoginID,
                    Email = user.Email,
                    ContactNo = user.ContactNo,
                };
                return RedirectToAction("Index", "Home");
            }

            ViewData["ErrorTxt"] = "Login was unsuccessful.";
            return View(model);
        }

        private UserCredentialModel GetUser(LoginModel objUser)
        {
            var password = CryptographyHelper.CreateMD5Hash(objUser.Password);
            var user = _userCredential.Get(objUser.LoginID, password);
            return user;
        }

        public ActionResult LogOff()
        {
            return RedirectToLogOff();
        }

        public ActionResult RedirectToLogOff()
        {

            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
        [HttpGet]
        public ActionResult GetUserClaims()
        {
            var dd = _userCredential.GetProfileDetails(_userInfo.Id);
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
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdateUser(BusinessDomainRegisterModel json)
        {
            var userModel = _userCredential.GetByLoginID(json.UserName);
            if (userModel == null)
                return Json(new { Success = false, Message = "User does not found" });
            var response = _userCredential.Update(new Common.Models.UserCredentialModel
            {
                Id = json.Id,
                FullName = json.UserFullName,
                UserTypeId = json.UserTypeId,
                Email = json.Email,
                ContactNo = json.PhoneNumber,
                IsActive = true,
                CompanyName = json.CompanyName
            });

            return Json(new { response.Success, Message = "Updated successfully" });
        }
        [HttpPost]
        public ActionResult ChangePassword(LocalPasswordModel json)
        {
            if (ModelState.IsValid)
            {
                var password = CryptographyHelper.CreateMD5Hash(json.OldPassword);
                var user = _userCredential.Get(json.UserName, password);
                if (user == null)
                    return Json(new { Success = false, Message = "Invalid userid/password." });


                var response = _userCredential.ChangePassword(user.Id, CryptographyHelper.CreateMD5Hash(json.ConfirmPassword));
                return Json(new { response.Success, Message = "Updated successfully" });
            }
            return Json(new { Success = false, Message = "Oops!try again." });
        }
        public ActionResult UpdateProfile()
        {
            return PartialView();
        }
        public ActionResult ChangePassword()
        {
            return PartialView();
        }
    }
}
