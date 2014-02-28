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

        Process process;

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        public OCMatchWinnerOddsScraper()
        {
            barbieEntity = new bARBieEntities();
            process = new Process { StartInfo = startInfo };
        }

        public void ScrapeAllOdds()
        {
            var fixtures = barbieEntity.OddsCheckerFootballFixtures;

            foreach (var fixture in fixtures)
            {

                CallMatchWinnerOddsScraper(process, fixture.ID, fixture.CountryID, fixture.CompetitionID,
                                        fixture.HomeTeam, fixture.AwayTeam, fixture.MatchWinnerOddsUrl);


                    

            }

            //process.StandardInput.WriteLine("exit");
            //process.WaitForExit();
        }

        //private void CallMatchWinnerOddsScraper(Process process, int fixtureId, int? countryId, int competitionId,
        //                                        string homeTeam, string awayTeam, string matchWinnerOddsUrl)
        //{
        //    var scraperFileHomeDir = @"C:\bARBie\bARBie\ScrapingScripts\OddsChecker";
        //    var scraperFileName = "ocScrapeFootballMatchWinnerOdds.js";
        //    var logFileName = "ocScrapeFootballMatchWinnerOdds.log";

        //    process.Start();
        //    process.StandardInput.WriteLine(@"cd " + scraperFileHomeDir);

        //    string countryIdString = countryId.HasValue ? countryId.Value.ToString() : "NULL";

        //    var nodeInputString = "node " + scraperFileName + " " + fixtureId + " " + countryIdString
        //                            + " " + competitionId + " " + homeTeam + " " + awayTeam
        //                            + " " + matchWinnerOddsUrl + " >> " + logFileName;

        //    process.StandardInput.WriteLine(nodeInputString);

        //    process.StandardInput.WriteLine("exit");
        //    process.WaitForExit();
        //}

        

        private void CallMatchWinnerOddsScraper(Process process, int fixtureId, int? countryId, int competitionId,
                                                string homeTeam, string awayTeam, string matchWinnerOddsUrl)
        {
            //ProcessStartInfo startInfo = new ProcessStartInfo
            //{
            //    FileName = "cmd.exe",
            //    RedirectStandardInput = true,
            //    RedirectStandardOutput = true,
            //    UseShellExecute = false,
            //    CreateNoWindow = true
            //};

            //var process = new Process { StartInfo = startInfo };

            var scraperFileHomeDir = @"C:\bARBie\bARBie\ScrapingScripts\OddsChecker";
            var scraperFileName = "ocScrapeFootballMatchWinnerOdds.js";
            var logFileName = "ocScrapeFootballMatchWinnerOdds.log";

            process.Start();
            process.StandardInput.WriteLine(@"cd " + scraperFileHomeDir);

            string countryIdString = countryId.HasValue ? countryId.Value.ToString() : "NULL";

            var nodeInputString = "node " + scraperFileName + " " + fixtureId + " " + countryIdString
                                    + " " + competitionId + " " + homeTeam + " " + awayTeam
                                    + " " + matchWinnerOddsUrl + " >> " + logFileName;

            process.StandardInput.WriteLine(nodeInputString);

            process.StandardInput.WriteLine("exit");
            process.WaitForExit();
            //await process.WaitForExitAsync();
        }

        /// <summary>
        /// Waits asynchronously for the process to exit.
        /// </summary>
        /// <param name="process">The process to wait for cancellation.</param>
        /// <param name="cancellationToken">A cancellation token. If invoked, the task will return 
        /// immediately as cancelled.</param>
        /// <returns>A Task representing waiting for the process to end.</returns>
        //public static Task WaitForExitAsync(this Process process,
        //    CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    var tcs = new TaskCompletionSource<object>();
        //    process.EnableRaisingEvents = true;
        //    process.Exited += (sender, args) => tcs.SetResult(null);
        //    if (cancellationToken != default(CancellationToken))
        //        cancellationToken.Register(tcs.SetCanceled);

        //    return tcs.Task;
        //}
    }


}
