using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace Barbie.Tests.ModelHelpers
{
    public static class OddsCheckerFootballOdds_Helper
    {
        public static OddsCheckerFootballOdds CreateRecord(int id, int fixtureID)
        {
            var record = new OddsCheckerFootballOdds
            {
                ID = id,
                FixtureID = fixtureID,
                CountryID = 1,
                CompetitionID = 1,
                Prediction = "Falkirk",
                Updated = DateTime.Now
            };

            return record;
        }
    }
}
