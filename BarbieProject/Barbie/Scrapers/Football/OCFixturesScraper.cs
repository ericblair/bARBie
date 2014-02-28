using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace Scrapers.Football
{
    public class OCFixturesScraper
    {
        bARBieEntities barbieEntity;

        Process process;

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
                        
        public OCFixturesScraper()
        {
            barbieEntity = new bARBieEntities();
            process = new Process { StartInfo = startInfo };
        }

        // Scrape all fixtures for country
        // Scrape all fixtures for competetion

        public void ScrapeAllFixtures()
        {
            var competitions = barbieEntity.OddsCheckerCompetitionUrls;
            
            foreach (var competition in competitions)
            {
                CallFixtureScraper(process, competition.CountryID, competition.CompetitionID, competition.Url);
            }
        }

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
