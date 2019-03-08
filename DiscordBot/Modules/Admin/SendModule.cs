using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using DiscordBot.Common.Preconditions;
using DiscordBot.Common;
using DiscordBot.Objects;

namespace DiscordBot.Modules.Admin
{
    [Name("Send Commands")]
    [Group("send")]
    [RequireContext(ContextType.Guild | ContextType.DM)]
    [MinPermissions(PermissionLevel.ServerAdmin)]
    public class SendModule : ModuleBase
    {
        [Command("")]
        public async Task Send()
        {
            await ReplyAsync("**Syntax:** " +
                             Guild.Load(Context.Guild.Id).Prefix + "send [type] [mention/id] [message]\n```INI\n" +
                "Available Commands\n" +
                "-----------------------------\n" +
                "[ 1] Channel Message\n" +
                "[1a] send channelmessage [mention/id] [message]\n" +
                "[1b] send cmessage [mention/id] [message]\n" +
                "[1c] send channelmsg [mention/id] [message]\n" +
                "-----------------------------\n" +
                "[ 2] Private Message\n" +
                "[2a] send privatemessage [mention/id] [message]\n" +
                "[2b] send pm [mention/id] [message]\n" +
                "[2c] send directmessage [mention/id] [message]\n" +
                "[2d] send dm [mention/id] [message]\n" +
                "```");
        }

        [Command("channelmessage"), Summary("Sends a message to the channel specified.")]
        [Alias("cmessage", "channelmsg")]
        public async Task SendChannelMessage([Summary("The channel id to send the message to.")] ITextChannel channel, [Remainder]string message = null)
        {
            if(channel == null || message == null)
            {
                await ReplyAsync("**Syntax:** " + Guild.Load(Context.Guild.Id).Prefix + "send channelmessage [Channel ID] [Message]");
                return;
            }
            
            await channel.SendMessageAsync(message);

            EmbedAuthorBuilder eab = new EmbedAuthorBuilder()
                .WithName("Log Message");
            EmbedFooterBuilder efb = new EmbedFooterBuilder()
                .WithText("Message sent to " + channel.Mention + " | Sent by @" + Context.User.Username);

            EmbedBuilder eb = new EmbedBuilder()
                .WithAuthor(eab)
                .WithTitle("Author: @" + Context.User.Username)
                .WithColor(new Color(181, 81, 215))
                .WithDescription(message)
                .WithFooter(efb)
                .WithCurrentTimestamp();

            await ReplyAsync("", false, eb.Build());
        }

        [Command("privatemessage"), Summary("Sends a private message to the user specified.")]
        [Alias("pm", "dm", "directmessage")]
        public async Task SendPrivateMessage([Summary("The user to send the message to.")] IUser user = null, [Remainder]string message = null)
        {
            if (user == null || message == null)
            {
                await ReplyAsync("**Syntax:** " + Guild.Load(Context.Guild.Id).Prefix + "send privatemessage [@User] [Message]");
                return;
            }

            await user.GetOrCreateDMChannelAsync().Result.SendMessageAsync(message);

            EmbedAuthorBuilder eab = new EmbedAuthorBuilder()
                .WithName("to: @" + user.Username);
            EmbedFooterBuilder efb = new EmbedFooterBuilder()
                .WithText("Message Sent to @" + user.Username + " | Sent by @" + Context.User.Username);

            EmbedBuilder eb = new EmbedBuilder()
                .WithAuthor(eab)
                .WithTitle("from: @" + Context.User.Username)
                .WithColor(new Color(181, 81, 215))
                .WithDescription(message)
                .WithFooter(efb)
                .WithCurrentTimestamp();

            await ReplyAsync("", false, eb.Build());
        }
    }
}
