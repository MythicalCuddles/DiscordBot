using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using DiscordBot.Extensions;
using Newtonsoft.Json;

namespace DiscordBot.Common
{
    public class User
    {
        [JsonIgnore]
        private static string DirectoryPath { get; } = "MythicalCuddles/DiscordBot/users/";
        [JsonIgnore]
        private static string Extension { get; } = ".json";

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
        
        public bool TeamMember { get; set; }
        public string EmbedAuthorBuilderIconUrl { get; set; }
        public string EmbedFooterBuilderIconUrl { get; set; }
        public string FooterText { get; set; }

        /// Socials
        public string MinecraftUsername { get; set; }
        public string Snapchat { get; set; }
        public string InstagramUsername { get; set; }
        public string GitHubUsername { get; set; }
        public string WebsiteName { get; set; }
        public string WebsiteUrl { get; set; }

        public bool IsBotIgnoringUser { get; set; }


		public static bool CreateUserFile(ulong uId)
        {
            string fileName = DirectoryPath + uId + Extension;
            string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), fileName);
            if (!File.Exists(file))
            {
                string path = Path.GetDirectoryName(file);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var user = new User();
                user.SaveJson(uId);
                
                new LogMessage(LogSeverity.Info, "User Files", fileName + " created.").PrintToConsole();
                
                return true;
            }
            else
            {
                new LogMessage(LogSeverity.Info, "User Files", fileName + " loaded.").PrintToConsole();
                return false;
            }
        }

        private void SaveJson(ulong uId)
        {
            string fileName = DirectoryPath + uId + Extension;

            string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), fileName);
            File.WriteAllText(file, ToJson());
        }

        public static User Load(ulong uId)
        {
            string fileName = DirectoryPath + uId + Extension;

            string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), fileName);
            return JsonConvert.DeserializeObject<User>(File.ReadAllText(file));
        }

        private string ToJson()
            => JsonConvert.SerializeObject(this, Formatting.Indented);
        
        public static void UpdateUser(ulong uId, int? coins = null, int? mythicalTokens = null, int? level = null, int? exp = null,
            string name = null, string gender = null, string pronouns = null, string about = null, string customPrefix = null,
            byte? aboutR = null, byte? aboutG = null, byte? aboutB = null, 
            bool? teamMember = null, string embedAuthorBuilderIconUrl = null, string embedFooterBuilderIconUrl = null, string footerText = null, 
            string minecraftUsername = null, string snapchat = null, string instagram = null, string github = null, string websiteName = null, string websiteUrl = null,
            bool? isBotIgnoringUser = null)
        {
            var user = new User()
            {
                Level = level ?? Load(uId).Level,
                EXP = exp ?? Load(uId).EXP,
                
                Name = name ?? Load(uId).Name,
                Gender = gender ?? Load(uId).Gender,
                Pronouns = pronouns ?? Load(uId).Pronouns,
                About = about ?? Load(uId).About,
                CustomPrefix = customPrefix ?? Load(uId).CustomPrefix,
                
                AboutR = aboutR ?? Load(uId).AboutR,
                AboutG = aboutG ?? Load(uId).AboutG,
                AboutB = aboutB ?? Load(uId).AboutB,
                
                TeamMember = teamMember ?? Load(uId).TeamMember,
                EmbedAuthorBuilderIconUrl = embedAuthorBuilderIconUrl ?? Load(uId).EmbedAuthorBuilderIconUrl,
                EmbedFooterBuilderIconUrl = embedFooterBuilderIconUrl ?? Load(uId).EmbedFooterBuilderIconUrl,
                FooterText = footerText ?? Load(uId).FooterText,
                
                MinecraftUsername = minecraftUsername ?? Load(uId).MinecraftUsername,
                Snapchat = snapchat ?? Load(uId).Snapchat,
                InstagramUsername = instagram ?? Load(uId).InstagramUsername,
                GitHubUsername = github ?? Load(uId).GitHubUsername,
                WebsiteName = websiteName ?? Load(uId).WebsiteName,
                WebsiteUrl = websiteUrl ?? Load(uId).WebsiteUrl,
                
                IsBotIgnoringUser = isBotIgnoringUser ?? Load(uId).IsBotIgnoringUser,
            };
            user.SaveJson(uId);
        }
    }
}
