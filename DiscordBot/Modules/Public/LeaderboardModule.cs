using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

using DiscordBot.Common.Preconditions;
using DiscordBot.Common;
using DiscordBot.Extensions;
using DiscordBot.Objects;

namespace DiscordBot.Modules.Public
{
    [Name("Leaderboard Commands")]
    [MinPermissions(PermissionLevel.User)]
    [RequireContext(ContextType.Guild)]
    [Group("leaderboard")]
    public class LeaderboardModule : ModuleBase
    {
        [Command("")]
        public async Task Leaderboard()
        {
            await GetGuildCoinLeaderboard().ConfigureAwait(false);
        }

        [Command("global"), Summary("Global Leaderboard for the coins system.")]
        public async Task GetGlobalCoinLeaderboard()
        {
            await ShowLeaderboard(Context).ConfigureAwait(false);
        }

        [Command("guild"), Summary("Guild Leaderboard for the coins system.")]
        public async Task GetGuildCoinLeaderboard()
        {
            await ShowLeaderboard(Context, isGuild:true).ConfigureAwait(false);
        }

        private async Task ShowLeaderboard(ICommandContext context, bool isGuild = false)
        {
            var listAmount = Configuration.Load().LeaderboardAmount;
            var userList = new List<Tuple<int, SocketGuildUser>>();

            foreach (var g in DiscordBot.Bot.Guilds)
            {
                if (isGuild)
                {
                    if (g.Id == context.Guild.Id)
                    {
                        foreach (var u in g.Users)
                        {
                            if (userList.All(i => i.Item2.Id != u.Id) && !u.IsBot)
                            {
                                userList.Add(new Tuple<int, SocketGuildUser>(u.GetEXP(), u));
                            }
                        }
                    }
                }
                else
                {
                    foreach (var u in g.Users)
                    {
                        if (userList.All(i => i.Item2.Id != u.Id) && !u.IsBot)
                        {
                            userList.Add(new Tuple<int, SocketGuildUser>(u.GetEXP(), u));
                        }
                    }
                }
            }

            var sortedList =
                userList.OrderByDescending(intTuple => intTuple.Item1).ToList();

            if (sortedList.Count < listAmount)
            {
                listAmount = sortedList.Count;
            }

            var eb = new EmbedBuilder()
            {
                Color = new Color(Configuration.Load().LeaderboardEmbedColor),
                ThumbnailUrl = Configuration.Load().LeaderboardTrophyUrl
            }.WithCurrentTimestamp();

            if (isGuild)
            {
                eb.WithAuthor("Guild Leaderboard - Top " + listAmount + "");
                eb.WithFooter("Did you know? You can do \"" + Guild.Load(context.Guild.Id).Prefix + "leaderboard global\" to see the global leaderboard!");
            }
            else
            {
                eb.WithAuthor("Global Leaderboard - Top " + listAmount + "");
                eb.WithFooter("Did you know? You can do \"" + Guild.Load(context.Guild.Id).Prefix + "leaderboard guild\" to see the guild leaderboard!");
            }

            var sb = new StringBuilder().Append("```INI\n");
            var shownList = new List<Tuple<int, SocketGuildUser>>();
            for (var i = 0; i < listAmount; i++)
            {
                sb.Append("[" + (i + 1) + "] @" + sortedList[i].Item2.Username + ": Level " + sortedList[i].Item2.GetLevel() + " (" + sortedList[i].Item1 + " EXP)\n");
                shownList.Add(new Tuple<int, SocketGuildUser>(sortedList[i].Item1, sortedList[i].Item2));
            }

            if (shownList.All(i => i.Item2.Id != context.User.Id))
            {
                sb.Append("...\n");
                var pos = sortedList.FindIndex(t => t.Item2.Id == context.User.Id);

                if (sortedList.ElementAtOrDefault(pos - 1) != null)
                {
                    sb.Append("[" + (pos) + "] @" + sortedList[pos - 1].Item2.Username + ": Level " + User.Load(sortedList[pos - 1].Item2.Id).Level + " (" + sortedList[pos - 1].Item1 + " EXP)\n");   
                }
                sb.Append("[" + (pos + 1) + "] @" + sortedList[pos].Item2.Username + ": Level " + User.Load(sortedList[pos].Item2.Id).Level + " (" +  sortedList[pos].Item1 + " EXP)\n"); // Shown for User
                if ((pos + 1) < sortedList.Count)
                {
                    if (sortedList[pos + 1] != null)
                    {
                        sb.Append("[" + (pos + 2) + "] @" + sortedList[pos + 1].Item2.Username + ": Level " + User.Load(sortedList[pos + 1].Item2.Id).Level + " (" +  sortedList[pos + 1].Item1 + " EXP)\n");
                    }
                }
            }
            sb.Append("```");
            
            eb.WithDescription(sb.ToString());
            await ReplyAsync("", false, eb.Build());
        }
    }
}
