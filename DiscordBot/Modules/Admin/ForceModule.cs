using System.Threading.Tasks;

using Discord.Commands;
using Discord;

using DiscordBot.Common.Preconditions;
using DiscordBot.Common;
using DiscordBot.Extensions;

namespace DiscordBot.Modules.Admin
{
    [Name("Force Commands")]
    [Group("force")]
    [RequireContext(ContextType.Guild)]
    [MinPermissions(PermissionLevel.ServerAdmin)]
    public class ForceModule : ModuleBase
    {
        [Command(""), Summary("Get the available options for the force command.")]
        public async Task Force()
        {
            await ReplyAsync("**Syntax:** " +
                GuildConfiguration.Load(Context.Guild.Id).Prefix + "force [command] [user mention / id] [value]\n```INI\n" +
                             "Available Commands\n" +
                             "------------------\n" +
                             "[ 1] force about [mention/id] [value]\n" +
                             "[ 2] force name [mention/id] [value]\n" +
                             "[ 3] force gender [mention/id] [value]\n" +
                             "[ 4] force pronouns [mention/id] [value]\n" +
                             "[ 5] force coins [mention/id] [value]\n" +
                             "[ 6] force mythicaltokens [mention/id] [value]\n" +
                             "[ 7] force minecraftusername [mention/id] [value]\n" +
                             "[ 8] force instagram [mention/id] [value]\n" +
                             "[ 9] force snapchat [mention/id] [value]\n" +
                             "[10] force github [mention/id] [value]\n" +
                             "[11] force prefix [mention/id] [value]\n" +
                             "[12] force websitename [mention/id] [value]\n" +
                             "[13] force websiteurl [mention/id] [value]\n" +
                             "```");
        }

        [Command("about"), Summary("Force set the about message for the specified user.")]
        public async Task ForceAbout(IUser user, [Remainder]string about)
        {
            if (Context.User.HasHigherPermissionLevel(user))
            {
                User.UpdateUser(user.Id, about: about);

                var eb = new EmbedBuilder()
                    .WithDescription(Context.User.Username + " changed " + user.Mention + "'s about text successfully.")
                    .WithColor(Color.DarkGreen);

                await ReplyAsync("", false, eb.Build());
            }
            else
            {
                await ReplyAsync(Context.User.Mention + ", you don't have a high enough permission level to do this to that user!");
            }
        }

        [Command("name"), Summary("Force set the name message for the specified user.")]
        public async Task ForceName(IUser user, [Remainder]string name)
        {
            if (Context.User.HasHigherPermissionLevel(user))
            {
                User.UpdateUser(user.Id, name:name);

                var eb = new EmbedBuilder()
                    .WithDescription(Context.User.Username + " changed " + user.Mention + "'s name text successfully.")
                    .WithColor(Color.DarkGreen);

                await ReplyAsync("", false, eb.Build());
            }
            else
            {
                await ReplyAsync(Context.User.Mention + ", you don't have a high enough permission level to do this to that user!");
            }
        }

        [Command("gender"), Summary("Force set the gender message for the specified user.")]
        public async Task ForceGender(IUser user, [Remainder]string gender)
        {
            if (Context.User.HasHigherPermissionLevel(user))
            {
                User.UpdateUser(user.Id, gender:gender);

                var eb = new EmbedBuilder()
                    .WithDescription(Context.User.Username + " changed " + user.Mention + "'s gender text successfully.")
                    .WithColor(Color.DarkGreen);

                await ReplyAsync("", false, eb.Build());
            }
            else
            {
                await ReplyAsync(Context.User.Mention + ", you don't have a high enough permission level to do this to that user!");
            }
        }

        [Command("pronouns"), Summary("Force set the pronouns message for the specified user.")]
        public async Task ForcePronouns(IUser user, [Remainder]string pronouns)
        {
            if (Context.User.HasHigherPermissionLevel(user))
            {
                User.UpdateUser(user.Id, pronouns:pronouns);

                var eb = new EmbedBuilder()
                    .WithDescription(Context.User.Username + " changed " + user.Mention + "'s pronoun text successfully.")
                    .WithColor(Color.DarkGreen);

                await ReplyAsync("", false, eb.Build());
            }
            else
            {
                await ReplyAsync(Context.User.Mention + ", you don't have a high enough permission level to do this to that user!");
            }
        }

        [Command("coins"), Summary("Force set the coins for the specified user.")]
        public async Task ForceCoins(IUser user, int newValue)
        {
            if (Context.User.HasHigherPermissionLevel(user))
            {
                int oldCoins = User.Load(user.Id).Coins;
                User.UpdateUser(user.Id, coins: newValue);

                var eb = new EmbedBuilder()
                    .WithDescription(Context.User.Username + " changed " + user.Mention + "'s coins successfully.")
                    .WithFooter("Was: " + oldCoins + " | Note: Your action has been logged!")
                    .WithColor(Color.DarkGreen);

                await ReplyAsync("", false, eb.Build());
                await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("", false, eb.Build());
            }
            else
            {
                await ReplyAsync(Context.User.Mention + ", you don't have a high enough permission level to do this to that user!");
            }
        }

