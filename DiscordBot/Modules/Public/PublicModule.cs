using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Common;
using DiscordBot.Common.Preconditions;
using DiscordBot.Extensions;

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
    }
}
