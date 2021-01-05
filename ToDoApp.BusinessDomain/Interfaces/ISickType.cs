using ToDoApp.Common.Models;
using System.Collections.Generic;
using ToDoApp.BusinessDomain.Models;

namespace ToDoApp.BusinessDomain.Interfaces
{
    public interface ISickType
    {
        List<SickType> GetAllSickType();
        ResponseModel Create(SickType model);
        ResponseModel DeleteSickType(int id);
        List<SickType> GetSickTypeById(int id);
    }
}