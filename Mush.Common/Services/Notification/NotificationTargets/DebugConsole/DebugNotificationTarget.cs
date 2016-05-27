using Mush.Common.Services.Fetching;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Mush.Common.Services.Notification.NotificationTargets.DebugConsole
{
    public class DebugNotificationTarget : INotificationTarget
    {
        private static readonly string nl = Environment.NewLine;

        public void Notify(IEnumerable<HeroFetchResult> infos)
        {
            var infosList = infos.ToList();

            var checkInfos = infosList.Select(BuildInfoString).ToList();
            var summaryInfo = checkInfos.Count == 0
                ? "No subscribers"
                : checkInfos.Aggregate((current, next) => current + nl + nl + next);

            Debug.WriteLine(
                $"========================{nl}" +
                $"Notification message:{nl}" +
                $"{summaryInfo}{nl}");

            var commonNames = CommonNamesHelper.GetCommonCharacterNames(infosList);
            if (commonNames.Count > 0)
            {
                var commonNamesStringBuilder = new StringBuilder("Common: ");
                foreach (var name in commonNames)
                {
                    commonNamesStringBuilder
                        .Append(name)
                        .Append("; ");
                }
                Debug.WriteLine($"{commonNamesStringBuilder}{nl}");
            }
        }

        private string BuildInfoString(HeroFetchResult result)
        {
            return $"{result.Name} ({result.Id}):{nl}\t" +
                   result.Characters.Aggregate((current, next) => $"{current}{nl}\t" + next);
        }
    }
}