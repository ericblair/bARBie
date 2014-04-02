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
        bARBieEntities _barbieEntity;
        IConfigHelper _configHelper;

        private int _expiryLimitHours;
        private DateTime _expiryDate;

        public DeleteExpiredData(bARBieEntities barbieEntity, IConfigHelper configHelper)
        {
            _barbieEntity = barbieEntity;
            _configHelper = configHelper;

            setDataExpiryDate();
        }

        private void setDataExpiryDate()
        {
            _expiryLimitHours = _configHelper.DataExpirationLimitHours();
            _expiryDate = DateTime.Now.AddHours(-_expiryLimitHours);
        }

        public void CleanFixtureAndOddsFromBFAndOCTables()
        {
            var bfExpiredFixtures = _barbieEntity.BetFairFootballFixtures.Where(x => x.MatchDateTime < _expiryDate).ToList();
            var ocExpiredFixtures = _barbieEntity.OddsCheckerFootballFixtures.Where(x => x.MatchDateTime < _expiryDate).ToList();

            var bfExpiredFixtureIDs = bfExpiredFixtures.Select(x => x.ID).ToList();
            var ocExpiredFixtureIDs = ocExpiredFixtures.Select(x => x.ID).ToList();

            var expiredFixtureMaps = _barbieEntity.FootballFixturesMap
                                        .Where(x => bfExpiredFixtureIDs.Contains(x.BetFairFixtureID) || ocExpiredFixtureIDs.Contains(x.OddsCheckerFixtureID))
                                        .ToList();
            var expiredFixtureMapIDs = expiredFixtureMaps.Select(x => x.ID).ToList();

            var bfExpiredOdds = _barbieEntity.BetFairFootballOdds.Where(x => expiredFixtureMapIDs.Contains(x.ID)).ToList();
            var ocExpiredOdds = _barbieEntity.OddsCheckerFootballOdds.Where(x => expiredFixtureMapIDs.Contains(x.ID)).ToList();

            deleteBetFairFootballOddsRecords(bfExpiredOdds);
            deleteOddsCheckerFootballOddsRecords(ocExpiredOdds);
            deleteFixtureMapRecords(expiredFixtureMaps);
            deleteBetFairFootballFixtureRecords(bfExpiredFixtures);
            deleteOddsCheckerFootballFixtureRecords(ocExpiredFixtures);
        }

        private void deleteBetFairFootballOddsRecords(IList<BetFairFootballOdds> recordsToDelete)
        {
            foreach (var record in recordsToDelete)
            {
                _barbieEntity.BetFairFootballOdds.Remove(record);
            }

            _barbieEntity.SaveChanges();
        }

        private void deleteOddsCheckerFootballOddsRecords(IList<OddsCheckerFootballOdds> recordsToDelete)
        {
            foreach (var record in recordsToDelete)
            {
                _barbieEntity.OddsCheckerFootballOdds.Remove(record);
            }

            _barbieEntity.SaveChanges();
        }

        private void deleteFixtureMapRecords(IList<FootballFixturesMap> recordsToDelete)
        {
            foreach (var record in recordsToDelete)
            {
                _barbieEntity.FootballFixturesMap.Remove(record);
            }

            _barbieEntity.SaveChanges();
        }

        private void deleteBetFairFootballFixtureRecords(IList<BetFairFootballFixtures> recordsToDelete)
        {
            foreach (var record in recordsToDelete)
            {
                _barbieEntity.BetFairFootballFixtures.Remove(record);
            }

            _barbieEntity.SaveChanges();
        }

        private void deleteOddsCheckerFootballFixtureRecords(IList<OddsCheckerFootballFixtures> recordsToDelete)
        {
            foreach (var record in recordsToDelete)
            {
                _barbieEntity.OddsCheckerFootballFixtures.Remove(record);
            }

            _barbieEntity.SaveChanges();
        }
    }
}
