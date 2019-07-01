using Discord;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Discord.WebSocket;

using Discord.Addons.EmojiTools;

using DiscordBot.Common;
using DiscordBot.Objects;

namespace DiscordBot.Extensions
{
	public static class Extensions
    {
		#region Variables
		public static readonly Emoji
			ArrowLeft = EmojiExtensions.FromText(":arrow_left:"),
			ArrowRight = EmojiExtensions.FromText(":arrow_right:"),
            
            LetterA = EmojiExtensions.FromText(":regional_indicator_a:"),
			LetterB = EmojiExtensions.FromText(":regional_indicator_b:"),
			LetterC = EmojiExtensions.FromText(":regional_indicator_c:"),
			LetterD = EmojiExtensions.FromText(":regional_indicator_d:"),
			LetterE = EmojiExtensions.FromText(":regional_indicator_e:"),
			LetterF = EmojiExtensions.FromText(":regional_indicator_f:"),
			LetterG = EmojiExtensions.FromText(":regional_indicator_g:"),
			LetterH = EmojiExtensions.FromText(":regional_indicator_h:"),
			LetterI = EmojiExtensions.FromText(":regional_indicator_i:"),
			LetterJ = EmojiExtensions.FromText(":regional_indicator_j:"),
			LetterK = EmojiExtensions.FromText(":regional_indicator_k:"),
			LetterL = EmojiExtensions.FromText(":regional_indicator_l:"),
			LetterM = EmojiExtensions.FromText(":regional_indicator_m:"),
			LetterN = EmojiExtensions.FromText(":regional_indicator_n:"),
			LetterO = EmojiExtensions.FromText(":regional_indicator_o:"),
			LetterP = EmojiExtensions.FromText(":regional_indicator_p:"),
			LetterQ = EmojiExtensions.FromText(":regional_indicator_q:"),
			LetterR = EmojiExtensions.FromText(":regional_indicator_r:"),
			LetterS = EmojiExtensions.FromText(":regional_indicator_s:"),
			LetterT = EmojiExtensions.FromText(":regional_indicator_t:"),
			LetterU = EmojiExtensions.FromText(":regional_indicator_u:"),
			LetterV = EmojiExtensions.FromText(":regional_indicator_v:"),
			LetterW = EmojiExtensions.FromText(":regional_indicator_w:"),
			LetterX = EmojiExtensions.FromText(":regional_indicator_x:"),
			LetterY = EmojiExtensions.FromText(":regional_indicator_y:"),
			LetterZ = EmojiExtensions.FromText(":regional_indicator_z:");
        #endregion
        
        #region Dictionaries
        public static List<string> SlotEmotes = new List<string>()
        {
            ":green_apple:",
            ":apple:",
            ":pear:",
            ":tangerine:",
            ":lemon:",
            ":banana:",
            ":watermelon:",
            ":grapes:",
            ":strawberry:",
            ":melon:",
            ":cherries:",
            ":peach:",
            ":pineapple:",
            ":tomato:",
            ":eggplant:",
            ":hot_pepper:",
            ":corn:",
            ":sweet_potato:",
            ":carrot:",
            ":honey_pot:",
            ":bread:",
            ":cheese:",
            ":poultry_leg:",
            ":meat_on_bone:",
            ":fried_shrimp:",
            ":cooking:",
            ":hamburger:",
            ":fries:",
            ":hotdog:",
            ":pizza:",
            ":rice_ball:"
        };
        public static Dictionary<int, string> ReactionUnicodes = new Dictionary<int, string>
        {
            { 0, "0⃣" },
            { 1, "1⃣" },
            { 2, "2⃣" },
            { 3, "3⃣" },
            { 4, "4⃣" },
            { 5, "5⃣" },
            { 6, "6⃣" },
            { 7, "7⃣" },
            { 8, "8⃣" },
            { 9, "9⃣" },
            { 11, "🇦" },
            { 12, "🇧" },
            { 13, "🇨" },
            { 14, "🇩" },
            { 15, "🇪" },
            { 16, "🇫" },
            { 17, "🇬" },
            { 18, "🇭" },
            { 19, "🇮" },
            { 20, "🇯" }
        };
        public static Dictionary<int, string> ReactionStrings = new Dictionary<int, string>
        {
            { 0, ":zero:" },
            { 1, ":one:" },
            { 2, ":two:" },
            { 3, ":three:" },
            { 4, ":four:" },
            { 5, ":five:" },
            { 6, ":six:" },
            { 7, ":seven:" },
            { 8, ":eight:" },
            { 9, ":nine:" },
            { 10, ":regional_indicator_a:" },
            { 11, ":regional_indicator_b:" },
            { 12, ":regional_indicator_c:" },
            { 13, ":regional_indicator_d:" },
            { 14, ":regional_indicator_e:" },
            { 15, ":regional_indicator_f:" },
            { 16, ":regional_indicator_g:" },
            { 17, ":regional_indicator_h:" },
            { 18, ":regional_indicator_i:" },
            { 19, ":regional_indicator_j:" }
        };
        #endregion

