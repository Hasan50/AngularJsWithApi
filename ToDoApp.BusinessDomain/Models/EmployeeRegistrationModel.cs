using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp.BusinessDomain.Models
{
    public class EmployeeRegistrationModel
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public string UserType { get; set; }
        public string GeofanceLat { get; set; }
        public string GeofanceLng { get; set; }
        public string Designation { get; set; }
        public string DepartmentId { get; set; }
        public int CompanyAgentId { get; set; }
        public bool IsAutoCheckPoint { get; set; }
        public string AutoCheckPointTime { get; set; }
        public string MaximumOfficeHours { get; set; }
        public string OfficeOutTime { get; set; }
        public string EmployeeCode { get; set; }
        public string GeofanceLocation { get; set; }
    }
}
