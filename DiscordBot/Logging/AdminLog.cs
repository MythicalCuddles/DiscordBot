using System.Collections.Generic;
using DiscordBot.Database;

namespace DiscordBot.Logging
{
    public class AdminLog
    {
        internal static void Log(ulong executedBy, string action, ulong executedIn, ulong? userMention = null)
        {
            List<(string, string)> queryParams = new List<(string id, string value)>
            {
                ("@executedBy", executedBy.ToString()),
                ("@action", action),
                ("@executedIn", executedIn.ToString())
            };

            queryParams.Add(userMention != null
                ? ("@mentionedUser", userMention.ToString())
                : ("@mentionedUser", null));

            DatabaseActivity.ExecuteNonQueryCommand(
                "INSERT IGNORE INTO " +
                "admin_log(logId,executedBy,action,executedAt,executedIn,userMentioned) " +
                "VALUES (NULL, @executedBy, @action, CURRENT_TIMESTAMP, @executedIn, @mentionedUser);", queryParams);
        }
    }
}