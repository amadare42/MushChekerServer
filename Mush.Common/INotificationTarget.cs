using System.Collections.Generic;

namespace Mush.Common
{
    public interface INotificationTarget
    {
        void SendUserInfos(IEnumerable<HeroCheckResult> infos);
    }
}