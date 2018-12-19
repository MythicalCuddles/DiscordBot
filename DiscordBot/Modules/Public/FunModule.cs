using System;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;

using DiscordBot.Common;
using DiscordBot.Common.Preconditions;
using DiscordBot.Extensions;
using DiscordBot.Other;

namespace DiscordBot.Modules.Public
{
    [Name("Fun Commands")]
    [MinPermissions(PermissionLevel.User)]
    [RequireContext(ContextType.Guild)]
    public class FunModule : ModuleBase
    {
        private readonly Random _random = new Random();
        
        [Command("rolldice"), Summary("Rolls a x sided dice.")]
        public async Task RollDice(int numberOfDice = 1)
        {
            if (numberOfDice > 10)
            {
                await ReplyAsync("You can not roll more than 10 dice at one time, " + Context.User.Mention);
                numberOfDice = 10;
            }
            else if(numberOfDice < 1)
            {
                await ReplyAsync("You need to roll at least 1 dice, " + Context.User.Mention);
                return;
            }

            EmbedBuilder eb = new EmbedBuilder()
                .WithDescription(Context.User.Mention + " rolled " + numberOfDice + " 6-sided dice.");

            int totalOfRoll = 0;
            for(int i = 0; i < numberOfDice; i++)
            {
                var roll = _random.RandomNumber(1, 6);
                totalOfRoll += roll;

                eb.AddField("Dice " + (i + 1), roll.ToString(), true);
            }

            eb.AddField("Sum of roll", totalOfRoll);
            eb.WithFooter("Did you know? You can roll more dice by doing \"" +
                          GuildConfiguration.Load(Context.Guild.Id).Prefix + "rolldice [number of dice]\"!");

            await ReplyAsync("", false, eb.Build());
        }

        [Command("dice20"), Summary("Rolls a 20 sided dice.")]
        [Alias("d20")]
        public async Task Roll20Dice()
        {
            int value = _random.RandomNumber(1, 20);
            await ReplyAsync("A 20-sided dice was rolled, and landed on: " + value);
        }

        [Command("flipcoin"), Summary("Flips a two sided coin.")]
        [Alias("flip", "tosscoin", "coinflip")]
        public async Task FlipCoin()
        {
            var value = _random.RandomNumber(1, 2);

            switch (value)
            {
                case 1:
                    await ReplyAsync("A coin has been flipped, and landed on: **Heads**");
                    break;
                case 2:
                    await ReplyAsync("A coin has been flipped, and landed on: **Tails**");
                    break;
                default:
                    await ReplyAsync("A coin had been flipped, but got lost while landing.");
                    break;
            }
        }

        #region Images & Randoms
        [Command("approve"), Summary("Sends a picture stating \"Harold Likes This\".")]
        public async Task HaroldApproves()
        {
            await ReplyAsync("https://i.imgur.com/wHrJIfT.png");
        }

        [Command("nice"), Summary("NOICE!")]
        public async Task Noice()
        {
            await ReplyAsync("https://media.giphy.com/media/yJFeycRK2DB4c/giphy.gif");
        }

        [Command("disagree"), Summary("")]
        public async Task Disagree()
        {
            await ReplyAsync("https://i.imgur.com/3qfnX5M.png");
        }

        [Command("hehe"), Summary("Hehe!")]
        public async Task Hehe()
        {
            await ReplyAsync("https://media.giphy.com/media/9MFsKQ8A6HCN2/giphy.gif");
        }

        [Command("gitgud"), Summary("git gud")]
        public async Task Gitgud()
        {
            await ReplyAsync("https://cdn.discordapp.com/attachments/235124701964271618/310169979083423744/received_10206534636842919.jpeg");
        }

        [Command("wat"), Summary("")]
        public async Task Wat()
        {
            await ReplyAsync("https://giphy.com/gifs/wat-loop-old-woman-jA8TT03Sj2pXO");
        }

