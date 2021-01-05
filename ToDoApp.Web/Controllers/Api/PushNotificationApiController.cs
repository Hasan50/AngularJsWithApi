using ToDoApp.Common;
using ToDoApp.BusinessDomain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ToDoApp.BusinessDomain.Interfaces;
using ToDoApp.Common.Models;
using ToDoApp.Web.Controllers.Api;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace AttndApp.Web.Controllers.Api
{
    public class PushNotificationApiController : BaseApiController
    {
        private readonly IUserCredential _userCredential;


        private static readonly HttpClient client = new HttpClient();
        public PushNotificationApiController()
        {
            _userCredential = RTUnityMapper.GetInstance<IUserCredential>();

        }
        [HttpPost]
        public IHttpActionResult TaskNotification(string AssignedToId)
        {
            List<UserCredentialModel> PushTokenList = new List<UserCredentialModel>();
            PushTokenList = _userCredential.GetPushToken(this.CompanyId).Where(x => x.Id == AssignedToId).ToList();
            var dd = _userCredential.GetProfileDetails(this.UserId);
            push(PushTokenList, "New task", "You got new task from " + dd.FullName);
            return Ok(new { Success = true });
        }
        [HttpPost]
        public IHttpActionResult ApplyLeaveNotification()
        {
            List<UserCredentialModel> PushTokenList = new List<UserCredentialModel>();
            PushTokenList = _userCredential.GetPushToken(this.CompanyId).Where(x => x.CompanyId == this.CompanyId && x.UserTypeId == (int)UserType.User).ToList();
            var dd = _userCredential.GetProfileDetails(this.UserId);
            push(PushTokenList, "Leave application. ", dd.FullName + " apply for Leave");
            return Ok(new { Success = true });
        }
        [HttpPost]
        public IHttpActionResult LeaveApproveNotification(string applicantId)
        {
            List<UserCredentialModel> PushTokenList = new List<UserCredentialModel>();
            PushTokenList = _userCredential.GetPushToken(this.CompanyId).Where(x => x.Id == applicantId).ToList();
            push(PushTokenList, "Approved ", "Your leave have approved");
            return Ok(new { Success = true });
        }
        [HttpPost]
        public IHttpActionResult LeaveRejectNotification(string applicantId)
        {
            List<UserCredentialModel> PushTokenList = new List<UserCredentialModel>();
            PushTokenList = _userCredential.GetPushToken(this.CompanyId).Where(x => x.Id == applicantId).ToList();
            push(PushTokenList, "Rejected ", "Your leave have Rejected");
            return Ok(new { Success = true });
        }
        [HttpPost]
        public IHttpActionResult TaskStatusNotification(string createdById)
        {
            List<UserCredentialModel> PushTokenList = new List<UserCredentialModel>();
            PushTokenList = _userCredential.GetPushToken(this.CompanyId).Where(x => x.Id == createdById).ToList();
            push(PushTokenList, "Status Change", "Task status have been changed");
            return Ok(new { Success = true });
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