        #region General Extensions
        public static string GetTimestamp(this DateTime value)
        {
            return value.ToString("dd/MM/yyyy] [HH:mm:ss");
        }
        public static string GetMonthText(this int monthValue)
        {
            string month;
            switch (monthValue)
            {
                case 1: month = "January"; break;
                case 2: month = "February"; break;
                case 3: month = "March"; break;
                case 4: month = "April"; break;
                case 5: month = "May"; break;
                case 6: month = "June"; break;
                case 7: month = "July"; break;
                case 8: month = "August"; break;
                case 9: month = "September"; break;
                case 10: month = "October"; break;
                case 11: month = "November"; break;
                case 12: month = "December"; break;
                default: month = "Unable to get month"; break;
            }
            return month;
        }
        public static string GetDiceFace(this int value)
        {
            switch (value)
            {
                case 1: return "http://i.imgur.com/IMdC6hG.png";
                case 2: return "http://i.imgur.com/3F0qYkC.png";
                case 3: return "http://i.imgur.com/vje4R3X.png";
                case 4: return "http://i.imgur.com/o5UiOW6.png";
                case 5: return "http://i.imgur.com/iM3HSaU.png";
                case 6: return "http://i.imgur.com/2KTFim8.png";
                default: return "";
            }
        }
        public static List<List<string>> SplitList(this List<string> source, int nSize = 10)
        {
            var list = new List<List<string>>();

            for (int i = 0; i < source.Count; i += nSize)
            {
                list.Add(source.GetRange(i, Math.Min(nSize, source.Count - i)));
            }

            return list;
        }
        public static int RandomNumber(this Random source, int minValue, int maxValue)
        {
            return source.Next(minValue, maxValue + 1);
        }
        #endregion
		
        #region Message Extensions
        public static IMessage DeleteAfter(this IUserMessage msg, int seconds)
        {
            Task.Run(async () =>
            {
                await Task.Delay(seconds * 1000);
                try { await msg.DeleteAsync().ConfigureAwait(false); }
                catch
                {
                    // ignored
                }
            });
            return msg;
        }
        public static IMessage ModifyAfter(this IUserMessage msg, string message, int seconds)
        {
            Task.Run(async () =>
            {
                await Task.Delay(seconds * 1000);
                try { await msg.ModifyAsync(x => x.Content = message).ConfigureAwait(false); }
                catch
                {
                    // ignored
                }
            });
            return msg;
        }
        #endregion

