using ToDoApp.BusinessDomain.Models;
using System.Collections.Generic;

namespace ToDoApp.Web.Services
{

    internal interface ILocationBarService
    {
        List<LocationBarViewModel> GetLocatioBarData(List<UserMovementLogModel> result);
    }
}