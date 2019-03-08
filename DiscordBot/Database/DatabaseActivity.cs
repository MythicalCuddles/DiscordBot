using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using DiscordBot.Common;
using DiscordBot.Extensions;
using MySql.Data.MySqlClient;

namespace DiscordBot.Database
{
    public static class DatabaseActivity
    {
        private static bool databaseExists;
        
        public static void CheckForDatabase()
        {
            #region Database Configuration Checks
            
            // Running through the configuration file - checking for database information.
            if (String.IsNullOrEmpty(Configuration.Load().DatabaseHost))
            {
                new LogMessage(LogSeverity.Info, "Database Configuration", "Enter Database Hostname (default: 127.0.0.1):").PrintToConsole();
                string host = Console.ReadLine();

                if (string.IsNullOrEmpty(host))
                {
                    host = "127.0.0.1";
                    new LogMessage(LogSeverity.Warning, "Database Configuration", "No value was entered for the database host. Using default (127.0.0.1)").PrintToConsole();
                }
                
                Configuration.UpdateConfiguration(databaseHost:host);
            }
            
            if (Configuration.Load().DatabasePort == 0)
            {
                new LogMessage(LogSeverity.Info, "Database Configuration", "Enter Database Port (default: 3306):").PrintToConsole();
                string port = Console.ReadLine();
                int portN = 0;

                if (string.IsNullOrEmpty(port))
                {
                    portN = 3306;
                    new LogMessage(LogSeverity.Warning, "Database Configuration", "No database port entered. Using default (3306)").PrintToConsole();
                } 
                else if (!(Int32.TryParse(port, out portN)))
                {
                    new LogMessage(LogSeverity.Warning, "Database Configuration", "Unable to parse database port. Using default (3306)").PrintToConsole();
                }
                
                Configuration.UpdateConfiguration(databasePort:portN);
            }
            
            if (String.IsNullOrEmpty(Configuration.Load().DatabaseUser))
            {
                new LogMessage(LogSeverity.Info, "Database Configuration", "Enter Database Username (default: root):").PrintToConsole();
                string user = Console.ReadLine();

                if (string.IsNullOrEmpty(user))
                {
                    user = "root";
                    new LogMessage(LogSeverity.Warning, "Database Configuration", "No value was entered for the database user. Using default (root)").PrintToConsole();
                }
                
                Configuration.UpdateConfiguration(databaseUser:user);
            }
            
            if ((String.IsNullOrEmpty(Configuration.Load().DatabaseUser) ||
                 String.IsNullOrEmpty(Configuration.Load().DatabaseName)) &&
                String.IsNullOrEmpty(Configuration.Load().DatabasePassword))
            {
                new LogMessage(LogSeverity.Info, "Database Configuration", "Enter Database Password (default: ):").PrintToConsole();
                string pass = Console.ReadLine();

                if (string.IsNullOrEmpty(pass))
                {
                    pass = "";
                    new LogMessage(LogSeverity.Warning, "Database Configuration", "No value was entered for the database password. Using default ()").PrintToConsole();
                }
                
                Configuration.UpdateConfiguration(databasePassword:pass);
            }
            
            if (String.IsNullOrEmpty(Configuration.Load().DatabaseName))
            {
                new LogMessage(LogSeverity.Info, "Database Configuration", "Enter Database Name (default: discordbot):").PrintToConsole();
                string name = Console.ReadLine();

                if (string.IsNullOrEmpty(name))
                {
                    name = "discordbot";
                    new LogMessage(LogSeverity.Warning, "Database Configuration", "No value was entered for the database name. Using default (discordbot)").PrintToConsole();
                }
                
                Configuration.UpdateConfiguration(databaseName:name);
            }

            #endregion
            
            new LogMessage(LogSeverity.Info, "Database Configuration", "Database information loaded from Configuration.").PrintToConsole();

            CreateDatabaseIfNotExists();
            CreateTablesIfNotExists();
        }

