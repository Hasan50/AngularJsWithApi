using ToDoApp.Common;
using ToDoApp.Common.Models;
using System;
using System.Collections.Generic;
using System.Data;
using ToDoApp.DataAccess.Common;
using ToDoApp.BusinessDomain.Interfaces;
using ToDoApp.BusinessDomain.Mappers;
using ToDoApp.BusinessDomain.Models;
using System.Linq;

namespace ToDoApp.BusinessDomain.DataAccess
{
    public class AttendanceDataAccess : BaseDatabaseHandler, IAttendance
    {
        public ResponseModel CheckIn(AttendanceEntryModel model)
        {
            var errMessage = string.Empty;
            var queryParamList = new QueryParamList
                {
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@UserId", ParamValue =model.UserId},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@AttendanceDate", ParamValue =DateTime.UtcNow,DBType=DbType.DateTime},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@CheckInTime", ParamValue = DateTime.UtcNow,DBType=DbType.DateTime},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@DailyWorkingTimeInMin", ParamValue = model.OfficeHour,DBType=DbType.Int32},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@AllowOfficeLessTime", ParamValue = model.AllowOfficeLessTime,DBType=DbType.Int32},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@CheckInTimeFile", ParamValue = model.CheckInTimeFile},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@IsCheckOut", ParamValue =model.IsCheckOut,DBType=DbType.Boolean},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@IsCheckIn", ParamValue =model.IsCheckIn,DBType=DbType.Boolean},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@CompanyId", ParamValue =model.CompanyId,DBType=DbType.Int32},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@agentId", ParamValue =model.CompanyAgentId,DBType=DbType.Int32}
                };


            const string sql = @"IF NOT EXISTS(SELECT * FROM Attendance A WHERE A.UserId=@UserId AND CONVERT(DATE,AttendanceDate)=CONVERT(DATE,@AttendanceDate))
                                BEGIN
								        IF @CheckInTime is not null
								        BEGIN
                                            DECLARE @OfficeEndTime varchar(50)=null
                                            SELECT TOP 1 @OfficeEndTime=u.OfficeEndTime FROM CompanyAgent U WHERE U.Id=@agentId
                                            Update Attendance set CheckOutTime=@OfficeEndTime where UserId=@UserId and CheckOutTime IS NULL
                                            INSERT INTO Attendance(UserId,AttendanceDate,CheckInTime,DailyWorkingTimeInMin,AllowOfficeLessTime,CheckInTimeFile,IsCheckIn,IsCheckOut,CompanyId,CompanyAgentId)
				                            VALUES(@UserId,@AttendanceDate,@CheckInTime,@DailyWorkingTimeInMin,@AllowOfficeLessTime,@CheckInTimeFile,@IsCheckIn,@IsCheckOut,@CompanyId,@agentId)
                                        END
                                END
                                ELSE
							    BEGIN
									DECLARE @id bigint
									SELECT TOP 1 @id=Id FROM Attendance WHERE UserId=@UserId AND IsCheckOut=1 ORDER BY Id DESC
                                    UPDATE Attendance SET CheckInTime=CheckInTime,CheckInTimeFile=@CheckInTimeFile,IsCheckIn=@IsCheckIn,IsCheckOut=@IsCheckOut WHERE Id=@id
								END";
            DBExecCommandEx(sql, queryParamList, ref errMessage);

            return new ResponseModel { Success = string.IsNullOrEmpty(errMessage) };
        }

        public ResponseModel CheckOut(AttendanceEntryModel model)
        {
            var errMessage = string.Empty;
            var queryParamList = new QueryParamList
                {
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@UserId", ParamValue =model.UserId},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@LessTimeReason", ParamValue =model.Reason},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@CheckOutTime", ParamValue = DateTime.UtcNow,DBType=DbType.DateTime},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@CheckOutTimeFile", ParamValue = model.CheckOutTimeFile},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@IsCheckOut", ParamValue =model.IsCheckOut,DBType=DbType.Boolean},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@IsCheckIn", ParamValue =model.IsCheckIn,DBType=DbType.Boolean}
                };

            const string sql = @"DECLARE @id bigint
                                SELECT TOP 1 @id=Id FROM Attendance WHERE UserId=@UserId AND IsCheckIn=1 ORDER BY Id DESC
                                UPDATE Attendance SET CheckOutTime=@CheckOutTime,IsCheckIn=@IsCheckIn,IsCheckOut=@IsCheckOut,LessTimeReason=@LessTimeReason,CheckOutTimeFile=@CheckOutTimeFile WHERE Id=@id";
            DBExecCommandEx(sql, queryParamList, ref errMessage);

            return new ResponseModel { Success = string.IsNullOrEmpty(errMessage) };
        }

        public ResponseModel SaveCheckPoint(UserMovementLogModel model)
        {
            var errMessage = string.Empty;
            var queryParamList = new QueryParamList
                {
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Id", ParamValue =Guid.NewGuid().ToString()},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@UserId", ParamValue =model.UserId},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@LogDateTime", ParamValue =DateTime.UtcNow,DBType=DbType.DateTime2},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Latitude", ParamValue =model.Latitude},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Longitude", ParamValue =model.Longitude},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@LogLocation", ParamValue =model.LogLocation},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@IsCheckInPoint", ParamValue =model.IsCheckInPoint},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@IsCheckOutPoint", ParamValue =model.IsCheckOutPoint},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@DeviceName", ParamValue =model.DeviceName},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@DeviceOSVersion", ParamValue =model.DeviceOSVersion}
                };

            const string sql = @"INSERT INTO UserMovementLog(Id,UserId,LogDateTime,Latitude,Longitude,LogLocation,IsCheckInPoint,
                                    IsCheckOutPoint,DeviceName,DeviceOSVersion)
				                 VALUES(@Id,@UserId,@LogDateTime,@Latitude,@Longitude,@LogLocation,@IsCheckInPoint,
                                    @IsCheckOutPoint,@DeviceName,@DeviceOSVersion)";
            DBExecCommandEx(sql, queryParamList, ref errMessage);

            return new ResponseModel { Success = string.IsNullOrEmpty(errMessage) };
        }

        public ResponseModel SaveCheckPointDetail(UserMovementLogDetailsModel model)
        {
            var errMessage = string.Empty;
            var queryParamList = new QueryParamList
                {
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@UserId", ParamValue =model.UserId},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@LogDateTime", ParamValue =DateTime.UtcNow,DBType=DbType.DateTime2},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Latitude", ParamValue =model.Latitude},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Longitude", ParamValue =model.Longitude},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@LogLocation", ParamValue =model.LogLocation},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@IsCheckInPoint", ParamValue =model.IsCheckInPoint},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@IsCheckOutPoint", ParamValue =model.IsCheckOutPoint},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@DeviceName", ParamValue =model.DeviceName},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@DeviceOSVersion", ParamValue =model.DeviceOSVersion}
                };

            const string sql = @"INSERT INTO UserMovementLogDetails(UserId,LogDateTime,Latitude,Longitude,LogLocation,IsCheckInPoint,
                                    IsCheckOutPoint,DeviceName,DeviceOSVersion)
				                 VALUES(@UserId,@LogDateTime,@Latitude,@Longitude,@LogLocation,@IsCheckInPoint,
                                    @IsCheckOutPoint,@DeviceName,@DeviceOSVersion)";
            DBExecCommandEx(sql, queryParamList, ref errMessage);

            return new ResponseModel { Success = string.IsNullOrEmpty(errMessage) };
        }
        public List<AttendanceModel> GetAttendanceCompanyAgentFeed(string companyAgentId, DateTime date)
        {
             string  sql = @" SELECT c.Id,t.UserId,C.CompanyAgentId,t.AttendanceDate,t.CheckInTime,t.CheckOutTime,t.AllowOfficeLessTime,T.IsLeave,c.EmployeeCode,ca.AgentName,ca.Id as AgentId,ca.OfficeStartTime,ca.OfficeEndTime,t.IsCheckIn,t.IsCheckOut,
                                t.LessTimeReason,t.DailyWorkingTimeInMin,C.Id EmployeeId,t.CheckInTimeFile,t.CheckOutTimeFile
                                ,C.UserName EmployeeName,C.Designation,d.DepartmentName,c.ImageFileName,c.PhoneNumber,null IsAutoCheckPoint,null AutoCheckPointTime, null TotalHours, null TotalMinutes 
                                FROM Attendance t
                                LEFT JOIN EmployeeUser C ON T.UserId=C.UserId 
                                LEFT JOIN UserCredentials CR ON C.UserId=CR.Id
                                left join Department d on c.DepartmentId=d.Id
                                left join CompanyAgent ca on ca.Id=T.CompanyAgentId
                                where convert(date,t.AttendanceDate)=convert(date,@date) and (@CompanyAgentId IS Null Or c.CompanyAgentId=@CompanyAgentId)
                                
                                UNION ALL

                                SELECT t.Id,t.UserId,t.CompanyAgentId,@date AttendanceDate,NULL CheckInTime,NULL CheckOutTime,NULL AllowOfficeLessTime,NULL IsLeave,t.EmployeeCode,null AgentName,null AgentId,null OfficeStartTime,null OfficeEndTime,null IsCheckIn,null IsCheckOut,
                                NULL LessTimeReason,NULL DailyWorkingTimeInMin,t.Id EmployeeId,null CheckInTimeFile,null CheckOutTimeFile
                                ,t.UserName EmployeeName,t.Designation,d.DepartmentName,t.ImageFileName,t.PhoneNumber,null IsAutoCheckPoint,null AutoCheckPointTime, null TotalHours, null TotalMinutes 
                                FROM EmployeeUser t
                                LEFT JOIN UserCredentials CR ON T.UserId=CR.Id
                                left join Department d on t.DepartmentId=d.Id
                                where  t.UserId NOT in (
                                SELECT T.UserId FROM Attendance t
                                where convert(date,t.AttendanceDate)=convert(date,@date)
                                )";
            
            var queryParamList = new QueryParamList
                {
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@date", ParamValue = date,DBType=DbType.DateTime},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@CompanyAgentId", ParamValue = (companyAgentId =="undefined" || string.IsNullOrEmpty(companyAgentId) )?null:companyAgentId}
                };
            var data = ExecuteDBQuery(sql, queryParamList, AttendanceMapper.ToAttendance);
            return data;
        }
        public List<AttendanceModel> GetAttendanceFeed(DateTime date)
        {
            const string sql = @"
                                SELECT c.Id,t.UserId,C.CompanyAgentId,t.AttendanceDate,t.CheckInTime,t.CheckOutTime,t.AllowOfficeLessTime,T.IsLeave,c.EmployeeCode,ca.AgentName,ca.Id as AgentId,ca.OfficeStartTime,ca.OfficeEndTime,t.IsCheckIn,t.IsCheckOut,
                                t.LessTimeReason,t.DailyWorkingTimeInMin,C.Id EmployeeId,t.CheckInTimeFile,t.CheckOutTimeFile
                                ,C.UserName EmployeeName,C.Designation,d.DepartmentName,c.ImageFileName,c.PhoneNumber,null IsAutoCheckPoint,null AutoCheckPointTime, null TotalHours, null TotalMinutes 
                                FROM Attendance t
                                LEFT JOIN EmployeeUser C ON T.UserId=C.UserId 
                                LEFT JOIN UserCredentials CR ON C.UserId=CR.Id
                                left join Department d on c.DepartmentId=d.Id
                                left join CompanyAgent ca on ca.Id=T.CompanyAgentId
                                where convert(date,t.AttendanceDate)=convert(date,@date)

                                UNION ALL

                                SELECT t.Id,t.UserId,t.CompanyAgentId,@date AttendanceDate,NULL CheckInTime,NULL CheckOutTime,NULL AllowOfficeLessTime,NULL IsLeave,t.EmployeeCode,ca.AgentName,ca.Id as AgentId,null OfficeStartTime,null OfficeEndTime,null IsCheckIn,null IsCheckOut,
                                NULL LessTimeReason,NULL DailyWorkingTimeInMin,t.Id EmployeeId,null CheckInTimeFile,null CheckOutTimeFile
                                ,t.UserName EmployeeName,t.Designation,d.DepartmentName,t.ImageFileName,t.PhoneNumber,null IsAutoCheckPoint,null AutoCheckPointTime, null TotalHours, null TotalMinutes 
                                FROM EmployeeUser t
                                LEFT JOIN UserCredentials CR ON T.UserId=CR.Id
                                left join Department d on t.DepartmentId=d.Id
                                left join CompanyAgent ca on ca.Id=t.CompanyAgentId
                                where  t.UserId NOT in (
                                SELECT T.UserId FROM Attendance t
                                where convert(date,t.AttendanceDate)=convert(date,@date)
                                )";
            var queryParamList = new QueryParamList
                {
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@date", ParamValue = date,DBType=DbType.DateTime}
                };
            var data = ExecuteDBQuery(sql, queryParamList, AttendanceMapper.ToAttendance);
            return data;
        }

        public List<AttendanceModel> GetAttendance(DateTime startDate, DateTime endDate)
        {
            const string sql = @"
                                SELECT c.Id,t.UserId,t.AttendanceDate,t.CheckInTime,t.CheckOutTime,t.AllowOfficeLessTime,T.IsLeave,c.EmployeeCode,ca.AgentName,ca.Id as AgentId,ca.OfficeStartTime,ca.OfficeEndTime,t.IsCheckIn,t.IsCheckOut,
                                t.LessTimeReason,t.DailyWorkingTimeInMin,C.Id EmployeeId,t.CheckInTimeFile,t.CheckOutTimeFile
                                ,C.UserName EmployeeName,C.Designation,d.DepartmentName,c.ImageFileName,c.PhoneNumber, null TotalHours, null TotalMinutes, null IsAutoCheckPoint,null AutoCheckPointTime 
                                FROM Attendance t
                                LEFT JOIN EmployeeUser C ON T.UserId=C.UserId 
                                LEFT JOIN UserCredentials CR ON C.UserId=CR.Id
                                left join Department d on c.DepartmentId=d.Id
                                left join CompanyAgent ca on ca.Id=T.CompanyAgentId
                                where  (CONVERT(DATE,AttendanceDate) BETWEEN convert(date,@startDate) AND convert(date,@endDate))";
            var queryParamList = new QueryParamList
                {
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@startDate", ParamValue = startDate,DBType=DbType.DateTime},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@endDate", ParamValue = endDate,DBType=DbType.DateTime}
                };
            var data = ExecuteDBQuery(sql, queryParamList, AttendanceMapper.ToAttendance);
            return data;
        }

        public List<AttendanceModel> GetAttendance(string userId, DateTime startDate, DateTime endDate)
        {
            const string sql = @"SELECT c.Id,t.UserId,t.AttendanceDate,t.CheckInTime,t.CheckOutTime,t.AllowOfficeLessTime,T.IsLeave,c.EmployeeCode,ca.AgentName,ca.Id as AgentId,ca.OfficeStartTime,ca.OfficeEndTime,t.IsCheckIn,t.IsCheckOut,
                                t.LessTimeReason,t.DailyWorkingTimeInMin,C.Id EmployeeId,t.CheckInTimeFile,t.CheckOutTimeFile
                                ,C.UserName EmployeeName,C.Designation,d.DepartmentName,c.ImageFileName,c.PhoneNumber,null IsAutoCheckPoint,null AutoCheckPointTime, null TotalHours, null TotalMinutes 
                                FROM Attendance t
                                LEFT JOIN EmployeeUser C ON T.UserId=C.UserId 
                                LEFT JOIN UserCredentials CR ON C.UserId=CR.Id
                                left join Department d on c.DepartmentId=d.Id
                                left join CompanyAgent ca on ca.Id=T.CompanyAgentId
                                where  T.UserId=@userId and (CONVERT(DATE,AttendanceDate) BETWEEN convert(date,@startDate) AND convert(date,@endDate))";
            var queryParamList = new QueryParamList
                {
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@userId", ParamValue = userId},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@startDate", ParamValue = startDate,DBType=DbType.DateTime},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@endDate", ParamValue = endDate,DBType=DbType.DateTime}
                };
            var data = ExecuteDBQuery(sql, queryParamList, AttendanceMapper.ToAttendance);
            return data;
        }
        public List<AttendanceModel> GetAttendanceWithAgent(string agentId, DateTime startDate, DateTime endDate)
        {
            const string sql = @"SELECT c.Id,t.UserId,t.AttendanceDate,t.CheckInTime,t.CheckOutTime,t.AllowOfficeLessTime,T.IsLeave,c.EmployeeCode,ca.AgentName,ca.Id as AgentId,ca.OfficeStartTime,ca.OfficeEndTime,t.IsCheckIn,t.IsCheckOut,
                                t.LessTimeReason,t.DailyWorkingTimeInMin,C.Id EmployeeId,t.CheckInTimeFile,t.CheckOutTimeFile
                                ,C.UserName EmployeeName,C.Designation,d.DepartmentName,c.ImageFileName,c.PhoneNumber,null IsAutoCheckPoint,null AutoCheckPointTime, null TotalHours, null TotalMinutes 
                                FROM Attendance t
                                LEFT JOIN EmployeeUser C ON T.UserId=C.UserId 
                                LEFT JOIN UserCredentials CR ON C.UserId=CR.Id
                                left join Department d on c.DepartmentId=d.Id
                                left join CompanyAgent ca on ca.Id=T.CompanyAgentId
                                where  ca.Id=@agentId and (CONVERT(DATE,AttendanceDate) BETWEEN convert(date,@startDate) AND convert(date,@endDate))";
            var queryParamList = new QueryParamList
                {
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@agentId", ParamValue = agentId},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@startDate", ParamValue = startDate,DBType=DbType.DateTime},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@endDate", ParamValue = endDate,DBType=DbType.DateTime}
                };
            var data = ExecuteDBQuery(sql, queryParamList, AttendanceMapper.ToAttendance);
            return data;
        }
        public AttendanceModel GetMyTodayAttendance(string userId, DateTime date)
        {
            const string sql = @"IF EXISTS(SELECT TOP 1 * FROM Attendance A WHERE A.UserId=@userId and convert(date,A.AttendanceDate)=convert(date,@date))
                                BEGIN
	                                SELECT T.Id,t.UserId,t.AttendanceDate,t.CheckInTime,t.CheckOutTime,t.AllowOfficeLessTime,T.IsLeave,c.EmployeeCode,ca.AgentName,ca.Id as AgentId,ca.OfficeStartTime,ca.OfficeEndTime,t.IsCheckIn,t.IsCheckOut,
	                                t.LessTimeReason,t. DailyWorkingTimeInMin,C.Id EmployeeId,t.CheckInTimeFile,t.CheckOutTimeFile,ca.GeofanceLat, ca.GeofanceLng,com.GeofanceRadious,com.GeofanceTime
	                     	        ,C.UserName EmployeeName,c.Designation,d.DepartmentName,c.ImageFileName,c.PhoneNumber,c.IsAutoCheckPoint,c.AutoCheckPointTime, null TotalHours, null TotalMinutes 
                                    FROM Attendance t
	                                LEFT JOIN EmployeeUser C ON T.UserId=C.UserId 
	                                LEFT JOIN UserCredentials CR ON C.UserId=CR.Id
	                                left join Department d on c.DepartmentId=d.Id
                                    left join CompanyAgent ca on ca.Id=T.CompanyAgentId
									left join Company com on com.Id=c.CompanyId
	                                where t.UserId=@userId and convert(date,t.AttendanceDate)=convert(date,@date)
                                END
                                ELSE
                                BEGIN
                                  SELECT T.Id,t.UserId,@date AttendanceDate,NULL CheckInTime,NULL CheckOutTime,NULL AllowOfficeLessTime,NULL IsLeave,t.EmployeeCode,null AgentName,null AgentId,null OfficeStartTime,null OfficeEndTime,null IsCheckIn,null IsCheckOut,
                                    NULL LessTimeReason,NULL DailyWorkingTimeInMin,t.Id EmployeeId,null CheckInTimeFile,null CheckOutTimeFile, null GeofanceLat, null GeofanceLng,com.GeofanceRadious,com.GeofanceTime
                                    ,t.UserName EmployeeName,t.Designation,d.DepartmentName,t.ImageFileName,t.PhoneNumber,t.IsAutoCheckPoint,t.AutoCheckPointTime, null TotalHours, null TotalMinutes
                                    FROM EmployeeUser t
                                    LEFT JOIN UserCredentials CR ON T.UserId=CR.Id
                                    left join Department d on t.DepartmentId=d.Id

									left join Company com on com.Id=t.CompanyId
                                    where t.UserId=@userId
                                END";
            var queryParamList = new QueryParamList
                {
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@userId", ParamValue = userId},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@date", ParamValue = date,DBType=DbType.DateTime}
                };
            var data = ExecuteDBQuery(sql, queryParamList, AttendanceMapper.ToAttendanceToday);
            return (data != null && data.Count > 0) ? data.FirstOrDefault() : null;
        }

        public List<UserMovementLogModel> GetMovementDetails(string userId, DateTime date)
        {
            const string sql = @"SELECT T.*,c.FullName UserName FROM UserMovementLog t
                                LEFT JOIN UserCredentials c on t.UserId=c.Id
                                where t.UserId=@userId and convert(date,t.LogDateTime)=convert(date,@date)";
            var queryParamList = new QueryParamList
                {
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@userId", ParamValue = userId},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@date", ParamValue = date,DBType=DbType.DateTime}
                };
            var data = ExecuteDBQuery(sql, queryParamList, AttendanceMapper.ToMovementLog);
            return data;
        }
        public List<UserMovementLogModel> GetMovementDetailsAll(DateTime date)
        {
            const string sql = @"select a.*,e.UserName from UserMovementLog a
                                right join (select UserId,max(LogDateTime)LogDateTime from UserMovementLog  where convert(date,LogDateTime)=convert(date,@date) group by UserId)  t on t.UserId=a.UserId and t.LogDateTime=a.LogDateTime
                                left join EmployeeUser e on a.UserId=e.UserId";
            var queryParamList = new QueryParamList
                {
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@date", ParamValue = date,DBType=DbType.DateTime}
                };
            var data = ExecuteDBQuery(sql, queryParamList, AttendanceMapper.ToMovementLogAll);
            return data;
        }
    }
}
