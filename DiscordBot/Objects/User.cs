using System;
using System.Threading.Tasks;
using DiscordBot.Database;
using MySql.Data.MySqlClient;

namespace DiscordBot.Objects
{
    public class User
    {
        public int Level { get; set; } = 0;
        public int EXP { get; set; } = 0;
        
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Pronouns { get; set; }
        public string About { get; set; }
        
        public string CustomPrefix { get; set; }
        
        public byte AboutR { get; set; } = 140;
        public byte AboutG { get; set; } = 90;
        public byte AboutB { get; set; } = 210;

        public bool TeamMember { get; set; } = false;
        public string EmbedAuthorBuilderIconUrl { get; set; }
        public string EmbedFooterBuilderIconUrl { get; set; }
        public string FooterText { get; set; }

        public string MinecraftUsername { get; set; }
        public string SnapchatUsername { get; set; }
        public string InstagramUsername { get; set; }
        public string GitHubUsername { get; set; }
        public string PokemonGoFriendCode { get; set; } 
        public string WebsiteName { get; set; }
        public string WebsiteUrl { get; set; }

        public bool IsBotIgnoringUser { get; set; } = false;
        
        public User(int? level = null,
            int? exp = null,
            string name = null,
            string gender = null,
            string pronouns = null,
            string about = null,
            string customPrefix = null,
            byte? aboutR = null,
            byte? aboutG = null,
            byte? aboutB = null,
            bool? teamMember = null,
            string embedAuthorBuilderIconUrl = null,
            string embedFooterBuilderIconUrl = null,
            string footerText = null,
            string minecraftUsername = null,
            string snapchatUsername = null,
            string instagramUsername = null,
            string gitHubUsername = null,
            string pokemonGoFriendCode = null,
            string websiteName = null,
            string websiteUrl = null,
            bool? isBotIgnoring = null)
        {
            Level = level ?? Level;
            EXP = exp ?? EXP;
            Name = name;
            Gender = gender;
            Pronouns = pronouns;
            About = about;
            CustomPrefix = customPrefix;
            AboutR = aboutR ?? AboutR;
            AboutG = aboutG ?? AboutG;
            AboutB = aboutB ?? AboutB;
            TeamMember = teamMember ?? TeamMember;
            EmbedAuthorBuilderIconUrl = embedAuthorBuilderIconUrl;
            EmbedFooterBuilderIconUrl = embedFooterBuilderIconUrl;
            FooterText = footerText;
            MinecraftUsername = minecraftUsername;
            SnapchatUsername = snapchatUsername;
            InstagramUsername = instagramUsername;
            GitHubUsername = gitHubUsername;
            PokemonGoFriendCode = pokemonGoFriendCode;
            WebsiteName = websiteName;
            WebsiteUrl = websiteUrl;
            IsBotIgnoringUser = isBotIgnoring ?? IsBotIgnoringUser;
        }
        
        public static User Load(ulong uId)
        {
            User user = new User();
                
            (MySqlDataReader dr, MySqlConnection conn) reader = DatabaseActivity.ExecuteReader("SELECT * FROM users WHERE id=" + uId + ";");
            
            while (reader.dr.Read())
            {
                user.Level = reader.dr.GetInt32("level");
                user.EXP = reader.dr.GetInt32("exp");
                user.Name = reader.dr["name"].ToString();
                user.Gender = reader.dr["gender"].ToString();
                user.Pronouns = reader.dr["pronouns"].ToString();
                user.About = reader.dr["about"].ToString();
                user.CustomPrefix = reader.dr["customPrefix"].ToString();
                user.AboutR = reader.dr.GetByte("aboutR");
                user.AboutG = reader.dr.GetByte("aboutG");
                user.AboutB = reader.dr.GetByte("aboutB");

                if (reader.dr["teamMember"].ToString().ToUpper() == "Y")
                {
                    user.TeamMember = true;
                }

                user.EmbedAuthorBuilderIconUrl = reader.dr["authorIconURL"].ToString();
                user.EmbedFooterBuilderIconUrl = reader.dr["footerIconURL"].ToString();
                user.FooterText = reader.dr["footerText"].ToString();
                user.MinecraftUsername = reader.dr["minecraftUsername"].ToString();
                user.SnapchatUsername = reader.dr["snapchatUsername"].ToString();
                user.InstagramUsername = reader.dr["instagramUsername"].ToString();
                user.GitHubUsername = reader.dr["githubUsername"].ToString();
                user.PokemonGoFriendCode = reader.dr["pokemonGoFriendCode"].ToString();
                user.WebsiteName = reader.dr["websiteName"].ToString();
                user.WebsiteUrl = reader.dr["websiteURL"].ToString();

                if (reader.dr["isBeingIgnored"].ToString().ToUpper() == "Y")
                {
                    user.IsBotIgnoringUser = true;
                }
            }
            
            reader.dr.Close();
            reader.conn.Close();
            
            return user;
        }
    }
}
