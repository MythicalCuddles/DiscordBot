using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;

using DiscordBot.Common;
using DiscordBot.Extensions;
using DiscordBot.Objects;

namespace DiscordBot.Handlers
{
    public class ReactionHandler
    {
        public static async Task ReactionAdded(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            if (reaction.User.Value.IsBot)
            {
                return;
            }

            if(Configuration.Load().AwardingEXPEnabled)
            {
                if (Configuration.Load().AwardingEXPReactionEnabled)
                {
                    reaction.User.Value.AwardEXPToUser(channel.GetGuild());
                }

                if (Configuration.Load().AwardingEXPReactPostEnabled)
                {
                    message.Value.Author.AwardEXPToUser(channel.GetGuild());
                }
            }
            
            await new LogMessage(LogSeverity.Info, "ReactionAdded", "[" + channel.GetGuild() + "/#" + channel.Name + "] " + "[@ " + reaction.User.Value.Username + 
                                                                "] : " + reaction.Emote.Name).PrintToConsole();
        }
    }
}
