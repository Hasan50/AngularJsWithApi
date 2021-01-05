using ToDoApp.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using ToDoApp.DataAccess.Common;
using ToDoApp.BusinessDomain.Interfaces;
using ToDoApp.BusinessDomain.Mappers;
using ToDoApp.BusinessDomain.Models;

namespace ToDoApp.BusinessDomain.DataAccess
{
    public class NoticeBoardDataAccess : BaseDatabaseHandler, INoticeBoard
    {
        private readonly IEmployee _employee;
        public NoticeBoardDataAccess(IEmployee employee)
        {
            _employee = employee;

        }


        public List<NoticeBoard> GetNoticeBoard()
        {
            string err = string.Empty;
            string sql = @"SELECT nb.*  FROM [NoticeBoard] as nb  order by CreateDate Desc";

            var results = ExecuteDBQuery( sql , null, NoticeBoardMapper.ToNoticeBoardModel );


            return results;
        }

        public NoticeBoard GetNoticeBoardById(string noticeId )
        {
            string err = string.Empty;
            string sql = @"select * from NoticeBoard Where Id='" + noticeId+"'";
            var results = ExecuteDBQuery( sql , null , NoticeBoardMapper.ToNoticeBoardModel );
            return results.Any() ? results.FirstOrDefault() : null;
        }

        public NoticeDepartmentVIewModel CreateNoticeBoard( NoticeDepartmentVIewModel model)
        {
            var err = string.Empty;
            Database db = GetSQLDatabase();
            var returnId = -1;
            using ( DbConnection conn = db.CreateConnection() )
            {
                conn.Open();
                DbTransaction trans = conn.BeginTransaction();
                try
                {
                    model.Id = Guid.NewGuid().ToString();
                    returnId = SaveToNoticeBoard( model , db , trans );
                    trans.Commit();
                }
                catch ( Exception ex )
                {
                    trans.Rollback();
                    err = ex.Message;
                }
            }
            return model;
        }


        public int SaveToNoticeBoard( NoticeDepartmentVIewModel model , Database db , DbTransaction trans )
        {
            var errMessage = string.Empty;

            var queryParamList = new QueryParamList
                    {
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Id",ParamValue =model.Id ,DBType = DbType.String},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Details",ParamValue =model.Details},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@PostingDate", ParamValue =DateTime.UtcNow, DBType=DbType.DateTime},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@ImageFileName", ParamValue =model.ImageFileName},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@CreatedBy", ParamValue =model.CreatedBy},
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@CreateDate", ParamValue =DateTime.UtcNow, DBType=DbType.DateTime},
                    };
            const string sql = @"INSERT INTO NoticeBoard (Id, Details, PostingDate, ImageFileName, CreatedBy, CreateDate) VALUES( @Id, @Details, @PostingDate,@ImageFileName, @CreatedBy, @CreateDate)";
            return DBExecCommandExTran( sql , queryParamList , trans , db , ref errMessage );
        }

       
    }
}
