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
        bARBieEntities barbieEntity = new bARBieEntities();
        // Public methods:
        // Scrape all fixtures
        public void ScrapeAllFixtures()
        {
            // Get all competition urls from database
            var competitions = barbieEntity.OddsCheckerCompetitionUrls;

            foreach (var competition in competitions)
            {
                CallFixtureScraper(competition.CountryID, competition.CompetitionID, competition.Url);
            }

            // foreach competition url,
            // call node to run the oc script, passing in url to scrape fixtures from
        }
        // Scrape all fixtures for country
        // Scrape all fixtures for competetion

        private void CallFixtureScraper(int? countryId, int competitionId, string competitionUrl)
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
            
            var startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = new Process { StartInfo = startInfo };

            process.Start();
            process.StandardInput.WriteLine(@"cd " + scraperFileHomeDir);
            process.StandardInput.WriteLine(@"node {0} {1] {2} {3} >> {4}", scraperFileName, 
                                            countryId.HasValue ? countryId.Value.ToString() : "NULL", 
                                            competitionId, competitionUrl, logFileName);
            


        }

    }
}
