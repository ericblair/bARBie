using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace Barbie.ArbFinders
{
    public class BFMatchWinnerOddsCollection
    {
        bARBieEntities _barbieEntity;
        FootballFixturesMap _fixtureMap;

        string _homeTeam;
        string _awayTeam;
        BetFairFootballOdds _latestHomeOdds = null;
        BetFairFootballOdds _latestAwayOdds = null;
        BetFairFootballOdds _latestDrawOdds = null;

        public string HomeTeam { get { return _homeTeam; } }
        public string AwayTeam { get { return _awayTeam; } }
        public BetFairFootballOdds HomeWinOdds { get { return _latestHomeOdds; } }
        public BetFairFootballOdds AwayWinOdds { get { return _latestAwayOdds; } }
        public BetFairFootballOdds DrawOdds { get { return _latestDrawOdds; } }

        public BFMatchWinnerOddsCollection(FootballFixturesMap fixtureMap)
        {
            _barbieEntity = new bARBieEntities();
            _fixtureMap = fixtureMap;

            setHomeTeam();
            setHomeWinOdds();
            setAwayTeam();
            setAwayWinOdds();
            setDrawOdds();
        }

        private void setHomeTeam()
        {
            _homeTeam = _barbieEntity.BetFairFootballFixtures
                                    .FirstOrDefault(x => x.ID == _fixtureMap.BetFairFixtureID)
                                    .HomeTeam;
        }

        private void setAwayTeam()
        {
            _awayTeam = _barbieEntity.BetFairFootballFixtures
                                    .FirstOrDefault(x => x.ID == _fixtureMap.BetFairFixtureID)
                                    .AwayTeam;
        }

        private void setHomeWinOdds()
        {
            _latestHomeOdds = _barbieEntity.BetFairFootballOdds
                                        .Where(x => x.FixtureID == _fixtureMap.BetFairFixtureID)
                                        .Where(x => x.Prediction == _homeTeam)
                                        .OrderByDescending(x => x.Updated)
                                        .Select(x => x)
                                        .FirstOrDefault();
        }

        private void setAwayWinOdds()
        {
            _latestAwayOdds = _barbieEntity.BetFairFootballOdds
                                        .Where(x => x.FixtureID == _fixtureMap.BetFairFixtureID)
                                        .Where(x => x.Prediction == _awayTeam)
                                        .OrderByDescending(x => x.Updated)
                                        .Select(x => x)
                                        .FirstOrDefault();
        }

        private void setDrawOdds()
        {
            _latestDrawOdds = _barbieEntity.BetFairFootballOdds
                                        .Where(x => x.FixtureID == _fixtureMap.BetFairFixtureID)
                                        .Where(x => x.Prediction == "draw")
                                        .OrderByDescending(x => x.Updated)
                                        .Select(x => x)
                                        .FirstOrDefault();
        }
    }
}
