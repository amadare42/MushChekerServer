using Mush.Common;
using Mush.Common.HeroChecking;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Mush.Web.Services
{
    public static class SubscriptionResultExtenstions
    {
        public static string ToMessageString(this HeroCheckService.SubscriptionResult result)
        {
            switch (result)
            {
                case HeroCheckService.SubscriptionResult.Updated:
                    return "Subscribtion was succesfully updated!";

                case HeroCheckService.SubscriptionResult.Registered:
                    return "Subscribed succesfully!";

                case HeroCheckService.SubscriptionResult.NotChanged:
                    return "Already subscribed. Update not needed.";

                default:
                    return "Unknown subscribtion result.";
            }
        }
    }

    public class HeroCheckService
    {
        private readonly IHeroFetcher fetcher;

        public enum SubscriptionResult
        {
            Registered,
            NotChanged,
            Updated
        }

        private readonly List<SubscriberInfo> subscribers = new List<SubscriberInfo>();
        private readonly object lockRoot = new object();

        public HeroCheckService(IHeroFetcher fetcher)
        {
            this.fetcher = fetcher;
        }

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

        public Task<IEnumerable<HeroCheckResult>> MakeCheckAsync()
        {
            var usersCopy = new List<SubscriberInfo>(subscribers);
            return Task<IEnumerable<HeroCheckResult>>.Factory.StartNew(() =>
            {
                List<HeroCheckResult> infos = new List<HeroCheckResult>(usersCopy.Count);
                foreach (var user in usersCopy)
                {
                    try
                    {
                        var characters = fetcher.GetCharacters(user.mush_sid, user.saved_tid_sess);
                        infos.Add(new HeroCheckResult(user.name, user.mush_sid, characters));
                    }
                    catch (Exception ex)
                    {
                        infos.Add(new HeroCheckResult(user.name, user.mush_sid, new[] { ex.Message }));
                        Debug.WriteLine("Exception during getting characters: {0}", ex);
                    }
                }
                return infos;
            });
        }
    }
}