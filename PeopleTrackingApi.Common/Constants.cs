
using System;
using System.Collections.Generic;

namespace PeopleTrackingApi.Common
{
    public class Constants
    {
        public const string ConnectionStringName = "IsahakCon";
        public static string CurrentUser = "A5151013-DF9F-4234-B0BA-7556A035011D";
        public static string CurrentLanguage = "B388B67E-1BDA-4677-809D-FE28E37BF39A";
        public const string DateFormat = "dd/MM/yyyy";
        public const string TimeFormat = "HH:mm:ss";
        public const string TimeFormatMin = "HH:mm";
        public const string DateTimeFormat = "dd/MM/yyyy HH:mm:ss";
        public const string DateSeparator = "/";
        public const string ServerDateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        public const string DateLongFormat = "dd-MMM-yyyy";
        public const string DateTimeLongFormat = "dd-MMM-yyyy hh:mm";
        public const string DefaultLanguage = "french";
    }

    public static class DateTimeConverter
    {
        public static DateTime ToZoneTimeBD(this DateTime t)
        {
#if DEBUG
            string bnTimeZoneKey = "Bangladesh Standard Time";
#else
            string bnTimeZoneKey = "Central European Standard Time";
#endif
            TimeZoneInfo bdTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc((t), bdTimeZone);

        }
    }

    public enum UserType
    {
        SuperAdmin = 1,
        Admin = 2,
        User = 3
    }

    public enum NotificationType
    {
        Task = 1,
        Leave = 2
    }

    public class MenuModel
    {
        public string path { get; set; }
        public string title { get; set; }
        public string icon { get; set; }
        public string cssClass { get; set; }
        public bool isShowMenu { get; set; }
        public List<MenuItemModel> children { get; set; }
    }

    public class MenuItemModel
    {
        public string path { get; set; }
        public string title { get; set; }
        public string icon { get; set; }
        public string cssClass { get; set; }
    }

    public class MenuCollection
    {

        public static List<MenuModel> GetMenu(int userTypeId, string lang)
        {
            switch (userTypeId)
            {
                case (int)UserType.SuperAdmin:
                    return GetSuperAdminMenu(lang);
                case (int)UserType.Admin:
                    return GetAdminMenu(lang);
                default:
                    return new List<MenuModel>();
            }
        }

        private static List<MenuModel> GetSuperAdminMenu(string lang)
        {
            var list = new List<MenuModel>();
            if (lang == "en")
            {
                list.Add(new MenuModel { path = "/super-admin-dashboard", title = "Dashboard", icon = "developer_board", cssClass = "dashboard-menu-icon", isShowMenu = true, children = new List<MenuItemModel>() });
                list.Add(new MenuModel { path = "/settings/company", title = "My Clients", icon = "content_paste", cssClass = "dashboard-menu-icon", isShowMenu = true, children = new List<MenuItemModel>() });
                list.Add(new MenuModel { path = "/settings/employee", title = "Employee List", icon = "group", cssClass = "leave-menu-icon", isShowMenu = true, children = new List<MenuItemModel>() });
                list.Add(new MenuModel { path = "/login", title = "Logout", icon = "logout", cssClass = "logout-menu-icon", isShowMenu = true, children = new List<MenuItemModel>() });
            }
            else
            {
                list.Add(new MenuModel { path = "/super-admin-dashboard", title = "Tableau de bord", icon = "developer_board", cssClass = "dashboard-menu-icon", isShowMenu = true, children = new List<MenuItemModel>() });
                list.Add(new MenuModel { path = "/settings/company", title = "Mis clientes", icon = "content_paste", cssClass = "dashboard-menu-icon", isShowMenu = true, children = new List<MenuItemModel>() });
                list.Add(new MenuModel { path = "/settings/employee", title = "Liste des employés", icon = "contact_mail", cssClass = "leave-menu-icon", isShowMenu = true, children = new List<MenuItemModel>() });
                list.Add(new MenuModel { path = "/login", title = "Cerrar sesión", icon = "logout", cssClass = "logout-menu-icon", isShowMenu = true, children = new List<MenuItemModel>() });
            }

            return list;
        }

