using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using DiscordBot.Common.Preconditions;
using DiscordBot.Common;
using DiscordBot.Extensions;
using MelissaNet;

namespace DiscordBot.Modules.Owner
{
    [Name("Configuration Commands")]
    [MinPermissions(PermissionLevel.BotOwner)]
    public class ConfigModule : ModuleBase
    {
        [Group("editconfig")]
        public class ConfigurationModule : ModuleBase
        {
            [Command("")]
            public async Task SendSyntax()
            {
                await ReplyAsync("**Syntax:** " +
                                 GuildConfiguration.Load(Context.Guild.Id).Prefix + "editconfig [command] [command syntax]\n```INI\n" +
                                 "Available Commands\n" +
                                 "-----------------------------\n" +
                                 "[ 1] activity [activity type no] [activity message] [activity link]\n" +
                                 "[a.] [activity type no] - [-1 Disabled] [0 Playing] [1 Streaming] [2 Listening] [3 Watching]\n" +
                                 "[ 2] status [status]\n" +
                                 "[a.] [status] - [online] [donotdisturb] [idle] [invisible]\n" +
                                 "[ 3] toggleunknowncommand\n" +
                                 "[ 4] leaderboardamount [number of users to display]\n" +
                                 "[ 5] quoteprice [price to add quote]\n" +
                                 "[ 6] prefixprice [price to change custom prefix]\n" +
                                 "[ 7] rgbprice [price to change custom RGB]\n" +
                                 "[ 8] senpaichance [number 1-100]\n" +
                                 "[ 9] globallogchannel [channel mention / channel id]\n" +
                                 "[10] rule34 [max number for random to use]\n" +
                                 "[11] minlengthforcoins [string length for coins]\n" +
                                 "[12] leaderboardtrophyurl [link]\n" +
                                 "[13] leaderboardembedcolor [uint id]\n" +
                                 "[14] toggleawardingcoins\n" +
                                 "[15] toggleawardingtokens\n" +
                                 "```");
            }

            [Command("activity"), Summary("Changes the playing message of the bot, and changes it to streaming mode if twitch link is inserted.")]
            public async Task SetGameActivity(int activityType = -1, string activityMessage = null, string activityLink = null)
            {
                if (activityType != -1 && activityMessage == null)
                {
                    await ReplyAsync("**Syntax:** " + GuildConfiguration.Load(Context.Guild.Id).Prefix + "editconfig activity [activity type no] [activity message] [activity link]\n\n" +
                                     "```Activity Types: \n-1 - Disabled\n0 - Playing\n1 - Streaming\n2 - Listening\n3 - Watching");
                    return;
                }

                Configuration.UpdateConfiguration(statusText: activityMessage);
                Configuration.UpdateConfiguration(statusLink: activityLink);
                Configuration.UpdateConfiguration(statusActivity: activityType);

                if (activityType == -1)
                {
                    await DiscordBot.Bot.SetGameAsync(activityMessage);
                }
                else
                {
                    await DiscordBot.Bot.SetGameAsync(activityMessage, activityLink, (ActivityType)activityType);
                }

                var eb = new EmbedBuilder()
                    .WithDescription(Context.User.Username + " updated " + DiscordBot.Bot.CurrentUser.Mention + "'s activity message.")
                    .WithColor(Color.DarkGreen);

                await ReplyAsync("", false, eb.Build());
            }

            [Group("status")]
            public class StatusModule : ModuleBase
            {
                [Command("online"), Summary("Sets the bot's status to online.")]
                [Alias("active", "green")]
                public async Task SetOnline()
                {
                    Configuration.UpdateConfiguration(status: UserStatus.Online);
                    await DiscordBot.Bot.SetStatusAsync(UserStatus.Online);
                    await ReplyAsync("Status updated to Online, " + Context.User.Mention);
                }

                [Command("donotdisturb"), Summary("Sets the bot's status to do not disturb.")]
                [Alias("dnd", "disturb", "red")]
                public async Task SetBusy()
                {
                    Configuration.UpdateConfiguration(status: UserStatus.DoNotDisturb);
                    await DiscordBot.Bot.SetStatusAsync(UserStatus.DoNotDisturb);
                    await ReplyAsync("Status updated to Do Not Disturb, " + Context.User.Mention);
                }

                [Command("idle"), Summary("Sets the bot's status to idle.")]
                [Alias("afk", "yellow")]
                public async Task SetIdle()
                {
                    Configuration.UpdateConfiguration(status: UserStatus.AFK);
                    await DiscordBot.Bot.SetStatusAsync(UserStatus.AFK);
                    await ReplyAsync("Status updated to Idle, " + Context.User.Mention);
                }

                [Command("invisible"), Summary("Sets the bot's status to invisible.")]
                [Alias("hidden", "offline", "grey")]
                public async Task SetInvisible()
                {
                    Configuration.UpdateConfiguration(status: UserStatus.Invisible);
                    await DiscordBot.Bot.SetStatusAsync(UserStatus.Invisible);
                    await ReplyAsync("Status updated to Invisible, " + Context.User.Mention);
                }
            }

            [Command("toggleunknowncommand"), Summary("Toggles the unknown command message.")]
            public async Task ToggleUc()
            {
                Configuration.UpdateConfiguration(unknownCommandEnabled: !Configuration.Load().UnknownCommandEnabled);
                await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("UnknownCommand has been toggled by " + Context.User.Mention + " (enabled: " + Configuration.Load().UnknownCommandEnabled.ToYesNo() + ")");
            }

