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

            scrapeOCOdds();

            //scrapeBFOdds();

            //runArbFinder();

            //masterTimer.Stop();

            //var time = masterTimer.ElapsedMilliseconds / 1000;

            //Console.WriteLine(time);
            //Console.ReadLine();
        }

        private static void scrapeOCFixtures()
        {
            var ocScraperTimer = Stopwatch.StartNew();

            var ocScraper = new OCFixturesScraper();
            ocScraper.ScrapeAllFixtures();

            ocScraperTimer.Stop();
            var ocScraperTimerTime = ocScraperTimer.ElapsedMilliseconds / 1000;

            Console.WriteLine("ocScraperTimer: " + ocScraperTimerTime);
        }

        private static void scrapeBFFixtures()
        {
            var bfScraperTimer = Stopwatch.StartNew();

            var bfScraper = new BFFixturesScraper();
            bfScraper.ScrapeAllFixtures();

            bfScraperTimer.Stop();
            var bfScraperTimerTime = bfScraperTimer.ElapsedMilliseconds / 1000;

            Console.WriteLine("bfScraperTimer: " + bfScraperTimerTime);
        }

        private static void scrapeOCOdds()
        {
            var ocMatchWinnerOddsScraperTimer = Stopwatch.StartNew();

            var ocMatchWinnerOddsScraper = new OCMatchWinnerOddsScraper();
            ocMatchWinnerOddsScraper.ScrapeOddsForUnexpiredFixtures();

            ocMatchWinnerOddsScraperTimer.Stop();
            var ocMatchWinnerOddsScraperTimerTime = ocMatchWinnerOddsScraperTimer.ElapsedMilliseconds / 1000;

            Console.WriteLine(ocMatchWinnerOddsScraperTimerTime);
        }

        private static void scrapeBFOdds()
        {
            var bfMatchWinnerOddsScraperTimer = Stopwatch.StartNew();

            var bfMatchWinnerOddsScraper = new BFMatchWinnerOddsScraper();
            bfMatchWinnerOddsScraper.ScrapeOddsForUnexpiredFixtures();

            bfMatchWinnerOddsScraperTimer.Stop();
            var bfMatchWinnerOddsScraperTimerTime = bfMatchWinnerOddsScraperTimer.ElapsedMilliseconds / 1000;

            Console.WriteLine(bfMatchWinnerOddsScraperTimerTime);
        }

        private static void runMapper()
        {
            var mapperTimer = Stopwatch.StartNew();

            var mapper = new FixtureMappers.MatchWinner_BF_OC();
            mapper.RunMapper();

            mapperTimer.Stop();
            var mapperTimerTime = mapperTimer.ElapsedMilliseconds / 1000;

            Console.WriteLine(mapperTimerTime);
        }

        private static void runArbFinder()
        {
            var arbFinderTimer = Stopwatch.StartNew();

            var arbFinder = new ArbFinders.MatchWinner_BF_OC();
            arbFinder.CheckAllUnexpiredMappedFixtures();

            arbFinderTimer.Stop();
            var arbFinderTimerTime = arbFinderTimer.ElapsedMilliseconds / 1000;

            Console.WriteLine(arbFinderTimerTime);
        }
    }
}
