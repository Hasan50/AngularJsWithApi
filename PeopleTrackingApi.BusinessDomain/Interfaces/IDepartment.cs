using PeopleTrackingApi.Common.Models;
using System.Collections.Generic;
using PeopleTrackingApi.BusinessDomain.Models;

namespace PeopleTrackingApi.BusinessDomain.Interfaces
{
    public interface IDepartment
    {
        List<Department> GetDepartment();
        Department Create(Department model,string userId);
        ResponseModel UpdateDepartment(Department model);
        ResponseModel DeleteDepartmentById(string id);
    }
}