using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.util.world;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    /// <summary>
    /// This class is used to communicate a movementrequest.
    /// </summary>
    public class MovementRequestMessage : TurnMessage
    {

        [JsonProperty]
        private Specs specs;

        /// <summary>
        /// Constructor of the class MovementRequestMessage
        /// </summary>
        /// <param name="clientID">the id of the client</param>
        /// <param name="characterID">the id of the character</param>
        /// <param name="path">the path the character requests to take</param>
        public MovementRequestMessage(int clientID, int characterID, List<Position> path) : base(characterID, clientID, ActionType.MessageType.MOVEMENT_REQUEST)
        {
            Specs specs = new Specs();
            specs.path = path;
            this.specs = specs;
        }
    }
}
