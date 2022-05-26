using System;
using System.Collections.Generic;
using Server.Clients;
using GameData.gameObjects;
using Server.ClientManagement.Clients;
using Serilog;
using System.Runtime.CompilerServices;
using GameData.network.util.world;
using GameData.network.util.enums;
using System.Runtime.ExceptionServices;
using System.Linq;
using WebSocketSharp;
using CommandLine;

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
        public ServerMessageController messageController { get; set; }

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
        private Party()
        {
            connectedClients = new List<Client>();

            Log.Debug("A new party was created!");
        }

        /// <summary>
        /// get the reference to the current instance for implementing the singleton pattern
        /// </summary>
        /// <returns></returns>
        public static Party GetInstance()
        {
            if (singleton == null) {
                singleton = new Party();
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
        /// return all active client, so all players
        /// </summary>
        /// <returns>a list of all players or an empty list, if there are no players</returns>
        public List<Player> GetActivePlayers()
        {
            List<Client> foundActiveClients = connectedClients.FindAll(client => client.IsActivePlayer);

            if (foundActiveClients.Count == 0)
            {
                // there are no players
                Log.Debug("There were no active clients, so player found");
                return new List<Player>();
            } else
            {
                return (List<Player>)foundActiveClients.Cast<Player>();
            }
        }

        /// <summary>
        /// starts a new party, so prepare it and execute it until a winning condition becomes true or an error occur
        /// </summary>
        public void Start()
        {

        }

        /// <summary>
        /// execute the preparation of the game, so create to disjoint sets of great houses and offer them to the clients
        /// </summary>
        public void PrepareGame()
        {
            // get two disjoint sets of each two great houses and offer them to the client
            GreatHouseType[] firstSet;
            GreatHouseType[] secondSet;

            List<Player> activePlayers = GetActivePlayers();

            GreatHouseType[] possibleGreatHousesForFirstSet = { GreatHouseType.ATREIDES, GreatHouseType.CORRINO, GreatHouseType.HARKONNEN, GreatHouseType.ORDOS, GreatHouseType.RICHESE, GreatHouseType.VERNIUS };
            firstSet = GetTwoRandomGreatHouses(possibleGreatHousesForFirstSet);

            GreatHouseType[] possibleGreatHousesForSecondSet = possibleGreatHousesForFirstSet.Except(firstSet).ToArray();
            secondSet = GetTwoRandomGreatHouses(possibleGreatHousesForSecondSet);

            messageController.DoSendHouseOffer(activePlayers[0].ClientID, firstSet);
            messageController.DoSendHouseOffer(activePlayers[1].ClientID, secondSet);

            // set the choice sets as offered great house sets in the player, so later can be checked, whether they chose a possble great house
            activePlayers[0].OfferedGreatHouses = firstSet;
            activePlayers[1].OfferedGreatHouses = secondSet;
        }

        /// <summary>
        /// get two random great house (types) from a set, with possible great houses (types)
        /// </summary>
        /// <param name="possibleGreatHouses">set of possible great house types</param>
        /// <returns>an array with two entries, which contain to different great house types</returns>
        private GreatHouseType[] GetTwoRandomGreatHouses(GreatHouseType[] possibleGreatHouses)
        {
            return possibleGreatHouses.OrderBy(n => Guid.NewGuid()).ToArray().SubArray(0, 2);
        }

        /// <summary>
        /// gets a client from the list of all connected clients by its session id
        /// </summary>
        /// <param name="sessionID">the session id of the client</param>
        /// <returns>the reference to the client or null, if the client was not found</returns>
        public Client GetClientBySessionID(string sessionID)
        {
            return connectedClients.Find(client => client.SessionID == sessionID);
        }


        /// <summary>
        /// gets a player from the list of all connected clients by its session id
        /// </summary>
        /// <param name="sessionID">the session id of the player</param>
        /// <returns>the reference to the client or null, if the player was not found</returns>
        public Player GetPlayerBySessionID(string sessionID)
        {
            Client foundClient = GetClientBySessionID(sessionID);

            if (foundClient != null)
            {
                if (foundClient.IsActivePlayer)
                {
                    return (Player)foundClient;
                }
                else
                {
                    Log.Error("There is no player with the session ID " + sessionID);
                }
            }
            return null;
        }
    }
}
