using ToDoApp.BusinessDomain;
using ToDoApp.BusinessDomain.Interfaces;
using ToDoApp.BusinessDomain.Models;
using ToDoApp.Common;
using ToDoApp.Common.Models;
using ToDoApp.Web.Filters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ToDoApp.Web.Controllers
{
    [ValidateUser]
    public class MyClientsController : BaseReportController
    {
        private readonly ICompany _company;

        public MyClientsController()
        {
            _company = RTUnityMapper.GetInstance<ICompany>();
        }
        public ActionResult Index()
        {
            return PartialView();
        }
        public ActionResult ClientCreate()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult Save(Company json)
        {
            if (json.Id == 0)
            {
                json.CreatedById = this._userInfo.Id;
                var response = _company.Create(json);
                if (!response.Success)
                    return Json(new { response.Success, Message = "Client saved failed" });

                if (response.Success)
                {
                    SendMailToUser(json.CompanyAdminEmail, json.CompanyAdminLoginID, json.CompanyAdminPassword);
                    return Json(new { response.Success, Message = "Client saved successfully" });
                }

            }
            var uResponse = _company.Update(json);
            if (uResponse.Success)
                return Json(new { uResponse.Success, Message = "Client updated successfully" });
            return Json(new { uResponse.Success, Message = "Client updated failed" });
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
        [HttpGet]
        public ActionResult GetCompanyList()
        {
            var response = _company.GetCompanyList();
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            var query = _company.GetCompanyList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public void ClientsExportToExcel()
        {
            var listOfData = _company.GetCompanyList();
            ExportToExcelAsFormated(listOfData.ToList(), "MyClients_" + DateTime.Now.TimeOfDay, "My Clients");
        }

        [HttpPost]
        public ActionResult UpdateCompany(Company json)
        {
            var response = _company.Update(json);
            return Json(response);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var response = _company.GetCompanyList().FirstOrDefault(x => x.Id == id);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetUserCompany()
        {
            var response = _company.GetCompanyList().FirstOrDefault(x => x.Id == _userInfo.CompanyId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var response = _company.Delete(id);
            if (response.Success)
            {
                return Json(new ResponseModel { Success = response.Success, Message = "Success" }, JsonRequestBehavior.AllowGet);

            }
            return Json(new ResponseModel { Success = response.Success, Message = "Faild" }, JsonRequestBehavior.AllowGet);
        }
    }
}
