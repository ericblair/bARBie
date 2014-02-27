using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Scrapers;

namespace Barbie
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("hello");
            var ocScraper = new OCFixturesScraper();
            ocScraper.ScrapeAllFixtures();
            Console.WriteLine("Bye then");
            Console.ReadLine();
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
