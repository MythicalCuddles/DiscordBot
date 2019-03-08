using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;

using DiscordBot.Common;
using DiscordBot.Database;
using DiscordBot.Extensions;
using DiscordBot.Objects;
using Google.Protobuf.WellKnownTypes;

namespace DiscordBot.Handlers
{
    public static class GuildHandler
    {
        public static async Task JoinedGuild(SocketGuild socketGuild)
        {
            await InsertGuildToDB(socketGuild);
            
            foreach (SocketGuildChannel c in socketGuild.Channels)
            {
                await ChannelHandler.InsertChannelToDB(c);
            }

            await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync(socketGuild.Name + " has been added to " + DiscordBot.Bot.CurrentUser.Username + "'s guild list. \n" + socketGuild.Owner.Username + " is the owner (" + socketGuild.Owner.Id + ")");
            
            await new LogMessage(LogSeverity.Info, "JoinedGuild", "[" + socketGuild.Name + "] " + "[@" + socketGuild.Owner.Username + 
                                                              "] : Bot Joined the Guild").PrintToConsole();
        }

        public static async Task LeftGuild(SocketGuild socketGuild)
        {
            foreach (SocketGuildChannel c in socketGuild.Channels)
            {
                await ChannelHandler.RemoveChannelFromDB(c);
            }

            await RemoveGuildFromDB(socketGuild);
        }

        public static async Task GuildUpdated(SocketGuild cachedSocketGuild, SocketGuild socketGuild)
        {
            await UpdateGuildInDB(socketGuild);
        }

        public static async Task InsertGuildToDB(SocketGuild g)
        {
            SocketGuildUser gBot = g.GetUser(DiscordBot.Bot.CurrentUser.Id);
            
            List<(string, string)> queryParams = new List<(string id, string value)>()
            {
                ("@guildID", g.Id.ToString()),
                ("@guildName", g.Name),
                ("@guildIcon", g.IconUrl),
                ("@ownedBy", g.Owner.Id.ToString()),
                ("@dateJoined", gBot.JoinedAt.Value.DateTime.ToString("u")), //"yyyy-MM-dd HH:mm:ss"
                ("@guildPrefix", "$") 
            };
				    
            int rowsUpdated = DatabaseActivity.ExecuteNonQueryCommand(
                "INSERT IGNORE INTO " +
                "guilds(guildID,guildName,guildIcon,ownedBy,dateJoined,guildPrefix) " +
                "VALUES (@guildID, @guildName, @guildIcon, @ownedBy, @dateJoined, @guildPrefix);", queryParams);

            if (rowsUpdated == 1)
            {
                await SendMessageToGuild(g);
            }
        }

        private static async Task RemoveGuildFromDB(SocketGuild g)
        {
            DatabaseActivity.ExecuteNonQueryCommand("DELETE FROM guilds WHERE guildID=" + g.Id + ";");
        }

        private static async Task UpdateGuildInDB(SocketGuild g)
        {
            List<(string, string)> queryParams = new List<(string id, string value)>()
            {
                ("@guildID", g.Id.ToString()),
                ("@guildName", g.Name),
                ("@guildIcon", g.IconUrl),
                ("@ownedBy", g.Owner.Id.ToString())
            };

            DatabaseActivity.ExecuteNonQueryCommand(
                "UPDATE guilds SET guildName=@guildName, guildIcon=@guildIcon, ownedBy=@ownedBy WHERE guildID=@guildID",
                queryParams);
        }

        private static async Task SendMessageToGuild(SocketGuild socketGuild)
        {
            Guild g = Guild.Load(socketGuild.Id);
            
            EmbedBuilder eb = new EmbedBuilder()
            .WithTitle("Thank you for adding " + DiscordBot.Bot.CurrentUser.Username + " to your guild!")
            .WithDescription("Congratulations on adding " + DiscordBot.Bot.CurrentUser.Username + " to " + socketGuild.Name + "! Please follow the steps below to configure me!" +
                             "```INI\n" +
                             "-- IMPORTANT! --\n" +
                             "[1] Prefix: You can change the default prefix by typing \"" + g.Prefix + "guildprefix [prefix]\"\n" +
                             "[2] Welcome Message: Type \"" + g.Prefix + "setwelcomemessage\" to view flags and see how to set up the welcome message.\n" +
                             "[3] Welcome Channel: Set the channel the welcome message is posted by typing \"" + g.Prefix + "welcomechannel [channel mention]\"\n" +
                             "[4] Log Channel: We now need a channel where we can post things for your eyes only! Type \"" + g.Prefix + "logchannel [channel mention]\"\n" +
                             "[5] Bot Channel: Finally, we need a channel where the bot can post things for everyone to see! Type \"" + g.Prefix + "botchannel [channel mention]\"\n" +
                             "-- Optional --\n" +
                             "[6] Senpai Command: You can toggle senpai by typing \"" + g.Prefix + "togglesenpai\"\n" +
                             "[7] Quote Command: You can toggle quotes by typing \"" + g.Prefix + "togglequotes\"\n" +
                             "[8] Awarding EXP: You can toggle exp awarding for channels by typing \"" + g.Prefix + "toggleexpawarding [channel mention]\"\n" +
                             "\n[More] If you're interested in setting up NSFW commands and changing other settings, please visit the wiki.\n" +
                             "```")
            .WithFooter("Warning: Server Owner's may only change the configuration for the guild.")
            .WithThumbnailUrl(DiscordBot.Bot.CurrentUser.GetAvatarUrl())
            .WithColor(56, 226, 40);
            await socketGuild.DefaultChannel.SendMessageAsync("", false, eb.Build());
            
            eb = new EmbedBuilder()
                .WithTitle("Seen this message before?")
                .WithDescription("We apologise for the inconvience. Seeing this message again means that your guild configuration files have been reset. " +
                                 "Due to this bot being constantly updated, this might happen more than we, or you, would like." +
                                 "\n\n" +
                                 "It's easy to fix however. Please follow the steps above or head to the wiki and follow the quick setup guide again and your server will be good to go once more!" +
                                 "\n\n" +
                                 "Sorry about that.\n- Melissa (@" + Configuration.Load().Developer.GetUser().Username + "#" + Configuration.Load().Developer.GetUser().Discriminator + ")\n" +
                                 "\t& the rest of the MythicalCuddlesXYZ Team")
                .WithColor(244, 226, 66)
                .WithCurrentTimestamp()
                .WithFooter("This message was posted here as this is your guild's default channel.");
            await socketGuild.DefaultChannel.SendMessageAsync("", false, eb.Build());
        }
    }
}