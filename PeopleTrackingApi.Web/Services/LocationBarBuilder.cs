using PeopleTrackingApi.BusinessDomain.Models;
using System.Collections.Generic;

namespace PeopleTrackingApi.Web.Services
{
    public class LocationBarBuilder
    {
        public List<LocationBarViewModel> Build(List<UserMovementLogModel> result)
        {
            ILocationBarService locationBarService;
            switch (result.Count)
            {
                case 1:
                    locationBarService = new OneBarLocationService();
                    break;
                //case 2:
                //    locationBarService = new TwoBarLocationService();
                //    break;
                default:
                    locationBarService = new MultipleBarLocationService();
                    break;
            }
            return locationBarService.GetLocatioBarData(result);
        }
    }
}