using System;
using System.Collections.Generic;
using Server.Clients;
using GameData.gameObjects;
using Server.ClientManagement.Clients;
using Serilog;
using System.Runtime.CompilerServices;
using GameData.network.util.world;

namespace Server
{
    /// <summary>
    /// Represents a "Deserts of Dune"-Party, which is used for playing the game, so executing the game logic as well as the game start and ending.
    /// </summary>
    /// <remarks>
    /// Therefore this class stores all information about the party (identifier of this party) and the connected clients to this party.
    /// If two players are connected, the party can be prepared and started with this class. Afterwards it executes all game phases via the <see cref="RoundHandler"/>.
    /// Furthermore this class regularly check for the winning condition and can end the party or launch the end game phase.
    /// </remarks>
    /// TODO: do not work with singleton and references on both sides (message controller)
    public class Party
    {
        private static Party singleton;
        public ServerMessageController messageController { get;  set; }

        /// <summary>
        /// the identifier of this party / lobby
        /// </summary>
        public string LobbyCode { get; }

        /// <summary>
        /// the amount of ai clients joined to this party
        /// </summary>
        public int CpuCount { get; }

        /// <summary>
        /// list of all players connected to this party
        /// </summary>
        private readonly List<Client> connectedClients;

        /// <summary>
        /// hides the constructor for implementing the singleton pattern
        /// </summary>
        /// <param name="lobbyCode">the unique identifier of this party</param>
        private Party(string lobbyCode)
        {
            LobbyCode = lobbyCode;
            connectedClients = new List<Client>();

            Log.Debug("A new party was created with the code: " + lobbyCode);
        }

        public static Party GetInstance(string lobbyCode)
        {
            if (singleton == null) {
                singleton = new Party(lobbyCode);
            }
            return singleton;
        }

        /// <summary>
        /// adds a new Client to the party
        /// </summary>
        /// <param name="client"></param>
        public void AddClient(Client client)
        {
            connectedClients.Add(client);
        }

        /// <summary>
        /// checks, whether there are already two players registred in the party
        /// </summary>
        /// <returns>true, if there are already two players registred</returns>
        public bool AreTwoPlayersRegistred()
        {
            return connectedClients.FindAll(client => client.IsActivePlayer).Count == 2;
        }

        /// <summary>
        /// starts a new party, so prepare it and execute it until a winning condition becomes true or an error occur
        /// </summary>
        public void Start()
        {

        }

        public void PrepareGame()
        {
            // get two disjoint sets of each two great houses and offer them to the client
            GreatHouse[] firstSet;
            GreatHouse[] secondSet;
        }
    }
}