        public static MySqlConnection GetDatabaseConnection()
        {
            string connectionString;
            if (databaseExists)
            {
                connectionString = String.Format("server={0};port={1};user id={2}; password={3}; database={4}; CharSet=utf8mb4", Configuration.Load().DatabaseHost, Configuration.Load().DatabasePort.ToString(), Configuration.Load().DatabaseUser, Configuration.Load().DatabasePassword, Configuration.Load().DatabaseName);
            }
            else
            {
                connectionString = String.Format("server={0};port={1};user id={2}; password={3}", Configuration.Load().DatabaseHost, Configuration.Load().DatabasePort.ToString(), Configuration.Load().DatabaseUser, Configuration.Load().DatabasePassword);
            }
            
            MySqlConnection connection = new MySqlConnection(connectionString);
            
            try
            {
                //connection.Open();
                return connection;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

            return connection;
        }

        public static int ExecuteNonQueryCommand(string query, List<(string name, string value)> queryParams = null)
        {
            using (MySqlConnection conn = GetDatabaseConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Prepare();
            
                if (queryParams != null)
                {
                    foreach(var s in queryParams)
                    {
                        MySqlParameter param = new MySqlParameter();
                        param.ParameterName = s.name;
                        param.Value = s.value;

                        cmd.Parameters.Add(param);
                    }
                }

                try
                {
                    int rows = cmd.ExecuteNonQuery();
                    conn.CloseAsync();

                    new LogMessage(LogSeverity.Info, "Database Command",
                        "Command: " + cmd.CommandText + " | Rows affected: " + rows).PrintToConsole();

                    return rows;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
        }

        public static (MySqlDataReader,MySqlConnection) ExecuteReader(string query)
        {
            MySqlConnection conn = GetDatabaseConnection();
            MySqlCommand cmd = new MySqlCommand(query, conn);
            
            conn.Open();
            
            try
            {
                MySqlDataReader dr = cmd.ExecuteReader();

                //new LogMessage(LogSeverity.Info, "Database Command", "Command: " + cmd.CommandText).PrintToConsole();

                return (dr, conn);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            
        }

        private static void CreateDatabaseIfNotExists()
        {
            ExecuteNonQueryCommand(string.Format("CREATE DATABASE IF NOT EXISTS {0};", Configuration.Load().DatabaseName));
            ExecuteNonQueryCommand(string.Format(
                "ALTER DATABASE {0} CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci;",
                Configuration.Load().DatabaseName)); // set the charset of the database to allow for 
            
            databaseExists = true;
        }

        private static void CreateTablesIfNotExists()
        {
            ExecuteNonQueryCommand("CREATE TABLE IF NOT EXISTS `awards` (" +
                                   "`awardID` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT, " +
                                   "`userID` bigint(20) UNSIGNED NOT NULL, " +
                                   "`awardText` text COLLATE utf8mb4_unicode_ci NOT NULL, " +
                                   "`dateAwarded` date NOT NULL," +
                                   "PRIMARY KEY (awardId)" +
                                   ") ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci;");
            
            
            ExecuteNonQueryCommand("CREATE TABLE IF NOT EXISTS `users` (" +
                                   "`id` bigint(20) UNSIGNED NOT NULL," +
                                   "`username` text COLLATE utf8mb4_unicode_ci NOT NULL," +
                                   "`avatarUrl` text COLLATE utf8mb4_unicode_ci," +
                                   "`level` int(10) UNSIGNED NOT NULL DEFAULT '0'," +
                                   "`exp` int(10) UNSIGNED NOT NULL DEFAULT '0'," +
                                   "`name` text COLLATE utf8mb4_unicode_ci," +
                                   "`gender` text COLLATE utf8mb4_unicode_ci," +
                                   "`pronouns` text COLLATE utf8mb4_unicode_ci," +
                                   "`about` text COLLATE utf8mb4_unicode_ci," +
                                   "`customPrefix` text COLLATE utf8mb4_unicode_ci," +
                                   "`aboutR` tinyint(3) UNSIGNED NOT NULL DEFAULT '140'," +
                                   "`aboutG` tinyint(3) UNSIGNED NOT NULL DEFAULT '90'," +
                                   "`aboutB` tinyint(3) UNSIGNED NOT NULL DEFAULT '210'," +
                                   "`teamMember` char(1) COLLATE utf8mb4_unicode_ci NOT NULL DEFAULT 'N'," +
                                   "`authorIconURL` text COLLATE utf8mb4_unicode_ci," +
                                   "`footerIconURL` text COLLATE utf8mb4_unicode_ci," +
                                   "`footerText` text COLLATE utf8mb4_unicode_ci," +
                                   "`pokemonGoFriendCode` text COLLATE utf8mb4_unicode_ci," +
                                   "`minecraftUsername` text COLLATE utf8mb4_unicode_ci," +
                                   "`snapchatUsername` text COLLATE utf8mb4_unicode_ci," +
                                   "`instagramUsername` text COLLATE utf8mb4_unicode_ci," +
                                   "`githubUsername` text COLLATE utf8mb4_unicode_ci," +
                                   "`websiteName` text COLLATE utf8mb4_unicode_ci," +
                                   "`websiteURL` text COLLATE utf8mb4_unicode_ci," +
                                   "`isBeingIgnored` char(1) NOT NULL DEFAULT 'N'," +
                                   "PRIMARY KEY (`id`)" +
                                   ") ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

            ExecuteNonQueryCommand("CREATE TABLE IF NOT EXISTS `guilds` (" +
                                   "`guildID` bigint(20) UNSIGNED NOT NULL," +
                                   "`guildName` text COLLATE utf8mb4_unicode_ci NOT NULL," +
                                   "`guildIcon` text COLLATE utf8mb4_unicode_ci NOT NULL," +
                                   "`ownedBy` bigint(20) UNSIGNED NOT NULL," +
                                   "`dateJoined` date NOT NULL," +
                                   "`guildPrefix` text COLLATE utf8mb4_unicode_ci NOT NULL," +
                                   "`welcomeChannelID` bigint(20) UNSIGNED DEFAULT NULL," +
                                   "`welcomeMessage` text COLLATE utf8mb4_unicode_ci," +
                                   "`logChannelID` bigint(20) UNSIGNED DEFAULT NULL," +
                                   "`botChannelID` bigint(20) UNSIGNED DEFAULT NULL," +
                                   "`senpaiEnabled` tinyint(1) NOT NULL DEFAULT '1'," +
                                   "`quotesEnabled` tinyint(1) NOT NULL DEFAULT '1'," +
                                   "`nsfwCommandsEnabled` tinyint(1) NOT NULL DEFAULT '0'," +
                                   "`ruleGambleChannelID` bigint(20) UNSIGNED DEFAULT NULL," +
                                   "PRIMARY KEY (`guildID`)" +
                                   ") ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci;");
            
            ExecuteNonQueryCommand("CREATE TABLE IF NOT EXISTS `channels` (" +
                                   "`channelID` bigint(20) NOT NULL," +
                                   "`inGuildID` bigint(20) NOT NULL," +
                                   "`channelName` text COLLATE utf8mb4_unicode_ci NOT NULL," +
                                   "`channelType` enum('SocketTextChannel','SocketVoiceChannel','SocketCategoryChannel') COLLATE utf8mb4_unicode_ci NOT NULL," +
                                   "`awardingEXP` tinyint(1) NOT NULL DEFAULT '1'," +
                                   "PRIMARY KEY (`channelID`)" +
                                   ") ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
            
            ExecuteNonQueryCommand("CREATE TABLE IF NOT EXISTS `bans` (" +
                                   "`banID` int(11) NOT NULL AUTO_INCREMENT," +
                                   "`issuedTo` bigint(20) UNSIGNED NOT NULL," +
                                   "`issuedBy` bigint(20) UNSIGNED NOT NULL," +
                                   "`inGuild` bigint(20) UNSIGNED NOT NULL," +
                                   "`banDescription` text COLLATE utf8mb4_unicode_ci NOT NULL," +
                                   "`dateIssued` datetime NOT NULL," +
                                   "PRIMARY KEY (`banID`)" +
                                   ") ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
        }
    }
}