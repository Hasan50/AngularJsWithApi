using System;
using System.ComponentModel;

namespace ToDoApp.BusinessDomain.Models
{
    public class Company
    {
        [Browsable(false)]
        public int Id { get; set; }
        [Browsable(false)]
        public bool IsDateTripHistory { get; set; }
        [Browsable(false)]
        public bool IsShowTaskMenu { get; set; }
        [Browsable(false)]
        public bool IsShowLeaveMenu { get; set; }
        [Browsable(false)]
        public bool IsShowTaskDateFilter { get; set; }
        [Browsable(false)]
        public bool IsMobileAppAccesible { get; set; }
        [Browsable(false)]
        public bool IsActive { get; set; }
        [Browsable(false)]
        public DateTime? CreatedDate { get; set; }
        [Browsable(false)]
        public string CreatedById { get; set; }
        [Browsable(false)]
        public string GeofanceLat { get; set; }
        [Browsable(false)]
        public string GeofanceLng { get; set; }

        [Browsable(false)]
        public string ContactPersonName { get; set; }
        [Browsable(false)]
        public string CompanyAdminPassword { get; set; }
        [Browsable(false)]
        public string UserLanguage { get; set; }
        [DisplayName("Client Name")]
        public string CompanyName { get; set; }
        [DisplayName("Contact No")]
        public string PhoneNumber { get; set; }
        [DisplayName("Admin Name")]
        public string CompanyAdminName { get; set; }
        [DisplayName("Login ID")]
        public string CompanyAdminLoginID { get; set; }
        [DisplayName("Geo Fance Time")]
        public int? GeofanceTime { get; set; }

        [DisplayName("Geo Fance Region")]
        public int? GeofanceRadious { get; set; }
        [DisplayName("Date Trip History")]
        public string DateTripHistory
        {
            get { return IsDateTripHistory ? "Yes" : "No"; }
        }
        [DisplayName("Task Menu")]
        public string ShowTaskMenu
        {
            get { return IsShowTaskMenu ? "Yes" : "No"; }
        }

        [DisplayName("Leave Menu")]
        public string ShowLeaveMenu
        {
            get { return IsShowLeaveMenu ? "Yes" : "No"; }
        }
        [DisplayName("Task Date Filter")]
        public string ShowTaskDateFilter
        {
            get { return IsShowTaskDateFilter ? "Yes" : "No"; }
        }
        [DisplayName("Mobile App Accesible")]
        public string MobileAppAccesible
        {
            get { return IsMobileAppAccesible ? "Yes" : "No"; }
        }
        [DisplayName("Status")]
        public string ActiveStatus
        {
            get { return IsActive ? "Active" : "In Active"; }
        }
        [Browsable(false)]
        public string Address { get; set; }

        [Browsable(false)]
        public string CompanyAdminEmail { get; set; }



      

    }
}
