using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using DiscordBot.Extensions;

namespace DiscordBot.Logging
{
    public class TransactionLogger
    {
        private const string FileName = "MythicalCuddles/DiscordBot/common/transactions.txt";
        public static List<string> TransactionsList = new List<string>();
        public static List<List<string>> SplicedTransactionsList = new List<List<string>>();
        public static List<ulong> TransactionMessages = new List<ulong>();
        public static List<int> PageNumber = new List<int>();

        private static void LoadTransactions()
        {
            string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), FileName);
            TransactionsList = File.ReadAllLines(file).ToList();

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

                SaveTransactionsToFile();

                Console.Write(@"status: [");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write(@"ok");
                Console.ResetColor();
                Console.WriteLine(@"]    " + FileName + @": created.");
            }
            LoadTransactions();
        }

        private static void SaveTransactionsToFile()
        {
            string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), FileName);
            File.WriteAllLines(file, TransactionsList);
        }

        public static void AddTransaction(string transaction)
        {
            String timeStamp = DateTime.Now.GetTimestamp();

            TransactionsList.Add("[" + timeStamp + "] " + transaction);
            SaveTransactionsToFile();
        }

        public static void SpliceTransactionsIntoList()
        {
            SplicedTransactionsList = TransactionsList.SplitList();
        }
        public static List<string> GetSplicedTransactions(int pageNumber)
        {
            pageNumber = pageNumber - 1;
            return SplicedTransactionsList[pageNumber];
        }
        public static int GetSplicedTransactonListCount
        {
            get { return SplicedTransactionsList.Count(); }
        }
    }
}
