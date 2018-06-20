using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord.Commands;
using Discord.WebSocket;

using DiscordBot.Common.Preconditions;
using DiscordBot.Common;

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
            await GetGuildCoinLeaderboard();
        }

        [Command("global"), Summary("Global Leaderboard for the coins system.")]
        public async Task GetGlobalCoinLeaderboard()
        {
            await ShowLeaderboard(Context);
        }

        [Command("guild"), Summary("Guild Leaderboard for the coins system.")]
        public async Task GetGuildCoinLeaderboard()
        {
            await ShowLeaderboard(Context, isGuild:true);
        }

        private async Task ShowLeaderboard(ICommandContext context, bool isGuild = false)
        {
            int listAmount = Configuration.Load().LeaderboardAmount;
            List<Tuple<int, SocketGuildUser>> userList = new List<Tuple<int, SocketGuildUser>>();

            foreach (SocketGuild g in MogiiBot3.Bot.Guilds)
            {
                if (isGuild && g.Id == context.Guild.Id)
                {
                    foreach (SocketGuildUser u in g.Users)
                    {
                        if (userList.All(i => i.Item2.Id != u.Id) && !u.IsBot)
                        {
                            userList.Add(new Tuple<int, SocketGuildUser>(User.Load(u.Id).Coins, u));
                        }
                    }
                }
                else
                {
                    foreach (SocketGuildUser u in g.Users)
                    {
                        if (userList.All(i => i.Item2.Id != u.Id) && !u.IsBot)
                        {
                            userList.Add(new Tuple<int, SocketGuildUser>(User.Load(u.Id).Coins, u));
                        }
                    }
                }
            }

            List<Tuple<int, SocketGuildUser>> sortedList =
                userList.OrderByDescending(intTuple => intTuple.Item1).ToList();

            if (sortedList.Count < listAmount)
                listAmount = sortedList.Count;

            StringBuilder sb = new StringBuilder();

            if (isGuild)
            {
                sb.Append("**Guild Leaderboard - Top " + listAmount + "**\n```");
            }
            else
            {
                sb.Append("**Global Leaderboard - Top " + listAmount + "**\n```");
            }

            List<Tuple<int, SocketGuildUser>> shownList = new List<Tuple<int, SocketGuildUser>>();
            for (int i = 0; i < listAmount; i++)
            {
                sb.Append((i + 1) + ". @" + sortedList[i].Item2.Username + ": " + sortedList[i].Item1 + " coin(s)\n");
                shownList.Add(new Tuple<int, SocketGuildUser>(sortedList[i].Item1, sortedList[i].Item2));
            }

            if (shownList.All(i => i.Item2.Id != context.User.Id))
            {
                sb.Append("...\n");
                int pos = sortedList.FindIndex(t => t.Item2.Id == context.User.Id);

                sb.Append((pos) + ". @" + sortedList[pos - 1].Item2.Username + ": " + sortedList[pos - 1].Item1 + " coin(s)\n");
                sb.Append((pos + 1) + ". @" + sortedList[pos].Item2.Username + ": " + sortedList[pos].Item1 + " coin(s)\n"); // Shown for User
                sb.Append((pos + 2) + ". @" + sortedList[pos + 1].Item2.Username + ": " + sortedList[pos + 2].Item1 + " coin(s)\n");
            }


            sb.Append("```");
            await ReplyAsync(sb.ToString());
        }
    }
}
