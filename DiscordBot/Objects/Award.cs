using System;

namespace DiscordBot.Objects
{
    public class Award
    {
        private ulong _awardId;
        private ulong _userId;
        private string _awardText;
        private DateTime _dateAwarded;

        public Award() { }

        public Award(ulong awardId, ulong userId, string awardText)
        {
            _awardId = awardId;
            _userId = userId;
            _awardText = awardText;
        }
        
        public Award(ulong awardId, ulong userId, string awardText, DateTime dateAwarded)
        {
            _awardId = awardId;
            _userId = userId;
            _awardText = awardText;
            _dateAwarded = dateAwarded;
        }

        public ulong AwardId
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
    }
}