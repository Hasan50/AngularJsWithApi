using PeopleTrackingApi.Common.Models;
using System.Collections.Generic;
using PeopleTrackingApi.BusinessDomain.Models;

namespace PeopleTrackingApi.BusinessDomain.Interfaces
{
    public interface IEmployee
    {
        EmployeeUser Create(EmployeeUser model);
        EmployeeCompanyAgent CreateEmpAgent(EmployeeCompanyAgent model);
        ResponseModel UpdateEmployeeCrediantial(PortalUserViewModel model);
        List<EmployeeUser> GetEmployee();
        ResponseModel Delete(string id);
        ResponseModel DeleteEmployeeAgent(int companyAgentId, int employeeId);
        EmployeeUser GetEmployeeById(int id);
        List<TextValuePairModel> GetEmployeeAsTextValue();
        EmployeeUser GetByPortalUserId(string userId);
        List<EmployeeCompanyAgent> GetEmployeeAgent(int empId);
        ResponseModel UpdateEmployee(PortalUserViewModel model);
        ResponseModel UpdatePushToken(string userId, string pushToken);
    }
}