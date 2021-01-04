﻿using PeopleTrackingApi.Common.Models;
using System.Collections.Generic;
using PeopleTrackingApi.BusinessDomain.Models;

namespace PeopleTrackingApi.BusinessDomain.Interfaces
{
    public interface IEmployeeTask
    {

        List<TaskAttachment> GetTaskAttachments(string taskId);
        List<TaskModel> GetRelatedToMeTaskList(TaskModel sModel);

        List<TaskModel> GetTaskList(TaskModel sModel);
        ResponseModel DeleteTask(string id);
        TaskModel GetTaskDetails(string id);
        List<TaskModel> GetTasks(int companyId);
        ResponseModel AddOrUpdate(TaskModel model, List<TaskAttachment> attachmentsModel);
    }
}