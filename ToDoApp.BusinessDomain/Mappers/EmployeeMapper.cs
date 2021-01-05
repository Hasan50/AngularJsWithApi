using ToDoApp.Common.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using ToDoApp.BusinessDomain.Models;

namespace ToDoApp.BusinessDomain.Mappers
{
    public static class EmployeeMapper
    {

        public static List<EmployeeUser> ToEmployeeModel(DbDataReader readers)
        {
            if (readers == null)
                return null;
            var models = new List<EmployeeUser>();
            while (readers.Read())
            {
                var model = new EmployeeUser
                {
                    Id = Convert.ToInt32(readers["Id"]),
                    UserId = Convert.IsDBNull(readers["UserId"]) ? string.Empty : Convert.ToString(readers["UserId"]),
                    UserName = Convert.IsDBNull(readers["UserName"]) ? string.Empty : Convert.ToString(readers["UserName"]),
                    Designation = Convert.IsDBNull(readers["Designation"]) ? string.Empty : Convert.ToString(readers["Designation"]),
                    CompanyId = Convert.ToInt32(readers["CompanyId"]),
                    CompanyName = Convert.IsDBNull(readers["CompanyName"]) ? string.Empty : Convert.ToString(readers["CompanyName"]),
                    PhoneNumber = Convert.IsDBNull(readers["PhoneNumber"]) ? string.Empty : Convert.ToString(readers["PhoneNumber"]),
                    Email = Convert.IsDBNull(readers["Email"]) ? string.Empty : Convert.ToString(readers["Email"]),
                    ImageFileName = Convert.IsDBNull(readers["ImageFileName"]) ? string.Empty : Convert.ToString(readers["ImageFileName"]),
                    ImageFileId = Convert.IsDBNull(readers["ImageFileId"]) ? string.Empty : Convert.ToString(readers["ImageFileId"]),
                    DepartmentName = Convert.IsDBNull(readers["DepartmentName"]) ? string.Empty : Convert.ToString(readers["DepartmentName"]),
                    IsActive = Convert.IsDBNull(readers["IsActive"]) ? false : Convert.ToBoolean(readers["IsActive"]),
                    EmployeeCode = Convert.IsDBNull(readers["EmployeeCode"]) ? string.Empty : Convert.ToString(readers["EmployeeCode"]),
                    Gender = Convert.IsDBNull(readers["Gender"]) ? string.Empty : Convert.ToString(readers["Gender"]),
                };
                models.Add(model);
            }
            return models;
        }
        public static List<EmployeeCompanyAgent> ToEmployeeCompanyAgentModel(DbDataReader readers)
        {
            if (readers == null)
                return null;
            var models = new List<EmployeeCompanyAgent>();
            while (readers.Read())
            {
                var model = new EmployeeCompanyAgent
                {
                    Id = Convert.ToInt32(readers["Id"]),
                    CompanyAgentId = Convert.ToInt32(readers["CompanyAgentId"]),
                    EmployeeUserId = Convert.ToInt32(readers["EmployeeUserId"]),
                    AgentName = Convert.IsDBNull(readers["AgentName"]) ? string.Empty : Convert.ToString(readers["AgentName"]),
                    CreatedDate = Convert.IsDBNull(readers["CreatedDate"]) ? (DateTime?)null : Convert.ToDateTime(readers["CreatedDate"]),
                };
                models.Add(model);
            }
            return models;
        }
        public static List<TextValuePairModel> ToTextValuePairModel(DbDataReader readers)
        {
            if (readers == null)
                return null;
            var models = new List<TextValuePairModel>();
            while (readers.Read())
            {
                var model = new TextValuePairModel
                {
                    Text = Convert.IsDBNull(readers["Name"]) ? string.Empty : Convert.ToString(readers["Name"]),
                    Value = Convert.IsDBNull(readers["Id"]) ? string.Empty : Convert.ToString(readers["Id"])
                };
                models.Add(model);
            }
            return models;
        }
    }
}