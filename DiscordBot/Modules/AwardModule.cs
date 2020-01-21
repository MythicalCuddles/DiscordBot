using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Discord.Addons.Interactive;

using DiscordBot.Common;
using DiscordBot.Common.Preconditions;
using DiscordBot.Exceptions;
using DiscordBot.Extensions;
using DiscordBot.Handlers;
using DiscordBot.Logging;
using DiscordBot.Objects;

namespace DiscordBot.Modules
{
    [RequireContext(ContextType.Guild)]
    [Name("Award Commands")]
    public class AwardModule : InteractiveBase
    {
        [MinPermissions(PermissionLevel.User)]
        [Command("awards"), Summary("")]
        public async Task ShowUserAwards(IUser user = null)
        {
            var userSpecified = user as SocketGuildUser ?? Context.User as SocketGuildUser;

            if (userSpecified == null)
            {
                await ReplyAsync("User not found, please try again.");
                return;
            }
            
            EmbedAuthorBuilder eab = new EmbedAuthorBuilder();
            eab.IconUrl = Configuration.Load().AwardsIconUrl;
            eab.Name = userSpecified.Username + "'s Awards";

            EmbedBuilder eb = new EmbedBuilder()
                .WithAuthor(eab)
                .WithTitle("Award Name - Date Obtained")
                .WithThumbnailUrl(userSpecified.GetAvatarUrl())
                .WithColor(userSpecified.GetCustomRGB());
            
            List<Award> awards = new List<Award>();

            foreach (Award a in Award.Awards)
            {
                if (a.UserId == userSpecified.Id)
                {
                    awards.Add(a);
                }
            }
            awards.Sort((x, y) => x.DateAwarded.CompareTo(y.DateAwarded));
            
            StringBuilder sb = new StringBuilder();

            if (awards.Count == 0)
            {
                sb.Append("No awards found.");
            }
            else
            {
                DateTime date = DateTime.Now;
                bool showAllAwards = Configuration.Load().ShowAllAwards;
                
                foreach (Award a in awards)
                {
                    if (!showAllAwards)
                    {
                        int result = DateTime.Compare(date, a.DateAwarded);
                        // result : -1:Future Date, 1:Past Date, 1:Today
                        if (result >= 0)
                        {
                            sb.Append(a.AwardText + " - " + a.DateAwarded.ToString("dd/MM/yyyy") + "\n");
                        }
                    }
                    else
                    {
                        sb.Append(a.AwardText + " - " + a.DateAwarded.ToString("dd/MM/yyyy") + "\n");
                    }
                }
            }

            eb.WithDescription(sb.ToString());
            await ReplyAsync("", false, eb.Build());
        }

        [MinPermissions(PermissionLevel.ServerAdmin)]
        [Command("giveaward"), Summary("")]
        public async Task AddUserAward(IUser user = null, string aCategory = null, string aText = null)
        {
            if (user == null || aCategory == null || aText == null)
            {
                EmbedBuilder eb = new EmbedBuilder
                    {
                        Title = "Invalid Syntax",
                        Description = "**Syntax:** " + Guild.Load(Context.Guild.Id).Prefix + "giveaward [user] [award category] [award name]",
                        Color = new Color(210, 47, 33)
                    };

                eb.AddField("Notes", "Be sure to use speech marks (\"\") when specifying an award category or name!");
                eb.WithCurrentTimestamp();

                await ReplyAsync("", false, eb.Build());
                return;
            }
            
            Award.AddAward(user.Id, aCategory, aText);
            
            AdminLog.Log(Context.User.Id, Context.Message.Content, Context.Guild.Id, user.Id);

            EmbedBuilder replyEmbed = new EmbedBuilder()
                {
                    Title = "Congratulations " + user.Username + "!",
                    Color = new Color(0, 255, 0),
                    Description = user.Mention + " earned an award!",
                    Footer = new EmbedFooterBuilder()
                    {
                        Text = "Awarded by " + Context.User.Username + " (AID: " + Award.Awards[Award.Awards.Count-1].AwardId + ")"
                    }
                }
                .AddField("Award Category", aCategory)
                .AddField("Award", aText)
                .WithCurrentTimestamp();
            
            await ReplyAsync("", false, replyEmbed.Build());
            
            replyEmbed = new EmbedBuilder()
                {
                    Title = "New Award",
                    Description = user.Mention + " was given an award by " + Context.User.Mention,
                    Footer = new EmbedFooterBuilder()
                    {
                        Text = "ID: " + Award.Awards[Award.Awards.Count-1].AwardId
                    }
                }
                .AddField("Award Category", aCategory)
                .AddField("Award", aText)
                .WithCurrentTimestamp();
            
            await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("", false, replyEmbed.Build());
        }

