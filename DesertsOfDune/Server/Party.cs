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
using Server.Configuration;
using GameData.network.util.world.mapField;
using GameData.network.util.world.character;
using System.Threading;

namespace Server
{
    /// <summary>
    /// Represents a "Deserts of Dune"-Party, which is used for playing the game, so executing the game logic as well as the game start and ending.
    /// </summary>
    /// <remarks>
    /// Therefore this class stores all information about the party (identifier of this party) and the connected clients to this party.
    /// If two players are connected, the party can be prepared and started with this class. Afterwards it executes all game phases via the <see cref="GameData.gameObjects.RoundHandler"/>.
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
        /// the round handler for this party, which execute the rounds in the correct order and handles the user input
        /// </summary>
        public RoundHandler RoundHandler { get; }

        public Thread roundHandlerThread { get; private set; }

        /// <summary>
        /// the map of this game / party
        /// </summary>
        public Map map { get; }

        /// <summary>
        /// hides the constructor for implementing the singleton pattern and create all necessary instances
        /// </summary>
        /// <remarks>
        /// creats a new round handler, with the configuration values from the the party configuration.
        /// So ensure, that the party configuration is properly loaded!
        /// </remarks>
        private Party()
        {
            connectedClients = new List<Client>();
            map = new Map(ScenarioConfiguration.SCENARIO_WIDTH, ScenarioConfiguration.SCENARIO_HEIGHT, ScenarioConfiguration.GetInstance().scenario);
            RoundHandler = new RoundHandler(PartyConfiguration.GetInstance().numbOfRounds, PartyConfiguration.GetInstance().spiceMinimum, map);
            Noble.greatHouseConventionBroken = false;

            Log.Debug("A new party was created!");
        }

        /// <summary>
        /// get the reference to the current instance for implementing the singleton pattern
        /// </summary>
        /// <returns></returns>
        public static Party GetInstance()
        {
            if (singleton == null)
            {
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
            List<Player> foundActiveClients = new List<Player>();
            foreach (var activePlayer in connectedClients)
            {
                if (activePlayer.IsActivePlayer)
                {
                    foundActiveClients.Add((Player)activePlayer);
                }
            }

            if (foundActiveClients.Count == 0)
            {
                // there are no players
                Log.Debug("There were no active clients, so player found");
            }
            return foundActiveClients;
        }

        /// <summary>
        /// starts a new game, so place the characters of the players around their cities and trigger the first round
        /// </summary>
        public void Start()
        {
            Log.Debug("Matching the cities to the players...");
            MatchGreatHouseToCity();

            // cities were matched to the characters, so send the game config message to the clients
            messageController.DoSendGameConfig();

            Log.Debug("Place the characters of each player around it's city...");
            PlaceCharactersAroundCity();

            Log.Information("The party was prepared, so both player chose their Greathouse. The party now will start ... ");
            roundHandlerThread = new Thread(RoundHandler.NextRound);
            roundHandlerThread.Start();
       //     RoundHandler.NextRound();
            Log.Debug("Triggered first round by round handler");
        }

        /// <summary>
        /// execute the preparation of the game, so create to disjoint sets of great houses and offer them to the clients
        /// </summary>
        public void PrepareGame()
        {
            Log.Information("Preparing Game");
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

        /// <summary>
        /// gets a player from the list of all players, whose character has the given character id
        /// </summary>
        /// <param name="characterID">the id of the character, whose matched player need to be determined</param>
        /// <returns>the player, who has the character with the given id or null, if the character do not belong to any player</returns>
        /// TODO: do not return null
        public Player GetPlayerByCharacterID(int characterID)
        {
            Player player = null; 
            foreach (Player activePlayer in GetActivePlayers())
            {
                Character foundCharacter = activePlayer.UsedGreatHouse.Characters.Find((character) => character.CharacterId == characterID);
                if (foundCharacter != null)
                {
                    return activePlayer;
                }
            }
            return player;
        }

        /// <summary>
        /// decide for each city, which player will use this city and match the city to the player
        /// </summary>
        public void MatchGreatHouseToCity()
        {
            List<City> cities = map.GetCitiesOnMap();
            List<Player> players = GetActivePlayers();

            // pick the city for the first player and the second player
            Random random = new Random();
            if (random.NextDouble() < 0.5)
            {
                players[0].City = cities[0];
                players[1].City = cities[1];

                
            }
            else
            {
                players[0].City = cities[1];
                players[1].City = cities[0];
            }

        }

        /// <summary>
        /// places all characters of all players randomly around their cities
        /// </summary>
        public void PlaceCharactersAroundCity()
        {
            List<Player> players = GetActivePlayers();

            foreach (Player player in players)
            {
                // get city and place the characters from the great house around
                // List<MapField> neighborFieldsOfCity = map.GetNeighborFields(player.City);

                // place the characters around the city
                foreach (Character character in player.UsedGreatHouse.Characters)
                {
                    MapField fieldForCharacter = this.map.GetRandomFreeApproachableNeighborField(player.City);
                    fieldForCharacter.PlaceCharacter(character);
                    character.CurrentMapfield = fieldForCharacter;

                    messageController.DoSpawnCharacterDemand(character);
                    // neighborFieldsOfCity.Remove(fieldForCharacter);
                }
                
            }
        }

        /*private MapField GetRandomField(List<MapField> fields)
        {
            Random random = new Random();
            return fields[random.Next(fields.Count)];
        }*/

        public List<Client> GetConnectedClients()
        {
            return this.connectedClients;
        }
    }
}
