using System;

namespace DiscordBot.Objects
{
    public class Award
    {
        private ulong awardId;
        private ulong userId;
        private string awardText;
        private DateTime dateAwarded;

        public Award() { }

        public Award(ulong awardId, ulong userId, string awardText)
        {
            this.awardId = awardId;
            this.userId = userId;
            this.awardText = awardText;
        }
        
        public Award(ulong awardId, ulong userId, string awardText, DateTime dateAwarded)
        {
            this.awardId = awardId;
            this.userId = userId;
            this.awardText = awardText;
            this.dateAwarded = dateAwarded;
        }

        public ulong AwardId
        {
            get => awardId;
            set => awardId = value;
        }

        public ulong UserId
        {
            get => userId;
            set => userId = value;
        }

        public string AwardText
        {
            get => awardText;
            set => awardText = value;
        }

        public DateTime DateAwarded
        {
            get => dateAwarded;
            set => dateAwarded = value;
        }
    }
}