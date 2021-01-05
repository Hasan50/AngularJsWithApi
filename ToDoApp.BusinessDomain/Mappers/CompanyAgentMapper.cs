using System;
using System.Collections.Generic;
using System.Data.Common;
using ToDoApp.BusinessDomain.Models;

namespace ToDoApp.BusinessDomain.Mappers
{
    public static class CompanyAgentMapper
    {

        public static List<CompanyAgent> ToCompanyAgentModel(DbDataReader readers)
        {
            if (readers == null)
                return null;
            var models = new List<CompanyAgent>();

            while (readers.Read())
            {
                var model = new CompanyAgent
                {
                    Id = Convert.ToInt32(readers["Id"]),
                    AgentName = Convert.IsDBNull(readers["AgentName"]) ? string.Empty : Convert.ToString(readers["AgentName"]),
                    Address = Convert.IsDBNull(readers["Address"]) ? string.Empty : Convert.ToString(readers["Address"]),
                    PhoneNumber = Convert.IsDBNull(readers["PhoneNumber"]) ? string.Empty : Convert.ToString(readers["PhoneNumber"]),
                    CreatedById = Convert.IsDBNull(readers["CreatedById"]) ? string.Empty : Convert.ToString(readers["CreatedById"]),
                    CreatedDate = Convert.IsDBNull(readers["CreatedDate"]) ? (DateTime?)null : Convert.ToDateTime(readers["CreatedDate"]),
                    IsActive = Convert.IsDBNull(readers["IsActive"]) ? false : Convert.ToBoolean(readers["IsActive"]),
                    Email = Convert.IsDBNull(readers["Email"]) ? string.Empty : Convert.ToString(readers["Email"]),
                    GeofanceLat = Convert.IsDBNull(readers["GeofanceLat"]) ? string.Empty : Convert.ToString(readers["GeofanceLat"]),
                    GeofanceLng = Convert.IsDBNull(readers["GeofanceLng"]) ? string.Empty : Convert.ToString(readers["GeofanceLng"]),
                    ContactPersonName = Convert.IsDBNull(readers["ContactPersonName"]) ? string.Empty : Convert.ToString(readers["ContactPersonName"]),
                    OfficeStartTime = Convert.IsDBNull(readers["OfficeStartTime"]) ? string.Empty : Convert.ToString(readers["OfficeStartTime"]),
                    OfficeEndTime = Convert.IsDBNull(readers["OfficeEndTime"]) ? string.Empty : Convert.ToString(readers["OfficeEndTime"]),
                };

                models.Add(model);
            }

            return models;
        }
    }
}