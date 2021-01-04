using System.ComponentModel;

namespace PeopleTrackingApi.BusinessDomain.Models
{
    public class EmployeeUser 
    {
        [Browsable(false)]
        public int Id { get; set; }
        [Browsable(false)]
        public string UserId { get; set; }
        [Browsable(false)]
        public int CompanyId { get; set; }
        [Browsable(false)]
        public int CompanyAgentId { get; set; }
        [DisplayName("Employee Name")]
        public string UserName { get; set; }
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("Company Name")]
        public string CompanyName { get; set; }
        [DisplayName("Company Agent")]
        public string CompanyAgentName { get; set; }

        [Browsable(false)]
        public string Designation { get; set; }
        [Browsable(false)]
        public string GeofanceLat { get; set; }
        [Browsable(false)]
        public string GeofanceLng { get; set; }
        [Browsable(false)]
        public string ImageFileName { get; set; }
        [Browsable(false)]
        public string ImageFileId { get; set; }
        [Browsable(false)]
        public bool? IsAutoCheckPoint { get; set; }
        [Browsable(false)]
        public string AutoCheckPointTime { get; set; }
        [Browsable(false)]
        public string MaximumOfficeHours { get; set; }
        [Browsable(false)]
        public string OfficeOutTime { get; set; }
        [Browsable(false)]
        public string DepartmentName { get; set; }
        [Browsable(false)]
        public bool IsActive { get; set; }
        [Browsable(false)]
        public string EmployeeCode { get; set; }
        [Browsable(false)]
        public string Gender { get; set; }

        [DisplayName("Status")]
        public string ActiveStatus
        {
            get { return IsActive ? "Active" : "In Active"; }
        }
        [Browsable(false)]
        public string GeofanceLocation { get; set; }
    }
}
