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
            switch (message.getMessageType())
            {
                case "ACTION":
                    // TODO: ActionMessage right?
                    ActionMessage actionMessage = (ActionMessage)message;
                    return JsonConvert.SerializeObject(actionMessage);
                case "ACTION_REQUEST":
                    ActionRequestMessage actionRequestMessage = (ActionRequestMessage)message;
                    return JsonConvert.SerializeObject(actionRequestMessage);
                case "CHANGE_PLAYER_SPICE_DEMAND":
                    ChangePlayerSpiceDemandMessage changePlayerSpiceMessage = (ChangePlayerSpiceDemandMessage)message;
                    return JsonConvert.SerializeObject(changePlayerSpiceMessage);
                case "CHARACTER_STAT_CHANGE_DEMAND":
                    ChangeCharacterStatisticsDemandMessage characterStatisticsMessage = (ChangeCharacterStatisticsDemandMessage)message;
                    return JsonConvert.SerializeObject(characterStatisticsMessage);
                case "CREATE":
                    CreateMessage createMessage = (CreateMessage)message;
                    return JsonConvert.SerializeObject(createMessage);
                case "DEBUG":
                    DebugMessage debugMessage = (DebugMessage)message;
                    return JsonConvert.SerializeObject(debugMessage);
                case "END_TURN_REQUEST":
                    EndTurnRequestMessage endTurnRequestMessage = (EndTurnRequestMessage)message;
                    return JsonConvert.SerializeObject(endTurnRequestMessage);
            }
            return "";

            /*
            if (message.getMessageType().Equals("END_TURN_REQUEST"))
            {
                EndTurnRequestMessage debugMessage = (EndTurnRequestMessage)message;
                data = JsonConvert.SerializeObject(debugMessage);
            } else
            {
                data = JsonConvert.SerializeObject(message, new JsonSerializerSettings());
            }
            
            return data; */
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
