﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace Barbie.ArbFinders
{
    public class MatchWinner_BF_OC
    {
        bARBieEntities _barbieEntity;
        IConfigHelper _configHelper;
        IArbFinder _arbFinder;

        private int _matchExpiryLimitMins;
        private DateTime _matchExpiryDateTime;

        public MatchWinner_BF_OC(bARBieEntities barbieEntity, IConfigHelper configHelper, IArbFinder arbFinder)
        {
            _barbieEntity = barbieEntity;
            _configHelper = configHelper;
            _arbFinder = arbFinder;

            _matchExpiryDateTime = DateTime.Now.AddMinutes(-_matchExpiryLimitMins);
        }

        public void CheckAllUnexpiredMappedFixtures()
        {
            var mappedFixtures = _barbieEntity.FootballFixturesMap
                                    .Where(x => x.OddsCheckerFootballFixtures.MatchDateTime >= _matchExpiryDateTime)
                                    .OrderBy(x => x.OddsCheckerFootballFixtures.MatchDateTime)
                                    .ToList();

            _arbFinder.FindArbs(mappedFixtures);
        }

        // TODO: think about the arbs expiry process
        public void SetArbsExpiredForFinishedMatches()
        {
            // Get all arbs where matchdatetime has expired
            var expiredFixtures = (from arb in _barbieEntity.Arbs_Football_MatchWinner
                                   join map in _barbieEntity.FootballFixturesMap on arb.FixtureMapID equals map.ID
                                   join oc in _barbieEntity.OddsCheckerFootballFixtures on map.OddsCheckerFixtureID equals oc.ID
                                   where oc.MatchDateTime < _matchExpiryDateTime
                                   select arb).ToList();

            if (expiredFixtures.Count > 0)
            {
                foreach (var arb in expiredFixtures)
                {
                    arb.Expired = true;
                    arb.Updated = DateTime.Now;
                }

                _barbieEntity.SaveChanges();
            }
        }

        public void SetArbsExpiredForOutdatedOdds()
        {
            // Check to see if the arb reflects the latest odds
            var arbs = _barbieEntity.Arbs_Football_MatchWinner
                        .Where(x => x.Expired == false || x.Expired == null)
                        .OrderBy(x => x.MatchDateTime)
                        .ToList();

            foreach (var arb in arbs)
            {
                var bfFixtureId = _barbieEntity.FootballFixturesMap.Where(x => x.ID == arb.FixtureMapID).Select(x => x.BetFairFixtureID).First();
                var ocFixtureId = _barbieEntity.FootballFixturesMap.Where(x => x.ID == arb.FixtureMapID).Select(x => x.OddsCheckerFixtureID).First();

                var bfOddsUpdated = _barbieEntity.BetFairFootballOdds.Where(x => x.FixtureID == bfFixtureId).OrderByDescending(x => x.ID).Select(x => x.Updated).First();
                var ocOddsUpdated = _barbieEntity.OddsCheckerFootballOdds.Where(x => x.FixtureID == ocFixtureId).OrderByDescending(x => x.ID).Select(x => x.Updated).First();

                if (arb.BetFairUpdated < bfOddsUpdated && arb.OddsCheckerUpdated < ocOddsUpdated)
                {
                    arb.Expired = true;
                    arb.Updated = DateTime.Now;
                    _barbieEntity.SaveChanges();
                }
            }
        }
    }
}
