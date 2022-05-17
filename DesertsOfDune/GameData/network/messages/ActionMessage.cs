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
    public class ActionMessage : TurnMessage
    {
        [JsonProperty]
        private string action;
        [JsonProperty(Order = 5)]
        private Position target;
        [JsonProperty(Order = 7)]
        private int targetID;

        /// <summary>
        /// Constructor of the class ActionMessage
        /// </summary>
        /// <param name="clientID">the id of the client</param>
        /// <param name="characterID">the id of the character</param>
        /// <param name="action">the action that is triggered</param>
        /// <param name="target">the target of the action</param>
        /// <param name="targetID">the id of the target</param>
        public ActionMessage(int clientID, int characterID, ActionType.ActionType action, Position target, int targetID) : base(characterID,clientID,ActionType.MessageType.ACTION)
        {
            this.action = Enum.GetName(typeof(ActionType.ActionType), action);
            this.target = target;
            this.targetID = targetID;
        }

    }
}
