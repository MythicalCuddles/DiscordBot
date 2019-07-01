using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;

using DiscordBot.Common;
using DiscordBot.Database;
using DiscordBot.Extensions;
using DiscordBot.Objects;
using MelissaNet;
using MySql.Data.MySqlClient;

namespace DiscordBot.Handlers
{
    public class UserHandler
    {
        public static async Task UserJoined(SocketGuildUser e)
        {
            //Insert new users into the database by using INSERT IGNORE
            List<(string, string)> queryParams = new List<(string id, string value)>()
            {
                ("@username", e.Username),
                ("@avatarUrl", e.GetAvatarUrl())
            };
					
            int rowsUpdated = DatabaseActivity.ExecuteNonQueryCommand(
                "INSERT IGNORE INTO " +
                "users(id,username,avatarUrl) " +
                "VALUES (" + e.Id + ", @username, @avatarUrl);", queryParams);
					
            //end.
            
            if (rowsUpdated == 1)
            {
                EmbedBuilder eb = new EmbedBuilder()
                {
                    Title = e.Guild.Name + " - User Joined - " + e.Username,
                    Description = "@" + e.Username + "\n" + e.Id,
                    Color = new Color(28, 255, 28),
                    ThumbnailUrl = e.GetAvatarUrl()
                }.WithCurrentTimestamp();
                await Guild.Load(e.Guild.Id).LogChannelID.GetTextChannel().SendMessageAsync("", false, eb.Build());

                if (Guild.Load(e.Guild.Id).WelcomeMessage != null && Guild.Load(e.Guild.Id).WelcomeChannelID != 0)
                {
                    await Guild.Load(e.Guild.Id).WelcomeChannelID.GetTextChannel()
                        .SendMessageAsync(Guild.Load(e.Guild.Id).WelcomeMessage.ModifyStringFlags(e));
                }
                
                
                EmbedBuilder lEB = new EmbedBuilder()
                {
                    Title = "New User - " + e.Username,
                    Description = e.Id + " added to the database successfully.",
                    Color = new Color(28, 255, 28),
                    ThumbnailUrl = e.GetAvatarUrl()
                }.WithCurrentTimestamp();
                
                await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("", false, lEB.Build());
            }
            else
            {
                EmbedBuilder eb = new EmbedBuilder()
                {
                    Title = e.Guild.Name + " - User Joined - " + e.Username,
                    Description = "",
                    ThumbnailUrl = e.GetAvatarUrl(),
                    Color = new Color(28, 255, 28),
                    Footer = new EmbedFooterBuilder()
                    {
                        Text = "ID: " + e.Id + " • Team Member: " + e.IsTeamMember().ToYesNo()
                    }
                }.WithCurrentTimestamp();

                if (e.GetAbout() != null)
                {
                    eb.AddField("About " + e.Username, e.GetAbout());
                }

                eb.AddField("Username", "@" + e.Username + "#" + e.DiscriminatorValue, true);
                eb.AddField("Level", e.GetLevel(), true);
                eb.AddField("EXP", e.GetEXP(), true);
                eb.AddField("Account Created", e.UserCreateDate(), true);
                eb.AddField("Joined Guild", e.GuildJoinDate(), true);

                if (!String.IsNullOrEmpty(e.GetName()))
                {
                    eb.AddField("Name", e.GetName(), true);
                }
                if (!String.IsNullOrEmpty(e.GetGender()))
                {
                    eb.AddField("Gender", e.GetGender(), true);
                }
                if (!String.IsNullOrEmpty(e.GetPronouns()))
                {
                    eb.AddField("Pronouns", e.GetPronouns(), true);
                }
                if (!String.IsNullOrEmpty(e.GetMinecraftUsername()))
                {
                    eb.AddField("Minecraft Username", e.GetMinecraftUsername(), true);
                }
                if (!String.IsNullOrEmpty(e.GetInstagramUsername()))
                {
                    eb.AddField("Instagram",
                        "[" + e.GetInstagramUsername() + "](https://www.instagram.com/" + e.GetInstagramUsername() +
                        "/)", true);
                }
                if (!String.IsNullOrEmpty(e.GetSnapchatUsername()))
                {
                    eb.AddField("Snapchat",
                        "[" + e.GetSnapchatUsername() + "](https://www.snapchat.com/add/" + e.GetSnapchatUsername() +
                        "/)", true);
                }
                if (!String.IsNullOrEmpty(e.GetGitHubUsername()))
                {
                    eb.AddField("GitHub",
                        "[" + e.GetGitHubUsername() + "](https://github.com/" + e.GetGitHubUsername() + "/)", true);
                }
                if (!String.IsNullOrEmpty(e.GetFooterText()))
                {
                    eb.AddField("Footer Text", e.GetFooterText(), true);
                }

                await Guild.Load(e.Guild.Id).LogChannelID.GetTextChannel().SendMessageAsync("", false, eb.Build());
            }
        }

