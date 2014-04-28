using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DAL;
using Barbie.Arbs;

namespace Barbie.Tests.ArbFinders.MatchWinner_BF_OC_Tests
{
    /// <summary>
    /// Tests for Barbie.ArbFinders.MatchWinner_BF_OC.UpdateArb() method
    /// </summary>
    [TestClass]
    public class UpdateArbTests
    {
        Mock<bARBieEntities> _mockContext;
        Mock<IRepository> _mockRepository;
        Mock<IConfigHelper> _mockConfigHelper;
        Mock<IArbFinder> _mockArbFinder;
        MatchWinner_BF_OC_Controller _testClass;

        // Create mock objects for the tables used in test
        FakeDbSet<Arbs_Football_MatchWinner> _mockArbsFootballMatchWinnerTable;

        [TestInitialize]
        public void Initialize()
        {
            _mockContext = new Mock<bARBieEntities>();
            _mockRepository = new Mock<IRepository>();
            _mockConfigHelper = new Mock<IConfigHelper>();
            _mockArbFinder = new Mock<IArbFinder>();

            //_mockConfigHelper.Setup(m => m.MaxTotalMatchTimeMins()).Returns(_maxMatchTimeMins);

            _testClass = new MatchWinner_BF_OC_Controller(_mockContext.Object, _mockRepository.Object, _mockConfigHelper.Object, _mockArbFinder.Object);

            _mockArbsFootballMatchWinnerTable = new FakeDbSet<Arbs_Football_MatchWinner>();

            // Set mockContext tables to FakeDbSet objects
            _mockContext.Object.Arbs_Football_MatchWinner = _mockArbsFootballMatchWinnerTable;
        }

