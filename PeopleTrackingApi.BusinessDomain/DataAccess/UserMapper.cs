using System;
using System.Collections.Generic;
using System.Data.Common;
using PeopleTrackingApi.Common.Models;


namespace PeopleTrackingApi.BusinessDomain.DataAccess
{
    public static class UserMapper
    {
        
        public static List<UserCredentialModel> ToUserFullDetails(DbDataReader readers)
        {
            if (readers == null)
                return null;
            var models = new List<UserCredentialModel>();

            while (readers.Read())
            {
                var model = new UserCredentialModel
                {
                    Id = Convert.IsDBNull(readers["Id"]) ? string.Empty : Convert.ToString(readers["Id"]),
                    FullName = Convert.IsDBNull(readers["FullName"]) ? string.Empty : Convert.ToString(readers["FullName"]),
                    Email = Convert.IsDBNull(readers["Email"]) ? string.Empty : Convert.ToString(readers["Email"]),
                    ContactNo = Convert.IsDBNull(readers["ContactNo"]) ? string.Empty : Convert.ToString(readers["ContactNo"]),
                    CreatedAt = Convert.IsDBNull(readers["CreatedAt"]) ? string.Empty : Convert.ToDateTime(readers["CreatedAt"]).ToShortDateString(),
                    UserTypeId = Convert.ToInt32(readers["UserTypeId"]),
                    IsActive = Convert.ToBoolean(readers["IsActive"]),
                    LoginID = Convert.IsDBNull(readers["LoginID"]) ? string.Empty : Convert.ToString(readers["LoginID"]),
                    Password = Convert.IsDBNull(readers["Password"]) ? string.Empty : Convert.ToString(readers["Password"]),
                    CompanyId = Convert.IsDBNull(readers["CompanyId"]) ? 0 : Convert.ToInt32(readers["CompanyId"]),
                    PushToken = Convert.IsDBNull(readers["PushToken"]) ? string.Empty : Convert.ToString(readers["PushToken"])
                };

                models.Add(model);
            }

            return models;
        }

    }
}
