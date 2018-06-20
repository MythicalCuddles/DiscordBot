using System;

using Discord;

using DiscordBot.Common;

namespace DiscordBot.Extensions
{
    public static class UserExtensions
    {
        public static int GetCoins(this IUser user)
        {
            return User.Load(user.Id).Coins;
        }
        public static int GetMythicalTokens(this IUser user)
        {
            return User.Load(user.Id).MythicalTokens;
        }

        public static string GetName(this IUser user)
        {
            return User.Load(user.Id).Name;
        }
        
        public static string GetGender(this IUser user)
        {
            return User.Load(user.Id).Gender;
        }
        
        public static string GetPronouns(this IUser user)
        {
            return User.Load(user.Id).Pronouns;
        }
        
        public static string GetAbout(this IUser user)
        {
            return User.Load(user.Id).About;
        }

        public static string GetCustomPrefix(this IUser user)
        {
            return User.Load(user.Id).CustomPrefix;
        }

        public static string GetMinecraftUser(this IUser user)
        {
            return User.Load(user.Id).MinecraftUsername;
        }
        
        public static string GetSnapchatUsername(this IUser user)
        {
            return User.Load(user.Id).Snapchat;
        }

        public static string GetFooterText(this IUser user)
        {
            return User.Load(user.Id).FooterText;
        }

        public static string GetWebsiteName(this IUser user)
        {
            return User.Load(user.Id).WebsiteName;
        }

        public static string GetWebsiteUrl(this IUser user)
        {
            return User.Load(user.Id).WebsiteUrl;
        }


        public static string GetEmbedAuthorBuilderIconUrl(this IUser user)
        {
            return User.Load(user.Id).EmbedAuthorBuilderIconUrl;
        }

        public static string GetEmbedFooterBuilderIconUrl(this IUser user)
        {
            return User.Load(user.Id).EmbedFooterBuilderIconUrl;
        }
    }
}
