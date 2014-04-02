using System;

namespace Barbie
{
    public interface IConfigHelper
    {
        int DataExpirationLimitHours();
        int StringMatchingMaxLevenshteinValue();
        int BetFairCommisionPercentage();
        int MaxTotalMatchTimeMins();
    }
}
