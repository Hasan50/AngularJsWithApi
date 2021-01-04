using System;
using System.Collections.Generic;
using System.Data.Common;
using PeopleTrackingApi.BusinessDomain.Models;

namespace PeopleTrackingApi.BusinessDomain.Mappers
{
    public static class SickTypeMapper
    {

        public static List<SickType> ToSickTypeModel(DbDataReader readers)
        {
            if (readers == null)
                return null;
            var models = new List<SickType>();

            while (readers.Read())
            {
                var model = new SickType
                {
                    Id = Convert.ToInt32(readers["Id"]),
                    CompanyId = Convert.ToInt32(readers["CompanyId"]),
                    CompanyName = Convert.IsDBNull(readers["CompanyName"]) ? string.Empty : Convert.ToString(readers["CompanyName"]),
                    Name = Convert.IsDBNull(readers["Name"]) ? string.Empty : Convert.ToString(readers["Name"]),
                };

                models.Add(model);
            }

            return models;
        }
    }
}