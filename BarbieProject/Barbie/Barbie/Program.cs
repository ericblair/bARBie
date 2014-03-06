using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Scrapers.Football;
using Barbie.FixtureMappers;

namespace Barbie
{
    class Program
    {
        static void Main(string[] args)
        {
            //var ocScraper = new OCFixturesScraper();
            //ocScraper.ScrapeAllFixtures();

            //var ocMatchWinnerOddsScraper = new OCMatchWinnerOddsScraper();
            //ocMatchWinnerOddsScraper.ScrapeOddsForUnexpiredFixtures();

            //var bfFixturesScraper = new BFFixturesScraper();
            //bfFixturesScraper.ScrapeAllFixtures();

            //var bfMatchWinnerOddsScraper = new BFMatchWinnerOddsScraper();
            //bfMatchWinnerOddsScraper.ScrapeAllOdds();

            //FootballArbFinder fab = new FootballArbFinder();

            //var watch = Stopwatch.StartNew();

            //fab.FindArbs();

            //watch.Stop();

            //var time = watch.ElapsedMilliseconds / 1000;

            //Console.WriteLine(time);
            //Console.ReadLine();

            var mapper = new MatchWinner_BF_OC();
            mapper.RunMapper();
        }
    }
}
