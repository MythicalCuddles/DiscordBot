using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                DatabaseActivity.ExecuteNonQueryCommand("UPDATE users SET teamMember='Y' WHERE id='" + user.Id + "';");

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
                DatabaseActivity.ExecuteNonQueryCommand("UPDATE users SET teamMember='N' WHERE id='" + user.Id + "';");

                await ReplyAsync(user.Mention + " has been removed from the team by " + Context.User.Mention);
            }
            else
            {
                await ReplyAsync(user.Mention + " is not part of the team, " + Context.User.Mention + "!");
            }
        }

        [Command("editfooter")]
        public async Task EditFooter(IUser user, [Remainder] string footer)
        {
            if (user == null || footer == null)
            {
                await ReplyAsync("**Syntax:** " + Guild.Load(Context.Guild.Id).Prefix + "editfooter [@User] [Footer]");
                return;
            }

            List<(string, string)> queryParams = new List<(string, string)>()
            {
                ("@footerText", footer)
            };
            DatabaseActivity.ExecuteNonQueryCommand(
                "UPDATE users SET footerText=@footerText WHERE id='" + user.Id + "';", queryParams);

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
                await ReplyAsync("**Syntax:** " + Guild.Load(Context.Guild.Id).Prefix +
                                 "editiconurl [@User] [Author/Footer] [Link to Icon/Image]");
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

                    List<(string, string)> authorQueryParams = new List<(string, string)>()
                    {
                        ("@embedAuthorBuilderIconUrl", newLink)
                    };
                    DatabaseActivity.ExecuteNonQueryCommand(
                        "UPDATE users SET authorIconURL=@embedAuthorBuilderIconUrl WHERE id='" + user.Id + "';",
                        authorQueryParams);

                    eb.WithColor(Color.DarkGreen);
                    eb.WithDescription(Context.User.Username + " successfully updated " + user.Mention +
                                       "'s Author Icon to: " + url);
                    eb.WithFooter("Old Link: " + oldLink);
                    await ReplyAsync("", false, eb.Build());
                    break;
                case "FOOTER":
                    oldLink = User.Load(user.Id).EmbedAuthorBuilderIconUrl;

                    List<(string, string)> footerQueryParams = new List<(string, string)>()
                    {
                        ("@embedFooterBuilderIconUrl", newLink)
                    };
                    DatabaseActivity.ExecuteNonQueryCommand(
                        "UPDATE users SET footerIconURL=@embedFooterBuilderIconUrl WHERE id='" + user.Id + "';",
                        footerQueryParams);

                    eb.WithColor(Color.DarkGreen);
                    eb.WithDescription(Context.User.Username + " successfully updated " + user.Mention +
                                       "'s Footer Icon to: " + url);
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
            char ignoreUser = (!User.Load(user.Id).IsBotIgnoringUser).ToYesNo()[0];
            List<(string, string)> queryParams = new List<(string, string)>()
            {
                ("@botIgnoringUser", ignoreUser.ToString())
            };
            DatabaseActivity.ExecuteNonQueryCommand(
                "UPDATE users SET isBeingIgnored=@botIgnoringUser WHERE id='" + user.Id + "';", queryParams);

            if (User.Load(user.Id).IsBotIgnoringUser)
            {
                await ReplyAsync(Context.User.Mention + ", " + DiscordBot.Bot.CurrentUser.Username +
                                 " will start ignoring " + user.Mention);
            }
            else
            {
                await ReplyAsync(Context.User.Mention + ", " + DiscordBot.Bot.CurrentUser.Username +
                                 " will start to listen to " + user.Mention);
            }
        }

        [Command("die")]
        public async Task KillProgram(string confirmation = null)
        {
            if (confirmation.ToUpper() != "CONFIRM")
            {
                await ReplyAsync("**Please confirm by entering the TwoAuth code as follows:** " +
                                 Guild.Load(Context.Guild.Id).Prefix + "die confirm\n" +
                                 "Issuing this command will log the Bot out and terminate the process.");
                return;
            }

            Context.Message.DeleteAfter(1);

            EmbedBuilder eb = new EmbedBuilder()
            {
                Title = "",
                Color = new Color(User.Load(Context.User.Id).AboutR, User.Load(Context.User.Id).AboutG,
                    User.Load(Context.User.Id).AboutB),
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
                                 Guild.Load(Context.Guild.Id).Prefix + "addreaction [message id] [emote]");
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
                            await ReplyAsync(Context.User.Mention +
                                             ", if that message exists in my cache, I've added a reaction to it.");
                            return;
                        }
                    }
                }
            }
        }
    }
}