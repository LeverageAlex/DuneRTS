using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.messages
{
    /// <summary>
    /// This class is used to comunicate the end game mechanism.
    /// </summary>
    public class EndGameMessage : Message
    {
        /// <summary>
        /// Constructor of the class EndGameMessage
        /// </summary>
        public EndGameMessage() : base("v1", Enums.MessageType.ENDGAME)
        {

        }
    }
}
