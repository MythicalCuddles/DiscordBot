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

	    private static readonly CommandService CommandService = new CommandService();
        private static readonly IServiceProvider ServiceProvider = ConfigureServices();

        internal static async Task RunBotAsync()
        {
            // Create Tasks for Bot Events
            #region Events
            Bot.Log += Log;
            
            Bot.UserJoined += UserHandler.UserJoined;
            Bot.UserLeft += UserHandler.UserLeft;
	        Bot.UserUpdated += UserHandler.UserUpdated;
            
            Bot.ChannelCreated += ChannelHandler.ChannelCreated;
            Bot.ChannelDestroyed += ChannelHandler.ChannelDestroyed;
	        Bot.ChannelUpdated += ChannelHandler.ChannelUpdated;
            
            Bot.JoinedGuild += GuildHandler.JoinedGuild;
	        Bot.LeftGuild += GuildHandler.LeftGuild;
	        Bot.GuildUpdated += GuildHandler.GuildUpdated;

	        Bot.UserBanned += UserHandler.UserBanned;
	        Bot.UserUnbanned += UserHandler.UserUnbanned;
            
            Bot.ReactionAdded += ReactionHandler.ReactionAdded;
            
            Bot.MessageReceived += MessageReceived;
            
            Bot.Ready += Ready;
            
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

        internal static Task Log(LogMessage logMessage)
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
	        Console.WriteLine($@"{DateTime.Now,-19} [{logMessage.Severity,8}] {logMessage.Source}: {logMessage.Message}"); // .PrintToConsole uses this Console.WriteLine so do not change it!
            Console.ForegroundColor = cc;
	        return Task.CompletedTask;
        }
	    
	    private static readonly List<Tuple<SocketGuildUser, SocketGuild>> OfflineList = new List<Tuple<SocketGuildUser, SocketGuild>>();
        private static async Task Ready()
        {
	        List<ulong> guildsInDatabase = new List<ulong>();

	        if (Configuration.Load().ActivityStream == null)
	        {
		        IActivity activity = new Game(Configuration.Load().ActivityName, (ActivityType)Configuration.Load().ActivityType);
		        await Bot.SetActivityAsync(activity);
	        }
	        else
	        {
		        IActivity activity = new StreamingGame(Configuration.Load().ActivityName, Configuration.Load().ActivityStream);
		        await Bot.SetActivityAsync(activity);
	        }
//            await Bot.SetGameAsync(Configuration.Load().StatusText, Configuration.Load().StatusLink,
//                (ActivityType) Configuration.Load().StatusActivity);

	        await Bot.SetStatusAsync(Configuration.Load().Status);

			ModeratorModule.ActiveForDateTime = DateTime.Now;

	        var dataReader = DatabaseActivity.ExecuteReader("SELECT * FROM guilds;").Item1;
	        while (dataReader.Read())
	        {
		        ulong id = dataReader.GetUInt64("guildID");
		        guildsInDatabase.Add(id);
	        }
	        
	        await new LogMessage(LogSeverity.Info, "Startup", "-----------------------------------------------------------------").PrintToConsole();
			foreach (SocketGuild g in Bot.Guilds)
			{
			    Console.ResetColor();
				await new LogMessage(LogSeverity.Info, "Startup", "Attempting to load " + g.Name).PrintToConsole();

				await GuildHandler.InsertGuildToDB(g);
				guildsInDatabase.Remove(g.Id);
				await new LogMessage(LogSeverity.Info, "Startup", "-----------------------------------------------------------------").PrintToConsole();

				foreach (SocketGuildChannel c in g.Channels)
				{
					await ChannelHandler.InsertChannelToDB(c);
				}
				await new LogMessage(LogSeverity.Info, "Startup", "-----------------------------------------------------------------").PrintToConsole();

				await ReadyAddUsersToDatabase(g).ConfigureAwait(false);
				await new LogMessage(LogSeverity.Info, "Startup", "-----------------------------------------------------------------").PrintToConsole();

				await ReadyAddBansToDatabase(g).ConfigureAwait(false);
				Methods.PrintConsoleSplitLine();
            }

	        foreach (ulong id in guildsInDatabase)
	        {
		        await GuildHandler.RemoveGuildFromDB(id.GetGuild());
		        DatabaseActivity.ExecuteNonQueryCommand("DELETE FROM channels WHERE inGuildID=" + id);
		        Console.WriteLine(id + @" has been removed from the database.");
	        }
	        
	        await LoadAllFromDatabase();
	        
	        await new LogMessage(LogSeverity.Info, "Startup", Bot.CurrentUser.Username + " loaded.").PrintToConsole();
			
	        // Send message to log channel to announce bot is up and running.
			Version v = Assembly.GetExecutingAssembly().GetName().Version;
			EmbedBuilder eb = new EmbedBuilder()
					.WithTitle("Startup Notification")
					.WithColor(59, 212, 50)
					.WithThumbnailUrl(Bot.CurrentUser.GetAvatarUrl())
					.WithDescription("**" + Bot.CurrentUser.Username + "** : ready event executed.")
                    .AddField("Version", v.Major + "." + v.Minor + "." + v.Build + "." + v.Revision, true)
                    .AddField("MelissaNet", VersionInfo.Version, true)
					.AddField("Latency", Bot.Latency + "ms", true)
					
					.AddField("Awards", Award.Awards.Count, true)
					.AddField("Quotes", Quote.Quotes.Count, true)
					.AddField("Quote Requests", RequestQuote.RequestQuotes.Count, true)
					
                    .WithCurrentTimestamp();
				await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("", false, eb.Build());

            if (OfflineList.Any())
            {
	            await new LogMessage(LogSeverity.Info, "Startup", OfflineList.Count + " new users added.").PrintToConsole();
				foreach (Tuple<SocketGuildUser, SocketGuild> tupleList in OfflineList)
				{
					await new LogMessage(LogSeverity.Warning, "Startup", tupleList.Item1.Username + " (" + tupleList.Item1.Id + ") joined " + tupleList.Item2.Name + " while the Bot was offline.").PrintToConsole();
					await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("[ALERT] While " + Bot.CurrentUser.Username + " was offline, " + tupleList.Item1.Mention + " (" + tupleList.Item1.Id + ") joined " + tupleList.Item2.Name + ". They have been added to the database.");
				}
            }
        }
	    
	    private static Task ReadyAddUsersToDatabase(SocketGuild g)
	    {
		    foreach (SocketGuildUser u in g.Users)
		    {
			    //Insert new users into the database by using INSERT IGNORE
			    List<(string, string)> queryParams = new List<(string id, string value)>()
			    {
				    ("@username", u.Username),
				    ("@avatarUrl", u.GetAvatarUrl())
			    };
					
			    int rowsUpdated = DatabaseActivity.ExecuteNonQueryCommand(
				    "INSERT IGNORE INTO " +
				    "users(id,username,avatarUrl) " +
				    "VALUES (" + u.Id + ", @username, @avatarUrl);", queryParams);
					
			    //end.
					
			    if (rowsUpdated > 0) // If any rows were affected, add the user to the list to be dealt with later.
			    {
				    OfflineList.Add(new Tuple<SocketGuildUser, SocketGuild>(u, g));
			    }
		    }
		    
		    return Task.CompletedTask;
	    }
	    private static async Task ReadyAddBansToDatabase(SocketGuild g)
	    {
			if (g.GetUser(Bot.CurrentUser.Id).IsGuildAdministrator() || g.GetUser(Bot.CurrentUser.Id).GuildPermissions.BanMembers)
			{
				var bans = await g.GetBansAsync();
				foreach (IBan b in bans)
				{
					var (dataReader, mysqlConnection) = DatabaseActivity.ExecuteReader("SELECT * FROM bans WHERE issuedTo=" + b.User.Id + " AND inGuild=" + g.Id + ";");
					
					int count = 0;
					while (dataReader.Read())
					{
						count++;
					}
            
					dataReader.Close();
					mysqlConnection.Close();

					if (count != 0) continue;
					
					//Insert banned users into the database by using INSERT IGNORE
					List<(string, string)> queryParams = new List<(string id, string value)>()
					{
						("@issuedTo", b.User.Id.ToString()),
						("@issuedBy", Bot.CurrentUser.Id.ToString()), // unable to get the issuedBy user ID, so use the Bot ID instead.
						("@inGuild", g.Id.ToString()),
						("@reason", b.Reason),
						("@date", DateTime.Now.ToString("u"))
					};
					
					DatabaseActivity.ExecuteNonQueryCommand(
						"INSERT IGNORE INTO " +
						"bans(issuedTo,issuedBy,inGuild,banDescription,dateIssued) " +
						"VALUES (@issuedTo, @issuedBy, @inGuild, @reason, @date);", queryParams);
					
					//end.
				}
			}
			else
			{
				await new LogMessage(LogSeverity.Info, "Guild Bans", "Unable to get banned users - Bot doesn't have the required permission(s).").PrintToConsole();
			}
	    }
	    
	    private static async Task LoadAllFromDatabase() {
		    Award.Awards = Award.LoadAll();
		    await new LogMessage(LogSeverity.Info, "Startup", "Loaded " + Award.Awards.Count + " Awards from the Database.").PrintToConsole();
		    Quote.Quotes = Quote.LoadAll();
		    await new LogMessage(LogSeverity.Info, "Startup", "Loaded " + Quote.Quotes.Count + " Quotes from the Database.").PrintToConsole();
		    RequestQuote.RequestQuotes = RequestQuote.LoadAll();
		    await new LogMessage(LogSeverity.Info, "Startup", "Loaded " + RequestQuote.RequestQuotes.Count + " RequestQuotes from the Database.").PrintToConsole();
	    }

        private static async Task Disconnected(Exception exception)
        {
	        await new LogMessage(LogSeverity.Critical, "Disconnected", exception.ToString()).PrintToConsole();
        }
	    
        private static async Task MessageReceived(SocketMessage messageParam)
        {
            if (!(messageParam is SocketUserMessage message)) { return; } // If the message is null, return.
            if (message.Author.IsBot) { return; } // If the message was posted by a BOT account, return.
            if (message.Author.IsUserIgnoredByBot() && message.Author.Id != Configuration.Load().Developer) { return; } // If the bot is ignoring the user AND the user NOT Melissa.

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

	        await new LogMessage(LogSeverity.Info, "MessageReceived", "[" + messageParam.Channel.GetGuild().Name + "/#" + messageParam.Channel.Name + "] " + "[@" + 
	                                                            messageParam.Author.Username + "] : " + messageParam.Content).PrintToConsole();

            var uPrefix = message.Author.GetCustomPrefix();
            var gPrefix = Guild.Load(message.Channel.GetGuild().Id).Prefix;
            if (string.IsNullOrEmpty(uPrefix)) { uPrefix = gPrefix; } // Fixes an issue with users not receiving coins due to null prefix.
            var argPos = 0;
            if (message.HasStringPrefix(gPrefix, ref argPos) || 
                message.HasMentionPrefix(Bot.CurrentUser, ref argPos) || 
                message.HasStringPrefix(uPrefix, ref argPos)) {
                var context = new SocketCommandContext(Bot, message);
                var result = await CommandService.ExecuteAsync(context, argPos, ServiceProvider);

                if (!result.IsSuccess && Configuration.Load().UnknownCommandEnabled)
                {
                    var errorMessage = await context.Channel.SendMessageAsync(messageParam.Author.Mention + ", " + result.ErrorReason);

	                await new LogMessage(LogSeverity.Error, "MessageReceived", message.Author.Username + " - " + result.ErrorReason).PrintToConsole();

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
                    .WithColor(message.Author.GetCustomRGB());

	            await message.Channel.SendMessageAsync("", false, eb.Build());
            }
            else
            {
	            if(Configuration.Load().AwardingEXPEnabled)
                {
	                if (message.Content.Length >= Configuration.Load().MinLengthForEXP)
	                {
						if (Channel.Load(message.Channel.Id).AwardingEXP)
						{
							message.Author.AwardEXPToUser(message.Channel.GetGuild());
						}
	                }
                }
            }
        }
    }
}
