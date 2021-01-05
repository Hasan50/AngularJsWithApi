using ToDoApp.BusinessDomain;
using ToDoApp.BusinessDomain.Interfaces;
using ToDoApp.BusinessDomain.Models;
using ToDoApp.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Http;

namespace ToDoApp.Web.Controllers.Api
{
    public class CompanyApiController : BaseApiController
    {
        private readonly ICompany _companyRepository;
        public CompanyApiController()
        {
            _companyRepository = RTUnityMapper.GetInstance<ICompany>();
        }
      
        [HttpPost]
        public IHttpActionResult Save([FromBody]Company json)
        {
            //var password = GenerateRandomNumber();
            //json.CompanyAdminPassword = CryptographyHelper.CreateMD5Hash(password);
            json.CreatedById = this.UserId;
            var response = _companyRepository.Create(json);
            if (response.Success)
                SendMailToUser(json.CompanyAdminEmail, json.CompanyAdminLoginID, json.CompanyAdminPassword);

            return Ok(response);
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

        [HttpPost]
        public IHttpActionResult UpdateCompany(Company json)
        {
            var response = _companyRepository.Update(json);
            return Ok(response);
        }
        [HttpGet]
        public IHttpActionResult GetCompanyList()
        {
            var response = _companyRepository.GetCompanyList();
            return Ok(response);
        }
        [HttpGet]
        public IHttpActionResult GetCompanyPermission()
        {
            var result = _companyRepository.GetCompanyListById(this.CompanyId).FirstOrDefault();
            return Ok(new {result.CompanyName,CompanyId= result.Id.ToString(),result.IsMobileAppAccesible,result.IsShowLeaveMenu,result.IsShowTaskMenu,result.UserLanguage });
        }
        private static string _numbers = "0123456789";
        Random random = new Random();


        private string GenerateRandomNumber()
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

    }
}
