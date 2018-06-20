using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

using DiscordBot.Common;
using DiscordBot.Other;
using DiscordBot.Logging;

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
            Console.Write(@"MogiiBot 3: [");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(@"Version " + ProgramVersion.Major + @"." + ProgramVersion.Minor + @"." + ProgramVersion.Build + @"." + ProgramVersion.Revision);
            Console.ResetColor();
            Console.WriteLine(@"]    ");

            Console.WriteLine(@"A Discord.Net Bot");
            Console.WriteLine(@"-----------------------------------------------------------------");

            Console.Write(@"Developed by Melissa Brennan (");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(@"@MythicalCuddles");
            Console.ResetColor();
            Console.WriteLine(@")");

            Console.Write(@"MelissaNet: [");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(@"Version " + VersionInfo.Version);
            Console.ResetColor();
            Console.WriteLine(@"]    ");

            Console.WriteLine(@"Web: www.mythicalcuddles.xyz");
            Console.WriteLine(@"Project: mogiibot.mythicalcuddles.xyz");
            Console.WriteLine(@"Contact: melissa@mythicalcuddles.xyz");
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(@"Copyright 2017 - 2018 Melissa Brennan | Licensed under the MIT License.");
            Console.WriteLine(@"-----------------------------------------------------------------");

            Configuration.EnsureExists();
            StringConfiguration.EnsureExists();
            QuoteHandler.EnsureExists();
            VoteLinkHandler.EnsureExists();
            TransactionLogger.EnsureExists();
            Console.WriteLine(@"-----------------------------------------------------------------");
            
            if (Configuration.Load().SecretKey == null)
            {
                Console.WriteLine(@"Two Factor Authentication Running - Please save the QR Code in your authenticator app!");
                Console.WriteLine(@"-----------------------------------------------------------------");
                Application.Run(new frmAuth());
            }

            try
            {
                new DiscordBot().RunBotAsync().GetAwaiter().GetResult();
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.ToString());
                Console.WriteLine(Environment.NewLine + Environment.NewLine);

                Task.Delay(1000);

                StartBot();
            }

        }
    }
}
