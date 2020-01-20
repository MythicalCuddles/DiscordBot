using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;

using DiscordBot.Common;
using DiscordBot.Common.Preconditions;
using DiscordBot.Objects;

namespace DiscordBot.Modules.Public
{
    [Name("Help Commands")]
    [RequireGuild(222022898397741057)]
    public class HelpModule : ModuleBase<CommandContext>
    {
        public CommandService Service;

        public HelpModule(CommandService service)
        {
            Service = service;
        }

        [Command("help")]
        public async Task HelpAsync(string command = null)
        {
            if(command == null)
            {
                string prefix = Guild.Load(Context.Guild.Id).Prefix;
                var builder = new EmbedBuilder
                {
                    Color = new Color(114, 137, 218),
                    Description = "These are the commands you can use."
                };

                foreach (var module in Service.Modules)
                {
                    var description = new StringBuilder();
                    foreach (var cmd in module.Commands)
                    {
                        var result = await cmd.CheckPreconditionsAsync(Context);
                        if (result.IsSuccess)
                        {
                            if (string.IsNullOrEmpty(cmd.Summary))
                            {
                                description.Append($"{prefix}{cmd.Aliases.First()}\n");
                            }
                            else
                            {
                                description.Append($"{prefix}{cmd.Aliases.First()} - {cmd.Summary}\n");
                            }
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(description.ToString()))
                    {
                        builder.AddField(x =>
                        {
                            x.Name = module.Name;
                            x.Value = description;
                            x.IsInline = false;
                        });
                    }
                }

                await ReplyAsync(Context.User.Mention + ", I've sent you a DM with the information you requested.");
                await Context.User.GetOrCreateDMChannelAsync().Result.SendMessageAsync("", false, builder.Build());
            }
            else
            {
                var result = Service.Search(Context, command);

                if (!result.IsSuccess)
                {
                    await ReplyAsync($"Sorry, I couldn't find a command like **{command}**.");
                    return;
                }

                var builder = new EmbedBuilder
                {
                    Color = new Color(114, 137, 218),
                    Description = $"Here are some commands like **{command}**"
                };

                foreach (var match in result.Commands)
                {
                    var cmd = match.Command;

                    builder.AddField(x =>
                    {
                        x.Name = string.Join(", ", cmd.Aliases);
                        x.Value = $"Parameters: {string.Join(", ", cmd.Parameters.Select(p => p.Name))}\n" +
                                  $"Remarks: {cmd.Remarks}";
                        x.IsInline = false;
                    });
                }

                await ReplyAsync(Context.User.Mention + ", I've sent you a DM with the information you requested.");
                await Context.User.GetOrCreateDMChannelAsync().Result.SendMessageAsync("", false, builder.Build());
            }
        }
    }

}
