﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using ToDoApp.BusinessDomain.Models;

namespace ToDoApp.BusinessDomain.Mappers
{
    public static class EmployeeLeaveMapper
    {

        public static List<EmployeeLeaveModel> ToEmployeeLeaveMapperModel(DbDataReader readers)
        {
            if (readers == null)
                return null;
            var models = new List<EmployeeLeaveModel>();

            while (readers.Read())
            {
                var model = new EmployeeLeaveModel
                {
                    Id = Convert.ToInt32(readers["Id"]),
                    UserId = Convert.ToString(readers["UserId"]),
                    EmployeeId = Convert.ToInt32(readers["EmployeeId"]),
                    FromDate = Convert.ToDateTime(readers["FromDate"]),
                    ToDate = Convert.ToDateTime(readers["ToDate"]),
                    IsHalfDay = Convert.IsDBNull(readers["IsHalfDay"]) ? (bool?)null : Convert.ToBoolean(readers["IsHalfDay"]),
                    SickTypeId = Convert.IsDBNull(readers["SickTypeId"]) ? (int?)null : Convert.ToInt32(readers["SickTypeId"]),
                    LeaveTypeId = Convert.ToInt32(readers["LeaveTypeId"]),
                    SickType = Convert.IsDBNull(readers["SickType"]) ? string.Empty : Convert.ToString(readers["SickType"]),
                    LeaveReason = Convert.IsDBNull(readers["LeaveReason"]) ? string.Empty : Convert.ToString(readers["LeaveReason"]),
                    CreatedAt = Convert.IsDBNull(readers["CreatedAt"]) ? string.Empty : Convert.ToDateTime(readers["CreatedAt"]).ToString(),
                    IsApproved = Convert.IsDBNull(readers["IsApproved"]) ? false : Convert.ToBoolean(readers["IsApproved"]),
                    IsRejected = Convert.IsDBNull(readers["IsRejected"]) ?false : Convert.ToBoolean(readers["IsRejected"]),
                    ApprovedById = Convert.IsDBNull(readers["ApprovedById"]) ? string.Empty : Convert.ToString(readers["ApprovedById"]).ToString(),
                    ApprovedAt = Convert.IsDBNull(readers["ApprovedAt"]) ? (DateTime?)null : Convert.ToDateTime(readers["ApprovedAt"]),
                    EmployeeName = Convert.IsDBNull(readers["EmployeeName"]) ? string.Empty : Convert.ToString(readers["EmployeeName"]),
                    ApprovedBy = Convert.IsDBNull(readers["ApprovedBy"]) ? string.Empty : Convert.ToString(readers["ApprovedBy"]),

                };

                models.Add(model);
            }

            return models;
        }
    }
}