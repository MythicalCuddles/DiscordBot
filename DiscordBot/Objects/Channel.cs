using DiscordBot.Database;
using MySql.Data.MySqlClient;

namespace DiscordBot.Objects
{
    public class Channel
    {
        public bool AwardingEXP = true;
        
        public Channel(bool? awardingEXP = null)
        {
            AwardingEXP = awardingEXP ?? AwardingEXP;
        }
        
        public static Channel Load(ulong channelID)
        {
            Channel channel = new Channel();
                
            (MySqlDataReader dr, MySqlConnection conn) reader = DatabaseActivity.ExecuteReader("SELECT * FROM channels WHERE channelID=" + channelID + ";");
            
            while (reader.dr.Read())
            {
                if ((bool)reader.dr["awardingEXP"])
                    channel.AwardingEXP = true;
            }
            
            reader.dr.Close();
            reader.conn.Close();
            
            return channel;
        }
    }
}