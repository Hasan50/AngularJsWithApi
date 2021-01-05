using ToDoApp.BusinessDomain.Models;
using System.Collections.Generic;

namespace ToDoApp.Web.Services
{
    internal class OneBarLocationService : ILocationBarService
    {
        public List<LocationBarViewModel> GetLocatioBarData(List<UserMovementLogModel> result)
        {
            var data = new List<LocationBarViewModel>
            {
                new LocationBarViewModel
                {
                    LogTimeVw=result[0].LogTimeVw,
                    UserId=result[0].UserId,
                    UserName=result[0].UserName,
                    Lattitude=result[0].Latitude,
                    Longitude=result[0].Longitude,
                    LogDateTime=result[0].LogDateTime,
                    LogLocation=result[0].LogLocation,
                    Title = string.Format("In at {0}", result[0].LogTimeVw),
                    Width = 960
                }
            };
            return data;
        }
    }
}