            [Command("leaderboardamount"), Summary("Set the amount of users who show up in the leaderboards.")]
            public async Task SetLeaderboardAmount(int value)
            {
                int oldValue = Configuration.Load().LeaderboardAmount;
                Configuration.UpdateConfiguration(leaderboardAmount: value);
                await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync(Context.User.Mention + " has updated the Leaderboard amount to: " + value + " (was: " + oldValue + ")");
            }

            [Command("quoteprice"), Summary("")]
            public async Task ChangeQuotePrice(int price)
            {
                int oldPrice = Configuration.Load().QuoteCost;
                Configuration.UpdateConfiguration(quoteCost: price);
                await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("**" + Context.User.Mention + "** has updated the quote cost to **" + price + "** coins. (Was: **" + oldPrice + "** coins)");
            }

            [Command("prefixprice"), Summary("")]
            public async Task ChangePrefixPrice(int price)
            {
                int oldPrice = Configuration.Load().PrefixCost;
                Configuration.UpdateConfiguration(prefixCost: price);
                await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("**" + Context.User.Mention + "** has updated the prefix cost to **" + price + "** coins. (Was: **" + oldPrice + "** coins)");
            }

            [Command("rgbprice"), Summary("")]
            public async Task ChangeRGBPrice(int price)
            {
                int oldPrice = Configuration.Load().RGBCost;
                Configuration.UpdateConfiguration(rgbCost: price);
                await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("**" + Context.User.Mention + "** has updated the RGB cost to **" + price + "** coins. (Was: **" + oldPrice + "** coins)");
            }

            [Command("senpaichance"), Summary("")]
            public async Task ChangeSenpaiChance(int chanceValue)
            {
                int oldChance = Configuration.Load().SenpaiChanceRate;
                Configuration.UpdateConfiguration(senpaiChanceRate: chanceValue);
                await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("**" + Context.User.Mention + "** has updated the senpai chance to **" + chanceValue + "%**. (Was: **" + oldChance + "%**)");
            }

            [Command("globallogchannel"), Summary("")]
            public async Task SetGlobalLogChannel(SocketTextChannel channel)
            {
                Configuration.UpdateConfiguration(logChannelId: channel.Id);
                await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync(Context.User.Mention + " has updated \"LogChannelID\" to: " + channel.Mention);
            }

            [Command("rule34"), Summary("Set the max random value for the Rule34 Gamble.")]
            public async Task SetRule34Max(int value)
            {
                int oldValue = Configuration.Load().MaxRuleXGamble;
                Configuration.UpdateConfiguration(maxRuleXGamble: value);
                await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync(Context.User.Mention + " has updated the Rule34 Max to: " + value + " (was: " + oldValue + ")");
            }

            [Command("minlengthforcoins"), Summary("Set the required length of a message for a user to receive (a) coin(s).")]
            public async Task SetRequiredMessageLengthForCoins(int value)
            {
                int oldValue = Configuration.Load().MinLengthForCoin;
                Configuration.UpdateConfiguration(minLengthForCoin: value);

                await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync(Context.User.Mention + " has updated the MinLengthForCoin amount to: " + value + " (was: " + oldValue + ")");
            }

            [Command("toggleawardingcoins"), Summary("Toggles if users receive coins.")]
            public async Task ToggleAwardingCoins()
            {
                Configuration.UpdateConfiguration(awardingCoinsEnabled: !Configuration.Load().AwardingCoinsEnabled);
                await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("Awarding Coins has been toggled by " + Context.User.Mention + " (enabled: " + Configuration.Load().AwardingCoinsEnabled.ToYesNo() + ")");
            }

            [Command("toggleawardingtokens"), Summary("Toggles if users receive tokens.")]
            public async Task ToggleAwardingTokens()
            {
                Configuration.UpdateConfiguration(awardingTokensEnabled: !Configuration.Load().AwardingTokensEnabled);
                await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("Awarding Tokens has been toggled by " + Context.User.Mention + " (enabled: " + Configuration.Load().AwardingTokensEnabled.ToYesNo() + ")");
            }

            [Command("leaderboardtrophyurl"), Summary("")]
            public async Task SetLeaderboardTrophyUrl(string link)
            {
                string oldValue = Configuration.Load().LeaderboardTrophyUrl;
                Configuration.UpdateConfiguration(leaderboardTrophyUrl: link);
                await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync(Context.User.Mention + " has updated the Leaderboard Trophy URL to: " + link + " (was: " + oldValue + ")");
            }

            [Command("leaderboardembedcolor"), Summary("")]
            public async Task SetLeaderboardEmbedColor(uint colorId)
            {
                uint oldValue = Configuration.Load().LeaderboardEmbedColor;
                Configuration.UpdateConfiguration(leaderboardEmbedColor: colorId);
                await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync(Context.User.Mention + " has updated the Leaderboard Embed Color ID to: " + colorId + " (was: " + oldValue + ")");
            }
        }

        [Group("editstring")]
        public class StringsConfigurationModule : ModuleBase
        {
            [Command("")]
            public async Task SendSyntax()
            {
                await ReplyAsync("**Syntax:** " +
                                 GuildConfiguration.Load(Context.Guild.Id).Prefix + "editstring [variable] [command syntax]\n```" +
                                 "Available Commands\n" +
                                 "-----------------------------\n" +
                                 "-> editstring DefaultWebsiteName [name]\n" +
                                 "```");
            }

            [Command("defaultwebsitename"), Summary("Sets the default name for users website.")]
            public async Task SetDefaultWebsiteName([Remainder] string name = null)
            {
                StringConfiguration.UpdateConfiguration(websiteName: name);
                await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("DefaultWebsiteName has been changed by " + Context.User.Mention + " to " + name);
            }
        }
    }
}
