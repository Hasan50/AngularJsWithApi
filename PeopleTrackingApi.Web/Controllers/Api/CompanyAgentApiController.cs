using PeopleTrackingApi.BusinessDomain;
using PeopleTrackingApi.BusinessDomain.Interfaces;
using PeopleTrackingApi.BusinessDomain.Models;
using PeopleTrackingApi.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Web.Http;

namespace PeopleTrackingApi.Web.Controllers.Api
{
    public class CompanyAgentApiController : BaseApiController
    {
        private readonly ICompanyAgent _companyAgentRepository;
        public CompanyAgentApiController()
        {
            _companyAgentRepository = RTUnityMapper.GetInstance<ICompanyAgent>();
        }
      
        [HttpPost]
        public IHttpActionResult Save([FromBody]CompanyAgent json)
        {
            json.CreatedById = this.UserId;
            var response = _companyAgentRepository.Create(json);
            return Ok(response);
        }


        [HttpPost]
        public IHttpActionResult UpdateCompanyAgent(CompanyAgent json)
        {
            var response = _companyAgentRepository.Update(json);
            return Ok(response);
        }

        [HttpGet]
        public IHttpActionResult GetCompanyAgentList()
        {
            var response = _companyAgentRepository.GetCompanyAgentList(this.CompanyId);
            return Ok(response);
        }
        [HttpGet]
        public IHttpActionResult GetCompanyAgentListWithEmployee()
        {
            var response = _companyAgentRepository.GetCompanyAgentListWithEmployee(this.UserId);
            return Ok(response);
        }
    }
}
