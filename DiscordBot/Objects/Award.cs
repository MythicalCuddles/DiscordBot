using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using DiscordBot.Database;
using DiscordBot.Exceptions;
using DiscordBot.Extensions;
using MySql.Data.MySqlClient;

namespace DiscordBot.Objects
{
    public class Award
    {
        private int _awardId;
        private ulong _userId;
        private string _awardText, _awardCategory;
        private DateTime _dateAwarded;
        
        internal static List<Award> Awards = new List<Award>();

        public Award() { }
        
        public Award(int awardId, ulong userId, string awardCategory, string awardText, DateTime dateAwarded)
        {
            _awardId = awardId;
            _userId = userId;
            _awardCategory = awardCategory;
            _awardText = awardText;
            _dateAwarded = dateAwarded;
        }

        public int AwardId
        {
            get => _awardId;
            set => _awardId = value;
        }

        public ulong UserId
        {
            get => _userId;
            set => _userId = value;
        }

        public string AwardCategory
        {
            get => _awardCategory;
            set => _awardCategory = value;
        }

        public string AwardText
        {
            get => _awardText;
            set => _awardText = value;
        }

        public DateTime DateAwarded
        {
            get => _dateAwarded;
            set => _dateAwarded = value;
        }

        internal static List<Award> LoadAll()
        {
            List<Award> awards = new List<Award>();

            (MySqlDataReader dr, MySqlConnection conn) reader = DatabaseActivity.ExecuteReader("SELECT * FROM awards;");

            while (reader.dr.Read())
            {
                var a = new Award
                {
                    _awardId = reader.dr.GetInt32("awardID"),
                    _userId = reader.dr.GetUInt64("userID"),
                    _awardCategory = reader.dr.GetString("awardCategory"),
                    _awardText = reader.dr.GetString("awardText"),
                    _dateAwarded = reader.dr.GetDateTime("dateAwarded")
                };
                
                awards.Add(a);
            }
            
            reader.dr.Close();
            reader.conn.Close();

            return awards;
        }

        public static Award GetAward(int id)
        {
            foreach (var award in Awards.Where(award => award.AwardId == id))
            {
                return award;
            }

            throw new AwardNotFoundException("Unable to find Award with ID " + id);
        }

        private static async Task ReloadAll()
        {
            try
            {
                Awards.Clear();
                Awards = LoadAll();
            }
            catch (Exception e)
            {
                await new LogMessage(LogSeverity.Critical, "Award Manager", e.Message).PrintToConsole();
            }
        }

        internal static void AddAward(ulong userId, string awardCategory, string awardText)
        {
            List<(string, string)> queryParams = new List<(string id, string value)>()
            {
                ("@id", userId.ToString()),
                ("@aCat", awardCategory),
                ("@aText", awardText)
            };

            DatabaseActivity.ExecuteNonQueryCommand(
                "INSERT IGNORE INTO " +
                "awards(userId, awardText, awardCategory, dateAwarded) " +
                "VALUES(@id, @aText, @aCat, CURRENT_TIMESTAMP);", queryParams);
            
            ReloadAll();
        }

        internal static bool DeleteAward(int id)
        {
            Awards.Remove(Awards.Find(award => award._awardId == id));

            var rowsAffected = DatabaseActivity.ExecuteNonQueryCommand(
                "DELETE FROM awards WHERE awardID=" + id + ";");

            return rowsAffected == 1;
        }
    }
}