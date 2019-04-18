using System;
using System.Text.RegularExpressions;

using Discord.WebSocket;

using DiscordBot.Common;
using DiscordBot.Objects;

namespace DiscordBot.Extensions
{
    public static class StringExtensions
    {
        public static string ModifyStringFlags(this string message, SocketGuildUser e)
        {
            string msg = message;

            // User Flags
            msg = Regex.Replace(msg, "{USER.MENTION}", e.Mention, RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{USER.USERNAME}", e.Username, RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{USER.DISCRIMINATOR}", e.Discriminator, RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{USER.AVATARURL}", e.GetAvatarUrl(), RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{USER.ID}", e.Id.ToString(), RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{USER.CREATEDATE}", e.UserCreateDate(), RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{USER.JOINDATE}", e.GuildJoinDate(), RegexOptions.IgnoreCase);

            // Guild Flags
            msg = Regex.Replace(msg, "{GUILD.NAME}", e.Guild.Name, RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{GUILD.OWNER.USERNAME}", e.Guild.Owner.Username, RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{GUILD.OWNER.MENTION}", e.Guild.Owner.Mention, RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{GUILD.OWNER.ID}", e.Guild.Owner.Id.ToString(), RegexOptions.IgnoreCase);
			msg = Regex.Replace(msg, "{GUILD.PREFIX}", Guild.Load(e.Guild.Id).Prefix, RegexOptions.IgnoreCase);

            msg = Regex.Replace(msg, "{GUILD.OWNER.CUSTOMS.NAME}", (e.Guild.Owner.GetName() ?? "[NAME NOT SET]"), RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{GUILD.OWNER.CUSTOMS.MINECRAFTUSERNAME}", (e.Guild.Owner.GetMinecraftUsername() ?? "[MINECRAFT USERNAME NOT SET]"), RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{GUILD.OWNER.CUSTOMS.INSTAGRAMUSERNAME}", (e.Guild.Owner.GetInstagramUsername() ?? "[INSTAGRAM USERNAME NOT SET]"), RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{GUILD.OWNER.CUSTOMS.GITHUBUSERNAME}", (e.Guild.Owner.GetGitHubUsername() ?? "[GITHUB USERNAME NOT SET]"), RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{GUILD.OWNER.CUSTOMS.WEBSITEURL}", (e.Guild.Owner.GetWebsiteUrl() ?? "[WEBSITE URL NOT SET]"), RegexOptions.IgnoreCase);

            // Developer Flags
            msg = Regex.Replace(msg, "{DEVELOPER.USERNAME}", Configuration.Load().Developer.GetUser().Username, RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{DEVELOPER.MENTION}", Configuration.Load().Developer.GetUser().Mention, RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{DEVELOPER.ID}", Configuration.Load().Developer.GetUser().Id.ToString(), RegexOptions.IgnoreCase);
            
            msg = Regex.Replace(msg, "{DEVELOPER.CUSTOMS.NAME}", (Configuration.Load().Developer.GetUser().GetName() ?? "[NAME UNSET]"), RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{DEVELOPER.CUSTOMS.MINECRAFTUSERNAME}", (Configuration.Load().Developer.GetUser().GetMinecraftUsername() ?? "[MINECRAFT USERNAME NOT SET]"), RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{DEVELOPER.CUSTOMS.INSTAGRAMUSERNAME}", (Configuration.Load().Developer.GetUser().GetInstagramUsername() ?? "[INSTAGRAM USERNAME NOT SET]"), RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{DEVELOPER.CUSTOMS.GITHUBUSERNAME}", (Configuration.Load().Developer.GetUser().GetGitHubUsername() ?? "[GITHUB USERNAME NOT SET]"), RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{DEVELOPER.CUSTOMS.WEBSITEURL}", (Configuration.Load().Developer.GetUser().GetWebsiteUrl() ?? "[WEBSITE URL NOT SET]"), RegexOptions.IgnoreCase);

            return msg;
        }
    }
}
