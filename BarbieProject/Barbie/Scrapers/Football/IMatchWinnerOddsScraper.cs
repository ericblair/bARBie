using System;
namespace Scrapers.Football
{
    public interface IMatchWinnerOddsScraper
    {
        void ScrapeAllOdds();
        void ScrapeOddsAfter(DateTime limit);
        void ScrapeOddsBefore(DateTime limit);
        void ScrapeOddsBetween(DateTime start, DateTime end);
        void ScrapeOddsForCompetition(int competitionId);
        void ScrapeOddsForFixture(int fixtureId);
        void ScrapeOddsForUnexpiredFixtures();
    }
}
