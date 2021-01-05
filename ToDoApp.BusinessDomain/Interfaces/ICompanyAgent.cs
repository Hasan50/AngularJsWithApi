using ToDoApp.Common.Models;
using System.Collections.Generic;
using ToDoApp.BusinessDomain.Models;

namespace ToDoApp.BusinessDomain.Interfaces
{
    public interface ICompanyAgent
    {
        List<CompanyAgent> GetCompanyAgentList(int companyId);
        List<CompanyAgent> GetCompanyAgentDetails(int Id);
        List<CompanyAgent> GetCompanyAgentListWithEmployee(string userId);
        ResponseModel Create(CompanyAgent model);
        ResponseModel Update(CompanyAgent model);
        ResponseModel UpdateLogo(int id, string fileId, string fileName);
        ResponseModel Delete(int id);
    }
}