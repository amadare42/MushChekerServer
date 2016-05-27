using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Mush.Common.Services;
using Mush.Common.Services.Fetching.Fetchers;
using Mush.Common.Services.Filter;
using Mush.Common.Services.Notification;
using Mush.Common.Services.Notification.NotificationTargets.DebugConsole;
using Mush.Common.Services.Notification.NotificationTargets.Slack;
using Mush.Common.Services.Subscribtions;
using Mush.Services;
using Mush.Web.Core;

namespace Mush.Web
{
    public class WebApiApplication : HttpApplication
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
            //SubscribtionManagerService
            var subscribtionManager = new SubscribtionManagerService();
            ServiceLocator.Register<ISubscribtionManager>(subscribtionManager);

            //SlackNotificationTarget
            var slackNotificationTarget = new SlackNotificationTarget();
            ServiceLocator.Register(slackNotificationTarget);

            //HeroFilterService
            var heroFilterService = new HeroFilterService();
            ServiceLocator.Register<IHeroFilterService>(heroFilterService);

            //NotificationService
            var notificationService = new NotificationService();
            ServiceLocator.Register<INotificationService>(notificationService);

            //HeroCheckService
            var fetcher = new WebRequestHeroFetcher();
            var fetchingService = new FetchingService(fetcher, subscribtionManager);
            var heroCheckService = new HeroCheckService(fetchingService, heroFilterService, notificationService);
            ServiceLocator.Register<IHeroCheckService>(heroCheckService);

            //NotificationService Targets
            notificationService.Targets.Add(slackNotificationTarget);
#if DEBUG
            notificationService.Targets.Add(new DebugNotificationTarget());
#endif

            //AutoCheckService
            var autoCheckService = new AutoCheckService(heroCheckService);
            ServiceLocator.Register(autoCheckService);
        }
    }
}