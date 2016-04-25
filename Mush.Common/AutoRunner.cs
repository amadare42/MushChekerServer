using System;
using System.Reactive.Linq;
using System.Threading;

namespace Mush.Common
{
    public class AutoRunner
    {
        public Action Action { get; private set; }
        public TimeSpan Interval { get; private set; }
        private CancellationTokenSource cancellationTokenSource;

        public bool IsRuning
        {
            get
            {
                return cancellationTokenSource != null
                  && !cancellationTokenSource.IsCancellationRequested;
            }
        }

        public AutoRunner()
        {
            //empty
        }

        public AutoRunner(Action action)
        {
            Action = action;
        }

        public void Start(TimeSpan interval)
        {
            StartNew(Action, interval);
        }

        public void StartNew(Action action, TimeSpan interval)
        {
            if (IsRuning)
                this.Stop();
            Action = action;
            Interval = interval;
            IObservable<long> observable = Observable.Interval(interval);
            cancellationTokenSource = new CancellationTokenSource();
            observable.Subscribe(x => action(), cancellationTokenSource.Token);
        }

        public void Stop()
        {
            cancellationTokenSource?.Cancel();
        }
    }
}