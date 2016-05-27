using System.Collections.Generic;
using System.Linq;
using Mush.Common.Services.Subscribtions;

namespace Mush.Services
{
    public class SubscribtionManagerService : ISubscribtionManager
    {
        private readonly object lockRoot = new object();
        private readonly List<SubscriberInfo> subscribers = new List<SubscriberInfo>();

        public IEnumerable<SubscriberInfo> Subscribers
        {
            get
            {
                lock (lockRoot)
                {
                    return subscribers;
                }
            }
        }

        public SubscriptionResult Subscribe(SubscriberInfo subscriber)
        {
            var result = SubscriptionResult.Registered;

            lock (lockRoot)
            {
                var existedSubscriber = subscribers.FirstOrDefault(u => u.mush_sid == subscriber.mush_sid);
                if (existedSubscriber != null)
                {
                    subscribers.Remove(existedSubscriber);
                    result = existedSubscriber.saved_tid_sess == subscriber.saved_tid_sess
                        ? SubscriptionResult.NotChanged
                        : SubscriptionResult.Updated;
                }

                subscribers.Add(subscriber);
            }

            return result;
        }

        public void Unsubscribe(SubscriberInfo subscriberInfo)
        {
            lock (lockRoot)
            {
                subscribers.Remove(subscriberInfo);
            }
        }
    }
}