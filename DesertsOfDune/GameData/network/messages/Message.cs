using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.messages
{
    /// <summary>
    /// The base Message class all Messages extend this class.
    /// </summary>
    public abstract class Message
    {
        protected string version;
        protected Enums.MessageType type;

        /// <summary>
        /// The base constructor of the class Message
        /// </summary>
        /// <param name="version">version of the Message</param>
        /// <param name="type">the Messagetype of the Message</param>
        public Message(string version, Enums.MessageType type)
        {
            this.version = version;
            this.type = type;
        }
        /// <summary>
        /// Getter forthe field type
        /// </summary>
        /// <returns>Messagetype</returns>
        public Enums.MessageType getMessageType()
        {
            return this.type;
        }
    }
}
