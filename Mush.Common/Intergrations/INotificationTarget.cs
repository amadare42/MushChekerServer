using System.Collections.Generic;

namespace Mush.Common.Intergrations
{
    public interface INotificationTarget
    {
        void SendUserInfos(IEnumerable<HeroCheckResult> infos);
    }
}