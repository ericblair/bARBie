using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace Barbie.ArbFinders
{
    public class OCMatchWinnerOddsCollection
    {
        bARBieEntities _barbieEntity;
        FootballFixturesMap _fixtureMap;

        string _homeTeam;
        string _awayTeam;
        OddsCheckerFootballOdds _latestHomeOdds = null;
        OddsCheckerFootballOdds _latestAwayOdds = null;
        OddsCheckerFootballOdds _latestDrawOdds = null;

        public string HomeTeam { get { return _homeTeam; } }
        public string AwayTeam { get { return _awayTeam; } }
        public OddsCheckerFootballOdds HomeWinOdds { get { return _latestHomeOdds; } }
        public OddsCheckerFootballOdds AwayWinOdds { get { return _latestAwayOdds; } }
        public OddsCheckerFootballOdds DrawOdds { get { return _latestDrawOdds; } }

        public OCMatchWinnerOddsCollection(FootballFixturesMap fixtureMap)
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
            _homeTeam = _barbieEntity.OddsCheckerFootballFixtures
                                    .FirstOrDefault(x => x.ID == _fixtureMap.OddsCheckerFixtureID)
                                    .HomeTeam;
        }

        private void setAwayTeam()
        {
            _awayTeam = _barbieEntity.OddsCheckerFootballFixtures
                                    .FirstOrDefault(x => x.ID == _fixtureMap.OddsCheckerFixtureID)
                                    .AwayTeam;
        }

        private void setHomeWinOdds()
        {
            _latestHomeOdds = _barbieEntity.OddsCheckerFootballOdds
                                        .Where(x => x.FixtureID == _fixtureMap.OddsCheckerFixtureID)
                                        .Where(x => x.Prediction == _homeTeam)
                                        .OrderByDescending(x => x.Updated)
                                        .Select(x => x)
                                        .FirstOrDefault();
        }

        private void setAwayWinOdds()
        {
            _latestAwayOdds = _barbieEntity.OddsCheckerFootballOdds
                                        .Where(x => x.FixtureID == _fixtureMap.OddsCheckerFixtureID)
                                        .Where(x => x.Prediction == _awayTeam)
                                        .OrderByDescending(x => x.Updated)
                                        .Select(x => x)
                                        .FirstOrDefault();
        }

        private void setDrawOdds()
        {
            _latestDrawOdds = _barbieEntity.OddsCheckerFootballOdds
                                        .Where(x => x.FixtureID == _fixtureMap.OddsCheckerFixtureID)
                                        .Where(x => x.Prediction == "draw")
                                        .OrderByDescending(x => x.Updated)
                                        .Select(x => x)
                                        .FirstOrDefault();
        }
    }
}