        [MinPermissions(PermissionLevel.ServerAdmin)]
        [Command("listawards"), Summary("")]
        public async Task ListAllAwards()
        {
            EmbedBuilder eb;
            if (!Award.Awards.Any())
            {
                eb = new EmbedBuilder
                {
                    Title = "Awards",
                    Description = "There is no awards in the database.",
                    Color = new Color(235, 160, 40)
                };

                await ReplyAsync("", false, eb.Build());
                return;
            }
            
            AdminLog.Log(Context.User.Id, Context.Message.Content, Context.Guild.Id);
            
            List<string> awardPages = new List<string>();
            Award.Awards.Select((v, i) => new { Value = v, Index = i / 10 })
                .GroupBy(x => x.Index).ToList()
                .ForEach(x => awardPages.Add(String.Join("\n", x.Select(z =>
                {
                    try
                    {
                        return z.Value.UserId.GetUser().Mention + " - " + z.Value.AwardText + " (ID: " + z.Value.AwardId + ")";
                    }
                    catch (UserNotFoundException)
                    {
                        return z.Value.UserId + " - " + z.Value.AwardText + " (ID: " + z.Value.AwardId + ")";
                    }
                }))));
            
            PaginatedMessage msg = new PaginatedMessage
            {
                Title = "Awards List",
                Pages = awardPages,
                Color = new Color(211, 214, 77),
                Options = new PaginatedAppearanceOptions() { DisplayInformationIcon = false }
            };

            await PagedReplyAsync(msg);
        }
        
        [MinPermissions(PermissionLevel.ServerAdmin)]
        [Command("deleteaward", RunMode = RunMode.Async)]
        public async Task DeleteUserAward(string id = null)
        {
            if (id == null)
            {
                EmbedBuilder eb = new EmbedBuilder
                    {
                        Title = "Invalid Syntax",
                        Description = "**Syntax:** " + Guild.Load(Context.Guild.Id).Prefix + "deleteaward [id]",
                        Color = new Color(210, 47, 33)
                    }
                    .WithCurrentTimestamp();

                await ReplyAsync("", false, eb.Build());
                return;
            }

            if (int.TryParse(id, out var aId))
            {
                Award award;

                try
                {
                    award = Award.GetAward(aId);
                }
                catch (AwardNotFoundException exception)
                {
                    await ReplyAsync(Context.User.Mention + ", I couldn't find an award with that ID. Please try again.");
                    await new LogMessage(LogSeverity.Warning, "AwardModule", exception.Message).PrintToConsole();
                    return;
                }
                
                AdminLog.Log(Context.User.Id, Context.Message.Content, Context.Guild.Id);

                EmbedBuilder awardEmbed = new EmbedBuilder()
                    {
                        Title = "Are you sure you wish to delete Award ID: " + award.AwardId,
                        Description = "You're about to delete the following award:",
                        Color = new Color(255, 0, 0),
                        Footer = new EmbedFooterBuilder()
                        {
                            Text = "Responding with anything but \"Confirm\" will cancel the operation."
                        }
                    }
                    .AddField("Award Category", award.AwardCategory)
                    .AddField("Award", award.AwardText)
                    .WithCurrentTimestamp();

                try
                {
                    awardEmbed.AddField("Awarded To", award.UserId.GetUser().Mention);
                }
                catch (UserNotFoundException exception)
                {
                    awardEmbed.AddField("Awarded To", award.UserId.ToString());
                    await new LogMessage(LogSeverity.Warning, "AwardModule", exception.Message).PrintToConsole();
                }

                awardEmbed
                    .AddField("Date Awarded", award.DateAwarded.ToLongDateString())
                    .AddField("Confirmation",
                        "If you wish to delete this award, please type \"Confirm\" into the chat, or type \"Cancel\" if you wish to cancel");

                await ReplyAsync("", false, awardEmbed.Build());
                
                var response = await NextMessageAsync();
                if (response != null)
                {
                    if (response.Content.ToUpper() == "CONFIRM")
                    {
                        AdminLog.Log(Context.User.Id, Context.Message.Content + " confirm", Context.Guild.Id);

                        Award.DeleteAward(award.AwardId);
                        
                        await ReplyAsync("Award ID: " + award.AwardId + " has been deleted.");
                    }
                    else if (response.Content.ToUpper() == "CANCEL")
                    {
                        await ReplyAsync(Context.User.Mention + ", the operation was cancelled successfully.");
                    }
                    else
                    {
                        await ReplyAsync(Context.User.Mention + ", we've cancelled the deleteaward operation as none of the keywords were mentioned.");
                    }
                }
                else
                {
                    await ReplyAsync(Context.User.Mention + ", you didn't reply before the timeout, therefore the operation has been automatically cancelled.");
                }
            }
            else
            {
                EmbedBuilder eb = new EmbedBuilder
                    {
                        Title = "Invalid Syntax",
                        Description = "**Syntax:** " + Guild.Load(Context.Guild.Id).Prefix + "deleteaward [id]",
                        Color = new Color(210, 47, 33),
                        Footer = new EmbedFooterBuilder()
                        {
                            Text = "You can use the command listawards to find the ID's of the quotes."
                        }
                    }
                    .WithCurrentTimestamp();

                await ReplyAsync("", false, eb.Build());
            }
        }
        
