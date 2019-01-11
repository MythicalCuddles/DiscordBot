using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Common;
using DiscordBot.Common.Preconditions;
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
            
            
            string connectionString = String.Format("server={0};port={1};user id={2}; password={3}; database={4}", Configuration.Load().DatabaseHost, Configuration.Load().DatabasePort.ToString(), Configuration.Load().DatabaseUser, Configuration.Load().DatabasePassword, Configuration.Load().DatabaseName);
            MySqlConnection connection = new MySqlConnection(connectionString);
            List<Award> awards = new List<Award>();

            try
            {
                connection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "SELECT * FROM awards WHERE userId=" + userSpecified.Id + ";";
            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                awards.Add(new Award()
                {
                    awardId = (ulong) dr["awardId"],
                    userId = (ulong) dr["userId"],
                    awardText = dr["awardText"].ToString(),
                    dateAwarded = (DateTime)dr["dateAwarded"]
                });
            }

            connection.Close();
            
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