using PeopleTrackingApi.Common.Models;
using System.Collections.Generic;
using PeopleTrackingApi.BusinessDomain.Models;

namespace PeopleTrackingApi.BusinessDomain.Interfaces
{
    public interface ISickType
    {
        List<SickType> GetAllSickType();
        ResponseModel Create(SickType model);
        ResponseModel DeleteSickType(int id);
        List<SickType> GetSickTypeById(int id);
    }
}