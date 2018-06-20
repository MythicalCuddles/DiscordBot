using System;
using System.IO;

using Newtonsoft.Json;

namespace DiscordBot.Common
{
    public class StringConfiguration
    {
        [JsonIgnore]
        private static string FileName { get; } = "MythicalCuddles/DiscordBot/config/strings.json";
        [JsonIgnore]
        private static readonly string File = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), FileName);

        // VARIABLES
        public string DefaultWebsiteName = "Personal Website";

        // END

        public static void EnsureExists()
        {
            if (!System.IO.File.Exists(File))
            {
                string path = Path.GetDirectoryName(File);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var stringConfig = new StringConfiguration();
                stringConfig.SaveJson();

                Console.Write(@"status: [");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write(@"ok");
                Console.ResetColor();
                Console.WriteLine(@"]    " + FileName + @": created.");
            }

            Console.Write(@"status: [");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write(@"ok");
            Console.ResetColor();
            Console.WriteLine(@"]    " + FileName + @": loaded.");
        }

        public void SaveJson()
        {
            System.IO.File.WriteAllText(File, ToJson());
        }

        public static StringConfiguration Load()
        {
            return JsonConvert.DeserializeObject<StringConfiguration>(System.IO.File.ReadAllText(File));
        }

        public string ToJson()
            => JsonConvert.SerializeObject(this, Formatting.Indented);

        public static void UpdateConfiguration(string websiteName = null)
        {
            var stringConfig = new StringConfiguration()
            {
                DefaultWebsiteName = websiteName ?? Load().DefaultWebsiteName,
            };
            stringConfig.SaveJson();
        }
    }
}
