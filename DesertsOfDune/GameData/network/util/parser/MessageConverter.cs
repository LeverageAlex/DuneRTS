using System;
using GameData.network.messages;
using Newtonsoft.Json;

namespace GameData.network.util.parser
{
    /// <summary>
    /// This class is responsible for converting Message Objects to Json and vice versa.
    /// </summary>
    public static class MessageConverter
    {
        /// <summary>
        /// This method converts a Message Object to a Json string
        /// </summary>
        /// <param name="message">the message object</param>
        /// <returns>the json string</returns>
        static public String FromMessage(Message message)
        {
            // test output
            string data;

            if (message.getMessageType().Equals("END_TURN_REQUEST"))
            {
                EndTurnRequestMessage debugMessage = (EndTurnRequestMessage)message;
                data = JsonConvert.SerializeObject(debugMessage);
            } else
            {
                data = JsonConvert.SerializeObject(message, new JsonSerializerSettings());
            }
            
            return data;
        }

        /// <summary>
        /// This method converts a json String to a Message object
        /// </summary>
        /// <param name="message">the json string to be converted</param>
        /// <returns>the Message object to be created.</returns>
        static public Message ToMessage(String message)
        {
            
            EndTurnRequestMessage m = JsonConvert.DeserializeObject<EndTurnRequestMessage>(message);
            Console.WriteLine(m.ToString());
            return m;
        }
    }
}
