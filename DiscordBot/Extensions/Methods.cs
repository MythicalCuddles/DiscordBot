using Discord;

namespace DiscordBot.Extensions
{
    public class Methods
    {
        public static async void PrintConsoleSplitLine()
        {
            await new LogMessage(LogSeverity.Info, "",
                "-----------------------------------------------------------------").PrintToConsole();
        }
    }
}