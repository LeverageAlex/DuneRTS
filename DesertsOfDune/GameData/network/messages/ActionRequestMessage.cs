using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.messages
{
    /// <summary>
    /// This class is used to comunicate action requests.
    /// </summary>
    public class ActionRequestMessage : TurnMessage
    {
        private Enums.ActionType action;
        private Position target;
        private int targetID;

        /// <summary>
        /// Constructor of the class ActionRequestMessage
        /// </summary>
        /// <param name="clientID">the id of the client</param>
        /// <param name="characterID">the id of the character</param>
        /// <param name="action">the action the client requests</param>
        /// <param name="target">the target the client wants to effect</param>
        /// <param name="targetID">the id of the target</param>
        public ActionRequestMessage(int clientID, int characterID, Enums.ActionType action, Position target, int targetID) : base(characterID,clientID,Enums.MessageType.ACTION_REQUEST)
        {
            this.action = action;
            this.target = target;
            this.targetID = targetID;
        }
    }
}
