using System.Collections.Generic;
using System.Threading.Tasks;
using Mush.Common.Services.Fetching;

namespace Mush.Common.Services
{
    public interface IHeroCheckService
    {
        Task<IEnumerable<HeroFetchResult>> MakeCheckAsync();
    }
}