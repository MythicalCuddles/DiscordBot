using System;
using System.Runtime.Serialization;

namespace DiscordBot.Exceptions
{
    [Serializable]
    public class AwardNotFoundException : Exception
    {
        public AwardNotFoundException()
        {
        }

        public AwardNotFoundException(string message) : base(message)
        {
        }

        public AwardNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        protected AwardNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}