using Mush.Common.Services;
using Mush.Common.Services.Filter;
using Mush.Common.Services.Notification.NotificationTargets.Slack;
using Mush.Common.Services.Subscribtions;
using Mush.Services;
using Mush.Web.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Mush.Web.Controllers
{
    public class MushApiController : ApiController
    {
        [Route("RegisterSubscriber")]
        [HttpPost]
        public string RegisterSubscriber([FromBody] SubscriberInfo subscriberInfo)
        {
            try
            {
                var subscribtionResult = ServiceLocator.Get<ISubscribtionManager>()
                    .Subscribe(subscriberInfo);
                return subscribtionResult.ToMessageString();
            }
            catch (Exception ex)
            {
                return $"Error happened during registration: {ex.Message}";
            }
        }

        [Route("GetSubscribers")]
        [HttpGet]
        public IEnumerable<string> GetSubscribers()
        {
            return ServiceLocator.Get<ISubscribtionManager>()
                .Subscribers.Select(sub => sub.name);
        }

        [Route("RegisterSlackUrl")]
        [HttpPost]
        public IEnumerable<string> RegisterSlackUrl([FromBody] string url)
        {
            var slackIntegration = ServiceLocator.Get<SlackNotificationTarget>();
            slackIntegration.AddUrl(url);
            return slackIntegration.GetUrls();
        }

        [Route("GetSlackUrls")]
        [HttpPost]
        public IEnumerable<string> GetSlackUrls()
        {
            return ServiceLocator.Get<SlackNotificationTarget>().GetUrls();
        }

        [Route("UnregisterSlackUrl")]
        [HttpPost]
        public IEnumerable<string> UnregisterSlackUrl([FromBody] string url)
        {
            var slackIntegration = ServiceLocator.Get<SlackNotificationTarget>();
            slackIntegration.RemoveUrl(url);
            return slackIntegration.GetUrls();
        }

        [Route("RegisterAndCheck")]
        [HttpPost]
        public async Task<IHttpActionResult> RegisterAndCheck([FromBody] SubscriberInfo subscriberInfo)
        {
            var regStatus = RegisterSubscriber(subscriberInfo);

            return Ok(new
            {
                registrationStatus = regStatus,
                checkResult = await MakeCheck()
            });
        }

        [Route("MakeCheck")]
        [HttpPost]
        public async Task<IHttpActionResult> MakeCheck()
        {
            var heroCheckService = ServiceLocator.Get<IHeroCheckService>();
            var characterInfos = await heroCheckService.MakeCheckAsync();

            return Ok(characterInfos);
        }

        [Route("AutoCheckDelay")]
        [HttpPost]
        public IHttpActionResult ConfigureAutoCheck([FromBody] int delay)
        {
            var autoCheckService = ServiceLocator.Get<AutoCheckService>();
            if (delay <= 0)
            {
                if (!autoCheckService.IsRunning)
                {
                    return Ok("Autoucheck is disabled.");
                }
                autoCheckService.Stop();
                return Ok("Autoucheck was disabled.");
            }
            autoCheckService.Start(TimeSpan.FromSeconds(delay));
            return Ok($"Autoucheck was enabled with delay {delay} sec.");
        }

        [Route("FilterConfig")]
        [HttpPost]
        public IHttpActionResult SetFilterConfig([FromBody] HeroFilterConfig config)
        {
            var filterService = ServiceLocator.Get<IHeroFilterService>();
            filterService.Config = config;
            return Ok(filterService.Config);
        }

        [Route("FilterConfig")]
        [HttpGet]
        public IHttpActionResult GetFilterConfig([FromBody] HeroFilterConfig config)
        {
            var filterService = ServiceLocator.Get<IHeroFilterService>();
            return Ok(filterService.Config);
        }

        [Route("ServerStatus")]
        [HttpGet]
        public IHttpActionResult GetServerStatus()
        {
            var autocheckService = ServiceLocator.Get<AutoCheckService>();
            var filterService = ServiceLocator.Get<IHeroFilterService>();
            return Ok(
                new
                {
                    Subscribers = GetSubscribers(),
                    SlackUrls = GetSlackUrls(),
                    AutocheckInterval = autocheckService.IsRunning
                        ? autocheckService.Interval.Seconds
                        : -1,
                    FilterConfig = filterService.Config
                });
        }

        [Route("ApiVersion")]
        [HttpGet]
        public IEnumerable<int> GetApiVersion()
        {
            return new[] { 0, 1, 1 };
        }
    }
}