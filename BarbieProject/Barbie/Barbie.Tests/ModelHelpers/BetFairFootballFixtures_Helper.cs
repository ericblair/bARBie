using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace Barbie.Tests.ModelHelpers
{
    public static class BetFairFootballFixtures_Helper
    {
        public static BetFairFootballFixtures CreateRecord(int id, DateTime matchDateTime)
        {
            var record = new BetFairFootballFixtures
            {
                ID = id,
                CompetitionID = 1,
                MatchDateTime = matchDateTime,
                HomeTeam = "Rangers",
                AwayTeam = "Celtic",
                MatchWinnerOddsUrl = "http://test.com/test1"
            };

            return record;
        }

        public static BetFairFootballFixtures CreateRecord(int id, int competitionID, DateTime matchDateTime, string homeTeam, string awayTeam)
        {
            var record = new BetFairFootballFixtures
            {
                ID = id,
                CompetitionID = competitionID,
                MatchDateTime = matchDateTime,
                HomeTeam = homeTeam,
                AwayTeam =awayTeam,
                MatchWinnerOddsUrl = "http://test.com/test1"
            };

            return record;
        }
    }
}
