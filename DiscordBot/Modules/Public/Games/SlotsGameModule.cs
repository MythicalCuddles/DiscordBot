using System;
using System.Text;
using System.Threading.Tasks;

using Discord.Commands;

using DiscordBot.Common.Preconditions;
using DiscordBot.Common;
using DiscordBot.Logging;

namespace DiscordBot.Modules.Public.Games
{
    [Name("Slot Game Commands")]
    [MinPermissions(PermissionLevel.User)]
    [RequireContext(ContextType.Guild)]
    public class SlotsGameModule : ModuleBase
    {
        private readonly Random _random = new Random();

        [Command("buyslots"), Summary("")]
        [Alias("slots")]
        public async Task PlaySlots(int inputCoins)
        {
            if (inputCoins < 1)
            {
                await ReplyAsync("You can not play with " + inputCoins + " coin(s)!");
                return;
            }

            if (inputCoins > User.Load(Context.User.Id).Coins)
            {
                await ReplyAsync(Context.User.Mention + ", you don't have enough coin(s) to play with.");
                return;
            }

            int slotEmotesCount = Extensions.Extensions.SlotEmotes.Count;
            int one = _random.Next(0, slotEmotesCount), two = _random.Next(0, slotEmotesCount), three = _random.Next(0, slotEmotesCount);

            StringBuilder sb = new StringBuilder()
                .Append("**[  :slot_machine: l SLOTS ]**\n")
                .Append("------------------\n")
                .Append(Extensions.Extensions.SlotEmotes[_random.Next(0, slotEmotesCount)] + " : " + Extensions.Extensions.SlotEmotes[_random.Next(0, slotEmotesCount)] + " : " + Extensions.Extensions.SlotEmotes[_random.Next(0, slotEmotesCount)] + "\n")
                .Append(Extensions.Extensions.SlotEmotes[one] + " : " + Extensions.Extensions.SlotEmotes[two] + " : " + Extensions.Extensions.SlotEmotes[three] + " :arrow_left: \n")
                .Append(Extensions.Extensions.SlotEmotes[_random.Next(0, slotEmotesCount)] + " : " + Extensions.Extensions.SlotEmotes[_random.Next(0, slotEmotesCount)] + " : " + Extensions.Extensions.SlotEmotes[_random.Next(0, slotEmotesCount)] + "\n")
                .Append("------------------\n");
            
            User.UpdateUser(Context.User.Id, coins: (User.Load(Context.User.Id).Coins - inputCoins));

            if (one == two && two == three)
            {
                sb.Append("| : : : : **WIN** : : : : |\n\n");
                int coinsWon = (inputCoins * 2) + inputCoins;
                sb.Append("**" + Context.User.Username + "** bet **" + inputCoins + "** coin(s) and won **" + coinsWon + "** coin(s).");
                
                User.UpdateUser(Context.User.Id, coins: (User.Load(Context.User.Id).Coins + coinsWon));
                TransactionLogger.AddTransaction(Context.User.Username + " [" + Context.User.Id + "] bet " + inputCoins + " coins on the slots and won " + coinsWon + " coins. [ALL]");
            }
            else if (one == two || two == three || one == three)
            {
                sb.Append("| : : : : **WIN** : : : : |\n\n");
                int coinsWon = (inputCoins) + inputCoins;
                sb.Append("**" + Context.User.Username + "** bet **" + inputCoins + "** coin(s) and won **" + coinsWon + "** coin(s).");
                
                User.UpdateUser(Context.User.Id, coins: (User.Load(Context.User.Id).Coins + coinsWon));
                TransactionLogger.AddTransaction(Context.User.Username + " [" + Context.User.Id + "] bet " + inputCoins + " coins on the slots and won " + coinsWon + " coins. [TWO]");
            }
            else
            {
                sb.Append("| : : :  **LOST**  : : : |\n\n");
                sb.Append("**" + Context.User.Username + "** bet **" + inputCoins + "** coin(s) and lost.");
                TransactionLogger.AddTransaction(Context.User.Username + " [" + Context.User.Id + "] bet " + inputCoins + " coins on the slots and lost. [NONE]");
            }

            await ReplyAsync(sb.ToString());
        }
    }
}
