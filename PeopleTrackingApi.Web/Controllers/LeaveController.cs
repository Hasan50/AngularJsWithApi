using PeopleTrackingApi.BusinessDomain;
using PeopleTrackingApi.BusinessDomain.Interfaces;
using PeopleTrackingApi.BusinessDomain.Models;
using PeopleTrackingApi.Web.Filters;
using PeopleTrackingApi.Web.Helpers;
using PeopleTrackingApi.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PeopleTrackingApi.Web.Controllers
{
    [ValidateUser]
    public class LeaveController : BaseReportController
    {
        private readonly IEmployeeLeave _leaveRepository;

        public LeaveController()
        {
            _leaveRepository = RTUnityMapper.GetInstance<IEmployeeLeave>();
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
        private List<EmployeeLeaveModel> getLeaveData(string fromDate, string toDate, string userName)
        {
            if (!string.IsNullOrEmpty(fromDate) || !string.IsNullOrEmpty(toDate))
            {
                return _leaveRepository.GetLeave(_userInfo.CompanyId).Where(r=>r.FromDate.Date >= Convert.ToDateTime(fromDate).Date && r.ToDate.Date <= Convert.ToDateTime(toDate).Date).ToList() ;
            }
            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate) && !string.IsNullOrEmpty(userName))
            {
                return _leaveRepository.GetLeave(_userInfo.CompanyId).Where(r => r.FromDate.Date >= Convert.ToDateTime(fromDate).Date && r.ToDate <= Convert.ToDateTime(toDate).Date && r.EmployeeName.Contains(userName)).ToList();
            }
            if (string.IsNullOrEmpty(fromDate) && string.IsNullOrEmpty(toDate) && !string.IsNullOrEmpty(userName))
            {
                return _leaveRepository.GetLeave(_userInfo.CompanyId).Where(r => r.EmployeeName.Contains(userName)).ToList();
            }
            return _leaveRepository.GetLeave(_userInfo.CompanyId).ToList();
        }
        public JsonResult GetAll(string fromDate, string toDate, string userName)
        {
            var data = getLeaveData(fromDate, toDate, userName);
            var result = data.AsQueryable();
           
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Approved(int id)
        {
            var response = _leaveRepository.Approved(id, _userInfo.Id);
            if (response.Success)
            {
                return Json(new { response.Success, Message = "Saved successfully" });
            }
            return Json(new { response.Success, Message = "Saved failed" });
        }

        [HttpPost]
        public ActionResult Rejected(int id)
        {
            var response = _leaveRepository.Rejected(id);
            if (response.Success)
            {
                return Json(new { response.Success, Message = "Saved successfully" });
            }
            return Json(new { response.Success, Message = "Saved failed" });
        }
        [HttpPost]
        public ActionResult Save(EmployeeLeaveModel json)
        {
            var model = new EmployeeLeaveModel
            {
                EmployeeId = json.EmployeeId,
                FromDate = Convert.ToDateTime(json.LeaveApplyFrom),
                ToDate = Convert.ToDateTime(json.LeaveApplyTo),
                IsHalfDay = json.IsHalfDay,
                LeaveTypeId = json.LeaveTypeId,
                LeaveReason = json.LeaveReason,
                CreatedAt = DateTime.Now.ToString(),
                IsApproved = false,
                IsRejected = false,
                RejectReason = json.RejectReason,
                ApprovedById = null,
                ApprovedAt = null,
                UserId = json.UserId,
                CompanyId = _userInfo.CompanyId
            };
            var response = _leaveRepository.CreateEmployeeLeave(model);
            if (response.Success)
            {
                return Json(new { response.Success, Message = "Saved successfully" });
            }
            return Json(new { response.Success, Message = "Saved failed" });
        }


        [HttpGet]
        public ActionResult GetLeaveDetails(int id)
        {
            var result = _leaveRepository.GetLeaveById(id).FirstOrDefault();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public void ExportToExcel(string fromDate, string toDate, string userName)
        {
            var listOfData = getLeaveData(fromDate, toDate, userName).AsQueryable();
            ExportToExcelAsFormated(listOfData.ToList(), "LeaveReport_" + DateTime.Now.TimeOfDay, "Leave Report");
        }
    }
}
