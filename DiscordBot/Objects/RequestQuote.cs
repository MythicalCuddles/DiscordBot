using System;
using System.Collections.Generic;
using DiscordBot.Database;
using MySql.Data.MySqlClient;

namespace DiscordBot.Objects
{
    public class RequestQuote
    {
        internal static List<RequestQuote> RequestQuotes;
        
        internal ulong CreatedBy, CreatedIn;
        internal DateTime QCreatedTimestamp;
        internal string QuoteText;

        internal int RequestId;

        private RequestQuote()
        {
        }

        internal RequestQuote(int requestId, ulong createdBy, ulong createdIn, string quoteText,
            DateTime createdTimestamp)
        {
            RequestId = requestId;
            CreatedBy = createdBy;
            QuoteText = quoteText;
            QCreatedTimestamp = createdTimestamp;
            CreatedIn = createdIn;
        }

        internal static List<RequestQuote> LoadAll()
        {
            var requests = new List<RequestQuote>();
            (MySqlDataReader dr, MySqlConnection conn) reader =
                DatabaseActivity.ExecuteReader("SELECT * FROM requested_quotes;");

            while (reader.dr.Read())
            {
                var q = new RequestQuote
                {
                    RequestId = reader.dr.GetInt32("requestQuoteId"),
                    CreatedBy = reader.dr.GetUInt64("requestedBy"),
                    QuoteText = reader.dr.GetString("quoteText"),
                    QCreatedTimestamp = reader.dr.GetDateTime("dateCreated"),
                    CreatedIn = reader.dr.GetUInt64("requestedIn")
                };

                requests.Add(q);
            }

            reader.dr.Close();
            reader.conn.Close();

            return requests;
        }

        internal static bool AddRequestQuote(string quote, ulong creatorId, ulong createdIn)
        {
            List<(string, string)> queryParams = new List<(string id, string value)>
            {
                ("@requestedBy", creatorId.ToString()),
                ("@quoteText", quote),
                ("@createdIn", createdIn.ToString())
            };

            var rowsUpdated = DatabaseActivity.ExecuteNonQueryCommand(
                "INSERT IGNORE INTO " +
                "requested_quotes(requestQuoteId, requestedBy, quoteText, dateCreated, requestedIn) " +
                "VALUES(NULL, @requestedBy, @quoteText, CURRENT_TIMESTAMP, @createdIn);", queryParams);

            (MySqlDataReader dr, MySqlConnection conn) reader =
                DatabaseActivity.ExecuteReader("SELECT * FROM requested_quotes ORDER BY requestQuoteId DESC LIMIT 1");
            
            var id = 0;
            while (reader.dr.Read())
            {
                id = reader.dr.GetInt32("requestQuoteId");
            }
            
            reader.dr.Close();
            reader.conn.Close();

            RequestQuotes.Add(new RequestQuote
            {
                RequestId = id,
                CreatedBy = creatorId,
                QuoteText = quote,
                QCreatedTimestamp = DateTime.Now,
                CreatedIn = createdIn
            });

            return rowsUpdated == 1;
        }

        internal static bool ApproveRequestQuote(int quoteId, ulong approvedBy, ulong approvedIn)
        {
            (MySqlDataReader dr, MySqlConnection conn) reader =
                DatabaseActivity.ExecuteReader("SELECT * FROM requested_quotes WHERE requestQuoteId = " + quoteId +
                                               " LIMIT 1");
            var quote = new RequestQuote();
            while (reader.dr.Read())
            {
                quote.RequestId = reader.dr.GetInt32("requestQuoteId");
                quote.CreatedBy = reader.dr.GetUInt64("requestedBy");
                quote.QuoteText = reader.dr.GetString("quoteText");
                quote.QCreatedTimestamp = reader.dr.GetDateTime("dateCreated");
                quote.CreatedIn = reader.dr.GetUInt64("requestedIn");
            }

            reader.dr.Close();
            reader.conn.Close();

            var added = Quote.AddQuote(quote.QuoteText, quote.CreatedBy, quote.CreatedIn, approvedBy, approvedIn,
                quote.QCreatedTimestamp);
            RemoveRequestQuote(quote.RequestId);

            return added;
        }

        internal static bool RemoveRequestQuote(int quoteId)
        {
            RequestQuotes.Remove(RequestQuotes.Find(quote => quote.RequestId == quoteId));

            var rowsAffected = DatabaseActivity.ExecuteNonQueryCommand(
                "DELETE FROM requested_quotes WHERE requestQuoteId=" + quoteId + ";");

            return rowsAffected == 1;
        }
    }
}