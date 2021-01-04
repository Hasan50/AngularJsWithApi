using PeopleTrackingApi.BusinessDomain.Models;
using PeopleTrackingApi.Common;
using System;
using System.Collections.Generic;

namespace PeopleTrackingApi.Web.Services
{
    /// <summary>
    /// at least 3 record.
    /// bar total width 720
    /// calculate width using 2 records comparing logic
    /// IsGapBarPin depends on one check and another check out time gap. if checkin then IsGapBarPin must be tru. if checkout then compare with next record
    /// </summary>
    internal class MultipleBarLocationService : ILocationBarService
    {
        public List<LocationBarViewModel> GetLocatioBarData(List<UserMovementLogModel> result)
        {
            var data = new List<LocationBarViewModel>();
            var count = 960 / result.Count;
            var barWidth = 0;
            for (var i = 0; i < result.Count; i++)
            {
                var x = result[i];

                //calculate width and set. bar total width 720
                int width = count;
                var isGapBar = false;
                if (x.IsCheckOutPoint.HasValue && x.IsCheckOutPoint.Value)
                {
                    isGapBar = true;
                }
                double timeDiff = 0;
                var isStarting = false;
                if (i != 0 && i != result.Count - 1)
                {
                    DateTime date1 = result[i].LogDateTime.HasValue ? result[i].LogDateTime.Value : new DateTime();
                    DateTime date2 = result[i + 1].LogDateTime.HasValue ? result[i + 1].LogDateTime.Value : new DateTime();
                    TimeSpan ts = date2.ToZoneTimeBD() - date1.ToZoneTimeBD();
                    timeDiff = Math.Round(ts.TotalMinutes) < 1 ? 1 : Math.Round(ts.TotalMinutes);
                }
                if (i == 0) //Starting time difference
                {
                    DateTime s = result[0].LogDateTime.HasValue ? result[0].LogDateTime.Value : new DateTime();
                    TimeSpan ts = new TimeSpan(12, 0, 0);
                    s = s.Date + ts;
                    DateTime date1 = s;
                    DateTime date2 = result[0].LogDateTime.HasValue ? result[0].LogDateTime.Value : new DateTime();
                    TimeSpan tss = date1.ToZoneTimeBD() - date2.ToZoneTimeBD();
                    timeDiff = Math.Round(tss.TotalMinutes) < 1 ? 1 : Math.Round(tss.TotalMinutes);
                    isStarting = true;

                    var dataMode = new LocationBarViewModel
                    {
                        LogTimeVw = "00:00:00",
                        UserId = x.UserId,
                        UserName = x.UserName,
                        Lattitude = x.Latitude,
                        Longitude = x.Longitude,
                        LogDateTime = x.LogDateTime,
                        LogLocation = x.LogLocation,
                        Title = "00:00", //string.Format("{0} {1}", (x.IsCheckInPoint.HasValue && x.IsCheckInPoint.Value) ? "In" : "Out", x.LogTimeVw),
                        Width = (int)timeDiff,
                        IsGapBarPin = isGapBar,
                        IsStarting = isStarting,
                        IsLastOut = i == result.Count - 1 && x.IsCheckOutPoint.HasValue && x.IsCheckOutPoint.Value,
                        LastTitle = i == result.Count - 1 ? x.LogTimeVw : null//string.Format("{0} {1}", (x.IsCheckInPoint.HasValue && x.IsCheckInPoint.Value) ? "In" : "Out", x.LogTimeVw) : null,
                    };
                    data.Add(dataMode);
                    barWidth += (int)timeDiff;
                }
                if (i == 0 && isStarting)
                {
                    DateTime date1 = result[i].LogDateTime.HasValue ? result[i].LogDateTime.Value : new DateTime();
                    DateTime date2 = result[i + 1].LogDateTime.HasValue ? result[i + 1].LogDateTime.Value : new DateTime();
                    TimeSpan ts = date2.ToZoneTimeBD() - date1.ToZoneTimeBD();
                    timeDiff = Math.Round(ts.TotalMinutes) < 1 ? 1 : Math.Round(ts.TotalMinutes);
                    isStarting = false;
                }
                width = (int)timeDiff;
                //if (i == 0 && isStarting==true) //Starting time and pin
                //{
                //    var dataMode = new LocationBarViewModel
                //    {
                //        LogTimeVw = "00:00:00",
                //        UserId = x.UserId,
                //        UserName = x.UserName,
                //        Lattitude = x.Latitude,
                //        Longitude = x.Longitude,
                //        LogDateTime = x.LogDateTime,
                //        LogLocation = x.LogLocation,
                //        Title = "00:00", //string.Format("{0} {1}", (x.IsCheckInPoint.HasValue && x.IsCheckInPoint.Value) ? "In" : "Out", x.LogTimeVw),
                //        Width = width,
                //        IsGapBarPin = isGapBar,
                //        IsStarting = isStarting,
                //        IsLastOut = i == result.Count - 1 && x.IsCheckOutPoint.HasValue && x.IsCheckOutPoint.Value,
                //        LastTitle = i == result.Count - 1 ? x.LogTimeVw : null//string.Format("{0} {1}", (x.IsCheckInPoint.HasValue && x.IsCheckInPoint.Value) ? "In" : "Out", x.LogTimeVw) : null,
                //    };
                //    data.Add(dataMode);
                //    barWidth += width;
                //    isStarting = false;
                //}
                var dataModel = new LocationBarViewModel
                {
                    LogTimeVw = x.LogTimeVw,
                    UserId = x.UserId,
                    UserName = x.UserName,
                    Lattitude = x.Latitude,
                    Longitude = x.Longitude,
                    LogDateTime = x.LogDateTime,
                    LogLocation = x.LogLocation,
                    Title = x.LogTimeVw, //string.Format("{0} {1}", (x.IsCheckInPoint.HasValue && x.IsCheckInPoint.Value) ? "In" : "Out", x.LogTimeVw),
                    Width = width <35?35:width,
                    IsGapBarPin = isGapBar,
                    IsStarting = isStarting,
                    IsLastOut = i == result.Count - 1 && x.IsCheckOutPoint.HasValue && x.IsCheckOutPoint.Value,
                    LastTitle = i == result.Count - 1 ? x.LogTimeVw : null//string.Format("{0} {1}", (x.IsCheckInPoint.HasValue && x.IsCheckInPoint.Value) ? "In" : "Out", x.LogTimeVw) : null,
                };
                data.Add(dataModel);
                barWidth += width;
            }
            return ReturnData(data, barWidth);
        }

        public List<LocationBarViewModel> ReturnData(List<LocationBarViewModel> models,int barWidth)
        {
            if (barWidth <= 960)
                return models;

            barWidth = 0;
            foreach (var item in models)
            {
                item.Width = item.Width>40? item.Width - 10:item.Width;
                barWidth += item.Width;
            }
            return ReturnData(models, barWidth);
        }
    }
}