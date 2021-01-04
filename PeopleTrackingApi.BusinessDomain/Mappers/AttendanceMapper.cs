using System;
using System.Collections.Generic;
using System.Data.Common;
using PeopleTrackingApi.BusinessDomain.Models;

namespace PeopleTrackingApi.BusinessDomain.Mappers
{
   public class AttendanceMapper
    {
        public static List<AttendanceModel> ToAttendance(DbDataReader readers)
        {
            if (readers == null)
                return null;
            var models = new List<AttendanceModel>();

            while (readers.Read())
            {
                var model = new AttendanceModel
                {
                   // CompanyAgentId = Convert.ToInt32(readers["CompanyAgentId"]),
                    EmployeeId = Convert.IsDBNull(readers["EmployeeId"]) ? (int?)null : Convert.ToInt32(readers["EmployeeId"]),
                    UserId = Convert.IsDBNull(readers["UserId"]) ? string.Empty : Convert.ToString(readers["UserId"]),
                    EmployeeName = Convert.IsDBNull(readers["EmployeeName"]) ? string.Empty : Convert.ToString(readers["EmployeeName"]),
                    PhoneNumber = Convert.IsDBNull(readers["PhoneNumber"]) ? string.Empty : Convert.ToString(readers["PhoneNumber"]),
                    AttendanceDate = Convert.IsDBNull(readers["AttendanceDate"]) ? (DateTime?)null : Convert.ToDateTime(readers["AttendanceDate"]),
                    CheckInTime = Convert.IsDBNull(readers["CheckInTime"]) ? (DateTime?)null : Convert.ToDateTime(readers["CheckInTime"]),
                    CheckOutTime = Convert.IsDBNull(readers["CheckOutTime"]) ? (DateTime?)null : Convert.ToDateTime(readers["CheckOutTime"]),
                    LessTimeReason = Convert.IsDBNull(readers["LessTimeReason"]) ? string.Empty : Convert.ToString(readers["LessTimeReason"]),
                    DepartmentName = Convert.IsDBNull(readers["DepartmentName"]) ? string.Empty : Convert.ToString(readers["DepartmentName"]),
                    Designation = Convert.IsDBNull(readers["Designation"]) ? string.Empty : Convert.ToString(readers["Designation"]),
                    ImageFileName = Convert.IsDBNull(readers["ImageFileName"]) ? string.Empty : Convert.ToString(readers["ImageFileName"]),
                    DailyWorkingTimeInMin = Convert.IsDBNull(readers["DailyWorkingTimeInMin"]) ? (int?)null: Convert.ToInt32(readers["DailyWorkingTimeInMin"]),
                    OfficeStartTime = Convert.IsDBNull(readers["OfficeStartTime"]) ? string.Empty : Convert.ToString(readers["OfficeStartTime"]),
                    OfficeEndTime = Convert.IsDBNull(readers["OfficeEndTime"]) ? string.Empty : Convert.ToString(readers["OfficeEndTime"]),
                    AllowOfficeLessTimeInMin = Convert.IsDBNull(readers["AllowOfficeLessTime"]) ? (int?)null : Convert.ToInt32(readers["AllowOfficeLessTime"]),
                    IsLeave = Convert.IsDBNull(readers["IsLeave"]) ? (bool?)null : Convert.ToBoolean(readers["IsLeave"]),
                    IsCheckIn = Convert.IsDBNull(readers["IsCheckIn"]) ? (bool?)null : Convert.ToBoolean(readers["IsCheckIn"]),
                    IsCheckOut = Convert.IsDBNull(readers["IsCheckOut"]) ? (bool?)null : Convert.ToBoolean(readers["IsCheckOut"]),
                    IsAutoCheckPoint = Convert.IsDBNull(readers["IsAutoCheckPoint"]) ? (bool?)null : Convert.ToBoolean(readers["IsAutoCheckPoint"]),
                    AutoCheckPointTime = Convert.IsDBNull(readers["AutoCheckPointTime"]) ? string.Empty : Convert.ToString(readers["AutoCheckPointTime"]),

                    CheckInTimeFile = Convert.IsDBNull(readers["CheckInTimeFile"]) ? string.Empty : Convert.ToString(readers["CheckInTimeFile"]),
                    CheckOutTimeFile = Convert.IsDBNull(readers["CheckOutTimeFile"]) ? string.Empty : Convert.ToString(readers["CheckOutTimeFile"]),
                    EmployeeCode = Convert.IsDBNull(readers["EmployeeCode"]) ? string.Empty : Convert.ToString(readers["EmployeeCode"]),
                    AgentName = Convert.IsDBNull(readers["AgentName"]) ? string.Empty : Convert.ToString(readers["AgentName"]),
                    AgentId = Convert.IsDBNull(readers["AgentId"]) ? (int?)null : Convert.ToInt32(readers["AgentId"]),

                };

                models.Add(model);
            }

            return models;
        }
        public static List<AttendanceModel> ToAttendanceToday(DbDataReader readers)
        {
            if (readers == null)
                return null;
            var models = new List<AttendanceModel>();

            while (readers.Read())
            {
                var model = new AttendanceModel
                {
                    // CompanyAgentId = Convert.ToInt32(readers["CompanyAgentId"]),
                    EmployeeId = Convert.IsDBNull(readers["EmployeeId"]) ? (int?)null : Convert.ToInt32(readers["EmployeeId"]),
                    UserId = Convert.IsDBNull(readers["UserId"]) ? string.Empty : Convert.ToString(readers["UserId"]),
                    EmployeeName = Convert.IsDBNull(readers["EmployeeName"]) ? string.Empty : Convert.ToString(readers["EmployeeName"]),
                    PhoneNumber = Convert.IsDBNull(readers["PhoneNumber"]) ? string.Empty : Convert.ToString(readers["PhoneNumber"]),
                    OfficeStartTime = Convert.IsDBNull(readers["OfficeStartTime"]) ? string.Empty : Convert.ToString(readers["OfficeStartTime"]),
                    OfficeEndTime = Convert.IsDBNull(readers["OfficeEndTime"]) ? string.Empty : Convert.ToString(readers["OfficeEndTime"]),
                    AttendanceDate = Convert.IsDBNull(readers["AttendanceDate"]) ? (DateTime?)null : Convert.ToDateTime(readers["AttendanceDate"]),
                    CheckInTime = Convert.IsDBNull(readers["CheckInTime"]) ? (DateTime?)null : Convert.ToDateTime(readers["CheckInTime"]),
                    CheckOutTime = Convert.IsDBNull(readers["CheckOutTime"]) ? (DateTime?)null : Convert.ToDateTime(readers["CheckOutTime"]),
                    LessTimeReason = Convert.IsDBNull(readers["LessTimeReason"]) ? string.Empty : Convert.ToString(readers["LessTimeReason"]),
                    DepartmentName = Convert.IsDBNull(readers["DepartmentName"]) ? string.Empty : Convert.ToString(readers["DepartmentName"]),
                    Designation = Convert.IsDBNull(readers["Designation"]) ? string.Empty : Convert.ToString(readers["Designation"]),
                    ImageFileName = Convert.IsDBNull(readers["ImageFileName"]) ? string.Empty : Convert.ToString(readers["ImageFileName"]),
                    DailyWorkingTimeInMin = Convert.IsDBNull(readers["DailyWorkingTimeInMin"]) ? (int?)null : Convert.ToInt32(readers["DailyWorkingTimeInMin"]),

                    AllowOfficeLessTimeInMin = Convert.IsDBNull(readers["AllowOfficeLessTime"]) ? (int?)null : Convert.ToInt32(readers["AllowOfficeLessTime"]),
                    IsLeave = Convert.IsDBNull(readers["IsLeave"]) ? (bool?)null : Convert.ToBoolean(readers["IsLeave"]),
                    IsCheckIn = Convert.IsDBNull(readers["IsCheckIn"]) ? (bool?)null : Convert.ToBoolean(readers["IsCheckIn"]),
                    IsCheckOut = Convert.IsDBNull(readers["IsCheckOut"]) ? (bool?)null : Convert.ToBoolean(readers["IsCheckOut"]),

                    IsAutoCheckPoint = Convert.IsDBNull(readers["IsAutoCheckPoint"]) ? (bool?)null : Convert.ToBoolean(readers["IsAutoCheckPoint"]),
                    AutoCheckPointTime = Convert.IsDBNull(readers["AutoCheckPointTime"]) ? string.Empty : Convert.ToString(readers["AutoCheckPointTime"]),

                    CheckInTimeFile = Convert.IsDBNull(readers["CheckInTimeFile"]) ? string.Empty : Convert.ToString(readers["CheckInTimeFile"]),
                    CheckOutTimeFile = Convert.IsDBNull(readers["CheckOutTimeFile"]) ? string.Empty : Convert.ToString(readers["CheckOutTimeFile"]),
                    EmployeeCode = Convert.IsDBNull(readers["EmployeeCode"]) ? string.Empty : Convert.ToString(readers["EmployeeCode"]),
                    AgentName = Convert.IsDBNull(readers["AgentName"]) ? string.Empty : Convert.ToString(readers["AgentName"]),
                    AgentId = Convert.IsDBNull(readers["AgentId"]) ? (int?)null : Convert.ToInt32(readers["AgentId"]),
                    GeofanceLat = Convert.IsDBNull(readers["GeofanceLat"]) ? string.Empty : Convert.ToString(readers["GeofanceLat"]),
                    GeofanceLng = Convert.IsDBNull(readers["GeofanceLng"]) ? string.Empty : Convert.ToString(readers["GeofanceLng"]),
                    GeofanceTime = Convert.IsDBNull(readers["GeofanceTime"]) ? (int?)null : Convert.ToInt32(readers["GeofanceTime"]),
                    GeofanceRadious = Convert.IsDBNull(readers["GeofanceRadious"]) ? (int?)null : Convert.ToInt32(readers["GeofanceRadious"]),

                };

                models.Add(model);
            }

            return models;
        }
        public static List<UserMovementLogModel> ToMovementLog(DbDataReader readers)
        {
            if (readers == null)
                return null;
            var models = new List<UserMovementLogModel>();

            while (readers.Read())
            {
                var model = new UserMovementLogModel
                {
                    Id = Convert.IsDBNull(readers["Id"]) ? string.Empty : Convert.ToString(readers["Id"]),
                    UserId = Convert.IsDBNull(readers["UserId"]) ? string.Empty : Convert.ToString(readers["UserId"]),
                    UserName = Convert.IsDBNull(readers["UserName"]) ? string.Empty : Convert.ToString(readers["UserName"]),
                    LogDateTime = Convert.IsDBNull(readers["LogDateTime"]) ? (DateTime?)null : Convert.ToDateTime(readers["LogDateTime"]),
                    Latitude = Convert.IsDBNull(readers["Latitude"]) ? (decimal?)null : Convert.ToDecimal(readers["Latitude"]),
                    Longitude = Convert.IsDBNull(readers["Longitude"]) ? (decimal?)null : Convert.ToDecimal(readers["Longitude"]),
                    LogLocation = Convert.IsDBNull(readers["LogLocation"]) ? string.Empty : Convert.ToString(readers["LogLocation"]),
                    DeviceName = Convert.IsDBNull(readers["DeviceName"]) ? string.Empty : Convert.ToString(readers["DeviceName"]),
                    DeviceOSVersion = Convert.IsDBNull(readers["DeviceOSVersion"]) ? string.Empty : Convert.ToString(readers["DeviceOSVersion"]),
                    IsCheckInPoint = Convert.IsDBNull(readers["IsCheckInPoint"]) ? (bool?)null : Convert.ToBoolean(readers["IsCheckInPoint"]),
                    IsCheckOutPoint = Convert.IsDBNull(readers["IsCheckOutPoint"]) ? (bool?)null : Convert.ToBoolean(readers["IsCheckOutPoint"]),
                };

                models.Add(model);
            }

            return models;
        }
        public static List<UserMovementLogModel> ToMovementLogAll(DbDataReader readers)
        {
            if (readers == null)
                return null;
            var models = new List<UserMovementLogModel>();

            while (readers.Read())
            {
                var model = new UserMovementLogModel
                {
                    Id = Convert.IsDBNull(readers["Id"]) ? string.Empty : Convert.ToString(readers["Id"]),
                    UserId = Convert.IsDBNull(readers["UserId"]) ? string.Empty : Convert.ToString(readers["UserId"]),
                    UserName = Convert.IsDBNull(readers["UserName"]) ? string.Empty : Convert.ToString(readers["UserName"]),
                    LogDateTime = Convert.IsDBNull(readers["LogDateTime"]) ? (DateTime?)null : Convert.ToDateTime(readers["LogDateTime"]),
                    Latitude = Convert.IsDBNull(readers["Latitude"]) ? (decimal?)null : Convert.ToDecimal(readers["Latitude"]),
                    Longitude = Convert.IsDBNull(readers["Longitude"]) ? (decimal?)null : Convert.ToDecimal(readers["Longitude"]),
                    LogLocation = Convert.IsDBNull(readers["LogLocation"]) ? string.Empty : Convert.ToString(readers["LogLocation"]),
                    DeviceName = Convert.IsDBNull(readers["DeviceName"]) ? string.Empty : Convert.ToString(readers["DeviceName"]),
                    DeviceOSVersion = Convert.IsDBNull(readers["DeviceOSVersion"]) ? string.Empty : Convert.ToString(readers["DeviceOSVersion"]),
                    IsCheckInPoint = Convert.IsDBNull(readers["IsCheckInPoint"]) ? (bool?)null : Convert.ToBoolean(readers["IsCheckInPoint"]),
                    IsCheckOutPoint = Convert.IsDBNull(readers["IsCheckOutPoint"]) ? (bool?)null : Convert.ToBoolean(readers["IsCheckOutPoint"]),
                };

                models.Add(model);
            }

            return models;
        }
    }
}
