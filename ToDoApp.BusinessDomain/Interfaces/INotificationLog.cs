using ToDoApp.BusinessDomain.Models;
using System.Collections.Generic;

namespace ToDoApp.BusinessDomain.Interfaces
{
    public interface INotificationLog
    {
        void Add(NotificationLogModel model);
        List<NotificationLogModel> GetAll(int companyId);
        List<NotificationLogModel> GetTop10(int companyId);
    }
}
