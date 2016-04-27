using Mush.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Mush.Common.NotificationTargets.Slack;
using Mush.Services;
using Mush.Services.HeroCheck;
using Mush.Web.Core;

namespace Mush.Web.Controllers
{
    public class MushApiController : ApiController
    {
        [Route("RegisterSubscriber")]
        [HttpPost]
        public string RegisterSubscriber([FromBody]SubscriberInfo subscriberInfo)
        {
            try
            {
                var subscribtionResult = ServiceLocator.Get<HeroCheckService>()
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
            return ServiceLocator.Get<HeroCheckService>()
                .Subscribers.Select(sub => sub.name);
        }

        [Route("RegisterSlackUrl")]
        [HttpPost]
        public IEnumerable<string> RegisterSlackUrl([FromBody]string url)
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
        public IEnumerable<string> UnregisterSlackUrl([FromBody]string url)
        {
            var slackIntegration = ServiceLocator.Get<SlackNotificationTarget>();
            slackIntegration.RemoveUrl(url);
            return slackIntegration.GetUrls();
        }

        [Route("RegisterAndCheck")]
        [HttpPost]
        public async Task<IHttpActionResult> RegisterAndCheck([FromBody]SubscriberInfo subscriberInfo)
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
            var heroCheckService = ServiceLocator.Get<HeroCheckService>();
            var notificationService = ServiceLocator.Get<NotificationService>();

            var characterInfos = (await heroCheckService.MakeCheckAsync()).ToList();
            notificationService.SendUserInfos(characterInfos);
            return Ok(characterInfos);
        }

        [Route("AutoCheckDelay")]
        [HttpPost]
        public IHttpActionResult ConfigureController([FromBody]int delay)
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

        [Route("ServerStatus")]
        [HttpGet]
        public IHttpActionResult GetServerStatus()
        {
            var autocheckService = ServiceLocator.Get<AutoCheckService>();
            return Ok(
            new
            {
                Subscribers = GetSubscribers(),
                SlackUrls = GetSlackUrls(),
                AutocheckInterval = autocheckService.IsRunning
                    ? autocheckService.Interval.Seconds
                    : -1
            });
        }

        [Route("ApiVersion")]
        [HttpGet]
        public IEnumerable<int> GetApiVersion()
        {
            return new[] { 0, 1 };
        }
    }
}