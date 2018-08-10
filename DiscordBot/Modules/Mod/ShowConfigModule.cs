using System;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;

using DiscordBot.Common;
using DiscordBot.Common.Preconditions;
using DiscordBot.Extensions;

using MelissaNet;

namespace DiscordBot.Modules.Mod
{
    [Name("Show Settings Commands")]
    [Group("showconfig")]
    [RequireContext(ContextType.Guild)]
    [MinPermissions(PermissionLevel.ServerMod)]
    public class ShowConfigModule : ModuleBase
    {
        [Command("")]
        public async Task ShowConfig()
        {
            await ReplyAsync("**Syntax:** " +
                             GuildConfiguration.Load(Context.Guild.Id).Prefix + "showconfig [config]\n```INI\n" +
                             "Available Commands\n" +
                             "-----------------------------\n" +
                             "[ 1] showconfig all\n" +
                             "[ 2] showconfig bot\n" +
                             "[ 3] showconfig guild\n" +
                             "[ 4] showconfig strings\n" +
                             "```");
        }

        [Command("all")]
        public async Task ShowAllConfigs()
        {
            await ShowBotConfig();
            await ShowGuildConfig();
            await ShowStringConfiguration();
        }

        [Command("bot")]
        public async Task ShowBotConfig()
        {
            EmbedBuilder eb = new EmbedBuilder()
                .WithTitle("Bot Configuration")
                .WithFooter("Bot Owner permissions required to change these variables!");

            eb.WithDescription("```INI\n" +
                               "[ 1] Developer [ " + (Configuration.Load().Developer.GetUser().Username ?? "Melissa") + " ]\n" +
                               "[ 2] Developer ID [ " + Configuration.Load().Developer + " ]\n" +
                               "[ 3] Status Text [ " + (Configuration.Load().StatusText ?? "") + " ]\n" +
                               "[ 4] Status Link [ " + (Configuration.Load().StatusLink ?? "") + " ]\n" +
                               "[ 5] Status Activity [ " + Configuration.Load().StatusActivity.ToActivityType() + " ]\n" +
                               "[ 6] Status [ " + Configuration.Load().Status + " ]\n" +
                               "[ 7] Unknown Command Enabled [ " + Configuration.Load().UnknownCommandEnabled.ToYesNo() + " ]\n" +
                               "[ 8] Awarding Coins Enabled [ " + Configuration.Load().AwardingCoinsEnabled.ToYesNo() + " ]\n" +
                               "[ 9] Awarding Tokens Enabled [ " + Configuration.Load().AwardingTokensEnabled.ToYesNo() + " ]\n" +
                               "[10] Leaderboard Amount [ " + Configuration.Load().LeaderboardAmount + " ]\n" +
                               "[11] Quote Cost [ " + Configuration.Load().QuoteCost + " coin(s) ]\n" +
                               "[12] Prefix Cost [ " + Configuration.Load().PrefixCost + " coin(s) ]\n" +
                               "[13] RGB Cost [ " + Configuration.Load().RGBCost + " coin(s) ]\n" +
                               "[14] Senpai Chance Rate [ " + Configuration.Load().SenpaiChanceRate + "/100 ]\n" +
                               "[15] Global Log Channel [ #" + (Configuration.Load().LogChannelId.GetTextChannel().Name ?? "UNDEFINED") + " ]\n" +
                               "[16] Global Log Channel ID [ " + Configuration.Load().LogChannelId + " ]\n" +
                               "[17] Respects [ " + Configuration.Load().Respects + " ]\n" +
                               "[18] Min Length For Coin(s) [ " + Configuration.Load().MinLengthForCoin + " ]\n" +
                               "[19] Max Rule34 Gamble ID [ " + Configuration.Load().MaxRuleXGamble + " ]\n" +
                               "```");

            await ReplyAsync("", false, eb.Build());

        }

        [Command("strings")]
        public async Task ShowStringConfiguration()
        {
            EmbedBuilder eb = new EmbedBuilder()
                .WithTitle("String Configuration")
                .WithFooter("Bot Owner permissions required to change these variables!");

            eb.WithDescription("```INI\n" +
                               "[ 1] Default Website Name [ " + (StringConfiguration.Load().DefaultWebsiteName ?? "UNDEFINED") + " ]\n" +
                               "```");

            await ReplyAsync("", false, eb.Build());
        }

        [Command("guild")]
        public async Task ShowGuildConfig()
        {
            try
            {
                EmbedBuilder eb = new EmbedBuilder()
                    .WithTitle("Guild Configuration")
                    .WithFooter("Guild Configuration can be edited by Guild Owner/Administrator");

                eb.WithDescription("```INI\n" +
                                   "[ 1] Prefix [ " + (GuildConfiguration.Load(Context.Guild.Id).Prefix ?? "UNDEFINED") + " ]\n" +
                                   "[ 2] Welcome Channel [ #" + (GuildConfiguration.Load(Context.Guild.Id).WelcomeChannelId.GetTextChannel().Name ?? "UNDEFINED") + " ]\n" +
                                   "[ 3] Welcome Channel ID [ " + (GuildConfiguration.Load(Context.Guild.Id).WelcomeChannelId) + " ]\n" +
                                   "[ 4] Log Channel [ #" + (GuildConfiguration.Load(Context.Guild.Id).LogChannelId.GetTextChannel().Name ?? "UNDEFINED") + " ]\n" +
                                   "[ 5] Log Channel ID [ " + (GuildConfiguration.Load(Context.Guild.Id).LogChannelId) + " ]\n" +
                                   "[ 6] Bot Channel [ #" + (GuildConfiguration.Load(Context.Guild.Id).BotChannelId.GetTextChannel().Name ?? "UNDEFINED") + " ]\n" +
                                   "[ 7] Bot Channel ID [ " + (GuildConfiguration.Load(Context.Guild.Id).BotChannelId) + " ]\n" +
                                   "[ 8] Senpai Enabled [ " + GuildConfiguration.Load(Context.Guild.Id).SenpaiEnabled.ToYesNo() + " ]\n" +
                                   "[ 9] Quotes Enabled [ " + GuildConfiguration.Load(Context.Guild.Id).QuotesEnabled.ToYesNo() + " ]\n" +
                                   "[10] NSFW Commands Enabled [ " + GuildConfiguration.Load(Context.Guild.Id).EnableNsfwCommands.ToYesNo() + " ]\n" +
                                   "[11] Rule34 Channel [ #" + (GuildConfiguration.Load(Context.Guild.Id).RuleGambleChannelId.GetTextChannel().Name ?? "UNDEFINED") + " ]\n" +
                                   "[12] Rule34 Channel ID [ " + (GuildConfiguration.Load(Context.Guild.Id).RuleGambleChannelId) + " ]\n" +
                                   "```");

                await ReplyAsync("", false, eb.Build());
            }
            catch (Exception e)
            {
                await ReplyAsync(
                    "It appears that your Guild Configuration has not been set-up completely. Please complete all the steps before using this command.");
                Console.WriteLine(e);
                Console.WriteLine(e.Message);
            }
        }
    }
}
