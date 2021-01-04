using PeopleTrackingApi.BusinessDomain;
using PeopleTrackingApi.BusinessDomain.Interfaces;
using PeopleTrackingApi.Common;
using System.Linq;
using System.Web.Http;

namespace PeopleTrackingApi.Web.Controllers.Api
{
    public class UserMenuApiController : BaseApiController
    {
        private readonly IUserCredential _userCredential;
        private readonly ICompany _company;
        public UserMenuApiController()
        {
            _userCredential = RTUnityMapper.GetInstance<IUserCredential>();
            _company = RTUnityMapper.GetInstance<ICompany>();
        }
        [HttpGet]
        public IHttpActionResult GetMenuList(string lang)
        {
            var userProfile = _userCredential.GetProfileDetails(this.UserId);
            var result = MenuCollection.GetMenu(userProfile.UserTypeId,lang);
            var companyName = "My Application";
            if (this.CompanyId > 0)
            {
                var companyModel = _company.GetCompanyList().FirstOrDefault(x=>x.Id==this.CompanyId);
                if (companyModel != null)
                {
                    companyName = companyModel.CompanyName;
                    if ((int)UserType.Admin== userProfile.UserTypeId)
                    {
                        foreach (var item in result)
                        {
                            if (item.path== "/tasks/relatedToMe" && companyModel.IsShowTaskMenu)
                            {
                                item.isShowMenu = true;
                            }
                            if (item.path == "/leaves/all-leaves" && companyModel.IsShowLeaveMenu)
                            {
                                item.isShowMenu = true;
                            }
                        }
                    }
                }
            }
            

            return Ok(new { MenuList=result,CompanyName=companyName});
        }
    }
}
