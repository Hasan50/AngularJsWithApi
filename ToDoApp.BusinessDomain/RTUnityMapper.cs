using Microsoft.Practices.Unity;
using ToDoApp.BusinessDomain.DataAccess;
using ToDoApp.BusinessDomain.Interfaces;

namespace ToDoApp.BusinessDomain
{
    public class RTUnityMapper
    {
        private static IUnityContainer _container;

        public static void RegisterComponents(IUnityContainer container)
        {
            _container = container;
            
            container.RegisterType<IDepartment, DepartmentDataAccess>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICompany, CompanyDataAccess>(new ContainerControlledLifetimeManager());
            container.RegisterType<IEmployee, EmployeeDataAccess>(new ContainerControlledLifetimeManager());
            container.RegisterType<INoticeBoard, NoticeBoardDataAccess>(new ContainerControlledLifetimeManager());
            container.RegisterType<IEmployeeLeave, EmployeeLeaveDataAccess>(new ContainerControlledLifetimeManager());
            container.RegisterType<IAttendance, AttendanceDataAccess>(new ContainerControlledLifetimeManager());
            container.RegisterType<IUserCredential, UserCredentialDataAccess>(new ContainerControlledLifetimeManager());
            container.RegisterType<IEmployeeTask, EmployeeTaskDataAccess>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICompanyAgent, CompanyAgentDataAccess>(new ContainerControlledLifetimeManager());
            container.RegisterType<ISickType, SickTypeDataAccess>(new ContainerControlledLifetimeManager());
            container.RegisterType<INotificationLog, NotificationLogDataAccess>(new ContainerControlledLifetimeManager());

        }

        public static T GetInstance<T>()
        {
            try
            {
                return _container.Resolve<T>();
            }
            catch (ResolutionFailedException exception)
            {
            }
            return default(T);
        }
    }
}
