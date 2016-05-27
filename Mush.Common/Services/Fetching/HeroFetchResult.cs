using System.Collections.Generic;

namespace Mush.Common.Services.Fetching
{
    public class HeroFetchResult
    {
        public readonly IEnumerable<string> Characters;
        public readonly string Id;
        public readonly string Name;

        public HeroFetchResult(string name, string mush_sid, IEnumerable<string> characters)
        {
            Name = name;
            Id = mush_sid;
            Characters = characters;
        }
    }
}