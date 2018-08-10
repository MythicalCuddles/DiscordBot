using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Common;
using DiscordBot.Common.Preconditions;
using DiscordBot.Extensions;
using DiscordBot.Logging;

namespace DiscordBot.Modules.Public.Games
{
    [Name("Raid Gamemode Commands")]
    [Group("raid")]
    [MinPermissions(PermissionLevel.BotOwner)]
    [RequireContext(ContextType.Guild)]
    //todo: finish development - coming soon
    public class RaidGamemodeModule : ModuleBase
    {
        [Command("create"), Summary("Creates a raid for other users to join.")]
        public async Task CreateRaid(int? investment = null)
        {
            if (User.Load(Context.User.Id).InRaid != 0) // User is in a raid team or is currently planning raid.
            {
                await ReplyAsync(Context.User.Id + ", you're already in a raid team!");
                return;
            }
            
            if (investment == null)
            {
                await ReplyAsync("**Syntax:** " + GuildConfiguration.Load(Context.Guild.Id).Prefix + "raid create [coin investment] \n" +
                                 "```Coin investment is the amount of coins you're going to spend on equipment for yourself. Other players that join also have to pay the investment cost to get the same level of equipment as you.```");
                return;
            }

            if (Configuration.Load().RaidsEnabled == false)
            {
                await ReplyAsync(Context.User.Mention + ", no new raids can be planned at this time. Please try again later.");
                return;
            }
            
            if (Context.User.GetCoins() < investment)
            {
                await ReplyAsync(Context.User.Mention +
                                 ", you don't have enough coins to buy the equipment required to do a raid.");
                return;
            }
            
            User.UpdateUser(Context.User.Id, 
                inRaid:Context.User.Id, 
                raidInvestment:investment);
            Context.User.AwardCoinsToUser(-investment);
            
            await ReplyAsync(Context.User.Mention + " is forming a raid team. Type \"" + GuildConfiguration.Load(Context.Guild.Id).Prefix + "raid join " + Context.User.Mention + "\" to join their team!");
        }

        [Command("start"), Summary("Lets the raid leader start their raid.")]
        public async Task StartRaid()
        {
            if (User.Load(Context.User.Id).InRaid == 0)
            {
                await ReplyAsync(Context.User.Id + ", you're not organising a raid team!");
                return;
            }
            
            if (Context.User.Id != User.Load(Context.User.Id).InRaid)
            {
                await ReplyAsync(Context.User.Mention +
                                 ", you can not start a raid that you are organising. Please wait for " +
                                 User.Load(Context.User.Id).InRaid.GetUser().Mention + " to start their raid.");
                return;
            }
            
            foreach (var g in DiscordBot.Bot.Guilds)
            {
                foreach (var u in g.Users)
                {
                    if (User.Load(u.Id).InRaid == Context.User.Id)
                    {
                        User.UpdateUser(u.Id, lastRaid: DateTime.Now);
                    }
                }
            }
            
            // todo: add senario (win/lose)
            
            // Clear users from raid.
            foreach (var g in DiscordBot.Bot.Guilds)
            {
                foreach (var u in g.Users)
                {
                    if (User.Load(u.Id).InRaid == Context.User.Id)
                    {
                        User.UpdateUser(u.Id, inRaid:0, raidInvestment:0);
                    }
                }
            }
        }

        [Command("join"), Summary("Lets the user join the specified user's raid team.")]
        public async Task JoinRaid(IUser raidLeader)
        {
            if (User.Load(Context.User.Id).InRaid != 0)
            {
                if (User.Load(Context.User.Id).InRaid == raidLeader.Id) // User is in a raid team or is currently planning raid.
                {
                    await ReplyAsync(Context.User.Id + ", you're already in the raid you're organising!");
                }
                else
                {
                    await ReplyAsync(Context.User.Id + ", you're already in that raid team!");
                }
                
                return;
            }

            if (Context.User.GetCoins() < User.Load(raidLeader.Id).RaidInvestment)
            {
                await ReplyAsync(Context.User.Mention +
                                 ", you don't have enough coins to buy the equipment required to join " + raidLeader.Mention + "'s raid.");
                return;
            }

            if (raidLeader.Id != User.Load(raidLeader.Id).InRaid) // if the user they mentioned is in their own raid, they are the leader.
            {
                await ReplyAsync(Context.User.Mention +
                                 ", that user doesn't seem to be organising a raid at this time.");
                return;
            }
            
            User.UpdateUser(Context.User.Id, 
                inRaid:raidLeader.Id, 
                raidInvestment:User.Load(raidLeader.Id).RaidInvestment);
            Context.User.AwardCoinsToUser(-User.Load(raidLeader.Id).RaidInvestment);

            await ReplyAsync(Context.User.Mention + ", you have joined " + raidLeader.Mention + "'s raid team. Please wait for them to start the raid.");
        }

        [Command("leave"), Summary("Allows the user to leave their raid.")]
        public async Task LeaveRaid()
        {
            // leave the raid.
            
            // reset all users if the raid planner has left.
        }

        private async Task<bool> InCooldown(IUser user)
        {
            return !((DateTime.Now - User.Load(user.Id).LastRaid).TotalSeconds >= Configuration.Load().RaidCooldownInSeconds);
        }
    }
}