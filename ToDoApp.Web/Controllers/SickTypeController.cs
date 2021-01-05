using ToDoApp.BusinessDomain;
using ToDoApp.BusinessDomain.Interfaces;
using ToDoApp.BusinessDomain.Models;
using ToDoApp.Web.Filters;
using ToDoApp.Web.Helpers;
using ToDoApp.Web.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ToDoApp.Web.Controllers
{
    [ValidateUser]
    public class SickTypeController : BaseReportController
    {
        private readonly ISickType _sickTypeRepository;

        public SickTypeController()
        {
            _sickTypeRepository = RTUnityMapper.GetInstance<ISickType>();
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
            var result = _sickTypeRepository.GetAllSickType().Where(r => r.CompanyId == _userInfo.CompanyId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Save(SickType json)
        {
            json.CompanyId = _userInfo.CompanyId;
            var response = _sickTypeRepository.Create(json);
            if (response.Success)
            {
                return Json(new { response.Success, Message = "Saved successfully" });
            }
            return Json(new { response.Success, Message = "Saved failed" });
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var response = _sickTypeRepository.DeleteSickType(id);
            if (response.Success)
            {
                return Json(new { response.Success, Message = "Delete successfully" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { response.Success, Message = "Delete failed" }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetSickTypeDetails(int id)
        {
            var result = _sickTypeRepository.GetSickTypeById(id).FirstOrDefault();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public void ExportToExcel(GridSettings grid)
        {
            var listOfData = _sickTypeRepository.GetAllSickType().Where(r=> r.CompanyId ==_userInfo.CompanyId).AsQueryable();
            var listOfFilteredRequest = FilterHelper.JQGridFilter(listOfData, grid).AsQueryable();
            ExportToExcelAsFormated(listOfFilteredRequest.ToList(), "SickTypeReport_" + DateTime.Now.TimeOfDay, "Sick Type");
        }
    }
}
