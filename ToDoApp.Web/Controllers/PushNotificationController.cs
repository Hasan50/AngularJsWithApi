using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ToDoApp.BusinessDomain;
using ToDoApp.BusinessDomain.Interfaces;
using ToDoApp.Common;
using ToDoApp.Common.Models;
using ToDoApp.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;

namespace ToDoApp.Web.Controllers
{
    [ValidateUser]
    public class PushNotificationController : BaseReportController
    {
        private readonly IUserCredential _userCredential;


        private static readonly HttpClient client = new HttpClient();
        public PushNotificationController()
        {
            _userCredential = RTUnityMapper.GetInstance<IUserCredential>();

        }
        [HttpPost]
        public ActionResult TaskNotification(string AssignedToId)
        {
            List<UserCredentialModel> PushTokenList = new List<UserCredentialModel>();
            PushTokenList = _userCredential.GetPushToken(_userInfo.CompanyId).Where(x => x.Id == AssignedToId).ToList();
            var dd = _userCredential.GetProfileDetails(_userInfo.Id);
            push(PushTokenList, "New task", "You got new task from " + dd.FullName);
            return Json(new { Success=true, Message = "Client updated failed" });
        }
        [HttpPost]
        public ActionResult ApplyLeaveNotification()
        {
            List<UserCredentialModel> PushTokenList = new List<UserCredentialModel>();
            PushTokenList = _userCredential.GetPushToken(_userInfo.CompanyId).Where(x => x.CompanyId == _userInfo.CompanyId && x.UserTypeId == (int)UserType.User).ToList();
            var dd = _userCredential.GetProfileDetails(_userInfo.Id);
            push(PushTokenList, "Leave application. ", dd.FullName + " apply for Leave");
            return Json(new { Success=true, Message = "Client updated failed" });
        }
        [HttpPost]
        public ActionResult LeaveApproveNotification(string applicantId)
        {
            List<UserCredentialModel> PushTokenList = new List<UserCredentialModel>();
            PushTokenList = _userCredential.GetPushToken(_userInfo.CompanyId).Where(x => x.Id == applicantId).ToList();
            push(PushTokenList, "Approved ", "Your leave has been approved");
            return Json(new { Success=true, Message = "Client updated failed" });
        }
        [HttpPost]
        public ActionResult LeaveRejectNotification(string applicantId)
        {
            List<UserCredentialModel> PushTokenList = new List<UserCredentialModel>();
            PushTokenList = _userCredential.GetPushToken(_userInfo.CompanyId).Where(x => x.Id == applicantId).ToList();
            push(PushTokenList, "Rejected ", "Your leave has been Rejected");
            return Json(new { Success=true, Message = "Client updated failed" });
        }
        [HttpPost]
        public ActionResult TaskStatusNotification(string createdById)
        {
            List<UserCredentialModel> PushTokenList = new List<UserCredentialModel>();
            PushTokenList = _userCredential.GetPushToken(_userInfo.CompanyId).Where(x => x.Id == createdById).ToList();
            push(PushTokenList, "Status Change", "Task status have been changed");
            return Json(new { Success=true, Message = "Client updated failed" });
        }

        private void push(List<UserCredentialModel> pushlist, string title, string body)
        {
            foreach (var item in pushlist)
            {
                if (item.PushToken != null)
                {
                    var jFcmData = new JObject();
                    var jData = new JObject();
                    jData.Add("data", "go here");
                    jFcmData.Add("to", item.PushToken);
                    jFcmData.Add("sound", "default");
                    jFcmData.Add("title", title);
                    jFcmData.Add("body", body);
                    jFcmData.Add("data", jData);

                    var myContent = JsonConvert.SerializeObject(jFcmData);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    try
                    {
                        var response = client.PostAsync("https://exp.host/--/api/v2/push/send", byteContent).Result;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

            }
        }

    }
}
