using PeopleTrackingApi.BusinessDomain;
using PeopleTrackingApi.BusinessDomain.Interfaces;
using PeopleTrackingApi.BusinessDomain.Models;
using PeopleTrackingApi.Common;
using PeopleTrackingApi.Common.Models;
using PeopleTrackingApi.Web.Filters;
using PeopleTrackingApi.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PeopleTrackingApi.Web.Controllers
{
    [ValidateUser]
    public class EmployeeController : BaseReportController
    {
        private readonly IEmployee _employeeRepository;
        private readonly IUserCredential _userCredential;

        public EmployeeController()
        {
            _employeeRepository = RTUnityMapper.GetInstance<IEmployee>();
            _userCredential = RTUnityMapper.GetInstance<IUserCredential>();
        }
        public ActionResult AllEmployee()
        {
            return PartialView();
        }
        public ActionResult EmployeeCreate()
        {
            return PartialView();
        }
        [HttpGet]
        public JsonResult GetAll()
        {
            var query = _employeeRepository.GetEmployee();
            foreach (var item in query)
            {
                item.CompanyAgentName = String.Join(",", _employeeRepository.GetEmployeeAgent(item.Id).Select(r => r.AgentName));
            }
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Get(int id)
        {
            var query = _employeeRepository.GetEmployee().FirstOrDefault(x => x.Id == id);
            query.CompanyAgentName = String.Join(",", _employeeRepository.GetEmployeeAgent(query.Id).Select(r => r.AgentName));
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public void EmployeeExportToExcel()
        {
            var listOfData = _employeeRepository.GetEmployee();
            foreach (EmployeeUser item in listOfData)
            {
                item.CompanyAgentName = String.Join(",", _employeeRepository.GetEmployeeAgent(item.Id).Select(r => r.AgentName));
            }
            ExportToExcelAsFormated(listOfData.ToList(), "EmployeesReport" + DateTime.Now.TimeOfDay, "Employees");
        }
        [HttpPost]
        public ActionResult CreateEmployee(EmployeeRegistrationModel json, List<EmployeeCompanyAgent> employeeCompanyAgent)
        {
            if (json == null)
                return Json(new { Success = true, Message = "Please create a company from setting menu." });
            var model = new EmployeeRegistrationModel
            {
                Email = json.Email,
                PhoneNumber = json.PhoneNumber,
                Password = json.Password,
                UserName = json.UserFullName,
                Gender = json.Gender,
                UserFullName = json.UserFullName,
                UserType = json.UserType,
                Designation = json.Designation,
                DepartmentId = json.DepartmentId,
                IsAutoCheckPoint = json.IsAutoCheckPoint,
                AutoCheckPointTime = json.AutoCheckPointTime,


            };

            var response = CreateUser(model);
            if (!response.Success)
            {
                _userCredential.DeleteUser(response.ReturnCode);
                return Json(new { Success = false, Message = response.Message });
            }
            EmployeeUser empUser = new EmployeeUser
            {
                UserId = response.ReturnCode,
                CompanyId = _userInfo.CompanyId,
                UserName = model.UserFullName,
                PhoneNumber = model.PhoneNumber,
                Designation = model.Designation,
                IsAutoCheckPoint = model.IsAutoCheckPoint,
                AutoCheckPointTime = model.AutoCheckPointTime,
                EmployeeCode = json.EmployeeCode,
                Gender = json.Gender,
            };
            var userResponse = _employeeRepository.Create(empUser);
            if (userResponse != null)
            {
                var empId = _employeeRepository.GetByPortalUserId(empUser.UserId).Id;
                foreach (var item in employeeCompanyAgent)
                {
                    item.EmployeeUserId = empId;
                    _employeeRepository.CreateEmpAgent(item);
                }
            }
            return Json(new { Success = true, Message = response.Message });
        }
        private ResponseModel CreateUser(EmployeeRegistrationModel model)
        {
            var userModel = _userCredential.GetByLoginID(model.PhoneNumber, UserType.User);
            if (userModel != null)
                return new ResponseModel { Message = "This mobile number already exists." };
            //var p = GeneratePassword();
            var password = CryptographyHelper.CreateMD5Hash(model.Password);
            var response = _userCredential.Save(new UserCredentialModel
            {
                FullName = model.UserFullName,
                CompanyId = _userInfo.CompanyId,
                UserTypeId = (int)UserType.User,
                Email = model.Email,
                ContactNo = model.PhoneNumber,
                LoginID = model.PhoneNumber,
                IsActive = true,
                Password = password,

            });

            if (response.Success)
            {
                try
                {
                    Task.Run(() => SendMailToUser(model.Email, model.PhoneNumber, model.Password));
                }
                catch (Exception ex)
                {

                }

            }
            return new ResponseModel { Success = response.Success, Message = "Success", ReturnCode = response.ReturnCode };
        }
        public void SendMailToUser(string email, string loginID, string p)
        {
            if (string.IsNullOrEmpty(email))
                return;

            var sb = new StringBuilder();
            sb.Append(string.Format("Please download App from playstore."));
            sb.Append(string.Format("<div></div>"));
            sb.Append(string.Format("<div>Your Login ID : {0}</div>", loginID));
            sb.Append(string.Format("<div>Your Password : {0}</div>", p));

            var recipient = new List<string> { email };
            new Email(ConfigurationManager.AppSettings["EmailSender"], ConfigurationManager.AppSettings["EmailSender"], "Your User Credential", sb.ToString()).SendEmail(recipient, ConfigurationManager.AppSettings["EmailSenderPassword"]);
        }
        [HttpPost]
        public ActionResult Update(PortalUserViewModel json, List<EmployeeCompanyAgent> employeeCompanyAgent)
        {
            var response = _employeeRepository.UpdateEmployee(json);
            if (response.Success)
            {
                _employeeRepository.UpdateEmployeeCrediantial(json);
                if (!string.IsNullOrEmpty(json.Password))
                    ChangePassword(new LocalPasswordModel { UserName = json.UserId, NewPassword = json.Password });

            }
            if (response.Success)
                DeleteCompnayAgent(json,employeeCompanyAgent);

            if (response.Success && employeeCompanyAgent!=null)
            {

                foreach (var item in employeeCompanyAgent)
                {

                    _employeeRepository.CreateEmpAgent(item);
                }
                return Json(new ResponseModel { Success = response.Success, Message = "Success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new ResponseModel { Success = response.Success, Message = "Faild" }, JsonRequestBehavior.AllowGet);

        }
        private ResponseModel DeleteCompnayAgent(PortalUserViewModel json, List<EmployeeCompanyAgent> employeeCompanyAgent)
        {
            var response = new ResponseModel();
            var existingList = _employeeRepository.GetEmployeeAgent(Convert.ToInt32(json.Id));
            if (employeeCompanyAgent != null)
            {
                var deletedData = existingList.Select(r => new { r.CompanyAgentId, r.EmployeeUserId }).Except(employeeCompanyAgent.Select(r => new { r.CompanyAgentId, r.EmployeeUserId })).ToList();
                foreach (var d in deletedData)
                {
                     response = _employeeRepository.DeleteEmployeeAgent(d.CompanyAgentId, d.EmployeeUserId);
                }
            }
            else
            {
                foreach (var d in existingList)
                {
                     response = _employeeRepository.DeleteEmployeeAgent(d.CompanyAgentId, d.EmployeeUserId);
                }
            }
            return response;
        }
        private ResponseModel ChangePassword(LocalPasswordModel json)
        {
            if (ModelState.IsValid)
            {
                var user = _userCredential.GetProfileDetails(json.UserName);
                if (user == null)
                    return new ResponseModel { Success = false, Message = "Invalid userid/password." };


                var response = _userCredential.ChangePassword(user.Id, CryptographyHelper.CreateMD5Hash(json.NewPassword));
                return new ResponseModel { Success = response.Success, Message = "Updated successfully" };
            }
            return new ResponseModel { Success = false, Message = "Oops!try again." };
        }
        [HttpGet]
        public ActionResult DeleteEmployee(string id)
        {
            var response = _employeeRepository.Delete(id);
            if (response.Success)
            {
                return Json(new ResponseModel { Success = response.Success, Message = "Success" }, JsonRequestBehavior.AllowGet);

            }
            return Json(new ResponseModel { Success = response.Success, Message = "Faild" }, JsonRequestBehavior.AllowGet);
        }
    }
}
