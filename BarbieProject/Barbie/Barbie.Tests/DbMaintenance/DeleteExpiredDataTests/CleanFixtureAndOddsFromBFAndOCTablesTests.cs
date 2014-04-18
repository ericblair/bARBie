using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DAL;
using Barbie.DbMaintenance;

namespace Barbie.Tests.DbMaintenance.DeleteExpiredDataTests
{
    /// <summary>
    /// Tests for Barbie.DbMaintenance.DeleteExpiredDataTests.CleanFixtureAndOddsFromBFAndOCTables() method
    /// </summary>
    [TestClass]
    public class CleanFixtureAndOddsFromBFAndOCTablesTests
    {
        Mock<bARBieEntities> _mockContext;
        Mock<IConfigHelper> _mockConfigHelper;
        DeleteExpiredData _testClass;

        int _expiryLimitHours;

        // Create mock objects for the tables used in test
        FakeDbSet<BetFairFootballFixtures> mockBetFairFootballFixturesTable;
        FakeDbSet<OddsCheckerFootballFixtures> mockOddsCheckerFootballFixturesTable;
        FakeDbSet<FootballFixturesMap> mockFootballFixturesMapTable;
        FakeDbSet<BetFairFootballOdds> mockBetFairFootballOddsTable;
        FakeDbSet<OddsCheckerFootballOdds> mockOddsCheckerFootballOddsTable;

        [TestInitialize]
        public void Initialize()
        {
            _mockContext = new Mock<bARBieEntities>();
            _mockConfigHelper = new Mock<IConfigHelper>();

            _expiryLimitHours = 6;
            _mockConfigHelper.Setup(m => m.DataExpirationLimitHours()).Returns(_expiryLimitHours);

            _testClass = new DeleteExpiredData(_mockContext.Object, _mockConfigHelper.Object);

            mockBetFairFootballFixturesTable = new FakeDbSet<BetFairFootballFixtures>();
            mockOddsCheckerFootballFixturesTable = new FakeDbSet<OddsCheckerFootballFixtures>();
            mockFootballFixturesMapTable = new FakeDbSet<FootballFixturesMap>();
            mockBetFairFootballOddsTable = new FakeDbSet<BetFairFootballOdds>();
            mockOddsCheckerFootballOddsTable = new FakeDbSet<OddsCheckerFootballOdds>();

            // Set mockContext tables to FakeDbSet objects
            _mockContext.Object.BetFairFootballFixtures = mockBetFairFootballFixturesTable;
            _mockContext.Object.OddsCheckerFootballFixtures = mockOddsCheckerFootballFixturesTable;
            _mockContext.Object.FootballFixturesMap = mockFootballFixturesMapTable;
            _mockContext.Object.BetFairFootballOdds = mockBetFairFootballOddsTable;
            _mockContext.Object.OddsCheckerFootballOdds = mockOddsCheckerFootballOddsTable;
        }

        [TestMethod]
        public void CleanFixtureAndOddsFromBFAndOCTables_DeletesOnlyExpiredData()
        {
            // Arrange

            // For each table cleaned by method under test, add an expired and unexpired record
            var expiredMatchDateTime = DateTime.Now.AddHours(-(_expiryLimitHours + 2));
            var unexpiredMatchDateTime = DateTime.Now.AddHours(-(_expiryLimitHours - 2));

            var betFairFootballFixtureExpiredRecord = ModelHelpers.BetFairFootballFixtures_Helper.CreateRecord(id: 1, matchDateTime: expiredMatchDateTime);
            var betFairFootballFixtureUnexpiredRecord = ModelHelpers.BetFairFootballFixtures_Helper.CreateRecord(id: 2, matchDateTime: unexpiredMatchDateTime);

            mockBetFairFootballFixturesTable.Add(betFairFootballFixtureExpiredRecord);
            mockBetFairFootballFixturesTable.Add(betFairFootballFixtureUnexpiredRecord);

            var oddsCheckerFootballFixtureExpiredRecord = ModelHelpers.OddsCheckerFootballFixtures_Helper.CreateRecord(id: 1, matchDateTime: expiredMatchDateTime);
            var oddsCheckerFootballFixtureUnexpiredRecord = ModelHelpers.OddsCheckerFootballFixtures_Helper.CreateRecord(id: 2, matchDateTime: unexpiredMatchDateTime);

            mockOddsCheckerFootballFixturesTable.Add(oddsCheckerFootballFixtureExpiredRecord);
            mockOddsCheckerFootballFixturesTable.Add(oddsCheckerFootballFixtureUnexpiredRecord);

            var footballFixtureMapExpiredRecord = ModelHelpers.FootballFixturesMap_Helper.CreateRecord(id: 1, betFairFixtureID: 1, oddsCheckerFixtureID: 1);
            var footballFixtureMapUnexpiredRecord = ModelHelpers.FootballFixturesMap_Helper.CreateRecord(id: 2, betFairFixtureID: 2, oddsCheckerFixtureID: 2);

            mockFootballFixturesMapTable.Add(footballFixtureMapExpiredRecord);
            mockFootballFixturesMapTable.Add(footballFixtureMapUnexpiredRecord);

            var betFairFootballOddsExpiredRecord = ModelHelpers.BetFairFootballOdds_Helper.CreateRecord(id: 1, fixtureID: 1);
            var betFairFootballOddsUnexpiredRecord = ModelHelpers.BetFairFootballOdds_Helper.CreateRecord(id: 2, fixtureID: 2);
            mockBetFairFootballOddsTable.Add(betFairFootballOddsExpiredRecord);
            mockBetFairFootballOddsTable.Add(betFairFootballOddsUnexpiredRecord);

            var oddsCheckerFootballOddsExpiredRecord = ModelHelpers.OddsCheckerFootballOdds_Helper.CreateRecord(id: 1, fixtureID: 1);
            var oddsCheckerFootballOddsUnexpiredRecord = ModelHelpers.OddsCheckerFootballOdds_Helper.CreateRecord(id: 2, fixtureID: 2);
            mockOddsCheckerFootballOddsTable.Add(oddsCheckerFootballOddsExpiredRecord);
            mockOddsCheckerFootballOddsTable.Add(oddsCheckerFootballOddsUnexpiredRecord);

            // Act

            _testClass.CleanFixtureAndOddsFromBFAndOCTables();

            // Assert

            Assert.AreEqual(1, _mockContext.Object.BetFairFootballFixtures.Count());
            Assert.AreEqual(2, _mockContext.Object.BetFairFootballFixtures.First().ID);

            Assert.AreEqual(1, _mockContext.Object.OddsCheckerFootballFixtures.Count());
            Assert.AreEqual(2, _mockContext.Object.OddsCheckerFootballFixtures.First().ID);

            Assert.AreEqual(1, _mockContext.Object.FootballFixturesMap.Count());
            Assert.AreEqual(2, _mockContext.Object.FootballFixturesMap.First().ID);

            Assert.AreEqual(1, _mockContext.Object.BetFairFootballOdds.Count());
            Assert.AreEqual(2, _mockContext.Object.BetFairFootballOdds.First().ID);

            Assert.AreEqual(1, _mockContext.Object.OddsCheckerFootballOdds.Count());
            Assert.AreEqual(2, _mockContext.Object.OddsCheckerFootballOdds.First().ID);
        }
    }
}
