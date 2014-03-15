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
    public class OCMatchWinnerOddsScraper : IMatchWinnerOddsScraper
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
            var processes = new List<Process>();

            foreach (var fixture in fixtures)
            {
                var process = new Process();
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.FileName = "C:\\Program Files\\nodejs\\node.exe";
                process.StartInfo.WorkingDirectory = scraperFileHomeDir;
                process.StartInfo.Arguments = String.Format("{0} \"{1}\" \"{2}\" \"{3}\" \"{4}\" \"{5}\" \"{6}\" >> {7}",
                                                scraperFileName, fixture.ID.ToString(), fixture.CountryID.ToString(),
                                                fixture.CompetitionID.ToString(), fixture.HomeTeam, fixture.AwayTeam,
                                                fixture.MatchWinnerOddsUrl, logFileName);

                processes.Add(process);
            }

            var task = Task.Factory.StartNew(() =>
            {
                Parallel.ForEach(processes, process =>
                {
                    process.Start();
                });
            });

            task.Wait();
        }
    }
}
