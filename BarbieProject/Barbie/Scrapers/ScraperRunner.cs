using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scrapers
{
    public static class ScraperRunner
    {
        /// <summary>
        /// Pass each command to the node script to scrape odds from site
        /// </summary>
        /// <param name="processCommands"></param>
        public static void CallNodeScripts(List<string> processCommands, string scraperFileHomeDir)
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
    }
}
