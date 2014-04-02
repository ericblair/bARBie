using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DAL;
using Barbie.FixtureMappers;

namespace Barbie.Tests.FixtureMappers.MatchWinner_BF_OC_Tests
{
    /// <summary>
    /// Tests for Barbie.FixtureMappers.MatchWinner_BF_OC_Tests.MapFixtures() method
    /// </summary>
    [TestClass]
    public class MapFixturesTests
    {
        Mock<bARBieEntities> _mockContext;
        MatchWinner_BF_OC _testClass;

        int _maxLevenshteinValue;

        Mock<IConfigHelper> _mockConfigHelper;

        // Create mock objects for the tables used in test
        FakeDbSet<BetFairFootballFixtures> mockBetFairFootballFixturesTable;
        FakeDbSet<OddsCheckerFootballFixtures> mockOddsCheckerFootballFixturesTable;
        FakeDbSet<FootballFixturesMap> mockFootballFixturesMapTable;

        [TestInitialize]
        public void Initialize()
        {
            _mockContext = new Mock<bARBieEntities>();
            _mockConfigHelper = new Mock<IConfigHelper>();

            _maxLevenshteinValue = 15;
            _mockConfigHelper.Setup(m => m.StringMatchingMaxLevenshteinValue()).Returns(_maxLevenshteinValue);

            _testClass = new MatchWinner_BF_OC(_mockContext.Object, _mockConfigHelper.Object);

            mockBetFairFootballFixturesTable = new FakeDbSet<BetFairFootballFixtures>();
            mockOddsCheckerFootballFixturesTable = new FakeDbSet<OddsCheckerFootballFixtures>();
            mockFootballFixturesMapTable = new FakeDbSet<FootballFixturesMap>();

            // Set mockContext tables to FakeDbSet objects
            _mockContext.Object.BetFairFootballFixtures = mockBetFairFootballFixturesTable;
            _mockContext.Object.OddsCheckerFootballFixtures = mockOddsCheckerFootballFixturesTable;
            _mockContext.Object.FootballFixturesMap = mockFootballFixturesMapTable;
        }

        [TestMethod]
        public void MapFixtures_VerifyCanMapExactMatch()
        {
            // Arrange

            int id = 1;
            int competitionID = 1;
            DateTime matchDateTime = DateTime.Now;
            string homeTeam = "Southampton";
            string awayTeam = "Sunderland";

            var betFairFixture = ModelHelpers.BetFairFootballFixtures_Helper.CreateRecord(id, competitionID, matchDateTime, homeTeam, awayTeam);
            mockBetFairFootballFixturesTable.Add(betFairFixture);

            var oddsCheckerFixture = ModelHelpers.OddsCheckerFootballFixtures_Helper.CreateRecord(id, competitionID, matchDateTime, homeTeam, awayTeam);
            mockOddsCheckerFootballFixturesTable.Add(oddsCheckerFixture);

            // Act

            _testClass.MapFixtures();

            // Assert

            Assert.AreEqual(1, _mockContext.Object.FootballFixturesMap.Count());
            Assert.AreEqual(id, _mockContext.Object.FootballFixturesMap.First().BetFairFixtureID);
            Assert.AreEqual(id, _mockContext.Object.FootballFixturesMap.First().OddsCheckerFixtureID);
        }

        [TestMethod]
        public void MapFixtures_VerifyMatchesCorrectFixtures()
        {
            int id1 = 1;
            int id2 = 2;
            int competitionID = 1;
            DateTime matchDateTime = DateTime.Now;

            var betFairFixture1 = ModelHelpers.BetFairFootballFixtures_Helper.CreateRecord(id1, competitionID, matchDateTime, 
                                                                                            homeTeam: "Sunderland", awayTeam: "Man Utd");
            var betFairFixture2 = ModelHelpers.BetFairFootballFixtures_Helper.CreateRecord(id2, competitionID, matchDateTime,
                                                                                            homeTeam: "Southampton", awayTeam: "Man City");
            mockBetFairFootballFixturesTable.Add(betFairFixture1);
            mockBetFairFootballFixturesTable.Add(betFairFixture2);

            var oddsCheckerFixture1 = ModelHelpers.OddsCheckerFootballFixtures_Helper.CreateRecord(id1, competitionID, matchDateTime,
                                                                                            homeTeam: "Sunderland AFC", awayTeam: "MUTD");
            var oddsCheckerFixture2 = ModelHelpers.OddsCheckerFootballFixtures_Helper.CreateRecord(id2, competitionID, matchDateTime,
                                                                                            homeTeam: "Sthmptn", awayTeam: "Manchester City");
            mockOddsCheckerFootballFixturesTable.Add(oddsCheckerFixture1);
            mockOddsCheckerFootballFixturesTable.Add(oddsCheckerFixture2);

            // Act

            _testClass.MapFixtures();

            // Assert

            Assert.AreEqual(2, _mockContext.Object.FootballFixturesMap.Count());
            Assert.AreEqual(
                _mockContext.Object.FootballFixturesMap.Where(x => x.BetFairFixtureID == id1).First().ID,
                _mockContext.Object.FootballFixturesMap.Where(x => x.OddsCheckerFixtureID == id1).First().ID);
            Assert.AreEqual(
                _mockContext.Object.FootballFixturesMap.Where(x => x.BetFairFixtureID == id2).First().ID,
                _mockContext.Object.FootballFixturesMap.Where(x => x.OddsCheckerFixtureID == id2).First().ID);
        }
    }
}
