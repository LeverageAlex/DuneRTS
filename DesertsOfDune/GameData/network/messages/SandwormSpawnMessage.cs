using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.util.world;

namespace GameData.network.messages
{
    /// <summary>
    /// This class is used to comunicate the spawn of a sandworm
    /// </summary>
    public class SandwormSpawnMessage : TurnMessage
    {
        private Position position;

        /// <summary>
        /// Constructor of the class SandwormSpawnMessage
        /// </summary>
        /// <param name="clientID">the id of the client</param>
        /// <param name="characterID">the id of the character</param>
        /// <param name="pos">the position the sandworm is spawned on</param>
        public SandwormSpawnMessage(int clientID, int characterID, Position pos) :base(characterID,clientID,MessageType.SANDWORM_SPAWN)
        {
            this.position = pos;
        }

    }
}
