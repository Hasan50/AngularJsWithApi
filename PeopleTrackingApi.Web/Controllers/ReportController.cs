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
    public class ReportController : BaseReportController
    {
        private readonly IAttendance _attendance;

        public ReportController()
        {
            _attendance = RTUnityMapper.GetInstance<IAttendance>();
        }
        public ActionResult Index()
        {
            return PartialView();
        }
        public ActionResult Create()
        {
            return PartialView();
        }
        public ActionResult CompanyWiseDetails()
        {
            return PartialView();
        }
        [HttpGet]
        public JsonResult GetAll(string fromDate,string toDate,int? companyAgentId)
        {

            var query = GetEmployeeAttendanceDateRange(new DateTime(DateTime.Now.Year, DateTime.UtcNow.Month, 1), DateTime.UtcNow).AsQueryable();
            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate) && companyAgentId !=null)
            {
                query = GetEmployeeAttendanceDateRange(Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate)).Where(r=>r.AgentId==companyAgentId).AsQueryable();
            }
            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate) && companyAgentId == null)
            {
                query = GetEmployeeAttendanceDateRange(Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate)).AsQueryable();
            }
            var employeeWiseList = GetAttendanceData(query.ToList());
            var agentWiseList = GetAgentWiseList(query.ToList());
            return Json(new { EmployeeList = employeeWiseList,AgentWiseCount= agentWiseList }, JsonRequestBehavior.AllowGet);
        }

        private List<AgentWiseAttendaneModel> GetAgentWiseList(List<AttendanceModel> result)
        {
            var aList = new List<AgentWiseAttendaneModel>();
            var agentList = result.Where(x=>x.AgentId.HasValue).GroupBy(x => x.AgentId).ToList();
            foreach(var a in agentList)
            {

                var aModel = new AgentWiseAttendaneModel
                {
                    TotalEmployee = result.Count(c => c.AgentId == a.Key),
                    AgentId=a.Key,
                    AgentName=result.FirstOrDefault(c=>c.AgentId==a.Key).AgentName
                };
                var totalMinute = result.Where(y => y.AgentId == a.Key).Sum(x => x.TotalStayTimeInMinute);
                aModel.TotalTime = string.Format("{0}:{1}", totalMinute / 60, totalMinute % 60);
                aList.Add(aModel);
            }

            return aList;
        }


        private List<AttendanceTotalModel> GetAttendanceData(List<AttendanceModel> result)
        {
            var aList = new List<AttendanceTotalModel>();
            var empList = result.GroupBy(x => x.UserId).ToList();

            foreach (var e in empList)
            {
                var aModel = new AttendanceTotalModel();
                var employee = result.FirstOrDefault(x => x.UserId == e.Key);
                aModel.EmployeeName = employee.EmployeeName;
                aModel.DepartmentName = employee.DepartmentName;
                aModel.ImageFileName = employee.ImageFileName;
                aModel.UserId = e.Key;
                aModel.AgentId = employee.AgentId;
                aModel.TotalPresent = result.Count(y => y.UserId == e.Key && y.IsPresent);
                aModel.TotalLeave = result.Count(y => y.UserId == e.Key && y.IsLeave.HasValue && y.IsLeave.Value);

                aModel.TotalCheckedOutMissing = result.Count(y => y.UserId == e.Key && y.NotCheckedOut);
                var totalMinute = result.Where(y => y.UserId == e.Key).Sum(x => x.TotalStayTimeInMinute);
                aModel.TotalStayTime = string.Format("{0}:{1}", totalMinute / 60, totalMinute % 60);

                var officeHourInMin = result.Where(y => y.UserId == e.Key).Sum(x => x.DailyWorkingTimeInMin);
                aModel.TotalOfficeHour = string.Format("{0}:{1}", officeHourInMin / 60, officeHourInMin % 60);

                var overTimeOrDue = totalMinute - officeHourInMin;
                aModel.OvertimeOrDueHour = string.Format("{0}:{1}", overTimeOrDue / 60, overTimeOrDue % 60);

                aList.Add(aModel);
            }

            return aList;
        }
        private List<AttendanceModel> GetEmployeeAttendanceDateRange(DateTime startdate, DateTime enddate)
        {

            var result = _attendance.GetAttendance(startdate, enddate);
            if (result == null)
                return new List<AttendanceModel>();

            return result;
        }
        [HttpGet]
        public ActionResult GetMonthlyAttendanceDetailswithDate(string userId,string fromDate, string toDate)
        {
           var query= _attendance.GetAttendance(userId, new DateTime(DateTime.Now.Year, DateTime.UtcNow.Month, 1), DateTime.UtcNow).OrderBy(x => x.AttendanceDate);
            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate) )
            {
                query = _attendance.GetAttendance(userId,Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate)).OrderBy(x => x.AttendanceDate);
            }
       
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetAgentMonthlyAttendanceDetailswithDate(string agentId, string fromDate, string toDate)
        {
            var query = _attendance.GetAttendanceWithAgent(agentId, new DateTime(DateTime.UtcNow.Year,DateTime.UtcNow.Month, 1), DateTime.UtcNow).OrderBy(x => x.AttendanceDate);
            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                query = _attendance.GetAttendanceWithAgent(agentId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate)).OrderBy(x => x.AttendanceDate);
            }

            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public void ExportToExcel( string fromDate, string toDate, int? companyAgentId,int activeTab)
        {
            var listOfData = GetEmployeeAttendanceDateRange(new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1), DateTime.UtcNow).AsQueryable();
            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate) && companyAgentId != null)
            {
                listOfData = GetEmployeeAttendanceDateRange(Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate)).Where(r => r.AgentId == companyAgentId).AsQueryable();
            }
            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate) && companyAgentId == null)
            {
                listOfData = GetEmployeeAttendanceDateRange(Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate)).AsQueryable();
            }
            if (activeTab==1)
            {
                var employeeWiseList = GetAttendanceData(listOfData.ToList());
            ExportToExcelAsFormated(employeeWiseList.ToList(), "AttendanceReport_" + DateTime.Now.TimeOfDay, "Attendance Report");
            }
            else
            {
                var agentWiseList = GetAgentWiseList(listOfData.ToList());
                ExportToExcelAsFormated(agentWiseList.ToList(), "AttendanceReport_" + DateTime.Now.TimeOfDay, "Attendance Report");
            }

        }
        public void ExportDetailsToExcel(string userId,string fromDate, string toDate)
        {
            var query = _attendance.GetAttendance(userId, new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1), DateTime.UtcNow).OrderBy(x => x.AttendanceDate);
            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                query = _attendance.GetAttendance(userId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate)).OrderBy(x => x.AttendanceDate);
            }
            ExportToExcelAsFormated(query.ToList(), "AttendanceDetailsReport_" + DateTime.UtcNow.TimeOfDay, "Attendance Details Report");
        }
        public void ExportAgentDetailsToExcel(string agentId, string fromDate, string toDate)
        {
            var query = _attendance.GetAttendanceWithAgent(agentId, new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1), DateTime.UtcNow).OrderBy(x => x.AttendanceDate);
            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                query = _attendance.GetAttendanceWithAgent(agentId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate)).OrderBy(x => x.AttendanceDate);
            }
            ExportToExcelAsFormated(query.ToList(), "AgentDetailsReport_" + DateTime.UtcNow.TimeOfDay, "Agent Details Report");
        }
    }
}
