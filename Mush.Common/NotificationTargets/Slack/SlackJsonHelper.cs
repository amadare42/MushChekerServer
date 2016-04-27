using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mush.Common.NotificationTargets.Slack
{
    public static class SlackJsonHelper
    {
        public static string GenerateMessageJson(IEnumerable<HeroCheckResult> userInfos)
        {
            return JsonConvert.SerializeObject(new
            {
                username = "Mush checker",
                text = "<!here|here> Available heroes:",
                icon_emoji = ":mushroom:",
                attachments = userInfos.Select(GetAttachment)
            });
        }

        private static readonly string[] Markdown = { "text", "pretext" };

        private static object GetAttachment(HeroCheckResult info)
        {
            StringBuilder sb = new StringBuilder($"*{info.Name}*:");
            foreach (var character in info.Characters)
            {
                sb.Append($"\n\t{character}");
            }

            return new
            {
                mrkdwn_in = Markdown,
                text = sb.ToString()
            };
        }
    }
}