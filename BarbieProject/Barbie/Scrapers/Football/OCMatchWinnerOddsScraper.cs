using System;
using System.Collections.Generic;
using System.Configuration;
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

        public void ScrapeAllOdds()
        {
            var fixtures = barbieEntity.OddsCheckerFootballFixtures;

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