        [Command("why"), Summary("y tho")]
        public async Task WhyTho()
        {
            await ReplyAsync("https://i.imgur.com/W2l8dvp.png");
        }
		
		[Command("lmao"), Summary("Sends the lmao image.")]
		public async Task Lmao()
		{
			await ReplyAsync("https://i.imgur.com/zBoZFbJ.png");
		}

		[Command("groot"), Summary("Sends a picture of baby groot dancing.")]
        public async Task BabyGroot()
        {
            await ReplyAsync("https://media.giphy.com/media/14b13BDH3V81wc/giphy.gif");
        }

        [Command("answer"), Summary("THE ANSWER TO LIFE, THE UNIVERSE AND EVERYTHING")]
        [Alias("question")]
        public async Task UltimateQuestionOfLifeTheUniverseAndEverything()
        {
            await ReplyAsync("42");
        }
        #endregion 

        [Command("noticeme"), Summary("Will Senpai notice you?")]
        public async Task Senpai()
        {
            if (GuildConfiguration.Load(Context.Guild.Id).SenpaiEnabled)
            {
                if (_random.RandomNumber(0, 100) <= Configuration.Load().SenpaiChanceRate)
                {
					await ReplyAsync(Context.User.Mention + ", Senpai has noticed you!");
                }
                else
                {
                    await ReplyAsync(Context.User.Mention + ", Senpai has not noticed you this time around...");
                }
            }
            else
            {
                await ReplyAsync(Context.User.Mention + ", Senpai can not notice you if Senpai is in bed sleeping.");
            }
        }
        
        [Command("quote"), Summary("Get a random quote from the list.")]
        public async Task GenerateQuote()
        {
            if (GuildConfiguration.Load(Context.Guild.Id).QuotesEnabled)
            {
                int generatedNumber = _random.Next(0, QuoteHandler.QuoteList.Count);

                await ReplyAsync(QuoteHandler.QuoteList[generatedNumber]);
            }
            else
            {
                await ReplyAsync("Quotes are currently disabled. Try again later.");
            }
        }

        [Command("buyquote"), Summary("Request a quote to be added for a price.")]
        public async Task RequestToAddQuote([Remainder]string quote = null)
		{
		    if (GuildConfiguration.Load(Context.Guild.Id).QuotesEnabled)
		    {
		        int userLevel = User.Load(Context.User.Id).Level;
		        int quoteLevelRequirement = Configuration.Load().QuoteLevelRequirement;
		        
                
		        if(userLevel < quoteLevelRequirement)
		        {
		            await ReplyAsync(Context.User.Mention + ", you need to be level " + quoteLevelRequirement + "+ to add a quote request."); 
		            return;
		        }

                if (quote == null)
		        {
		            await ReplyAsync("**Syntax:** " +
		                             GuildConfiguration.Load(Context.Guild.Id).Prefix + "buyquote [quote]\n```" +
		                             "**Information:**\n" +
		                             "-----------------------------\n" +
		                             "• Your quote will not be added instantly to the list. A staff member must first verify that it is safe to put on the list.\n" +
		                             "```");
		            return;
		        }

		        QuoteHandler.AddAndUpdateRequestQuotes(quote);
		        await ReplyAsync(Context.User.Mention + ", your quote has been added to the list, and should be verified by a staff member shortly.");

		        await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("**New Quote**\nQuote requested by: **" + Context.User.Mention + "**\nQuote: " + quote);
		        await GuildConfiguration.Load(Context.Guild.Id).LogChannelId.GetTextChannel().SendMessageAsync("**New Quote**\n" + quote + "\n\n*Do " + GuildConfiguration.Load(Context.Guild.Id).Prefix + "listrequestquotes to view the ID and other quotes.*");
            }
		    else
		    {
		        await ReplyAsync("Quotes are currently disabled. Try again later.");
		    }
        }
    }
}
