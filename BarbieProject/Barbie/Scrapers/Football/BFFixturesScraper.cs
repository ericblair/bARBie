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

        /// <summary>
        /// Scrape the fixtures for specified competition
        /// </summary>
        /// <param name="competitionId">FootballCompetitions.ID</param>
        public void ScrapeFixturesForCompetition(int competitionId)
        {
            var competitions = barbieEntity.BetFairCompetitionUrls
                                .Where(x => x.CompetitionID == competitionId)
                                .ToList();

            ScrapeFixtures(competitions);
        }

        private void ScrapeFixtures(List<BetFairCompetitionUrls> competitions)
        {
            var processes = new List<Process>();

            foreach (var competition in competitions)
            {
                var process = new Process();
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.FileName = "node.exe";
                process.StartInfo.WorkingDirectory = scraperFileHomeDir;
                process.StartInfo.Arguments = String.Format("{0} \"{1}\" \"{2}\" \"{3}\" >> {4}",
                                                scraperFileName, competition.CountryID.ToString(),
                                                competition.CompetitionID.ToString(), competition.Url,
                                                logFileName);

                processes.Add(process);
            }

            var task = Task.Factory.StartNew(() =>
            {
                Parallel.ForEach(processes, process =>
                {
                    process.Start();
                    var output = process.StandardOutput.ReadToEnd();
                    var error = process.StandardError.ReadToEnd();
                    Console.WriteLine("output: " + output);
                    Console.WriteLine("error: " + error);
                });
            });
        }
    }
}
