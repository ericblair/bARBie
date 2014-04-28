using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DAL;
using Barbie.Arbs;

namespace Barbie.Tests.ArbFinders.MatchWinner_BF_OC_Controller_Tests
{
    /// <summary>
    /// Tests for Barbie.ArbFinders.MatchWinner_BF_OC_Controller_.ExpireArbsForFinishedMatches() method
    /// </summary>
    [TestClass]
    public class ExpireArbsForFinishedMatchesTests
    {
        Mock<bARBieEntities> _mockContext;
        Mock<IRepository> _mockRepository;
        Mock<IConfigHelper> _mockConfigHelper;
        Mock<IArbFinder> _mockArbFinder;
        MatchWinner_BF_OC_Controller _testClass;

        // Create mock objects for the tables used in test
        FakeDbSet<Arbs_Football_MatchWinner> _mockArbsFootballMatchWinnerTable;

        int _maxMatchTimeMins = 200;

        [TestInitialize]
        public void Initialize()
        {
            _mockContext = new Mock<bARBieEntities>();
            _mockRepository = new Mock<IRepository>();
            _mockConfigHelper = new Mock<IConfigHelper>();
            _mockArbFinder = new Mock<IArbFinder>();

            _mockConfigHelper.Setup(m => m.MaxTotalMatchTimeMins()).Returns(_maxMatchTimeMins);

            _testClass = new MatchWinner_BF_OC_Controller(_mockContext.Object, _mockRepository.Object, _mockConfigHelper.Object, _mockArbFinder.Object);

            _mockArbsFootballMatchWinnerTable = new FakeDbSet<Arbs_Football_MatchWinner>();

            // Set mockContext tables to FakeDbSet objects
            _mockContext.Object.Arbs_Football_MatchWinner = _mockArbsFootballMatchWinnerTable;
        }

        [TestMethod]
        public void ExpireArbsForFinishedMatches_NoExpiredArbs_NothingUpdated()
        {
            // Arrange

            var unexpiredRecord = ModelHelpers.Arbs_Football_MatchWinner_Helper.CreateRecord(id: 1, expired: null, matchDateTime: DateTime.Now);
            _mockArbsFootballMatchWinnerTable.Add(unexpiredRecord);

            // Act

            _testClass.ExpireArbsForFinishedMatches();

            // Assert

            Assert.IsNull(_mockContext.Object.Arbs_Football_MatchWinner.ElementAt(0).Expired);
            Assert.IsNull(_mockContext.Object.Arbs_Football_MatchWinner.ElementAt(0).Updated);
        }

        [TestMethod]
        public void ExpireArbsForFinishedMatches_DoesntUpdatedArbsAlreadyMarkedAsExpired()
        {
            // Arrange

            var expiredRecord = ModelHelpers.Arbs_Football_MatchWinner_Helper.CreateRecord(id: 1, expired: true, matchDateTime: DateTime.Now.AddMinutes(-(_maxMatchTimeMins + 1)));
            _mockArbsFootballMatchWinnerTable.Add(expiredRecord);

            // Act

            _testClass.ExpireArbsForFinishedMatches();

            // Assert

            Assert.IsNull(_mockContext.Object.Arbs_Football_MatchWinner.ElementAt(0).Updated);
        }

        [TestMethod]
        public void ExpiredArbsForFinishedMatches_UpdatesExpiredArb()
        {
            // Arrange

            var expiredRecord = ModelHelpers.Arbs_Football_MatchWinner_Helper.CreateRecord(id: 1, expired: null, matchDateTime: DateTime.Now.AddMinutes(-(_maxMatchTimeMins + 1)));
            _mockArbsFootballMatchWinnerTable.Add(expiredRecord);

            // Act

            _testClass.ExpireArbsForFinishedMatches();

            // Assert

            Assert.IsTrue(_mockContext.Object.Arbs_Football_MatchWinner.ElementAt(0).Expired.Value);
            Assert.IsNotNull(_mockContext.Object.Arbs_Football_MatchWinner.ElementAt(0).Updated);
        }

        [TestMethod]
        public void ExpiredArbsForFinishedMatches_UpdatesOnlyExpiredArbs()
        {
            // Arrange

            var unexpiredRecord = ModelHelpers.Arbs_Football_MatchWinner_Helper.CreateRecord(id: 1, expired: null, matchDateTime: DateTime.Now.AddMinutes(-(_maxMatchTimeMins - 10)));
            var expiredRecord = ModelHelpers.Arbs_Football_MatchWinner_Helper.CreateRecord(id: 2, expired: null, matchDateTime: DateTime.Now.AddMinutes(-(_maxMatchTimeMins + 10)));

            _mockArbsFootballMatchWinnerTable.Add(unexpiredRecord);
            _mockArbsFootballMatchWinnerTable.Add(expiredRecord);

            // Act

            _testClass.ExpireArbsForFinishedMatches();

            // Assert

            Assert.IsNull(_mockContext.Object.Arbs_Football_MatchWinner.First(x => x.ID == 1).Expired);
            Assert.IsTrue(_mockContext.Object.Arbs_Football_MatchWinner.First(x => x.ID == 2).Expired.Value);
        }
    }
}
