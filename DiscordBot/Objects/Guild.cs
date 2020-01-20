using System.Collections.Generic;
using DiscordBot.Common;
using DiscordBot.Database;
using MelissaNet;
using MySql.Data.MySqlClient;

namespace DiscordBot.Objects
{
    public class Guild
    {
        public string Prefix { get; set; }
        public string WelcomeMessage { get; set; }
        public ulong WelcomeChannelID { get; set; }
        public ulong LogChannelID { get; set; }
        public ulong BotChannelID { get; set; }
        
        public bool SenpaiEnabled { get; set; }
        public bool QuotesEnabled { get; set; }
        
        public bool NSFWCommandsEnabled { get; set; }
        public ulong RuleGambleChannelID { get; set; }
        
        public Guild(string prefix = null, string welcomeMessage = null, ulong? welcomeChannelID = null,
            ulong? logChannelID = null, ulong? botChannelID = null, bool? senpaiEnabled = null, bool? quotesEnabled = null,
            bool? nsfwCommandsEnabled = null, ulong? ruleGambleChannelID = null)
        {
            Prefix = prefix ?? Prefix;
            WelcomeMessage = welcomeMessage ?? WelcomeMessage;
            WelcomeChannelID = welcomeChannelID ?? WelcomeChannelID;
            LogChannelID = logChannelID ?? LogChannelID;
            BotChannelID = botChannelID ?? BotChannelID;
            SenpaiEnabled = senpaiEnabled ?? SenpaiEnabled;
            QuotesEnabled = quotesEnabled ?? QuotesEnabled;
            NSFWCommandsEnabled = nsfwCommandsEnabled ?? NSFWCommandsEnabled;
            RuleGambleChannelID = ruleGambleChannelID ?? RuleGambleChannelID;
        }

        public static Guild Load(ulong guildID)
        {
            Guild guild = new Guild();
                
            (MySqlDataReader dr, MySqlConnection conn) reader = DatabaseActivity.ExecuteReader("SELECT * FROM guilds WHERE guildID=" + guildID + ";");
            
            while (reader.dr.Read())
            {
                guild.Prefix = reader.dr["guildPrefix"].ToString();
                guild.WelcomeMessage = reader.dr["welcomeMessage"].ToString();
                guild.WelcomeChannelID = GetChannelIDOrDefault(reader.dr, "welcomeChannelID");
                guild.LogChannelID = GetChannelIDOrDefault(reader.dr, "logChannelID");
                guild.BotChannelID = GetChannelIDOrDefault(reader.dr, "botChannelID");
                guild.SenpaiEnabled = reader.dr.GetBoolean("senpaiEnabled");
                guild.QuotesEnabled = reader.dr.GetBoolean("quotesEnabled");
                guild.NSFWCommandsEnabled = reader.dr.GetBoolean("nsfwCommandsEnabled");
                guild.RuleGambleChannelID = GetChannelIDOrDefault(reader.dr, "ruleGambleChannelID");
            }
            
            reader.dr.Close();
            reader.conn.Close();
            
            return guild;
        }

        private static ulong GetChannelIDOrDefault(MySqlDataReader reader, string columnName)
        {
            return reader.IsDBNull(reader.GetOrdinal(columnName)) ? Configuration.Load().DefaultUndefinedChannelID : reader.GetUInt64(columnName);
        }

        public static void UpdateGuild(ulong guildID, string prefix = null, string welcomeMessage = null,
            ulong? welcomeChannelID = null,
            ulong? logChannelID = null, ulong? botChannelID = null, bool? senpaiEnabled = null,
            bool? quotesEnabled = null,
            bool? nsfwCommandsEnabled = null, ulong? ruleGambleChannelID = null)
        {
            Guild g = Load(guildID);
            
            List<(string, string)> queryParams = new List<(string, string)>()
            {
                ("@guildID", guildID.ToString()),
                ("@guildPrefix", prefix ?? g.Prefix),
                ("@welcomeMessage", welcomeMessage ?? g.WelcomeMessage),
                ("@welcomeChannelID", (welcomeChannelID ?? g.WelcomeChannelID).ToString()),
                ("@logChannelID", (logChannelID ?? g.LogChannelID).ToString()),
                ("@botChannelID", (botChannelID ?? g.BotChannelID).ToString()),
                ("@senpaiEnabled", (senpaiEnabled ?? g.SenpaiEnabled).ToOneOrZero().ToString()),
                ("@quotesEnabled", (quotesEnabled ?? g.QuotesEnabled).ToOneOrZero().ToString()),
                ("@nsfwCommandsEnabled", (nsfwCommandsEnabled ?? g.NSFWCommandsEnabled).ToOneOrZero().ToString()),
                ("@ruleGambleChannelID", (ruleGambleChannelID ?? g.RuleGambleChannelID).ToString())
            };
            DatabaseActivity.ExecuteNonQueryCommand("UPDATE guilds SET " +
                                                    "guildPrefix=@guildPrefix, " +
                                                    "welcomeMessage=@welcomeMessage, " +
                                                    "welcomeChannelID=@welcomeChannelID, " +
                                                    "logChannelID=@logChannelID, " +
                                                    "botChannelID=@botChannelID, " +
                                                    "senpaiEnabled=@senpaiEnabled, " +
                                                    "quotesEnabled=@quotesEnabled, " +
                                                    "nsfwCommandsEnabled=@nsfwCommandsEnabled, " +
                                                    "ruleGambleChannelID=@ruleGambleChannelID " +
                                                    "WHERE guildID=@guildID;", queryParams);
        }
    }
}