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
       // [JsonProperty]
        //private Position target;
        [JsonProperty]
        private Specs specs;
        [JsonProperty]
        private int targetID;

        /// <summary>
        /// Constructor of the class ActionRequestMessage
        /// </summary>
        /// <param name="clientID">the id of the client</param>
        /// <param name="characterID">the id of the character</param>
        /// <param name="action">the action the client requests</param>
        /// <param name="target">the target the client wants to effect</param>
        /// <param name="targetID">the id of the target</param>
        public ActionRequestMessage(int clientID, int characterID, ActionType.ActionType action, Position target, int targetID) : base(characterID,clientID,ActionType.MessageType.ACTION_REQUEST)
        {
            this.action = Enum.GetName(typeof(ActionType.ActionType), action);
            Specs specs = new Specs();
            specs.target = target;
            this.specs = specs;
            this.targetID = targetID;
        }
    }
}
