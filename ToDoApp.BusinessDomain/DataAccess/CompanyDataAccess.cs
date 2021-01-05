using ToDoApp.BusinessDomain.Interfaces;
using ToDoApp.BusinessDomain.Mappers;
using ToDoApp.BusinessDomain.Models;
using ToDoApp.Common;
using ToDoApp.Common.Models;
using ToDoApp.DataAccess.Common;
using System;
using System.Collections.Generic;
using System.Data;

namespace ToDoApp.BusinessDomain.DataAccess
{
    public class CompanyDataAccess : BaseDatabaseHandler, ICompany
    {

        public List<Company> GetCompanyList()
        {
            string err = string.Empty;
            string sql = @"select * from Company";
            var results = ExecuteDBQuery(sql, null, CompanyMapper.ToCompanyModel);
            return results;
        }
        public List<Company> GetCompanyListById(int Id)
        {
            string err = string.Empty;
            var queryParamList = new QueryParamList
                    {
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Id", ParamValue =Id},
                    };
            string sql = @"select * from Company where Id=@Id";
            var results = ExecuteDBQuery(sql, queryParamList, CompanyMapper.ToCompanyModel);
            return results;
        }
        public ResponseModel Create(Company model)
        {
            var err = string.Empty;

            var queryParamList = new QueryParamList
                    {
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Id", ParamValue =model.Id},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@CompanyName", ParamValue =model.CompanyName},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Address", ParamValue =model.Address},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@PhoneNumber", ParamValue =model.PhoneNumber},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@CreatedById", ParamValue =model.CreatedById},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@IsDateTripHistory", ParamValue =model.IsDateTripHistory},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@IsShowTaskMenu", ParamValue =model.IsShowTaskMenu},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@IsShowLeaveMenu", ParamValue =model.IsShowLeaveMenu},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@IsShowTaskDateFilter", ParamValue =model.IsShowTaskDateFilter},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@IsMobileAppAccesible", ParamValue =model.IsMobileAppAccesible},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@IsActive", ParamValue =model.IsActive},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@CompanyAdminName", ParamValue =model.CompanyAdminName},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@CompanyAdminEmail", ParamValue =model.CompanyAdminEmail},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@CompanyAdminLoginID", ParamValue =model.CompanyAdminLoginID},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@CompanyAdminPassword", ParamValue =model.CompanyAdminPassword},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@ContactPersonName", ParamValue =model.ContactPersonName},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@GeofanceTime", ParamValue =model.GeofanceTime},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@GeofanceRadious", ParamValue =model.GeofanceRadious},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@UserKey", ParamValue =Guid.NewGuid().ToString()},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@UserLanguage", ParamValue =Constants.DefaultLanguage},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@CreatedDate", ParamValue =DateTime.UtcNow,DBType=DbType.DateTime},
                    };


            const string sql = @"IF NOT EXISTS(SELECT TOP 1 * FROM Company C WHERE C.Id=@Id)
                                BEGIN
                                DECLARE @cId INT
                                INSERT INTO Company(CompanyName,Address,PhoneNumber,CreatedDate,CreatedById,IsDateTripHistory,IsShowTaskMenu,IsShowLeaveMenu,IsShowTaskDateFilter,IsMobileAppAccesible,IsActive,CompanyAdminName,CompanyAdminEmail,CompanyAdminLoginID,ContactPersonName,GeofanceTime,GeofanceRadious,UserLanguage) 
                                VALUES(@CompanyName,@Address,@PhoneNumber,@CreatedDate,@CreatedById,@IsDateTripHistory,@IsShowTaskMenu,@IsShowLeaveMenu,@IsShowTaskDateFilter,@IsMobileAppAccesible,1,@CompanyAdminName,@CompanyAdminEmail,@CompanyAdminLoginID,@ContactPersonName,@GeofanceTime,@GeofanceRadious,@UserLanguage)
                                SET @cId=SCOPE_IDENTITY()
                                INSERT INTO UserCredentials(Id,FullName,Email,LoginID,Password,UserTypeId,IsActive,CreatedAt,CompanyId)
                                VALUES(@UserKey,@CompanyAdminName,@CompanyAdminEmail,@CompanyAdminLoginID,@CompanyAdminPassword,2,1,@CreatedDate,@cId)
                                END
                                ELSE
                                BEGIN
                                UPDATE Company  SET CompanyName=@CompanyName,ContactPersonName=@ContactPersonName,Address =@Address ,PhoneNumber = @PhoneNumber,CompanyAdminName=@CompanyAdminName,CompanyAdminEmail=@CompanyAdminEmail,IsDateTripHistory=@IsDateTripHistory,IsShowTaskMenu=@IsShowTaskMenu,IsShowLeaveMenu=@IsShowLeaveMenu,IsShowTaskDateFilter=@IsShowTaskDateFilter,IsMobileAppAccesible=@IsMobileAppAccesible,IsActive=@IsActive,UserLanguage=@UserLanguage WHERE Id=@Id
                                END";
            DBExecCommandEx(sql, queryParamList, ref err);

            return new ResponseModel { Success = string.IsNullOrEmpty(err) };
        }
        public ResponseModel Update(Company model)
        {
            var err = string.Empty;
            const string sql = @"UPDATE Company  SET CompanyName=@CompanyName,ContactPersonName=@ContactPersonName, Address =@Address ,PhoneNumber = @PhoneNumber,CompanyAdminName=@CompanyAdminName,
                                CompanyAdminEmail=@CompanyAdminEmail,IsDateTripHistory=@IsDateTripHistory,IsShowTaskMenu=@IsShowTaskMenu,IsShowLeaveMenu=@IsShowLeaveMenu,IsShowTaskDateFilter=@IsShowTaskDateFilter,
                                IsMobileAppAccesible=@IsMobileAppAccesible,IsActive=@IsActive,GeofanceTime=@GeofanceTime,GeofanceLat=@GeofanceLat,GeofanceLng=@GeofanceLng,GeofanceRadious=@GeofanceRadious,UserLanguage=@UserLanguage WHERE Id=@Id";
            var queryParamList = new QueryParamList
                           {
                                new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Id", ParamValue =model.Id},
                                new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@CompanyName", ParamValue =model.CompanyName},
                                new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Address", ParamValue =model.Address},
                                new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@PhoneNumber", ParamValue =model.PhoneNumber},
                                new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@CompanyAdminName", ParamValue =model.CompanyAdminName},
                                new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@CompanyAdminEmail", ParamValue =model.CompanyAdminEmail},
                                new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@IsDateTripHistory", ParamValue =model.IsDateTripHistory},
                                new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@IsShowTaskMenu", ParamValue =model.IsShowTaskMenu},
                                new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@IsShowLeaveMenu", ParamValue =model.IsShowLeaveMenu},
                                new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@IsShowTaskDateFilter", ParamValue =model.IsShowTaskDateFilter},
                                new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@IsMobileAppAccesible", ParamValue =model.IsMobileAppAccesible},
                                new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@IsActive", ParamValue =model.IsActive},
                                new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@CreatedDate", ParamValue =DateTime.UtcNow,DBType=DbType.DateTime},
                                new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@ContactPersonName", ParamValue =model.ContactPersonName},
                                new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@GeofanceTime", ParamValue =model.GeofanceTime},
                                new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@GeofanceLat", ParamValue =model.GeofanceLat},
                                new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@GeofanceLng", ParamValue =model.GeofanceLng},
                                new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@GeofanceRadious", ParamValue =model.GeofanceRadious},
                                new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@UserLanguage", ParamValue =model.UserLanguage},
                            };
            DBExecCommandEx(sql, queryParamList, ref err);

            return new ResponseModel { Success = string.IsNullOrEmpty(err) };
        }

        public ResponseModel UpdateLogo(int id, string fileId, string fileName)
        {
            var err = string.Empty;
            const string sql = @"UPDATE Company  SET ImageFileName=@ImageFileName, ImageFileId =@ImageFileId WHERE Id=@Id";
            var queryParamList = new QueryParamList
                           {
                                new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Id", ParamValue =id},
                                new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@ImageFileName", ParamValue =fileName},
                                new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@ImageFileId", ParamValue =fileId}

                            };
            DBExecCommandEx(sql, queryParamList, ref err);

            return new ResponseModel { Success = string.IsNullOrEmpty(err) };
        }


        public ResponseModel Delete(int id)
        {
            var errMessage = string.Empty;
            var queryParamList = new QueryParamList
                    {
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@id", ParamValue =id},
                    };
            const string sql = @"DELETE FROM UserCredentials WHERE CompanyId=@id
                                delete from Company where Id=@id";
            DBExecCommandEx(sql, queryParamList, ref errMessage);

            return new ResponseModel { Success = string.IsNullOrEmpty(errMessage) };
        }

    }
}