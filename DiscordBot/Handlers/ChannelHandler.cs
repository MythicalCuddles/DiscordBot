using System;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;

using DiscordBot.Common;
using DiscordBot.Extensions;

namespace DiscordBot.Handlers
{
	public class ChannelHandler
	{
		public static async Task ChannelCreated(SocketChannel channel)
		{

			if (channel is ITextChannel textChannelParam)
			{
			    EmbedBuilder eb = new EmbedBuilder()
			    {
                    Title = "New Text Channel",
                    Description = textChannelParam.Mention,
                    Color = new Color(0x52cf35)
			    }
			    .AddField("Channel ID", textChannelParam.Id)
			    .AddField("Channel Name", textChannelParam.Name)
			    .AddField("Channel Topic", textChannelParam.Topic)
                .AddField("Guild ID", textChannelParam.GuildId)
			    .AddField("Guild Name", textChannelParam.Guild.Name);

                await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("", false, eb.Build());
            }
			else if (channel is IVoiceChannel voiceChannelParam)
			{
			    EmbedBuilder eb = new EmbedBuilder()
			    {
			        Title = "New Voice Channel",
			        Description = voiceChannelParam.Name,
			        Color = new Color(0x52cf35)
			    }
			    .AddField("Channel ID", voiceChannelParam.Id)
			    .AddField("Channel Name", voiceChannelParam.Name)
			    .AddField("User Limit", voiceChannelParam.UserLimit)
			    .AddField("Guild ID", voiceChannelParam.GuildId)
			    .AddField("Guild Name", voiceChannelParam.Guild.Name);

			    await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("", false, eb.Build());
			}
            else if (channel is IPrivateChannel privateChannelParam)
			{
			    EmbedBuilder eb = new EmbedBuilder()
			    {
			        Title = "New Private Channel",
			        Description = privateChannelParam.Name,
			        Color = new Color(0x52cf35)
			    }
			    .AddField("Channel ID", privateChannelParam.Id)
			    .AddField("Channel Name", privateChannelParam.Name);

			    foreach (var u in privateChannelParam.Recipients)
			    {
			        eb.AddField("Recipient", u.Mention, true);
			    }

			    await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("", false, eb.Build());
			    return;
			}
            else 
			{
				await new LogMessage(LogSeverity.Warning, "ChannelHandler", channel.Id + " type is unknown.").PrintToConsole();
			}

            Channel.EnsureExists(channel.Id);
		}

		public static async Task ChannelDestroyed(SocketChannel channel)
		{
			if (channel is ITextChannel textChannelParam)
			{
			    EmbedBuilder eb = new EmbedBuilder()
			    {
			        Title = "Removed Text Channel",
			        Description = textChannelParam.Mention,
			        Color = new Color(0xff003c)
                }
			    .AddField("Channel ID", textChannelParam.Id)
			    .AddField("Channel Name", textChannelParam.Name)
			    .AddField("Channel Topic", textChannelParam.Topic)
			    .AddField("Guild ID", textChannelParam.GuildId)
			    .AddField("Guild Name", textChannelParam.Guild.Name);

			    await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("", false, eb.Build());
            }
			else if (channel is IVoiceChannel voiceChannelParam)
			{
			    EmbedBuilder eb = new EmbedBuilder()
			    {
			        Title = "Removed Text Channel",
			        Description = voiceChannelParam.Name,
			        Color = new Color(0xff003c)
			    }
			    .AddField("Channel ID", voiceChannelParam.Id)
			    .AddField("Channel Name", voiceChannelParam.Name)
			    .AddField("User Limit", voiceChannelParam.UserLimit)
			    .AddField("Guild ID", voiceChannelParam.GuildId)
			    .AddField("Guild Name", voiceChannelParam.Guild.Name);

			    await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("", false, eb.Build());
			}
			else if (channel is IPrivateChannel privateChannelParam)
			{
			    EmbedBuilder eb = new EmbedBuilder()
			        {
			            Title = "Removed Private Channel",
			            Description = privateChannelParam.Name,
			            Color = new Color(0x52cf35)
			        }
			        .AddField("Channel ID", privateChannelParam.Id)
			        .AddField("Channel Name", privateChannelParam.Name);

			    foreach (var u in privateChannelParam.Recipients)
			    {
			        eb.AddField("Recipient", u.Mention, true);
			    }

			    await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("", false, eb.Build());
			}
            else
			{
				await new LogMessage(LogSeverity.Critical, "UserExtensions", channel.Id + " type is unknown.").PrintToConsole();
			}
		}
	}
}
