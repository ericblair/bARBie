using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DAL;
using Scrapers;

namespace Scrapers.Football
{
    public class BFMatchWinnerOddsScraper
    {
        bARBieEntities barbieEntity;

        string scraperFileHomeDir = ConfigurationManager.AppSettings["BetFairScriptsDir"];
        string scraperFileName = ConfigurationManager.AppSettings["BetFairMatchOddsScraperScript"];
        string logFileName = ConfigurationManager.AppSettings["BetFairMatchOddsScraperLog"];

        public BFMatchWinnerOddsScraper()
        {
            barbieEntity = new bARBieEntities();
        }

        /// <summary>
        /// Scrape odds for all fixtures in BetFairFootballFixtures table
        /// </summary>
        public void ScrapeAllOdds()
        {
            var fixtures = barbieEntity.BetFairFootballFixtures
                            .OrderBy(x => x.MatchDateTime)
                            .ToList();

            ScrapeOdds(fixtures);
        }

        /// <summary>
        /// Create a command for each fixture passed in and then 
        /// pass the commands to a helper to run them
        /// </summary>
        /// <param name="fixtures"></param>
        private void ScrapeOdds(List<BetFairFootballFixtures> fixtures)
        {
            var processCommands = new List<string>();

            foreach (var fixture in fixtures)
            {
                var cmd = BuildScraperCommandPromptString(scraperFileName, logFileName, fixture);

                processCommands.Add(cmd);
            }

            ScraperRunner.CallNodeScripts(processCommands, scraperFileHomeDir);
        }

        /// <summary>
        /// Build the command string that will be passed to the Process
        /// </summary>
        /// <param name="scraperFileName"></param>
        /// <param name="logFileName"></param>
        /// <param name="fixture"></param>
        /// <returns></returns>
        private string BuildScraperCommandPromptString(string scraperFileName, string logFileName,
                                                        BetFairFootballFixtures fixture)
        {
            string countryIdString = fixture.CountryID.HasValue ? fixture.CountryID.Value.ToString() : "NULL";

            var nodeInputString = "node " + scraperFileName + " " + fixture.ID
                                    + " " + countryIdString + " " + fixture.CompetitionID + @" """ + fixture.HomeTeam
                                    + @""" """ + fixture.AwayTeam + @""" " + fixture.MatchWinnerOddsUrl + " >> " + logFileName;

            return nodeInputString;
        }
    }
}
