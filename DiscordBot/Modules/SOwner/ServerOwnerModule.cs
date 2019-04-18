using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using DiscordBot.Common;
using DiscordBot.Common.Preconditions;
using DiscordBot.Database;
using DiscordBot.Extensions;
using DiscordBot.Objects;
using MelissaNet;

namespace DiscordBot.Modules.SOwner
{
    [MinPermissions(PermissionLevel.ServerOwner)]
    [Name("Server Owner Commands")]
    public class ServerOwnerModule : ModuleBase
    {
        [Command("guildprefix"), Summary("Set the prefix for the bot for the guild.")]
        public async Task SetPrefix(string prefix)
        {
            //GuildConfiguration.UpdateGuild(Context.Guild.Id, prefix:prefix);
            
            Guild.UpdateGuild(Context.Guild.Id, prefix:prefix);
            
            await ReplyAsync(Context.User.Mention + " has updated the Prefix to: " + prefix);
        }

        [Command("setwelcome"), Summary("Set the welcome message to the specified string.")]
        [Alias("setwelcomemessage", "sw")]
        public async Task SetWelcomeMessage([Remainder]string message = null)
        {
            if (message == null)
            {
                StringBuilder sb = new StringBuilder()
                    .Append("**Syntax:** " + Guild.Load(Context.Guild.Id).Prefix + "setwelcome [Welcome Message]\n\n")
                    .Append("```Available Flags\n")
                    .Append("---------------\n")
                    .Append("User Flags:\n")
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
					.Append("{GUILD.PREFIX} - " + Guild.Load(Context.Guild.Id).Prefix + "\n")
					.Append("```");

                await ReplyAsync(sb.ToString());
                return;
            }

            //GuildConfiguration.UpdateGuild(Context.Guild.Id, welcomeMessage:message);
            Guild.UpdateGuild(Context.Guild.Id, welcomeMessage:message);
            await ReplyAsync("Welcome message has been changed successfully by " + Context.User.Mention + "\n\n**SAMPLE WELCOME MESSAGE**\n" + Guild.Load(Context.Guild.Id).WelcomeMessage.ModifyStringFlags(Context.User as SocketGuildUser));
        }

        [Command("welcomechannel"), Summary("Set the welcome channel for the server.")]
        public async Task SetWelcomeChannel(SocketTextChannel channel)
        {
            //GuildConfiguration.UpdateGuild(Context.Guild.Id, welcomeChannelId:channel.Id);
            Guild.UpdateGuild(Context.Guild.Id, welcomeChannelID:channel.Id);
            await ReplyAsync(Context.User.Mention + " has updated the Welcome Channel to: " + channel.Mention);
        }

        [Command("logchannel"), Summary("Set the log channel for the server.")]
        public async Task SetLogChannel(SocketTextChannel channel)
        {
            //GuildConfiguration.UpdateGuild(Context.Guild.Id, logChannelId: channel.Id);
            Guild.UpdateGuild(Context.Guild.Id, logChannelID:channel.Id);
            await ReplyAsync(Context.User.Mention + " has updated the Log Channel to: " + channel.Mention);
        }

        [Command("botchannel"), Summary("Set the bot channel for the server.")]
        public async Task SetBotChannel(SocketTextChannel channel)
        {
            //GuildConfiguration.UpdateGuild(Context.Guild.Id, botChannelId: channel.Id);
            Guild.UpdateGuild(Context.Guild.Id, botChannelID:channel.Id);
            await ReplyAsync(Context.User.Mention + " has updated the Bot Channel to: " + channel.Mention);
        }

        [Command("togglesenpai"), Summary("Toggles the senpai command.")]
        public async Task ToggleSenpai()
        {
            //GuildConfiguration.UpdateGuild(Context.Guild.Id, senpaiEnabled: !Guild.Load(Context.Guild.Id).SenpaiEnabled);
            Guild.UpdateGuild(Context.Guild.Id, senpaiEnabled: (!Guild.Load(Context.Guild.Id).SenpaiEnabled));
            await Guild.Load(Context.Guild.Id).LogChannelID.GetTextChannel().SendMessageAsync("Senpai has been toggled by " + Context.User.Mention + " (enabled: " + Guild.Load(Context.Guild.Id).SenpaiEnabled + ")");
        }

        [Command("togglequotes"), Summary("")]
        public async Task ToggleQuotes()
        {
            //GuildConfiguration.UpdateGuild(Context.Guild.Id, quotesEnabled: !Guild.Load(Context.Guild.Id).QuotesEnabled);
            Guild.UpdateGuild(Context.Guild.Id, quotesEnabled: (!Guild.Load(Context.Guild.Id).QuotesEnabled));
            await Guild.Load(Context.Guild.Id).LogChannelID.GetTextChannel().SendMessageAsync("Quotes have been toggled by " + Context.User.Mention + " (enabled: " + Guild.Load(Context.Guild.Id).QuotesEnabled + ")");
        }

        [Command("togglensfwstatus"), Summary("")]
        public async Task ToggleNsfwStatus()
        {
            //GuildConfiguration.UpdateGuild(Context.Guild.Id, enableNsfwCommands: !Guild.Load(Context.Guild.Id).NSFWCommandsEnabled);
            Guild.UpdateGuild(Context.Guild.Id, nsfwCommandsEnabled: (!Guild.Load(Context.Guild.Id).NSFWCommandsEnabled));
            await Guild.Load(Context.Guild.Id).LogChannelID.GetTextChannel().SendMessageAsync("NSFW Server Status have been toggled by " + Context.User.Mention + " (NSFW Server? " + Guild.Load(Context.Guild.Id).NSFWCommandsEnabled + ")");
        }

        [Command("rule34channel"), Summary("Set the Rule34 channel for the server.")]
        public async Task SetNsfwRule34Channel(SocketTextChannel channel)
        {
            //GuildConfiguration.UpdateGuild(Context.Guild.Id, ruleGameChannelId: channel.Id);
            Guild.UpdateGuild(Context.Guild.Id, ruleGambleChannelID: channel.Id);
            await ReplyAsync(Context.User.Mention + " has updated the Rule34 Gamble Channel to: " + channel.Mention);
        }
        
        [Command("toggleexpawarding"), Summary("Toggle if the channel awards EXP for posted messages.")]
        public async Task ToggleChannelAwardingEXPStatus(SocketTextChannel channel = null)
        {
            SocketTextChannel workingWithChannel = channel ?? Context.Channel as SocketTextChannel;

            if (channel == null)
            {
                await ReplyAsync("Syntax: " + Guild.Load(Context.Guild.Id).Prefix +
                                 "toggleexpawarding [#channel]");
                return;
            }

            bool value = !Channel.Load(workingWithChannel.Id).AwardingEXP;
                
            List<(string, string)> queryParams = new List<(string, string)>()
            {
                ("@awardingEXP", value.ToOneOrZero().ToString())
            };
            DatabaseActivity.ExecuteNonQueryCommand("UPDATE channels SET awardingEXP=@awardingEXP WHERE channelID='" + workingWithChannel.Id + "';", queryParams);

            //Channel.UpdateChannel(workingWithChannel.Id, awardingEXP: value);

            IUserMessage msg;
            if (value)
            {
                msg = await ReplyAsync(Context.User.Mention + ", this channel will award EXP to the users who's messages are typed here.");
            }
            else
            {
                msg = await ReplyAsync(Context.User.Mention + ", this channel will no longer award EXP to the users who's messages are typed here.");
            }

            Context.Message.DeleteAfter(15);
            msg.DeleteAfter(15);
        }
    }
}
