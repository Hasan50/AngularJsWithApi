using PeopleTrackingApi.BusinessDomain;
using PeopleTrackingApi.BusinessDomain.Interfaces;
using PeopleTrackingApi.Web.Filters;
using PeopleTrackingApi.Web.Helpers;
using System.Web.Mvc;

namespace PeopleTrackingApi.Web.Controllers
{
    [ValidateUser]
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

        public ActionResult DisplayMapLocation()
        {
            return PartialView();
        }
        public ActionResult DetailsBarMapLocation()
        {
            return PartialView();
        }

        [HttpGet]
        public JsonResult GetTopNotifications()
        {
            var query = _notificationLog.GetTop10(_userInfo.CompanyId);
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllNotification()
        {
            var query = _notificationLog.GetAll(_userInfo.CompanyId);
            return Json(query, JsonRequestBehavior.AllowGet);
        }
    }
}
