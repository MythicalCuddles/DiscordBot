using System;
using System.IO;
using Discord;
using DiscordBot.Extensions;
using Newtonsoft.Json;

namespace DiscordBot.Common
{
    public class GuildConfiguration
    {
        public string Prefix { get; set; } = "$";
        public string WelcomeMessage { get; set; } = "";
        public ulong WelcomeChannelId { get; set; } = 447769497344933900;
        public ulong LogChannelId { get; set; } = 447769497344933900;
        public ulong BotChannelId { get; set; } = 447769497344933900;

        public bool SenpaiEnabled { get; set; } = true;
        public bool QuotesEnabled { get; set; } = true;

        public bool EnableNsfwCommands { get; set; }
        public ulong RuleGambleChannelId { get; set; } = 447769497344933900;

        public static void EnsureExists(ulong guildId)
        {
            string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), GetPath(guildId));
            if (!File.Exists(file))
            {
                string path = Path.GetDirectoryName(file);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var guildConfig = new GuildConfiguration();
                guildConfig.SaveJson(guildId);

                Console.Write(@"status: [");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write(@"ok");
                Console.ResetColor();
                Console.WriteLine(@"]    " + GetPath(guildId) + @": created.");


                EmbedBuilder eb = new EmbedBuilder()
                .WithTitle("Thank you for adding " + DiscordBot.Bot.CurrentUser.Username + " to your guild!")
                .WithDescription("Congratulations on adding " + DiscordBot.Bot.CurrentUser.Username + " to " + guildId.GetGuild().Name + "! Please follow the steps below to configure me!" +
                                 "```INI\n" +
                                 "[1] Prefix: You can change the default prefix by typing \"" + Load(guildId).Prefix + "guildprefix [prefix]\"\n" +
                                 "[2] Welcome Message: Type \"" + Load(guildId).Prefix + "setwelcomemessage\" to view flags and see how to set up the welcome message.\n" +
                                 "[3] Welcome Channel: Set the channel the welcome message is posted by typing \"" + Load(guildId).Prefix + "welcomechannel [channel mention]\"\n" +
                                 "[4] Log Channel: We now need a channel where we can post things for your eyes only! Type \"" + Load(guildId).Prefix + "logchannel [channel mention]\"\n" +
                                 "-- Optional --\n" +
                                 "[5] Senpai Command: You can toggle senpai by typing \"" + Load(guildId).Prefix + "togglesenpai\"\n" +
                                 "[6] Quote Command: You can toggle quotes by typing \"" + Load(guildId).Prefix + "togglequotes\"\n" +
                                 "\n[More] If you're interested in setting up NSFW commands and changing other settings, please visit the wiki.\n" +
                                 "```")
                .WithFooter("Warning: Server Owner's may only change the configuration for the guild.")
                .WithThumbnailUrl(DiscordBot.Bot.CurrentUser.GetAvatarUrl())
                .WithColor(56, 226, 40);
                guildId.GetGuild().DefaultChannel.SendMessageAsync("", false, eb.Build());

                eb = new EmbedBuilder()
                    .WithTitle("Seen this message before?")
                    .WithDescription("We apologise for the inconvience. Seeing this message again means that your guild configuration files have been reset." +
                                     "Due to this bot being constantly updated, this might happen more than we, or you, would like." +
                                     "\n\n" +
                                     "It's easy to fix however. Please follow the steps above or head to the wiki and follow the quick setup guide again and your server will be good to go once more!" +
                                     "\n\n" +
                                     "Sorry about that.\n- Melissa (@" + Configuration.Load().Developer.GetUser().Username + "#" + Configuration.Load().Developer.GetUser().Discriminator + ")\n" +
                                     "\t& the rest of the MythicalCuddlesXYZ Team")
                    .WithColor(244, 226, 66)
                    .WithCurrentTimestamp()
                    .WithFooter("This message was posted here as this is your guild's default channel.");
                guildId.GetGuild().DefaultChannel.SendMessageAsync("", false, eb.Build());


            }

            Console.Write(@"status: [");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write(@"ok");
            Console.ResetColor();
            Console.WriteLine(@"]    " + GetPath(guildId) + @": loaded.");
        }

        private static string GetPath(ulong guildId)
        {
            return "MythicalCuddles/DiscordBot/config/guilds/" + guildId + ".json";
        }

        public void SaveJson(ulong guildId)
        {
            string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), GetPath(guildId));
            File.WriteAllText(file, ToJson());
        }

        public static GuildConfiguration Load(ulong guildId)
        {
            string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), GetPath(guildId));
            return JsonConvert.DeserializeObject<GuildConfiguration>(File.ReadAllText(file));
        }

        public string ToJson()
            => JsonConvert.SerializeObject(this, Formatting.Indented);

        public static void UpdateGuild(ulong guildId, string prefix = null, string welcomeMessage = null,
            ulong? welcomeChannelId = null, ulong? logChannelId = null,
            bool? senpaiEnabled = null, bool? quotesEnabled = false, bool? enableNsfwCommands = null, 
            ulong? ruleGameChannelId = null)
        {
            var guild = new GuildConfiguration()
            {
                Prefix = prefix ?? Load(guildId).Prefix,
                WelcomeMessage = welcomeMessage ?? Load(guildId).WelcomeMessage,
                WelcomeChannelId = welcomeChannelId ?? Load(guildId).WelcomeChannelId,
                LogChannelId = logChannelId ?? Load(guildId).LogChannelId,
                SenpaiEnabled = senpaiEnabled ?? Load(guildId).SenpaiEnabled,
                QuotesEnabled = quotesEnabled ?? Load(guildId).QuotesEnabled,
                EnableNsfwCommands = enableNsfwCommands ?? Load(guildId).EnableNsfwCommands,
                RuleGambleChannelId = ruleGameChannelId ?? Load(guildId).RuleGambleChannelId
            };
            guild.SaveJson(guildId);
        }
    }
}
