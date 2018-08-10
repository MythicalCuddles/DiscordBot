using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using DiscordBot.Common;
using DiscordBot.Common.Preconditions;
using DiscordBot.Extensions;

namespace DiscordBot.Modules.SOwner
{
    [MinPermissions(PermissionLevel.ServerOwner)]
    [Name("Server Owner Commands")]
    public class ServerOwnerModule : ModuleBase
    {
        [Command("guildprefix"), Summary("Set the prefix for the bot for the guild.")]
        public async Task SetPrefix(string prefix)
        {
            GuildConfiguration.UpdateGuild(Context.Guild.Id, prefix:prefix);
            await ReplyAsync(Context.User.Mention + " has updated the Prefix to: " + prefix);
        }

        [Command("setwelcome"), Summary("Set the welcome message to the specified string.")]
        [Alias("setwelcomemessage", "sw")]
        public async Task SetWelcomeMessage([Remainder]string message = null)
        {
            if (message == null)
            {
                StringBuilder sb = new StringBuilder()
                    .Append("**Syntax:** " + GuildConfiguration.Load(Context.Guild.Id).Prefix + "setwelcome [Welcome Message]\n\n")
                    .Append("```Available Flags\n")
                    .Append("---------------\n")
                    .Append("New User Flags:\n")
                    .Append("{USER.MENTION} - @" + Context.User.Username + "\n")
                    .Append("{USER.USERNAME} - " + Context.User.Username + "\n")
                    .Append("{USER.DISCRIMINATOR} - " + Context.User.Discriminator + "\n")
                    .Append("{USER.AVATARURL} - " + Context.User.GetAvatarUrl() + "\n")
                    .Append("{USER.ID} - " + Context.User.Id + "\n")
                    .Append("{USER.CREATEDATE} - " + Context.User.UserCreateDate() + "\n")
                    .Append("{USER.JOINDATE} - " + Context.User.GuildJoinDate() + "\n")
                    .Append("---------------\n")
                    .Append("Guild Flags:\n")
                    .Append("{GUILD.NAME} - " + Context.Guild.Name + "\n")
                    .Append("{GUILD.OWNER.USERNAME} - " + Context.Guild.GetOwnerAsync().Result.Username + "\n")
                    .Append("{GUILD.OWNER.MENTION} - @" + Context.Guild.GetOwnerAsync().Result.Username  + "\n")
                    .Append("{GUILD.OWNER.ID} - " + Context.Guild.GetOwnerAsync().Result.Id + "\n")
					.Append("{GUILD.PREFIX} - " + GuildConfiguration.Load(Context.Guild.Id).Prefix + "\n")
					.Append("```");

                await ReplyAsync(sb.ToString());
                return;
            }

            GuildConfiguration.UpdateGuild(Context.Guild.Id, welcomeMessage:message);
            await ReplyAsync("Welcome message has been changed successfully by " + Context.User.Mention + "\n\n**SAMPLE WELCOME MESSAGE**\n" + GuildConfiguration.Load(Context.Guild.Id).WelcomeMessage.ModifyStringFlags(Context.User as SocketGuildUser));
        }

        [Command("welcomechannel"), Summary("Set the welcome channel for the server.")]
        public async Task SetWelcomeChannel(SocketTextChannel channel)
        {
            GuildConfiguration.UpdateGuild(Context.Guild.Id, welcomeChannelId:channel.Id);
            await ReplyAsync(Context.User.Mention + " has updated the Welcome Channel to: " + channel.Mention);
        }

        [Command("logchannel"), Summary("Set the log channel for the server.")]
        public async Task SetLogChannel(SocketTextChannel channel)
        {
            GuildConfiguration.UpdateGuild(Context.Guild.Id, logChannelId: channel.Id);
            await ReplyAsync(Context.User.Mention + " has updated the Log Channel to: " + channel.Mention);
        }

        [Command("togglesenpai"), Summary("Toggles the senpai command.")]
        public async Task ToggleSenpai()
        {
            GuildConfiguration.UpdateGuild(Context.Guild.Id, senpaiEnabled: !GuildConfiguration.Load(Context.Guild.Id).SenpaiEnabled);
            await GuildConfiguration.Load(Context.Guild.Id).LogChannelId.GetTextChannel().SendMessageAsync("Senpai has been toggled by " + Context.User.Mention + " (enabled: " + GuildConfiguration.Load(Context.Guild.Id).SenpaiEnabled + ")");
        }

        [Command("togglequotes"), Summary("")]
        public async Task ToggleQuotes()
        {
            GuildConfiguration.UpdateGuild(Context.Guild.Id, quotesEnabled: !GuildConfiguration.Load(Context.Guild.Id).QuotesEnabled);
            await GuildConfiguration.Load(Context.Guild.Id).LogChannelId.GetTextChannel().SendMessageAsync("Quotes have been toggled by " + Context.User.Mention + " (enabled: " + GuildConfiguration.Load(Context.Guild.Id).QuotesEnabled + ")");
        }

        [Command("togglensfwstatus"), Summary("")]
        public async Task ToggleNsfwStatus()
        {
            GuildConfiguration.UpdateGuild(Context.Guild.Id, enableNsfwCommands: !GuildConfiguration.Load(Context.Guild.Id).EnableNsfwCommands);
            await GuildConfiguration.Load(Context.Guild.Id).LogChannelId.GetTextChannel().SendMessageAsync("NSFW Server Status have been toggled by " + Context.User.Mention + " (NSFW Server? " + GuildConfiguration.Load(Context.Guild.Id).EnableNsfwCommands + ")");
        }

        [Command("rule34channel"), Summary("Set the Rule34 channel for the server.")]
        public async Task SetNsfwRule34Channel(SocketTextChannel channel)
        {
            GuildConfiguration.UpdateGuild(Context.Guild.Id, ruleGameChannelId: channel.Id);
            await ReplyAsync(Context.User.Mention + " has updated the Rule34 Gamble Channel to: " + channel.Mention);
        }

        [Command("toggleawardingcoins"), Summary("Toggle if the channel awards coins for typing messages.")]
        public async Task ToggleChannelCoinStatus(SocketTextChannel channel = null)
        {
            SocketTextChannel workingWithChannel = channel ?? Context.Channel as SocketTextChannel;

            if (channel == null)
            {
                await ReplyAsync("Syntax: " + GuildConfiguration.Load(Context.Guild.Id).Prefix +
                                 "toggleawardingcoins [#channel]");
                return;
            }

            bool value = !Channel.Load(workingWithChannel.Id).AwardingCoins;

            Channel.UpdateChannel(workingWithChannel.Id, awardingCoins: value);

            IUserMessage msg;
            if (value)
            {
                msg = await ReplyAsync(Context.User.Mention + ", this channel is now awarding coins to the messages typed here.");
            }
            else
            {
                msg = await ReplyAsync(Context.User.Mention + ", this channel is no longer awarding coins to the messages typed here.");
            }

            Context.Message.DeleteAfter(15);
            msg.DeleteAfter(15);
        }
    }
}
