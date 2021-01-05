using ToDoApp.Common;
using ToDoApp.Common.Models;

namespace ToDoApp.Web.Helpers
{
    public static class CommonUtility
    {
        static CommonUtility()
        {

        }
        public static UserSessionModel GetCurrentUser()
        {
            return System.Web.HttpContext.Current.Session[Constants.CurrentUser] as UserSessionModel;
        }
        public static UserLanguageSessionModel GetCurrentUserLanguage()
        {
            try
            {
                return System.Web.HttpContext.Current.Session[Constants.CurrentLanguage] as UserLanguageSessionModel;
            }
            catch
            {
                return null;
            }
        }
        public static UserSessionModel UserDetails
        {
            get
            {
                var loggedInUser = GetCurrentUser();
                return loggedInUser != null ? loggedInUser : new UserSessionModel();
            }
        }
        public static bool IsSuperAdmin
        {
            get
            {
                var loggedInUser = GetCurrentUser();
                return loggedInUser != null && (UserType)loggedInUser.UserTypeId == UserType.SuperAdmin;
            }
        }
        public static bool IsAdmin
        {
            get
            {
                var loggedInUser = GetCurrentUser();
                return loggedInUser != null && (UserType)loggedInUser.UserTypeId == UserType.Admin;
            }
        }
        public static UserLanguageModel UserLanguageMenu
        {
            get
            {
                var loggedInUserLanguage = GetCurrentUserLanguage();
                if (loggedInUserLanguage != null && loggedInUserLanguage.Language == "en")
                {
                    return new UserLanguageModel
                    {
                        Dashboard = "Dashboard",
                        Company = "Company",
                        Employees = "Employees",
                        Leaves = "Leaves",
                        Reports = "Reports",
                        Tasks = "Tasks",
                        Setting = "Setting",
                        SickType = "SickType"
                    };
                }
                else
                {
                    return new UserLanguageModel
                    {
                        Dashboard = "Tableau de bord",
                        Company = "Entreprise",
                        Employees = "Employés",
                        Leaves = "Abscence",
                        Reports = "Rapports",
                        Tasks = "Tâches",
                        Setting = "Réglage",
                        SickType = "SickType"
                    };
                }
            }
        }
    }
}