using PeopleTrackingApi.BusinessDomain.Models;
using System.Collections.Generic;

namespace PeopleTrackingApi.Web.Services
{

    internal interface ILocationBarService
    {
        List<LocationBarViewModel> GetLocatioBarData(List<UserMovementLogModel> result);
    }
}