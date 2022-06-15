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
        public string action { get; }
        [JsonProperty]
        public Specs specs { get; }

        /// <summary>
        /// Constructor of the class ActionRequestMessage
        /// </summary>
        /// <param name="clientID">the id of the client</param>
        /// <param name="characterID">the id of the character</param>
        /// <param name="action">the action the client requests</param>
        /// <param name="target">the target the client wants to effect</param>
        [JsonConstructor]
        public ActionRequestMessage(int clientID, int characterID, ActionType action, Specs specs) : base(characterID,clientID,MessageType.ACTION_REQUEST)
        {
            this.action = Enum.GetName(typeof(ActionType), action);
            this.specs = specs;
            
          //  s.target = target;
          //  this.specs = s;
        }
    }
}
