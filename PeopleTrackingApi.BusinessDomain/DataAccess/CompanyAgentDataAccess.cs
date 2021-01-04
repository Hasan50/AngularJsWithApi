using PeopleTrackingApi.BusinessDomain.Interfaces;
using PeopleTrackingApi.BusinessDomain.Mappers;
using PeopleTrackingApi.BusinessDomain.Models;
using PeopleTrackingApi.Common;
using PeopleTrackingApi.Common.Models;
using PeopleTrackingApi.DataAccess.Common;
using System;
using System.Collections.Generic;
using System.Data;

namespace PeopleTrackingApi.BusinessDomain.DataAccess
{
    public class CompanyAgentDataAccess : BaseDatabaseHandler, ICompanyAgent
    {

        public List<CompanyAgent> GetCompanyAgentList(int companyId)
        {
            string err = string.Empty;
            var queryParamList = new QueryParamList
                    {
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@companyId", ParamValue =companyId},
                    };
            string sql = @"select * from CompanyAgent where CompanyId=@companyId";
            var results = ExecuteDBQuery(sql, queryParamList, CompanyAgentMapper.ToCompanyAgentModel);
            return results;
        }

        public List<CompanyAgent> GetCompanyAgentListWithEmployee(string userId)
        {
            string err = string.Empty;
            var queryParamList = new QueryParamList
                    {
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@userId", ParamValue =userId},
                    };
            string sql = @"select * from CompanyAgent c right join EmployeeCompanyAgent ec on ec.CompanyAgentId=c.Id left join EmployeeUser eu on eu.Id=ec.EmployeeUserId where eu.UserId=@userId";
            var results = ExecuteDBQuery(sql, queryParamList, CompanyAgentMapper.ToCompanyAgentModel);
            return results;
        }
        public List<CompanyAgent> GetCompanyAgentDetails(int Id)
        {
            string err = string.Empty;
            var queryParamList = new QueryParamList
                    {
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Id", ParamValue =Id},
                    };
            string sql = @"select * from CompanyAgent where Id=@Id";
            var results = ExecuteDBQuery(sql, queryParamList, CompanyAgentMapper.ToCompanyAgentModel);
            return results;
        }
        public ResponseModel Create(CompanyAgent model)
        {
            var err = string.Empty;
            var queryParamList = new QueryParamList
                    {
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Id", ParamValue =model.Id},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@CompanyId", ParamValue =model.CompanyId},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@AgentName", ParamValue =model.AgentName},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Address", ParamValue =model.Address},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@PhoneNumber", ParamValue =model.PhoneNumber},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@CreatedById", ParamValue =model.CreatedById},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@IsActive", ParamValue =model.IsActive},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Email", ParamValue =model.Email},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@ContactPersonName", ParamValue =model.ContactPersonName},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@CreatedDate", ParamValue =DateTime.UtcNow,DBType=DbType.DateTime},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@GeofanceLat", ParamValue =model.GeofanceLat},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@GeofanceLng", ParamValue =model.GeofanceLng},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@OfficeStartTime", ParamValue =model.OfficeStartTime},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@OfficeEndTime", ParamValue =model.OfficeEndTime},

            };
            const string sql = @"IF NOT EXISTS(SELECT TOP 1 * FROM CompanyAgent C WHERE C.Id=@Id)
                                BEGIN
                                
                                INSERT INTO CompanyAgent(AgentName,Address,PhoneNumber,CreatedDate,CreatedById,IsActive,Email,ContactPersonName,CompanyId,GeofanceLat,OfficeStartTime,OfficeEndTime) 
                                VALUES(@AgentName,@Address,@PhoneNumber,@CreatedDate,@CreatedById,1,@Email,@ContactPersonName,@CompanyId,@GeofanceLat,@OfficeStartTime,@OfficeEndTime)
                                END
                                ELSE
                                BEGIN
                                UPDATE CompanyAgent  SET AgentName=@AgentName,ContactPersonName=@ContactPersonName,Address =@Address ,PhoneNumber = @PhoneNumber,Email=@Email,IsActive=@IsActive,GeofanceLat=@GeofanceLat, GeofanceLng=@GeofanceLng,OfficeStartTime=@OfficeStartTime,OfficeEndTime=@OfficeEndTime WHERE Id=@Id
                                END";
            DBExecCommandEx(sql, queryParamList, ref err);
            return new ResponseModel { Success = string.IsNullOrEmpty(err) };
        }
        public ResponseModel Update(CompanyAgent model)
        {
            var err = string.Empty;
            const string sql = @"UPDATE CompanyAgent  SET AgentName=@AgentName,ContactPersonName=@ContactPersonName, Address =@Address ,PhoneNumber = @PhoneNumber,Email=@Email,IsActive=@IsActive,GeofanceLat=@GeofanceLat,GeofanceLng=@GeofanceLng,OfficeStartTime=@OfficeStartTime,OfficeEndTime=@OfficeEndTime  WHERE Id=@Id";
            var queryParamList = new QueryParamList
                           {
                                new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Id", ParamValue =model.Id},
                                new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@AgentName", ParamValue =model.AgentName},
                                new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Address", ParamValue =model.Address},
                                new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@PhoneNumber", ParamValue =model.PhoneNumber},
                                new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Email", ParamValue =model.Email},
                                new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@IsActive", ParamValue =model.IsActive},
                                new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@CreatedDate", ParamValue =DateTime.UtcNow,DBType=DbType.DateTime},
                                new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@ContactPersonName", ParamValue =model.ContactPersonName},
                                                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@GeofanceLat", ParamValue =model.GeofanceLat},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@GeofanceLng", ParamValue =model.GeofanceLng},
                                                new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@OfficeStartTime", ParamValue =model.OfficeStartTime},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@OfficeEndTime", ParamValue =model.OfficeEndTime},

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
            const string sql = @"delete from CompanyAgent where Id=@id";
            DBExecCommandEx(sql, queryParamList, ref errMessage);
            return new ResponseModel { Success = string.IsNullOrEmpty(errMessage) };
        }

    }
}