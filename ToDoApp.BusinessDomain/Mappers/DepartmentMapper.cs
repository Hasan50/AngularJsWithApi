using System;
using System.Collections.Generic;
using System.Data.Common;
using ToDoApp.BusinessDomain.Models;

namespace ToDoApp.BusinessDomain.Mappers
{
    public static class DepartmentMapper
    {

        public static List<Department> ToDepartmentModel(DbDataReader readers)
        {
            if (readers == null)
                return null;
            var models = new List<Department>();

            while (readers.Read())
            {
                var model = new Department
                {
                    Id = Convert.ToInt32(readers["Id"]),
                    DepartmentName = Convert.IsDBNull(readers["DepartmentName"]) ? string.Empty : Convert.ToString(readers["DepartmentName"]),
                };

                models.Add(model);
            }

            return models;
        }
    }
}