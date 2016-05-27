using Mush.Common.Services.Fetching;
using Mush.Common.Services.Notification;
using System.Collections.Generic;
using System.Linq;

namespace Mush.Services
{
    public class NotificationService : INotificationService
    {
        public readonly List<INotificationTarget> Targets = new List<INotificationTarget>();

        public void Notify(IEnumerable<HeroFetchResult> results)
        {
            var infosList = results.ToList();
            foreach (var target in Targets)
            {
                target.Notify(infosList);
            }
        }
    }
}