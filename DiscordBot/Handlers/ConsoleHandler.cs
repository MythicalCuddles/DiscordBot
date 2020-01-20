using System;
using System.Threading.Tasks;
using Discord;
using DiscordBot.Extensions;

namespace DiscordBot.Handlers
{
    public static class ConsoleHandler
    {
        internal static Task Log(LogMessage logMessage)
        {
            // NOTE: The extension .PrintToConsole uses method so do not change it!
            var cc = Console.ForegroundColor;
            switch (logMessage.Severity)
            {
                case LogSeverity.Critical:
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;

                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;

                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;

                case LogSeverity.Verbose:
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
            }
            Console.WriteLine($@"{DateTime.Now,-19} [{logMessage.Severity,8}] {logMessage.Source}: {logMessage.Message}");
            Console.ForegroundColor = cc;
            return Task.CompletedTask;
            // EON
        }
    }
}