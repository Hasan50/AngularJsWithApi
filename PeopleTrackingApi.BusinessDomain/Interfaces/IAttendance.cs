using PeopleTrackingApi.Common.Models;
using System;
using System.Collections.Generic;
using PeopleTrackingApi.BusinessDomain.Models;

namespace PeopleTrackingApi.BusinessDomain.Interfaces
{
    public interface IAttendance
    {
        ResponseModel CheckIn(AttendanceEntryModel model);
        ResponseModel CheckOut(AttendanceEntryModel model);
        ResponseModel SaveCheckPoint(UserMovementLogModel model);
        ResponseModel SaveCheckPointDetail(UserMovementLogDetailsModel model);
        List<AttendanceModel> GetAttendanceFeed(DateTime date);
        List<AttendanceModel> GetAttendanceCompanyAgentFeed(string companyAgentId, DateTime date);
        List<AttendanceModel> GetAttendance(DateTime startDate, DateTime endDate);
        List<AttendanceModel> GetAttendance(string userId,DateTime startDate, DateTime endDate);
        List<UserMovementLogModel> GetMovementDetails(string userId, DateTime date);
        List<UserMovementLogModel> GetMovementDetailsAll(DateTime date);
        AttendanceModel GetMyTodayAttendance(string userId, DateTime date);
        List<AttendanceModel> GetAttendanceWithAgent(string agentId, DateTime startDate, DateTime endDate);

    }
}
