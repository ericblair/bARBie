using System;
using System.Collections.Generic;
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

        public OCMatchWinnerOddsScraper()
        {
            barbieEntity = new bARBieEntities();
        }

        public void ScrapeAllOdds()
        {
            var scraperFileHomeDir = @"C:\bARBie\bARBie\ScrapingScripts\OddsChecker";
            var scraperFileName = "ocScrapeFootballMatchWinnerOdds.js";
            var logFileName = "ocScrapeFootballMatchWinnerOdds.log";

            var fixtures = barbieEntity.OddsCheckerFootballFixtures;

            var processCommands = new List<string>();

            foreach (var fixture in fixtures)
            {
                var cmd = BuildScraperCommandPromptString
                                (
                                    scraperFileName, logFileName, fixture.ID,
                                    fixture.CountryID, fixture.CompetitionID, fixture.HomeTeam,
                                    fixture.AwayTeam, fixture.MatchWinnerOddsUrl
                                );

                processCommands.Add(cmd);
            }

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
                                                        int fixtureId, int? countryId, int competitionId,
                                                        string homeTeam, string awayTeam, string matchWinnerOddsUrl)
        {
            string countryIdString = countryId.HasValue ? countryId.Value.ToString() : "NULL";

            var nodeInputString = "node " + scraperFileName + " " + fixtureId 
                                    + " " + countryIdString + " " + competitionId + " " + homeTeam 
                                    + " " + awayTeam + " " + matchWinnerOddsUrl + " >> " + logFileName;

            return nodeInputString;
        }
    }


}