        public static async Task UserLeft(SocketGuildUser e)
        {
            EmbedBuilder eb = new EmbedBuilder()
            {
                Description = "",
                ThumbnailUrl = e.GetAvatarUrl(),
                Color = new Color(255, 28, 28),
                Footer = new EmbedFooterBuilder()
                {
                    Text = "ID: " + e.Id + " • Team Member: " + e.IsTeamMember().ToYesNo()
                }
            }.WithCurrentTimestamp();

            if (e.Nickname != null)
            {
                eb.WithTitle(e.Guild.Name + " - User Left - " + e.Nickname);
            }
            else
            {
                eb.WithTitle(e.Guild.Name + " - User Left - " + e.Username);
            }

            if (e.GetAbout() != null)
            {
                eb.AddField("About " + e.Username, e.GetAbout());
            }

            eb.AddField("Username", "@" + e.Username + "#" + e.DiscriminatorValue, true);
            eb.AddField("Level", e.GetLevel(), true);
            eb.AddField("EXP", e.GetEXP(), true);
            eb.AddField("Account Created", e.UserCreateDate(), true);
            eb.AddField("Joined Guild", e.GuildJoinDate(), true);

            if (e.GetName() != null)
            {
                eb.AddField("Name", e.GetName(), true);
            }
            if (e.GetGender() != null)
            {
                eb.AddField("Gender", e.GetGender(), true);
            }
            if (e.GetPronouns() != null)
            {
                eb.AddField("Pronouns", e.GetPronouns(), true);
            }
            if (e.GetMinecraftUsername() != null)
            {
                eb.AddField("Minecraft Username", e.GetMinecraftUsername(), true);
            }
            if (e.GetSnapchatUsername() != null)
            {
                eb.AddField("Snapchat",
                    "[" + e.GetSnapchatUsername() + "](https://www.snapchat.com/add/" + e.GetSnapchatUsername() + "/)",
                    true);
            }
            if (e.GetFooterText() != null)
            {
                eb.AddField("Footer Text", e.GetFooterText(), true);
            }

            await Guild.Load(e.Guild.Id).LogChannelID.GetTextChannel().SendMessageAsync("", false, eb.Build());
        }

        public static async Task UserUpdated(SocketUser cachedUser, SocketUser user)
        {
            if (user.Username != cachedUser.Username || user.GetAvatarUrl() != cachedUser.GetAvatarUrl()) // If user has updated username or avatar
            {
                List<(string, string)> queryParams = new List<(string, string)>()
                {
                    ("@username", user.Username),
                    ("@avatarUrl", user.GetAvatarUrl())
                };
                DatabaseActivity.ExecuteNonQueryCommand("UPDATE users SET username=@username, avatarUrl=@avatarUrl WHERE id='" + user.Id + "';", queryParams);

            }
        }

        public static async Task UserBanned(SocketUser socketUser, SocketGuild socketGuild)
        {
            //Insert banned users into the database by using INSERT IGNORE
            List<(string, string)> queryParams = new List<(string id, string value)>()
            {
                ("@issuedTo", socketUser.Id.ToString()),
                ("@issuedBy", DiscordBot.Bot.CurrentUser.Id.ToString()),
                ("@inGuild", socketGuild.Id.ToString()),
                ("@reason", socketGuild.GetBanAsync(socketUser.Id).GetAwaiter().GetResult().Reason),
                ("@date", DateTime.Now.ToString("u"))
            };

            DatabaseActivity.ExecuteNonQueryCommand(
                "INSERT IGNORE INTO " +
                "bans(issuedTo,issuedBy,inGuild,banDescription,dateIssued) " +
                "VALUES (@issuedTo, @issuedBy, @inGuild, @reason, @date);", queryParams);

            //end.
        }

        public static async Task UserUnbanned(SocketUser socketUser, SocketGuild socketGuild)
        {
            DatabaseActivity.ExecuteNonQueryCommand("DELETE FROM bans WHERE issuedTo=" + socketUser.Id + " AND inGuild=" + socketGuild.Id + ";");
        }
    }
}
