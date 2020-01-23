using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Common;
using DiscordBot.Extensions;
using DiscordBot.Objects;

namespace DiscordBot.Handlers
{
    public static class MessageHandler
    {
        internal static async Task MessageReceived(SocketMessage messageParam)
        {
            if (!(messageParam is SocketUserMessage message)) { return; } // If the message is null, return.
            if (message.Author.IsBot) { return; } // If the message was posted by a BOT account, return.
            if (message.Author.IsUserIgnoredByBot() && message.Author.Id != Configuration.Load().Developer) { return; } // If the bot is ignoring the user AND the user NOT Melissa.

            // If the message came from somewhere that is not a text channel -> Private Message
            if (!(messageParam.Channel is ITextChannel))
            {
                EmbedFooterBuilder efb = new EmbedFooterBuilder()
                    .WithText("UID: " + message.Author.Id + " | MID: " + message.Id);
                EmbedBuilder eb = new EmbedBuilder()
                    .WithTitle("Private Message - Posted By: @" + message.Author.Username + "#" + message.Author.Discriminator)
                    .WithDescription(message.Content)
                    .WithFooter(efb)
                    .WithCurrentTimestamp();

                await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("", false, eb.Build());

                return;
            }

	        await new LogMessage(LogSeverity.Info, "MessageReceived", "[" + messageParam.Channel.GetGuild().Name + "/#" + messageParam.Channel.Name + "] " + "[@" + 
	                                                            messageParam.Author.Username + "] : " + messageParam.Content).PrintToConsole();

            var uPrefix = message.Author.GetCustomPrefix();
            var gPrefix = Guild.Load(message.Channel.GetGuild().Id).Prefix;
            if (string.IsNullOrEmpty(uPrefix)) { uPrefix = gPrefix; } // Fixes an issue with users not receiving coins due to null prefix.
            var argPos = 0;
            if (message.HasStringPrefix(gPrefix, ref argPos) || 
                message.HasMentionPrefix(DiscordBot.Bot.CurrentUser, ref argPos) || 
                message.HasStringPrefix(uPrefix, ref argPos)) {
                var context = new SocketCommandContext(DiscordBot.Bot, message);
                var result = await DiscordBot.CommandService.ExecuteAsync(context, argPos, DiscordBot.ServiceProvider);

                if (!result.IsSuccess && Configuration.Load().UnknownCommandEnabled)
                {
                    var errorMessage = await context.Channel.SendMessageAsync(messageParam.Author.Mention + ", " + result.ErrorReason);

	                await new LogMessage(LogSeverity.Error, "MessageReceived", message.Author.Username + " - " + result.ErrorReason).PrintToConsole();

                    errorMessage.DeleteAfter(20);
                }
            }
            else if (message.Content.ToUpper() == "F") // If the message is just "F", pay respects.
            {
	            var respects = Configuration.Load().Respects + 1;
                Configuration.UpdateConfiguration(respects: respects);

                var eb = new EmbedBuilder()
                    .WithDescription("**" + message.Author.Username + "** has paid their respects.")
                    .WithFooter("Total Respects: " + respects)
                    .WithColor(message.Author.GetCustomRGB());

	            await message.Channel.SendMessageAsync("", false, eb.Build());
            }
            else
            {
	            if(Configuration.Load().AwardingEXPEnabled)
                {
	                if (message.Content.Length >= Configuration.Load().MinLengthForEXP)
	                {
						if (Channel.Load(message.Channel.Id).AwardingEXP)
						{
							message.Author.AwardEXPToUser(message.Channel.GetGuild());
						}
	                }
                }
            }
        }
    }
}