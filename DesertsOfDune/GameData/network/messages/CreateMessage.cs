using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace GameData.network.messages
{
    /// <summary>
    /// This class is used as a message to create a game
    /// </summary>
    public class CreateMessage : Message
    {
        [JsonProperty]
        public string LobbyCode { get; }
        [JsonProperty]
<<<<<<< HEAD
        private bool spectate;
=======
        public bool Spectate { get; }
>>>>>>> feature/clientConnectionToServer

        /// <summary>
        /// Constructor of the class CreateMessage
        /// </summary>
        /// <param name="lobbyCode">the lobbyCode of the game lobby</param>
<<<<<<< HEAD
        /// <param name="spectate">true, if the client wants to spectate the game</param>
        public CreateMessage(string lobbyCode,bool spectate) : base("1.0", MessageType.CREATE)
        {
            this.lobbyCode = lobbyCode;
            this.spectate = spectate;
=======
        /// <param name="spectate">notification if client is spectator</param>
        public CreateMessage(string lobbyCode,bool spectate) : base("v1", MessageType.CREATE)
        {
            this.LobbyCode = lobbyCode;
            this.Spectate = spectate;
>>>>>>> feature/clientConnectionToServer
        }
    }
}
