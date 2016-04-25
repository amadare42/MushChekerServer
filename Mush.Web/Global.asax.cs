using Mush.Common.HeroChecking;
using Mush.Common.Intergrations.Slack;
using Mush.Web.Models;
using Mush.Web.Services;
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
            slackNotificationTarget.AddUrl("https://hooks.slack.com/services/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");
            ServiceLocator.Register(slackNotificationTarget);

            //NotificationService
            var notificationService = new NotificationService();
            notificationService.Targets.Add(slackNotificationTarget);
            ServiceLocator.Register(notificationService);
        }
    }
}