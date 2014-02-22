using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Barbie
{
    class Program
    {
        static void Main(string[] args)
        {
            FootballArbFinder fab = new FootballArbFinder();

            var watch = Stopwatch.StartNew();

            fab.FindArbs();

            watch.Stop();

            var time = watch.ElapsedMilliseconds / 1000;

            Console.WriteLine(time);
            Console.ReadLine();
        }
    }
}
