using Mush.Common;
using Mush.Common.Services.Fetching;
using Mush.Common.Services.Filter;
using System.Collections.Generic;
using System.Linq;

namespace Mush.Services
{
    public class HeroFilterService : IHeroFilterService
    {
        private readonly Comparer comparer = new Comparer();

        private readonly object lockRoot = new object();
        private HeroFilterConfig config;
        private List<HeroFetchResult> previousResult;

        public HeroFilterService(HeroFilterConfig config)
        {
            this.config = config;
            previousResult = new List<HeroFetchResult>();
        }

        public HeroFilterService() : this(new HeroFilterConfig())
        {
        }

        public HeroFilterConfig Config
        {
            get { return config; }
            set
            {
                lock (lockRoot)
                {
                    config = value;
                }
            }
        }

        public IEnumerable<HeroFetchResult> CheckResults(IEnumerable<HeroFetchResult> results)
        {
            var res = InternalCheckResults(results);
            if (res.Count != 0)
                previousResult = res;
            return res;
        }

        private bool CheckRepeated(List<HeroFetchResult> resultsList)
        {
            return previousResult.SequenceEqual(resultsList, comparer);
        }

        private bool CheckDifferentCount(List<HeroFetchResult> resultsList)
        {
            var count = -1;
            foreach (var checkResult in resultsList)
            {
                if (count == -1)
                {
                    count = checkResult.Characters.ToList().Count;
                    continue;
                }

                if (count != checkResult.Characters.ToList().Count)
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckFullMatch(List<HeroFetchResult> resultsList)
        {
            var resultGauge = resultsList[0].Characters.ToList();

            for (var i = 1; i < resultsList.Count; i++)
            {
                var resultCharacters = resultsList[i].Characters;
                if (!resultGauge.SequenceEqual(resultCharacters))
                {
                    return false;
                }
            }

            return true;
        }

        private int GetMatchCount(List<HeroFetchResult> resultsList)
        {
            return CommonNamesHelper.GetCommonCharacterNames(resultsList).Count;
        }

        private List<HeroFetchResult> InternalCheckResults(IEnumerable<HeroFetchResult> results)
        {
            lock (lockRoot)
            {
                var resultsList = results.OrderBy(r => r.Id).ToList();

                if (resultsList.Count <= 1)
                    return resultsList;

                var repeated = config.SkipRepeated && CheckRepeated(resultsList);
                var differentCount = config.SkipDifferentCount && CheckDifferentCount(resultsList);

                if (repeated || differentCount)
                    return new List<HeroFetchResult>();

                var fullMatch = config.DontSkipOnFullMatch && CheckFullMatch(resultsList);

                if (fullMatch)
                    return resultsList;

                var enoughMatches = config.MinimumHeroMatchCount == 0 ||
                                    GetMatchCount(resultsList) >= config.MinimumHeroMatchCount;
                if (enoughMatches)
                    return resultsList;
            }

            return new List<HeroFetchResult>();
        }

        private class Comparer : IEqualityComparer<HeroFetchResult>
        {
            public bool Equals(HeroFetchResult x, HeroFetchResult y)
            {
                return x.Characters.SequenceEqual(y.Characters);
            }

            public int GetHashCode(HeroFetchResult obj)
            {
                return obj.Name.GetHashCode();
            }
        }
    }
}