using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp.BusinessDomain.Models
{
    public class SickType
    {
        [Browsable(false)]
        [Key]
        public int Id { get; set; }
        [Browsable(false)]
        public int CompanyId { get; set; }
        [DisplayName("Client Name")]
        public string Name { get; set; }
        [DisplayName("Client Name")]
        public string CompanyName { get; set; }
        [Browsable(false)]
        public string CreateDate { get; set; }
    }
}
