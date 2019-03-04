using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Common;
using DiscordBot.Common.Preconditions;
using DiscordBot.Database;
using DiscordBot.Extensions;
using DiscordBot.Objects;
using MySql.Data.MySqlClient;

namespace DiscordBot.Modules
{
    [RequireContext(ContextType.Guild)]
    [Name("Award Commands")]
    public class AwardModule : ModuleBase
    {
        [MinPermissions(PermissionLevel.User)]
        [Command("awards"), Summary("")]
        public async Task ShowUserAwards(IUser user = null)
        {
            var userSpecified = user as SocketGuildUser ?? Context.User as SocketGuildUser;

            if (userSpecified == null)
            {
                await ReplyAsync("User not found, please try again.");
                return;
            }
            
            EmbedAuthorBuilder eab = new EmbedAuthorBuilder();
            eab.IconUrl = Configuration.Load().AwardsIconUrl;
            eab.Name = userSpecified.Username + "'s Awards";

            EmbedBuilder eb = new EmbedBuilder()
                .WithAuthor(eab)
                .WithTitle("Award Name - Date Obtained")
                .WithThumbnailUrl(userSpecified.GetAvatarUrl())
                .WithColor(userSpecified.GetCustomRGB());
            
            List<Award> awards = new List<Award>();
            
            (MySqlDataReader dr, MySqlConnection conn) reader = DatabaseActivity.ExecuteReader("SELECT * FROM awards WHERE userId=" + userSpecified.Id + ";");
            
            while (reader.dr.Read())
            {
                awards.Add(new Award()
                {
                    awardId = (ulong) reader.dr["awardID"],
                    userId = (ulong) reader.dr["userID"],
                    awardText = reader.dr["awardText"].ToString(),
                    dateAwarded = (DateTime)reader.dr["dateAwarded"]
                });
            }
            
            reader.dr.Close();
            reader.conn.Close();
            
            awards.Sort((x, y) => x.dateAwarded.CompareTo(y.dateAwarded)); // newest awards will appear first
            //awards.Sort((x, y) => y.dateAwarded.CompareTo(x.dateAwarded)); // oldest awards will appear first
            
            StringBuilder sb = new StringBuilder();

            if (awards.Count == 0)
            {
                sb.Append("No awards found.");
            }
            else
            {
                DateTime date = DateTime.Now;
                bool showAllAwards = Configuration.Load().ShowAllAwards;
                
                foreach (Award a in awards)
                {
                    if (!showAllAwards)
                    {
                        int result = DateTime.Compare(date, a.dateAwarded);
                        // result : -1:Future Date, 1:Past Date, 1:Today
                        
                        if (result >= 0)
                        {
                            sb.Append(a.awardText + " - " + a.dateAwarded.ToString("dd/MM/yyyy") + "\n");
                        }
                    }
                    else
                    {
                        sb.Append(a.awardText + " - " + a.dateAwarded.ToString("dd/MM/yyyy") + "\n");
                    }
                }
            }

            eb.WithDescription(sb.ToString());
            await ReplyAsync("", false, eb.Build());
        }
    }
}