using System.Web.Mvc;
using System.Web.Routing;

namespace Mush.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Default", "{controller}/{action}/{id}", new {action = "Index", id = UrlParameter.Optional}
                );

            routes.MapRoute("Console", "{controller}/{action}", new {controller = "Console", action = "Index"}
                );
        }
    }
}