using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using DiscordBot.Common;
using DiscordBot.Common.Preconditions;
using DiscordBot.Database;
using DiscordBot.Extensions;
using DiscordBot.Handlers;
using DiscordBot.Objects;
using MySql.Data.MySqlClient;

namespace DiscordBot.Modules.Public
{
    [MinPermissions(PermissionLevel.User)]
    [RequireContext(ContextType.Guild)]
    [Name("Profile Commands")]
    public class ProfileModule : ModuleBase
    {
        [Group("editprofile")]
        public class EditProfileModule : ModuleBase
        {
            [Command("")]
            public async Task SendSyntax()
            {
                await ReplyAsync("**Syntax:** " +
                                 GuildConfiguration.Load(Context.Guild.Id).Prefix + "editprofile [command] [value]\n```INI\n" +
                                 "Available Commands\n" +
                                 "-----------------------------\n" +
                                 "[ 1] editprofile name [value]\n" +
                                 "[ 2] editprofile gender [value]\n" +
                                 "[ 3] editprofile pronouns [value]\n" +
                                 "[ 4] editprofile about [value]\n" +
                                 "[ 5] editprofile customprefix [value]\n" +
                                 "[5a] This feature requires level " + Configuration.Load().PrefixLevelRequirement + "!\n" +
                                 "[ 6] editprofile customrgb [value]\n" +
                                 "[5a] This feature requires level " + Configuration.Load().RGBLevelRequirement + "!\n" +
                                 "[ 7] editprofile mcusername [value]\n" +
                                 "[ 8] editprofile snapchat [value]\n" +
                                 "[ 9] editprofile instagram [value]\n" +
                                 "[10] editprofile github [value]\n" +
                                 "[11] editprofile websitename [value]\n" +
                                 "[12] editprofile websiteurl [value]\n" +
                                 "```");
            }
            
            #region User Sets
            [Command("name"), Summary("")]
            public async Task SetName([Remainder]string name)
            {
                //User.UpdateUser(Context.User.Id, name: name);
                
                List<(string, string)> queryParams = new List<(string, string)>()
                {
                    ("@name", name)
                };
                DatabaseActivity.ExecuteNonQueryCommand("UPDATE users SET name=@name WHERE id='" + Context.User.Id + "';", queryParams);
                
                await ReplyAsync(Context.User.Mention + ", you have successfully updated the name on your profile to \"" + name + "\"!");
            }
    
            [Command("gender"), Summary("")]
            public async Task SetGender([Remainder]string gender)
            {
                //User.UpdateUser(Context.User.Id, gender: gender);
                
                List<(string, string)> queryParams = new List<(string, string)>()
                {
                    ("@gender", gender)
                };
                DatabaseActivity.ExecuteNonQueryCommand("UPDATE users SET gender=@gender WHERE id='" + Context.User.Id + "';", queryParams);
                
                await ReplyAsync(Context.User.Mention + ", you have successfully updated the gender on your profile to \"" + gender + "\"!");
            }
    
            [Command("pronouns"), Summary("Set your pronouns!")]
            public async Task SetUserPronouns([Remainder]string pronouns)
            {
                //User.UpdateUser(Context.User.Id, pronouns: pronouns);
                
                List<(string, string)> queryParams = new List<(string, string)>()
                {
                    ("@pronouns", pronouns)
                };
                DatabaseActivity.ExecuteNonQueryCommand("UPDATE users SET pronouns=@pronouns WHERE id='" + Context.User.Id + "';", queryParams);

                await ReplyAsync(Context.User.Mention + ", you have successfully updated the pronouns on your profile to \"" + pronouns + "\"!");
            }
    
            [Command("about"), Summary("Set your about message!")]
            public async Task SetUserAbout([Remainder]string aboutMessage)
            {
                //User.UpdateUser(Context.User.Id, about: aboutMessage);
                
                List<(string, string)> queryParams = new List<(string, string)>()
                {
                    ("@aboutMessage", aboutMessage)
                };
                DatabaseActivity.ExecuteNonQueryCommand("UPDATE users SET about=@aboutMessage WHERE id='" + Context.User.Id + "';", queryParams);

                await ReplyAsync(Context.User.Mention + ", you have successfully updated the about section on your profile!");
            }
    
