using System;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;

using DiscordBot.Common;
using DiscordBot.Extensions;

namespace DiscordBot.Handlers
{
    public static class GuildHandler
    {
        public static async Task BotOnJoinedGuild(SocketGuild socketGuild)
        {
            GuildConfiguration.EnsureExists(socketGuild.Id);

            foreach (SocketGuildChannel c in socketGuild.Channels)
                Channel.EnsureExists(c.Id);

            await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync(socketGuild.Name + " has been added to MogiiBot's guild list. \n" + socketGuild.Owner.Username + " is the owner (" + socketGuild.Owner.Id + ")");
        }
    }
}