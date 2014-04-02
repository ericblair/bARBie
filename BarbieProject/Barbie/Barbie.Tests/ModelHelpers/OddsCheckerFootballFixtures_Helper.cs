using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace Barbie.Tests.ModelHelpers
{
    public static class OddsCheckerFootballFixtures_Helper
    {
        public static OddsCheckerFootballFixtures CreateRecord(int id, DateTime matchDateTime)
        {
            var record = new OddsCheckerFootballFixtures
            {
                ID = id,
                CountryID = 1,
                CompetitionID = 1,
                MatchDateTime = matchDateTime,
                HomeTeam = "Man Utd",
                AwayTeam = "Man City",
                MatchWinnerOddsUrl = "http://test.com/test1"
            };

            return record;
        }
    }
}
