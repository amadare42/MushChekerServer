using System.Collections.Generic;

namespace Mush.Common.Services.Subscribtions
{
    public interface ISubscribtionManager
    {
        IEnumerable<SubscriberInfo> Subscribers { get; }

        SubscriptionResult Subscribe(SubscriberInfo subscriber);

        void Unsubscribe(SubscriberInfo subscriberInfo);
    }
}