using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using DiscordBot.Common.Preconditions;
using DiscordBot.Common;
using DiscordBot.Extensions;

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
                                 GuildConfiguration.Load(Context.Guild.Id).Prefix + "editconfig [variable] [command syntax]\n```" +
                                 "Available Commands\n" +
                                 "-----------------------------\n" +
                                 "-> editconfig activity [activity type no] [activity message] [activity link]\n" +
                                 "• activity type no: -1 - Disabled, 0 - Playing, 1 - Streaming, 2 - Listening, 3 - Watching\n" +
                                 "-> editconfig status [status]\n" +
                                 "• status: online, donotdisturb, idle, invisible\n" +
                                 "-> editconfig toggleunknowncommand\n" +
                                 "-> editconfig leaderboardamount [number of users to display]\n" +
                                 "-> editconfig quoteprice [price to add quote]\n" +
                                 "-> editconfig prefixprice [price to change custom prefix]\n" +
                                 "-> editconfig senpaichance [number 1-100]\n" +
                                 "-> editconfig globallogchannel [channel mention / channel id]\n" +
                                 "-> editconfig rule34 [max number for random to use]\n" +
                                 "-> editconfig minlengthforcoins [string length for coins]\n" +
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
                    await MogiiBot3.Bot.SetGameAsync(activityMessage);
                }
                else
                {
                    await MogiiBot3.Bot.SetGameAsync(activityMessage, activityLink, (ActivityType)activityType);
                }

                var eb = new EmbedBuilder()
                    .WithDescription(Context.User.Username + " updated " + MogiiBot3.Bot.CurrentUser.Mention + "'s activity message.")
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
                    await MogiiBot3.Bot.SetStatusAsync(UserStatus.Online);
                    await ReplyAsync("Status updated to Online, " + Context.User.Mention);
                }

                [Command("donotdisturb"), Summary("Sets the bot's status to do not disturb.")]
                [Alias("dnd", "disturb", "red")]
                public async Task SetBusy()
                {
                    Configuration.UpdateConfiguration(status: UserStatus.DoNotDisturb);
                    await MogiiBot3.Bot.SetStatusAsync(UserStatus.DoNotDisturb);
                    await ReplyAsync("Status updated to Do Not Disturb, " + Context.User.Mention);
                }

                [Command("idle"), Summary("Sets the bot's status to idle.")]
                [Alias("afk", "yellow")]
                public async Task SetIdle()
                {
                    Configuration.UpdateConfiguration(status: UserStatus.AFK);
                    await MogiiBot3.Bot.SetStatusAsync(UserStatus.AFK);
                    await ReplyAsync("Status updated to Idle, " + Context.User.Mention);
                }

                [Command("invisible"), Summary("Sets the bot's status to invisible.")]
                [Alias("hidden", "offline", "grey")]
                public async Task SetInvisible()
                {
                    Configuration.UpdateConfiguration(status: UserStatus.Invisible);
                    await MogiiBot3.Bot.SetStatusAsync(UserStatus.Invisible);
                    await ReplyAsync("Status updated to Invisible, " + Context.User.Mention);
                }
            }

            [Command("toggleunknowncommand"), Summary("Toggles the unknown command message.")]
            public async Task ToggleUc()
            {
                Configuration.UpdateConfiguration(unknownCommandEnabled: !Configuration.Load().UnknownCommandEnabled);
                await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("UnknownCommand has been toggled by " + Context.User.Mention + " (enabled: " + Configuration.Load().UnknownCommandEnabled + ")");
            }

            [Command("leaderboardamount"), Summary("Set the amount of users who show up in the leaderboards.")]
            public async Task SetLeaderboardAmount(int value)
            {
                int oldValue = Configuration.Load().LeaderboardAmount;
                Configuration.UpdateConfiguration(leaderboardAmount: value);
                await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync(Context.User.Mention + " has updated the Leaderboard amount to: " + value + " (was: " + oldValue + ")");
            }

            [Command("quoteprice"), Summary("")]
            [Alias("changequoteprice", "updatequoteprice")]
            public async Task ChangeQuotePrice(int price)
            {
                int oldPrice = Configuration.Load().QuoteCost;
                Configuration.UpdateConfiguration(quoteCost: price);
                await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("**" + Context.User.Mention + "** has updated the quote cost to **" + price + "** coins. (Was: **" + oldPrice + "** coins)");
            }

            [Command("prefixprice"), Summary("")]
            [Alias("changeprefixprice", "updateprefixprice")]
            public async Task ChangePrefixPrice(int price)
            {
                int oldPrice = Configuration.Load().PrefixCost;
                Configuration.UpdateConfiguration(prefixCost: price);
                await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("**" + Context.User.Mention + "** has updated the prefix cost to **" + price + "** coins. (Was: **" + oldPrice + "** coins)");
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
