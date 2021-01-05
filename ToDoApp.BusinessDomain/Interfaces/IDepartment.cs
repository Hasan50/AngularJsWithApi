using ToDoApp.Common.Models;
using System.Collections.Generic;
using ToDoApp.BusinessDomain.Models;

namespace ToDoApp.BusinessDomain.Interfaces
{
    public interface IDepartment
    {
        List<Department> GetDepartment();
        Department Create(Department model,string userId);
        ResponseModel UpdateDepartment(Department model);
        ResponseModel DeleteDepartmentById(string id);
    }
}