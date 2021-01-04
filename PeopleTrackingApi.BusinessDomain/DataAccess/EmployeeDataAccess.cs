using PeopleTrackingApi.Common;
using PeopleTrackingApi.Common.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using PeopleTrackingApi.DataAccess.Common;
using PeopleTrackingApi.BusinessDomain.Interfaces;
using PeopleTrackingApi.BusinessDomain.Mappers;
using PeopleTrackingApi.BusinessDomain.Models;
using System;

namespace PeopleTrackingApi.BusinessDomain.DataAccess
{
    public class EmployeeDataAccess : BaseDatabaseHandler, IEmployee
    {

        public EmployeeUser GetEmployeeById(int id)
        {
            string err = string.Empty;
            string sql = @"SELECT E.*,D.DepartmentName from EmployeeUser e
                            LEFT JOIN Department D ON E.DepartmentId=D.Id where E.Id=@id";
            var queryParamList = new QueryParamList
                    {
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@id", ParamValue =id},
                    };
            var results = ExecuteDBQuery(sql, queryParamList, EmployeeMapper.ToEmployeeModel);
            return results.Any() ? results.FirstOrDefault() : null;
        }

        public List<EmployeeUser> GetEmployee()
        {
            string err = string.Empty;
            string sql = @"SELECT E.*,U.Email,null DepartmentName,co.CompanyName from EmployeeUser e
                          Left JOIN UserCredentials U ON E.UserId=U.Id                         
                            LEFT JOIN Company co on E.CompanyId=co.Id
                           -- where e.CompanyId=@CompanyId
                            ";

            var results = ExecuteDBQuery(sql, null, EmployeeMapper.ToEmployeeModel);
            return results;
        }
        public List<EmployeeCompanyAgent> GetEmployeeAgent(int empId)
        {
            string err = string.Empty;
            string sql = @"SELECT e.*,g.AgentName from EmployeeCompanyAgent e left join CompanyAgent g on e.CompanyAgentId=g.Id where e.EmployeeUserId =@empId ";
            var queryParamList = new QueryParamList
                    {
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@empId", ParamValue =empId},
                    };
            var results = ExecuteDBQuery(sql, queryParamList, EmployeeMapper.ToEmployeeCompanyAgentModel);
            return results;
        }
        public EmployeeUser GetByPortalUserId(string userId)
        {
            string err = string.Empty;
            string sql = @"SELECT E.*,U.Email,null DepartmentName,co.CompanyName from EmployeeUser e
                          Left JOIN UserCredentials U ON E.UserId=U.Id                         
                            LEFT JOIN Company co on E.CompanyId=co.Id where E.UserId=@userId";
            var queryParamList = new QueryParamList
                    {
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@userId", ParamValue =userId},
                    };
            var data = ExecuteDBQuery(sql, queryParamList, EmployeeMapper.ToEmployeeModel);
            return (data != null && data.Count > 0) ? data.FirstOrDefault() : null;
        }

        public EmployeeUser Create(EmployeeUser model)
        {
            var errMessage = string.Empty;
            var queryParamList = new QueryParamList
                    {
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@UserName", ParamValue =model.UserName},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Designation", ParamValue =model.Designation},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@PhoneNumber", ParamValue =model.PhoneNumber},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@ImageFileName", ParamValue =model.ImageFileName},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@ImageFileId", ParamValue =model.ImageFileId},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@UserId", ParamValue =model.UserId},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@CompanyId", ParamValue =model.CompanyId},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@EmployeeCode", ParamValue =model.EmployeeCode},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Gender", ParamValue =model.Gender},

                    };
            const string sql = @"INSERT INTO [EmployeeUser] (EmployeeCode,[UserName] ,[Designation] , [PhoneNumber], [ImageFileName],[ImageFileId],
                                [UserId],[CompanyId],IsActive,Gender) VALUES (@EmployeeCode,@UserName, @Designation, @PhoneNumber, @ImageFileName, @ImageFileId,@UserId,@CompanyId,1,@Gender)";

            DBExecCommandEx(sql, queryParamList, ref errMessage);

            return model;
        }
        public EmployeeCompanyAgent CreateEmpAgent(EmployeeCompanyAgent model)
        {
            var errMessage = string.Empty;
            var queryParamList = new QueryParamList
                    {
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@EmployeeUserId", ParamValue =model.EmployeeUserId},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@CompanyAgentId", ParamValue =model.CompanyAgentId},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@CreatedDate", ParamValue = DateTime.UtcNow,DBType=DbType.DateTime},

                    };
            const string sql = @"IF NOT EXISTS(SELECT TOP 1 * FROM EmployeeCompanyAgent C WHERE C.EmployeeUserId=@EmployeeUserId AND C.CompanyAgentId=@CompanyAgentId)
