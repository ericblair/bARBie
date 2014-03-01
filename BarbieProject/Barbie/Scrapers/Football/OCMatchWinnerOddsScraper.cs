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

        // scrape odds for specified match

        /// <summary>
        /// Scrape odds for fixture matching ID provided
        /// </summary>
        /// <param name="fixtureId"></param>
        public void ScrapeOddsForFixture(int fixtureId)
        {
            var fixtures = barbieEntity.OddsCheckerFootballFixtures
                            .Where(x => x.ID == fixtureId)
                            .ToList();

            ScrapeOdds(fixtures);
        }

        // scrape odds for competition / country


        private void ScrapeOdds(List<OddsCheckerFootballFixtures> fixtures)
        {
            var processCommands = new List<string>();

            foreach (var fixture in fixtures)
            {
                var cmd = BuildScraperCommandPromptString(scraperFileName, logFileName, fixture);

                processCommands.Add(cmd);
            }

            CallNodeScripts(processCommands);
        }

        private void CallNodeScripts(List<string> processCommands)
        {
            var allProcesses = Task.Factory.StartNew(() =>
            {
                var processes = processCommands.Select(cmd =>
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo
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
                    process.StandardInput.WriteLine(cmd);
                    process.StandardInput.WriteLine("exit");
                    process.WaitForExit();

                    return process;
                }).ToArray();

                foreach (var process in processes)
                {
                    process.WaitForExit();
                    process.Dispose();
                }
            });

            allProcesses.Wait();
        }

        private string BuildScraperCommandPromptString(string scraperFileName, string logFileName, 
                                                        OddsCheckerFootballFixtures fixture)
        {
            string countryIdString = fixture.CountryID.HasValue ? fixture.CountryID.Value.ToString() : "NULL";

            var nodeInputString = "node " + scraperFileName + " " + fixture.ID
                                    + " " + countryIdString + " " + fixture.CompetitionID + " " + fixture.HomeTeam
                                    + " " + fixture.AwayTeam + " " + fixture.MatchWinnerOddsUrl + " >> " + logFileName;

            return nodeInputString;
        }
    }
}
