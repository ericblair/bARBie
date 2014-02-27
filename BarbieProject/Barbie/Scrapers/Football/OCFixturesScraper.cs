using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace Scrapers
{
    public class OCFixturesScraper
    {
        bARBieEntities barbieEntity;
        
        
        public OCFixturesScraper()
        {
            barbieEntity = new bARBieEntities();
        }
        
        public void ScrapeAllFixtures()
        {
            barbieEntity = new bARBieEntities();

            var competitions = barbieEntity.OddsCheckerCompetitionUrls;

            var startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = new Process { StartInfo = startInfo };

            foreach (var competition in competitions)
            {
                CallFixtureScraper(process, competition.CountryID, competition.CompetitionID, competition.Url);
            }

        }

        // Scrape all fixtures for country
        // Scrape all fixtures for competetion

        private void CallFixtureScraper(Process process, int? countryId, int competitionId, string competitionUrl)
        {
            // Thoughts: 
            // By simply omitting the following lines:
            //process.StandardInput.WriteLine("exit");
            //process.WaitForExit();
            // from the end of this method, it becomes asynch in the sense that the method does
            // not wait for the node process to complete before returning.

            // TODO: Load location of scraper script, name of file and log file from config file
            var scraperFileHomeDir = @"C:\bARBie\bARBie\ScrapingScripts\OddsChecker";
            var scraperFileName = "ocScrapeFootballFixtures.js";
            var logFileName = "ocScrapeFootballFixtures.log";

            process.Start();
            process.StandardInput.WriteLine(@"cd " + scraperFileHomeDir);

            string countryIdString = countryId.HasValue ? countryId.Value.ToString() : "NULL";

            var nodeInputString = "node " + scraperFileName + " " + countryIdString + " " + competitionId
                                     + " " + competitionUrl + " >> " + logFileName;

            process.StandardInput.WriteLine(nodeInputString);

            process.StandardInput.WriteLine("exit");
            process.WaitForExit();

        }

    }
}
