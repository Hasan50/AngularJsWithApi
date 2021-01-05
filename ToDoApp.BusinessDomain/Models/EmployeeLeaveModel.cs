using ToDoApp.Common;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoApp.BusinessDomain.Models
{
    public class EmployeeLeaveModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Browsable(false)]
        public int Id { get; set; }
        [Browsable(false)]
        public int EmployeeId { get; set; }
        
        [Browsable(false)]
        public DateTime FromDate { get; set; }
        [Browsable(false)]
        public DateTime ToDate { get; set; }
        [Browsable(false)]
        public bool? IsHalfDay { get; set; }
        [Browsable(false)]
        public int LeaveTypeId { get; set; }
        [Browsable(false)]
        public int? SickTypeId { get; set; }


        [Browsable(false)]
        public string CreatedAt { get; set; }
        [Browsable(false)]
        public bool IsApproved { get; set; }
        [Browsable(false)]
        public bool IsRejected { get; set; }
        [Browsable(false)]
        public string RejectReason { get; set; }

        [Browsable(false)]
        public string ApprovedById { get; set; }
        [Browsable(false)]
        public DateTime? ApprovedAt { get; set; }
        [Browsable(false)]
        public string LeaveApplyFrom { get; set; }
        [Browsable(false)]
        public string LeaveApplyTo { get; set; }
        [Browsable(false)]
        public string UserId { get; set; }
        [Browsable(false)]
        public int CompanyId { get; set; }
        [DisplayName("Requester Name")]
        public string EmployeeName { get; set; }
        [DisplayName("Leave From")]
        public string FromDateVw
        {
            get { return FromDate.ToString(Constants.DateFormat); }
        }
        [DisplayName("Leave To")]
        public string ToDateVw
        {
            get { return ToDate.ToString(Constants.DateFormat); }
        }
        [DisplayName("Leave Type")]
        public string LeaveType
        {
            get
            {
                return EnumUtility.GetDescriptionFromEnumValue((LeaveType)LeaveTypeId);
            }
        }
        [DisplayName("Sick Type")]
        public string SickType { get; set; }

        [DisplayName("Leave For")]
        public int LeaveInDays
        {
            get { return ((int)ToDate.Subtract(FromDate).TotalDays) + 1; }
        }

        [DisplayName("Leave Reason")]
        public string LeaveReason { get; set; }

        [Browsable(false)]
        public string ApprovedAtVw
        {
            get { return ApprovedAt.HasValue? ApprovedAt.Value.ToString(Constants.DateFormat) :string.Empty; }
        }

        [Browsable(false)]

        public string ApprovedBy { get; set; }

        [DisplayName("Status")]
        public string LeaveStatus
        {
            get { return IsApproved ? "Approved" : (IsRejected?"Rejected":"Pending"); }
        }
    }

    public enum LeaveType
    {
        [Description("Casual Leave")]
        CasualLeave = 1,
        [Description("Sick Leave")]
        SickLeave = 2
    }
}
