using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    /// <summary>
    /// The base Message class all Messages extend this class.
    /// </summary>
    public abstract class Message
    {
        [JsonProperty(Order = -4)]
        protected string version;
        [JsonProperty(Order = -5)]
        protected string type;

        [JsonIgnore]
        protected MessageType messageType;

        /// <summary>
        /// The base constructor of the class Message
        /// </summary>
        /// <param name="version">version of the Message</param>
        /// <param name="type">the Messagetype of the Message</param>
        protected Message(string version, MessageType type)
        {
            this.version = version;
            this.type = Enum.GetName(typeof(MessageType), type);
            this.messageType = type;
        }
        /// <summary>
        /// Getter for the field type
        /// </summary>
        /// <returns>Messagetype</returns>
        public MessageType GetMessageType()
        {
            return this.messageType;
        }

        public string GetMessageTypeAsString()
        {
            return this.type;
        }
    }
}
