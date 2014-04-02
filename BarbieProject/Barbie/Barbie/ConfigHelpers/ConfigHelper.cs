using System;
using System.Configuration;

namespace Barbie
{
    public class ConfigHelper : IConfigHelper
    {
        public int DataExpirationLimitHours()
        {
            int expiryLimitHours;

            if (Int32.TryParse(ConfigurationManager.AppSettings["DataExpirationLimitHours"], out expiryLimitHours))
            {
                // TODO: log error 
                throw new ConfigurationErrorsException();
            }

            return expiryLimitHours;
        }

        public int StringMatchingMaxLevenshteinValue()
        {
            int maxValue;

            if (Int32.TryParse(ConfigurationManager.AppSettings["StringMatchingMaxLevenshteinValue"], out maxValue))
            {
                // TODO: log error 
                throw new ConfigurationErrorsException();
            }

            return maxValue;
        }

        public int BetFairCommisionPercentage()
        {
            int commision;

            if (Int32.TryParse(ConfigurationManager.AppSettings["BetFairCommisionPercentage"], out commision))
            {
                // TODO: log error 
                throw new ConfigurationErrorsException();
            }

            return commision;
        }

        public int MaxTotalMatchTimeMins()
        {
            int maxMatchTime;

            if (Int32.TryParse(ConfigurationManager.AppSettings["MaxTotalMatchTimeMins"], out maxMatchTime))
            {
                // TODO: log error 
                throw new ConfigurationErrorsException();
            }

            return maxMatchTime;
        }
    }
}
