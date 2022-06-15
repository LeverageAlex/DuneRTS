using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.util.world;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    /// <summary>
    /// This class is used to comunicate the spawn of a sandworm
    /// </summary>
    public class SandwormSpawnDemandMessage : TurnMessage
    {
        [JsonProperty]
        public Position position { get; }

        /// <summary>
        /// Constructor of the class SandwormSpawnMessage
        /// </summary>
        /// <param name="clientID">the id of the client</param>
        /// <param name="characterID">the id of the character</param>
        /// <param name="pos">the position the sandworm is spawned on</param>
        public SandwormSpawnDemandMessage(int clientID, int characterID, Position position) :base(characterID,clientID,MessageType.SANDWORM_SPAWN_DEMAND)
        {
            this.position = position;
        }

    }
}
