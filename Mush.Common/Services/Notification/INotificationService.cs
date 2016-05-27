using System.Collections.Generic;
using Mush.Common.Services.Fetching;

namespace Mush.Common.Services.Notification
{
    public interface INotificationService
    {
        void Notify(IEnumerable<HeroFetchResult> results);
    }
}