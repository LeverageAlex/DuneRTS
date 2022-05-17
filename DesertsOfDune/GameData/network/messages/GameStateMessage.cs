using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.messages
{
    /// <summary>
    /// This class is used to comunicate the gamestate
    /// </summary>
    public class GameStateMessage : ClientServerMessage
    {
        private string[] history;

        /// <summary>
        /// Constructor of the class GameStateMessage
        /// </summary>
        /// <param name="history">the history of the Game</param>
        /// <param name="history">the client id</param>
        public GameStateMessage(string[] history, int clientID) : base(clientID,Enums.MessageType.GAMESTATE)
        {
            this.history = history;
        }
    }
}
