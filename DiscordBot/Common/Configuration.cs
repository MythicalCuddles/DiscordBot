using System;
using System.IO;

using Newtonsoft.Json;

using Discord;
using DiscordBot.Extensions;
using MelissaNet;

namespace DiscordBot.Common
{
    public class Configuration
    {
        [JsonIgnore]
        private static string FileName { get; } = "MythicalCuddles/DiscordBot/config/configuration.json";
        [JsonIgnore]
        private static readonly string File = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), FileName);

        public string BotToken { get; set; }
        
        public ulong Developer { get; set; } = 149991092337639424;
        public string StatusText { get; set; } = null;
        public string StatusLink { get; set; } = null;
        public int StatusActivity { get; set; } = -1;
        public UserStatus Status { get; set; } = UserStatus.Online;
        
        public bool UnknownCommandEnabled { get; set; } = true;
        public bool AwardingEXPEnabled { get; set; } = true;
        
        public int LeaderboardAmount { get; set; } = 5;
        public int QuoteLevelRequirement { get; set; } = 10;
        public int PrefixLevelRequirement { get; set; } = 25;
        public int RGBLevelRequirement { get; set; } = 15;
        public int SenpaiChanceRate { get; set; } = 5;
        
        public ulong LogChannelId { get; set; } = 447769497344933900;

        public int Respects { get; set; }
        public int MinLengthForEXP { get; set; }

        public string LeaderboardTrophyUrl { get; set; } = "https://i.imgur.com/Fancl1L.png";
        public uint LeaderboardEmbedColor { get; set; } = 16766287;

        /// NSFW Variables
        public int MaxRuleXGamble { get; set; } = 2353312;

        public static void EnsureExists()
        {
            if (!System.IO.File.Exists(File))
            {
                string path = Path.GetDirectoryName(File);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var config = new Configuration();

                new LogMessage(LogSeverity.Warning, "Configuration", "No configuration file was found. Lets set one up now!").PrintToConsole();

                new LogMessage(LogSeverity.Warning, "Configuration", "Please enter the Bot Token:").PrintToConsole();
                config.BotToken = Cryptography.EncryptString(Console.ReadLine());
                new LogMessage(LogSeverity.Info, "Configuration", "Token saved to " + FileName + "!").PrintToConsole();

                new LogMessage(LogSeverity.Warning, "Configuration", "Console will now be cleared for security reasons. Press the 'enter' key to continue.").PrintToConsole();
                Console.ReadLine();

                Console.Clear();

                config.SaveJson();
                
                new LogMessage(LogSeverity.Info, "Configuration", FileName + " created.").PrintToConsole();
            }

            if (Load().BotToken.IsNullOrEmpty() || Load().BotToken.IsNullOrWhiteSpace())
            {
                var config = new Configuration();

                new LogMessage(LogSeverity.Warning, "Configuration", "The Bot Token was not found.").PrintToConsole();

                new LogMessage(LogSeverity.Info, "Configuration", "Please enter the Bot Token:").PrintToConsole();
                config.BotToken = Cryptography.EncryptString(Console.ReadLine());
                new LogMessage(LogSeverity.Info, "Configuration", "Token saved to " + FileName + "!").PrintToConsole();

                new LogMessage(LogSeverity.Warning, "Configuration", "Console will now be cleared for security reasons. Press the 'enter' key to continue.").PrintToConsole();
                Console.ReadLine();

                Console.Clear();

                config.SaveJson();
                
                
            }

            new LogMessage(LogSeverity.Info, "Configuration", FileName + " loaded.").PrintToConsole();
        }
        
        public void SaveJson()
        {
            System.IO.File.WriteAllText(File, ToJson());
        }
        
        public static Configuration Load()
        {
            return JsonConvert.DeserializeObject<Configuration>(System.IO.File.ReadAllText(File));
        }
        
        public string ToJson()
            => JsonConvert.SerializeObject(this, Formatting.Indented);
        
        public static void UpdateConfiguration(string botToken = null, ulong? developer = null, string statusText = null, string statusLink = null,
            int? statusActivity = null, UserStatus? status = null, bool? unknownCommandEnabled = null, bool? awardingEXPEnabled = null,
            int? leaderboardAmount = null, string leaderboardTrophyUrl = null, uint? leaderboardEmbedColor = null,
            int? quoteLevelRequirement = null, int? prefixLevelRequirement = null, int? senpaiChanceRate = null, int? rgbLevelRequirement = null,
            ulong? logChannelId = null, int? respects = null, int? minLengthForEXP = null, int? maxRuleXGamble = null)
        {
            var config = new Configuration()
            {
                BotToken = botToken ?? Load().BotToken,
                Developer = developer ?? Load().Developer,

                StatusText = statusText ?? Load().StatusText,
                StatusLink = statusLink ?? Load().StatusLink,
                StatusActivity = statusActivity ?? Load().StatusActivity,
                Status = status ?? Load().Status,

                UnknownCommandEnabled = unknownCommandEnabled ?? Load().UnknownCommandEnabled,
                AwardingEXPEnabled = awardingEXPEnabled ?? Load().AwardingEXPEnabled,
                
                LeaderboardAmount = leaderboardAmount ?? Load().LeaderboardAmount,
                LeaderboardTrophyUrl = leaderboardTrophyUrl ?? Load().LeaderboardTrophyUrl,
                LeaderboardEmbedColor = leaderboardEmbedColor ?? Load().LeaderboardEmbedColor,
                
                QuoteLevelRequirement = quoteLevelRequirement ?? Load().QuoteLevelRequirement,
                PrefixLevelRequirement = prefixLevelRequirement ?? Load().PrefixLevelRequirement,
                RGBLevelRequirement = rgbLevelRequirement ?? Load().RGBLevelRequirement,
                SenpaiChanceRate = senpaiChanceRate ?? Load().SenpaiChanceRate,

                LogChannelId = logChannelId ?? Load().LogChannelId,

                Respects = respects ?? Load().Respects,
                MinLengthForEXP = minLengthForEXP ?? Load().MinLengthForEXP,

                MaxRuleXGamble = maxRuleXGamble ?? Load().MaxRuleXGamble
            };
            config.SaveJson();
        }
    }
}
