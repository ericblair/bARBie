using System;
using System.Collections.Generic;
using System.Configuration;
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

        private int expiryLimitHours;
        private DateTime expiryDate;

        public DeleteExpiredData()
        {
            barbieEntity = new bARBieEntities();

            if (Int32.TryParse(ConfigurationManager.AppSettings["DataExpirationLimitHours"], out expiryLimitHours))
            {
                // log error 
                return;
            }

            expiryDate = DateTime.Now.AddHours(-expiryLimitHours);
        }

        public void CleanFixtureAndOddsFromBF_OC()
        {
            var bfExpiredFixtures = barbieEntity.BetFairFootballFixtures.Where(x => x.MatchDateTime < expiryDate);
            var ocExpiredFixtures = barbieEntity.OddsCheckerFootballFixtures.Where(x => x.MatchDateTime < expiryDate);

            var bfExpiredFixtureIDs = bfExpiredFixtures.Select(x => x.ID).ToList();
            var ocExpiredFixtureIDs = ocExpiredFixtures.Select(x => x.ID).ToList();

            var expiredFixtureMaps = barbieEntity.FootballFixturesMap
                                        .Where(x => bfExpiredFixtureIDs.Contains(x.BetFairFixtureID) || ocExpiredFixtureIDs.Contains(x.OddsCheckerFixtureID));
            var expiredFixtureMapIDs = expiredFixtureMaps.Select(x => x.ID).ToList();

            var bfExpiredOdds = barbieEntity.BetFairFootballOdds.Where(x => expiredFixtureMapIDs.Contains(x.ID));
            var ocExpiredOdds = barbieEntity.OddsCheckerFootballOdds.Where(x => expiredFixtureMapIDs.Contains(x.ID));

            deleteBetFairFootballOddsRecords(bfExpiredOdds);
            deleteOddsCheckerFootballOddsRecords(ocExpiredOdds);
            deleteFixtureMapRecords(expiredFixtureMaps);
            deleteBetFairFootballFixtureRecords(bfExpiredFixtures);
            deleteOddsCheckerFootballFixtureRecords(ocExpiredFixtures);
        }

        private void deleteBetFairFootballOddsRecords(IQueryable<BetFairFootballOdds> recordsToDelete)
        {
            foreach (var record in recordsToDelete)
            {
                barbieEntity.BetFairFootballOdds.Remove(record);
            }

            barbieEntity.SaveChanges();
        }

        private void deleteOddsCheckerFootballOddsRecords(IQueryable<OddsCheckerFootballOdds> recordsToDelete)
        {
            foreach (var record in recordsToDelete)
            {
                barbieEntity.OddsCheckerFootballOdds.Remove(record);
            }

            barbieEntity.SaveChanges();
        }

        private void deleteFixtureMapRecords(IQueryable<FootballFixturesMap> recordsToDelete)
        {
            foreach (var record in recordsToDelete)
            {
                barbieEntity.FootballFixturesMap.Remove(record);
            }

            barbieEntity.SaveChanges();
        }

        private void deleteBetFairFootballFixtureRecords(IQueryable<BetFairFootballFixtures> recordsToDelete)
        {
            foreach (var record in recordsToDelete)
            {
                barbieEntity.BetFairFootballFixtures.Remove(record);
            }

            barbieEntity.SaveChanges();
        }

        private void deleteOddsCheckerFootballFixtureRecords(IQueryable<OddsCheckerFootballFixtures> recordsToDelete)
        {
            foreach (var record in recordsToDelete)
            {
                barbieEntity.OddsCheckerFootballFixtures.Remove(record);
            }

            barbieEntity.SaveChanges();
        }
    }
}
