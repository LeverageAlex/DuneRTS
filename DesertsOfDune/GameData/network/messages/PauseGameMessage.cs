using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.messages
{
    /// <summary>
    /// This class is used to communicate the paus of the game.
    /// </summary>
    public class PauseGameMessage : Message
    {
        private int requestedByClientID;
        private bool pause;

        /// <summary>
        /// Constructor of the class PauseGameMessage
        /// </summary>
        /// <param name="requestedByClientID">the client id</param>
        /// <param name="pause">true if the game is paused.</param>
        public PauseGameMessage(int requestedByClientID, bool pause) : base("v1", Enums.MessageType.PAUSE_GAME)
        {
            this.pause = pause;
            this.requestedByClientID = requestedByClientID;
        }
    }
}
