using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.messages
{
    /// <summary>
    /// This class is responsible for the creation of Messages.
    /// </summary>
    public class MessageFactory
    {
        /// <summary>
        /// Constructor of the class MessageFactory
        /// </summary>
        public MessageFactory()
        {

        }
        /// <summary>
        /// This method creates a new Message
        /// </summary>
        /// <param name="type">type of Message that should be created</param>
        /// <param name="args">arguments of for the message</param>
        /// <returns></returns>
        public Message createMessage(MessageType type, string[] args)
        {
            return null;
        }
    }
}
