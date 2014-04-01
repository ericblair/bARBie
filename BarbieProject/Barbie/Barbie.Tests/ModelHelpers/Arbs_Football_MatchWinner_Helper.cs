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
    }
}
