using System;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;

using DiscordBot.Common;
using DiscordBot.Common.Preconditions;
using DiscordBot.Extensions;
using DiscordBot.Logging;

namespace DiscordBot.Modules.Public.Games
{
    [Name("Dice Game Commands")]
    [Group("dice")]
    [MinPermissions(PermissionLevel.User)]
    [RequireContext(ContextType.Guild)]
    public class HigherLowerGameModule : ModuleBase
    {
        private readonly Random _random = new Random();

        [Command("")]
        public async Task Dice()
        {
            EmbedBuilder eb = new EmbedBuilder()
                .WithTitle("Syntax")
                .WithDescription(GuildConfiguration.Load(Context.Guild.Id).Prefix + "dice [higher/lower] [coins]")
                .WithFooter("Looking to roll some dice? Use \"" + GuildConfiguration.Load(Context.Guild.Id).Prefix + "rolldice\"");
            await ReplyAsync("", false, eb.Build());

        }

        [Command("higher")]
        public async Task DiceHigherBet(int coinsBet)
        {
            int userCoins = User.Load(Context.User.Id).Coins;
            if (coinsBet > userCoins)
            {
                await ReplyAsync("You do not have that many coin(s) to bet with, " + Context.User.Mention);
                return;
            }

            int botOne = _random.RandomNumber(1, 6), botTwo = _random.RandomNumber(1, 6), botTotal = botOne + botTwo;
            int userOne = _random.RandomNumber(1, 6), userTwo = _random.RandomNumber(1, 6), userTotal = userOne + userTwo;

            EmbedBuilder eb = new EmbedBuilder()
                .WithTitle("Dice Game")
                .WithDescription(Context.User.Username + " placed a bet that they would roll a higher total than " + MogiiBot3.Bot.CurrentUser.Username)
                .AddField(MogiiBot3.Bot.CurrentUser.Username + "'s Roll", MogiiBot3.Bot.CurrentUser.Mention + " rolled **" + botOne + "** and **" + botTwo + "** giving a total of **" + botTotal + "**", true)
                .AddField(Context.User.Username + "'s Roll", Context.User.Mention + " has rolled **" + userOne + "** and **" + userTwo + "** giving a total of **" + userTotal + "**", true);
            
            eb.WithColor(User.Load(Context.User.Id).AboutR, User.Load(Context.User.Id).AboutG,
                User.Load(Context.User.Id).AboutB);
            
            User.UpdateUser(Context.User.Id, coins: (userCoins - coinsBet));

            if (botTotal > userTotal)
            {
                eb.AddField(MogiiBot3.Bot.CurrentUser.Username + " Won!", Context.User.Username + " bet " + coinsBet + " coin(s) and lost.");
                TransactionLogger.AddTransaction(Context.User.Username + " [" + Context.User.Id + "] bet " + coinsBet + " on higher and lost.");
            }
            else if (botTotal == userTotal)
            {
                eb.AddField("No-one Won!", Context.User.Username + " bet " + coinsBet + " coin(s) and drew with " + MogiiBot3.Bot.CurrentUser.Username + ".");
                TransactionLogger.AddTransaction(Context.User.Username + " [" + Context.User.Id + "] bet " + coinsBet + " on higher and didn't win nor lose.");
                User.UpdateUser(Context.User.Id, coins: (userCoins + coinsBet));
            }
            else
            {
                int coinsWon = (coinsBet * 2);
                eb.AddField(Context.User.Username + " Won!", Context.User.Username + " bet " + coinsBet + " coin(s) and won " + coinsWon + ".");
                TransactionLogger.AddTransaction(Context.User.Username + " [" + Context.User.Id + "] bet " + coinsBet + " on higher and won " + coinsWon + " coins.");
                User.UpdateUser(Context.User.Id, coins: (userCoins + coinsWon));
            }
            
            await ReplyAsync("", false, eb.Build());
        }

        [Command("lower")]
        public async Task DiceLowerBet(int coinsBet)
        {
            int userCoins = User.Load(Context.User.Id).Coins;
            if (coinsBet > userCoins)
            {
                await ReplyAsync("You do not have that many coin(s) to bet with, " + Context.User.Mention);
                return;
            }

            int botOne = _random.RandomNumber(1, 6), botTwo = _random.RandomNumber(1, 6), botTotal = botOne + botTwo;
            int userOne = _random.RandomNumber(1, 6), userTwo = _random.RandomNumber(1, 6), userTotal = userOne + userTwo;
            
            EmbedBuilder eb = new EmbedBuilder()
                .WithTitle("Dice Game")
                .WithDescription(Context.User.Username + " placed a bet that they would roll a lower total than " + MogiiBot3.Bot.CurrentUser.Username)
                .AddField(MogiiBot3.Bot.CurrentUser.Username + "'s Roll", MogiiBot3.Bot.CurrentUser.Mention + " rolled **" + botOne + "** and **" + botTwo + "** giving a total of **" + botTotal + "**")
                .AddField(Context.User.Username + "'s Roll", Context.User.Mention + " has rolled **" + userOne + "** and **" + userTwo + "** giving a total of **" + userTotal + "**");

            eb.WithColor(User.Load(Context.User.Id).AboutR, User.Load(Context.User.Id).AboutG,
                User.Load(Context.User.Id).AboutB);

            User.UpdateUser(Context.User.Id, coins: (userCoins - coinsBet));

            if (botTotal < userTotal)
            {
                eb.AddField(MogiiBot3.Bot.CurrentUser.Username + " Won!", Context.User.Username + " bet " + coinsBet + " coin(s) and lost.");
                TransactionLogger.AddTransaction(Context.User.Username + " [" + Context.User.Id + "] bet " + coinsBet + " on lower and lost.");
            }
            else if (botTotal == userTotal)
            {
                eb.AddField("No-one Won!", Context.User.Username + " bet " + coinsBet + " coin(s) and drew with " + MogiiBot3.Bot.CurrentUser.Username + ".");
                TransactionLogger.AddTransaction(Context.User.Username + " [" + Context.User.Id + "] bet " + coinsBet + " on lower and didn't win nor lose.");
                User.UpdateUser(Context.User.Id, coins: (userCoins + coinsBet));
            }
            else
            {
                int coinsWon = (coinsBet * 2);
                eb.AddField(Context.User.Username + " Won!", Context.User.Username + " bet " + coinsBet + " coin(s) and won " + coinsWon + ".");
                TransactionLogger.AddTransaction(Context.User.Username + " [" + Context.User.Id + "] bet " + coinsBet + " on lower and won " + coinsWon + " coins.");
                User.UpdateUser(Context.User.Id, coins: (userCoins + coinsBet));
            }
            
            await ReplyAsync("", false, eb.Build());

        }
    }
}
