using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using DiscordBot.Common;
using DiscordBot.Common.Preconditions;
using DiscordBot.Extensions;

using Google.Authenticator;

using MelissaNet;

namespace DiscordBot.Modules.Owner
{
    [Name("Owner Commands")]
    [MinPermissions(PermissionLevel.BotOwner)]
    public class OwnerModule : ModuleBase
    {
        [Command("addteammember"), Summary("Add a member to the team. Gives them a star on $about")]
        public async Task AddTeamMember(IUser user)
        {
            if (!User.Load(user.Id).TeamMember)
            {
                User.UpdateUser(user.Id, teamMember:true);
                await ReplyAsync(user.Mention + " has been added to the team by " + Context.User.Mention);
            }
            else
            {
                await ReplyAsync(user.Mention + " is already part of the team, " + Context.User.Mention + "!");
            }
        }

        [Command("removeteammember"), Summary("Add a member to the team. Gives them a star on $about")]
        public async Task RemoveTeamMember(IUser user)
        {
            if (User.Load(user.Id).TeamMember)
            {
                User.UpdateUser(user.Id, teamMember:false);
                await ReplyAsync(user.Mention + " has been removed from the team by " + Context.User.Mention);
            }
            else
            {
                await ReplyAsync(user.Mention + " is not part of the team, " + Context.User.Mention + "!");
            }
        }

        [Command("editfooter")]
        public async Task EditFooter(IUser user, [Remainder]string footer)
        {
            if(user == null || footer == null)
            {
                await ReplyAsync("**Syntax:** " + GuildConfiguration.Load(Context.Guild.Id).Prefix + "editfooter [@User] [Footer]");
                return;
            }
            
            User.UpdateUser(user.Id, footerText:footer);

            var eb = new EmbedBuilder()
                .WithDescription(Context.User.Username + " updated " + user.Mention + "'s footer successfully.")
                .WithColor(Color.DarkGreen);
            var message = await ReplyAsync("", false, eb.Build());

            Context.Message.DeleteAfter(10);
            message.DeleteAfter(10);
        }

        [Command("editiconurl")]
        public async Task EditIconUrl(IUser user = null, string position = null, string url = null)
        {
            if (user == null || position == null)
            {
                await ReplyAsync("**Syntax:** " + GuildConfiguration.Load(Context.Guild.Id).Prefix + "editiconurl [@User] [Author/Footer] [Link to Icon/Image]");
                return;
            }

            var eb = new EmbedBuilder();
            string oldLink, newLink = url;

            if (url.Contains('<') && url.Contains('>'))
            {
                newLink = url.FindAndReplaceFirstInstance("<", "");
                newLink = newLink.FindAndReplaceFirstInstance(">", "");
            }

            switch (position.ToUpper())
            {
                case "AUTHOR":
                    oldLink = User.Load(user.Id).EmbedAuthorBuilderIconUrl;
                    User.UpdateUser(user.Id, embedAuthorBuilderIconUrl:newLink);
                    eb.WithColor(Color.DarkGreen);
                    eb.WithDescription(Context.User.Username + " successfully updated " + user.Mention + "'s Author Icon to: " + url);
                    eb.WithFooter("Old Link: " + oldLink);
                    await ReplyAsync("", false, eb.Build());
                    break;
                case "FOOTER":
                    oldLink = User.Load(user.Id).EmbedAuthorBuilderIconUrl;
                    User.UpdateUser(user.Id, embedFooterBuilderIconUrl:newLink);
                    eb.WithColor(Color.DarkGreen);
                    eb.WithDescription(Context.User.Username + " successfully updated " + user.Mention + "'s Footer Icon to: " + url);
                    eb.WithFooter("Old Link: " + oldLink);
                    await ReplyAsync("", false, eb.Build());
                    break;
                default:
                    eb.WithColor(Color.DarkGreen);
                    eb.WithDescription("");
                    await ReplyAsync("", false, eb.Build());
                    break;
            }
        }

        [Command("botignore"), Summary("Make the bot ignore a user.")]
        public async Task BotIgnore(IUser user)
        {
            User.UpdateUser(user.Id, isBotIgnoringUser:!User.Load(user.Id).IsBotIgnoringUser);

            if(User.Load(user.Id).IsBotIgnoringUser)
            {
                await ReplyAsync(Context.User.Mention + ", " + DiscordBot.Bot.CurrentUser.Username + " will start ignoring " + user.Mention);
            }
            else
            {
                await ReplyAsync(Context.User.Mention + ", " + DiscordBot.Bot.CurrentUser.Username + " will start to listen to " + user.Mention);
            }
        }

        [Command("die")]
        public async Task KillProgram(string confirmation = null)
        {
            if (confirmation.ToUpper() != "CONFIRM")
            {
                await ReplyAsync("**Please confirm by entering the TwoAuth code as follows:** " + GuildConfiguration.Load(Context.Guild.Id).Prefix + "die confirm\n" +
                                 "Issuing this command will log the Bot out and terminate the process.");
                return;
            }

            Context.Message.DeleteAfter(1);

            EmbedBuilder eb = new EmbedBuilder()
            {
                Title = "",
                Color = new Color(User.Load(Context.User.Id).AboutR, User.Load(Context.User.Id).AboutG, User.Load(Context.User.Id).AboutB),
                Description = ""
            }.AddField("", "");

            await ReplyAsync("", false, eb.Build());

            await DiscordBot.Bot.LogoutAsync();
            Environment.Exit(0);
        }

        [Command("addreaction")]
        public async Task AddReactionAsync(ulong? id = null, string emote = null)
        {
            if (id == null || emote == null)
            {
                await ReplyAsync("**Syntax:** " +
                                 GuildConfiguration.Load(Context.Guild.Id).Prefix + "addreaction [message id] [emote]");
                return;
            }

            foreach (var g in DiscordBot.Bot.Guilds)
            {
                foreach (var c in g.TextChannels)
                {
                    var msgs = c.GetMessagesAsync().GetEnumerator().Current;
                    
                    foreach (var m in msgs)
                    {
                        var msg = c.GetMessageAsync(m.Id).GetAwaiter().GetResult() as SocketUserMessage;

                        if (msg.Id == id)
                        {
                            await msg.AddReactionAsync(new Emoji(emote));
                            await ReplyAsync(Context.User.Mention + ", if that message exists in my cache, I've added a reaction to it.");
                            return;
                        }
                    }
                }
            }
        }
    }
}