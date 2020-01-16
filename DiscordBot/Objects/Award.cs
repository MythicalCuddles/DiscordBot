using System;
using System.Collections.Generic;
using DiscordBot.Database;
using MySql.Data.MySqlClient;

namespace DiscordBot.Objects
{
    public class Award
    {
        private int _awardId;
        private ulong _userId;
        private string _awardText;//, _awardType;
        private DateTime _dateAwarded;
        
        internal static List<Award> Awards = new List<Award>();

        public Award() { }

        public Award(int awardId, ulong userId, string awardText)
        {
            _awardId = awardId;
            _userId = userId;
            _awardText = awardText;
        }
        
        public Award(int awardId, ulong userId, string awardText, DateTime dateAwarded)
        {
            _awardId = awardId;
            _userId = userId;
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
                    _awardText = reader.dr.GetString("awardText"),
                    _dateAwarded = reader.dr.GetDateTime("dateAwarded")
                };
                
                awards.Add(a);
            }
            
            reader.dr.Close();
            reader.conn.Close();

            return awards;
        }
    }
}