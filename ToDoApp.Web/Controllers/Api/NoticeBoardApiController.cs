using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ToDoApp.BusinessDomain;
using ToDoApp.BusinessDomain.Interfaces;
using ToDoApp.BusinessDomain.Models;

namespace ToDoApp.Web.Controllers.Api
{
    public class NoticeBoardApiController : BaseApiController
    {
        private readonly INoticeBoard _noticeBoardRepository;
        public NoticeBoardApiController()
        {
            _noticeBoardRepository = RTUnityMapper.GetInstance<INoticeBoard>();
        }
        [HttpGet]
        public HttpResponseMessage GetNoticeBoard()
        {
            var result = _noticeBoardRepository.GetNoticeBoard();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        public HttpResponseMessage GetNoticeBoardById(string Id)
        {
            var result = _noticeBoardRepository.GetNoticeBoardById(Id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        /// <summary>
        /// Notice save for only text or image also
        /// </summary>
        /// <param name="jObject"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SaveNoticeBoard(JObject jObject)
        {
            dynamic json = jObject;
            var noticeBoard = new NoticeDepartmentVIewModel
            {
                Details = json.Details,
                ImageFileName = json.ImageFileName,
                CreatedBy = json.CreatedBy
            };
            
            var response = _noticeBoardRepository.CreateNoticeBoard(noticeBoard);
            return Ok(response);
            
        }
    }
}
