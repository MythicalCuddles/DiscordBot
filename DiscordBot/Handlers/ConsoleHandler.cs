using System;
using Discord;

namespace DiscordBot.Handlers
{
    public static class ConsoleHandler
    {
        private const LogSeverity ExceptionSeverity = LogSeverity.Error; // For overloading methods below to handle multiple exceptions.

        public static void PrintExceptionToLog(string source, Exception e)
        {
            new LogMessage(ExceptionSeverity, source, e.Message);
        }
    }
}