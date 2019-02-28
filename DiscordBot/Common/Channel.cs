//using System;
//using System.IO;
//using Discord;
//using DiscordBot.Extensions;
//using Newtonsoft.Json;
//
//namespace DiscordBot.Common
//{
//    public class Channel
//    {
//        [JsonIgnore]
//        private static string DirectoryPath { get; } = "MythicalCuddles/DiscordBot/channels/";
//        [JsonIgnore]
//        private static string Extension { get; } = ".json";
//
//        public bool AwardingEXP { get; set; } = true;
//
//        public static void EnsureExists(ulong cId)
//        {
//            string fileName = DirectoryPath + cId + Extension;
//            string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), fileName);
//            if (!File.Exists(file))
//            {
//                string path = Path.GetDirectoryName(file);
//                if (!Directory.Exists(path))
//                    Directory.CreateDirectory(path);
//
//                var channel = new Channel();
//                channel.SaveJson(cId);
//                
//                new LogMessage(LogSeverity.Info, "Channel Files", fileName + " created.").PrintToConsole();
//            }
//
//            new LogMessage(LogSeverity.Info, "Channel Files", fileName + " loaded.").PrintToConsole();
//        }
//
//        private void SaveJson(ulong cId)
//        {
//            string fileName = DirectoryPath + cId + Extension;
//            string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), fileName);
//            File.WriteAllText(file, ToJson());
//        }
//
//        public static Channel Load(ulong cId)
//        {
//            string fileName = DirectoryPath + cId + Extension;
//            string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), fileName);
//            return JsonConvert.DeserializeObject<Channel>(File.ReadAllText(file));
//        }
//
//        private string ToJson()
//            => JsonConvert.SerializeObject(this, Formatting.Indented);
//
//        public static void UpdateChannel(ulong cId, bool? awardingEXP)
//        {
//            var channel = new Channel()
//            {
//                AwardingEXP = awardingEXP ?? Load(cId).AwardingEXP
//            };
//            channel.SaveJson(cId);
//        }
//    }
//}
