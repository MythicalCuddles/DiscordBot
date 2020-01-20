using System;
using System.Linq;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using DiscordBot.Common.Preconditions;
using DiscordBot.Common;
using DiscordBot.Exceptions;
using DiscordBot.Extensions;
using DiscordBot.Handlers;
using DiscordBot.Logging;

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
            // Bot Counts
            int bGuildCount = DiscordBot.Bot.Guilds.Count();
            int tTextChannelCount = 0, tVoiceChannelCount = 0, tCategoryChannelCount = 0;
            int tChannelCount = 0, tUserCount = 0;
            
            // Guild Counts
            int gTextChannelCount = DiscordBot.Bot.GetGuild(Context.Guild.Id).TextChannels.Count();
            int gVoiceChannelCount = DiscordBot.Bot.GetGuild(Context.Guild.Id).VoiceChannels.Count();
            int gCategoryChannelCount = DiscordBot.Bot.GetGuild(Context.Guild.Id).CategoryChannels.Count();
            int gTotalCount = 0;
            int gUserCount = DiscordBot.Bot.GetGuild(Context.Guild.Id).MemberCount;

            foreach(SocketGuild g in DiscordBot.Bot.Guilds)
            {
                tTextChannelCount += g.TextChannels.Count();
                tVoiceChannelCount += g.VoiceChannels.Count();
                tCategoryChannelCount += g.CategoryChannels.Count();
                tUserCount += g.Users.Count();
            }

            gTotalCount = gTextChannelCount + gVoiceChannelCount + gCategoryChannelCount;
            tChannelCount = tTextChannelCount + tVoiceChannelCount + tCategoryChannelCount;

            EmbedAuthorBuilder eab = new EmbedAuthorBuilder()
                .WithName(DiscordBot.Bot.CurrentUser.Username + " - Statistics & Information");
            EmbedBuilder eb = new EmbedBuilder();
                eb.WithAuthor(eab);

                eb.AddField("Bot Name",
                    DiscordBot.Bot.CurrentUser.Username + "#" + DiscordBot.Bot.CurrentUser.Discriminator, true);
                try
                {
                    eb.AddField("Developer Name",
                        Configuration.Load().Developer.GetUser().Username + "#" +
                        Configuration.Load().Developer.GetUser().Discriminator, true);
                }
                catch (UserNotFoundException exception)
                {
                    eb.AddField("Developer Name", "Melissa", true);
                    await new LogMessage(LogSeverity.Warning, "ModeratorModule", exception.Message + " - Using \"Melissa\" instead.").PrintToConsole();
                }
                eb.AddField("Developer ID", Configuration.Load().Developer, true);

                eb.AddField("DiscordBot Version", "v" + ProgramVersion, true);
                eb.AddField("MelissaNET Version", "v" + MelissaNet.VersionInfo.Version, true);
                eb.AddField(".NET Version", typeof(string).Assembly.ImageRuntimeVersion, true);

                eb.AddField("Bot Statistics",
                    "**Active for:** " + CalculateUptime() + "\n" +
                    "**Latency:** " + DiscordBot.Bot.Latency + "ms" + "\n" +
                    "**Server Time:** " + DateTime.Now.ToString("h:mm:ss tt") + "\n");

                eb.AddField("Guild Count", bGuildCount, true);
                eb.AddField("Channel Count",
                    tChannelCount + " (T:" + tTextChannelCount + " | V: " + tVoiceChannelCount + " | C: " +
                    tCategoryChannelCount + ")", true);
                eb.AddField("User Count", tUserCount, true);

                eb.AddField("Guild Statistics",
                    "**Owner:** " +
                    ((SocketGuildUser) Context.Guild.GetOwnerAsync().GetAwaiter().GetResult()).Username + "#" +
                    ((SocketGuildUser) Context.Guild.GetOwnerAsync().GetAwaiter().GetResult()).Discriminator + "\n" +
                    "**Owner Id:** " + ((SocketGuildUser) Context.Guild.GetOwnerAsync().GetAwaiter().GetResult()).Id +
                    "\n" +
                    "**Channel Count:** " + gTotalCount + " (T: " + gTextChannelCount + " | V: " + gVoiceChannelCount +
                    " | C: " + gCategoryChannelCount + ")\n" +
                    "**User Count:** " + gUserCount + "\n");

                eb.AddField("Website", "[MythicalCuddles](https://mythicalcuddles.xyz/)", true);
                eb.AddField("GitHub", "[Source Code](https://github.com/MythicalCuddles/DiscordBot)", true);
                eb.AddField("Download", "[Latest Version](https://github.com/MythicalCuddles/DiscordBot/releases)",
                    true);

                eb.WithCurrentTimestamp();
                eb.WithThumbnailUrl(DiscordBot.Bot.CurrentUser.GetAvatarUrl());
                eb.WithColor(new Color(255, 116, 140));

            AdminLog.Log(Context.User.Id, Context.Message.Content, Context.Guild.Id);
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
