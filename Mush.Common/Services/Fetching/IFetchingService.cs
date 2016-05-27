using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mush.Common.Services.Fetching
{
    public interface IFetchingService
    {
        Task<IEnumerable<HeroFetchResult>> FetchAsync();
    }
}