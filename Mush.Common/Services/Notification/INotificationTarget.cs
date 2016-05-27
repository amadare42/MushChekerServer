using Mush.Common.Services.Fetching;
using System.Collections.Generic;

namespace Mush.Common.Services.Notification
{
    public interface INotificationTarget
    {
        void Notify(IEnumerable<HeroFetchResult> infos);
    }
}