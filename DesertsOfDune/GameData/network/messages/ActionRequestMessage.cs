using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.util.world;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    /// <summary>
    /// This class is used to comunicate action requests.
    /// </summary>
    public class ActionRequestMessage : TurnMessage
    {
        [JsonProperty]
        private string action;
        [JsonProperty]
        private Specs specs;

        /// <summary>
        /// Constructor of the class ActionRequestMessage
        /// </summary>
        /// <param name="clientID">the id of the client</param>
        /// <param name="characterID">the id of the character</param>
        /// <param name="action">the action the client requests</param>
        /// <param name="target">the target the client wants to effect</param>
        public ActionRequestMessage(int clientID, int characterID, ActionType action, Position target) : base(characterID,clientID,MessageType.ACTION_REQUEST)
        {
            this.action = Enum.GetName(typeof(ActionType), action);
            Specs s = new Specs();
            s.target = target;
            this.specs = s;
        }
    }
}
