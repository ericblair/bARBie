using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace Barbie.DbMaintenance
{
    /// <summary>
    /// Delete expired fixtures and odds from betfair and oddschecker tables
    /// </summary>
    public class DeleteExpiredData
    {
        bARBieEntities barbieEntity;

        // TODO: Externalise
        private const int expiryLimitHours = 6;
        private DateTime expiryDate;

        public DeleteExpiredData()
        {
            barbieEntity = new bARBieEntities();
            expiryDate = DateTime.Now.AddHours(-expiryLimitHours);
        }

        public void Run()
        {
            var bfExpiredFixtures = barbieEntity.BetFairFootballFixtures.Where(x => x.MatchDateTime < expiryDate);
            var ocExpiredFixtures = barbieEntity.OddsCheckerFootballFixtures.Where(x => x.MatchDateTime < expiryDate);

            var bfExpiredFixtureIDs = bfExpiredFixtures.Select(x => x.ID).ToList();
            var ocExpiredFixtureIDs = ocExpiredFixtures.Select(x => x.ID).ToList();

            var expiredFixtureMaps = barbieEntity.FootballFixturesMap
                                        .Where(x => bfExpiredFixtureIDs.Contains(x.BetFairFixtureID) || ocExpiredFixtureIDs.Contains(x.OddsCheckerFixtureID));
            var expiredFixtureMapIDs = expiredFixtureMaps.Select(x => x.ID).ToList();

            var bfExpiredOdds = barbieEntity.BetFairFootballOdds.Where(x => expiredFixtureMapIDs.Contains(x.ID));

            foreach (var record in bfExpiredOdds)
            {
                barbieEntity.BetFairFootballOdds.Remove(record);
            }

            barbieEntity.SaveChanges();

            var ocExpiredOdds = barbieEntity.OddsCheckerFootballOdds.Where(x => expiredFixtureMapIDs.Contains(x.ID));

            foreach (var record in ocExpiredOdds)
            {
                barbieEntity.OddsCheckerFootballOdds.Remove(record);
            }

            barbieEntity.SaveChanges();

            foreach (var record in expiredFixtureMaps)
            {
                barbieEntity.FootballFixturesMap.Remove(record);
            }

            barbieEntity.SaveChanges();

            foreach (var record in bfExpiredFixtures)
            {
                barbieEntity.BetFairFootballFixtures.Remove(record);
            }

            barbieEntity.SaveChanges();

            foreach (var record in ocExpiredFixtures)
            {
                barbieEntity.OddsCheckerFootballFixtures.Remove(record);
            }

            barbieEntity.SaveChanges();
        }

        private void deleteExpiredFootballOdds()
        {
            //var bfExpiredOdds = barbieEntity.BetFairFootballOdds.Remove
        }
    }
}