        #region User Extensions
		public static bool IsBotOwner(this IUser user)
        {
            return Configuration.Load().Developer == user.Id;
        }
		public static bool IsTeamMember(this IUser user)
        {
            return User.Load(user.Id).TeamMember;
        }
		public static bool IsGuildOwner(this SocketGuildUser user, IGuild guild)
        {
            return guild.OwnerId == user.Id;
        }
		public static bool IsGuildAdministrator(this SocketGuildUser user)
        {
            return user.GuildPermissions.Administrator;
        }
		public static bool IsGuildModerator(this SocketGuildUser user)
        {
            return (user.GuildPermissions.KickMembers || user.GuildPermissions.BanMembers || user.GuildPermissions.ManageMessages || user.GuildPermissions.ManageChannels);
        }
        public static string UserCreateDate(this IUser user)
        {
            return user.CreatedAt.Day + " " + user.CreatedAt.Month.GetMonthText() + " " + user.CreatedAt.Year;
        }
        public static string GuildJoinDate(this IUser user)
        {
            SocketGuildUser u = user as SocketGuildUser;

            if (u?.JoinedAt != null)
            {
                return u.JoinedAt.Value.Day + " " + u.JoinedAt.Value.Month.GetMonthText() + " " +
                       u.JoinedAt.Value.Year;
            }

            return "Unknown Join Date";
        }
        #endregion

        public static int GetUserPermissionLevel(this SocketGuildUser user)
        {
            if (user.IsBotOwner())
            {
                return 10;
            }

            if (user.IsTeamMember())
            {
                return 9;
            }

            if (user.IsGuildOwner(user.Guild))
            {
                return 7;
            }

            if (user.IsGuildAdministrator())
            {
                return 6;
            }

            if (user.IsGuildModerator())
            {
                return 5;
            }

            if (user.IsBot)
            {
                return 8;
            }

            return 1;
        }

        public static bool HasHigherPermissionLevel(this IUser contextUser, IUser userMentioned)
        {
            return ((contextUser as SocketGuildUser).GetUserPermissionLevel() >=
                   (userMentioned as SocketGuildUser).GetUserPermissionLevel());
        }

        #region SocketUser Gets
        public static SocketUser GetUser(this ulong id)
        {
            if (!(DiscordBot.Bot.GetUser(id) is SocketUser user)) return null;
            return user;
        }
        #endregion

        #region SocketChannel Gets
        public static SocketTextChannel GetTextChannel(this ulong id)
        {
            try
            {
                if (!(DiscordBot.Bot.GetChannel(id) is SocketTextChannel channel))
                {
                    return null;
                }
                return channel;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static SocketVoiceChannel GetVoiceChannel(this ulong id)
        {
            if (!(DiscordBot.Bot.GetChannel(id) is SocketVoiceChannel channel))
            {
                return null;
            }
            return channel;
        }
        #endregion

        #region SocketGuild Gets
        public static SocketGuild GetGuild(this ulong id)
        {
            if (!(DiscordBot.Bot.GetGuild(id) is SocketGuild guild))
            {
                return null;
            }
            return guild;
        }
        public static SocketGuild GetGuild(this ISocketMessageChannel textChannel)
        {
            foreach (SocketGuild g in DiscordBot.Bot.Guilds)
            {
                foreach (SocketTextChannel tc in g.TextChannels)
                {
                    if (tc.Id == textChannel.Id)
                    {
                        return g;
                    }
                }
            }

            return null;
        }
        public static SocketGuild GetGuild(this SocketChannel channel)
        {
            foreach (SocketGuild g in DiscordBot.Bot.Guilds)
            {
                foreach (SocketTextChannel t in g.TextChannels)
                {
                    if (channel.Id == t.Id)
                    {
                        return g;
                    }
                }

                foreach (SocketVoiceChannel v in g.VoiceChannels)
                {
                    if (channel.Id == v.Id)
                    {
                        return g;
                    }
                }
            }

            return null;
        }
        #endregion

        public static ActivityType? ToActivityType(this int no)
        {
            switch (no)
            {
                case 0:
                    return ActivityType.Playing;
                case 1:
                    return ActivityType.Streaming;
                case 2:
                    return ActivityType.Listening;
                case 3:
                    return ActivityType.Watching;
                default:
                    return null;
            }
        }
        
        public static async Task PrintToConsole(this LogMessage logMessage)
        {
            await DiscordBot.Log(logMessage);
        }
    }
}
