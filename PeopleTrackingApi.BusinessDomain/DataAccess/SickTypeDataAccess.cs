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
    public class SickTypeDataAccess : BaseDatabaseHandler, ISickType
    {
        public SickTypeDataAccess() { }
        public List<SickType> GetAllSickType()
        {
            string err = string.Empty;

            string sql = @"SELECT s.*,c.CompanyName from SickType s left join Company c on s.CompanyId=c.Id";
            var results = ExecuteDBQuery(sql, null, SickTypeMapper.ToSickTypeModel);
            return results;
        }
        public ResponseModel Create(SickType model)
        {
            var errMessage = string.Empty;
            var queryParamList = new QueryParamList
                    {
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Id", ParamValue =model.Id},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Name", ParamValue =model.Name},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@CompanyId", ParamValue =model.CompanyId},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@CreateDate", ParamValue =DateTime.UtcNow,DBType=DbType.DateTime},
                    };
            const string sql = @"IF NOT EXISTS(SELECT TOP 1 * FROM SickType C WHERE C.Id=@Id)
                                BEGIN
                                INSERT INTO SickType(CompanyId,Name,CreateDate) 
                                VALUES(@CompanyId,@Name,@CreateDate)
                                END
                                ELSE
                                BEGIN
                                UPDATE SickType  SET Name=@Name WHERE Id=@Id
                                END";
            DBExecCommandEx(sql, queryParamList, ref errMessage);
            return new ResponseModel { Success = string.IsNullOrEmpty(errMessage) };
        }


        public ResponseModel DeleteSickType(int id)
        {
            var errMessage = string.Empty;
            var queryParamList = new QueryParamList
                    {
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@id", ParamValue =id},
                    };
            const string sql = @"delete from SickType where Id=@id";
            DBExecCommandEx(sql, queryParamList, ref errMessage);
            return new ResponseModel { Success = string.IsNullOrEmpty(errMessage) };
        }
        public List<SickType> GetSickTypeById(int id)
        {
            string err = string.Empty;
            string sql = @"SELECT s.*,c.CompanyName from SickType s left join Company c on s.CompanyId=c.Id where s.Id=@id";
            var queryParamList = new QueryParamList
                    {
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@id", ParamValue =id},
                    };
            var results = ExecuteDBQuery(sql, queryParamList, SickTypeMapper.ToSickTypeModel);
            return results;
        }
    }
}