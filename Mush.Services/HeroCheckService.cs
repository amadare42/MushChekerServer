using Mush.Common.Services;
using Mush.Common.Services.Fetching;
using Mush.Common.Services.Filter;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mush.Services
{
    public class HeroCheckService : IHeroCheckService
    {
        private readonly IFetchingService fetchingService;
        private readonly IHeroFilterService heroFilterService;
        private readonly NotificationService notificationService;

        public HeroCheckService(IFetchingService fetchingService, IHeroFilterService heroFilterService,
            NotificationService notificationService)
        {
            this.fetchingService = fetchingService;
            this.heroFilterService = heroFilterService;
            this.notificationService = notificationService;
        }

        public async Task<IEnumerable<HeroFetchResult>> MakeCheckAsync()
        {
            var results = await fetchingService.FetchAsync();
            var filteredResults = heroFilterService.CheckResults(results).ToList();
            if (filteredResults.Count > 0)
            {
                notificationService.Notify(filteredResults);
            }
            return filteredResults;
        }
    }
}