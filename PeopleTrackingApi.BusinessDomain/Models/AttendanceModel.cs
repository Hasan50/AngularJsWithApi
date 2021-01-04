using PeopleTrackingApi.Common;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Web.Script.Serialization;

namespace PeopleTrackingApi.BusinessDomain.Models
{

    public class AttendanceEntryModel
    {
        public string UserId { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string LogLocation { get; set; }
        public string DeviceName { get; set; }
        public string DeviceOSVersion { get; set; }
        public string Reason { get; set; }

        public int? OfficeHour { get; set; }
        public int? AllowOfficeLessTime { get; set; }
        public bool? IsCheckIn { get; set; }
        public bool? IsCheckOut { get; set; }
        public bool? IsLeave { get; set; }

        public string CheckInTimeFile { get; set; }
        public string CheckOutTimeFile { get; set; }
        public int? CompanyId { get; set; }
        public int? CompanyAgentId { get; set; }

    }
    public class AttendanceTotalModel
    {
        [Browsable(false)]
        public string UserId { get; set; }
        [DisplayName("Employee Name")]
        public string EmployeeName { get; set; }
        [DisplayName("Present(Days)")]
        public int? TotalPresent { get; set; }
        [Browsable(false)]
        public string Designation { get; set; }
        [Browsable(false)]
        public string DepartmentName { get; set; }
        [Browsable(false)]
        public string ImageFileName { get; set; }

        [DisplayName("Missing Out Time")]
        public int? TotalCheckedOutMissing { get; set; }
        [DisplayName("Completed Hours")]
        public string TotalStayTime { get; set; }
        public string TotalOfficeHour { get; set; }
        [DisplayName("Over/Due Time")]
        public string OvertimeOrDueHour { get; set; }
        [DisplayName("Leave(Days)")]
        public int? TotalLeave { get; set; }
        [Browsable(false)]
        public double TotalScore { get; set; }
        [Browsable(false)]
        public int? AgentId { get; set; }
    }
    public class AttendanceModel
    {
        [Browsable(false)]
        public int Id { get; set; }
        [Browsable(false)]
        public string UserId { get; set; }
        [Browsable(false)]
        [ScriptIgnore]
        public DateTime? AttendanceDate { get; set; }
        [Browsable(false)]
        [ScriptIgnore]
        public DateTime? CheckInTime { get; set; }
        [Browsable(false)]
        [ScriptIgnore]
        public DateTime? CheckOutTime { get; set; }
        [Browsable(false)]
        public bool? IsLeave { get; set; }
        [Browsable(false)]
        public string OfficeStartTime { get; set; }
        [Browsable(false)]
        public string OfficeEndTime { get; set; }

        [Browsable(false)]
        public string CheckInTimeFile { get; set; }
        [Browsable(false)]
        public string CheckOutTimeFile { get; set; }


        [Browsable(false)]
        public string LessTimeReason { get; set; }
        [Browsable(false)]
        public int? DailyWorkingTimeInMin { get; set; }
        [Browsable(false)]
        public int? AllowOfficeLessTimeInMin { get; set; }

        [Browsable(false)]
        public int? EmployeeId { get; set; }
        [DisplayName("Employee Name")]
        public string EmployeeName { get; set; }
        [Browsable(false)]
        public string PhoneNumber { get; set; }
        [Browsable(false)]
        public string Designation { get; set; }
        [Browsable(false)]
        public string DepartmentName { get; set; }
        [Browsable(false)]
        public bool? IsAutoCheckPoint { get; set; }
        [Browsable(false)]
        public string AutoCheckPointTime { get; set; }
        [Browsable(false)]
        public string EmployeeCode { get; set; }
       
        [Browsable(false)]
        public int? AgentId { get; set; }
        [Browsable(false)]
        public string GeofanceLat { get; set; }
        [Browsable(false)]
        public string GeofanceLng { get; set; }
        [Browsable(false)]
        public int? GeofanceTime { get; set; }
        [Browsable(false)]
        public int? GeofanceRadious { get; set; }
        [Browsable(false)]
        public bool? IsCheckIn { get; set; }
        [Browsable(false)]
        public bool? IsCheckOut { get; set; }
        [DisplayName("Date")]
        public string AttendanceDateVw
        {
            get { return AttendanceDate.HasValue ? AttendanceDate.Value.ToZoneTimeBD().ToString(Constants.DateLongFormat) : string.Empty; }
        }
        [DisplayName("Agent Name")]
        public string AgentName { get; set; }
        [Browsable(false)]
        public string AttendancceDayName
        {
            get { return AttendanceDate.HasValue ? AttendanceDate.Value.ToZoneTimeBD().ToString("ddd") : string.Empty; }
        }

