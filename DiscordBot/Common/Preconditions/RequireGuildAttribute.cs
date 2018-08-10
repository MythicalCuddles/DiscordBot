using System;
using System.Threading.Tasks;

using Discord.Commands;
using DiscordBot.Extensions;

namespace DiscordBot.Common.Preconditions
{
    // Feature is currently in testing and may not work as expected. Please report any issues back to @Melissa#0002
    
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequireGuildAttribute : PreconditionAttribute
    {
        private readonly ulong _id;

        public RequireGuildAttribute(ulong guildId)
        {
            _id = guildId;
        }

        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            return CheckGuildAsync(context, command, services);
        }

        public Task<PreconditionResult> CheckGuildAsync(ICommandContext commandContext, CommandInfo commandInfo, IServiceProvider serviceProvider)
        {
            ulong id = GetGuildId(commandContext);
            return Task.FromResult(id == _id ? PreconditionResult.FromSuccess() : PreconditionResult.FromError("This command can only be issued in the guild: " + _id.GetGuild() + "!"));
        }

        public ulong GetGuildId(ICommandContext context)
        {
            return context.Guild.Id;
        }
    }
}
