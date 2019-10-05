using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using DiscordBot.Common;
using DiscordBot.Extensions;
using MySql.Data.MySqlClient;

namespace DiscordBot.Database
{
    public static class DatabaseActivity
    {
        private static bool _databaseExists, _connectionTested;
        
        internal static void EnsureExists()
        {
            if(!_connectionTested) {
                var config = Configuration.Load();
            
                TestDatabaseSettings(config.DatabaseHost, config.DatabaseUser, config.DatabasePassword,
                    config.DatabasePort).GetAwaiter().GetResult();
            }
            CreateDatabaseIfNotExists();
            CreateTablesIfNotExists();
            
            Methods.PrintConsoleSplitLine();
        }
        
        internal static async Task<(bool connectionValid, bool databaseExists)> TestDatabaseSettings(string hostname = null, string username = null, string password = null, int? port = null, string database = null)
        {
            hostname = hostname ?? Configuration.Load().DatabaseHost;
            username = username ?? Configuration.Load().DatabaseUser;
            password = password ?? Configuration.Load().DatabasePassword;
            port = port ?? Configuration.Load().DatabasePort;
            database = database ?? Configuration.Load().DatabaseName;
            
            string connectionString =
                $"server={hostname};port={port.ToString()};user id={username}; password={password}";
            
            MySqlConnection connection;
            
            await new LogMessage(LogSeverity.Info, "Database Validation", "Verifying database settings.").PrintToConsole();
            
            // test to see if connection is valid.
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
                connection.Close();
                _connectionTested = true;
                await new LogMessage(LogSeverity.Info, "Database Validation", "Connection to database established.").PrintToConsole();
            }
            catch (Exception e)
            {
                await new LogMessage(LogSeverity.Error, "Database Validation", e.Message).PrintToConsole();
                return (false, false);
            }
            
            // test to see if database exists - required to be after connection is valid.
            connectionString = $"server={hostname};port={port.ToString()};user id={username}; password={password}; database={database}; CharSet=utf8mb4";
            
            await new LogMessage(LogSeverity.Info, "Database Validation", "Checking for database.").PrintToConsole();
            
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
                connection.Close();
                
                await new LogMessage(LogSeverity.Info, "Database Validation", "Database exists.").PrintToConsole();

                return (true, true);
            }
            catch (Exception)
            {
                await new LogMessage(LogSeverity.Info, "Database Validation", "Unable to find database.").PrintToConsole();
                return (true, false);
            }
        }

        private static MySqlConnection GetDatabaseConnection()
        {
            string connectionString = _databaseExists ? 
                $"server={Configuration.Load().DatabaseHost};" +
                $"port={Configuration.Load().DatabasePort.ToString()};" +
                $"user id={Configuration.Load().DatabaseUser};" +
                $"password={Configuration.Load().DatabasePassword};" +
                $"database={Configuration.Load().DatabaseName}; CharSet=utf8mb4"
                : $"server={Configuration.Load().DatabaseHost};" +
                  $"port={Configuration.Load().DatabasePort.ToString()};" +
                  $"user id={Configuration.Load().DatabaseUser};" +
                  $"password={Configuration.Load().DatabasePassword}";
            
            MySqlConnection connection = new MySqlConnection(connectionString);
            
            try
            {
                return connection;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        internal static int ExecuteNonQueryCommand(string query, List<(string name, string value)> queryParams = null)
        {
            using (var conn = GetDatabaseConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand(query, conn);
                cmd.Prepare();
            
                if (queryParams != null)
                {
                    foreach(var (name, value) in queryParams)
                    {
                        var param = new MySqlParameter
                        {
                            ParameterName = name, 
                            Value = value
                        };

                        cmd.Parameters.Add(param);
                    }
                }

                try
                {
                    int rows = cmd.ExecuteNonQuery();
                    conn.CloseAsync();

                    new LogMessage(LogSeverity.Debug, "Database Command","Command: " + cmd.CommandText + " | Rows affected: " + rows).PrintToConsole().GetAwaiter();

                    return rows;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
        }

        internal static (MySqlDataReader,MySqlConnection) ExecuteReader(string query)
        {
            MySqlConnection conn = GetDatabaseConnection();
            MySqlCommand cmd = new MySqlCommand(query, conn);
            
            conn.Open();
            
            try
            {
                return (cmd.ExecuteReader(), conn);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            
        }

        private static void CreateDatabaseIfNotExists()
        {
            ExecuteNonQueryCommand(
                $"CREATE DATABASE IF NOT EXISTS {Configuration.Load().DatabaseName} CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci;");
            
            _databaseExists = true;
        }

        private static void CreateTablesIfNotExists()
        {
            ExecuteNonQueryCommand("CREATE TABLE IF NOT EXISTS `awards` (" +
                                   "`awardID` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT, " +
                                   "`userID` bigint(20) UNSIGNED NOT NULL, " +
                                   "`awardText` text COLLATE utf8mb4_unicode_ci NOT NULL, " +
                                   "`awardType` text COLLATE utf8mb4_unicode_ci NOT NULL, " +
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