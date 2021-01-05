using System;
using System.Collections.Generic;
using System.Data.Common;
using ToDoApp.BusinessDomain.Models;

namespace ToDoApp.BusinessDomain.Mappers
{
    public static class CompanyMapper
    {

        public static List<Company> ToCompanyModel(DbDataReader readers)
        {
            if (readers == null)
                return null;
            var models = new List<Company>();

            while (readers.Read())
            {
                var model = new Company
                {
                    Id = Convert.ToInt32(readers["Id"]),
                    CompanyName = Convert.IsDBNull(readers["CompanyName"]) ? string.Empty : Convert.ToString(readers["CompanyName"]),
                    Address = Convert.IsDBNull(readers["Address"]) ? string.Empty : Convert.ToString(readers["Address"]),
                    PhoneNumber = Convert.IsDBNull(readers["PhoneNumber"]) ? string.Empty : Convert.ToString(readers["PhoneNumber"]),
                    CreatedById = Convert.IsDBNull(readers["CreatedById"]) ? string.Empty : Convert.ToString(readers["CreatedById"]),
                    CreatedDate = Convert.IsDBNull(readers["CreatedDate"]) ? (DateTime?)null : Convert.ToDateTime(readers["CreatedDate"]),
                    IsDateTripHistory = Convert.IsDBNull(readers["IsDateTripHistory"]) ? false : Convert.ToBoolean(readers["IsDateTripHistory"]),
                    IsShowTaskMenu = Convert.IsDBNull(readers["IsShowTaskMenu"]) ? false : Convert.ToBoolean(readers["IsShowTaskMenu"]),
                    IsShowLeaveMenu = Convert.IsDBNull(readers["IsShowLeaveMenu"]) ? false : Convert.ToBoolean(readers["IsShowLeaveMenu"]),
                    IsShowTaskDateFilter = Convert.IsDBNull(readers["IsShowTaskDateFilter"]) ? false : Convert.ToBoolean(readers["IsShowTaskDateFilter"]),
                    IsMobileAppAccesible = Convert.IsDBNull(readers["IsMobileAppAccesible"]) ? false : Convert.ToBoolean(readers["IsMobileAppAccesible"]),
                    IsActive = Convert.IsDBNull(readers["IsActive"]) ? false : Convert.ToBoolean(readers["IsActive"]),
                    CompanyAdminName = Convert.IsDBNull(readers["CompanyAdminName"]) ? string.Empty : Convert.ToString(readers["CompanyAdminName"]),
                    CompanyAdminEmail = Convert.IsDBNull(readers["CompanyAdminEmail"]) ? string.Empty : Convert.ToString(readers["CompanyAdminEmail"]),
                    CompanyAdminLoginID = Convert.IsDBNull(readers["CompanyAdminLoginID"]) ? string.Empty : Convert.ToString(readers["CompanyAdminLoginID"]),
                    ContactPersonName = Convert.IsDBNull(readers["ContactPersonName"]) ? string.Empty : Convert.ToString(readers["ContactPersonName"]),
                    GeofanceLat = Convert.IsDBNull(readers["GeofanceLat"]) ? string.Empty : Convert.ToString(readers["GeofanceLat"]),
                    GeofanceLng = Convert.IsDBNull(readers["GeofanceLng"]) ? string.Empty : Convert.ToString(readers["GeofanceLng"]),
                    GeofanceTime = Convert.IsDBNull(readers["GeofanceTime"]) ? (int?)null : Convert.ToInt32(readers["GeofanceTime"]),
                    GeofanceRadious = Convert.IsDBNull(readers["GeofanceRadious"]) ? (int?)null : Convert.ToInt32(readers["GeofanceRadious"]),
                    UserLanguage = Convert.IsDBNull(readers["UserLanguage"]) ? string.Empty : Convert.ToString(readers["UserLanguage"]),

                };

                models.Add(model);
            }

            return models;
        }
    }
}