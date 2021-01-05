using ToDoApp.BusinessDomain;
using ToDoApp.BusinessDomain.Interfaces;
using ToDoApp.Common;
using ToDoApp.Common.Models;
using ToDoApp.Web.Models;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace ToDoApp.Web.Controllers
{
    public class LanguageController : BaseReportController
    {
        private readonly ICompany _company;

        public LanguageController()
        {
            _company = RTUnityMapper.GetInstance<ICompany>();
        }

        [HttpGet]
        public ActionResult SetLanguage(string selectedLanguage)
        {

            var company = _company.GetCompanyListById(_userInfo.CompanyId).FirstOrDefault();
            company.UserLanguage = selectedLanguage;
           var response= _company.Update(company);
            if (response.Success)
            {
                System.Web.HttpContext.Current.Session[Constants.CurrentLanguage] = new UserLanguageSessionModel
                {
                    Language = company.UserLanguage,
                    CompanyId = company.Id,
                    UserId = _userInfo.Id
                };
            }
            
            return Json(selectedLanguage, JsonRequestBehavior.AllowGet);
        }



    }
}
