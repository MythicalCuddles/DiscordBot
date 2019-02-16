using System.Text;
using System.Threading.Tasks;

using Discord.Commands;
using Discord;

using DiscordBot.Extensions;
using DiscordBot.Common.Preconditions;
using DiscordBot.Common;
using DiscordBot.Objects;
using DiscordBot.Other;

namespace DiscordBot.Modules.Public
{
    [Name("Info Commands")]
    [MinPermissions(PermissionLevel.User)]
    public class InfoModule : ModuleBase
    {
        [Command("hotlines"), Summary("Sends hotline links for the user.")]
        public async Task LinkHotlines()
        {
			await ReplyAsync("If you **need help for yourself**, here are a few places where you can find voice and text hotlines.\n" +
			                 "<http://togetherweare-strong.tumblr.com/helpline> \n" +
			                 "<https://reddit.com/r/SuicideWatch/wiki/hotlines> \n" +
			                 "FAQ for Hotlines: <https://www.reddit.com/r/SWResources/comments/1c7ntr/suicide_hotline_faqs/> \n\n" +
			                 "If you are **concerned about someone else**, you should try to reach out to some of the hotlines in the above resources. Ensure you make it clear that you are contacting them on behalf of someone else and that you are concerned about them.\n" +
			                 "You may also need to assess the risk they may be putting themselves into. <https://www.reddit.com/r/SWResources/comments/1c7nqf/worried_about_someone_who_may_be_suicidal_heres/> has some advice on what to look out for, and how to determine how lethal their plan is, if a plan exists.\n" +
			                 "If you are concerned about their current state, but don't know how you can help, <https://www.reddit.com/r/SWResources/comments/igh87/concerned_but_dont_know_what_to_say_here_are_some/> can help guide you through talking to them. Remember that there is no \"correct\" way to address these issues, and being there for them and listening to what they have to say can make a big difference.");

            EmbedBuilder eb = new EmbedBuilder()
                .WithTitle("Alert! The hotlines command has been issed.")
                .WithDescription("Issued by: " + Context.User.Mention + "\nIssued in: <#" + Context.Channel.Id + ">")
                .WithCurrentTimestamp()
                .WithColor(242, 21, 21);

            await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("", false, eb.Build());
        }

        [Command("prefix"), Summary("Sends information on what prefixes are available to the user.")]
        public async Task PrefixInfo()
        {
            var userPrefix = User.Load(Context.User.Id).CustomPrefix;
            var guildPrefix = GuildConfiguration.Load(Context.Guild.Id).Prefix;

            EmbedBuilder eb = new EmbedBuilder()
                .WithAuthor(new EmbedAuthorBuilder().WithName("Available Prefixes for " + Context.User.Username).WithIconUrl(Context.User.GetAvatarUrl()))
                .WithDescription("Here are the prefixes you can use with " + DiscordBot.Bot.CurrentUser.Username)
                .AddField("Key", "[Prefix][Command]")
                .WithColor(Context.User.GetCustomRGB());

            if (!string.IsNullOrEmpty(userPrefix))
            {
                eb.AddField("User Set Prefix", userPrefix);
            }

            eb.AddField("Guild Prefix", guildPrefix);
            eb.AddField("Global Prefix", DiscordBot.Bot.CurrentUser.Mention);

            eb.WithFooter(new EmbedFooterBuilder().WithText("Interested in a custom prefix or changing your current prefix? " + guildPrefix + "editprofile customprefix"));

            await ReplyAsync("", false, eb.Build());
        }

        [Group("mogiicraft")]
        [RequireGuild(221250721046069249)]
        public class MogiiCraftCommands : ModuleBase
        { 
            [Command("poll"), Summary("Sends a link to the poll for the minecraft server.")]
            public async Task SendPollLink()
            {
                await ReplyAsync("https://docs.google.com/forms/d/e/1FAIpQLSe9CFsWWBlInGqgVqt4SieG6JW1E81zAjtWZeEoDUTH3xPE1w/viewform?c=0&w=1");
            }

            [Command("website"), Summary("Sends a link to the forums.")]
            public async Task SendWebsiteLink()
            {
                await ReplyAsync("We currently have a forums, but it isn't active. Anyways, you can visit it here: http://mogiicraft.proboards.com/");
            }

            [Command("minecraftip"), Summary("Posts the Minecraft IP into the chat.")]
            [Alias("ip")]
            public async Task SendMinecraftIp()
            {
                await ReplyAsync("The Minecraft Server IP is: mogiicraft.ddns.net:25635");
            }

            [Command("email"), Summary("Posts the email address into the chat.")]
            public async Task PostEmailAddress()
            {
                await ReplyAsync("Send complaints to `MogiiCraft.@pizza@gmail.com`");
            }

            [Command("vote"), Summary("Sends links to the voting websites for Minecraft.")]
            public async Task SendVotingLinks()
            {
                StringBuilder sb = new StringBuilder()
                    .Append("**Vote Links**\n" + Context.User.Mention + " use the following links to vote and support the server. You'll be given some diamonds in-game to say thanks :D\n");

                foreach (var link in VoteLinkHandler.VoteLinkList)
                {
                    sb.Append("<" + link + ">\n");
                }

                sb.Append("");

                await ReplyAsync(sb.ToString());
            }
        }
    }
}
