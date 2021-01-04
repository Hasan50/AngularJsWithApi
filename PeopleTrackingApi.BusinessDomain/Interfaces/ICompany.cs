using PeopleTrackingApi.Common.Models;
using System.Collections.Generic;
using PeopleTrackingApi.BusinessDomain.Models;

namespace PeopleTrackingApi.BusinessDomain.Interfaces
{
    public interface ICompany
    {
        List<Company> GetCompanyList();
        ResponseModel Create(Company model);
        ResponseModel Update(Company model);
        ResponseModel UpdateLogo(int id, string fileId, string fileName);
        ResponseModel Delete(int id);
        List<Company> GetCompanyListById(int Id);
    }
}