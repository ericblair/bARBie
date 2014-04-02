using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Scrapers.Football;
using Barbie.FixtureMappers;
using Barbie.ArbFinders;
using Barbie.DbMaintenance;

namespace Barbie
{
    class Program
    {
        static void Main(string[] args)
        {
            //scrapeOCFixtures();

            //scrapeBFFixtures();

            //runMapper();

            //scrapeOCOdds();

            //scrapeBFOdds();

            runArbFinder();

            //var archiver = new ArchiveOdds_MatchWinner_BF_OC();
            //archiver.ArchiveExpiredOdds();

            //var deleteExpiredRecords = new DeleteExpiredData();
            //deleteExpiredRecords.Run();
        }

        private static void scrapeOCFixtures()
        {
            var ocScraper = new OCFixturesScraper();
            ocScraper.ScrapeAllFixtures();
        }

        private static void scrapeBFFixtures()
        {
            var bfScraper = new BFFixturesScraper();
            bfScraper.ScrapeAllFixtures();
        }

        private static void scrapeOCOdds()
        {
            var ocMatchWinnerOddsScraper = new OCMatchWinnerOddsScraper();
            //ocMatchWinnerOddsScraper.ScrapeOddsForUnexpiredFixtures();

            var startDate = DateTime.Now.AddHours(-3);
            var endDate = DateTime.Now.AddDays(5);

            ocMatchWinnerOddsScraper.ScrapeOddsBetween(startDate, endDate);
        }

        private static void scrapeBFOdds()
        {
            var bfMatchWinnerOddsScraper = new BFMatchWinnerOddsScraper();
            //bfMatchWinnerOddsScraper.ScrapeOddsForUnexpiredFixtures();

            var startDate = DateTime.Now.AddHours(-3);
            var endDate = DateTime.Now.AddDays(5);

            bfMatchWinnerOddsScraper.ScrapeOddsBetween(startDate, endDate);
        }

        private static void runMapper()
        {
            //var mapper = new FixtureMappers.MatchWinner_BF_OC();
            //mapper.RunMapper();
        }

        private static void runArbFinder()
        {
            //var arbFinder = new ArbFinders.MatchWinner_BF_OC();
            
            //arbFinder.SetArbsExpiredForFinishedMatches();
            //arbFinder.CheckAllUnexpiredMappedFixtures();
            //arbFinder.SetArbsExpiredForFinishedMatches();
        }
    }
}
