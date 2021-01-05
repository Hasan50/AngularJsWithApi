using System;
using System.ComponentModel;
using System.IO;
using System.Web.Script.Serialization;
using ToDoApp.Common;

namespace ToDoApp.BusinessDomain.Models
{

    public class TaskModel
    {
        [Browsable(false)]
        public string Id { get; set; }

        [Browsable(false)]
        public string CreatedById { get; set; }
        [ScriptIgnore]
        [Browsable(false)]
        public DateTime? CreatedAt { get; set; }
        [Browsable(false)]
        public string AssignedToId { get; set; }
        [Browsable(false)]
        public int? StatusId { get; set; }
        [Browsable(false)]
        public int? TaskGroupId { get; set; }
        [Browsable(false)]
        [ScriptIgnore]
        public DateTime? DueDate { get; set; }

        [Browsable(false)]
        public int? PriorityId { get; set; }
        [Browsable(false)]
        public string UpdatedByName { get; set; }
        [ScriptIgnore]
        [Browsable(false)]
        public DateTime? UpdatedAt { get; set; }
        [Browsable(false)]
        public bool HasAttachments
        {
            get { return false; }
        }

        [Browsable(false)]
        public string CreatedByName { get; set; }
        [Browsable(false)]
        public bool CanDelete { get; set; }
        [Browsable(false)]
        public int? CompanyId { get; set; }
        [Browsable(false)]
        public string UpdatedById { get; set; }


        [DisplayName("Task No")]
        public int? TaskNo { get; set; }
        [DisplayName("Created Date")]
        public string CreatedAtVw
        {
            get { return CreatedAt.HasValue ? CreatedAt.Value.ToString(Constants.DateTimeFormat) : string.Empty; }
        }
        [DisplayName("Title")]
        public string Title { get; set; }
        [DisplayName("Description")]
        public string Description { get; set; }
        [DisplayName("Assign To")]
        public string AssignToName { get; set; }
        [DisplayName("Priority")]
        public string PriorityName
        {
            get
            {
                if (!PriorityId.HasValue)
                {
                    PriorityId = (int)TaskPriority.Normal;
                }
                return EnumUtility.GetDescriptionFromEnumValue((TaskPriority)PriorityId);
            }
        }
        [DisplayName("Due Date")]
        public string DueDateVw
        {
            get;set;
        }
        [DisplayName("Status")]
        public string StatusName
        {
            get
            {
                if (!StatusId.HasValue)
                    return string.Empty;
                return EnumUtility.GetDescriptionFromEnumValue((TaskStatus)StatusId);
            }
        }


        [Browsable(false)]
        public string UpdatedAtVw
        {
            get { return UpdatedAt.HasValue ? UpdatedAt.Value.ToString(Constants.DateTimeLongFormat) : string.Empty; }
        }





        
    }

    public class TaskAttachment
    {
        public string Id { get; set; }
        public string TaskId { get; set; }
        public string FileName { get; set; }
        public string BlobName { get; set; }
        public string FileExtention { get { return !string.IsNullOrEmpty(BlobName) ? Path.GetExtension(BlobName) : null; } }

        public string UpdatedById { get; set; }
        [ScriptIgnore]
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedAtVw
        {
            get { return UpdatedAt.HasValue ? UpdatedAt.Value.ToString(Constants.DateTimeLongFormat) : string.Empty; }
        }
    }
    public class TaskGroupModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BackGroundColor { get; set; }
        public string CreatedById { get; set; }
        public DateTime? CreatedAt { get; set; }

        public int? TotalTask { get; set; }

    }
    public class ToDoTaskShareModel
    {
        public string Id { get; set; }
        public string TaskId { get; set; }
        public string ShareWithId { get; set; }

    }
    public class ToDoTaskModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CreatedById { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool Completed { get; set; }

    }

    public enum TaskStatus
    {
        [Description("To Do")]
        ToDo = 1,
        [Description("In Progress")]
        InProgress = 2,
        [Description("Pause")]
        Pause = 3,
        [Description("Completed")]
        Completed = 4,
        [Description("Done & Bill Collected")]
        BillCollected = 5,
        [Description("Cancelled")]
        Cancelled = 6
    }

    public enum TaskPriority
    {
        [Description("Normal")]
        Normal = 1,
        [Description("High")]
        High = 2,
        [Description("Low")]
        Low = 3
    }
}
