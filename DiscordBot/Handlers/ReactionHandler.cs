using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;

using DiscordBot.Common;
using DiscordBot.Extensions;
using DiscordBot.Objects;
using DiscordBot.Other;

namespace DiscordBot.Handlers
{
    public class ReactionHandler
    {
        public static async Task ReactionAdded(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            if (reaction.User.Value.IsBot)
                return;

            if (QuoteHandler.QuoteMessages.Contains(message.Id))
            {
                await HandleQuoteReactions(message, reaction);
                return;
            }

            if (QuoteHandler.RequestQuoteMessages.Contains(message.Id))
            {
                await HandleRequestQuoteReactions(message, channel, reaction);
                return;
            }

            if(Configuration.Load().AwardingEXPEnabled)
            {
                message.Value.Author.AwardEXPToUser(channel.GetGuild());
                reaction.User.Value.AwardEXPToUser(channel.GetGuild());
            }
            
            await new LogMessage(LogSeverity.Info, "ReactionAdded", "[" + channel.GetGuild() + "/#" + channel.Name + "] " + "[@ " + reaction.User.Value.Username + 
                                                                "] : " + reaction.Emote.Name).PrintToConsole();
        }

        private static async Task HandleQuoteReactions(Cacheable<IUserMessage, ulong> message, SocketReaction reaction)
        {
            // Check to see if the next page or previous page was clicked.
            if (reaction.Emote.Name == Extensions.Extensions.ArrowLeft.Name)
            {
                if (QuoteHandler.PageNumber[QuoteHandler.QuoteMessages.IndexOf(message.Id)] == 1)
                    return;

                QuoteHandler.PageNumber[QuoteHandler.QuoteMessages.IndexOf(message.Id)]--;
            }
            else if (reaction.Emote.Name == Extensions.Extensions.ArrowRight.Name)
            {
                if (QuoteHandler.PageNumber[QuoteHandler.QuoteMessages.IndexOf(message.Id)] == QuoteHandler.GetQuotesListLength)
                    return;

                QuoteHandler.PageNumber[QuoteHandler.QuoteMessages.IndexOf(message.Id)]++;
            }

            StringBuilder sb = new StringBuilder()
            .Append("**Quote List** : *Page " + QuoteHandler.PageNumber[QuoteHandler.QuoteMessages.IndexOf(message.Id)] + "*\n```");

            List<string> quotes = QuoteHandler.GetQuotes(QuoteHandler.PageNumber[QuoteHandler.QuoteMessages.IndexOf(message.Id)]);

            for (int i = 0; i < quotes.Count; i++)
            {
                sb.Append(((i + 1) + ((QuoteHandler.PageNumber[QuoteHandler.QuoteMessages.IndexOf(message.Id)] - 1) * 10)) + ": " + quotes[i] + "\n");
            }

            sb.Append("```");

            await message.Value.ModifyAsync(msg => msg.Content = sb.ToString());
            await message.Value.RemoveAllReactionsAsync();

            if (QuoteHandler.PageNumber[QuoteHandler.QuoteMessages.IndexOf(message.Id)] == 1)
            {
                await message.Value.AddReactionAsync(Extensions.Extensions.ArrowRight);
            }
            else if (QuoteHandler.PageNumber[QuoteHandler.QuoteMessages.IndexOf(message.Id)] == QuoteHandler.GetQuotesListLength)
            {
                await message.Value.AddReactionAsync(Extensions.Extensions.ArrowLeft);
            }
            else
            {
                await message.Value.AddReactionAsync(Extensions.Extensions.ArrowLeft);
                await message.Value.AddReactionAsync(Extensions.Extensions.ArrowRight);
            }
        }

        private static async Task HandleRequestQuoteReactions(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            // Check to see if the next page or previous page was clicked.
            if (reaction.Emote.Name == Extensions.Extensions.ArrowLeft.Name)
            {
                if (QuoteHandler.RequestPageNumber[QuoteHandler.RequestQuoteMessages.IndexOf(message.Id)] == 1)
                    return;

                QuoteHandler.RequestPageNumber[QuoteHandler.RequestQuoteMessages.IndexOf(message.Id)]--;
            }
            else if (reaction.Emote.Name == Extensions.Extensions.ArrowRight.Name)
            {
                if (QuoteHandler.RequestPageNumber[QuoteHandler.RequestQuoteMessages.IndexOf(message.Id)] == QuoteHandler.GetRequestQuotesListLength)
                    return;

                QuoteHandler.RequestPageNumber[QuoteHandler.RequestQuoteMessages.IndexOf(message.Id)]++;
            }

            StringBuilder sb = new StringBuilder()
            .Append("**Request Quote List** : *Page " + QuoteHandler.RequestPageNumber[QuoteHandler.RequestQuoteMessages.IndexOf(message.Id)] + "*\nTo accept a quote, type **" + Guild.Load(channel.GetGuild().Id).Prefix + "acceptquote[id]**.\nTo reject a quote, type **" + Guild.Load(channel.GetGuild().Id).Prefix + "denyquote[id]**.\n```");

            List<string> requestQuotes = QuoteHandler.GetRequestQuotes(QuoteHandler.RequestPageNumber[QuoteHandler.RequestQuoteMessages.IndexOf(message.Id)]);

            for (int i = 0; i < requestQuotes.Count; i++)
            {
                sb.Append(((i + 1) + ((QuoteHandler.RequestPageNumber[QuoteHandler.RequestQuoteMessages.IndexOf(message.Id)] - 1) * 10)) + ": " + requestQuotes[i] + "\n");
            }

            sb.Append("```");

            await message.Value.ModifyAsync(msg => msg.Content = sb.ToString());
            await message.Value.RemoveAllReactionsAsync();

            if (QuoteHandler.RequestPageNumber[QuoteHandler.RequestQuoteMessages.IndexOf(message.Id)] == 1)
            {
                await message.Value.AddReactionAsync(Extensions.Extensions.ArrowRight);
            }
            else if (QuoteHandler.RequestPageNumber[QuoteHandler.RequestQuoteMessages.IndexOf(message.Id)] == QuoteHandler.GetRequestQuotesListLength)
            {
                await message.Value.AddReactionAsync(Extensions.Extensions.ArrowLeft);
            }
            else
            {
                await message.Value.AddReactionAsync(Extensions.Extensions.ArrowLeft);
                await message.Value.AddReactionAsync(Extensions.Extensions.ArrowRight);
            }
        }
    }
}