        private static List<MenuModel> GetAdminMenu(string lang)
        {
            var list = new List<MenuModel>();
            if (lang == "en")
            {
                list.Add(new MenuModel { path = "/dashboard", title = "Dashboard", icon = "developer_board", cssClass = "dashboard-menu-icon", isShowMenu = true, children = new List<MenuItemModel>() });
                //list.Add(new MenuModel { path = "/live-tracking", title = "Location On Map", icon = "edit_location", cssClass = "live-menu-icon", children = new List<MenuItemModel>() });
                list.Add(new MenuModel { path = "/tasks/relatedToMe", title = "Tasks", icon = "event_note", cssClass = "task-menu-icon", isShowMenu = false, children = new List<MenuItemModel>() });
                list.Add(new MenuModel { path = "/leaves/all-leaves", title = "Leaves", icon = "event_busy", cssClass = "leave-menu-icon", isShowMenu = false, children = new List<MenuItemModel>() });
                list.Add(new MenuModel { path = "/attendance/reports", title = "Reports", icon = "content_paste", cssClass = "attendance-menu-icon", isShowMenu = true, children = new List<MenuItemModel>() });
                list.Add(new MenuModel { path = "/employee-create", title = "Employee Create", icon = "content_paste", cssClass = "attendance-menu-icon", isShowMenu = true, children = new List<MenuItemModel>() });
                list.Add(new MenuModel { path = "/settings/employee", title = "Employee List", icon = "group", cssClass = "leave-menu-icon", isShowMenu = true, children = new List<MenuItemModel>() });
                list.Add(new MenuModel
                {
                    path = "#",
                    title = "Settings",
                    icon = "settings",
                    cssClass = "leave-menu-icon",
                    isShowMenu = true,
                    children = new List<MenuItemModel> {
                new MenuItemModel { path="/settings/company-agents",title="Company",icon="apartment",cssClass="leave-menu-icon" }
            }
                });


                //list.Add(new MenuModel
                //{
                //    path = "/settings",
                //    title = "Settings",
                //    icon = "settings",
                //    cssClass = "settings-menu-icon",
                //    children = new List<MenuItemModel>
                //    {
                //        new MenuItemModel { path = "/settings/employee", title = "Employee List", icon = "contact_mail", cssClass = "nav-item-sub" }
                //    }
                //});

                list.Add(new MenuModel { path = "/login", title = "Logout", icon = "logout", cssClass = "logout-menu-icon", isShowMenu = true, children = new List<MenuItemModel>() });

            }
            else
            {
                list.Add(new MenuModel { path = "/dashboard", title = "Tableau de bord", icon = "developer_board", cssClass = "dashboard-menu-icon", isShowMenu = true, children = new List<MenuItemModel>() });
                list.Add(new MenuModel { path = "/live-tracking", title = "Emplacement sur la carte", icon = "edit_location", cssClass = "live-menu-icon", children = new List<MenuItemModel>() });
                list.Add(new MenuModel { path = "/tasks/relatedToMe", title = "Liste de tâches", icon = "event_note", cssClass = "task-menu-icon", isShowMenu = false, children = new List<MenuItemModel>() });
                list.Add(new MenuModel { path = "/leaves/all-leaves", title = "Feuilles", icon = "event_busy", cssClass = "leave-menu-icon", isShowMenu = false, children = new List<MenuItemModel>() });
                list.Add(new MenuModel { path = "/attendance/reports", title = "Rapports", icon = "content_paste", cssClass = "attendance-menu-icon", isShowMenu = true, children = new List<MenuItemModel>() });
                list.Add(new MenuModel { path = "/employee-create", title = "Créer un employé", icon = "content_paste", cssClass = "attendance-menu-icon", isShowMenu = true, children = new List<MenuItemModel>() });
                list.Add(new MenuModel { path = "/settings/employee", title = "Liste des employés", icon = "contact_mail", cssClass = "leave-menu-icon", isShowMenu = true, children = new List<MenuItemModel>() });
                list.Add(new MenuModel
                {
                    path = "#",
                    title = "Paramètres",
                    icon = "settings",
                    cssClass = "leave-menu-icon",
                    isShowMenu = true,
                    children = new List<MenuItemModel> {
                new MenuItemModel { path="/settings/company-agents",title="Empresa",icon="apartment",cssClass="leave-menu-icon" }
            }
                });
                list.Add(new MenuModel { path = "/login", title = "Se déconnecter", icon = "logout", cssClass = "logout-menu-icon", isShowMenu = true, children = new List<MenuItemModel>() });
            }

            return list;
        }

    }

}
