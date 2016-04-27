using Mush.Common.HeroChecking;
using Mush.Common.NotificationTargets.Slack;
using Mush.Services;
using Mush.Services.HeroCheck;
using Mush.Web.Core;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Mush.Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ValueProviderFactories.Factories.Add(new JsonValueProviderFactory());
            RegisterServices();
        }

        private void RegisterServices()
        {
            //HeroCheckService
            var checkService = new HeroCheckService(new WebRequestHeroFetcher());
            ServiceLocator.Register(checkService);

            //SlackNotificationTarget
            var slackNotificationTarget = new SlackNotificationTarget();
            ServiceLocator.Register(slackNotificationTarget);

            //NotificationService
            var notificationService = new NotificationService();
            notificationService.Targets.Add(slackNotificationTarget);
            ServiceLocator.Register(notificationService);

            //AutoCheckService
            var autoCheckService = new AutoCheckService(checkService, notificationService);
            ServiceLocator.Register(autoCheckService);
        }
    }
}