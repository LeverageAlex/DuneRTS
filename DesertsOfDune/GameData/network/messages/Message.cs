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

        /// <summary>
        /// The base constructor of the class Message
        /// </summary>
        /// <param name="version">version of the Message</param>
        /// <param name="type">the Messagetype of the Message</param>
        public Message(string version, MessageType type)
        {
            this.version = version;
            this.type = Enum.GetName(typeof(MessageType), type);
        }
        /// <summary>
        /// Getter forthe field type
        /// </summary>
        /// <returns>Messagetype</returns>
        public string getMessageType()
        {
            return this.type;
        }
    }
}
