using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using PeopleTrackingApi.BusinessDomain;
using PeopleTrackingApi.Common;
using PeopleTrackingApi.BusinessDomain.Interfaces;
using PeopleTrackingApi.BusinessDomain.Models;
using PeopleTrackingApi.Common.Models;

namespace PeopleTrackingApi.Web.Controllers.Api
{
    /// <summary>
    /// Task image file save to Project inner folder.Folder Name UploadFiles
    /// </summary>
    public class TaskApiController : BaseApiController
    {
        private readonly IEmployeeTask _taskRepository;

        public TaskApiController()
        {
            _taskRepository = RTUnityMapper.GetInstance<IEmployeeTask>();
        }

        [HttpPost]
        public IHttpActionResult UploadDocuments()
        {
            var Message = string.Empty;
            string fileId = null;
            var httpRequest = HttpContext.Current.Request;
            var model = httpRequest.Params["title"];
            var postedFile = httpRequest.Files["BlobName"];
            var fileExtension = Path.GetExtension(postedFile.FileName);
            fileId = new string(Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", "-");
            fileId = fileId + DateTime.Now.ToString("yymmssfff") + fileExtension;
            var picPath = HttpContext.Current.Server.MapPath(@"~\UploadFiles\");
            if (!Directory.Exists(picPath))
            {
                Directory.CreateDirectory(picPath);
            }
            if (!IsValidFile(fileExtension))
                Message = "File Formate is not valid";
            var filePath = Path.Combine(picPath, fileId);
            Stream strm = postedFile.InputStream;
            if (fileExtension == ".pdf")
            {
                postedFile.SaveAs(filePath);
            }
            else
            {
                Compressimage(strm, filePath, postedFile.FileName);
            }
            //Compressimage(strm, filePath, postedFile.FileName);
            //postedFile.SaveAs(filePath);
            return Ok(new { ImagePath = fileId, Success = true });
        }
        public static void Compressimage(Stream sourcePath, string targetPath, String filename)
        {
            try
            {
                using (var image = Image.FromStream(sourcePath))
                {
                    float maxHeight = 1800.0f;
                    float maxWidth = 1800.0f;
                    int newWidth;
                    int newHeight;
                    string extension;
                    Bitmap originalBMP = new Bitmap(sourcePath);
                    int originalWidth = originalBMP.Width;
                    int originalHeight = originalBMP.Height;
                    if (originalWidth > maxWidth || originalHeight > maxHeight)
                    {
                        // To preserve the aspect ratio  
                        float ratioX = (float)maxWidth / (float)originalWidth;
                        float ratioY = (float)maxHeight / (float)originalHeight;
                        float ratio = Math.Min(ratioX, ratioY);
                        newWidth = (int)(originalWidth * ratio);
                        newHeight = (int)(originalHeight * ratio);
                    }
                    else
                    {
                        newWidth = (int)originalWidth;
                        newHeight = (int)originalHeight;

                    }
                    Bitmap bitMAP1 = new Bitmap(originalBMP, newWidth, newHeight);
                    Graphics imgGraph = Graphics.FromImage(bitMAP1);
                    extension = Path.GetExtension(targetPath);
                    if (extension == ".png" || extension == ".gif")
                    {
                        imgGraph.SmoothingMode = SmoothingMode.AntiAlias;
                        imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        imgGraph.DrawImage(originalBMP, 0, 0, newWidth, newHeight);


                        bitMAP1.Save(targetPath, image.RawFormat);

                        bitMAP1.Dispose();
                        imgGraph.Dispose();
                        originalBMP.Dispose();
                    }
                    else if (extension == ".jpg")
                    {

                        imgGraph.SmoothingMode = SmoothingMode.AntiAlias;
                        imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        imgGraph.DrawImage(originalBMP, 0, 0, newWidth, newHeight);
                        ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                        Encoder myEncoder = Encoder.Quality;
                        EncoderParameters myEncoderParameters = new EncoderParameters(1);
                        EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
                        myEncoderParameters.Param[0] = myEncoderParameter;
                        bitMAP1.Save(targetPath, jpgEncoder, myEncoderParameters);

                        bitMAP1.Dispose();
                        imgGraph.Dispose();
                        originalBMP.Dispose();

                    }


                }

            }
            catch (Exception)
            {
                throw;

            }
        }


        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        private bool IsValidFile(string ext)
        {
            string[] validFileFormate = new string[] { "jpg", "png", "gif", "webp" };
            for (int i = 0; i < validFileFormate.Length; i++)
            {
                string vF = "." + validFileFormate[i];
                if (vF == ext)
                {
                    return true;
                }
            }
            return false;
        }

        [HttpPost]
        public IHttpActionResult SaveTaskFromWeb()
        {
            var httpRequest = HttpContext.Current.Request;
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
            json.CompanyId = this.CompanyId;
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
                            var picPath = HttpContext.Current.Server.MapPath(@"~\UploadFiles\");
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

                result = _taskRepository.AddOrUpdate(json, taskAttachments);
            }
            else
            {
                result = _taskRepository.AddOrUpdate(json, null);
            }

            return Ok(result);
        }

        //[HttpPost]
        //public IHttpActionResult SaveTaskFromWeb([FromBody]TaskModel json)
        //{
        //    if (!json.StatusId.HasValue)
        //    {
        //        json.StatusId = (int)TaskStatus.ToDo;
        //    }
        //    if (!json.PriorityId.HasValue)
        //    {
        //        json.PriorityId = (int)TaskPriority.Normal;
        //    }
        //    json.CompanyId = this.CompanyId;
        //    var result = _taskRepository.AddOrUpdate(json, null);
        //    return Ok(result);
        //}

        [HttpPost]
        public IHttpActionResult SaveTask()
        {
            var httpRequest = HttpContext.Current.Request;
            var modelst = httpRequest.Params["taskmodel"];
            var model = JsonConvert.DeserializeObject<TaskModel>(modelst);
            var taskAttachmentsModelst = httpRequest.Params["taskAttachmentsModel"];
            var taskAttachmentsModelOb = JsonConvert.DeserializeObject<List<TaskAttachment>>(taskAttachmentsModelst);
            if (!model.StatusId.HasValue)
            {
                model.StatusId = (int)TaskStatus.ToDo;
            }
            if (!model.PriorityId.HasValue)
            {
                model.PriorityId = (int)TaskPriority.Normal;
            }
            model.CompanyId = this.CompanyId;
            model.UpdatedById = this.UserId;
            var result = _taskRepository.AddOrUpdate(model, taskAttachmentsModelOb);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetTaskAttachments(string taskId)
        {
            var result = _taskRepository.GetTaskAttachments(taskId);
            return Ok(result);
        }


        [HttpGet]
        public IHttpActionResult GetTasks()
        {
            var result = _taskRepository.GetTasks(this.CompanyId).OrderByDescending(x => x.CreatedAt);
            return Ok(result);
        }
        [HttpGet]
        public IHttpActionResult GetTasksWithFilter(string date, string username)
        {
            var list = _taskRepository.GetTasks(this.CompanyId).OrderByDescending(x => x.CreatedAt);
            var result = list;
            if (username != "null" && username != "undefined")
            {
                result = list.Where(r => Convert.ToDateTime(r.CreatedAt).ToString("yyyy-MM-dd") == Convert.ToDateTime(date).ToString("yyyy-MM-dd") && r.AssignToName == username).OrderByDescending(x => x.CreatedAt);
            }

            return Ok(result);
        }
        [HttpGet]
        public IHttpActionResult DeleteTask(string id)
        {
            var result = _taskRepository.DeleteTask(id);
            return Ok(result);
        }
        [HttpGet]
        public IHttpActionResult GetTaskDetails(string id)
        {
            var result = _taskRepository.GetTaskDetails(id);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetTaskStatusList()
        {
            var list = Enum.GetValues(typeof(TaskStatus)).Cast<TaskStatus>().Select(v => new NameIdPairModel
            {
                Name = EnumUtility.GetDescriptionFromEnumValue(v),
                Id = (int)v
            }).ToList();
            return Ok(list);
        }
        [HttpGet]
        public IHttpActionResult GetCreatedByMeTasks(string userId)
        {
            var result = _taskRepository.GetTaskList(new TaskModel { CreatedById = userId }).OrderByDescending(x => x.CreatedAt);
            return Ok(result);
        }
        [HttpGet]
        public IHttpActionResult GetAssignedToMeTasks(string userId)
        {
            var result = _taskRepository.GetTaskList(new TaskModel { AssignedToId = userId }).OrderByDescending(x => x.CreatedAt);
            return Ok(result);
        }
        [HttpGet]
        public IHttpActionResult GetRelatedToMeTasks(string userId)
        {
            var result = _taskRepository.GetRelatedToMeTaskList(new TaskModel { AssignedToId = userId }).OrderByDescending(x => x.CreatedAt);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetPriorityList()
        {
            var list = Enum.GetValues(typeof(TaskPriority)).Cast<TaskPriority>().Select(v => new NameIdPairModel
            {
                Name = EnumUtility.GetDescriptionFromEnumValue(v),
                Id = (int)v
            }).ToList();
            return Ok(list);
        }
    }
}