            [Command("customprefix"), Summary("Custom set the prefix for the user.")]
            public async Task CustomUserPrefix([Remainder]string prefix = null)
            {
                if (prefix == null)
                {
                    await ReplyAsync("**Syntax:** " + GuildConfiguration.Load(Context.Guild.Id).Prefix + "editprofile customprefix [prefix]\n\n`This feature requires you to be level " + Configuration.Load().PrefixLevelRequirement + "!`");
                    return;
                }
                
                if (Context.User.GetLevel() >= Configuration.Load().PrefixLevelRequirement)
                {
                    //User.UpdateUser(Context.User.Id, customPrefix: prefix);
                
                    List<(string, string)> queryParams = new List<(string, string)>()
                    {
                        ("@customPrefix", prefix)
                    };
                    DatabaseActivity.ExecuteNonQueryCommand("UPDATE users SET customPrefix=@customPrefix WHERE id='" + Context.User.Id + "';", queryParams);

                    await ReplyAsync(Context.User.Mention + ", you have set `" + prefix + "` as a custom prefix for yourself. Please do take note that the following prefixes will work for you:\n```KEY: [Prefix][Command]\n" + prefix + " - User Set Prefix\n" + GuildConfiguration.Load(Context.Guild.Id).Prefix + " - Guild Set Prefix\n@" + DiscordBot.Bot.CurrentUser.Username + " - Global Prefix```");
                }
                else
                {
                    await ReplyAsync(Context.User.Mention + ", this feature requires you to be level " + Configuration.Load().PrefixLevelRequirement + "!");
                }
            }
    
            [Command("customrgb"), Summary("Custom set the color of the about embed message.")]
            public async Task SetUserRgb(int r = -1, int g = -1, int b = -1)
            {
                if (r < 0 || r > 255 || g < 0 || g > 255 || b < 0 || b > 255)
                {
                    await ReplyAsync(Context.User.Mention + ", you have entered an invalid value. You can use this website to help get your RGB values - <http://www.colorhexa.com/>\n\n" +
                        "**Syntax:** " + GuildConfiguration.Load(Context.Guild.Id).Prefix + "editprofile customrgb [R value] [G value] [B value]\n**Example:** " + GuildConfiguration.Load(Context.Guild.Id).Prefix + "setcustomrgb 140 90 210");
                    return;
                }

                if (Context.User.GetLevel() >= Configuration.Load().RGBLevelRequirement)
                {
                    byte rValue, gValue, bValue;
    
                    try
                    {
                        byte.TryParse(r.ToString(), out rValue);
                        byte.TryParse(g.ToString(), out gValue);
                        byte.TryParse(b.ToString(), out bValue);
                    }
                    catch (Exception e)
                    {
                        ConsoleHandler.PrintExceptionToLog("ProfileModule/CustomRGB", e);
                        await ReplyAsync("An unexpected error has happened. Please ensure that you have passed through a byte value! (A number between 0 and 255)");
                        return;
                    }
    
                    //User.UpdateUser(Context.User.Id, aboutR: rValue);
                    //User.UpdateUser(Context.User.Id, aboutG: gValue);
                    //User.UpdateUser(Context.User.Id, aboutB: bValue);
                
                    List<(string, string)> queryParams = new List<(string, string)>()
                    {
                        ("@aboutR", rValue.ToString()),
                        ("@aboutG", gValue.ToString()),
                        ("@aboutB", bValue.ToString())
                    };
                    DatabaseActivity.ExecuteNonQueryCommand("UPDATE users SET aboutR=@aboutR, aboutG=@aboutG, aboutB=@aboutB WHERE id='" + Context.User.Id + "';", queryParams);
    
                    Color aboutColor = new Color(User.Load(Context.User.Id).AboutR, User.Load(Context.User.Id).AboutG, User.Load(Context.User.Id).AboutB);
    
                    EmbedBuilder eb = new EmbedBuilder()
                        .WithTitle("Sample Message")
                        .WithDescription("<-- FYI, this is what you updated.")
                        .WithColor(aboutColor);
                    
                    await ReplyAsync(Context.User.Mention + ", you have successfully updated your custom embed RGB!", false, eb.Build());
                }
                else
                {
                    await ReplyAsync(Context.User.Mention + ", this feature requires you to be level " + Configuration.Load().RGBLevelRequirement + "!");
                }
            }
    
