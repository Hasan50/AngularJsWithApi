using System;
using System.ComponentModel;

namespace ToDoApp.BusinessDomain.Models
{
    public class EmployeeCompanyAgent
    {
        [Browsable(false)]
        public int Id { get;set; }
        [Browsable(false)]
        public int EmployeeUserId { get; set; }
        [Browsable(false)]
        public int CompanyAgentId { get; set; }
        [DisplayName("Agent Name")]
        public string AgentName { get; set; }
        [Browsable(false)]
        public bool IsActive { get; set; }
        [Browsable(false)]
        public DateTime? CreatedDate { get; set; }
        
    }
}
