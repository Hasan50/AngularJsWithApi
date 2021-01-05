using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ToDoApp.BusinessDomain.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public string UserId { get; set; }
    }
}
