using System;
using System.IO;

using Newtonsoft.Json;

namespace DiscordBot.Common
{
    public class User
    {
        [JsonIgnore]
        private static string DirectoryPath { get; } = "MythicalCuddles/DiscordBot/users/";
        [JsonIgnore]
        private static string Extension { get; } = ".json";

        public int Coins { get; set; } = 0;
        public int MythicalTokens { get; set; } = 0;
        
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

                Console.Write(@"status: [");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write(@"alert");
                Console.ResetColor();
                Console.WriteLine(@"]    " + fileName + @": created.");
                return true;
            }
            else
            {
                Console.Write(@"status: [");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write(@"ok");
                Console.ResetColor();
                Console.WriteLine(@"]    " + fileName + @": already exists.");
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

        internal static bool SetCoinsForAll(int newValue = 0)
        {
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), DirectoryPath);
            DirectoryInfo d = new DirectoryInfo(filePath);

            Console.WriteLine(@"-----------------------------------------------------------------");
            Console.WriteLine(@"[WARNING] A command was issued resetting the coins for all users.");

            foreach (var file in d.GetFiles("*.json"))
            {
                string[] fileName = file.ToString().Split('.');

                Console.WriteLine(@"[Info] " + file + @" - " + Load(Convert.ToUInt64(fileName[0])).Coins + @" coins has been set to " + newValue + @"!");
                UpdateUser(Convert.ToUInt64(fileName[0]), newValue);
            }

            Console.WriteLine(@"-----------------------------------------------------------------");
            Console.Write(@"status: [");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write(@"ok");
            Console.ResetColor();
            Console.WriteLine(@"]  " + @"Coin Reset" + @": reset completed.");
            Console.WriteLine(@"-----------------------------------------------------------------");

            return true;
        }
        
        public static void UpdateUser(ulong uId, int? coins = null, string name = null, string gender = null, string pronouns = null,
            string about = null, string customPrefix = null,
            byte? aboutR = null, byte? aboutG = null, byte? aboutB = null, bool? teamMember = null,
            string embedAuthorBuilderIconUrl = null, string embedFooterBuilderIconUrl = null,
            string footerText = null, string minecraftUsername = null, string snapchat = null, bool? isBotIgnoringUser = null, 
            string websiteName = null, string websiteUrl = null, int? mythicalTokens = null)
        {
            var user = new User()
            {
                Coins = coins ?? Load(uId).Coins,
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
                IsBotIgnoringUser = isBotIgnoringUser ?? Load(uId).IsBotIgnoringUser,
                WebsiteName = websiteName ?? Load(uId).WebsiteName,
                WebsiteUrl = websiteUrl ?? Load(uId).WebsiteUrl,
                MythicalTokens = mythicalTokens ?? Load(uId).MythicalTokens
            };
            user.SaveJson(uId);
        }
    }
}
