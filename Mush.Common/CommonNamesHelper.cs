using Mush.Common.Services.Fetching;
using System.Collections.Generic;
using System.Linq;

namespace Mush.Common
{
    public class CommonNamesHelper
    {
        public static List<string> GetCommonCharacterNames(IEnumerable<HeroFetchResult> results)
        {
            var resultHeroesList = results.Select(r => r.Characters).ToList();
            if (resultHeroesList.Count <= 1)
                return new List<string>();
            var commonNames = resultHeroesList[0].ToList();
            for (var i = 1; i < resultHeroesList.Count; i++)
            {
                var heroes = resultHeroesList[i].ToList();
                for (var j = 0; j < commonNames.Count; j++)
                {
                    if (!heroes.Contains(commonNames[j]))
                    {
                        commonNames.RemoveAt(j);
                        j--;
                    }
                }

                if (commonNames.Count == 0)
                    break;
            }

            return commonNames;
        }
    }
}