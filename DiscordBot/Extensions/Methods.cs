using System;
using System.Threading.Tasks;
using Discord;
using Discord.Net.Providers.UDPClient;
using Discord.Net.Providers.WS4Net;
using Discord.WebSocket;
using DiscordBot.Common;

namespace DiscordBot.Extensions
{
    public static class Methods
    {
        public static async void PrintConsoleSplitLine()
        {
            await new LogMessage(LogSeverity.Info, "",
                "----------------------------------------------------------------------").PrintToConsole();
        }

        internal static async Task<bool> VerifyBotToken(string token = null)
        {
            if (token == null)
            {
                token = Configuration.Load().BotToken;
            }
            
            DiscordSocketClient testingClient = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug,
                DefaultRetryMode = RetryMode.AlwaysFail,
                ConnectionTimeout = int.MaxValue
            });
            
            await new LogMessage(LogSeverity.Info, "Bot Token Validation", "Verifying Bot Token.").PrintToConsole();
            
            try
            {
                await testingClient.LoginAsync(TokenType.Bot, token);
                await testingClient.StartAsync();
                
                await new LogMessage(LogSeverity.Info, "Bot Token Validation", "Successfully connected to DiscordAPI using Bot Token.").PrintToConsole();
                
                await testingClient.LogoutAsync();
            
                return true;
            }
            catch (Exception e)
            {
                await new LogMessage(LogSeverity.Warning, "Bot Token Validation",
                    "Token Invalid. " + e.Message).PrintToConsole();
                return false;
            }
        }
    }
}