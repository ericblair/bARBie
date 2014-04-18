using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace Barbie.Tests.ModelHelpers
{
    public static class Arbs_Football_MatchWinner_Helper
    {
        public static Arbs_Football_MatchWinner CreateRecord(int id, bool expired = false)
        {
            var record = new Arbs_Football_MatchWinner
            {
                ID = id,
                FixtureMapID = 1,
                MatchDateTime = DateTime.Now,
                HomeTeam = "Arsenal",
                AwayTeam = "Chelsea",
                Bookie = "Bet365",
                BookieOdds = 5,
                BetFairLayLevel = "LOW",
                BetFairOdds = 4,
                BetFairCash = 1000,
                Predication = "Arsenal",
                BetFairUpdated = DateTime.Now,
                OddsCheckerUpdated = DateTime.Now,
                Expired = expired,
                Created = DateTime.Now,
                Updated = null,
                ParentID = null
            };

            return record;
        }

        public static Arbs_Football_MatchWinner CreateRecord(int id = 1, bool? expired = false, DateTime? matchDateTime = null, string homeTeam = "Man City", 
                                                            string awayTeam = "Everton", string bookie = "Ladbrokes", decimal bookieOdds = 5, 
                                                            string betFairLevel = "LOW", decimal betFairOdds = 4, decimal betFairCash = 1000, 
                                                            string prediction = "Everton", DateTime? betFairUpdated = null, DateTime? oddsCheckerUpdated = null, 
                                                            DateTime? created = null, DateTime? updated = null, int parentID = 1)
        {
            if (matchDateTime == null)
                matchDateTime = DateTime.Now;
            if (betFairUpdated == null)
                betFairUpdated = DateTime.Now;
            if (oddsCheckerUpdated == null)
                oddsCheckerUpdated = DateTime.Now;
            if (created == null)
                created = DateTime.Now;

            var record = new Arbs_Football_MatchWinner
            {
                ID = id,
                FixtureMapID = 1,
                MatchDateTime = matchDateTime.Value,
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                Bookie = bookie,
                BookieOdds = bookieOdds,
                BetFairLayLevel = betFairLevel,
                BetFairOdds = betFairOdds,
                BetFairCash = betFairCash,
                Predication = prediction,
                BetFairUpdated = betFairUpdated.Value,
                OddsCheckerUpdated = oddsCheckerUpdated.Value,
                Expired = expired,
                Created = created.Value,
                Updated = updated,
                ParentID = parentID
            };

            return record;
        }
    }
}
