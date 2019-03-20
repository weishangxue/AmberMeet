using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AmberMeet.AppService.Base;

namespace AmberMeet
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            DependencyConfig.Configure();
            ObjectMapConfig.Configure();

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}