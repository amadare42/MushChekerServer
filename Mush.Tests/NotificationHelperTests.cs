using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mush.Common;
using Mush.Common.Services.Fetching;

namespace Mush.Tests
{
    [TestClass]
    public class NotificationHelperTests
    {
        [TestMethod]
        public void OneMatch()
        {
            var result = CommonNamesHelper.GetCommonCharacterNames(new[]
            {
                new HeroFetchResult("", "", new[] {"1", "2", "3", "4"}),
                new HeroFetchResult("", "", new[] {"10", "2", "30", "40"})
            });

            CollectionAssert.AreEquivalent(new[] {"2"}, result.ToArray());
        }

        [TestMethod]
        public void NoMatch()
        {
            var result = CommonNamesHelper.GetCommonCharacterNames(new[]
            {
                new HeroFetchResult("", "", new[] {"1", "23", "3", "4"}),
                new HeroFetchResult("", "", new[] {"10", "2", "30", "40"})
            });

            CollectionAssert.AreEquivalent(new string[0], result.ToArray());
        }

        [TestMethod]
        public void OneResult()
        {
            var result = CommonNamesHelper.GetCommonCharacterNames(new[]
            {
                new HeroFetchResult("", "", new[] {"1", "23", "3", "4"})
            });

            CollectionAssert.AreEquivalent(new string[0], result.ToArray());
        }

        [TestMethod]
        public void ManyResultsAndMatches()
        {
            var result = CommonNamesHelper.GetCommonCharacterNames(new[]
            {
                new HeroFetchResult("", "", new[] {"8", "23", "3", "5"}),
                new HeroFetchResult("", "", new[] {"1", "3", "a", "23"}),
                new HeroFetchResult("", "", new[] {"3", "0", "23", "9"}),
                new HeroFetchResult("", "", new[] {"b", "23", "3", "4"}),
                new HeroFetchResult("", "", new[] {"7", "23", "4", "3"})
            });

            CollectionAssert.AreEquivalent(new[] {"3", "23"}, result.ToArray());
        }
    }
}