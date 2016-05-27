using System;
using Mush.Common;
using Mush.Common.Services;

namespace Mush.Services
{
    public class AutoCheckService
    {
        private readonly IHeroCheckService heroCheckService;
        private readonly AutoRunner runner;

        public AutoCheckService(IHeroCheckService heroCheckService)
        {
            this.heroCheckService = heroCheckService;
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
            await heroCheckService.MakeCheckAsync();
        }
    }
}