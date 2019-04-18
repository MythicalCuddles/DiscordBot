using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;

using DiscordBot.Common;
using DiscordBot.Database;
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
			else if (channel is ICategoryChannel categoryChannelParam)
			{
				EmbedBuilder eb = new EmbedBuilder()
				{
					Title = "New Category Channel",
					Description = categoryChannelParam.Name,
					Color = new Color(0x52cf35)
				}
				.AddField("Channel ID", categoryChannelParam.Id)
				.AddField("Channel Name", categoryChannelParam.Name)
				.AddField("Guild ID", categoryChannelParam.GuildId)
				.AddField("Guild Name", categoryChannelParam.Guild.Name);

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
				return; // return prevents private messages from being entered into the database.
			}
            else 
			{
				await new LogMessage(LogSeverity.Warning, "ChannelHandler", channel.Id + " type is unknown.").PrintToConsole();
				return; // return prevents unknown channels from being entered into the database.
			}
			
			SocketGuildChannel gChannel = channel as SocketGuildChannel;
			await InsertChannelToDB(gChannel);
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
			else if (channel is ICategoryChannel categoryChannelParam)
			{
				EmbedBuilder eb = new EmbedBuilder()
					{
						Title = "Removed Category Channel",
						Description = categoryChannelParam.Name,
						Color = new Color(0xff003c)
					}
					.AddField("Channel ID", categoryChannelParam.Id)
					.AddField("Channel Name", categoryChannelParam.Name)
					.AddField("Guild ID", categoryChannelParam.GuildId)
					.AddField("Guild Name", categoryChannelParam.Guild.Name);

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
				return;
			}
			
			SocketGuildChannel gChannel = channel as SocketGuildChannel;
			await RemoveChannelFromDB(gChannel);
		}

		public static async Task ChannelUpdated(SocketChannel arg1, SocketChannel arg2)
		{
			SocketGuildChannel gChannel = arg2 as SocketGuildChannel;
			await UpdateChannelInDB(gChannel);
		}
		
		
		public static async Task InsertChannelToDB(SocketGuildChannel c)
		{
			List<(string, string)> queryParams = new List<(string id, string value)>()
			{
				("@channelName", c.Name),
				("@channelType", c.GetType().Name)
			};
				    
			DatabaseActivity.ExecuteNonQueryCommand(
				"INSERT IGNORE INTO " +
				"channels(channelID,inGuildID,channelName,channelType) " +
				"VALUES (" + c.Id + ", " + c.Guild.Id + ", @channelName, @channelType);", queryParams);
		}

		public static async Task RemoveChannelFromDB(SocketGuildChannel c)
		{
			DatabaseActivity.ExecuteNonQueryCommand(
				"DELETE FROM channels WHERE channelID=" + c.Id + ";");
		}

		private static async Task UpdateChannelInDB(SocketGuildChannel updatedChannel)
		{
			List<(string, string)> queryParams = new List<(string id, string value)>()
			{
				("@channelID", updatedChannel.Id.ToString()),
				("@channelName", updatedChannel.Name),
				("@channelType", updatedChannel.GetType().Name)
			};

			DatabaseActivity.ExecuteNonQueryCommand(
				"UPDATE guilds SET channelName=@channelName, channelType=@channelType WHERE channelID=@channelID",
				queryParams);
		}
	}
}
