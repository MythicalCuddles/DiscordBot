using System;
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

        public static MySqlConnection OpenDatabaseConnection()
        {
            string connectionString;
            if (databaseExists)
            {
                connectionString = String.Format("server={0};port={1};user id={2}; password={3}; database={4}", Configuration.Load().DatabaseHost, Configuration.Load().DatabasePort.ToString(), Configuration.Load().DatabaseUser, Configuration.Load().DatabasePassword, Configuration.Load().DatabaseName);
            }
            else
            {
                connectionString = String.Format("server={0};port={1};user id={2}; password={3}", Configuration.Load().DatabaseHost, Configuration.Load().DatabasePort.ToString(), Configuration.Load().DatabaseUser, Configuration.Load().DatabasePassword);
            }
            
            MySqlConnection connection = new MySqlConnection(connectionString);
            
            try
            {
                connection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

            return connection;
        }

        public static void ExecuteNonQueryCommand(string command)
        {
            MySqlCommand cmd = new MySqlCommand
            {
                Connection = OpenDatabaseConnection(),
                CommandText = command
            };

            try
            {
                int rows = cmd.ExecuteNonQuery();
                new LogMessage(LogSeverity.Info, "Database Command",
                    "Command: " + cmd.CommandText + " | Rows affected: " + rows).PrintToConsole();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public static MySqlDataReader ExecuteReader(string command)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = OpenDatabaseConnection();
            cmd.CommandText = command;
            MySqlDataReader dr = cmd.ExecuteReader();
            return dr;
        }

        private static void CreateDatabaseIfNotExists()
        {
            ExecuteNonQueryCommand(String.Format("CREATE DATABASE IF NOT EXISTS {0};", Configuration.Load().DatabaseName));
            databaseExists = true;
        }

        private static void CreateTablesIfNotExists()
        {
            ExecuteNonQueryCommand("CREATE TABLE IF NOT EXISTS `awards` (" +
                                   "`awardId` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT, " +
                                   "`userId` bigint(20) UNSIGNED NOT NULL, " +
                                   "`awardText` text CHARACTER SET utf8 COLLATE utf8_bin NOT NULL, " +
                                   "`dateAwarded` date NOT NULL," +
                                   "PRIMARY KEY (`awardId`))");
            
            
            ExecuteNonQueryCommand("CREATE TABLE IF NOT EXISTS `users` (" +
                                   "`id` bigint(20) UNSIGNED NOT NULL," +
                                   "`username` text CHARACTER SET utf8 NOT NULL," +
                                   "`avatarUrl` text CHARACTER SET utf8," +
                                   "`level` int(10) UNSIGNED NOT NULL DEFAULT '0'," +
                                   "`exp` int(10) UNSIGNED NOT NULL DEFAULT '0'," +
                                   "`name` text CHARACTER SET utf8," +
                                   "`gender` text CHARACTER SET utf8," +
                                   "`pronouns` text CHARACTER SET utf8," +
                                   "`about` text CHARACTER SET utf8," +
                                   "`customPrefix` char(1) CHARACTER SET utf8 DEFAULT NULL," +
                                   "`aboutR` tinyint(3) UNSIGNED NOT NULL DEFAULT '140'," +
                                   "`aboutG` tinyint(3) UNSIGNED NOT NULL DEFAULT '90'," +
                                   "`aboutB` tinyint(3) UNSIGNED NOT NULL DEFAULT '210'," +
                                   "`teamMember` char(1) CHARACTER SET utf8 NOT NULL DEFAULT 'N'," +
                                   "`authorIconURL` text CHARACTER SET utf8," +
                                   "`footerIconURL` text CHARACTER SET utf8," +
                                   "`footerText` text CHARACTER SET utf8," +
                                   "`pokemonGoFriendCode` text CHARACTER SET utf8," +
                                   "`minecraftUsername` text CHARACTER SET utf8," +
                                   "`snapchatUsername` text CHARACTER SET utf8," +
                                   "`instagramUsername` text CHARACTER SET utf8," +
                                   "`githubUsername` text CHARACTER SET utf8," +
                                   "`websiteName` text CHARACTER SET utf8," +
                                   "`websiteURL` text CHARACTER SET utf8," +
                                   "`isBeingIgnored` char(1) NOT NULL DEFAULT 'N'" +
                                   ")");
        }
    }
}