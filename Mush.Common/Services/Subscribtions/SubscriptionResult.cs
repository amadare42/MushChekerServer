namespace Mush.Common.Services.Subscribtions
{
    public enum SubscriptionResult
    {
        Registered,
        NotChanged,
        Updated
    }

    public static class SubscriptionResultExtenstions
    {
        public static string ToMessageString(this SubscriptionResult result)
        {
            switch (result)
            {
                case SubscriptionResult.Updated:
                    return "Subscribtion was succesfully updated!";

                case SubscriptionResult.Registered:
                    return "Subscribed succesfully!";

                case SubscriptionResult.NotChanged:
                    return "Already subscribed. Update not needed.";

                default:
                    return "Unknown subscribtion result.";
            }
        }
    }
}