BEGIN                                    
INSERT INTO [EmployeeCompanyAgent] ([EmployeeUserId],[CompanyAgentId],CreatedDate ) VALUES (@EmployeeUserId,@CompanyAgentId,@CreatedDate)
                                    END";

            DBExecCommandEx(sql, queryParamList, ref errMessage);
            return model;
        }
        public ResponseModel Delete(string id)
        {
            var errMessage = string.Empty;
            var queryParamList = new QueryParamList
                {
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@UserId", ParamValue =id},
                };

            const string sql = @"delete from UserMovementLog where UserId=@UserId
                                delete from EmployeeUser where UserId=@UserId
                                delete from EmployeeCompanyAgent where EmployeeUserId In (select Id from EmployeeUser where UserId=@UserId)
                                delete from UserCredentials where id=@UserId";
            DBExecCommandEx(sql, queryParamList, ref errMessage);

            return new ResponseModel { Success = string.IsNullOrEmpty(errMessage) };

        }
        public ResponseModel DeleteEmployeeAgent(int companyAgentId,int employeeId)
        {
            var errMessage = string.Empty;
            var queryParamList = new QueryParamList
                {
                         new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@EmployeeUserId", ParamValue =employeeId},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@CompanyAgentId", ParamValue =companyAgentId},
                };

            const string sql = @"IF EXISTS(SELECT TOP 1 * FROM EmployeeCompanyAgent C WHERE C.EmployeeUserId=@EmployeeUserId AND C.CompanyAgentId=@CompanyAgentId)
BEGIN                                    
                                    delete from EmployeeCompanyAgent where  EmployeeUserId=@EmployeeUserId AND CompanyAgentId=@CompanyAgentId
                                    END";
            DBExecCommandEx(sql, queryParamList, ref errMessage);
            return new ResponseModel { Success = string.IsNullOrEmpty(errMessage) };

        }
        public List<TextValuePairModel> GetEmployeeAsTextValue()
        {
            const string sql = @"SELECT E.UserId Id,e.UserName Name FROM EmployeeUser E
                                Left JOIN UserCredentials U ON E.UserId=U.Id and E.IsActive=1";

            return ExecuteDBQuery(sql, null, EmployeeMapper.ToTextValuePairModel);
        }

        public ResponseModel UpdateEmployee(PortalUserViewModel model)
        {
            var errMessage = string.Empty;
            var queryParamList = new QueryParamList
                {
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Id", ParamValue =model.Id},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@UserId", ParamValue =model.UserId},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Email", ParamValue =model.Email},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@UserFullName", ParamValue =model.UserFullName},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@DesignationName", ParamValue =model.DesignationName},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@PhoneNumber", ParamValue =model.PhoneNumber},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@ImageFileName", ParamValue =model.ImageFileName},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@ImageFileId", ParamValue =model.ImageFileId},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@IsActive", ParamValue =model.IsActive},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Gender", ParamValue =model.Gender},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Password", ParamValue =model.Password},
                };

            const string sql = @"UPDATE EmployeeUser SET UserName=@UserFullName,Designation=@DesignationName,PhoneNumber=@PhoneNumber,ImageFileName=@ImageFileName,ImageFileId=@ImageFileId,IsActive=@IsActive,Gender=@Gender WHERE Id=@Id";
            DBExecCommandEx(sql, queryParamList, ref errMessage);

            return new ResponseModel { Success = string.IsNullOrEmpty(errMessage) };
        }
        public ResponseModel UpdateEmployeeCrediantial(PortalUserViewModel model)
        {
            var errMessage = string.Empty;
            var queryParamList = new QueryParamList
                {
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@UserId", ParamValue =model.UserId},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Email", ParamValue =model.Email},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@UserFullName", ParamValue =model.UserFullName},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@PhoneNumber", ParamValue =model.PhoneNumber},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@IsActive", ParamValue =model.IsActive},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Gender", ParamValue =model.Gender},

                };

            const string sql = @"Update UserCredentials SET FullName=@UserFullName,Email=@Email,ContactNo=@PhoneNumber WHERE Id=@UserId";
            DBExecCommandEx(sql, queryParamList, ref errMessage);

            return new ResponseModel { Success = string.IsNullOrEmpty(errMessage) };
        }
        public ResponseModel UpdatePushToken(string userId, string pushToken)
        {
            var errMessage = string.Empty;
            var queryParamList = new QueryParamList
                {
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Id", ParamValue =userId},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@PushToken", ParamValue =pushToken},


                };

            const string sql = @"UPDATE UserCredentials SET PushToken=@PushToken
                                WHERE Id=@Id";
            DBExecCommandEx(sql, queryParamList, ref errMessage);

            return new ResponseModel { Success = string.IsNullOrEmpty(errMessage) };
        }
    }
}