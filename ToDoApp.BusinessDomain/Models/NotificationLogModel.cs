using ToDoApp.Common;
using System;
using System.Web.Script.Serialization;

namespace ToDoApp.BusinessDomain.Models
{
    public class NotificationLogModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int NotificationTypeId { get; set; }
        [ScriptIgnore]
        public DateTime? ActionAt { get; set; }
        [ScriptIgnore]
        public string ActionById { get; set; }
        public int? CompanyId { get; set; }

        public string ActionAtVw
        {
            get { return ActionAt.HasValue ? ActionAt.Value.ToZoneTimeBD().ToString(Constants.DateTimeFormat):string.Empty; }
        }
    }
}
