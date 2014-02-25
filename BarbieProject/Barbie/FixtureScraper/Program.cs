using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace FixtureScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = false
            };

            var process = new Process { StartInfo = startInfo };

            process.Start();
            //process.StandardInput.WriteLine(@"cd C:\bARBie\bARBie\ScrapingScripts\OddsChecker");
            //process.StandardInput.WriteLine(@"node ocFootball.js >> log.txt");
            //process.StandardInput.WriteLine("exit");
            //process.WaitForExit();

            process.StandardInput.WriteLine(@"cd C:\bARBie\bARBie\ScrapingScripts");
            process.StandardInput.WriteLine(@"node paramTest.js one two three >> paramTest.txt");

            Console.ReadLine();
        }


    }
}
