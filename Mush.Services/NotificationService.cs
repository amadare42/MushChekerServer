using System.Collections.Generic;
using System.Linq;
using Mush.Common;

namespace Mush.Services
{
    public class NotificationService
    {
        public readonly List<INotificationTarget> Targets = new List<INotificationTarget>();

        public void SendUserInfos(IEnumerable<HeroCheckResult> infos)
        {
            var infosList = infos.ToList();
            foreach (var target in Targets)
            {
                target.SendUserInfos(infosList);
            }
        }
    }
}