using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.Net.Providers.UDPClient;
using Discord.Net.Providers.WS4Net;
using Discord.WebSocket;

using DiscordBot.Common;
using DiscordBot.Database;
using DiscordBot.Exceptions;
using DiscordBot.Extensions;
using DiscordBot.Handlers;
using DiscordBot.Modules.Mod;
using DiscordBot.Objects;

using MelissaNet;

namespace DiscordBot
{
    public static class DiscordBot
    {
	    internal static DiscordSocketClient Bot { get; } = new DiscordSocketClient(new DiscordSocketConfig
        {
	        LogLevel = LogSeverity.Debug,
	        MessageCacheSize = 1000,
	        WebSocketProvider = WS4NetProvider.Instance,
	        UdpSocketProvider = UDPClientProvider.Instance,
	        DefaultRetryMode = RetryMode.AlwaysRetry,
	        AlwaysDownloadUsers = true,
	        ConnectionTimeout = int.MaxValue
        });

	    internal static readonly CommandService CommandService = new CommandService();
        internal static readonly IServiceProvider ServiceProvider = ConfigureServices();
        
        internal static IServiceProvider provider = new ServiceCollection()
	        .AddSingleton(Bot)
	        .AddSingleton<InteractiveService>()
	        .BuildServiceProvider();

        internal static async Task RunBotAsync()
        {
            #region Events
            Bot.Log += ConsoleHandler.Log;
            
            Bot.UserJoined += UserHandler.UserJoined;
            Bot.UserLeft += UserHandler.UserLeft;
	        Bot.UserUpdated += UserHandler.UserUpdated;
	        Bot.UserBanned += UserHandler.UserBanned;
	        Bot.UserUnbanned += UserHandler.UserUnbanned;
            
            Bot.ChannelCreated += ChannelHandler.ChannelCreated;
            Bot.ChannelDestroyed += ChannelHandler.ChannelDestroyed;
	        Bot.ChannelUpdated += ChannelHandler.ChannelUpdated;
            
            Bot.JoinedGuild += GuildHandler.JoinedGuild;
	        Bot.LeftGuild += GuildHandler.LeftGuild;
	        Bot.GuildUpdated += GuildHandler.GuildUpdated;

            Bot.MessageReceived += MessageHandler.MessageReceived;
            Bot.ReactionAdded += ReactionHandler.ReactionAdded;
            
            Bot.Ready += ReadyHandler.Ready;
            
            Bot.Disconnected += Disconnected;
            #endregion
	        
            await CommandService.AddModulesAsync(Assembly.GetEntryAssembly(), ServiceProvider);

            await Bot.LoginAsync(TokenType.Bot, Configuration.Load().BotToken);
            await Bot.StartAsync();

            // Keep the program running.
            await Task.Delay(-1).ConfigureAwait(false);
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection()
                .AddSingleton(Bot)
                .AddSingleton<InteractiveService>();
            return services.BuildServiceProvider();
        }

        private static async Task Disconnected(Exception exception)
        {
	        await new LogMessage(LogSeverity.Critical, "Disconnected", exception.ToString()).PrintToConsole();
        }
    }
}
