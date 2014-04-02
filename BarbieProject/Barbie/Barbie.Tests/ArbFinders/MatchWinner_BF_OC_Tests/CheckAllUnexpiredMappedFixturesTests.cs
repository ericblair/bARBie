using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DAL;
using Barbie.ArbFinders;

namespace Barbie.Tests.ArbFinders.MatchWinner_BF_OC_Tests
{
    /// <summary>
    /// Tests for Barbie.Tests.ArbFinders.CheckAllUnexpiredMappedFixtures() method
    /// </summary>
    [TestClass]
    public class CheckAllUnexpiredMappedFixturesTests
    {
        Mock<bARBieEntities> _mockContext;
        MatchWinner_BF_OC _testClass;
        Mock<IConfigHelper> _mockConfigHelper;

        int _betFairCommisionPercentage = 5;

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

            _mockConfigHelper.Setup(m => m.BetFairCommisionPercentage()).Returns(_betFairCommisionPercentage);

            _testClass = new MatchWinner_BF_OC(_mockContext.Object, _mockConfigHelper.Object);

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
        public void Test()
        {
            _testClass.CheckAllUnexpiredMappedFixtures();
        }
    }
}
