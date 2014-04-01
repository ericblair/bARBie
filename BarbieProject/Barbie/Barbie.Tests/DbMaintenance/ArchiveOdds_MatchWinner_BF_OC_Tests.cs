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
    /// Summary description for ArchiveOdds_MatchWinner_BF_OC_Tests
    /// </summary>
    [TestClass]
    public class ArchiveOdds_MatchWinner_BF_OC_Tests
    {
        Mock<bARBieEntities> mockContext;

        // Create mock objects for the tables used in test
        FakeDbSet<Arbs_Football_MatchWinner> mockArbsFootballMatchWinnerTable;
        FakeDbSet<Arbs_Football_MatchWinner_Expired> mockExpiredArbsFootballMatchWinnerTable;

        public ArchiveOdds_MatchWinner_BF_OC_Tests()
        {
            mockContext = new Mock<bARBieEntities>();

            mockArbsFootballMatchWinnerTable = new FakeDbSet<Arbs_Football_MatchWinner>();
            mockExpiredArbsFootballMatchWinnerTable = new FakeDbSet<Arbs_Football_MatchWinner_Expired>();
        }

        [TestInitialize()]
        public void Initialize()
        {
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

            var testContext = new ArchiveOdds_MatchWinner_BF_OC(mockContext.Object);
            testContext.ArchiveExpiredOdds();

            // Assert

            // Verify that expired arbs have been moved to expired table
            Assert.AreEqual(1, mockContext.Object.Arbs_Football_MatchWinner_Expired.Count());
            Assert.AreEqual(1, mockContext.Object.Arbs_Football_MatchWinner_Expired.ElementAt(0).OriginalID);
            
            // Verify that unexpired arbs haven't been touched
            Assert.AreEqual(1, mockContext.Object.Arbs_Football_MatchWinner.Count());
            Assert.AreEqual(2, mockContext.Object.Arbs_Football_MatchWinner.ElementAt(0).ID);
        }
    }
}
