using System;
using System.Reactive.Linq;
using System.Threading;

namespace Mush.Common
{
    public class AutoRunner
    {
        private CancellationTokenSource cancellationTokenSource;

        public AutoRunner()
        {
            //empty
        }

        public AutoRunner(Action action)
        {
            Action = action;
        }

        public Action Action { get; private set; }
        public TimeSpan Interval { get; private set; }

        public bool IsRuning
        {
            get
            {
                return cancellationTokenSource != null
                       && !cancellationTokenSource.IsCancellationRequested;
            }
        }

        public void Start(TimeSpan interval)
        {
            StartNew(Action, interval);
        }

        public void StartNew(Action action, TimeSpan interval)
        {
            if (IsRuning)
                Stop();
            Action = action;
            Interval = interval;
            var observable = Observable.Interval(interval);
            cancellationTokenSource = new CancellationTokenSource();
            observable.Subscribe(x => action(), cancellationTokenSource.Token);
        }

        public void Stop()
        {
            cancellationTokenSource?.Cancel();
        }
    }
}