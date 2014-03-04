using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace Barbie
{
    public class OddsCheckerMatchOdds
    {
        //private OddsCheckerFootballOdds _homeWinData = null;
        //private OddsCheckerFootballOdds _awayWinData = null;
        //private OddsCheckerFootballOdds _drawData = null;

        //public OddsCheckerFootballOdds HomeWinData { get { return _homeWinData; } }
        //public OddsCheckerFootballOdds AwayWinData { get { return _awayWinData; } }
        //public OddsCheckerFootballOdds DrawData { get { return _drawData; } }

        //public OddsCheckerMatchOdds(bARBieEntities barbieEntity, FootballMatch matchDetails)
        //{
        //    SetHomeWinData(barbieEntity, matchDetails);
        //    if (_homeWinData == null) return;
        //    SetAwayWinData(barbieEntity, matchDetails);
        //    if (_awayWinData == null) return;
        //    SetDrawData(barbieEntity, matchDetails);
        //}

        //public bool AreOddsSet { get { return oddsDataSet(); } }

        //private bool oddsDataSet()
        //{
        //    if (_homeWinData == null || _awayWinData == null || _drawData == null)
        //        return false;
        //    else
        //        return true;
        //}

        //private void SetHomeWinData(bARBieEntities barbieEntity, FootballMatch matchDetails)
        //{
        //    _homeWinData = (from x in barbieEntity.OddsCheckerFootballOdds
        //                    where x.Fixture == matchDetails.Fixture
        //                    && x.MatchDateTime == matchDetails.MatchDateTime
        //                    && x.Prediction == x.HomeTeam
        //                    orderby x.Updated descending
        //                    select x).FirstOrDefault();
        //}

        //private void SetAwayWinData(bARBieEntities barbieEntity, FootballMatch matchDetails)
        //{
        //    _awayWinData = (from x in barbieEntity.OddsCheckerFootballOdds
        //                    where x.Fixture == matchDetails.Fixture
        //                    && x.MatchDateTime == matchDetails.MatchDateTime
        //                    && x.Prediction == x.AwayTeam
        //                    orderby x.Updated descending
        //                    select x).FirstOrDefault();
        //}

        //private void SetDrawData(bARBieEntities barbieEntity, FootballMatch matchDetails)
        //{
        //    _drawData = (from x in barbieEntity.OddsCheckerFootballOdds
        //                 where x.Fixture == matchDetails.Fixture
        //                 && x.MatchDateTime == matchDetails.MatchDateTime
        //                 && x.Prediction == "draw"
        //                 orderby x.Updated descending
        //                 select x).FirstOrDefault();
        //}
    }
}
