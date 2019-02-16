using System.Collections.Generic;
using System.Threading.Tasks;

using Discord.Commands;
using Discord;

using DiscordBot.Common.Preconditions;
using DiscordBot.Common;
using DiscordBot.Database;
using DiscordBot.Extensions;
using DiscordBot.Objects;

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
                             "[ 5] force minecraft [mention/id] [value]\n" +
                             "[ 6] force instagram [mention/id] [value]\n" +
                             "[ 7] force snapchat [mention/id] [value]\n" +
                             "[ 8] force github [mention/id] [value]\n" +
                             "[ 9] force prefix [mention/id] [value]\n" +
                             "[10] force websitename [mention/id] [value]\n" +
                             "[11] force websiteurl [mention/id] [value]\n" +
                             "```");
        }

        [Command("about"), Summary("Force set the about message for the specified user.")]
        public async Task ForceAbout(IUser user, [Remainder]string about)
        {
            if (Context.User.HasHigherPermissionLevel(user))
            {
                //User.UpdateUser(user.Id, about: about);
                
                List<(string, string)> queryParams = new List<(string, string)>()
                {
                    ("@about", about)
                };
                DatabaseActivity.ExecuteNonQueryCommand("UPDATE users SET about=@about WHERE id='" + user.Id + "';", queryParams);

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
                //User.UpdateUser(user.Id, name:name);
                
                List<(string, string)> queryParams = new List<(string, string)>()
                {
                    ("@name", name)
                };
                DatabaseActivity.ExecuteNonQueryCommand("UPDATE users SET name=@name WHERE id='" + user.Id + "';", queryParams);

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
                //User.UpdateUser(user.Id, gender:gender);
                
                List<(string, string)> queryParams = new List<(string, string)>()
                {
                    ("@gender", gender)
                };
                DatabaseActivity.ExecuteNonQueryCommand("UPDATE users SET gender=@gender WHERE id='" + user.Id + "';", queryParams);

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
                //User.UpdateUser(user.Id, pronouns:pronouns);
                
                List<(string, string)> queryParams = new List<(string, string)>()
                {
                    ("@pronouns", pronouns)
                };
                DatabaseActivity.ExecuteNonQueryCommand("UPDATE users SET pronouns=@pronouns WHERE id='" + user.Id + "';", queryParams);

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

        [Command("minecraft"), Summary("Force set the minecraft username for the specified user.")]
        public async Task ForceMinecraftUsername(IUser user, [Remainder]string username)
        {
            if (Context.User.HasHigherPermissionLevel(user))
            {
                //User.UpdateUser(user.Id, minecraftUsername:username);
                
                List<(string, string)> queryParams = new List<(string, string)>()
                {
                    ("@minecraftUsername", username)
                };
                DatabaseActivity.ExecuteNonQueryCommand("UPDATE users SET minecraftUsername=@minecraftUsername WHERE id='" + user.Id + "';", queryParams);

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
                //User.UpdateUser(user.Id, instagram:username);
                
                List<(string, string)> queryParams = new List<(string, string)>()
                {
                    ("@instagramUsername", username)
                };
                DatabaseActivity.ExecuteNonQueryCommand("UPDATE users SET instagramUsername=@instagramUsername WHERE id='" + user.Id + "';", queryParams);

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
                //User.UpdateUser(user.Id, snapchat:username);
                
                List<(string, string)> queryParams = new List<(string, string)>()
                {
                    ("@snapchatUsername", username)
                };
                DatabaseActivity.ExecuteNonQueryCommand("UPDATE users SET snapchatUsername=@snapchatUsername WHERE id='" + user.Id + "';", queryParams);

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
                //User.UpdateUser(user.Id, github:username);
                
                List<(string, string)> queryParams = new List<(string, string)>()
                {
                    ("@githubUsername", username)
                };
                DatabaseActivity.ExecuteNonQueryCommand("UPDATE users SET githubUsername=@githubUsername WHERE id='" + user.Id + "';", queryParams);

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
                //User.UpdateUser(user.Id, customPrefix:prefix);
                
                List<(string, string)> queryParams = new List<(string, string)>()
                {
                    ("@customPrefix", prefix)
                };
                DatabaseActivity.ExecuteNonQueryCommand("UPDATE users SET customPrefix=@customPrefix WHERE id='" + user.Id + "';", queryParams);

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
                //User.UpdateUser(user.Id, websiteUrl: url);
                
                List<(string, string)> queryParams = new List<(string, string)>()
                {
                    ("@websiteUrl", url)
                };
                DatabaseActivity.ExecuteNonQueryCommand("UPDATE users SET websiteURL=@websiteUrl WHERE id='" + user.Id + "';", queryParams);

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
                //User.UpdateUser(user.Id, websiteName: name);
                
                List<(string, string)> queryParams = new List<(string, string)>()
                {
                    ("@websiteName", name)
                };
                DatabaseActivity.ExecuteNonQueryCommand("UPDATE users SET websiteName=@websiteName WHERE id='" + user.Id + "';", queryParams);

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
