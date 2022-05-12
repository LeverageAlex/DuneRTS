using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.util.world;

namespace GameData.network.messages
{
    /// <summary>
    /// This class is used to comunicate actions.
    /// </summary>
    public class ActionMessage : TurnMessage
    {
        private Enums.ActionType action;
        private Position target;
        private int targetID;

        /// <summary>
        /// Constructor of the class ActionMessage
        /// </summary>
        /// <param name="clientID">the id of the client</param>
        /// <param name="characterID">the id of the character</param>
        /// <param name="action">the action that is triggered</param>
        /// <param name="target">the target of the action</param>
        /// <param name="targetID">the id of the target</param>
        public ActionMessage(int clientID, int characterID, Enums.ActionType action, Position target, int targetID) : base(characterID,clientID,Enums.MessageType.ACTION)
        {
            this.action = action;
            this.target = target;
            this.targetID = targetID;
        }

    }
}
