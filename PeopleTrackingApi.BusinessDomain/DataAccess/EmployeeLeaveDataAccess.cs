using PeopleTrackingApi.Common;
using PeopleTrackingApi.Common.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using PeopleTrackingApi.DataAccess.Common;
using PeopleTrackingApi.BusinessDomain.Interfaces;
using PeopleTrackingApi.BusinessDomain.Mappers;
using PeopleTrackingApi.BusinessDomain.Models;

namespace PeopleTrackingApi.BusinessDomain.DataAccess
{
    public class EmployeeLeaveDataAccess : BaseDatabaseHandler, IEmployeeLeave
    {
        public EmployeeLeaveDataAccess() { }
        public List<EmployeeLeaveModel> GetLeaveWithFilter(DateTime fromDate,DateTime toDate,string userName,int companyId)
        {
            string err = string.Empty;
            string sql = "";
            if (userName !="null" && userName != "")
            {
                 sql = @"SELECT la.*,st.Name SickType ,eu.UserId,uc.FullName as EmployeeName,AP.FullName ApprovedBy from LeaveApplication as la
                            Left JOIN SickType st on la.SickTypeId=st.Id                            
                            Left JOIN EmployeeUser eu on eu.id= la.EmployeeId
                            Left JOIN UserCredentials uc on uc.id= eu.UserId
                            LEFT JOIN UserCredentials AP ON LA.ApprovedById=AP.Id
                            WHERE la.CompanyId =@CompanyId and (CONVERT(DATE,la.FromDate) BETWEEN convert(date,@FromDate) AND convert(date,@ToDate)) and eu.UserName like'%@userName'
                            order by la.Id desc";
            }
            else
            {
                 sql = @"SELECT la.*,st.Name SickType ,eu.UserId,uc.FullName as EmployeeName,AP.FullName ApprovedBy from LeaveApplication as la
                            Left JOIN SickType st on la.SickTypeId=st.Id                            
                            Left JOIN EmployeeUser eu on eu.id= la.EmployeeId
                            Left JOIN UserCredentials uc on uc.id= eu.UserId
                            LEFT JOIN UserCredentials AP ON LA.ApprovedById=AP.Id
                            WHERE la.CompanyId =@CompanyId and (CONVERT(DATE,la.FromDate) BETWEEN convert(date,@FromDate) AND convert(date,@ToDate))
                            order by la.Id desc";
            }

            var queryParamList = new QueryParamList
                    {
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@CompanyId", ParamValue = companyId},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@FromDate", ParamValue = fromDate.ToString(Constants.ServerDateTimeFormat), DBType=DbType.DateTime},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@ToDate", ParamValue = toDate.ToString(Constants.ServerDateTimeFormat), DBType=DbType.DateTime},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@userName", ParamValue = userName},
                    };
            var results = ExecuteDBQuery(sql, queryParamList, EmployeeLeaveMapper.ToEmployeeLeaveMapperModel);
            return results;
        }
        public List<EmployeeLeaveModel> GetLeave(int companyId)
        {
            string err = string.Empty;
            string sql = @"SELECT la.*,st.Name SickType ,eu.UserId,uc.FullName as EmployeeName,AP.FullName ApprovedBy from LeaveApplication as la
                            Left JOIN SickType st on la.SickTypeId=st.Id                            
                            Left JOIN EmployeeUser eu on eu.id= la.EmployeeId
                            Left JOIN UserCredentials uc on uc.id= eu.UserId
                            LEFT JOIN UserCredentials AP ON LA.ApprovedById=AP.Id
                            where  la.CompanyId ='" + companyId + "'  order by la.Id desc";
            var results = ExecuteDBQuery(sql, null, EmployeeLeaveMapper.ToEmployeeLeaveMapperModel);
            return results.Any() ? results : new List<EmployeeLeaveModel>();
        }

        public List<EmployeeLeaveModel> GetUserLeaves(string userId)
        {
            string err = string.Empty;
            string sql = @"SELECT la.*,st.Name SickType,eu.UserId,uc.FullName as EmployeeName,AP.FullName ApprovedBy from LeaveApplication as la
                            Left JOIN SickType st on la.SickTypeId=st.Id                            
                            Left JOIN EmployeeUser eu on eu.id= la.EmployeeId
                            Left JOIN UserCredentials uc on uc.id= eu.UserId
                            LEFT JOIN UserCredentials AP ON LA.ApprovedById=AP.Id
                            where  eu.UserId ='" + userId + "'  order by la.Id desc";
            var results = ExecuteDBQuery(sql, null, EmployeeLeaveMapper.ToEmployeeLeaveMapperModel);
            return results.Any() ? results : new List<EmployeeLeaveModel>();
        }

        public ResponseModel CreateEmployeeLeave(EmployeeLeaveModel model)
        {
            var errMessage = string.Empty;
            var queryParamList = new QueryParamList
                    {
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@userId", ParamValue = model.UserId},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@CompanyId", ParamValue = model.CompanyId},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@FromDate", ParamValue = model.FromDate.ToString(Constants.ServerDateTimeFormat), DBType=DbType.DateTime},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@ToDate", ParamValue = model.ToDate.ToString(Constants.ServerDateTimeFormat), DBType=DbType.DateTime},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@IsHalfDay", ParamValue =model.IsHalfDay, DBType=DbType.Boolean},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@LeaveTypeId", ParamValue =model.LeaveTypeId, DBType = DbType.Int32},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@SickTypeId", ParamValue =model.SickTypeId, DBType = DbType.Int32},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@LeaveReason", ParamValue = model.LeaveReason},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@CreatedAt", ParamValue =  Convert.ToDateTime(model.CreatedAt),DBType = DbType.DateTime},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@IsApproved", ParamValue = model.IsApproved, DBType=DbType.Boolean},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@IsRejected", ParamValue =model.IsRejected, DBType=DbType.Boolean},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@RejectReason", ParamValue =model.RejectReason},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@ApprovedById", ParamValue =model.ApprovedById},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@ApprovedAt", ParamValue =model.ApprovedAt, DBType = DbType.DateTime},
                    };
            const string sql = @"
                            DECLARE @employeeId INT
                            SELECT TOP 1 @employeeId=U.Id FROM EmployeeUser U WHERE U.UserId=@userId
                            INSERT INTO LeaveApplication (EmployeeId ,CompanyId,FromDate ,ToDate ,IsHalfDay,LeaveTypeId,SickTypeId ,LeaveReason,CreatedAt,IsApproved,IsRejected,RejectReason,ApprovedById,ApprovedAt) 
                            VALUES (@employeeId,@CompanyId ,@FromDate ,@ToDate ,@IsHalfDay,@LeaveTypeId,@SickTypeId ,@LeaveReason,@CreatedAt,@IsApproved,@IsRejected,@RejectReason,@ApprovedById,@ApprovedAt)";
            DBExecCommandEx(sql, queryParamList, ref errMessage);
            return new ResponseModel { Success = string.IsNullOrEmpty(errMessage), Message = errMessage };
        }


        public List<EmployeeLeaveModel> GetLeaveById(int id)
        {
            string err = string.Empty;
            string sql = @"SELECT la.*,st.Name SickType,eu.UserId,uc.FullName as EmployeeName,AP.FullName ApprovedBy from LeaveApplication as la
                            Left JOIN SickType st on la.SickTypeId=st.Id 
                            Left JOIN EmployeeUser eu on eu.id= la.EmployeeId
                            Left JOIN UserCredentials uc on uc.id= eu.UserId
                            LEFT JOIN UserCredentials AP ON LA.ApprovedById=AP.Id
                            where la.Id ='" + id + "'";
            var results = ExecuteDBQuery(sql, null, EmployeeLeaveMapper.ToEmployeeLeaveMapperModel);
            return results.Any() ? results : null;
        }

        public ResponseModel Approved(int id,string userId)
        {
            var errMessage = string.Empty;
            var queryParamList = new QueryParamList
                {
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Id", ParamValue =id},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@ApprovedById", ParamValue =userId},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@ApprovedAt", ParamValue =DateTime.UtcNow,DBType=DbType.DateTime},
                };

            const string sql = @"UPDATE LeaveApplication SET IsApproved=1,IsRejected=0,ApprovedById=@ApprovedById,ApprovedAt=@ApprovedAt WHERE Id=@Id";
            DBExecCommandEx(sql, queryParamList, ref errMessage);
            return new ResponseModel { Success = string.IsNullOrEmpty(errMessage) };
        }

        public ResponseModel Rejected(int id)
        {
            var errMessage = string.Empty;
            var queryParamList = new QueryParamList
                {
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Id", ParamValue =id},
                };

            const string sql = @"UPDATE LeaveApplication SET IsRejected=1,IsApproved=0 WHERE Id=@Id";
            DBExecCommandEx(sql, queryParamList, ref errMessage);
            return new ResponseModel { Success = string.IsNullOrEmpty(errMessage) };
        }
    }
}