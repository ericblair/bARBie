using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace Barbie.Tests.ModelHelpers
{
    public static class FootballFixturesMap_Helper
    {
        public static FootballFixturesMap CreateRecord(int id, int betFairFixtureID, int oddsCheckerFixtureID)
        {
            var record = new FootballFixturesMap
            {
                ID = id,
                BetFairFixtureID = betFairFixtureID,
                OddsCheckerFixtureID = oddsCheckerFixtureID
            };

            return record;
        }
    }
}
