using System.Collections.Generic;

namespace Mush.Common
{
    public class HeroCheckResult
    {
        public readonly string Name;
        public readonly string Id;
        public readonly IEnumerable<string> Characters;

        public HeroCheckResult(string name, string mush_sid, IEnumerable<string> characters)
        {
            this.Name = name;
            this.Id = mush_sid;
            this.Characters = characters;
        }
    }
}