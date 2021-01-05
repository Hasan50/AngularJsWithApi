using Newtonsoft.Json;
using ToDoApp.BusinessDomain;
using ToDoApp.BusinessDomain.Interfaces;
using ToDoApp.BusinessDomain.Models;
using ToDoApp.Common;
using ToDoApp.Common.Models;
using ToDoApp.Web.Filters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ToDoApp.Web.Controllers
{
    [ValidateUser]
    public class TaskController : BaseReportController
    {
        private readonly IEmployeeTask _taskRepository;
        private readonly INotificationLog _notificationLog;

        public TaskController()
        {
            _taskRepository = RTUnityMapper.GetInstance<IEmployeeTask>();
            _notificationLog = RTUnityMapper.GetInstance<INotificationLog>();
        }
        public ActionResult Index()
        {
            return PartialView();
        }
        public ActionResult TaskCreate()
        {
            return PartialView();
        }
        public ActionResult TaskDetails()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult SaveTask()
        {
            var httpRequest = Request;
            var postOb = httpRequest["postOb"];
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            TaskModel json = JsonConvert.DeserializeObject<TaskModel>(postOb, settings);
            if (!json.StatusId.HasValue)
            {
                json.StatusId = (int)TaskStatus.ToDo;
            }
            if (!json.PriorityId.HasValue)
            {
                json.PriorityId = (int)TaskPriority.Normal;
            }
            json.CompanyId = _userInfo.CompanyId;
            var result = new ResponseModel();
            if (httpRequest.Files.Count > 0)
            {
                List<TaskAttachment> taskAttachments = new List<TaskAttachment>();
                foreach (string file in httpRequest.Files)
                {
                    TaskAttachment taskAttachment = new TaskAttachment();
                    string fileId = null;
                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {

                        int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB  

                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png", ".pdf" };
                        var ext = Path.GetExtension(postedFile.FileName);
                        //var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {

                            var message = string.Format("Please Upload image of type .jpg,.gif,.png,.pdf");
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {

                            var message = string.Format("Please Upload a file upto 1 mb.");
                        }

                        else
                        {
                            fileId = new string(Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", "-");
                            fileId = fileId + DateTime.Now.ToString("yymmssfff") + extension;
                            taskAttachment.BlobName = fileId;
                            taskAttachment.FileName = fileId;
                            taskAttachments.Add(taskAttachment);
                            var picPath = HttpContext.Server.MapPath(@"~\UploadFiles\");
                            if (!Directory.Exists(picPath))
                            {
                                Directory.CreateDirectory(picPath);
                            }

                            var filePath = Path.Combine(picPath, fileId);
                            if (json.Id != null)
                            {
                                if (System.IO.File.Exists(Path.Combine(picPath, fileId)))
                                {
                                    System.IO.File.Delete(Path.Combine(picPath, fileId));
                                }
                            }
                            postedFile.SaveAs(filePath);

                        }
                    }
                }
                json.CreatedById = _userInfo.Id;
                result = _taskRepository.AddOrUpdate(json, taskAttachments);
                AddNotificationForTaskCreate(json);
            }
            else
            {
                result = _taskRepository.AddOrUpdate(json, null);
                if (string.IsNullOrEmpty(json.Id))
                    AddNotificationForTaskCreate(json);
            }
            return Json(new { result.Success, Message = "Task created successfully" });
        }

        private void AddNotificationForTaskCreate(TaskModel model)
        {
            try
            {
                _notificationLog.Add(new NotificationLogModel
                {
                    CompanyId = _userInfo.CompanyId,
                    ActionById = _userInfo.Id,
                    NotificationTypeId = (int)NotificationType.Task,
                    Title = string.Format("Task : {0}",model.Title),
                    Description = string.Format("{0}", model.Description)
                }); ;
            }
            catch (Exception ex)
            {

            }
        }

        [HttpGet]
        public ActionResult GetTaskAttachments(string taskId)
        {
            var result = _taskRepository.GetTaskAttachments(taskId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetTasks()
        {
            var result = _taskRepository.GetTasks(_userInfo.CompanyId).OrderByDescending(x => x.CreatedAt);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetTasksWithFilter(string date, string dueDate)
        {
            var list = _taskRepository.GetTasks(_userInfo.CompanyId).OrderByDescending(x => x.CreatedAt).AsQueryable();
            var query = list;
            if (date == "null") date = null;
            if (dueDate == "null") dueDate = null;
            if (date != null || dueDate !=null)
            {
                query = list.Where(r => Convert.ToDateTime(r.CreatedAt).ToString("yyyy-MM-dd") == Convert.ToDateTime(date).ToString("yyyy-MM-dd") || Convert.ToDateTime(r.DueDate).ToString("yyyy-MM-dd") == Convert.ToDateTime(dueDate).ToString("yyyy-MM-dd"));
            }

           
            return Json(query, JsonRequestBehavior.AllowGet); 
        }
        [HttpGet]
        public ActionResult GetTaskDetails(string id)
        {
            var result = _taskRepository.GetTaskDetails(id);
            return Json(result, JsonRequestBehavior.AllowGet); 
        }
        [HttpGet]
        public ActionResult GetTaskStatusList()
        {
            var list = Enum.GetValues(typeof(TaskStatus)).Cast<TaskStatus>().Select(v => new NameIdPairModel
            {
                Name = EnumUtility.GetDescriptionFromEnumValue(v),
                Id = (int)v
            }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet); 
        }
        [HttpGet]
        public ActionResult GetPriorityList()
        {
            var list = Enum.GetValues(typeof(TaskPriority)).Cast<TaskPriority>().Select(v => new NameIdPairModel
            {
                Name = EnumUtility.GetDescriptionFromEnumValue(v),
                Id = (int)v
            }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet); 
        }
        [HttpGet]
        public ActionResult DeleteTask(string id)
        {
            var result = _taskRepository.DeleteTask(id);
            if (result.Success)
            {
                return Json(new { result.Success, Message = "Task deleted successfully" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result.Success, Message = "Task deleted failed" }, JsonRequestBehavior.AllowGet);
        }
        public void SendMailToUser(string email, string loginID, string p)
        {
            if (string.IsNullOrEmpty(email))
                return;

            var sb = new StringBuilder();
            sb.Append(string.Format("Below is your portal login credential.You can login to application using your below credential."));
            sb.Append(string.Format("<div></div>"));
            sb.Append(string.Format("<div>Your Login ID : {0}</div>", loginID));
            sb.Append(string.Format("<div>Your Password : {0}</div>", p));

            var recipient = new List<string> { email };
            new Email(ConfigurationManager.AppSettings["EmailSender"],
                ConfigurationManager.AppSettings["EmailSender"],
                "Your User Credential of Head Body Best Application", sb.ToString())
                .SendEmail(recipient, ConfigurationManager.AppSettings["EmailSenderPassword"]);
        }

        public void ExportToExcel(string date, string dueDate)
        {
            var list = _taskRepository.GetTasks(_userInfo.CompanyId).AsQueryable();
            var listOfData = list;
            if (date == "null") date = null;
            if (dueDate == "null") dueDate = null;
            if (date != null || dueDate != null)
            {
                listOfData = list.Where(r => Convert.ToDateTime(r.CreatedAt).ToString("yyyy-MM-dd") == Convert.ToDateTime(date).ToString("yyyy-MM-dd") || Convert.ToDateTime(r.DueDate).ToString("yyyy-MM-dd") == Convert.ToDateTime(dueDate).ToString("yyyy-MM-dd")).OrderByDescending(x => x.CreatedAt);
            }
            ExportToExcelAsFormated(listOfData.ToList(), "TaskReport_" + DateTime.Now.TimeOfDay, "Task Report");
        }
    }
}
