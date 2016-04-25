﻿using System.Collections.Generic;

namespace Mush.Common.HeroChecking
{
    public interface IHeroFetcher
    {
        IEnumerable<string> GetCharacters(string mush_sid, string saved_tid_sess);
    }
}