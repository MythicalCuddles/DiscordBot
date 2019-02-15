using System;
using Discord;
using DiscordBot.Common;
using DiscordBot.Extensions;

namespace DiscordBot.Database
{
    public static class DatabaseActivity
    {
        public static void CheckForDatabase()
        {
            #region Database Configuration Checks
            
            // Running through the configuration file - checking for database information.
            if (String.IsNullOrEmpty(Configuration.Load().DatabaseHost))
            {
                new LogMessage(LogSeverity.Warning, "Database Configuration", "Enter Database Hostname (default: 127.0.0.1):").PrintToConsole();
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
                new LogMessage(LogSeverity.Warning, "Database Configuration", "Enter Database Port (default: 3306):").PrintToConsole();
                string port = Console.ReadLine();
                int portN = 0;

                if (Int32.TryParse(port, out portN))
                {
                    portN = 3306;
                    new LogMessage(LogSeverity.Warning, "Database Configuration", "Unable to parse database port. Using default (3306)").PrintToConsole();
                }
                
                Configuration.UpdateConfiguration(databasePort:portN);
            }
            
            if (String.IsNullOrEmpty(Configuration.Load().DatabaseUser))
            {
                new LogMessage(LogSeverity.Warning, "Database Configuration", "Enter Database Username (default: root):").PrintToConsole();
                string user = Console.ReadLine();

                if (string.IsNullOrEmpty(user))
                {
                    user = "root";
                    new LogMessage(LogSeverity.Warning, "Database Configuration", "No value was entered for the database user. Using default (root)").PrintToConsole();
                }
                
                Configuration.UpdateConfiguration(databaseUser:user);
            }
            
            if (String.IsNullOrEmpty(Configuration.Load().DatabasePassword))
            {
                new LogMessage(LogSeverity.Warning, "Database Configuration", "Enter Database Password (default: ):").PrintToConsole();
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
                new LogMessage(LogSeverity.Warning, "Database Configuration", "Enter Database Name (default: discordbot):").PrintToConsole();
                string name = Console.ReadLine();

                if (string.IsNullOrEmpty(name))
                {
                    name = "discordbot";
                    new LogMessage(LogSeverity.Warning, "Database Configuration", "No value was entered for the database name. Using default (discordbot)").PrintToConsole();
                }
                
                Configuration.UpdateConfiguration(databaseName:name);
            }

            #endregion
            
            
        } 
    }
}