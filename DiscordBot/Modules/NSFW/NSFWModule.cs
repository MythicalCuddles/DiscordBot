/* WARNING
 * THIS SECTION CONTAINS NSFW CONTENT
 * THIS IS YOUR ONLY WARNING! 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

using DiscordBot.Common.Preconditions;
using DiscordBot.Common;
using DiscordBot.Extensions;
using DiscordBot.Objects;
using HtmlAgilityPack;

using MelissaNet;

namespace DiscordBot.Modules.NSFW
{
    [Name("NSFW Commands")]
    [RequireContext(ContextType.Guild)]
    [MinPermissions(PermissionLevel.User)]
    public class NsfwModule : ModuleBase
    {
        private readonly Random _random = new Random();

        // Rule 34 Gamble for NSFW Server - Contains NSFW Links
        
        [Command("rule34gamble"), Summary("Head to #nsfw-rule34gamble and read the description for more information.")]
        [Alias("34gamble", "rule34")]
        public async Task Rule34Gamble(int postId = 0)
        {
            WebClient client = new WebClient();
            HtmlDocument doc = new HtmlDocument();

            if (Guild.Load(Context.Guild.Id).NSFWCommandsEnabled && Context.Channel.Id == Guild.Load(Context.Guild.Id).RuleGambleChannelID || Context.User.Id == Configuration.Load().Developer)
            {
                if (((SocketTextChannel) Context.Channel).IsNsfw)
                {
                    try
                    {
                        int id;
                        if (postId != 0 && Context.User.IsBotOwner())
                        {
                            id = postId;
                        }
                        else
                        {
                            id = _random.Next(1, Configuration.Load().MaxRuleXGamble);
                        }
                        var url = "https://rule34.xxx/index.php?page=post&s=view&id=" + id.ToString();
                        var html = client.DownloadString(url);
                        doc.LoadHtml(html);

                        List<HtmlNode> imageNodes = (from HtmlNode node in doc.DocumentNode.SelectNodes("//img")
                                      where node.Name == "img"
                                      select node).ToList();

                        List<string> images = new List<string>();
                        foreach (HtmlNode node in imageNodes)
                        {
                            images.Add(node.Attributes["src"].Value);
                        }
                        
                        foreach (string s in images)
						{

						    if (s.Contains("thumbnails") || s.Contains("samples"))
						    {
						        await Rule34Gamble().ConfigureAwait(false);
						        return;
						    }
						}

                        string link = images[3].FindAndReplaceFirstInstance("//", "temp");
                        link = link.FindAndReplaceFirstInstance("//", "/");
                        link = link.FindAndReplaceFirstInstance("temp", "//");

                        await new LogMessage(LogSeverity.Info, "Rule34Gamble", Context.User.Username + " got " + id).PrintToConsole();
                        
                        EmbedBuilder eb = new EmbedBuilder
                        {
                            Title = "Congratulations! You won the following image.",
                            ImageUrl = link,
                            Color = Context.User.GetCustomRGB(),
                            Footer = new EmbedFooterBuilder
                            {
                                Text = "ID: " + id
                            }
                        }.WithCurrentTimestamp();
                        
                        await ReplyAsync("", false, eb.Build());
                    }
                    catch (Exception ex)
                    {
                        await new LogMessage(LogSeverity.Warning, "Rule34Gamble/Exception", ex.Message).PrintToConsole();

						await ReplyAsync(Context.User.Mention + ", the random Id you got returned no image. Lucky you! Why not try again?");
                    }
                }
                else
                {
                    await ReplyAsync("**Warning!** This command requires the channel to have its NSFW setting enabled!\nDue to current restrictions with Discord.Net, this means that the channel must start with *nsfw-*");
                }
            }
        }
        
    }
}
