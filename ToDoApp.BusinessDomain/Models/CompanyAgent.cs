using System;
using System.ComponentModel;

namespace ToDoApp.BusinessDomain.Models
{
    public class CompanyAgent
    {
        [Browsable(false)]
        public int Id { get;set; }
        [Browsable(false)]
        public int CompanyId { get; set; }
        [DisplayName("Agent Name")]
        public string AgentName { get; set; }
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("Contact Person")]
        public string ContactPersonName { get; set; }
        [DisplayName("Address")]
        public string Address { get; set; }
        [Browsable(false)]
        public string GeofanceLat { get; set; }
        [Browsable(false)]
        public string GeofanceLng { get; set; }


        [DisplayName("Office Start Time")]
        public string OfficeStartTime { get; set; }
        [DisplayName("Office End Time")]
        public string OfficeEndTime { get; set; }
        [Browsable(false)]
        public bool IsActive { get; set; }
        [Browsable(false)]
        public DateTime? CreatedDate { get; set; }
        [Browsable(false)]
        public string CreatedById { get; set; }
        [DisplayName("Status")]
        public string ActiveStatus
        {
            get { return IsActive ? "Active" : "In Active"; }
        }
    }
}
