using PeopleTrackingApi.Common.Models;
using System.Collections.Generic;
using PeopleTrackingApi.BusinessDomain.Models;
using System;

namespace PeopleTrackingApi.BusinessDomain.Interfaces
{
    public interface IEmployeeLeave
    {
        List<EmployeeLeaveModel> GetLeave(int companyId);
        List<EmployeeLeaveModel> GetLeaveWithFilter(DateTime fromDate, DateTime toDate, string userName, int companyId);
        List<EmployeeLeaveModel> GetUserLeaves(string userId);
        List<EmployeeLeaveModel> GetLeaveById(int id);
        ResponseModel CreateEmployeeLeave(EmployeeLeaveModel model);
        ResponseModel Approved(int id, string userId);
        ResponseModel Rejected(int id);

    }
}