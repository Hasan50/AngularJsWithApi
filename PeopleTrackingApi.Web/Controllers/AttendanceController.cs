using PeopleTrackingApi.BusinessDomain;
using PeopleTrackingApi.BusinessDomain.Interfaces;
using PeopleTrackingApi.Common;
using PeopleTrackingApi.Web.Filters;
using PeopleTrackingApi.Web.Services;
using System;
using System.Linq;
using System.Web.Mvc;

namespace PeopleTrackingApi.Web.Controllers
{
    [ValidateUser]
    public class AttendanceController : BaseReportController
    {
        private readonly IAttendance _attendance;

        public AttendanceController()
        {
            _attendance = RTUnityMapper.GetInstance<IAttendance>();
        }
        [HttpGet]
        public JsonResult GetCompanyAgentAttendanceFeed(string companyAgentId, string date)
        {
            var result = _attendance.GetAttendanceCompanyAgentFeed(companyAgentId, Convert.ToDateTime(date).ToZoneTimeBD());
            return Json(new
            {
                EmployeeList = result.Where(r=>r.CheckInTime.HasValue),
                StatusCount = new
                {
                    TotalEmployee = result.Count,
                    TotalCheckIn = result.Count(x => x.CheckInTime.HasValue),
                    TotalCheckOut = result.Count(x => x.CheckOutTime.HasValue),
                    TotalNotAttend = result.Count(x => !x.CheckInTime.HasValue)
                }
            },JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetCompanyAgentAttendanceOfflineFeed(string companyAgentId, string date)
        {
            var result = _attendance.GetAttendanceCompanyAgentFeed(companyAgentId, Convert.ToDateTime(date).ToZoneTimeBD());
            return Json(new
            {
                EmployeeList = result,
                StatusCount = new
                {
                    TotalEmployee = result.Count,
                    TotalCheckIn = result.Count(x => x.CheckInTime.HasValue),
                    TotalCheckOut = result.Count(x => x.CheckOutTime.HasValue),
                    TotalNotAttend = result.Count(x => !x.CheckInTime.HasValue)
                }
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetMovementDetails(string userId)
        {
           var result= _attendance.GetMovementDetails(userId, DateTime.UtcNow).OrderBy(x => x.LogDateTime);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetLocationBarData(string userId,string date)
        {
            var result = _attendance.GetMovementDetails(userId, Convert.ToDateTime(date)).OrderBy(x => x.LogDateTime).ToList();
            if (!result.Any())
                return Json(result, JsonRequestBehavior.AllowGet);
            return Json(new LocationBarBuilder().Build(result), JsonRequestBehavior.AllowGet);
        }
    }
}
