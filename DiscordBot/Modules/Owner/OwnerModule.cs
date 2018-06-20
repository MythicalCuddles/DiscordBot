using System;
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
                await ReplyAsync(Context.User.Mention + ", " + MogiiBot3.Bot.CurrentUser.Username + " will start ignoring " + user.Mention);
            }
            else
            {
                await ReplyAsync(Context.User.Mention + ", " + MogiiBot3.Bot.CurrentUser.Username + " will start to listen to " + user.Mention);
            }
        }

        [Command("resetallcoins")]
        public async Task SetCoinsForAll(string confirmation = null)
        {
            if (confirmation == null)
            {
                await ReplyAsync("**Warning**\nIssuing this command will **reset all users coins**. This action is irreversible and any data not backed-up will be lost. Please ensure that you create a backup of the data if you wish to roll-back to the current state. If you wish to issue this command, please enter the TwoAuth code as follows: `" + GuildConfiguration.Load(Context.Guild.Id).Prefix + "resetallcoins [code]`");
                return;
            }

            TwoFactorAuthenticator tfA = new TwoFactorAuthenticator();
            var result = tfA.ValidateTwoFactorPIN(Cryptography.DecryptString(Configuration.Load().SecretKey), confirmation);

            if (result)
            {
                await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync(Context.User.Mention + " has forced all users coins to reset value.");

                User.SetCoinsForAll();

                var eb = new EmbedBuilder()
                    .WithDescription(Context.User.Username + " has successfully reset the coin value for all users.")
                    .WithColor(Color.DarkGreen)
                    .WithCurrentTimestamp()
                    .WithFooter("Info: This command has been logged successfully to " + Configuration.Load().LogChannelId.GetTextChannel().Name);

                await ReplyAsync("", false, eb.Build());
            }
            else
            {
                await ReplyAsync("Invalid Code.");
            }
        }

        [Command("globalmessage")]
        public async Task SendMessageToAllGuilds([Remainder]string message = null)
        {
            if (message == null)
            {
                await ReplyAsync("**Syntax:** " + GuildConfiguration.Load(Context.Guild.Id).Prefix + "globalmessage [message]\n" + 
                                 "This will post an embed message to all guilds. It's main purpose is to inform guild owners of updates and changes.");
                return;
            }
            
            EmbedBuilder eb = new EmbedBuilder()
            {
                Title = "Announcement from " + Context.User.Username,
                Color = new Color(User.Load(Context.User.Id).AboutR, User.Load(Context.User.Id).AboutG, User.Load(Context.User.Id).AboutB),
                Description = message
            }.WithCurrentTimestamp();

            foreach (SocketGuild g in MogiiBot3.Bot.Guilds)
            {
                await GuildConfiguration.Load(g.Id).LogChannelId.GetTextChannel().SendMessageAsync("", false, eb.Build());
            }

            await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("", false, eb.WithFooter("Message sent to all guilds").Build());
        }

        [Command("die")]
        public async Task KillProgram(string confirmation = null)
        {
            if (confirmation == null)
            {
                await ReplyAsync("**Please confirm by entering the TwoAuth code as follows:** " + GuildConfiguration.Load(Context.Guild.Id).Prefix + "die [code]\n" +
                                 "Issuing this command will log the Bot out and terminate the process.");
                return;
            }

            TwoFactorAuthenticator tfA = new TwoFactorAuthenticator();
            var result = tfA.ValidateTwoFactorPIN(Cryptography.DecryptString(Configuration.Load().SecretKey), confirmation);

            if (result)
            {
                Context.Message.DeleteAfter(1);

                EmbedBuilder eb = new EmbedBuilder()
                {
                    Title = "",
                    Color = new Color(User.Load(Context.User.Id).AboutR, User.Load(Context.User.Id).AboutG, User.Load(Context.User.Id).AboutB),
                    Description = ""
                }.AddField("", "");

                await ReplyAsync("", false, eb.Build());

                await MogiiBot3.Bot.LogoutAsync();
                Environment.Exit(0);
            }
            else
            {
                await ReplyAsync("Invalid Code.");
            }
        }
    }
}