        [Command("mythicaltokens"), Summary("Force set the Mythical Tokens for the specified user.")]
        public async Task ForceMythicalTokens(IUser user, int newValue)
        {
            if (Context.User.HasHigherPermissionLevel(user))
            {
                int old = User.Load(user.Id).MythicalTokens;
                User.UpdateUser(user.Id, mythicalTokens: newValue);

                var eb = new EmbedBuilder()
                    .WithDescription(Context.User.Username + " changed " + user.Mention + "'s Mythical Tokens successfully.")
                    .WithFooter("Was: " + old + " | Note: Your action has been logged!")
                    .WithColor(Color.DarkGreen);

                await ReplyAsync("", false, eb.Build());
                await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("", false, eb.Build());
            }
            else
            {
                await ReplyAsync(Context.User.Mention + ", you don't have a high enough permission level to do this to that user!");
            }
        }

        [Command("minecraftusername"), Summary("Force set the minecraft username for the specified user.")]
        public async Task ForceMinecraftUsername(IUser user, [Remainder]string username)
        {
            if (Context.User.HasHigherPermissionLevel(user))
            {
                User.UpdateUser(user.Id, minecraftUsername:username);

                var eb = new EmbedBuilder()
                    .WithDescription(Context.User.Username + " changed " + user.Mention + "'s Minecraft username text successfully.")
                    .WithColor(Color.DarkGreen);

                await ReplyAsync("", false, eb.Build());
            }
            else
            {
                await ReplyAsync(Context.User.Mention + ", you don't have a high enough permission level to do this to that user!");
            }
        }

        [Command("instagram"), Summary("")]
        public async Task ForceInstagramUsername(IUser user, [Remainder]string username)
        {
            if (Context.User.HasHigherPermissionLevel(user))
            {
                User.UpdateUser(user.Id, instagram:username);

                var eb = new EmbedBuilder()
                    .WithDescription(Context.User.Username + " changed " + user.Mention + "'s Instagram username text successfully.")
                    .WithColor(Color.DarkGreen);

                await ReplyAsync("", false, eb.Build());
            }
            else
            {
                await ReplyAsync(Context.User.Mention + ", you don't have a high enough permission level to do this to that user!");
            }
        }

        [Command("snapchat"), Summary("")]
        public async Task ForceSnapchatUsername(IUser user, [Remainder]string username)
        {
            if (Context.User.HasHigherPermissionLevel(user))
            {
                User.UpdateUser(user.Id, snapchat:username);

                var eb = new EmbedBuilder()
                    .WithDescription(Context.User.Username + " changed " + user.Mention + "'s Snapchat username text successfully.")
                    .WithColor(Color.DarkGreen);

                await ReplyAsync("", false, eb.Build());
            }
            else
            {
                await ReplyAsync(Context.User.Mention + ", you don't have a high enough permission level to do this to that user!");
            }
        }

        [Command("github"), Summary("")]
        public async Task ForceGithubUsername(IUser user, [Remainder]string username)
        {
            if (Context.User.HasHigherPermissionLevel(user))
            {
                User.UpdateUser(user.Id, github:username);

                var eb = new EmbedBuilder()
                    .WithDescription(Context.User.Username + " changed " + user.Mention + "'s GitHub username text successfully.")
                    .WithColor(Color.DarkGreen);

                await ReplyAsync("", false, eb.Build());
            }
            else
            {
                await ReplyAsync(Context.User.Mention + ", you don't have a high enough permission level to do this to that user!");
            }
        }

        [Command("prefix"), Summary("")]
        public async Task ForcePrefix(IUser user, [Remainder]string prefix)
        {
            if (Context.User.HasHigherPermissionLevel(user))
            {
                User.UpdateUser(user.Id, customPrefix:prefix);

                var eb = new EmbedBuilder()
                    .WithDescription(Context.User.Username + " changed " + user.Mention + "'s custom prefix successfully.")
                    .WithColor(Color.DarkGreen);

                await ReplyAsync("", false, eb.Build());
            }
            else
            {
                await ReplyAsync(Context.User.Mention + ", you don't have a high enough permission level to do this to that user!");
            }
        }

        [Command("websiteurl"), Summary("")]
        public async Task ForceWebsiteUrl(IUser user, [Remainder]string url)
        {
            if (Context.User.HasHigherPermissionLevel(user))
            {
                User.UpdateUser(user.Id, websiteUrl: url);

                var eb = new EmbedBuilder()
                    .WithDescription(Context.User.Username + " changed " + user.Mention + "'s website URL successfully.")
                    .WithColor(Color.DarkGreen);

                await ReplyAsync("", false, eb.Build());
            }
            else
            {
                await ReplyAsync(Context.User.Mention + ", you don't have a high enough permission level to do this to that user!");
            }
        }

        [Command("websitename"), Summary("")]
        public async Task ForceWebsiteName(IUser user, [Remainder]string name)
        {
            if (Context.User.HasHigherPermissionLevel(user))
            {
                User.UpdateUser(user.Id, websiteName: name);

                var eb = new EmbedBuilder()
                    .WithDescription(Context.User.Username + " changed " + user.Mention + "'s website name successfully.")
                    .WithColor(Color.DarkGreen);

                await ReplyAsync("", false, eb.Build());
            }
            else
            {
                await ReplyAsync(Context.User.Mention + ", you don't have a high enough permission level to do this to that user!");
            }
        }
    }
}