            [Command("mcusername"), Summary("")]
            [Alias("minecraftusername", "mcreg", "mcregister")]
            public async Task SetMinecraftUsername([Remainder]string username)
            {
                //User.UpdateUser(Context.User.Id, minecraftUsername:username);
                
                List<(string, string)> queryParams = new List<(string, string)>()
                {
                    ("@minecraft", username)
                };
                DatabaseActivity.ExecuteNonQueryCommand("UPDATE users SET minecraftUsername=@minecraft WHERE id='" + Context.User.Id + "';", queryParams);

                await ReplyAsync(Context.User.Mention + ", you have successfully updated the Minecraft username on your profile to \"" + username + "\"!");
            }
    
            [Command("snapchat"), Summary("")]
            public async Task SetSnapchatUsername([Remainder]string username)
            {
                //User.UpdateUser(Context.User.Id, snapchat:username);
                
                List<(string, string)> queryParams = new List<(string, string)>()
                {
                    ("@snapchat", username)
                };
                DatabaseActivity.ExecuteNonQueryCommand("UPDATE users SET snapchatUsername=@snapchat WHERE id='" + Context.User.Id + "';", queryParams);

                await ReplyAsync(Context.User.Mention + ", you have successfully updated the Snapchat username on your profile to \"" + username + "\"!");
            }
    
            [Command("instagram"), Summary("")]
            public async Task SetInstagramUsername([Remainder]string username)
            {
                //User.UpdateUser(Context.User.Id, instagram:username);
                
                List<(string, string)> queryParams = new List<(string, string)>()
                {
                    ("@instagram", username)
                };
                DatabaseActivity.ExecuteNonQueryCommand("UPDATE users SET instagramUsername=@instagram WHERE id='" + Context.User.Id + "';", queryParams);

                await ReplyAsync(Context.User.Mention + ", you have successfully updated the Instagram username on your profile to \"" + username + "\"!");
            }
    
            [Command("github"), Summary("")]
            public async Task SetGitHubUsername([Remainder]string username)
            {
                //User.UpdateUser(Context.User.Id, github:username);
                
                List<(string, string)> queryParams = new List<(string, string)>()
                {
                    ("@github", username)
                };
                DatabaseActivity.ExecuteNonQueryCommand("UPDATE users SET githubUsername=@github WHERE id='" + Context.User.Id + "';", queryParams);

                await ReplyAsync(Context.User.Mention + ", you have successfully updated the GitHub username on your profile to \"" + username + "\"!");
            }
    
            [Command("websitename"), Summary("")]
            public async Task SetWebsiteName([Remainder]string name)
            {
                //User.UpdateUser(Context.User.Id, websiteName: name);
                
                List<(string, string)> queryParams = new List<(string, string)>()
                {
                    ("@websiteName", name)
                };
                DatabaseActivity.ExecuteNonQueryCommand("UPDATE users SET websiteName=@websiteName WHERE id='" + Context.User.Id + "';", queryParams);

                await ReplyAsync(Context.User.Mention + ", you have successfully updated the website name on your profile to \"" + name + "\"!");
            }
    
            [Command("websiteurl"), Summary("")]
            public async Task SetWebsiteUrl([Remainder]string url)
            {
                //User.UpdateUser(Context.User.Id, websiteUrl: url);
                
                List<(string, string)> queryParams = new List<(string, string)>()
                {
                    ("@websiteUrl", url)
                };
                DatabaseActivity.ExecuteNonQueryCommand("UPDATE users SET websiteURL=@websiteUrl WHERE id='" + Context.User.Id + "';", queryParams);

                await ReplyAsync(Context.User.Mention + ", you have successfully updated the website URL on your profile to \"" + url + "\"!");
            }
    
