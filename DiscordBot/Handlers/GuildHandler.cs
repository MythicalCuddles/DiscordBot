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
            await InsertGuildToDB(socketGuild);
            
            foreach (SocketGuildChannel c in socketGuild.Channels)
            {
                await ChannelHandler.InsertChannelToDB(c);
            }

            await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync(socketGuild.Name + " has been added to " + DiscordBot.Bot.CurrentUser.Username + "'s guild list. \n" + socketGuild.Owner.Username + " is the owner (" + socketGuild.Owner.Id + ")");
            
            await new LogMessage(LogSeverity.Info, "JoinedGuild", "[" + socketGuild.Name + "] " + "[@" + socketGuild.Owner.Username + 
                                                              "] : Bot Joined the Guild").PrintToConsole();
        }

        public static async Task InsertGuildToDB(SocketGuild g)
        {
            SocketGuildUser gBot = g.GetUser(DiscordBot.Bot.CurrentUser.Id);
            
            List<(string, string)> queryParams = new List<(string id, string value)>()
            {
                ("@guildID", g.Id.ToString()),
                ("@guildName", g.Name),
                ("@guildIcon", g.IconUrl),
                ("@ownedBy", g.Owner.Id.ToString()),
                ("@dateJoined", gBot.JoinedAt.Value.DateTime.ToString("u")), //"yyyy-MM-dd HH:mm:ss"
                ("@guildPrefix", "$") 
            };
				    
            DatabaseActivity.ExecuteNonQueryCommand(
                "INSERT IGNORE INTO " +
                "guilds(guildID,guildName,guildIcon,ownedBy,dateJoined,guildPrefix) " +
                "VALUES (@guildID, @guildName, @guildIcon, @ownedBy, @dateJoined, @guildPrefix);", queryParams);
        }

        public static async Task LeftGuild(SocketGuild socketGuild)
        {
            
        }

        public static async Task GuildUpdated(SocketGuild cachedSocketGuild, SocketGuild socketGuild)
        {
            
        }

        public static async Task GuildMemberUpdated(SocketGuildUser cachedSocketGuildUser, SocketGuildUser socketGuildUser)
        {
            
        }
    }
}