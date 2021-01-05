﻿

(function () {
    var languageFactory = function ($http,$rootScope) {

        var getUserLanguage = function () {
            return $http.get('/MyClients/GetUserCompany',
                {})
                .then(function (response) {
                    return response;
                });
        };
        var setUserLanguage = function (lang) {
            return $http.get('/Language/SetLanguage?selectedLanguage='+lang,
                {})
                .then(function (response) {
                    return response;
                });
        };
        var getLanguageValue = function (lang) {
            if (lang == "en") {
                return {
                    Search: "Search",
                    TodayCheckin: "Today Check In",
                    TodayAttend: "Today Attend",
                    TodayAttendance: "Today Attendance",
                    Percentage: "Percentage",
                    TodayCheckout: "Today Checkout",
                    NoRrecordsToShowInThisView: 'Aucun enregistrement à afficher dans cette vue',
                    All: 'All',
                    Present: 'Present',
                    Absent: 'Absent',
                    EmployeeCode: 'Employee Code',
                    EmployeeCreate: 'Employee Create',
                    Name: 'Name',
                    Designation: 'Designation',
                    MobileNumber: 'Mobile Number',
                    Company: 'Company',
                    CompanyAgent: 'Company Agent',
                    Status: 'Status',
                    New: 'New',
                    EmployeeList: 'EMPLOYEE LIST',
                    NoRecords: 'No records to show in this view',
                    edit: 'edit',
                    Active: 'Active',
                    InActive: 'InActive',
                    EmployeeName: 'Employee Name',
                    Agent: 'Agent',
                    SelectAgent: "Select Agent",
                    SelectCompany: "Select Company",
                    Email: 'Email',
                    AutoCheckPoint: 'Auto Check Point',
                    ActiveInactive: "Active/Inactive",
                    Yes: 'Yes',
                    No: 'No',
                    CreateUpdateEmployee: "Create/Update Employee",
                    EmployeeCodeisrequired: 'Employee Code is required',
                    EmployeeNameisrequired: "Employee Name is required",
                    Save: 'Save',
                    MyAgent: 'MY AGENTS',
                    MyAgentCompany: 'My Agent Company',
                    AgentName: 'Agent Name',
                    Address: 'Address',
                    ContactNo: 'Contact No',
                    ContactPersonName: "Contact Person Name",
                    AgentAddress: 'Agent Address',
                    CreateUpdateAgents: 'Create/Update Agents',
                    isrequired: 'is required',
                    PhoneNumber: 'Phone Number',
                    Agentnameisrequired: 'Agent name is required',
                    LeaveList: 'LEAVE LIST',
                    FromDate: "From Date",
                    ToDate: "To Date",
                    Typeofusername: "Type of user name",
                    Requester: "Requester",
                    LeaveType: 'Leave Type',
                    LeaveReason: 'Leave Reason',
                    NoOfDays: "No Of Days",
                    APPROVED: "APPROVED",
                    REJECTED: "REJECTED",
                    PENDING: "PENDING",
                    Details: "Details",
                    Leavedetails: "Leave details",
                    LeaveInDays: "Leave In Days",
                    TaskList: "Task List",
                    Date: "Date",
                    CreateUpdateTask: "Create/Update Task",
                    Titleis: " Title is",
                    required: 'required',
                    SelectStatus: 'Select Status',
                    Pending: 'Pending',
                    AssignTo: "Assign To",
                    Priority: "Priority",
                    SelectPriority: "Select Priority",
                    Title: "Title",
                    ViewDetails: "View Details",
                    SelectEmployee: "Select Employee",
                    AllEmployee: "All Employee",
                    Login: "LOG IN",
                    TheSmartest: "The Smartest way to manage your business",
                    RememberMe: "Remember Me",
                    Enteryourusername: "Enter your username",
                    Enteryourpassword: "Enter your password",
                    TaskCreatedSuccessfully: "Task Created Successfully",
                    Success: "Success",
                    SomethingErrorTryagainlater: "Something Error.Try again later",
                    Error: "Error",
                    EmployeeUpdatedSuccessfully: "Employee Updated Successfully",
                    EmployeeCreatedSuccessfully: "Employee Created Successfully",
                    EmployeeDeletedSuccessfully: "Employee Deleted Successfully",
                    AgentSavedSuccessfully: "Agent Saved Successfully",
                    AgentUpdatedSuccessfully: "Agent Updated Successfully",
                    Logout: 'Logout',
                    Dashboard: "Dashboard",
                    InTime: "In Time",
                    OutTime: "Out Time",
                    WorkingHour: "Working Hour",
                    Department: "Department",
                    MaximumOfficeHours: "Maximum Office Hours",
                    OfficeOutTime: "Office Out Time",
                    LiveLocation: 'Live Location',
                    TotalClient: 'Total Client',

                    DATEWISEATTENDANCEREPORTS: "DATE WISE ATTENDANCE REPORTS",
                    PresenDays: "Present(Days)",
                    LeaveDays: "Leave(Days)",
                    CompletedHours: "Completed Hours",
                    OfficeHours: "Office Hours",
                    MissingOutTime: "Missing Out Time",
                    OverDueTime: "Over/Due Time",
                    AttendanceDetailsof: "Attendance Details of",
                    DueDate: "Due Date",
                    TotalEmployee: "Total Employee",
                    TotalCheckin: "Total Checkin",
                    TotalCheckout: "Total Checkout",
                    Tasks: "Tasks",
                    ExportToExcel: "Export To Excel",
                    TaskCreateDate: "Task Create Date",
                    TaskDueDate: "Task Due Date",
                    Reset: "Reset",
                    TaskNo: "Task No",
                    CreatedDate: "Created Date",
                    Description: "Description",
                    AddNewTask: "Add New Task",
                    Task: "Task",
                    Leave: "Leave",
                    RequesterName: "Requester Name",
                    LeaveInDays: "Leave In Days",
                    AttendanceReport: "Attendance Report",
                    PresentDays: "Present(Days)",
                    TaskDetails: "Task Details",
                    AttendanceDetailsOf: "Attendance Details Of",
                    AttendanceStatus: "Attendance Status",
                    CompleatedHours: "Compleated Hours",
                    SickType: "Sick Type",
                    WoringHour:"Working Hour"
                }
               
            } else {
                return {
                    Search: "rechercher",
                    TodayCheckin: "Today Check In",
                    TodayAttend: "Today Attend",
                    TodayAttendance: "Présent Aujourdhui",
                    Percentage: "Pourcentage",
                    TodayCheckout: "Today Checkout",
                    NoRrecordsToShowInThisView: 'Aucun enregistrement à afficher dans cette vue',
                    All: 'All',
                    Present: 'Present',
                    Absent: 'Absent',
                    EmployeeCode: 'Employee Code',
                    EmployeeCreate: 'Employee Create',
                    Name: 'Nom',
                    Designation: 'Designation',
                    MobileNumber: 'Telephone Mobile',
                    Company: 'Entreprise',
                    CompanyAgent: 'Nouvelle Entreprise',
                    Status: 'Status',
                    New: 'Nouveau',
                    EmployeeList: 'liste des employés',
                    NoRecords: 'Pas d\'enregistrement',
                    edit: 'edit',
                    Active: 'Active',
                    InActive: 'InActive',
                    EmployeeName: 'Nom employé',
                    Agent: 'Agent',
                    SelectAgent: "Select Agent",
                    SelectCompany: "Selectionner entreprise",
                    Email: 'Email',
                    AutoCheckPoint: 'Auto Check Point',
                    ActiveInactive: "Active/Inactive",
                    Yes: 'Yes',
                    No: 'No',
                    CreateUpdateEmployee: "Create/Update Employee",
                    EmployeeCodeisrequired: 'Employee Code is required',
                    EmployeeNameisrequired: "Employee Name is required",
                    Save: 'Enregistrer',
                    MyAgent: 'MY AGENTS',
                    MyAgentCompany: 'My Agent Company',
                    AgentName: 'Agent Name',
                    Address: 'Address',
                    ContactNo: 'Contact No',
                    ContactPersonName: "Contact Person Name",
                    AgentAddress: 'Agent Address',
                    CreateUpdateAgents: 'Create/Update Agents',
                    isrequired: 'is required',
                    PhoneNumber: 'Téléphone',
                    Agentnameisrequired: 'Agent name is required',
                    LeaveList: 'Liste Abscence',
                    FromDate: "From Date",
                    ToDate: "To Date",
                    Typeofusername: "Type of user name",
                    Requester: "Requester",
                    LeaveType: 'Type abscence',
                    LeaveReason: 'Raison Abscence',
                    NoOfDays: "No Of Days",
                    APPROVED: "APPROVE",
                    REJECTED: "REJETE",
                    PENDING: "En Attente",
                    Details: "Details",
                    Leavedetails: "Détails Abscence",
                    LeaveInDays: "Abscent Auj.",
                    TaskList: "LIste Tache",
                    Date: "Date",
                    CreateUpdateTask: "Create/Update Task",
                    Titleis: " Title is",
                    required: 'required',
                    SelectStatus: 'Select Status',
                    Pending: 'En attente',
                    AssignTo: "Assigner à",
                    Priority: "Priorité",
                    SelectPriority: "Priorité",
                    Title: "Titre",
                    ViewDetails: "Voir Details",
                    SelectEmployee: "Selectionner Employé",
                    AllEmployee: "Tous les Employés",
                    Login: "LOG IN",
                    TheSmartest: "The Smartest way to manage your business",
                    RememberMe: "Se souvenir",
                    Enteryourusername: "Login",
                    Enteryourpassword: "Mot de passe",
                    TaskCreatedSuccessfully: "Tache créée",
                    Success: "Success",
                    SomethingErrorTryagainlater: "Something Error.Try again later",
                    Error: "Error",
                    EmployeeUpdatedSuccessfully: "Employee Updated Successfully",
                    EmployeeCreatedSuccessfully: "Employee Created Successfully",
                    EmployeeDeletedSuccessfully: "Employee Deleted Successfully",
                    AgentSavedSuccessfully: "Agent Saved Successfully",
                    AgentUpdatedSuccessfully: "Agent Updated Successfully",
                    Logout: 'Logout',
                    Dashboard: "Dashboard",
                    InTime: "In Time",
                    OutTime: "Out Time",
                    WorkingHour: "Working Hour",
                    Department: "Department",
                    MaximumOfficeHours: "Maximum Office Hours",
                    OfficeOutTime: "Office Out Time",
                    LiveLocation: 'Live Location',
                    TotalClient: 'Total Client',

                    DATEWISEATTENDANCEREPORTS: "DATE WISE ATTENDANCE REPORTS",
                    PresenDays: "Present(Days)",
                    LeaveDays: "Leave(Days)",
                    CompletedHours: "Completed Hours",
                    OfficeHours: "Office Hours",
                    MissingOutTime: "Missing Out Time",
                    OverDueTime: "Over/Due Time",
                    AttendanceDetailsof: "Attendance Details of",
                    DueDate: "Due Date",
                    TotalEmployee: "Total Employee",
                    TotalCheckin: "Total Checkin",
                    TotalCheckout: "Total Checkout",
                    Tasks: "Tasks",
                    ExportToExcel: "Export To Excel",
                    TaskCreateDate: "Task Create Date",
                    TaskDueDate: "Task Due Date",
                    Reset: "Reset",
                    TaskNo: "Task No",
                    CreatedDate: "Created Date",
                    Description: "Description",
                    AddNewTask: "Add New Task",
                    Task: "Task",
                    Leave: "Leave",
                    RequesterName: "Requester Name",
                    LeaveInDays: "Leave In Days",
                    AttendanceReport: "Attendance Report",
                    PresentDays: "Present(Days)",
                    TaskDetails: "Task Details",
                    AttendanceDetailsOf: "Attendance Details Of",
                    AttendanceStatus: "Attendance Status",
                    CompleatedHours: "Compleated Hours",
                    SickType: "Sick Type",
                    WoringHour: "Woring Hour"


                }
            }
        };
      

        return {

            GetUserLanguage: getUserLanguage,
            SetUserLanguage: setUserLanguage,
            GetLanguageValue: getLanguageValue
        };
    };

    trackerApp.factory('languageFactory', languageFactory);

})();


