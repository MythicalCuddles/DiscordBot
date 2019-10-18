using System;
using System.Linq;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using DiscordBot.Common;
using DiscordBot.Common.Preconditions;
using DiscordBot.Extensions;
using DiscordBot.Logging;
using DiscordBot.Objects;
using MelissaNet;

namespace DiscordBot.Modules.TeamMember
{
    [Name("Team Commands")]
    [MinPermissions(PermissionLevel.TeamMember)]
    public class TeamMemberModule : ModuleBase
    {
        [Command("globalmessage")]
        public async Task SendMessageToAllGuilds([Remainder]string message = null)
        {
            if (message == null)
            {
                await ReplyAsync("**Syntax:** " + Guild.Load(Context.Guild.Id).Prefix + "globalmessage [message]\n" + 
                                 "This will post an embed message to all guilds. It's main purpose is to inform guild owners of updates and changes.");
                return;
            }

            AdminLog.Log(Context.User.Id, Context.Message.Content, Context.Guild.Id);
            
            EmbedBuilder eb = new EmbedBuilder()
            {
                Title = "Announcement from " + Context.User.Username,
                Color = new Color(User.Load(Context.User.Id).AboutR, User.Load(Context.User.Id).AboutG, User.Load(Context.User.Id).AboutB),
                Description = message
            }.WithCurrentTimestamp();

            foreach (SocketGuild g in DiscordBot.Bot.Guilds)
            {
                await Guild.Load(g.Id).LogChannelID.GetTextChannel().SendMessageAsync("", false, eb.Build());
            }

            await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("", false, eb.WithFooter("Message sent to all guilds").Build());
        }
    }
}