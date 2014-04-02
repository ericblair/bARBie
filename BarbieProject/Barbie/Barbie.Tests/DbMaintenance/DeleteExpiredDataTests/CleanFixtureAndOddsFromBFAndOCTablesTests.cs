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
    /// Tests for Barbie.DbMaintenance.DeleteExpiredDataTests.CleanFixtureAndOddsFromBFAndOCTablesTests() method
    /// </summary>
    [TestClass]
    public class CleanFixtureAndOddsFromBFAndOCTablesTests
    {
        Mock<bARBieEntities> mockContext;
        DeleteExpiredData testClass;

        Mock<IConfigHelper> mockConfigHelper;
        int expiryLimitHours;

        // Create mock objects for the tables used in test
        FakeDbSet<BetFairFootballFixtures> mockBetFairFootballFixturesTable;
        FakeDbSet<OddsCheckerFootballFixtures> mockOddsCheckerFootballFixturesTable;
        FakeDbSet<FootballFixturesMap> mockFootballFixturesMapTable;
        FakeDbSet<BetFairFootballOdds> mockBetFairFootballOddsTable;
        FakeDbSet<OddsCheckerFootballOdds> mockOddsCheckerFootballOddsTable;

        [TestInitialize()]
        public void Initialize()
        {
            mockContext = new Mock<bARBieEntities>();
            mockConfigHelper = new Mock<IConfigHelper>();

            expiryLimitHours = 6;
            mockConfigHelper.Setup(m => m.DataExpirationLimitHours()).Returns(expiryLimitHours);

            testClass = new DeleteExpiredData(mockContext.Object, mockConfigHelper.Object);

            mockBetFairFootballFixturesTable = new FakeDbSet<BetFairFootballFixtures>();
            mockOddsCheckerFootballFixturesTable = new FakeDbSet<OddsCheckerFootballFixtures>();
            mockFootballFixturesMapTable = new FakeDbSet<FootballFixturesMap>();
            mockBetFairFootballOddsTable = new FakeDbSet<BetFairFootballOdds>();
            mockOddsCheckerFootballOddsTable = new FakeDbSet<OddsCheckerFootballOdds>();

            // Set mockContext tables to FakeDbSet objects
            mockContext.Object.BetFairFootballFixtures = mockBetFairFootballFixturesTable;
            mockContext.Object.OddsCheckerFootballFixtures = mockOddsCheckerFootballFixturesTable;
            mockContext.Object.FootballFixturesMap = mockFootballFixturesMapTable;
            mockContext.Object.BetFairFootballOdds = mockBetFairFootballOddsTable;
            mockContext.Object.OddsCheckerFootballOdds = mockOddsCheckerFootballOddsTable;
        }


    }
}
