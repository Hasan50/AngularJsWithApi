using PeopleTrackingApi.BusinessDomain.Interfaces;
using PeopleTrackingApi.BusinessDomain.Models;
using PeopleTrackingApi.Common;
using PeopleTrackingApi.DataAccess.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace PeopleTrackingApi.BusinessDomain.DataAccess
{
    public class NotificationLogDataAccess : BaseDatabaseHandler, INotificationLog
    {
        public void Add(NotificationLogModel model)
        {
            var err = string.Empty;
            var queryParamList = new QueryParamList
                    {
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Id", ParamValue =model.Id},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Title", ParamValue =model.Title},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Description", ParamValue =model.Description},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@NotificationTypeId", ParamValue =model.NotificationTypeId},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@ActionAt", ParamValue =DateTime.UtcNow,DBType=DbType.DateTime},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@ActionById", ParamValue =model.ActionById},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@CompanyId", ParamValue =model.CompanyId},

            };
            const string sql = @"INSERT INTO NotificationLog(Title,Description,NotificationTypeId,ActionAt,ActionById,CompanyId) 
                                VALUES(@Title,@Description,@NotificationTypeId,@ActionAt,@ActionById,@CompanyId)";
            DBExecCommandEx(sql, queryParamList, ref err);

        }

        public List<NotificationLogModel> GetAll(int companyId)
        {
            string err = string.Empty;
            var queryParamList = new QueryParamList
                    {
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@companyId", ParamValue =companyId,DBType=DbType.Int32}
                    };
            string sql = @"select top 10 * from NotificationLog where CompanyId=@companyId order by ActionAt desc";
            var results = ExecuteDBQuery(sql, queryParamList, NotificationLogMapper.ToModel);
            return results;
        }

        public List<NotificationLogModel> GetTop10(int companyId)
        {
            string err = string.Empty;
            var queryParamList = new QueryParamList
                    {
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@companyId", ParamValue =companyId,DBType=DbType.Int32}
                    };
            string sql = @"select * from NotificationLog where CompanyId=@companyId order by ActionAt desc";
            var results = ExecuteDBQuery(sql, queryParamList, NotificationLogMapper.ToModel);
            return results;
        }
    }

    public static class NotificationLogMapper
    {

        public static List<NotificationLogModel> ToModel(DbDataReader readers)
        {
            if (readers == null)
                return null;
            var models = new List<NotificationLogModel>();

            while (readers.Read())
            {
                var model = new NotificationLogModel
                {
                    Id = Convert.ToInt32(readers["Id"]),
                    Title = Convert.IsDBNull(readers["Title"]) ? string.Empty : Convert.ToString(readers["Title"]),
                    Description = Convert.IsDBNull(readers["Description"]) ? string.Empty : Convert.ToString(readers["Description"]),
                    NotificationTypeId = Convert.ToInt32(readers["NotificationTypeId"]),
                    ActionAt = Convert.IsDBNull(readers["ActionAt"]) ? (DateTime?)null : Convert.ToDateTime(readers["ActionAt"])
                    
                };

                models.Add(model);
            }

            return models;
        }
    }
}
