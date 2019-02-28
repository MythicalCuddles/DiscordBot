using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;

using DiscordBot.Common;
using DiscordBot.Database;
using DiscordBot.Extensions;

namespace DiscordBot.Handlers
{
    public static class GuildHandler
    {
        public static async Task JoinedGuild(SocketGuild socketGuild)
        {
            GuildConfiguration.EnsureExists(socketGuild.Id);

            foreach (SocketGuildChannel c in socketGuild.Channels)
            {
                List<(string, string)> queryParams = new List<(string id, string value)>()
                {
                    ("@channelName", c.Name),
                    ("@channelType", c.GetType().Name)
                };
				    
                int rowsUpdated = DatabaseActivity.ExecuteNonQueryCommand(
                    "INSERT IGNORE INTO " +
                    "channels(channelID,inGuildID,channelName,channelType) " +
                    "VALUES (" + c.Id + ", " + c.Guild.Id + ", @channelName, @channelType);", queryParams);
            }

            await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync(socketGuild.Name + " has been added to " + DiscordBot.Bot.CurrentUser.Username + "'s guild list. \n" + socketGuild.Owner.Username + " is the owner (" + socketGuild.Owner.Id + ")");
            
            await new LogMessage(LogSeverity.Info, "JoinedGuild", "[" + socketGuild.Name + "] " + "[@" + socketGuild.Owner.Username + 
                                                              "] : Bot Joined the Guild").PrintToConsole();
        }
    }
}