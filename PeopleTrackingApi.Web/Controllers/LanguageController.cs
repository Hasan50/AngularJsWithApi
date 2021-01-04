using PeopleTrackingApi.BusinessDomain;
using PeopleTrackingApi.BusinessDomain.Interfaces;
using PeopleTrackingApi.Common;
using PeopleTrackingApi.Common.Models;
using PeopleTrackingApi.Web.Models;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace PeopleTrackingApi.Web.Controllers
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
