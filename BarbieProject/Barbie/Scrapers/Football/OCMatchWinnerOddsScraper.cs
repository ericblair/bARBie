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
    public class OCMatchWinnerOddsScraper
    {
        bARBieEntities barbieEntity;

        string scraperFileHomeDir = ConfigurationManager.AppSettings["OddsCheckerScriptsDir"];
        string scraperFileName = ConfigurationManager.AppSettings["OddsCheckerMatchOddsScraperScript"];
        string logFileName = ConfigurationManager.AppSettings["OddsCheckerMatchOddsScraperLog"];

        public OCMatchWinnerOddsScraper()
        {
            barbieEntity = new bARBieEntities();
        }

        /// <summary>
        /// Scrape odds for all fixtures in OddsCheckerFootballFixtures table
        /// </summary>
        public void ScrapeAllOdds()
        {
            var fixtures = barbieEntity.OddsCheckerFootballFixtures
                            .OrderBy(x => x.MatchDateTime)
                            .ToList();

            ScrapeOdds(fixtures);
        }

        /// <summary>
        /// Scrape odds for all fixtures which have not already been played
        /// </summary>
        public void ScrapeOddsForUnexpiredFixtures()
        {
            int matchExpiryLimitMins;

            if (!Int32.TryParse(ConfigurationManager.AppSettings["MatchExpiryLimitMins"], out matchExpiryLimitMins))
            {
                // log error 
                return;
            }

            var matchExpiryDateTime = DateTime.Now.AddMinutes(-matchExpiryLimitMins);

            var fixtures = barbieEntity.OddsCheckerFootballFixtures
                            .Where(x => x.MatchDateTime >= matchExpiryDateTime)
                            .OrderBy(x => x.MatchDateTime)
                            .ToList();

            ScrapeOdds(fixtures);
        }

        /// <summary>
        /// Scrape all odds being played before the datetime provided
        /// </summary>
        /// <param name="limit"></param>
        public void ScrapeOddsBefore(DateTime limit)
        {
            var fixtures = barbieEntity.OddsCheckerFootballFixtures
                            .Where(x => x.MatchDateTime <= limit)
                            .OrderBy(x => x.MatchDateTime)
                            .ToList();

            ScrapeOdds(fixtures);
        }

        /// <summary>
        /// Scrape all odds being played after the datetime provided
        /// </summary>
        /// <param name="limit"></param>
        public void ScrapeOddsAfter(DateTime limit)
        {
            var fixtures = barbieEntity.OddsCheckerFootballFixtures
                            .Where(x => x.MatchDateTime >= limit)
                            .OrderBy(x => x.MatchDateTime)
                            .ToList();

            ScrapeOdds(fixtures);
        }

        /// <summary>
        /// Scrape all odds being played between the datetimes provided
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void ScrapeOddsBetween(DateTime start, DateTime end)
        {
            var fixtures = barbieEntity.OddsCheckerFootballFixtures
                            .Where(x => x.MatchDateTime >= start)
                            .Where(x=> x.MatchDateTime <= end)
                            .OrderBy(x => x.MatchDateTime)
                            .ToList();

            ScrapeOdds(fixtures);
        }

        /// <summary>
        /// Scrape odds for fixture matching ID provided
        /// </summary>
        /// <param name="fixtureId">OddsCheckerFootballFixtures.ID</param>
        public void ScrapeOddsForFixture(int fixtureId)
        {
            var fixtures = barbieEntity.OddsCheckerFootballFixtures
                            .Where(x => x.ID == fixtureId)
                            .ToList();

            ScrapeOdds(fixtures);
        }

        /// <summary>
        /// Scrape odds for fixtures in the competition matching the ID provided
        /// </summary>
        /// <param name="competitionId">FootballCompetitions.ID</param>
        public void ScrapeOddsForCompetition(int competitionId)
        {
            var fixtures = barbieEntity.OddsCheckerFootballFixtures
                            .Where(x => x.CompetitionID == competitionId)
                            .ToList();

            ScrapeOdds(fixtures);
        }

        /// <summary>
        /// Create a command for each fixture passed in and then 
        /// pass the commands to a helper to run them
        /// </summary>
        /// <param name="fixtures"></param>
        private void ScrapeOdds(List<OddsCheckerFootballFixtures> fixtures)
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
                                                        OddsCheckerFootballFixtures fixture)
        {
            string countryIdString = fixture.CountryID.HasValue ? fixture.CountryID.Value.ToString() : "NULL";

            var nodeInputString = "node " + scraperFileName + " " + fixture.ID
                                    + " " + countryIdString + " " + fixture.CompetitionID + @" """ + fixture.HomeTeam
                                    + @""" """ + fixture.AwayTeam + @""" " + fixture.MatchWinnerOddsUrl + " >> " + logFileName;

            return nodeInputString;
        }
    }
}
