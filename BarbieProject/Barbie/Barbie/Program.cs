using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Scrapers.Football;

namespace Barbie
{
    class Program
    {
        static void Main(string[] args)
        {
            //var ocScraper = new OCFixturesScraper();
            //ocScraper.ScrapeFixturesForCompetition(1);

            //var ocMatchWinnerOddsScraper = new OCMatchWinnerOddsScraper();
            //ocMatchWinnerOddsScraper.ScrapeOddsForUnexpiredFixtures();

            var bfFixturesScraper = new BFFixturesScraper();
            bfFixturesScraper.ScrapeAllFixtures();

            //FootballArbFinder fab = new FootballArbFinder();

            //var watch = Stopwatch.StartNew();

            //fab.FindArbs();

            //watch.Stop();

            //var time = watch.ElapsedMilliseconds / 1000;

            //Console.WriteLine(time);
            //Console.ReadLine();
        }
    }
}
