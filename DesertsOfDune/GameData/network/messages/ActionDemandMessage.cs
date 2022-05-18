using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.util.world;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    /// <summary>
    /// This class is used to comunicate actions.
    /// </summary>
    public class ActionDemandMessage : TurnMessage
    {
        [JsonProperty]
        private string action;
        [JsonProperty(Order = 5)]
        private Specs specs;

        /// <summary>
        /// Constructor of the class ActionMessage
        /// </summary>
        /// <param name="clientID">the id of the client</param>
        /// <param name="characterID">the id of the character</param>
        /// <param name="action">the action that is triggered</param>
        /// <param name="target">the target of the action</param>
        /// <param name="targetID">the id of the target</param>
        public ActionDemandMessage(int clientID, int characterID, ActionType action, Position target, int targetID) : base(characterID,clientID,MessageType.ACTION_DEMAND)
        {
            this.action = Enum.GetName(typeof(ActionType), action);
            Specs specs = new Specs();
            specs.target = target;
            specs.targetID = targetID;
            this.specs = specs;
        }

    }
}
