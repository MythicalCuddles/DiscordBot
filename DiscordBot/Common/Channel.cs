using System;
using System.IO;

using Newtonsoft.Json;

namespace DiscordBot.Common
{
    public class Channel
    {
        [JsonIgnore]
        private static string DirectoryPath { get; } = "MythicalCuddles/DiscordBot/channels/";
        [JsonIgnore]
        private static string Extension { get; } = ".json";

        public bool AwardingCoins { get; set; } = true;

        public static void EnsureExists(ulong cId)
        {
            string fileName = DirectoryPath + cId + Extension;
            string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), fileName);
            if (!File.Exists(file))
            {
                string path = Path.GetDirectoryName(file);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var channel = new Channel();
                channel.SaveJson(cId);

                Console.Write(@"status: [");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write(@"ok");
                Console.ResetColor();
                Console.WriteLine(@"]    " + fileName + @": created.");
            }

            Console.Write(@"status: [");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write(@"ok");
            Console.ResetColor();
            Console.WriteLine(@"]    " + fileName + @": loaded.");
        }

        private void SaveJson(ulong cId)
        {
            string fileName = DirectoryPath + cId + Extension;
            string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), fileName);
            File.WriteAllText(file, ToJson());
        }

        public static Channel Load(ulong cId)
        {
            string fileName = DirectoryPath + cId + Extension;
            string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), fileName);
            return JsonConvert.DeserializeObject<Channel>(File.ReadAllText(file));
        }

        private string ToJson()
            => JsonConvert.SerializeObject(this, Formatting.Indented);

        //public static void SetAwardingCoins(ulong cId, bool value)
        //{
        //    string fileName = DirectoryPath + cId + Extension;
        //    string json = File.ReadAllText(fileName);

        //    dynamic jsonObj = JsonConvert.DeserializeObject(json);
        //    jsonObj["AwardingCoins"] = value;
        //    string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
        //    File.WriteAllText(fileName, output);
        //}

        public static void UpdateChannel(ulong cId, bool? awardingCoins)
        {
            var channel = new Channel()
            {
                AwardingCoins = awardingCoins ?? Load(cId).AwardingCoins
            };
            channel.SaveJson(cId);
        }
    }
}
