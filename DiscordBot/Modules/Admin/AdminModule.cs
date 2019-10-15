using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;

using DiscordBot.Common.Preconditions;
using DiscordBot.Common;
using DiscordBot.Extensions;
using DiscordBot.Objects;

namespace DiscordBot.Modules.Admin
{
    [Name("Admin Commands")]
    [RequireContext(ContextType.Guild)]
    [MinPermissions(PermissionLevel.ServerAdmin)]
    public class AdminModule : InteractiveBase
    {
        [Command("echo"), Summary("Echos a message.")]
        public async Task Say([Remainder, Summary("The text to echo")] string echo)
        {
            await ReplyAsync(echo);
            await Context.Message.DeleteAsync();
        }

        [Command("welcome"), Summary("Send the welcome messages to the user specified.")]
        public async Task SendWelcomeMessage(SocketGuildUser user)
        {
            await Guild.Load(Context.Guild.Id).WelcomeChannelID.GetTextChannel().SendMessageAsync(Guild.Load(Context.Guild.Id).WelcomeMessage.ModifyStringFlags(user));
            await Guild.Load(Context.Guild.Id).LogChannelID.GetTextChannel().SendMessageAsync("A welcome message for " + user.Mention + " has been posted. (Forced by: " + Context.User.Mention + ")");
        }

        [Command("addquote"), Summary("Add a quote to the list.")]
        public async Task AddQuote([Remainder]string quote = null)
        {
            EmbedBuilder eb;
            if (quote == null)
            {
                eb = new EmbedBuilder
                {
                    Title = "Invalid Syntax",
                    Description = "**Syntax:** " + Guild.Load(Context.Guild.Id).Prefix + "addquote [quote] (@creator)",
                    Color = new Color(210, 47, 33)
                };

                await ReplyAsync("", false, eb.Build());
                return;       
            }
            
            Quote.AddQuote(quote, Context.User.Id, Context.Guild.Id);
			
			eb = new EmbedBuilder()
				.WithDescription(Context.User.Mention + " Quote Added")
				.WithColor(33, 210, 47);

			await ReplyAsync("", false, eb.Build());
        }

        [Command("listquotes"), Summary("Sends a list of all the quotes.")]
        public async Task ListQuotes()
        {
            EmbedBuilder eb;
            if (!Quote.Quotes.Any())
            {
                eb = new EmbedBuilder
                {
                    Title = "Quotes",
                    Description = "There is no quotes in the database.",
                    Color = new Color(235, 160, 40)
                };

                await ReplyAsync("", false, eb.Build());
                return;
            }
            
            //todo: print quotes.
        }

        [Command("editquote"), Summary("Edit a quote from the list.")]
        public async Task EditQuote(int quoteId = 0, [Remainder]string quote = null)
        {
            EmbedBuilder eb;
            if (quoteId == 0 && quote == null)
            {
                eb = new EmbedBuilder
                {
                    Title = "Invalid Syntax",
                    Description = "**Syntax:** " + Guild.Load(Context.Guild.Id).Prefix + "editquote [quote ID] [quote text]",
                    Color = new Color(210, 47, 33)
                };

                await ReplyAsync("", false, eb.Build());
                return;
            }
            
            if (quoteId > Quote.Quotes.Count || quoteId == 0)
            {
                eb = new EmbedBuilder
                {
                    Title = "Unknown Quote ID",
                    Description = "Unknown Quote ID. Please check the quote list for valid IDs.",
                    Color = new Color(210, 47, 33)
                };

                await ReplyAsync("", false, eb.Build());
                return; 
            }
            
            string oldQuote = Quote.Quotes.Find(q => q.QId == quoteId).QuoteText;
            
            Quote.UpdateQuote(quoteId, quote);
            
            eb = new EmbedBuilder
            {
                Title = "Quote #" + quoteId + " edited",
                Description = "Quote: " + quote,
                Color = new Color(0, 255, 0),
                Footer = new EmbedFooterBuilder
                {
                    Text = "Old Quote: " + oldQuote
                }
            };

            await ReplyAsync("", false, eb.Build());
        }

