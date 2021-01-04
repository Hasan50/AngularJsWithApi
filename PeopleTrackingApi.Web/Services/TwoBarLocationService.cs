using PeopleTrackingApi.BusinessDomain.Models;
using System.Collections.Generic;

namespace PeopleTrackingApi.Web.Services
{
    internal class TwoBarLocationService : ILocationBarService
    {
        public List<LocationBarViewModel> GetLocatioBarData(List<UserMovementLogModel> result)
        {
            var x = result[0];
            var y = result[1];
            var data = new List<LocationBarViewModel>();
            var dataModel = new LocationBarViewModel
            {
                LogTimeVw = x.LogTimeVw,
                UserId = x.UserId,
                UserName = x.UserName,
                Lattitude = x.Latitude,
                Longitude = x.Longitude,
                LogDateTime = x.LogDateTime,
                LogLocation = x.LogLocation,
                Title = string.Format("In at {0}", x.LogTimeVw),
                Width = 960,
                IsLastOut = true,
                LastTitle = string.Format("Out at {0}", y.LogTimeVw),
                LastPinMap = new LastLocationBarDetailsModel()
            };
            dataModel.LastPinMap.Lattitude = y.Latitude;
            dataModel.LastPinMap.Longitude = y.Longitude;
            dataModel.LastPinMap.LogDateTime = y.LogDateTime;
            dataModel.LastPinMap.LogLocation = y.LogLocation;
            dataModel.LastPinMap.UserId = y.UserId;
            dataModel.LastPinMap.UserName = y.UserName;

            data.Add(dataModel);

            return data;
        }
    }
}