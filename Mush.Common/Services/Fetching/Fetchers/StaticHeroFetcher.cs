using System.Collections.Generic;

namespace Mush.Common.Services.Fetching.Fetchers
{
    public class StaticHeroFetcher : IHeroFetcher
    {
        private readonly Dictionary<string, IEnumerable<string>> heroes;

        public StaticHeroFetcher(Dictionary<string, IEnumerable<string>> heroes)
        {
            this.heroes = heroes;
        }

        public IEnumerable<string> GetCharacters(string mush_sid, string saved_tid_sess)
        {
            IEnumerable<string> result;
            if (heroes.TryGetValue(mush_sid, out result))
            {
                return result;
            }
            return new List<string>();
        }
    }
}