using System;
using System.Text.RegularExpressions;

using Discord.WebSocket;

using DiscordBot.Common;

namespace DiscordBot.Extensions
{
    public static class StringExtensions
    {
        public static string ModifyStringFlags(this string message, SocketGuildUser e)
        {
            string msg = message;

            msg = Regex.Replace(msg, "{USER.MENTION}", e.Mention, RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{USER.USERNAME}", e.Username, RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{USER.DISCRIMINATOR}", e.Discriminator, RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{USER.AVATARURL}", e.GetAvatarUrl(), RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{USER.ID}", e.Id.ToString(), RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{USER.CREATEDATE}", e.UserCreateDate(), RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{USER.JOINDATE}", e.GuildJoinDate(), RegexOptions.IgnoreCase);

            msg = Regex.Replace(msg, "{GUILD.NAME}", e.Guild.Name, RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{GUILD.OWNER.USERNAME}", e.Guild.Owner.Username, RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{GUILD.OWNER.MENTION}", e.Guild.Owner.Mention, RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{GUILD.OWNER.ID}", e.Guild.Owner.Id.ToString(), RegexOptions.IgnoreCase);
			msg = Regex.Replace(msg, "{GUILD.PREFIX}", GuildConfiguration.Load(e.Guild.Id).Prefix, RegexOptions.IgnoreCase);

            msg = Regex.Replace(msg, "{MELISSA.USERNAME}", Configuration.Load().Developer.GetUser().Username, RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{MELISSA.MENTION}", Configuration.Load().Developer.GetUser().Mention, RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{MELISSA.ID}", Configuration.Load().Developer.GetUser().Id.ToString(), RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{MELISSA.NET.VERSION}", MelissaNet.VersionInfo.Version, RegexOptions.IgnoreCase);

            return msg;
        }
    }
}