        [TestMethod]
        public void UpdateArb_ArbExpired_ArbNotUpdated()
        {
            // Arrange

            var expiredRecord = ModelHelpers.Arbs_Football_MatchWinner_Helper.CreateRecord(id: 1, expired: true, fixtureMapID: 1);

            // Act

            _testClass.UpdateArb(expiredRecord);

            // Assert

            _mockArbFinder.Verify(af => af.CheckLatestOddsForArbs(It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void UpdateArb_FixtureOddsNeverBeenUpdated_ArbNotUpdated()
        {
            // Arrange

            var fixtureMapID = 1;
            var betfairFixtureID = 2;
            var oddscheckerFixtureID = 3;

            _mockRepository.Setup(r => r.GetBetfairFixtureIdForArb(fixtureMapID)).Returns(betfairFixtureID);
            _mockRepository.Setup(r => r.GetDateTimeBetfairFootballOddsLastUpdated(betfairFixtureID)).Returns(null);

            _mockRepository.Setup(r => r.GetOddscheckerFixtureIdForArb(fixtureMapID)).Returns(oddscheckerFixtureID);
            _mockRepository.Setup(r => r.GetDateTimeOddscheckerFootballOddsLastUpdated(oddscheckerFixtureID)).Returns(null);

            var arbRecord = ModelHelpers.Arbs_Football_MatchWinner_Helper.CreateRecord(id: 1, expired: false, fixtureMapID: fixtureMapID);

            // Act

            _testClass.UpdateArb(arbRecord);

            // Assert

            _mockArbFinder.Verify(af => af.CheckLatestOddsForArbs(It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void UpdateArb_OddsNotUpdatedSinceArbLastUpdated_ArbNotUpdated()
        {
            // Arrange

            var fixtureMapID = 1;
            var betfairFixtureID = 2;
            var oddscheckerFixtureID = 3;

            var arbBetfairLastUpdated = DateTime.Now.AddDays(-1);
            var arbOddscheckerLastUpdated = DateTime.Now.AddDays(-1);

            var betfairOddsLastUpdated = DateTime.Now.AddDays(-3);
            var oddscheckerOddsLastUpdated = DateTime.Now.AddDays(-3);

            _mockRepository.Setup(r => r.GetBetfairFixtureIdForArb(fixtureMapID)).Returns(betfairFixtureID);
            _mockRepository.Setup(r => r.GetDateTimeBetfairFootballOddsLastUpdated(betfairFixtureID)).Returns(betfairOddsLastUpdated);

            _mockRepository.Setup(r => r.GetOddscheckerFixtureIdForArb(fixtureMapID)).Returns(oddscheckerFixtureID);
            _mockRepository.Setup(r => r.GetDateTimeOddscheckerFootballOddsLastUpdated(oddscheckerFixtureID)).Returns(oddscheckerOddsLastUpdated);

            var arbRecord = ModelHelpers.Arbs_Football_MatchWinner_Helper.CreateRecord(id: 1, expired: false, fixtureMapID: fixtureMapID,
                                                                                        betFairUpdated: arbBetfairLastUpdated, oddsCheckerUpdated: arbOddscheckerLastUpdated);

            // Act

            _testClass.UpdateArb(arbRecord);

            // Assert

            _mockArbFinder.Verify(af => af.CheckLatestOddsForArbs(It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void UpdateArb_BetfairOddsUpdatedSinceArbLastUpdated_ArbUpdated()
        {
            // Arrange

            var fixtureMapID = 1;
            var betfairFixtureID = 2;
            var oddscheckerFixtureID = 3;

            var arbBetfairLastUpdated = DateTime.Now.AddDays(-3);
            var arbOddscheckerLastUpdated = DateTime.Now.AddDays(-3);

            var betfairOddsLastUpdated = DateTime.Now.AddDays(-1);

            _mockRepository.Setup(r => r.GetBetfairFixtureIdForArb(fixtureMapID)).Returns(betfairFixtureID);
            _mockRepository.Setup(r => r.GetDateTimeBetfairFootballOddsLastUpdated(betfairFixtureID)).Returns(betfairOddsLastUpdated);

            _mockRepository.Setup(r => r.GetOddscheckerFixtureIdForArb(fixtureMapID)).Returns(oddscheckerFixtureID);
            _mockRepository.Setup(r => r.GetDateTimeOddscheckerFootballOddsLastUpdated(oddscheckerFixtureID)).Returns(null);

            var arbRecord = ModelHelpers.Arbs_Football_MatchWinner_Helper.CreateRecord(id: 1, expired: false, fixtureMapID: fixtureMapID, 
                                                                                        betFairUpdated: arbBetfairLastUpdated, oddsCheckerUpdated: arbOddscheckerLastUpdated);

            // Act

            _testClass.UpdateArb(arbRecord);

            // Assert

            _mockArbFinder.Verify(af => af.CheckLatestOddsForArbs(fixtureMapID), Times.Once);
        }

        [TestMethod]
        public void UpdateArb_OddscheckerOddsUpdatedSinceArbLastUpdated_ArbUpdated()
        {
            // Arrange

            var fixtureMapID = 1;
            var betfairFixtureID = 2;
            var oddscheckerFixtureID = 3;

            var arbBetfairLastUpdated = DateTime.Now.AddDays(-3);
            var arbOddscheckerLastUpdated = DateTime.Now.AddDays(-3);

            var oddscheckerOddsLastUpdated = DateTime.Now.AddDays(-1);

            _mockRepository.Setup(r => r.GetBetfairFixtureIdForArb(fixtureMapID)).Returns(betfairFixtureID);
            _mockRepository.Setup(r => r.GetDateTimeBetfairFootballOddsLastUpdated(betfairFixtureID)).Returns(null);

            _mockRepository.Setup(r => r.GetOddscheckerFixtureIdForArb(fixtureMapID)).Returns(oddscheckerFixtureID);
            _mockRepository.Setup(r => r.GetDateTimeOddscheckerFootballOddsLastUpdated(oddscheckerFixtureID)).Returns(oddscheckerOddsLastUpdated);

            var arbRecord = ModelHelpers.Arbs_Football_MatchWinner_Helper.CreateRecord(id: 1, expired: false, fixtureMapID: fixtureMapID,
                                                                                        betFairUpdated: arbBetfairLastUpdated, oddsCheckerUpdated: arbOddscheckerLastUpdated);

            // Act

            _testClass.UpdateArb(arbRecord);

            // Assert

            _mockArbFinder.Verify(af => af.CheckLatestOddsForArbs(fixtureMapID), Times.Once);
        }
    }
}
