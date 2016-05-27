using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mush.Common.Services.Fetching;
using Newtonsoft.Json;

namespace Mush.Common.Services.Notification.NotificationTargets.Slack
{
    public static class SlackJsonHelper
    {
        private static readonly string[] Markdown = {"text", "pretext"};

        public static string GenerateMessageJson(IEnumerable<HeroFetchResult> userInfos, List<string> commonNames)
        {
            return JsonConvert.SerializeObject(new
            {
                username = "Mush checker",
                text = "<!here|here> Available heroes:",
                icon_emoji = ":mushroom:",
                attachments = userInfos.Select(info => GetAttachment(info, commonNames))
            });
        }

        private static object GetAttachment(HeroFetchResult info, List<string> commonNames)
        {
            var sb = new StringBuilder($"*{info.Name}*:");
            foreach (var character in info.Characters)
            {
                var isCommonCharacter = commonNames.Contains(character);
                var characterString = isCommonCharacter
                    ? $"\n\t*{character}*"
                    : $"\n\t{character}";

                sb.Append(characterString);
            }

            return new
            {
                mrkdwn_in = Markdown,
                text = sb.ToString()
            };
        }
    }
}