using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp.BusinessDomain.Models
{
    public class RowsCount
    {
        public int Rows { get; set; }
    }

    public class StringReturnModel
    {
        public string Value { get; set; }
    }
    public class SuccessModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
    public class ValueCount
    {
        public int? TotalCount { get; set; }
    }
}
