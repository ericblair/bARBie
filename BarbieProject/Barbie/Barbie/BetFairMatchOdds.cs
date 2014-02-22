using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Objects;
using System.Data.Objects.SqlClient;

namespace Barbie
{
    public class BetFairMatchOdds
    {
        private BetFairFootballOdd _homeWinData = null;
        private BetFairFootballOdd _awayWinData = null;
        private BetFairFootballOdd _drawData = null;

        public BetFairFootballOdd HomeWinData { get { return _homeWinData; } }
        public BetFairFootballOdd AwayWinData { get { return _awayWinData; } }
        public BetFairFootballOdd DrawData { get { return _drawData; } }

        public BetFairMatchOdds(bARBieEntities barbieEntity, FootballMatch matchDetails)
        {
            SetHomeWinData(barbieEntity, matchDetails);
            if (_homeWinData == null) return;
            SetAwayWinData(barbieEntity, matchDetails);
            if (_awayWinData == null) return;
            SetDrawData(barbieEntity, matchDetails);
        }

        public bool AreOddsSet { get { return oddsDataSet(); } }

        private bool oddsDataSet()
        {
            if (_homeWinData == null || _awayWinData == null || _drawData == null)
                return false;
            else
                return true;
        }

        private void SetHomeWinData(bARBieEntities barbieEntity, FootballMatch matchDetails)
        {
            _homeWinData = (from x in barbieEntity.BetFairFootballOdds
                            where SqlFunctions.Difference(matchDetails.Fixture, x.Fixture) == 4
                            && x.MatchDateTime > EntityFunctions.AddHours(matchDetails.MatchDateTime, -1)
                            && x.MatchDateTime < EntityFunctions.AddHours(matchDetails.MatchDateTime, 3)
                            && x.Prediction == x.HomeTeam
                            orderby x.Updated descending
                            select x).FirstOrDefault();
        }

        private void SetAwayWinData(bARBieEntities barbieEntity, FootballMatch matchDetails)
        {
            _awayWinData = (from x in barbieEntity.BetFairFootballOdds
                            where SqlFunctions.Difference(matchDetails.Fixture, x.Fixture) == 4
                            && x.MatchDateTime > EntityFunctions.AddHours(matchDetails.MatchDateTime, -1)
                            && x.MatchDateTime < EntityFunctions.AddHours(matchDetails.MatchDateTime, 3)
                            && x.Prediction == x.AwayTeam
                            orderby x.Updated descending
                            select x).FirstOrDefault();
        }

        private void SetDrawData(bARBieEntities barbieEntity, FootballMatch matchDetails)
        {
            _drawData = (from x in barbieEntity.BetFairFootballOdds
                         where SqlFunctions.Difference(matchDetails.Fixture, x.Fixture) == 4
                         && x.MatchDateTime > EntityFunctions.AddHours(matchDetails.MatchDateTime, -1)
                         && x.MatchDateTime < EntityFunctions.AddHours(matchDetails.MatchDateTime, 3)
                         && x.Prediction == "draw"
                         orderby x.Updated descending
                         select x).FirstOrDefault();
        }
    }
}
