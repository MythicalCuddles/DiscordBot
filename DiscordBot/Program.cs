using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

using Discord;
using DiscordBot.Common;
using DiscordBot.Database;
using DiscordBot.Extensions;
using DiscordBot.Other;

using MelissaNet.Modules;

namespace DiscordBot
{
    public static class Program
    {
        private static readonly Version
            ProgramVersion =
                Assembly.GetExecutingAssembly().GetName()
                    .Version; // Gets the program version to compare with the update checker.

        public static void Main() // Entry point to the program.
        {
            StartBot(); // Run the StartBot method.
        }

        private static void StartBot() // Startup Method.
        {
            // Print Application Information to Console.
            Console.Write(@"DiscordBot: [");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(@"Version " + ProgramVersion.Major + @"." + ProgramVersion.Minor + @"." +
                          ProgramVersion.Build + @"." + ProgramVersion.Revision);
            Console.ResetColor();
            Console.WriteLine(@"]    ");

            // Print Developer Information to Console.
            Console.Write(@"Developed by Melissa Brennan (");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(@"@MythicalCuddles");
            Console.ResetColor();
            Console.WriteLine(@")");

            // Print Additional Information to the Console.
            Console.WriteLine(@"Web: www.mythicalcuddles.xyz");
            Console.WriteLine(@"Copyright 2017 - 2019 Melissa Brennan | Licensed under the MIT License.");

            // Run the Initializer for MelissaNET.
            MelissaNet.MelissaNet.Initialize();

            // Check for Updates using MelissaNet.
            CheckForUpdates();

            // Check if application configurations exist.
            Configuration.EnsureExists();
            StringConfiguration.EnsureExists();
            QuoteHandler.EnsureExists();
            VoteLinkHandler.EnsureExists();
            
            AttemptDataBaseConnection(); // Attempt a connection to the Database.

            PrintConsoleSplitLine();

            DiscordBot.RunBotAsync().GetAwaiter().GetResult(); // Start the Bot.
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
                    PrintConsoleSplitLine();

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
                    PrintConsoleSplitLine();
                }
            }
            catch (Exception e)
            {
                PrintConsoleSplitLine();
                await new LogMessage(LogSeverity.Error, "MelissaNet", "Unable to check for new updates.")
                    .PrintToConsole();
                await new LogMessage(LogSeverity.Error, "MelissaNet", e.Message).PrintToConsole();
                PrintConsoleSplitLine();
            }
        }

        private static async void AttemptDataBaseConnection()
        {
            try
            {
                DatabaseActivity.CheckForDatabase();
            }
            catch (Exception e)
            {
                var lm = new LogMessage(LogSeverity.Critical, "MySQL Database",
                    "Unable to connect to database. Is it currently running?", e);
                await lm.PrintToConsole();
                Console.ReadLine();
                Environment.Exit(0);
            }
        }

        private static async void PrintConsoleSplitLine()
        {
            await new LogMessage(LogSeverity.Info, "",
                "-----------------------------------------------------------------").PrintToConsole();
        }
    }
}