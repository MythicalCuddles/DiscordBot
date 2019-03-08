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
        private readonly WebClient _client = new WebClient();
        private readonly HtmlDocument _doc = new HtmlDocument();
        private string _html, _url;
        private int _id;
        [Command("rule34gamble"), Summary("Head to #nsfw-rule34gamble and read the description for more information.")]
        [Alias("34gamble", "rule34")]
        public async Task Rule34Gamble(int postId = 0)
        {
            if (Guild.Load(Context.Guild.Id).NSFWCommandsEnabled && Context.Channel.Id == Guild.Load(Context.Guild.Id).RuleGambleChannelID || Context.User.Id == Configuration.Load().Developer)
            {
                if (((SocketTextChannel) Context.Channel).IsNsfw)
                {
                    //var message = await ReplyAsync("Please wait while we draw your lucky number! (This shouldn't take long)");

                    try
                    {
                        if (postId != 0 && Context.User.IsBotOwner())
                        {
                            _id = postId;
                        }
                        else
                        {
                            _id = _random.Next(1, Configuration.Load().MaxRuleXGamble);
                        }
                        _url = "https://rule34.xxx/index.php?page=post&s=view&id=" + _id.ToString();
                        _html = _client.DownloadString(_url);
                        _doc.LoadHtml(_html);

                        List<HtmlNode> imageNodes = (from HtmlNode node in _doc.DocumentNode.SelectNodes("//img")
                                      where node.Name == "img"
                                      select node).ToList();

                        List<string> images = new List<string>();
                        foreach (HtmlNode node in imageNodes)
                        {
                            images.Add(node.Attributes["src"].Value);
                        }
                        
                        // for debugging
						//Console.Write(@"[");
						//Console.ForegroundColor = ConsoleColor.Cyan;
						//Console.Write(@"RULE34 GAMBLE");
						//Console.ResetColor();
						//Console.WriteLine(@"]: " + Context.User.Username + @" got the Gamble Id: " + _id.ToString() + Environment.NewLine + @"The following images were gathered using that Id:");
                        
                        foreach (string s in images)
						{
						    // for debugging
							//Console.WriteLine(s);

						    if (s.Contains("thumbnails") || s.Contains("samples"))
						    {
						        await Rule34Gamble();
						        return;
						    }
						}

                        string link = images[3].FindAndReplaceFirstInstance("//", "temp");
                        link = link.FindAndReplaceFirstInstance("//", "/");
                        link = link.FindAndReplaceFirstInstance("temp", "//");
                        
                        // for debugging
                        //Console.WriteLine(@"[Final] " + link);

                        await new LogMessage(LogSeverity.Info, "Rule34Gamble", Context.User.Username + " got " + _id).PrintToConsole();
                        
                        EmbedBuilder eb = new EmbedBuilder()
                        {
                            Title = "Congratulations! You won the following image.",
                            ImageUrl = link,
                            Color = Context.User.GetCustomRGB(),
                            Footer = new EmbedFooterBuilder()
                            {
                                Text = "ID: " + _id
                            }
                        }.WithCurrentTimestamp();
                        
                        //await ReplyAsync(Context.User.Mention + ", congratulations, you won the following image: \n" + link);
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
