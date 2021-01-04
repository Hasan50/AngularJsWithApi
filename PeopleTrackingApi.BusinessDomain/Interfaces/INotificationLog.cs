using PeopleTrackingApi.BusinessDomain.Models;
using System.Collections.Generic;

namespace PeopleTrackingApi.BusinessDomain.Interfaces
{
    public interface INotificationLog
    {
        void Add(NotificationLogModel model);
        List<NotificationLogModel> GetAll(int companyId);
        List<NotificationLogModel> GetTop10(int companyId);
    }
}
