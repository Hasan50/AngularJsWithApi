namespace ToDoApp.BusinessDomain.Models
{
    public class PortalUserViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public string LoggedOn { get; set; }
        public string ImageFileName { get; set; }
        public bool IsAutoCheckPoint { get; set; }
        public string AutoCheckPointTime { get; set; }
        public string MaximumOfficeHours { get; set; }
        public string OfficeOutTime { get; set; }
        public string ImageFileId { get; set; }
        public string DesignationName { get; set; }

        public bool IsActive { get; set; }
        public int CompanyAgentId { get; set; }
        public string EmployeeCode { get; set; }
        public string GeofanceLat { get; set; }
        public string GeofanceLng { get; set; }
    }
}
