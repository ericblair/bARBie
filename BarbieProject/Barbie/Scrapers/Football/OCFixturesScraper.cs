using System;
using System.Collections.Generic;
using System.Configuration;
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

        string scraperFileHomeDir = ConfigurationManager.AppSettings["OddsCheckerScriptsDir"];
        string scraperFileName = ConfigurationManager.AppSettings["OddsCheckerFixturesScraperScript"];
        string logFileName = ConfigurationManager.AppSettings["OddsCheckerFixturesScraperLog"];

        public OCFixturesScraper()
        {
            barbieEntity = new bARBieEntities();
        }

        // Scrape all fixtures for country
        // Scrape all fixtures for competetion

        public void ScrapeAllFixtures()
        {
            var competitions = barbieEntity.OddsCheckerCompetitionUrls.ToList();

            ScrapeFixtures(competitions);
        }


        private void ScrapeFixtures(List<OddsCheckerCompetitionUrls> competitions)
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
                                                        OddsCheckerCompetitionUrls competition)
        {
            string countryIdString = competition.CountryID.HasValue ? competition.CountryID.Value.ToString() : "NULL";

            var nodeInputString = "node " + scraperFileName + " " + countryIdString + " " + competition.ID
                                     + " " + competition.Url + " >> " + logFileName;

            return nodeInputString;
        }

    }
}
