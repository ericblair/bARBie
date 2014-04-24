using System;
using System.Data.Entity;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DAL;
using Barbie.DbMaintenance;
using System.Linq;

namespace Barbie.Tests.DbMaintenance
{
    /// <summary>
    /// Tests for Barbie.DbMaintenance.ArchiveOdds_MatchWinner_BF_OC class
    /// </summary>
    [TestClass]
    public class ArchiveExpiredOdds_Tests
    {
        Mock<bARBieEntities> mockContext;
        ArchiveOdds_MatchWinner_BF_OC testClass;

        // Create mock objects for the tables used in test
        FakeDbSet<Arbs_Football_MatchWinner> mockArbsFootballMatchWinnerTable;
        FakeDbSet<Arbs_Football_MatchWinner_Expired> mockExpiredArbsFootballMatchWinnerTable;

        [TestInitialize()]
        public void Initialize()
        {
            mockContext = new Mock<bARBieEntities>();
            testClass = new ArchiveOdds_MatchWinner_BF_OC(mockContext.Object);

            mockArbsFootballMatchWinnerTable = new FakeDbSet<Arbs_Football_MatchWinner>();
            mockExpiredArbsFootballMatchWinnerTable = new FakeDbSet<Arbs_Football_MatchWinner_Expired>();

            // Set mockContext tables to FakeDbSet objects
            mockContext.Object.Arbs_Football_MatchWinner = mockArbsFootballMatchWinnerTable;
            mockContext.Object.Arbs_Football_MatchWinner_Expired = mockExpiredArbsFootballMatchWinnerTable;
        }

        [TestMethod]
        public void ArchiveExpiredOdds_VerifyOnlyExpiredFixtureIsArchived()
        {
            // Arrange

            var expiredArbRecord = ModelHelpers.Arbs_Football_MatchWinner_Helper.CreateRecord(id: 1, expired: true);
            var unexpiredArbRecord = ModelHelpers.Arbs_Football_MatchWinner_Helper.CreateRecord(id: 2, expired: false);

            mockArbsFootballMatchWinnerTable.Add(expiredArbRecord);
            mockArbsFootballMatchWinnerTable.Add(unexpiredArbRecord);

            // Act

            testClass.ArchiveExpiredOdds();

            // Assert

            // Verify that expired arbs have been moved to expired table
            Assert.AreEqual(1, mockContext.Object.Arbs_Football_MatchWinner_Expired.Count());
            Assert.AreEqual(1, mockContext.Object.Arbs_Football_MatchWinner_Expired.First().OriginalID);
            
            // Verify that unexpired arbs haven't been touched
            Assert.AreEqual(1, mockContext.Object.Arbs_Football_MatchWinner.Count());
            Assert.AreEqual(2, mockContext.Object.Arbs_Football_MatchWinner.First().ID);
        }

        [TestMethod]
        public void ArchiveExpiredOdds_VerifyCorrectDataIsArchived()
        {
            // Arrange

            int id = 2;
            int fixtureMapID = 1;
            bool expired = true;
            DateTime matchDateTime = DateTime.Now;
            string homeTeam = "Everton";
            string awayTeam = "Liverpool";
            string bookie = "Bet365";
            decimal bookieOdds = 5;
            string betFairLevel = "LOW";
            decimal betFairOdds = 4;
            decimal betFairCash = 1000;
            string prediction = "Everton";
            DateTime betFairUpdated = DateTime.Now;
            DateTime oddsCheckerUpdated = DateTime.Now; 
            DateTime created = DateTime.Now;
            DateTime updated = DateTime.Now;
            int parentID = 1;

            var arbRecord = 
                ModelHelpers.Arbs_Football_MatchWinner_Helper.CreateRecord(id, expired, matchDateTime, fixtureMapID, homeTeam, awayTeam, bookie, bookieOdds,
                                                                        betFairLevel, betFairOdds, betFairCash, prediction, betFairUpdated,
                                                                        oddsCheckerUpdated, created, updated, parentID);

            mockArbsFootballMatchWinnerTable.Add(arbRecord);

            // Act

            testClass.ArchiveExpiredOdds();

            // Assert

            var expiredRecord = mockContext.Object.Arbs_Football_MatchWinner_Expired.First();
            AssertCorrectDataHasBeenArchived(arbRecord, expiredRecord);
        }

        // TODO: Consider moving to helper class if required elsewhere
        private void AssertCorrectDataHasBeenArchived(Arbs_Football_MatchWinner arbRecord, Arbs_Football_MatchWinner_Expired expiredRecord)
        {
            Assert.AreEqual(arbRecord.ID, expiredRecord.OriginalID);
            Assert.AreEqual(arbRecord.MatchDateTime, expiredRecord.MatchDateTime);
            Assert.AreEqual(arbRecord.HomeTeam, expiredRecord.HomeTeam);
            Assert.AreEqual(arbRecord.AwayTeam, expiredRecord.AwayTeam);
            Assert.AreEqual(arbRecord.Bookie, expiredRecord.Bookie);
            Assert.AreEqual(arbRecord.BookieOdds, expiredRecord.BookieOdds);
            Assert.AreEqual(arbRecord.BetFairLayLevel, expiredRecord.BetFairLayLevel);
            Assert.AreEqual(arbRecord.BetFairOdds, expiredRecord.BetFairOdds);
            Assert.AreEqual(arbRecord.BetFairCash, expiredRecord.BetFairCash);
            Assert.AreEqual(arbRecord.Predication, expiredRecord.Predication);
            Assert.AreEqual(arbRecord.BetFairUpdated, expiredRecord.BetFairUpdated);
            Assert.AreEqual(arbRecord.OddsCheckerUpdated, expiredRecord.OddsCheckerUpdated);
            Assert.AreEqual(arbRecord.Created, expiredRecord.Created);
            Assert.AreEqual(arbRecord.Updated, expiredRecord.Updated);
            Assert.AreEqual(arbRecord.ParentID, expiredRecord.ParentID);
        }
    }
}
