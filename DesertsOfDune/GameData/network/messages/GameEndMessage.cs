using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.util.world;

namespace GameData.network.messages
{
    /// <summary>
    /// This message is used to comunicate the statistics at the end of the game
    /// </summary>
    public class GameEndMessage : Message
    {
        private int winner;
        private int loser;
        private Statistics statistics;

        /// <summary>
        /// Constructor of the class GameEndMessage
        /// </summary>
        /// <param name="winner">the winner of the game</param>
        /// <param name="loser">the loser of the game</param>
        /// <param name="statistics">the statistics of the game</param>
        public GameEndMessage(int winner, int loser, Statistics statistics) : base("v1",Enums.MessageType.GAME_END)
        {
            this.winner = winner;
            this.loser = loser;
            this.statistics = statistics;
        }
    }
}
