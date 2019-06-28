using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Discord;
using DiscordBot.Extensions;

namespace DiscordBot.Other
{
    public static class QuoteHandler
    {
        private const string FileName = "MythicalCuddles/DiscordBot/common/quotes.txt";
        private const string RequestQuotesFileName = "MythicalCuddles/DiscordBot/common/requestquotes.txt";

        public static List<string> QuoteList = new List<string>();
        public static List<string> RequestQuoteList = new List<string>();

        private static List<List<string>> SplicedQuoteList = new List<List<string>>();
        private static List<List<string>> SplicedRequestQuoteList = new List<List<string>>();

        public static readonly List<ulong> QuoteMessages = new List<ulong>();
        public static List<ulong> RequestQuoteMessages = new List<ulong>();

        public static readonly List<int> PageNumber = new List<int>();
        public static List<int> RequestPageNumber = new List<int>();

        private static void LoadAllQuotes()
        {
            string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), FileName);
            QuoteList = File.ReadAllLines(file).ToList();

            new LogMessage(LogSeverity.Info, "QuoteHandler", FileName + " loaded.").PrintToConsole();
            
            file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), RequestQuotesFileName);
            RequestQuoteList = File.ReadAllLines(file).ToList();

            new LogMessage(LogSeverity.Info, "QuoteHandler", FileName + " loaded.").PrintToConsole();
        }

        public static void EnsureExists()
        {
            string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), FileName);
            if (!File.Exists(file))
            {
                string path = Path.GetDirectoryName(file);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                SaveQuotes();
                
                new LogMessage(LogSeverity.Info, "QuoteHandler", FileName + " created.").PrintToConsole();
            }
            
            file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), RequestQuotesFileName);
            if (!File.Exists(file))
            {
                string path = Path.GetDirectoryName(file);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                SaveRequestQuotes();
                
                new LogMessage(LogSeverity.Info, "QuoteHandler", FileName + " created.").PrintToConsole();
            }

            LoadAllQuotes();
        }

        private static void SaveQuotes()
        {
            string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), FileName);
            File.WriteAllLines(file, QuoteList);
        }

        private static void SaveRequestQuotes()
        {
            string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), RequestQuotesFileName);
            File.WriteAllLines(file, RequestQuoteList);
        }

        public static void AddAndUpdateQuotes(string quote)
        {
            QuoteList.Add(quote);
            SaveQuotes();
        }

        public static void AddAndUpdateRequestQuotes(string quote)
        {
            RequestQuoteList.Add(quote);
            SaveRequestQuotes();
        }

        public static void RemoveAndUpdateQuotes(int index)
        {
            QuoteList.RemoveAt(index);
            SaveQuotes();
        }

        public static void RemoveAndUpdateRequestQuotes(int index)
        {
            RequestQuoteList.RemoveAt(index);
            SaveRequestQuotes();
        }

        public static void UpdateQuote(int index, string quote)
        {
            QuoteList[index] = quote;
            SaveQuotes();
        }

        public static void SpliceQuotes()
        {
            SplicedQuoteList = QuoteList.SplitList();
        }
        public static List<string> GetQuotes(int pageNumber)
        {
            pageNumber = pageNumber - 1;
            return SplicedQuoteList[pageNumber];
        }
        public static int GetQuotesListLength => SplicedQuoteList.Count;

        public static void SpliceRequestQuotes()
        {
            SplicedRequestQuoteList = RequestQuoteList.SplitList();
        }
        public static List<string> GetRequestQuotes(int pageNumber)
        {
            pageNumber = pageNumber - 1;
            return SplicedRequestQuoteList[pageNumber];
        }
        public static int GetRequestQuotesListLength => SplicedRequestQuoteList.Count;
    }
}
