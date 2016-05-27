using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Mush.Common;
using Mush.Common.Services.Fetching;
using Mush.Common.Services.Subscribtions;

namespace Mush.Services
{
    public class FetchingService : IFetchingService
    {
        private readonly IHeroFetcher fetcher;
        private readonly ISubscribtionManager subscribtionManager;

        public FetchingService(IHeroFetcher fetcher, ISubscribtionManager subscribtionManager)
        {
            this.fetcher = fetcher;
            this.subscribtionManager = subscribtionManager;
        }

        public Task<IEnumerable<HeroFetchResult>> FetchAsync()
        {
            var subscribers = subscribtionManager.Subscribers;
            var usersCopy = new List<SubscriberInfo>(subscribers);
            return Task<IEnumerable<HeroFetchResult>>.Factory.StartNew(() =>
            {
                var infos = new List<HeroFetchResult>(usersCopy.Count);
                foreach (var user in usersCopy)
                {
                    try
                    {
                        var characters = fetcher.GetCharacters(user.mush_sid, user.saved_tid_sess);
                        infos.Add(new HeroFetchResult(user.name, user.mush_sid, characters));
                    }
                    catch (Exception ex)
                    {
                        infos.Add(new HeroFetchResult(user.name, user.mush_sid, new[] {ex.Message}));
                        Debug.WriteLine("Exception during getting characters: {0}", ex);
                    }
                }
                return infos;
            });
        }
    }
}