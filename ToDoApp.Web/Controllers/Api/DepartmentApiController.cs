using System.Net;
using System.Net.Http;
using System.Web.Http;
using ToDoApp.BusinessDomain;
using ToDoApp.BusinessDomain.Interfaces;
using ToDoApp.BusinessDomain.Models;

namespace ToDoApp.Web.Controllers.Api
{
    public class DepartmentApiController : BaseApiController
    {
        private readonly IDepartment _departmentRepository;
        public DepartmentApiController()
        {
            _departmentRepository = RTUnityMapper.GetInstance<IDepartment>();
        }
        [HttpGet]
        public HttpResponseMessage GetDepartment()
        {
            var result = _departmentRepository.GetDepartment();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [HttpGet]
        public HttpResponseMessage DeleteDepartmentById(string id)
        {
            var result = _departmentRepository.DeleteDepartmentById(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [HttpPost]
        public IHttpActionResult Save(Department json)
        {
            var department = new Department
            {
                DepartmentName = json.DepartmentName,
            };
            var response = _departmentRepository.Create(department, json.UserId);
            return Ok(response);
        }

        [HttpPost]
        public IHttpActionResult UpdateDepartment(Department json)
        {
            var department = new Department
            {
                Id = json.Id,
                DepartmentName = json.DepartmentName,
            };
            var response = _departmentRepository.UpdateDepartment(department);
            return Ok(response);
        }
    }
}
