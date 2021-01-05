using ToDoApp.Common;
using ToDoApp.Common.Models;
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
    public class DepartmentDataAccess : BaseDatabaseHandler, IDepartment
    {
        public DepartmentDataAccess() { }
        public List<Department> GetDepartment()
        {
            string err = string.Empty;

            string sql = @"SELECT * from Department";
            var results = ExecuteDBQuery(sql, null, DepartmentMapper.ToDepartmentModel);
            return results.Any() ? results : null;
        }
        public Department Create(Department model,string userId)
        {
            var err = string.Empty;
            Database db = GetSQLDatabase();
            var returnId = -1;
            using (DbConnection conn = db.CreateConnection())
            {
                conn.Open();
                DbTransaction trans = conn.BeginTransaction();
                try
                {
                    returnId = SaveDepartment(model, db, trans);
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    err = ex.Message;
                }
                if ( returnId >= 1 )
                {
                    string sql = @"select * from Department where Id in (select top 1 Id from Department order by Id desc)";
                    var results = ExecuteDBQuery( sql , null, DepartmentMapper.ToDepartmentModel );
                    model = results.Any() ? results.FirstOrDefault() : null;
                }
            }
            return model;
        }

        public ResponseModel UpdateDepartment(Department model)
        {
            var err = string.Empty;
            Database db = GetSQLDatabase();
            using (DbConnection conn = db.CreateConnection())
            {
                conn.Open();
                DbTransaction trans = conn.BeginTransaction();
                try
                {
                    const string sql = @"UPDATE Department SET DepartmentName = @DepartmentName WHERE Id=@Id";
                    var queryParamList = new QueryParamList
                           {
                                new QueryParamObj { ParamName = "@DepartmentName", ParamValue = model.DepartmentName},
                                new QueryParamObj { ParamName = "@Id", ParamValue = model.Id}
                            };
                    DBExecCommandEx(sql, queryParamList, ref err);
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    err = ex.Message;
                }
            }
            return new ResponseModel { Success = string.IsNullOrEmpty(err) };
        }
        public int SaveDepartment(Department model, Database db, DbTransaction trans)
        {

            var errMessage = string.Empty;

            var queryParamList = new QueryParamList
                    {
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@DepartmentName", ParamValue =model.DepartmentName},
                    };
            const string sql = @"INSERT INTO Department(DepartmentName) VALUES(@DepartmentName)";
            return DBExecCommandExTran(sql, queryParamList, trans, db, ref errMessage);
        }
        public ResponseModel DeleteDepartmentById(string id)
        {
            var errMessage = string.Empty;
            if(GetEmployeeByDepartmentId(id).Count>0) return new ResponseModel { Success = false,Message="Department have employee can't delete" };
            var queryParamList = new QueryParamList
                    {
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@id", ParamValue =id},
                    };
            const string sql = @"delete from Department where Id=@id";
          DBExecCommandEx(sql, queryParamList, ref errMessage);
            return new ResponseModel { Success = string.IsNullOrEmpty(errMessage) };
        }
        public List<EmployeeUser> GetEmployeeByDepartmentId(string id)
        {
            string err = string.Empty;
            string sql = @"SELECT E.*,null DepartmentName from EmployeeUser e where E.DepartmentId=@id";
            var queryParamList = new QueryParamList
                    {
                        new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@id", ParamValue =id},
                    };
            var results = ExecuteDBQuery(sql, queryParamList, EmployeeMapper.ToEmployeeModel);
            return results;
        }
    }
}