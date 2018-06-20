using System;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using DiscordBot.Common;
using DiscordBot.Common.Preconditions;
using DiscordBot.Extensions;
using DiscordBot.Logging;

namespace DiscordBot.Modules.Public
{
    [MinPermissions(PermissionLevel.User)]
    [RequireContext(ContextType.Guild)]
    [Name("Public Commands")]
    public class PublicModule : ModuleBase
    {
        [Command("hug"), Summary("Give your friend a hug!")]
        public async Task HugUser(IUser user)
        {
            await ReplyAsync(Context.User.Mention + " has given a massive hug to " + user.Mention);
        }

        [Command("about"), Summary("Returns the about description about the user specified.")]
        public async Task UserAbout(IUser user = null)
        {
            var userSpecified = user as SocketGuildUser ?? Context.User as SocketGuildUser;
            var typing = Context.Channel.EnterTypingState();

            if (userSpecified == null)
            {
                await ReplyAsync("User not found, please try again.");
                typing.Dispose();
                return;
            }

            EmbedAuthorBuilder eab = new EmbedAuthorBuilder();
            if(userSpecified.Nickname != null) eab.WithName("About " + userSpecified.Nickname);
            else eab.WithName("About " + userSpecified.Username);

            EmbedFooterBuilder efb = new EmbedFooterBuilder();
            if (userSpecified.IsTeamMember()) eab.WithIconUrl(userSpecified.GetEmbedAuthorBuilderIconUrl());
            if (userSpecified.GetFooterText() != null)
            {
                efb.WithText(userSpecified.GetFooterText());
                efb.WithIconUrl(userSpecified.GetEmbedFooterBuilderIconUrl());
            }

            Color aboutColor = new Color(User.Load(userSpecified.Id).AboutR, User.Load(userSpecified.Id).AboutG, User.Load(userSpecified.Id).AboutB);

            EmbedBuilder eb = new EmbedBuilder()
                .WithAuthor(eab)
                .WithFooter(efb)
                .WithThumbnailUrl(userSpecified.GetAvatarUrl())
                .WithDescription(User.Load(userSpecified.Id).About)
                .WithColor(aboutColor);

            if (User.Load(userSpecified.Id).Name != null)
                eb.AddField("Name", userSpecified.GetName(), true);

            if (User.Load(userSpecified.Id).Gender != null)
                eb.AddField("Gender", userSpecified.GetGender(), true);
            
            if (User.Load(userSpecified.Id).Pronouns != null)
                eb.AddField("Pronouns", userSpecified.GetPronouns(), true);
            
            eb.AddField("Coins", userSpecified.GetCoins(), true);
            eb.AddField("Mythical Tokens", userSpecified.GetMythicalTokens(), true);
            eb.AddField("Account Created", userSpecified.UserCreateDate(), true);
            eb.AddField("Joined Guild", userSpecified.GuildJoinDate(), true);
            
            if (User.Load(userSpecified.Id).MinecraftUsername != null)
                eb.AddField("Minecraft Username", userSpecified.GetMinecraftUser(), true);

            if (userSpecified.GetWebsiteUrl() != null)
            {
                string websiteUrl = userSpecified.GetWebsiteUrl();
                string websiteName = userSpecified.GetWebsiteName() ?? StringConfiguration.Load().DefaultWebsiteName;
                eb.AddField(websiteName, "[" + websiteName + "](" + websiteUrl + ")", true);
            }

            if (User.Load(userSpecified.Id).Snapchat != null)
            {
                string snapchat = userSpecified.GetSnapchatUsername();
                eb.AddField("Snapchat", "[" + snapchat + "](https://www.snapchat.com/add/" + snapchat + "/)", true);
            }

            if (User.Load(userSpecified.Id).CustomPrefix != null)
                eb.AddField("Custom Prefix", User.Load(userSpecified.Id).CustomPrefix, true);

            await ReplyAsync("", false, eb.Build());
            typing.Dispose(); // trial 
        }

        #region User Sets
        [Command("setname"), Summary("")]
        public async Task SetName([Remainder]string name)
        {
            User.UpdateUser(Context.User.Id, name: name);
            await ReplyAsync("Updated successfully, " + Context.User.Mention);
        }

        [Command("setgender"), Summary("")]
        public async Task SetGender([Remainder]string gender)
        {
            User.UpdateUser(Context.User.Id, gender: gender);
            await ReplyAsync("Updated successfully, " + Context.User.Mention);
        }

        [Command("setpronouns"), Summary("Set your pronouns!")]
        public async Task SetUserPronouns([Remainder]string pronouns)
        {
            User.UpdateUser(Context.User.Id, pronouns: pronouns);
            await ReplyAsync("Updated successfully, " + Context.User.Mention);
        }

