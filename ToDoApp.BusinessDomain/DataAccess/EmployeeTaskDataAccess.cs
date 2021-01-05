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
using ToDoApp.Common.Models;

namespace ToDoApp.BusinessDomain.DataAccess
{
    public class EmployeeTaskDataAccess : BaseDatabaseHandler, IEmployeeTask
    {
       
        public ResponseModel AddOrUpdate(TaskModel model, List<TaskAttachment> attachmentsModel)
        {
            var errMessage = string.Empty;
            Database db = GetSQLDatabase();
            using (DbConnection conn = db.CreateConnection())
            {
                conn.Open();
                DbTransaction trans = conn.BeginTransaction();
                try
                {
                    var returnOb = SaveTask(model, db, trans);
                    if (returnOb.Success && attachmentsModel!=null)
                    {
                        foreach (var item in attachmentsModel)
                        {
                            if (!string.IsNullOrEmpty(item.BlobName))
                            {
                                TaskAttachment taModel = new TaskAttachment()
                                {
                                    Id = item.Id == null ? Guid.NewGuid().ToString() : item.Id,
                                    TaskId = returnOb.ReturnCode,
                                    BlobName = item.BlobName,
                                    FileName = item.FileName,
                                    UpdatedAt = DateTime.Now,
                                    UpdatedById = model.CreatedById
                                };
                                SaveTaskAttachment(taModel, db, trans);
                            }
                        }

                    }
                    trans.Commit();
                }
                catch (Exception ex)
                {

                    trans.Rollback();
                    errMessage = ex.Message;
                }
            }
            return new ResponseModel { Success = string.IsNullOrEmpty(errMessage),Message = string.IsNullOrEmpty(errMessage) ? "Saved sucessfully" : "Problem in save" };
        }
        public ResponseModel SaveTask(TaskModel model, Database db, DbTransaction trans)
        {
            var errMessage = string.Empty;
            var guid = Guid.NewGuid().ToString();
            var queryParamList = new QueryParamList
                {
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Id", ParamValue =string.IsNullOrEmpty(model.Id)?guid:model.Id},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Title", ParamValue =model.Title},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Description", ParamValue = model.Description},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@CreatedAt", ParamValue = DateTime.UtcNow,DBType=DbType.DateTime},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@CreatedById", ParamValue = string.IsNullOrEmpty(model.CreatedById)?null:model.CreatedById},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@AssignedToId", ParamValue =string.IsNullOrEmpty(model.AssignedToId)?null:model.AssignedToId},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@StatusId", ParamValue = model.StatusId},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@DueDate", ParamValue = !string.IsNullOrEmpty(model.DueDateVw)?Convert.ToDateTime(model.DueDateVw):(DateTime?)null,DBType=DbType.DateTime},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@PriorityId", ParamValue = model.PriorityId},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@CompanyId", ParamValue = model.CompanyId},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@UpdatedById", ParamValue = model.UpdatedById}
                };

            const string sql = @"IF NOT EXISTS(SELECT TOP 1 P.Id FROM Task P WHERE P.Id=@Id)
                                BEGIN
                                 DECLARE @tNo INT=0
                                 SELECT @tNo=count(t.Id) FROM Task T
                                 INSERT INTO Task(Id,TaskNo,Title,Description,CreatedAt,CreatedById,AssignedToId,StatusId,DueDate,PriorityId,CompanyId)
				                 VALUES(@Id,@tNo+1,@Title,@Description,@CreatedAt,@CreatedById,@AssignedToId,@StatusId,@DueDate,@PriorityId,@CompanyId)
                                END
                                ELSE
                                BEGIN
                                  UPDATE Task SET Title=@Title,Description=@Description,AssignedToId=@AssignedToId,PriorityId=@PriorityId,
                                    StatusId=@StatusId,DueDate=@DueDate,UpdatedById=@UpdatedById,UpdatedAt=@CreatedAt WHERE Id=@Id
                                END";
            DBExecCommandExTran(sql, queryParamList, trans, db, ref errMessage);
            return new ResponseModel { Success = string.IsNullOrEmpty(errMessage), ReturnCode = string.IsNullOrEmpty(model.Id) ? guid : model.Id };

        }

        public ResponseModel SaveTaskAttachment(TaskAttachment model, Database db, DbTransaction trans)
        {
            var errMessage = string.Empty;
            var guid = Guid.NewGuid().ToString();
            var queryParamList = new QueryParamList
                {
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Id", ParamValue = string.IsNullOrEmpty(model.Id)?guid:model.Id},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@TaskId", ParamValue =model.TaskId},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@FileName", ParamValue =model.FileName},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@BlobName", ParamValue =model.BlobName},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@UpdatedAt", ParamValue =DateTime.UtcNow,DBType=DbType.DateTime},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@UpdatedById", ParamValue =model.UpdatedById},
                };

            const string sql = @"IF NOT EXISTS(SELECT TOP 1 P.Id FROM TaskAttachments P WHERE P.Id=@Id)
                                BEGIN
                               	INSERT INTO TaskAttachments(Id,TaskId,FileName,BlobName,UpdatedAt,UpdatedById) 
                                VALUES (@Id,@TaskId,@FileName,@BlobName,@UpdatedAt,@UpdatedById)
                                END
                                ELSE 
                                BEGIN
                                UPDATE TaskAttachments SET FileName=@FileName,
                                BlobName=@BlobName,UpdatedById=@UpdatedById,UpdatedAt=@UpdatedAt
	                            WHERE Id=@Id
                                END";
            DBExecCommandExTran(sql, queryParamList, trans, db, ref errMessage);
            return new ResponseModel { Success = string.IsNullOrEmpty(errMessage) };
        }

        public List<TaskAttachment> GetTaskAttachments(string taskId)
        {
            const string sql = @"SELECT * FROM TaskAttachments where TaskId=@taskId";
            var queryParamList = new QueryParamList
                {
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@taskId", ParamValue = taskId}
                };
            return ExecuteDBQuery(sql, queryParamList, EmployeeTaskMapper.ToTaskAttachment);
        }
        public List<TaskModel> GetRelatedToMeTaskList(TaskModel sModel)
        {
            const string sql = @"SELECT T.*,C.FullName AssignToName,CreatedBy.FullName CreatedByName,UpdatedBy.FullName UpdatedByName 
                                FROM Task t
                                LEFT JOIN UserCredentials C ON T.AssignedToId=C.Id  
                                    LEFT JOIN UserCredentials CreatedBy ON T.CreatedById=CreatedBy.Id 
                                    LEFT JOIN UserCredentials UpdatedBy ON T.UpdatedById=UpdatedBy.Id  where (T.AssignedToId=@userId OR T.CreatedById=@userId)";
            var queryParamList = new QueryParamList
                {
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@userId", ParamValue =sModel.AssignedToId}
                };
            return ExecuteDBQuery(sql, queryParamList, EmployeeTaskMapper.ToTask);
        }
        public List<TaskModel> GetTaskList(TaskModel sModel)
        {

            const string sql = @"SELECT T.*,C.FullName AssignToName,CreatedBy.FullName CreatedByName,UpdatedBy.FullName UpdatedByName 
                                FROM Task t
                                LEFT JOIN UserCredentials C ON T.AssignedToId=C.Id 
                                    LEFT JOIN UserCredentials CreatedBy ON T.CreatedById=CreatedBy.Id 
                                    LEFT JOIN UserCredentials UpdatedBy ON T.UpdatedById=UpdatedBy.Id 
                                    where 
                                   (@CreatedById is null or t.CreatedById=@CreatedById)
                                    and (@AssignedToId is null or t.AssignedToId=@AssignedToId)";
            var queryParamList = new QueryParamList
                {
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@CreatedById", ParamValue =string.IsNullOrEmpty(sModel.CreatedById)?null:sModel.CreatedById},
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@AssignedToId", ParamValue = string.IsNullOrEmpty(sModel.AssignedToId)?null:sModel.AssignedToId},
                };
            return ExecuteDBQuery(sql, queryParamList, EmployeeTaskMapper.ToTask);
        }


        public List<TaskModel> GetTasks(int companyId)
        {
            const string sql = @"SELECT T.*,C.FullName AssignToName,CreatedBy.FullName CreatedByName,UpdatedBy.FullName UpdatedByName 
                                FROM Task t
                                LEFT JOIN UserCredentials C ON T.AssignedToId=C.Id  
                                    LEFT JOIN UserCredentials CreatedBy ON T.CreatedById=CreatedBy.Id 
                                    LEFT JOIN UserCredentials UpdatedBy ON T.UpdatedById=UpdatedBy.Id WHERE T.CompanyId=@companyId";
            var queryParamList = new QueryParamList
                {
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@companyId", ParamValue =companyId}
                };
            return ExecuteDBQuery(sql, queryParamList, EmployeeTaskMapper.ToTask);
        }
        public TaskModel GetTaskDetails(string id)
        {
            const string sql = @"SELECT T.*,C.FullName AssignToName,CreatedBy.FullName CreatedByName,UpdatedBy.FullName UpdatedByName 
                                FROM Task t
                                LEFT JOIN UserCredentials C ON T.AssignedToId=C.Id  
                                    LEFT JOIN UserCredentials CreatedBy ON T.CreatedById=CreatedBy.Id 
                                    LEFT JOIN UserCredentials UpdatedBy ON T.UpdatedById=UpdatedBy.Id  where T.Id=@id";
            var queryParamList = new QueryParamList
                {
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@id", ParamValue =id}
                };
            var data = ExecuteDBQuery(sql, queryParamList, EmployeeTaskMapper.ToTask);
            return (data != null && data.Count > 0) ? data.FirstOrDefault() : null;
        }

        public ResponseModel DeleteTask(string id)
        {
            var errMessage = string.Empty;
            var queryParamList = new QueryParamList
                {
                    new QueryParamObj { ParamDirection = ParameterDirection.Input, ParamName = "@Id", ParamValue =id},
                };
            const string sql2 = @"IF EXISTS(SELECT TOP 1 P.Id FROM TaskAttachments P WHERE P.TaskId=@Id)
                                BEGIN
                                    DELETE FROM TaskAttachments WHERE TaskId=@Id
                                END";
            DBExecCommandEx(sql2, queryParamList, ref errMessage);
            const string sql = @"DELETE FROM Task WHERE Id=@Id";
            DBExecCommandEx(sql, queryParamList, ref errMessage);

            return new ResponseModel { Success = string.IsNullOrEmpty(errMessage) };
        }
    }
}
