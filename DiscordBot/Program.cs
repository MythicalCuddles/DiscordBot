using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Discord;
using DiscordBot.Common;
using DiscordBot.Database;
using DiscordBot.Extensions;
using DiscordBot.Other;

using MythicalCore;
using MelissaNet;

namespace DiscordBot
{
    public class Program
    {
        private static readonly Version ProgramVersion = Assembly.GetExecutingAssembly().GetName().Version;

        public static void Main(string[] args)
        {
            StartBot();
        }

        public static void StartBot()
        {
            Console.Write(@"DiscordBot: [");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(@"Version " + ProgramVersion.Major + @"." + ProgramVersion.Minor + @"." + ProgramVersion.Build + @"." + ProgramVersion.Revision);
            Console.ResetColor();
            Console.WriteLine(@"]    ");

            Console.Write(@"Developed by Melissa Brennan (");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(@"@MythicalCuddles");
            Console.ResetColor();
            Console.WriteLine(@")");
            
            Console.WriteLine(@"Web: www.mythicalcuddles.xyz");
            
            Console.WriteLine(@"Copyright 2017 - 2019 Melissa Brennan | Licensed under the MIT License.");
            Console.WriteLine(@"-----------------------------------------------------------------");
            
            MelissaNet.MelissaNet.Initialize();

            /*    Update Checker via MythicalCore    */
            try
            {
                var updateCheck = Updater.CheckForUpdate("DiscordBot", ProgramVersion);
                if (updateCheck.Item1)
                {
                    Console.WriteLine(@"-----------------------------------------------------------------");
                    LogMessage lm = new LogMessage(LogSeverity.Info, "MythicalCore",
                        "A new update has been found. Would you like to download?");
                    lm.PrintToConsole();
                    Console.WriteLine(@"-----------------------------------------------------------------");

                    DialogResult result = MessageBox.Show("A new update is available. Would you like to update?",
                        "DiscordBot Update Available", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (result == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(updateCheck.Item2);
                        Environment.Exit(0);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(@"-----------------------------------------------------------------");
                LogMessage lm = new LogMessage(LogSeverity.Error, "MythicalCore",
                    "Unable to check for new updates.");
                lm.PrintToConsole();
                Console.WriteLine(@"-----------------------------------------------------------------");
            }
            /*    Update End    */
            
            Configuration.EnsureExists();
            StringConfiguration.EnsureExists();
            QuoteHandler.EnsureExists();
            VoteLinkHandler.EnsureExists();

            try
            {
                DatabaseActivity.CheckForDatabase();
            }
            catch (Exception e)
            {
                LogMessage lm = new LogMessage(LogSeverity.Critical, "MySQL Database", "Unable to connect to database. Is it currently running?", e);
                lm.PrintToConsole();
                Environment.Exit(0);
            }
           
            
            Console.WriteLine(@"-----------------------------------------------------------------");
            
            new DiscordBot().RunBotAsync().GetAwaiter().GetResult();
        }
    }
}