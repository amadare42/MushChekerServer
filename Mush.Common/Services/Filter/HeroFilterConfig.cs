namespace Mush.Common.Services.Filter
{
    public class HeroFilterConfig
    {
        public bool SkipRepeated { get; set; } = true;
        public bool SkipDifferentCount { get; set; } = true;

        public int MinimumHeroMatchCount { get; set; } = 1;
        public bool DontSkipOnFullMatch { get; set; } = true;
    }
}