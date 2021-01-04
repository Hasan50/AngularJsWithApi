using System.Linq;
using System.Data;
using PeopleTrackingApi.Common;
using PeopleTrackingApi.Common.Models;
using System;
using PeopleTrackingApi.DataAccess.Common;
using PeopleTrackingApi.BusinessDomain.Interfaces;
using System.Collections.Generic;

namespace PeopleTrackingApi.BusinessDomain.DataAccess
{
    public class UserCredentialDataAccess : BaseDatabaseHandler, IUserCredential
    {
        public ResponseModel Save(UserCredentialModel model)
        {
            var errMessage = string.Empty;
            var pk = Guid.NewGuid().ToString();
            var queryParamList = new QueryParamList
                {
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Id", ParamValue = pk},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@FullName", ParamValue = model.FullName},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Email", ParamValue =model.Email},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@LoginID", ParamValue =model.LoginID},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@ContactNo", ParamValue = model.ContactNo},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Password", ParamValue =model.Password},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@UserTypeId", ParamValue =model.UserTypeId},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@CompanyId", ParamValue =string.IsNullOrEmpty(model.CompanyId.ToString())?null:model.CompanyId.ToString()}
                };

            const string sql = @"INSERT INTO UserCredentials(Id,FullName,Email,ContactNo,Password,UserTypeId,IsActive,CreatedAt,LoginID,CompanyId)
                                VALUES(@Id,@FullName,@Email,@ContactNo,@Password,@UserTypeId,1,GETDATE(),@LoginID,@CompanyId)";


            DBExecCommandEx(sql, queryParamList, ref errMessage);

            return new ResponseModel { Success = string.IsNullOrEmpty(errMessage), Message = errMessage, ReturnCode = pk };

        }
        public ResponseModel Update(UserCredentialModel model)
        {
            var errMessage = string.Empty;
            var queryParamList = new QueryParamList
                {
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Id", ParamValue = model.Id},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@FullName", ParamValue = model.FullName},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Email", ParamValue =model.Email},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@LoginID", ParamValue =model.LoginID},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@ContactNo", ParamValue = model.ContactNo},
                };
            const string sql = @"UPDATE UserCredentials set FullName=@FullName,Email=@Email,ContactNo=@ContactNo where Id=@Id";
            DBExecCommandEx(sql, queryParamList, ref errMessage);
            return new ResponseModel { Success = string.IsNullOrEmpty(errMessage), Message = errMessage };

        }
        public UserCredentialModel Get(string username, string password)
        {

            const string sql = @"SELECT U.* FROM UserCredentials U WHERE U.LoginID=@Username and U.Password=@Password AND U.IsActive=1";
            var queryParamList = new QueryParamList
                {
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Username", ParamValue = username},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Password", ParamValue = password}
                };
            var results = ExecuteDBQuery(sql, queryParamList, UserMapper.ToUserFullDetails);
            return results.Any() ? results.FirstOrDefault() : null;

        }

        public ResponseModel ChangePassword(string userInitial, string newPassword)
        {
            var err = string.Empty;

            const string sql = @"UPDATE UserCredentials set Password=@newPassword where Id=@userInitial";

            var queryParamList = new QueryParamList
               {
                    new QueryParamObj { ParamName = "@userInitial", ParamValue =userInitial},
                    new QueryParamObj { ParamName = "@newPassword", ParamValue =newPassword}
                };

            DBExecCommandEx(sql, queryParamList, ref err);
            return new ResponseModel { Success = string.IsNullOrEmpty(err) };
        }
        public ResponseModel DeleteUser(string Id)
        {
            var err = string.Empty;

            const string sql = @"DELETE FROM  UserCredentials where Id=@Id";

            var queryParamList = new QueryParamList
               {
                    new QueryParamObj { ParamName = "@Id", ParamValue =Id},
                };

            DBExecCommandEx(sql, queryParamList, ref err);
            return new ResponseModel { Success = string.IsNullOrEmpty(err) };
        }
        public UserCredentialModel GetProfileDetails(string userId)
        {
            const string sql = @"SELECT c.* FROM UserCredentials C WHERE c.Id=@userId and c.IsActive=1";
            var queryParamList = new QueryParamList
                {
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@userId", ParamValue = userId}
                };
            var results= ExecuteDBQuery(sql, queryParamList, UserMapper.ToUserFullDetails);
            return results.Any() ? results.FirstOrDefault() : null;
        }

        public UserCredentialModel GetByLoginID(string loginID)
        {
            const string sql = @"SELECT c.* FROM UserCredentials C WHERE c.LoginID=@loginID";
            var queryParamList = new QueryParamList
                {
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@loginID", ParamValue = loginID}
                };
            var results = ExecuteDBQuery(sql, queryParamList, UserMapper.ToUserFullDetails);
            return results.Any() ? results.FirstOrDefault() : null;
        }

        public UserCredentialModel GetByLoginID(string loginID,UserType uType)
        {
            const string sql = @"SELECT C.* FROM UserCredentials C
                                WHERE c.LoginID=@loginID AND C.UserTypeId=@uType AND C.IsActive=1";
            var queryParamList = new QueryParamList
                {
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@loginID", ParamValue = loginID},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@uType", ParamValue = (int)uType}
                };
            var results = ExecuteDBQuery(sql, queryParamList, UserMapper.ToUserFullDetails);
            return results.Any() ? results.FirstOrDefault() : null;
        }

        public UserCredentialModel GetByLoginID(string loginID,string password ,UserType uType)
        {
            const string sql = @"SELECT C.*
                                FROM UserCredentials C
                                WHERE c.LoginID=@loginID AND C.Password=@password AND C.UserTypeId=@uType";
            var queryParamList = new QueryParamList
                {
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@loginID", ParamValue = loginID},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@password", ParamValue = password},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@uType", ParamValue = (int)uType}
                };
            var results = ExecuteDBQuery(sql, queryParamList, UserMapper.ToUserFullDetails);
            return results.Any() ? results.FirstOrDefault() : null;
        }

        public UserCredentialModel GetUserFullInfo(string userId)
        {
            const string sql = @"SELECT c.* FROM UserCredentials c WHERE c.Id=@userId and c.IsActive=1";
            var queryParamList = new QueryParamList
                {
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@userId", ParamValue = userId}
                };
            var results = ExecuteDBQuery(sql, queryParamList, UserMapper.ToUserFullDetails);
            return results.Any() ? results.FirstOrDefault() : null;
        }
        public List<UserCredentialModel> GetPushToken(int companyId)
        {
            const string sql = @"SELECT c.*,u.CompanyId FROM UserCredentials C 
                                    LEFT JOIN EmployeeUser U ON C.id=U.UserId
                                     WHERE u.companyId=@companyId and c.IsActive=1";
            var queryParamList = new QueryParamList
                {
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@companyId", ParamValue = companyId}
                };
            var results = ExecuteDBQuery(sql, queryParamList, UserMapper.ToUserFullDetails);
            return results.Any() ? results : null;
        }

    }
}