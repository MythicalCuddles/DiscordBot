using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DiscordBot.Other
{
    public class VoteLinkHandler
    {
        private const string FileName = "MythicalCuddles/DiscordBot/common/voteLinks.txt";
        
        public static List<string> VoteLinkList = new List<string>();

        private static void LoadQuotes()
        {
            string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), FileName);
            VoteLinkList = File.ReadAllLines(file).ToList();

            Console.Write(@"status: [");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write(@"ok");
            Console.ResetColor();
            Console.WriteLine(@"]    " + FileName + @": loaded.");
        }

        public static void EnsureExists()
        {
            string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), FileName);
            if (!File.Exists(file))
            {
                string path = Path.GetDirectoryName(file);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                SaveLinks();

                Console.Write(@"status: [");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write(@"ok");
                Console.ResetColor();
                Console.WriteLine(@"]    " + FileName + @": created.");
            }
            LoadQuotes();
        }

        private static void SaveLinks()
        {
            string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), FileName);
            File.WriteAllLines(file, VoteLinkList);
        }

        public static void AddAndUpdateLinks(string link)
        {
            VoteLinkList.Add(link);
            SaveLinks();
        }

        public static void RemoveAndUpdateLinks(int index)
        {
            VoteLinkList.RemoveAt(index);
            SaveLinks();
        }

        public static void UpdateLink(int index, string link)
        {
            VoteLinkList[index] = link;
            SaveLinks();
        }
    }
}
