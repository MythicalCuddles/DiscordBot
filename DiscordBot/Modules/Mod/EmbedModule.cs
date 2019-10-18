using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using DiscordBot.Common.Preconditions;
using DiscordBot.Common;
using DiscordBot.Extensions;
using DiscordBot.Handlers;
using DiscordBot.Logging;
using DiscordBot.Objects;

namespace DiscordBot.Modules.Mod
{
    [Name("Embed Commands")]
    [Group("embed")]
    [MinPermissions(PermissionLevel.ServerMod)]
    public class EmbedModule : ModuleBase
    {
        private static readonly List<IUser> Users = new List<IUser>();
        private static readonly List<EmbedBuilder> UserEmbeds = new List<EmbedBuilder>();

        [Command("")]
        public async Task SendEmbedCommands()
        {
            await ReplyAsync("**Syntax:** " +
                             Guild.Load(Context.Guild.Id).Prefix + "embed [subcommand] [parameters...] \n" +
                             "Available Commands\n" +
                             "```ini\n" +
                             "[1] embed new\n" +
                             "[2] embed withtitle [\"title\"]\n" +
                             "[3] embed withdescription [\"description\"]\n" +
                             "[4] embed withfooter [\"footer\"] [\"footer url = null\"]\n" +
                             "[5] embed withcolor [\"R Value\"] [\"G Value\"] [\"B Value\"]\n" +
                             "[6] embed send [\"#channel = #" + Context.Channel.Name + "\"]\n" +
                             "```");
            AdminLog.Log(Context.User.Id, Context.Message.Content, Context.Guild.Id);
        }

        [Command("new")]
        public async Task NewEmbed()
        {
            if (Users.Contains(Context.User))
            {
                int index = Users.FindIndex(user => user == Context.User);
                UserEmbeds[index] = new EmbedBuilder();

                var message = await ReplyAsync("Your embed has been reset.");
                message.DeleteAfter(5);
            }
            else
            {
                Users.Add(Context.User);
                UserEmbeds.Add(new EmbedBuilder());

                var message = await ReplyAsync("Your embed has been created. Please type `" + Guild.Load(Context.Guild.Id).Prefix + "embed` to see the available commands.");
                message.DeleteAfter(5);
            }
        }

        [Command("withtitle")]
        public async Task EmbedWithTitle(string title)
        {
            if (Users.Contains(Context.User))
            {
                int index = Users.FindIndex(user => user == Context.User);
                UserEmbeds[index].WithTitle(title);

                var message = await ReplyAsync("added");
                message.DeleteAfter(10);
            }
            else
            {
                await NewEmbed().ConfigureAwait(false);
                await EmbedWithTitle(title).ConfigureAwait(false);
            }
        }

        [Command("withdescription")]
        public async Task EmbedWithDescription(string description)
        {
            if (Users.Contains(Context.User))
            {
                int index = Users.FindIndex(user => user == Context.User);

                UserEmbeds[index].WithDescription(description);

                var message = await ReplyAsync("added");
                message.DeleteAfter(10);
            }
            else
            {
                await NewEmbed().ConfigureAwait(false);
                await EmbedWithDescription(description).ConfigureAwait(false);
            }
        }

        [Command("withfooter")]
        public async Task EmbedWithFooter(string footerText, string footerURL = null)
        {
            if (Users.Contains(Context.User))
            {
                int index = Users.FindIndex(user => user == Context.User);

                UserEmbeds[index].WithFooter(footerText, footerURL);

                var message = await ReplyAsync("added");
                message.DeleteAfter(10);
            }
            else
            {
                await NewEmbed().ConfigureAwait(false);
                await EmbedWithFooter(footerText, footerURL).ConfigureAwait(false);
            }
        }

        [Command("withcolor")]
        [Alias("withcolour")]   
        public async Task EmbedWithColor(int r = -1, int g = -1, int b = -1)
        {
            if (r < 0 || r > 255 || g < 0 || g > 255 || b < 0 || b > 255)
            {
                await ReplyAsync(Context.User.Mention + ", you have entered an invalid value. You can use this website to help get your RGB values - <http://www.colorhexa.com/>\n\n" +
                                 "**Syntax:** " + Guild.Load(Context.Guild.Id).Prefix + "embed withcolor [R value] [G value] [B value]\n**Example:** " + Guild.Load(Context.Guild.Id).Prefix + "embed withcolor 140 90 210");
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
                ConsoleHandler.PrintExceptionToLog("EmbedModule", e);
                await ReplyAsync("An unexpected error has happened. Please ensure that you have passed through a byte value! (A number between 0 and 255)");
                return;
            }

            if (Users.Contains(Context.User))
            {
                int index = Users.FindIndex(user => user == Context.User);

                UserEmbeds[index].WithColor(rValue, gValue, bValue);

                var message = await ReplyAsync("added");
                message.DeleteAfter(10);
            }
            else
            {
                await NewEmbed().ConfigureAwait(false);
                await EmbedWithColor(r, g, b).ConfigureAwait(false);
            }
        }

        [Command("send")]
        public async Task SendEmbed(SocketTextChannel channel = null)
        {
            if (Users.Contains(Context.User))
            {
                int index = Users.FindIndex(user => user == Context.User);

                if (channel == null)
                {
                    channel = Context.Channel as SocketTextChannel;
                }

                if (channel != null)
                {
                    await channel.SendMessageAsync("", false, UserEmbeds[index].Build());
                }
            }
            else
            {
                await NewEmbed().ConfigureAwait(false);
                await SendEmbed(channel).ConfigureAwait(false);
            }
        }
    }
}
