using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Scrapers.Football;
using Barbie.FixtureMappers;
using Barbie.ArbFinders;

namespace Barbie
{
    class Program
    {
        static void Main(string[] args)
        {
            //var masterTimer = Stopwatch.StartNew();

            //scrapeOCFixtures();

            //scrapeBFFixtures();

            //runMapper();

            //scrapeOCOdds();

            //scrapeBFOdds();

            runArbFinder();

            //masterTimer.Stop();

            //var time = masterTimer.ElapsedMilliseconds / 1000;

            //Console.WriteLine(time);
            //Console.ReadLine();
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
            ocMatchWinnerOddsScraper.ScrapeOddsForUnexpiredFixtures();
        }

        private static void scrapeBFOdds()
        {
            var bfMatchWinnerOddsScraper = new BFMatchWinnerOddsScraper();
            bfMatchWinnerOddsScraper.ScrapeOddsForUnexpiredFixtures();
        }

        private static void runMapper()
        {
            var mapper = new FixtureMappers.MatchWinner_BF_OC();
            mapper.RunMapper();
        }

        private static void runArbFinder()
        {
            var arbFinder = new ArbFinders.MatchWinner_BF_OC();
            arbFinder.CheckAllUnexpiredMappedFixtures();
        }
    }
}
