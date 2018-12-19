using System;

namespace DiscordBot.Objects
{
    public class Award
    {
        public ulong awardId;
        public ulong userId;
        public string awardText;
        public DateTime dateAwarded;

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
    }
}