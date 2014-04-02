using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace Barbie.Tests.ModelHelpers
{
    public static class BetFairFootballOdds_Helper
    {
        public static BetFairFootballOdds CreateRecord(int id, int fixtureID)
        {
            var record = new BetFairFootballOdds
            {
                ID = id,
                FixtureID = fixtureID,
                CountryID = 1,
                CompetitionID = 1,
                Prediction = "Stenhousemuir",
                Updated = DateTime.Now
            };

            return record;
        }
    }
}
