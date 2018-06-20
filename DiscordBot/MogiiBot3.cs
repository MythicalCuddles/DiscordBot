using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.Net.Providers.UDPClient;
using Discord.Net.Providers.WS4Net;
using Discord.WebSocket;

using DiscordBot.Common;
using DiscordBot.Extensions;
using DiscordBot.Handlers;
using DiscordBot.Modules.Mod;

using MelissaNet;

namespace DiscordBot
{
    public class MogiiBot3
    {
        public static DiscordSocketClient Bot;
        public static CommandService CommandService;
        public static IServiceProvider ServiceProvider;

        public async Task RunBotAsync()
        {
            Bot = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug,
                MessageCacheSize = 1000,
                WebSocketProvider = WS4NetProvider.Instance,
                UdpSocketProvider = UDPClientProvider.Instance,
                DefaultRetryMode = RetryMode.AlwaysRetry,
                AlwaysDownloadUsers = true,
                ConnectionTimeout = int.MaxValue,

            });
            CommandService = new CommandService();
            ServiceProvider = ConfigureServices();

            // Create Tasks for Bot Events
            Bot.Log += Log;
            Bot.UserJoined += UserHandler.UserJoined;
            Bot.UserLeft += UserHandler.UserLeft;
            Bot.ChannelCreated += ChannelHandler.ChannelCreated;
            Bot.ChannelDestroyed += ChannelHandler.ChannelDestroyed;
            Bot.JoinedGuild += BotOnJoinedGuild;
            Bot.ReactionAdded += ReactionHandler.ReactionAdded;
            Bot.MessageReceived += MessageReceived;
            Bot.Ready += Ready;
            Bot.Disconnected += Disconnected;

            await CommandService.AddModulesAsync(Assembly.GetEntryAssembly(), ServiceProvider);

            await LoginAndStart();

