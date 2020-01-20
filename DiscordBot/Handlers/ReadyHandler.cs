using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;

using DiscordBot.Common;
using DiscordBot.Database;
using DiscordBot.Exceptions;
using DiscordBot.Extensions;
using DiscordBot.Modules.Mod;
using DiscordBot.Objects;

using MelissaNet;

namespace DiscordBot.Handlers
{
    public static class ReadyHandler
    {
        private static readonly List<Tuple<SocketGuildUser, SocketGuild>> OfflineList = new List<Tuple<SocketGuildUser, SocketGuild>>();
        internal static async Task Ready()
        {
	        if (Configuration.Load().ActivityStream == null)
	        {
		        IActivity activity = new Game(Configuration.Load().ActivityName, (ActivityType)Configuration.Load().ActivityType);
		        await DiscordBot.Bot.SetActivityAsync(activity);
	        }
	        else
	        {
		        IActivity activity = new StreamingGame(Configuration.Load().ActivityName, Configuration.Load().ActivityStream);
		        await DiscordBot.Bot.SetActivityAsync(activity);
	        }
	        await DiscordBot.Bot.SetStatusAsync(Configuration.Load().Status);

			
	        ModeratorModule.ActiveForDateTime = DateTime.Now;

			
			List<ulong> guildsInDatabase = new List<ulong>();
	        var dataReader = DatabaseActivity.ExecuteReader("SELECT * FROM guilds;").Item1;
	        while (dataReader.Read())
	        {
		        ulong id = dataReader.GetUInt64("guildID");
		        guildsInDatabase.Add(id);
	        }
	        
	        
	        await new LogMessage(LogSeverity.Info, "Startup", "-----------------------------------------------------------------").PrintToConsole();
			foreach (SocketGuild g in DiscordBot.Bot.Guilds)
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
	        
	        await new LogMessage(LogSeverity.Info, "Startup", DiscordBot.Bot.CurrentUser.Username + " loaded.").PrintToConsole();
			
	        // Send message to log channel to announce bot is up and running.
			Version v = Assembly.GetExecutingAssembly().GetName().Version;
			EmbedBuilder eb = new EmbedBuilder();
					eb.WithTitle("Startup Notification");
					eb.WithColor(59, 212, 50);
					eb.WithThumbnailUrl(DiscordBot.Bot.CurrentUser.GetAvatarUrl());

					eb.AddField("Bot Name", DiscordBot.Bot.CurrentUser.Username + "#" + DiscordBot.Bot.CurrentUser.Discriminator, true);
					try
					{
						eb.AddField("Developer Name",
							Configuration.Load().Developer.GetUser().Username + "#" +
							Configuration.Load().Developer.GetUser().Discriminator, true);
					}
					catch (UserNotFoundException exception)
					{
						eb.AddField("Developer Name", "Melissa", true);
						await new LogMessage(LogSeverity.Warning, "Startup", exception.Message + " - Using \"Melissa\" instead.").PrintToConsole();
					}
					eb.AddField("Developer ID", Configuration.Load().Developer, true);

					eb.AddField("DiscordBot Version", "v" + v, true);
					eb.AddField("MelissaNET Version", "v" + VersionInfo.Version, true);
					eb.AddField(".NET Version", typeof(string).Assembly.ImageRuntimeVersion, true);

					eb.AddField("Connection & Server Information",
						"**Latency:** " + DiscordBot.Bot.Latency + "ms" + "\n" +
						"**Server Time:** " + DateTime.Now.ToString("h:mm:ss tt") + "\n");

					eb.AddField("Awards", Award.Awards.Count, true);
					eb.AddField("Quotes", Quote.Quotes.Count, true);
					eb.AddField("Quote Requests", RequestQuote.RequestQuotes.Count, true);

					eb.WithCurrentTimestamp();
					eb.WithFooter("Ready event executed");
			await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("", false, eb.Build());

			await NewUsersOfflineAlert();
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
			if (g.GetUser(DiscordBot.Bot.CurrentUser.Id).IsGuildAdministrator() || g.GetUser(DiscordBot.Bot.CurrentUser.Id).GuildPermissions.BanMembers)
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
						("@issuedBy", DiscordBot.Bot.CurrentUser.Id.ToString()), // unable to get the issuedBy user ID, so use the Bot ID instead.
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
	    
	    private static async Task NewUsersOfflineAlert() {
		    if (OfflineList.Any())
		    {
			    await new LogMessage(LogSeverity.Info, "Startup", OfflineList.Count + " new users added.").PrintToConsole();
			    foreach (Tuple<SocketGuildUser, SocketGuild> tupleList in OfflineList)
			    {
				    await new LogMessage(LogSeverity.Info, "Startup", tupleList.Item1.Username + " (" + tupleList.Item1.Id + ") joined " + tupleList.Item2.Name + " while the Bot was offline.").PrintToConsole();
				    await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("[ALERT] While " + DiscordBot.Bot.CurrentUser.Username + " was offline, " + tupleList.Item1.Mention + " (" + tupleList.Item1.Id + ") joined " + tupleList.Item2.Name + ". They have been added to the database.");
			    }
		    }
	    } 
    }
}