        [Command("deletequote"), Summary("Delete a quote from the list. Make sure to `$listquotes` to get the ID for the quote being removed!")]
        public async Task RemoveQuote(int quoteId = 0)
        {
            EmbedBuilder eb;
            if (quoteId == 0)
            {
                eb = new EmbedBuilder
                {
                    Title = "Invalid Syntax / Invalid ID",
                    Description = "**Syntax:** " + Guild.Load(Context.Guild.Id).Prefix + "deletequote [id]",
                    Color = new Color(210, 47, 33)
                };

                await ReplyAsync("", false, eb.Build());
                return;
            }

            Quote q = Quote.Quotes.Find(quote => quote.QId == quoteId);
            if (q == null)
            {
                eb = new EmbedBuilder
                {
                    Title = "Unable to find Quote",
                    Description = "Unable to find a quote with that ID in the database.",
                    Color = new Color(210, 47, 33)
                };

                await ReplyAsync("", false, eb.Build());
                return;
            }
            
            Quote.DeleteQuote(quoteId);
            eb = new EmbedBuilder
            {
                Title = "Quote #" + quoteId + " deleted",
                Description = q.QuoteText,
                Footer = new EmbedFooterBuilder
                {
                    Text = "Author: @" + q.CreatorId.GetUser().Username
                }
            };
            await ReplyAsync("", false, eb.Build());
        }

        [Command("listrequestquotes"), Summary("Sends a list of all the request quotes.")]
        public async Task ListRequestQuotes()
        {
            EmbedBuilder eb;
            if (!RequestQuote.RequestQuotes.Any())
            {
                eb = new EmbedBuilder
                {
                    Title = "Pending Request Quotes",
                    Description = "There is currently 0 quotes pending review.",
                    Color = new Color(235, 160, 40)
                };

                await ReplyAsync("", false, eb.Build());
                return;
            }
            
            //todo: print quotes.
        }

        [Command("acceptquote"), Summary("Add a quote to the list.")]
        public async Task AcceptQuote(int quoteId = 0)
        {
            EmbedBuilder eb;
            if (quoteId == 0)
            {
                eb = new EmbedBuilder
                {
                    Title = "Invalid Syntax / Invalid ID",
                    Description = "**Syntax:** " + Guild.Load(Context.Guild.Id).Prefix + "acceptquote [id]",
                    Color = new Color(210, 47, 33)
                };

                await ReplyAsync("", false, eb.Build());
                return;
            }

            RequestQuote q = RequestQuote.RequestQuotes.Find(quote => quote.RequestId == quoteId);
            if (q == null)
            {
                eb = new EmbedBuilder
                {
                    Title = "Unable to find Quote",
                    Description = "Unable to find a request quote with that ID in the database.",
                    Color = new Color(210, 47, 33)
                };

                await ReplyAsync("", false, eb.Build());
                return;
            }
            RequestQuote.ApproveRequestQuote(quoteId, Context.User.Id, Context.Guild.Id);
            
            eb = new EmbedBuilder
            {
                Title = "Quote #" + quoteId + " approved",
                Author = new EmbedAuthorBuilder
                {
                    Name = "@" + q.CreatedBy.GetUser().Username,
                    IconUrl = q.CreatedBy.GetUser().GetAvatarUrl()
                },
                Description = q.QuoteText,
                Footer = new EmbedFooterBuilder
                {
                    Text = "Approved by @" + Context.User.Username,
                    IconUrl = Context.User.GetAvatarUrl()
                }
            };

            await ReplyAsync("", false, eb.Build());
            await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("", false, eb.Build());
        }

        [Command("denyquote"), Summary("")]
        [Alias("rejectquote")]
        public async Task DenyQuote(int quoteId)
        {
            EmbedBuilder eb;
            if (quoteId == 0)
            {
                eb = new EmbedBuilder
                {
                    Title = "Invalid Syntax / Invalid ID",
                    Description = "**Syntax:** " + Guild.Load(Context.Guild.Id).Prefix + "denyquote [id]",
                    Color = new Color(210, 47, 33)
                };

                await ReplyAsync("", false, eb.Build());
                return;
            }
            
            RequestQuote q = RequestQuote.RequestQuotes.Find(quote => quote.RequestId == quoteId);
            if (q == null)
            {
                eb = new EmbedBuilder
                {
                    Title = "Unable to find Quote",
                    Description = "Unable to find a request quote with that ID in the database.",
                    Color = new Color(210, 47, 33)
                };

                await ReplyAsync("", false, eb.Build());
                return;
            }
            RequestQuote.RemoveRequestQuote(quoteId);
            
            eb = new EmbedBuilder
            {
                Title = "Quote #" + quoteId + " rejected",
                Author = new EmbedAuthorBuilder
                {
                    Name = "@" + q.CreatedBy.GetUser().Username,
                    IconUrl = q.CreatedBy.GetUser().GetAvatarUrl()
                },
                Description = q.QuoteText,
                Footer = new EmbedFooterBuilder
                {
                    Text = "Rejected by @" + Context.User.Username,
                    IconUrl = Context.User.GetAvatarUrl()
                }
            };

            await ReplyAsync("", false, eb.Build());
            await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("", false, eb.Build());
        }
    }
}