            // Keep the program running.
            await Task.Delay(-1);
        }

        private IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection()
                .AddSingleton(Bot)
                .AddSingleton<InteractiveService>();
            return services.BuildServiceProvider();
        }

        private static async Task LoginAndStart()
        {
            try
            {
                await Bot.LoginAsync(TokenType.Bot, Cryptography.DecryptString(Configuration.Load().BotToken));
                await Bot.StartAsync();
            }
            catch (CryptographicException exception)
            {
                Console.WriteLine(@"Exception caught: " + exception.Source + Environment.NewLine + Environment.NewLine);
                ReEnterToken();
            }
            catch (Discord.Net.HttpException exception)
            {
                if (exception.HttpCode == HttpStatusCode.Unauthorized || exception.HttpCode == HttpStatusCode.Forbidden)
                {
                    ReEnterToken();
                }
            }
            catch (FormatException)
            {
                ReEnterToken();
            }
            catch (Exception)
            {
                Console.WriteLine(@"An error has occurred.");
                throw;
            }
        }

        private static void ReEnterToken(string reasoning = "The token stored on file doesn't seem to be working. Please re-enter the bot token.")
        {
            //TODO: Clean Up
            Console.WriteLine(reasoning);

            Console.Write(@"Token: ");
            Configuration.UpdateConfiguration(botToken:Cryptography.EncryptString(Console.ReadLine()));

            Console.WriteLine(
                @"Token saved successfully. Console will now be cleared for security reasons. Press the 'enter' key to continue.");
            Console.ReadLine();
            Console.Clear();

            new MogiiBot3().RunBotAsync().GetAwaiter().GetResult();
        }

        private static Task Log(LogMessage logMessage)
        {
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
	        Console.WriteLine($@"{DateTime.Now,-19} [{logMessage.Severity,8}] {logMessage.Source}: {logMessage.ToString()}");
            Console.ForegroundColor = cc;
	        return Task.CompletedTask;
        }

        private static async Task Ready()
        {
            List<Tuple<SocketGuildUser, SocketGuild>> offlineList = new List<Tuple<SocketGuildUser, SocketGuild>>();

            await Bot.SetGameAsync(Configuration.Load().StatusText, Configuration.Load().StatusLink,
                (ActivityType) Configuration.Load().StatusActivity);

			await Bot.SetStatusAsync(Configuration.Load().Status);

			ModeratorModule.ActiveForDateTime = DateTime.Now;

			Console.WriteLine(@"-----------------------------------------------------------------");
			foreach (SocketGuild g in Bot.Guilds)
			{
			    Console.ResetColor();
				Console.Write(@"status: [");
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.Write(@"find");
				Console.ResetColor();
				Console.WriteLine(@"]  " + g.Name + @": attempting to load.");

				GuildConfiguration.EnsureExists(g.Id);

				Console.WriteLine(@"-----------------------------------------------------------------");

				foreach (SocketGuildUser u in g.Users)
				{
					if (User.CreateUserFile(u.Id))
					{
					    offlineList.Add(new Tuple<SocketGuildUser, SocketGuild>(u, g));
					}
				}

			    Console.WriteLine(@"-----------------------------------------------------------------");

			    foreach (SocketGuildChannel c in g.Channels)
			    {
			        Channel.EnsureExists(c.Id);
			    }

			    Console.WriteLine(@"-----------------------------------------------------------------");
            }

            Console.Write(@"status: [");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write(@"info");
            Console.ResetColor();
            Console.Write(@"]  " + Bot.CurrentUser.Username + @" : ");
            if (offlineList.Any())
            {
                Console.WriteLine(offlineList.Count + @" new users added.");
                foreach (Tuple<SocketGuildUser, SocketGuild> tupleList in offlineList)
                {
                    Console.WriteLine(@"[ALERT] While " + Bot.CurrentUser.Username + @" was offline, " + tupleList.Item1.Mention + @" (" + tupleList.Item1.Id + @") joined " + tupleList.Item2.Name + @". They have been added to the database.");
                }
            }
            else
                Console.WriteLine(@"no new users added.");

            Console.Write(@"status: [");
		    Console.ForegroundColor = ConsoleColor.DarkGreen;
		    Console.Write(@"ok");
		    Console.ResetColor();
		    Console.WriteLine(@"]    " + Bot.CurrentUser.Id + @": " + Bot.CurrentUser.Username + @" loaded.");

			// Send message to log channel to announce bot is up and running.
			Version v = Assembly.GetExecutingAssembly().GetName().Version;
			EmbedBuilder eb = new EmbedBuilder()
					.WithTitle("Startup Notification")
					.WithColor(59, 212, 50)
					.WithThumbnailUrl(Bot.CurrentUser.GetAvatarUrl())
					.WithDescription("**" + Bot.CurrentUser.Username + "** : ready event executed.")
                    .AddField("Version", v.Major + "." + v.Minor + "." + v.Build + "." + v.Revision, true)
                    .AddField("Latest Version", MythicalCuddlesXYZ.CheckForNewVersion("MogiiBot3").Item1, true)
                    .AddField("MelissaNet", VersionInfo.Version, true)
					.AddField("Latency", Bot.Latency + "ms", true)
                    .WithCurrentTimestamp();
				await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("", false, eb.Build());

            if (offlineList.Any())
            {
                foreach (Tuple<SocketGuildUser, SocketGuild> tupleList in offlineList)
                {
                    await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("[ALERT] While " + Bot.CurrentUser.Username + " was offline, " + tupleList.Item1.Mention + " (" + tupleList.Item1.Id + ") joined " + tupleList.Item2.Name + ". They have been added to the database.");
                }
            }
        }

        private static async Task BotOnJoinedGuild(SocketGuild socketGuild)
        {
            GuildConfiguration.EnsureExists(socketGuild.Id);

            foreach (SocketGuildChannel c in socketGuild.Channels)
                Channel.EnsureExists(c.Id);

            await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync(socketGuild.Name + " has been added to MogiiBot's guild list. \n" + socketGuild.Owner.Username + " is the owner (" + socketGuild.Owner.Id + ")");
        }

        private static Task Disconnected(Exception exception)
        {
			Console.WriteLine(exception + Environment.NewLine);
            return Task.CompletedTask;
        }
	    
        private static async Task MessageReceived(SocketMessage messageParam)
        {
            if (!(messageParam is SocketUserMessage message)) return; // If the message is null, return.
            if (message.Author.IsBot) return; // If the message was posted by a BOT account, return.
            //if (!(messageParam.Channel is ITextChannel)) { return; } // If the message came from somewhere that is not a text channel.
            if (User.Load(message.Author.Id).IsBotIgnoringUser && message.Author.Id != MelissaNet.Discord.GetMelissaId()) { return; } // If the bot is ignoring the user AND the user NOT Melissa.

            // If the message came from somewhere that is not a text channel -> Private Message
            if (!(messageParam.Channel is ITextChannel))
            {
                EmbedFooterBuilder efb = new EmbedFooterBuilder()
                    .WithText("UID: " + message.Author.Id + " | MID: " + message.Id);
                EmbedBuilder eb = new EmbedBuilder()
                    .WithTitle("Private Message - Posted By: @" + message.Author.Username + "#" + message.Author.Discriminator)
                    .WithDescription(message.Content)
                    .WithFooter(efb)
                    .WithCurrentTimestamp();

                await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("", false, eb.Build());

                return;
            }

            var uPrefix = User.Load(message.Author.Id).CustomPrefix;
            var gPrefix = GuildConfiguration.Load(message.Channel.GetGuild().Id).Prefix;
            if (uPrefix == null) { uPrefix = gPrefix; } // Fixes an issue with users not receiving coins due to null prefix.
            var argPos = 0;
            if (message.HasStringPrefix(gPrefix, ref argPos) || 
                message.HasMentionPrefix(Bot.CurrentUser, ref argPos) || 
                message.HasStringPrefix(uPrefix, ref argPos)) {
                var context = new SocketCommandContext(Bot, message);
                var result = await CommandService.ExecuteAsync(context, argPos, ServiceProvider);

                if (!result.IsSuccess && Configuration.Load().UnknownCommandEnabled)
                {
                    var errorMessage = await context.Channel.SendMessageAsync(messageParam.Author.Mention + ", " + result.ErrorReason);

                    Console.Write(@"status: [");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(@"warning");
                    Console.ResetColor();
                    Console.WriteLine(@"]  " + message.Author.Username + @" : " + result.ErrorReason);

                    errorMessage.DeleteAfter(20);
                }
            }
            else if (message.Content.ToUpper() == "F") // If the message is just "F", pay respects.
            {
                var respects = Configuration.Load().Respects + 1;
                Configuration.UpdateConfiguration(respects: respects);

                var eb = new EmbedBuilder()
                    .WithDescription("**" + message.Author.Username + "** has paid their respects.")
                    .WithFooter("Total Respects: " + respects)
                    .WithColor(User.Load(message.Author.Id).AboutR, User.Load(message.Author.Id).AboutG, User.Load(message.Author.Id).AboutB);

                await message.Channel.SendMessageAsync("", false, eb.Build());
            }
            else
            {
                if (message.Content.Length >= Configuration.Load().MinLengthForCoin)
                {
                    if (Channel.Load(message.Channel.Id).AwardingCoins)
                    {
                        AwardCoinsToPlayer(message.Author);
                    }
                }
            }
        }

        public static void AwardCoinsToPlayer(IUser user, int coinsToAward = 1)
        {
            try
            {
                User.UpdateUser(user.Id, (user.GetCoins() + coinsToAward));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
