using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IRepository
    {
        // Arbs_Football_MatchWinner
        bool IsArbMarkedAsExpired(int arbID);

        // FootballFixturesMap
        int GetBetfairFixtureIdForArb(int arbFixtureMapID);
        int GetOddscheckerFixtureIdForArb(int arbFixtureMapID);

        // BetFairFootballOdds
        DateTime GetDateTimeBetfairFootballOddsLastUpdated(int fixtureID);

        // OddsCheckerFootballOdds
        DateTime GetDateTimeOddscheckerFootballOddsLastUpdated(int fixtureID);
    }
}
