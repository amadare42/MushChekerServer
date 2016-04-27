using Mush.Common;
using Mush.Services.HeroCheck;
using System;

namespace Mush.Services
{
    public class AutoCheckService
    {
        private readonly HeroCheckService heroCheckService;
        private readonly NotificationService notificationService;
        private readonly AutoRunner runner;

        public AutoCheckService(HeroCheckService heroCheckService, NotificationService notificationService)
        {
            this.heroCheckService = heroCheckService;
            this.notificationService = notificationService;
            runner = new AutoRunner(RunCheck);
        }

        public bool IsRunning => runner.IsRuning;

        public TimeSpan Interval => runner.Interval;

        public void Stop()
        {
            runner.Stop();
        }

        public void Start(TimeSpan interval)
        {
            runner.Start(interval);
        }

        private async void RunCheck()
        {
            var characterInfos = await heroCheckService.MakeCheckAsync();
            notificationService.SendUserInfos(characterInfos);
        }
    }
}