﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp.BusinessDomain.Models
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string Designation { get; set; }
        public bool IsAutoCheckPoint { get; set; }
        public string DepartmentName { get; set; }
        public int? DepartmentId { get; set; }
        public string GeofanceLat { get; set; }
        public string GeofanceLng { get; set; }
        public string AutoCheckPointTime { get; set; }
        public string MaximumOfficeHours { get; set; }
        public string OfficeOutTime { get; set; }
        public string ImageFileName { get; set; }
        public string ImageFileId { get; set; }
        public string Checkin { get; set; }
        public string Checkout { get; set; }
        public string CheckinTime { get; set; }
        public string CheckOutTime { get; set; }

        public bool IsCheckedIn
        {
            get
            {
                return !string.IsNullOrEmpty(Checkin) && string.IsNullOrEmpty(Checkout);
            }
        }
        public bool IsCheckedOut
        {
            get
            {
                return !string.IsNullOrEmpty(Checkin) && !string.IsNullOrEmpty(Checkout);
            }
        }

        public bool NotAttend
        {
            get
            {
                return string.IsNullOrEmpty(Checkin) && string.IsNullOrEmpty(Checkout);
            }
        }

    }
}
