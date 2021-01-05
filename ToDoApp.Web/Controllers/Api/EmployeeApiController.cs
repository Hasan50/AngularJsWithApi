using ToDoApp.Common;
using ToDoApp.Common.Models;
using System;
using System.Web.Http;
using ToDoApp.BusinessDomain;
using ToDoApp.BusinessDomain.Interfaces;
using ToDoApp.BusinessDomain.Models;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDoApp.Web.Controllers.Api
{
    public class EmployeeApiController : BaseApiController
    {
        private readonly IEmployee _employeeRepository;
        private readonly IUserCredential _userCredential;
        public EmployeeApiController()
        {
            _employeeRepository = RTUnityMapper.GetInstance<IEmployee>();
            _userCredential = RTUnityMapper.GetInstance<IUserCredential>();
        }
        [HttpPost]
        public IHttpActionResult CreateEmployee(EmployeeRegistrationModel json)
        {
            if (json == null)
                return Ok(new { Success = false, Message = "Please create a company from setting menu." });
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
                MaximumOfficeHours = json.MaximumOfficeHours,
                OfficeOutTime = json.OfficeOutTime,
            };

            var response = CreateUser(model);
            if (!response.Success)
                return Ok(response);
            EmployeeUser empUser = new EmployeeUser
            {
                UserId = response.Message,
                CompanyId = this.CompanyId,
                CompanyAgentId = json.CompanyAgentId,
                UserName = model.UserFullName,
                PhoneNumber = model.PhoneNumber,
                Designation = model.Designation,
                IsAutoCheckPoint = model.IsAutoCheckPoint,
                AutoCheckPointTime = model.AutoCheckPointTime,
                MaximumOfficeHours = model.MaximumOfficeHours,
                OfficeOutTime = model.OfficeOutTime,
                EmployeeCode = json.EmployeeCode,
                Gender = json.Gender
            };
            var userResponse = _employeeRepository.Create(empUser);
            if (userResponse != null)
            {
                var empId = _employeeRepository.GetByPortalUserId(empUser.UserId).Id;
                _employeeRepository.CreateEmpAgent(new EmployeeCompanyAgent { EmployeeUserId = empId,CompanyAgentId=json.CompanyAgentId,IsActive=true });
            }
            return Ok(new { Success = true, Message = response.Message });
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
                CompanyId = this.CompanyId,
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
            return new ResponseModel { Success = response.Success, Message = response.ReturnCode };
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


        private static string _numbers = "0123456789";
        Random random = new Random();


        private string GeneratePassword()
        {
            StringBuilder builder = new StringBuilder(6);
            string numberAsString = "";

            for (var i = 0; i < 6; i++)
            {
                builder.Append(_numbers[random.Next(0, _numbers.Length)]);
            }

            numberAsString = builder.ToString();
            return numberAsString;
        }


        [HttpGet]
        public IHttpActionResult DeleteEmployee(string id)
        {
            var userResponse = _employeeRepository.Delete(id);
            return Ok(userResponse);
        }
        [HttpPost]
        public IHttpActionResult UpdateEmployee(PortalUserViewModel json)
        {
            if (!string.IsNullOrEmpty(json.Password))
            {
                var password = CryptographyHelper.CreateMD5Hash(json.Password);
                json.Password = password;
            }
            var response = _employeeRepository.UpdateEmployee(json);
            if (response.Success)
            {
                _employeeRepository.UpdateEmployeeCrediantial(json);
            }
            return Ok(response);
        }

        [HttpGet]
        public IHttpActionResult GetEmployeeAsTextValue()
        {
            var userResponse = _employeeRepository.GetEmployeeAsTextValue();
            return Ok(userResponse);
        }

        [HttpGet]
        public IHttpActionResult GetEmployee()
        {
            var userResponse = _employeeRepository.GetEmployee();
            var data = (from x in userResponse
                        select new
                        {
                            x.Id,
                            x.UserId,
                            x.EmployeeCode,
                            x.UserName,
                            x.PhoneNumber,
                            x.MaximumOfficeHours,
                            DesignationName = x.Designation,
                            x.CompanyAgentId,
                            CompanyAgentName = String.Join(",", _employeeRepository.GetEmployeeAgent(x.Id).Select(r => r.AgentName)),
                            x.CompanyId,
                            x.DepartmentName,
                            x.IsAutoCheckPoint,
                            x.AutoCheckPointTime,
                            x.ImageFileId,
                            x.ImageFileName,
                            x.IsActive,
                            x.Gender

                        });
            return Ok(data);
        }
        [HttpPost]
        public IHttpActionResult UpdatePushToken(string userId, string PushToken)
        {
            var response = _employeeRepository.UpdatePushToken(userId, PushToken);
            return Ok(response);
        }

    }
}
