using PeopleTrackingApi.Common;
using System;

namespace PeopleTrackingApi.BusinessDomain.Models
{
    public class LocationBarViewModel
    {
        public string Title { get; set; }
        public int Width { get; set; }
        public bool IsGapBarPin { get; set; }
        public bool IsLastOut { get; set; }
        public bool IsStarting { get; set; }
        public string LastTitle { get; set; }
        public string LogTimeVw { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string LogLocation { get; set; }
        public decimal? Lattitude { get; set; }
        public decimal? Longitude { get; set; }
        public DateTime? LogDateTime { get; set; }
        public string LogDateTimeVw
        {
            get
            {
                return LogDateTime.HasValue ? LogDateTime.Value.ToZoneTimeBD().ToString(Constants.DateLongFormat) : string.Empty;
            }
        }

        public LastLocationBarDetailsModel LastPinMap { get; set; }
        public bool HasLastPinMap
        {
            get { return LastPinMap != null; }
        }
    }

    public class LastLocationBarDetailsModel
    {
        public string LogTimeVw { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string LogLocation { get; set; }
        public decimal? Lattitude { get; set; }
        public decimal? Longitude { get; set; }
        public DateTime? LogDateTime { get; set; }
        public string LogDateTimeVw
        {
            get
            {
                return LogDateTime.HasValue ? LogDateTime.Value.ToZoneTimeBD().ToString(Constants.DateLongFormat) : string.Empty;
            }
        }
    }
}
