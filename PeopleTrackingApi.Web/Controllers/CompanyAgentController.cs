using PeopleTrackingApi.BusinessDomain;
using PeopleTrackingApi.BusinessDomain.Interfaces;
using PeopleTrackingApi.BusinessDomain.Models;
using PeopleTrackingApi.Common.Models;
using PeopleTrackingApi.Web.Filters;
using PeopleTrackingApi.Web.Helpers;
using PeopleTrackingApi.Web.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace PeopleTrackingApi.Web.Controllers
{
    [ValidateUser]
    public class CompanyAgentController : BaseReportController
    {
        private readonly ICompanyAgent _companyAgentRepository;

        public CompanyAgentController()
        {
            _companyAgentRepository = RTUnityMapper.GetInstance<ICompanyAgent>();
        }
        public ActionResult Index()
        {
            return PartialView();
        }
        public ActionResult Create()
        {
            return PartialView();
        }
        [HttpGet]
        public JsonResult GetAll()
        {
            var result = _companyAgentRepository.GetCompanyAgentList(_userInfo.CompanyId).AsQueryable();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Save(CompanyAgent json)
        {
            json.CreatedById = _userInfo.Id;
            json.CompanyId = _userInfo.CompanyId;
            var response = _companyAgentRepository.Create(json);
            if (response.Success)
            {
                return Json(new { response.Success, Message = "Saved successfully" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { response.Success, Message = "Saved failed" }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult UpdateCompanyAgent(CompanyAgent json)
        {
            var response = _companyAgentRepository.Update(json);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetCompanyAgentList()
        {
            var response = _companyAgentRepository.GetCompanyAgentList(_userInfo.CompanyId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetCompanyAgentListWithCompany(int CompanyId)
        {
            var response = _companyAgentRepository.GetCompanyAgentList(CompanyId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
 
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var response = _companyAgentRepository.Delete(id);
            if (response.Success)
            {
                return Json(new ResponseModel { Success = response.Success, Message = "Success" }, JsonRequestBehavior.AllowGet);

            }
            return Json(new ResponseModel { Success = response.Success, Message = "Faild" }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetCompanyDetails(int id)
        {
            var result = _companyAgentRepository.GetCompanyAgentDetails(id).FirstOrDefault();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public void ExportToExcel()
        {
            var listOfData = _companyAgentRepository.GetCompanyAgentList(_userInfo.CompanyId);
            ExportToExcelAsFormated(listOfData.ToList(), "CompanyAgents_" + DateTime.Now.TimeOfDay, "Company Agent");
        }
    }
}
