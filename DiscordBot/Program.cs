using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using Discord;
using DiscordBot.Common;
using DiscordBot.Database;
using DiscordBot.Extensions;
using MelissaNet;
using MelissaNet.Modules;

namespace DiscordBot
{
    public static class Program
    {
        private static readonly Version
            ProgramVersion =
                Assembly.GetExecutingAssembly().GetName()
                    .Version; // Gets the program version to compare with the update checker.

        public static void Main(string[] args) // Entry point to the program.
        {
            Console.Title = "DiscordBot v" + ProgramVersion.Major + @"." + ProgramVersion.Minor + @"." +
                            ProgramVersion.Build + @"." + ProgramVersion.Revision + " | Developed by MythicalCuddles";
            
            args = args.Select(s => s.ToUpperInvariant()).ToArray(); // Set everything in args to UPPERCASE.
            StartBot(args); // Run the StartBot method.
        }

        private static async void StartBot(string[] args) // Startup Method.
        {
            // Print Application Information to Console.
            Console.Write($@"{DateTime.Now,-19} DiscordBot: [");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(@"Version " + ProgramVersion.Major + @"." + ProgramVersion.Minor + @"." +
                          ProgramVersion.Build + @"." + ProgramVersion.Revision);
            Console.ResetColor();
            Console.WriteLine(@"]    ");

            // Print Developer Information to Console.
            Console.Write($@"{DateTime.Now,-19} DiscordBot: Developed by Melissa Brennan (");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(@"@MythicalCuddles");
            Console.ResetColor();
            Console.WriteLine(@")");

            // Print Additional Information to the Console.
            Console.WriteLine($@"{DateTime.Now,-19} DiscordBot: Web: www.mythicalcuddles.xyz");
            Console.WriteLine($@"{DateTime.Now,-19} DiscordBot: Copyright 2017 - 2019 Melissa Brennan | Licensed under the MIT License.");
            
            Methods.PrintConsoleSplitLine();

            // Run the Initializer for MelissaNET.
            MelissaNet.MelissaNet.Initialize();
            
            Methods.PrintConsoleSplitLine();

            // Check for Updates using MelissaNet.
            CheckForUpdates();

            // Check if application configurations exist.
            Configuration.EnsureExists();
            StringConfiguration.EnsureExists();

            // Verify Settings
            bool invalidToken = false, invalidDbSettings = false;
            if (!Configuration.Load().FirstTimeRun)
            {
                Methods.PrintConsoleSplitLine();
                
                invalidToken = !Methods.VerifyBotToken().GetAwaiter().GetResult();
                invalidDbSettings = !DatabaseActivity.TestDatabaseSettings().GetAwaiter().GetResult().connectionValid;
            }
            
            Methods.PrintConsoleSplitLine();
            
            // Check & Run the configurator form if the configuration has not been setup or the -config arg has been passed.
            bool configArg = args.Contains("-CONFIG");
            await new LogMessage(LogSeverity.Debug, "Configurator",
                "Checking if the Configurator needs to run...").PrintToConsole();
            await new LogMessage(LogSeverity.Debug, "Configurator",
                "configArg: " + configArg.ToYesNo()).PrintToConsole();
            await new LogMessage(LogSeverity.Debug, "Configurator",
                "FirstTimeRun: " + Configuration.Load().FirstTimeRun.ToYesNo()).PrintToConsole();
            await new LogMessage(LogSeverity.Debug, "Configurator",
                "invalidToken: " + invalidToken.ToYesNo()).PrintToConsole();
            await new LogMessage(LogSeverity.Debug, "Configurator",
                "invalidDbSettings: " + invalidDbSettings.ToYesNo()).PrintToConsole();
            FrmConfigure.CheckRunConfigurator(configArg || Configuration.Load().FirstTimeRun || invalidToken || invalidDbSettings);

            Methods.PrintConsoleSplitLine();
            
            // Check to see if the database and tables exist.
            DatabaseActivity.EnsureExists();

            // Start the bot.
            DiscordBot.RunBotAsync().GetAwaiter().GetResult();
        }

        private static async void CheckForUpdates() // Requires MelissaNet to check for the latest version info.
        {
            try
            {
                var (releaseVersion, releaseUrl) = Updater.CheckForNewVersion("DiscordBot");
                if (releaseVersion > ProgramVersion)
                {
                    var lm = new LogMessage(LogSeverity.Info, "MelissaNet",
                        "A new update has been found. Would you like to download?");
                    await lm.PrintToConsole();
                    Methods.PrintConsoleSplitLine();

                    var result = MessageBox.Show("A new update is available. Would you like to update?",
                        "DiscordBot Update Available", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (result != DialogResult.Yes) return;
                    Process.Start(releaseUrl);
                    Environment.Exit(0);
                }
                else
                {
                    var lm = new LogMessage(LogSeverity.Info, "MelissaNet",
                        "DiscordBot Version matched our released version.");
                    await lm.PrintToConsole();
                    Methods.PrintConsoleSplitLine();
                }
            }
            catch (Exception e)
            {
                Methods.PrintConsoleSplitLine();
                await new LogMessage(LogSeverity.Error, "MelissaNet", "Unable to check for new updates.")
                    .PrintToConsole();
                await new LogMessage(LogSeverity.Error, "MelissaNet", e.Message).PrintToConsole();
                Methods.PrintConsoleSplitLine();
            }
        }
    }
}