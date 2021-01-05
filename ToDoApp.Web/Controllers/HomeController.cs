using ToDoApp.BusinessDomain;
using ToDoApp.BusinessDomain.Interfaces;
using ToDoApp.Web.Filters;
using ToDoApp.Web.Helpers;
using System.Web.Mvc;

namespace ToDoApp.Web.Controllers
{
    public class HomeController : BaseReportController
    {
        private readonly INotificationLog _notificationLog;
        public HomeController()
        {
            _notificationLog = RTUnityMapper.GetInstance<INotificationLog>();
        }
        public ActionResult Index()
        {
            //if(CommonUtility.IsSuperAdmin)
            //    return RedirectToAction("_SuperAdminDashboard");
            //return RedirectToAction("_AdminDashboard");
            return View();
        }

        public ActionResult Dashboard()
        {
            return PartialView();
        }

        public ActionResult _SuperAdminDashboard()
        {
            return PartialView();
        }
        public ActionResult _AdminDashboard()
        {
            return PartialView();
        }
       
    }
}
