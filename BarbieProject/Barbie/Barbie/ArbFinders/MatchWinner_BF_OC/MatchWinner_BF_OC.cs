using System;
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
        IRepository _repository;

        private int _matchExpiryLimitMins;
        private DateTime _matchExpiryDateTime;

        public MatchWinner_BF_OC(bARBieEntities barbieEntity, IRepository repository, IConfigHelper configHelper, IArbFinder arbFinder)
        {
            _repository = repository;
            _barbieEntity = barbieEntity;
            _configHelper = configHelper;
            _arbFinder = arbFinder;

            _matchExpiryLimitMins = _configHelper.MaxTotalMatchTimeMins();
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

        /// <summary>
        /// This method will mark arbs as expired if the date time of the match exceeds the expiry date time
        /// </summary>
        public void ExpireArbsForFinishedMatches()
        {
            var expiredFixtures = _barbieEntity.Arbs_Football_MatchWinner
                                        .Where(x => x.MatchDateTime < _matchExpiryDateTime && x.Expired != true)
                                        .ToList();

            foreach (var arb in expiredFixtures)
            {
                arb.Expired = true;
                arb.Updated = DateTime.Now;
            }

            _barbieEntity.SaveChanges();
        }

        /// <summary>
        /// Check to see if the odds have been updated since the arb was last updated. If arb is outdated, 
        /// check the odds again for an arb
        /// </summary>
        /// <param name="arb">The Arb record to update</param>
        public void UpdateArb(Arbs_Football_MatchWinner arb)
        {
            if (arb.Expired == true)
                return;

            var betfairOddsLastUpdated = _repository.GetDateTimeBetfairFootballOddsLastUpdated(_repository.GetBetfairFixtureIdForArb(arb.FixtureMapID));
            var oddscheckerOddsLastUpdated = _repository.GetDateTimeOddscheckerFootballOddsLastUpdated(_repository.GetOddscheckerFixtureIdForArb(arb.FixtureMapID));

            var betfairOddsHaveBeenUpdatedSinceArbWasUpdated = haveOddsBeenUpdatedSinceArbWasLastUpdated(betfairOddsLastUpdated, arb.BetFairUpdated);
            var oddscheckerOddsHaveBeenUpdatedSinceArbWasUpdated = haveOddsBeenUpdatedSinceArbWasLastUpdated(oddscheckerOddsLastUpdated, arb.OddsCheckerUpdated);

            if (betfairOddsHaveBeenUpdatedSinceArbWasUpdated || oddscheckerOddsHaveBeenUpdatedSinceArbWasUpdated)
            {
                _arbFinder.CheckLatestOddsForArbs(arb.FixtureMapID);
            }
        }

        private bool haveOddsBeenUpdatedSinceArbWasLastUpdated(DateTime oddsLastUpdated, DateTime arbLastUpdated)
        {
            if (oddsLastUpdated > arbLastUpdated)
                return true;
            else
                return false;
        }
    }
}
