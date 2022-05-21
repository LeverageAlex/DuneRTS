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
<<<<<<< HEAD
        private Specs specs;
=======
        private Specs Specs;
>>>>>>> feature/clientConnectionToServer

        /// <summary>
        /// Constructor of the class ActionRequestMessage
        /// </summary>
        /// <param name="clientID">the id of the client</param>
        /// <param name="characterID">the id of the character</param>
        /// <param name="action">the action the client requests</param>
        /// <param name="target">the target the client wants to effect</param>
<<<<<<< HEAD
        /// <param name="targetID">the id of the target</param>
=======
>>>>>>> feature/clientConnectionToServer
        public ActionRequestMessage(int clientID, int characterID, ActionType action, Position target) : base(characterID,clientID,MessageType.ACTION_REQUEST)
        {
            this.action = Enum.GetName(typeof(ActionType), action);
            Specs specs = new Specs();
            specs.target = target;
<<<<<<< HEAD
            this.specs = specs;
=======
            this.Specs = specs;
>>>>>>> feature/clientConnectionToServer
        }
    }
}
