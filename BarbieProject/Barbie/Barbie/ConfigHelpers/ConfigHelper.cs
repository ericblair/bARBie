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
    }
}
