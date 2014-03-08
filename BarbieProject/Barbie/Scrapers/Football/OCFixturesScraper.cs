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
        }

        /// <summary>
        /// Scrape the fixtures for all competitions 
        /// </summary>
        public void ScrapeAllFixtures()
        {
            var competitions = barbieEntity.OddsCheckerCompetitionUrls.ToList();

            ScrapeFixtures(competitions);
        }

        /// <summary>
        /// Scrape the fixtures for specified competition
        /// </summary>
        /// <param name="competitionId">FootballCompetitions.ID</param>
        public void ScrapeFixturesForCompetition(int competitionId)
        {
            var competitions = barbieEntity.OddsCheckerCompetitionUrls
                                .Where(x => x.CompetitionID == competitionId)
                                .ToList();

            ScrapeFixtures(competitions);
        }

        private void ScrapeFixtures(List<OddsCheckerCompetitionUrls> competitions)
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
                                });
                        });
        }

    }
}
