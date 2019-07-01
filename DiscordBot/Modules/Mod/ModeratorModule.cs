using System;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using DiscordBot.Common.Preconditions;
using DiscordBot.Common;
using DiscordBot.Extensions;

namespace DiscordBot.Modules.Mod
{
    [Name("Moderator Commands")]
    [RequireContext(ContextType.Guild)]
    [MinPermissions(PermissionLevel.ServerMod)]
    public class ModeratorModule : ModuleBase
    {
        private readonly Version ProgramVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

        [Command("stats"), Summary("Sends information about the bot.")]
        public async Task ShowStatistics()
        {
            int totalUserCount = 0, totalChannelCount = 0, totalTextChannelCount = 0;
            int totalGuildUserCount = 0, totalGuildChannelCount = 0, totalGuildTextChannelCount = 0;

            foreach (SocketGuild g in DiscordBot.Bot.Guilds)
            {
                foreach (SocketGuildChannel c in g.Channels)
                {
                    totalChannelCount++;

                    if (g.Id == Context.Guild.Id)
                    {
                        totalGuildChannelCount++;
                    }
                }
                foreach (SocketTextChannel t in g.TextChannels)
                {
                    totalTextChannelCount++;

                    if (g.Id == Context.Guild.Id)
                    {
                        totalGuildTextChannelCount++;
                    }
                }
                foreach (SocketGuildUser u in g.Users)
                {
                    totalUserCount++;

                    if (g.Id == Context.Guild.Id)
                    {
                        totalGuildUserCount++;
                    }
                }
            }

            EmbedAuthorBuilder eab = new EmbedAuthorBuilder()
                .WithName(DiscordBot.Bot.CurrentUser.Username + " Version " + ProgramVersion.Major + "." + ProgramVersion.Minor + "." + ProgramVersion.Build + "." + ProgramVersion.Revision);
            EmbedFooterBuilder efb = new EmbedFooterBuilder()
                .WithText("MelissaNet Version " + MelissaNet.VersionInfo.Version);
            EmbedBuilder eb = new EmbedBuilder()
                .WithAuthor(eab)

                .AddField("Bot Information", 
                    "**Name:** " + DiscordBot.Bot.CurrentUser.Username + "\n" +
                    "**Discriminator:** #" + DiscordBot.Bot.CurrentUser.Discriminator + "\n" +
                    "**Id:** " + DiscordBot.Bot.CurrentUser.Id)
                .AddField("Developer Information", 
                    "**Name:** " + Configuration.Load().Developer.GetUser().Username + "\n" +
                    "**Discriminator:** #" + Configuration.Load().Developer.GetUser().Discriminator + "\n" +
                    "**Id:** " + Configuration.Load().Developer)
                .AddField("Bot Statistics", 
                    "**Active for:** " + CalculateUptime() + "\n" +
                    "**Latency:** " + DiscordBot.Bot.Latency + "ms" + "\n" +
                    "**Server Time:** " + DateTime.Now.ToString("h:mm:ss tt") + "\n" +
                    "**Guild Count:** " + DiscordBot.Bot.Guilds.Count + "\n" +
                    "**User Count:** " + totalUserCount + "\n" +
                    "**Channel Count:** " + totalChannelCount + " (T:" + totalTextChannelCount + "/V:" + (totalChannelCount - totalTextChannelCount) + ")" + "\n")
                .AddField("Guild Statistics - " + Context.Guild.Name,
                    "**Owner:** " + ((SocketGuildUser) Context.Guild.GetOwnerAsync().GetAwaiter().GetResult()).Username + "\n" +
                    "**Owner Discriminator:** #" + ((SocketGuildUser) Context.Guild.GetOwnerAsync().GetAwaiter().GetResult()).Discriminator + "\n" +
                    "**Owner Id:** " + ((SocketGuildUser) Context.Guild.GetOwnerAsync().GetAwaiter().GetResult()).Id + "\n" +
                    "**Channel Count:** " + totalGuildChannelCount + " (" + totalGuildTextChannelCount + "/" + (totalGuildChannelCount - totalGuildTextChannelCount) + ")" + "\n" +
                    "**User Count:** " + totalGuildUserCount + "\n")

                .WithFooter(efb)
                .WithThumbnailUrl(DiscordBot.Bot.CurrentUser.GetAvatarUrl())
                .WithColor(new Color(255, 116, 140));

            await ReplyAsync("", false, eb.Build());
        }

        internal static DateTime ActiveForDateTime = new DateTime();
        private string CalculateUptime()
        {
            TimeSpan uptime = DateTime.Now - ActiveForDateTime;
            return (uptime.Days.ToString() + " day(s), " + uptime.Hours.ToString() + " hour(s), " + uptime.Minutes.ToString() + " minute(s), " + uptime.Seconds.ToString() + " second(s)");
        }
    }
}