            #endregion
            
        }
        
        [Command("about"), Summary("Returns the about description about the user specified.")]
        public async Task UserAbout(IUser user = null)
        {
            var userSpecified = user as SocketGuildUser ?? Context.User as SocketGuildUser;

            if (userSpecified == null)
            {
                await ReplyAsync("User not found, please try again.");
                return;
            }
            
            EmbedAuthorBuilder eab = new EmbedAuthorBuilder();
            if(!String.IsNullOrEmpty(userSpecified.Nickname)) eab.WithName("About " + userSpecified.Nickname);
            else eab.WithName("About " + userSpecified.Username);

            EmbedFooterBuilder efb = new EmbedFooterBuilder();
            if (userSpecified.IsTeamMember()) eab.WithIconUrl(userSpecified.GetEmbedAuthorBuilderIconUrl());
            if (!String.IsNullOrEmpty(userSpecified.GetFooterText()))
            {
                efb.WithText(userSpecified.GetFooterText());
                efb.WithIconUrl(userSpecified.GetEmbedFooterBuilderIconUrl());
            }

            EmbedBuilder eb = new EmbedBuilder()
                .WithAuthor(eab)
                .WithFooter(efb)
                .WithThumbnailUrl(userSpecified.GetAvatarUrl())
                .WithDescription(User.Load(userSpecified.Id).About)
                .WithColor(userSpecified.GetCustomRGB());

            if (!String.IsNullOrEmpty(userSpecified.GetName()))
                eb.AddField("Name", userSpecified.GetName(), true);

            if (!String.IsNullOrEmpty(userSpecified.GetGender()))
                eb.AddField("Gender", userSpecified.GetGender(), true);
            
            if (!String.IsNullOrEmpty(userSpecified.GetPronouns()))
                eb.AddField("Pronouns", userSpecified.GetPronouns(), true);
            
            eb.AddField("Level", userSpecified.GetLevel(), true);
            eb.AddField("EXP", userSpecified.GetEXP(), true);
            eb.AddField("Account Created", userSpecified.UserCreateDate(), true);
            eb.AddField("Joined Guild", userSpecified.GuildJoinDate(), true);
            
            if (!String.IsNullOrEmpty(userSpecified.GetMinecraftUsername()))
                eb.AddField("Minecraft Username", userSpecified.GetMinecraftUsername(), true);

            if (!String.IsNullOrEmpty(userSpecified.GetWebsiteUrl()))
                eb.AddField(StringConfiguration.Load().DefaultWebsiteName, "[" + (userSpecified.GetWebsiteName() ?? StringConfiguration.Load().DefaultWebsiteName) + "](" + userSpecified.GetWebsiteUrl() + ")", true);

            if (!String.IsNullOrEmpty(userSpecified.GetInstagramUsername()))
                eb.AddField("Instagram", "[" + userSpecified.GetInstagramUsername() + "](https://www.instagram.com/" + userSpecified.GetInstagramUsername() + "/)", true);

            if (!String.IsNullOrEmpty(userSpecified.GetSnapchatUsername()))
                eb.AddField("Snapchat", "[" + userSpecified.GetSnapchatUsername() + "](https://www.snapchat.com/add/" + userSpecified.GetSnapchatUsername() + "/)", true);

            if (!String.IsNullOrEmpty(userSpecified.GetGitHubUsername()))
                eb.AddField("GitHub", "[" + userSpecified.GetGitHubUsername() + "](https://github.com/" + userSpecified.GetGitHubUsername() + "/)", true);

            if (!String.IsNullOrEmpty(userSpecified.GetCustomPrefix()))
                eb.AddField("Custom Prefix", userSpecified.GetCustomPrefix(), true);
            
            await ReplyAsync("", false, eb.Build());
        }
    }
}