        [MinPermissions(PermissionLevel.ServerAdmin)]
        [Command("awardinfo"), Summary("")]
        public async Task GetAwardInfo(string id = null)
        {
            if (id == null)
            {
                EmbedBuilder eb = new EmbedBuilder
                    {
                        Title = "Invalid Syntax",
                        Description = "**Syntax:** " + Guild.Load(Context.Guild.Id).Prefix + "awardinfo [id]",
                        Color = new Color(210, 47, 33)
                    }
                    .WithCurrentTimestamp();

                await ReplyAsync("", false, eb.Build());
                return;
            }

            if (int.TryParse(id, out var aId))
            {
                Award award;

                try
                {
                    award = Award.GetAward(aId);
                }
                catch (AwardNotFoundException exception)
                {
                    await ReplyAsync(Context.User.Mention + ", I couldn't find an award with that ID. Please try again.");
                    await new LogMessage(LogSeverity.Warning, "AwardModule", exception.Message).PrintToConsole();
                    return;
                }
                
                AdminLog.Log(Context.User.Id, Context.Message.Content, Context.Guild.Id);

                EmbedBuilder awardEmbed = new EmbedBuilder()
                    {
                        Title = "Award #" + award.AwardId,
                        Color = new Color(211, 214, 77) 
                    }
                    .AddField("Award Category", award.AwardCategory)
                    .AddField("Award", award.AwardText)
                    .WithCurrentTimestamp();

                try
                {
                    awardEmbed.AddField("Awarded To", award.UserId.GetUser().Mention);
                }
                catch (UserNotFoundException exception)
                {
                    awardEmbed.AddField("Awarded To", award.UserId.ToString());
                    await new LogMessage(LogSeverity.Warning, "AwardModule", exception.Message).PrintToConsole();
                }

                awardEmbed
                    .AddField("Date Awarded", award.DateAwarded.ToLongDateString());

                await ReplyAsync("", false, awardEmbed.Build());
            }
            else
            {
                EmbedBuilder eb = new EmbedBuilder
                    {
                        Title = "Invalid Syntax",
                        Description = "**Syntax:** " + Guild.Load(Context.Guild.Id).Prefix + "awardinfo [id]",
                        Color = new Color(210, 47, 33),
                        Footer = new EmbedFooterBuilder()
                        {
                            Text = "You can use the command listawards to find the ID's of the quotes."
                        }
                    }
                    .WithCurrentTimestamp();

                await ReplyAsync("", false, eb.Build());
            }
        }
    }
}