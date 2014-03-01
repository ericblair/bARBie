using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace Scrapers.Football
{
    public class BFFixturesScraper
    {
        bARBieEntities barbieEntity;

        string scraperFileHomeDir = ConfigurationManager.AppSettings["BetFairScriptsDir"];
        string scraperFileName = ConfigurationManager.AppSettings["BetFairFixturesScraperScript"];
        string logFileName = ConfigurationManager.AppSettings["BetFairFixturesScraperLog"];

        public BFFixturesScraper()
        {
            barbieEntity = new bARBieEntities();
        }

        /// <summary>
        /// Scrape the fixtures for all competitions 
        /// </summary>
        public void ScrapeAllFixtures()
        {
            var competitions = barbieEntity.BetFairCompetitionUrls.ToList();

            ScrapeFixtures(competitions);
        }

        private void ScrapeFixtures(List<BetFairCompetitionUrls> competitions)
        {
            var processCommands = new List<string>();

            foreach (var competition in competitions)
            {
                var cmd = BuildScraperCommandPromptString(scraperFileName, logFileName, competition);

                processCommands.Add(cmd);
            }

            ScraperRunner.CallNodeScripts(processCommands, scraperFileHomeDir);
        }

        private string BuildScraperCommandPromptString(string scraperFileName, string logFileName,
                                                        BetFairCompetitionUrls competition)
        {
            string countryIdString = competition.CountryID.HasValue ? competition.CountryID.Value.ToString() : "NULL";

            var nodeInputString = "node " + scraperFileName + " " + countryIdString + " " + competition.ID
                                     + " " + competition.Url + " >> " + logFileName;

            return nodeInputString;
        }
    }
}
