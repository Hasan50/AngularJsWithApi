using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ToDoApp.BusinessDomain;
using ToDoApp.BusinessDomain.Interfaces;
using ToDoApp.BusinessDomain.Models;
using System.Linq;
using ToDoApp.Common;
using ToDoApp.Common.Models;

namespace ToDoApp.Web.Controllers.Api
{
    public class LeaveApiController : BaseApiController
    {
        private readonly IEmployeeLeave _leaveRepository;
        private readonly ISickType _sickType;
        private readonly INotificationLog _notificationLog;

        public LeaveApiController()
        {
            _leaveRepository = RTUnityMapper.GetInstance<IEmployeeLeave>();
            _sickType = RTUnityMapper.GetInstance<ISickType>();
            _notificationLog = RTUnityMapper.GetInstance<INotificationLog>();
        }
        [HttpGet]
        public HttpResponseMessage GetLeaveWithFilter(string fromDate, string toDate, string userName)
        {
            var result = _leaveRepository.GetLeaveWithFilter(Convert.ToDateTime(fromDate),Convert.ToDateTime(toDate),userName,this.CompanyId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [HttpGet]
        public HttpResponseMessage GetLeave()
        {
            var result = _leaveRepository.GetLeave(this.CompanyId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        public HttpResponseMessage GetApprovedLeave()
        {
            var result = _leaveRepository.GetLeave(this.CompanyId).Where(x=>x.IsApproved);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        public HttpResponseMessage GetRejectedLeave()
        {
            var result = _leaveRepository.GetLeave(this.CompanyId).Where(x => x.IsRejected);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [HttpGet]
        public HttpResponseMessage GetPendingLeave()
        {
            var result = _leaveRepository.GetLeave(this.CompanyId).Where(x => !x.IsApproved && !x.IsRejected);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        public HttpResponseMessage GetUserLeaves(string userId)
        {
            var result = _leaveRepository.GetUserLeaves(userId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [HttpGet]
        public HttpResponseMessage GetSickType()
        {
            var result = _sickType.GetAllSickType().Where(r=> r.CompanyId==this.CompanyId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [HttpPost]
        public IHttpActionResult CreateLeave(EmployeeLeaveModel json)
        {
            var model = new EmployeeLeaveModel
            {
                EmployeeId = json.EmployeeId,
                FromDate =Convert.ToDateTime(json.LeaveApplyFrom),
                ToDate = Convert.ToDateTime(json.LeaveApplyTo),
                IsHalfDay = json.IsHalfDay,
                LeaveTypeId = json.LeaveTypeId,
                SickTypeId=json.SickTypeId,
                LeaveReason = json.LeaveReason,
                CreatedAt = DateTime.Now.ToString(),
                IsApproved = false,
                IsRejected = false,
                RejectReason = json.RejectReason,
                ApprovedById = null,
                ApprovedAt = null,
                UserId=json.UserId,
                CompanyId=this.CompanyId
            };
            var response = _leaveRepository.CreateEmployeeLeave(model);
            if (!response.Success)
                return Ok(response);

            AddNotificationForLeaveApply(json);
            return Ok(new { Success = true });
        }

        private void AddNotificationForLeaveApply(EmployeeLeaveModel model)
        {
            try
            {
                _notificationLog.Add(new NotificationLogModel
                {
                    CompanyId = this.CompanyId,
                    ActionById = this.UserId,
                    NotificationTypeId=(int)NotificationType.Leave,
                    Title = string.Format("New leave apply at {0}",DateTime.UtcNow.ToZoneTimeBD().ToString(Constants.DateTimeFormat)),
                    Description=string.Format("Leave apply from {0} to {1}.Leave Reason: {2}",model.FromDateVw,model.ToDateVw,model.LeaveReason)
                }); ;
            }
            catch(Exception ex)
            {

            }
        }
        /// <summary>
        /// on leave approval automaticaly a notice post to Notice Board.so all employee can see his/her leave status
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Approved(int id,string userId)
        {
            var response = _leaveRepository.Approved(id,userId);         
            return Ok(response);
        }

        [HttpGet]
        public IHttpActionResult Rejected(int id)
        {
            var response = _leaveRepository.Rejected(id);
            return Ok(response);
        }


        /// <summary>
        /// here leve type fixed.you can change from enum.go to LeaveType and add/remove new item
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetLeaveTypeList()
        {
            var list = Enum.GetValues(typeof(LeaveType)).Cast<LeaveType>().Select(v => new NameIdPairModel
            {
                Name = EnumUtility.GetDescriptionFromEnumValue(v),
                Id = (int)v
            }).ToList();
            return Ok(list);
        }
    }
}