        [Browsable(false)]
        public string AttendancceDayNumber
        {
            get { return AttendanceDate.HasValue ? string.Format("{0}",AttendanceDate.Value.ToZoneTimeBD().Day) : string.Empty; }
        }
        [DisplayName("In Time")]
        public string CheckInTimeVw
        {
            get { return CheckInTime.HasValue ? CheckInTime.Value.ToZoneTimeBD().ToString(Constants.TimeFormatMin) : (IsLeave.HasValue && IsLeave.Value?"Leave": string.Empty); }
        }
        [DisplayName("Out Time")]
        public string CheckOutTimeVw
        {
            get { return CheckOutTime.HasValue ? CheckOutTime.Value.ToZoneTimeBD().ToString(Constants.TimeFormatMin) : string.Empty; }
        }
        [DisplayName("Compleated Hours")]
        public string OfficeStayHour
        {
            get
            {
                if (!CheckInTime.HasValue)
                    return string.Empty;
                TimeSpan result = CheckOutTime.HasValue? CheckOutTime.Value.ToZoneTimeBD().Subtract(CheckInTime.Value.ToZoneTimeBD()): DateTime.UtcNow.ToZoneTimeBD().Subtract(CheckInTime.Value.ToZoneTimeBD());
                int hours = result.Hours;
                int minutes = result.Minutes;

                return string.Format("{0}:{1}",hours,minutes);
               }
        }

        [Browsable(false)]
        public bool IsCheckedIn
        {
            get
            {
                return IsCheckIn.HasValue && IsCheckIn==true;
            }
        }

        [Browsable(false)]
        public bool IsPresent
        {
            get
            {
                return CheckInTime.HasValue;
            }
        }
        [Browsable(false)]
        public bool IsCheckedOut
        {
            get
            {
                return IsCheckOut.HasValue && IsCheckOut==true;
            }
        }

        [Browsable(false)]
        public bool NotCheckedOut
        {
            get
            {
                return CheckInTime.HasValue && !CheckOutTime.HasValue;
            }
        }

        [Browsable(false)]
        public bool NotAttend
        {
            get
            {
                return !CheckInTime.HasValue && !CheckOutTime.HasValue;
            }
        }

        [Browsable(false)]
        public string ImageFileName { get; set; }

        [Browsable(false)]
        public string Isgeofancing
        {
            get
            {
                if (!CheckInTime.HasValue)
                    return "null";
                else
                    return "Value";
            }
        }
        [Browsable(false)]
        public string Status
        {
            get
            {
                if (CheckInTime.HasValue && !CheckOutTime.HasValue)
                    return "Checked-In";
                else if (CheckInTime.HasValue && CheckOutTime.HasValue)
                    return "Checked-Out";
                else if (!CheckInTime.HasValue && !CheckOutTime.HasValue && !IsLeave.HasValue)
                    return "Absent";
                else if (!CheckInTime.HasValue && !CheckOutTime.HasValue && IsLeave.HasValue && IsLeave.Value)
                    return "Leave";
                else
                    return string.Empty;
            }
        }
        [Browsable(false)]
        public int? TotalStayTimeInMinute
        {
            get
            {
                if (!CheckInTime.HasValue)
                    return 0;
                TimeSpan result = CheckOutTime.HasValue ? CheckOutTime.Value.ToZoneTimeBD().Subtract(CheckInTime.Value.ToZoneTimeBD()) : CheckInTime.Value.ToZoneTimeBD().Subtract(CheckInTime.Value.ToZoneTimeBD());
                int hours = result.Hours;
                int minutes = result.Minutes;

                return hours * 60 + minutes;
            }
        }

    }
    public class UserMovementLogModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public DateTime? LogDateTime { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string LogLocation { get; set; }
        public bool? IsCheckInPoint { get; set; }
        public bool? IsCheckOutPoint { get; set; }
        public string DeviceName { get; set; }
        public string DeviceOSVersion { get; set; }
        public string LogTimeVw
        {
            get { return LogDateTime.HasValue ? LogDateTime.Value.ToZoneTimeBD().ToString(Constants.TimeFormatMin) : string.Empty; }
        }
    }
    public class UserMovementLogDetailsModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public DateTime? LogDateTime { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string LogLocation { get; set; }
        public bool? IsCheckInPoint { get; set; }
        public bool? IsCheckOutPoint { get; set; }
        public string DeviceName { get; set; }
        public string DeviceOSVersion { get; set; }
        public string LogTimeVw
        {
            get { return LogDateTime.HasValue ? LogDateTime.Value.ToZoneTimeBD().ToString(Constants.TimeFormatMin) : string.Empty; }
        }
    }

    public class AgentWiseAttendaneModel
    {
        [Browsable(false)]
        public int? AgentId { get; set; }
        [DisplayName("Entreprises")]
        public string AgentName { get; set; }
        [DisplayName("Nombres Employes")]
        public int? TotalEmployee { get; set; }
        [DisplayName("Total Temps")]
        public string TotalTime { get; set; }
        
    }
}