        [Command("setabout"), Summary("Set your about message!")]
        public async Task SetUserAbout([Remainder]string aboutMessage)
        {
            User.UpdateUser(Context.User.Id, about: aboutMessage);
            await ReplyAsync("Updated successfully, " + Context.User.Mention);
        }

        [Command("setprefix"), Summary("Custom set the prefix for the user.")]
        public async Task CustomUserPrefix([Remainder]string prefix = null)
        {
            if (prefix == null)
            {
                await ReplyAsync("**Syntax:** " + GuildConfiguration.Load(Context.Guild.Id).Prefix + "setprefix [prefix]\n\n`This feature will cost you: " + Configuration.Load().PrefixCost + " coin(s)`");
                return;
            }

            if (User.Load(Context.User.Id).Coins >= Configuration.Load().PrefixCost)
            {
                User.UpdateUser(Context.User.Id, customPrefix: prefix);
                User.UpdateUser(Context.User.Id, coins: (User.Load(Context.User.Id).Coins - Configuration.Load().PrefixCost));
                TransactionLogger.AddTransaction(Context.User.Username + " [" + Context.User.Id + "] paid " + Configuration.Load().PrefixCost + " for a custom prefix.");
                await ReplyAsync(Context.User.Mention + ", you have set `" + prefix + "` as a custom prefix for yourself. Please do take note that the following prefixes will work for you:\n```KEY: [Prefix][Command]\n" + prefix + " - User Set Prefix\n" + GuildConfiguration.Load(Context.Guild.Id).Prefix + " - Guild Set Prefix\n@" + DiscordBot.Bot.CurrentUser.Username + " - Global Prefix```");
            }
            else
            {
                await ReplyAsync("You do not have enough coins to pay for this feature, " + Context.User.Mention + "! This feature costs " + Configuration.Load().PrefixCost + " coins.");
            }

        }

        [Command("setcustomrgb"), Summary("Custom set the color of the about embed message.")]
        public async Task SetUserRgb(int r = -1, int g = -1, int b = -1)
        {
            if (r < 0 || r > 255 || g < 0 || g > 255 || b < 0 || b > 255)
            {
                await ReplyAsync(Context.User.Mention + ", you have entered an invalid value. You can use this website to help get your RGB values - <http://www.colorhexa.com/>\n\n" +
                    "**Syntax:** " + GuildConfiguration.Load(Context.Guild.Id).Prefix + "setcustomrgb [R value] [G value] [B value]\n**Example:** " + GuildConfiguration.Load(Context.Guild.Id).Prefix + "setcustomrgb 140 90 210");
                return;
            }

            byte rValue, gValue, bValue;

            try
            {
                byte.TryParse(r.ToString(), out rValue);
                byte.TryParse(g.ToString(), out gValue);
                byte.TryParse(b.ToString(), out bValue);
            }
            catch (Exception e)
            {
                Console.WriteLine(@"ERROR" + Environment.NewLine + e.Message);
                await ReplyAsync("An unexpected error has happened. Please ensure that you have passed through a byte value! (A number between 0 and 255)");
                return;
            }

            User.UpdateUser(Context.User.Id, aboutR: rValue);
            User.UpdateUser(Context.User.Id, aboutG: gValue);
            User.UpdateUser(Context.User.Id, aboutB: bValue);

            Color aboutColor = new Color(User.Load(Context.User.Id).AboutR, User.Load(Context.User.Id).AboutG, User.Load(Context.User.Id).AboutB);

            EmbedBuilder eb = new EmbedBuilder()
                .WithTitle("Sample Message")
                .WithDescription("<-- FYI, this is what you updated.")
                .WithColor(aboutColor);

            await ReplyAsync(Context.User.Mention + ", updated successfully.", false, eb.Build());
        }

        [Command("setmcusername"), Summary("")]
        [Alias("setminecraftusername", "mcreg", "mcregister")]
        public async Task SetMinecraftUsername([Remainder]string username)
        {
            User.UpdateUser(Context.User.Id, minecraftUsername:username);
            await ReplyAsync("Updated successfully, " + Context.User.Mention);
        }

        [Command("setsnapchat"), Summary("")]
        public async Task SetSnapchatUsername([Remainder]string username)
        {
            User.UpdateUser(Context.User.Id, snapchat:username);
            await ReplyAsync("Updated successfully, " + Context.User.Mention);
        }

        [Command("setwebsitename"), Summary("")]
        public async Task SetWebsiteName([Remainder]string name)
        {
            User.UpdateUser(Context.User.Id, websiteName: name);
            await ReplyAsync("Updated successfully, " + Context.User.Mention);
        }

        [Command("setwebsiteurl"), Summary("")]
        public async Task SetWebsiteUrl([Remainder]string url)
        {
            User.UpdateUser(Context.User.Id, websiteUrl: url);
            await ReplyAsync("Updated successfully, " + Context.User.Mention);
        }

        #endregion
    }
}
