using System;
using System.Collections.Generic;
using GameData.network.controller;
using GameData.network.messages;
using GameData.network.util.world;
using GameData.Clients;
using Serilog;
using GameData.ClientManagement.Clients;
using GameData.network.util.enums;
using GameData.network.util.world.greatHouse;
using System.Linq;
using GameData.network.util.parser;
using GameData.Configuration;
using Newtonsoft.Json;
using GameData.server.roundHandler;
using GameData.network.util.world.mapField;
using GameData.network.util.world.character;
using GameData.roundHandler;
using System.Threading;

namespace GameData
{
    public class ServerMessageController : MessageController
    {
        private bool firstPlayerGotGreatHousesAndGotRequestAck;

        public ServerMessageController()
        {
            this.firstPlayerGotGreatHousesAndGotRequestAck = false;
        }

        /// <summary>
        /// Client requests to join a party with a clintName and a flag if he is player or spectator.
        /// To join to the party, the connectionCode from the JoinMessage has to be equal to the lobbyCode of the created party.
        /// </summary>
        /// <param name="msg">JoinMessage with the value clientName, connectionCode and active flag if he is a player.</param>
        /// <param name="sessionID">the session id of the client, who wants to join</param>
        public override void OnJoinMessage(JoinMessage msg, string sessionID)
        {
            Client client;
            bool gameHasStarted = Party.GetInstance().AreTwoPlayersRegistred();

            // check, whether the new client is a player or spectator
            if (msg.isActive)
            {
                // check, whether there are already two active player
                if (Party.GetInstance().AreTwoPlayersRegistred())
                {
                    // already two players are registred, so send error
                    DoSendError(003, "There are already two players registred", sessionID);
                    return;
                }

                // check, whether active player is a human or an ai
                if (msg.isCpu)
                {
                    // client is an ai
                    client = new AIPlayer(msg.clientName, sessionID);
                }
                else
                {
                    // client is a human player
                    client = new HumanPlayer(msg.clientName, sessionID);
                }
            }
            else
            {
                if (!msg.isCpu)
                {
                    client = new Spectator(msg.clientName, sessionID);
                    Console.WriteLine("Registered new Spectator");
                    if (gameHasStarted)
                    {
                        //Game already running, so need to fix some issues
                        Party.GetInstance().AddClient(client);
                        DoAcceptJoin(client.ClientSecret, client.ClientID, sessionID);
                        OnGameStateRequestMessage(new GameStateRequestMessage(client.ClientID));
                        return;
                    }
                }
                else
                {
                    DoSendError(002, "isActive and isCpu do not match, because a cpu-client cannot be a spectator", sessionID);
                    return;
                }

            }
            Party.GetInstance().AddClient(client);
            // send join accept
            DoAcceptJoin(client.ClientSecret, client.ClientID, sessionID);

            // check, if with new client two players are registred and start party
            if (Party.GetInstance().AreTwoPlayersRegistred() && !gameHasStarted)
            {
                Party.GetInstance().PrepareGame();
            }
        }

        /// <summary>
        /// If a client loose connection to the server, he can rejoin to the game with the clientSecret from the JoinAcceptedMessage.
        /// </summary>
        /// <param name="msg">RejoinMessage with clientSecretParameter.</param>
        /// <param name="sessionID">he session id of the client, who wants to rejoin to the party</param>
        public override void OnRejoinMessage(RejoinMessage msg, string sessionID)
        {
            var connectedClients = Party.GetInstance().GetConnectedClients();
            bool rejoinSuccessful = false;
            foreach (var client in connectedClients)
            {
                if (client.ClientSecret == msg.clientSecret)
                {
                    client.SessionID = sessionID; //new sessionID for the rejoined client
                    rejoinSuccessful = true;
                    DoAcceptJoin(client.ClientSecret, client.ClientID, client.SessionID);
                    Log.Information($"Rejoin of client: {client.ClientName} was successful.");
                    //Sending Gamestate for syncing reasons
                    OnGameStateRequestMessage(new GameStateRequestMessage(client.ClientID));

                }
            }
            if (!rejoinSuccessful)
            {
                //disconnect the client because the reconnect failed
                DoSendError(005, "Rejoin failed because clientSecret do not match!", sessionID);
                Log.Information($"Rejoin of client failed.");
                ((ServerConnectionHandler)NetworkController.connectionHandler).sessionManager.CloseSession(sessionID, WebSocketSharp.CloseStatusCode.Normal, "clientSecret do not match!");
            }
        }

        /// <summary>
        /// executed, if the clients requested a certain great house
        /// </summary>
        /// <param name="msg">the HouseRequestMessage, which contains the house name</param>
        /// <param name="sessionID">the session id of the requesting client</param>
        public override void OnHouseRequestMessage(HouseRequestMessage msg, string sessionID)
        {
            GreatHouseType chosenGreatHouse = (GreatHouseType)Enum.Parse(typeof(GreatHouseType), msg.houseName);

            // get the player, who send this request
            Player requestingPlayer = Party.GetInstance().GetPlayerBySessionID(sessionID);

            if (requestingPlayer != null)
            {
                // check, whether this decision is valid, if not resend house offer and strike
                if (requestingPlayer.OfferedGreatHouses.Contains(chosenGreatHouse))
                {
                    requestingPlayer.UsedGreatHouse = GreatHouseFactory.CreateNewGreatHouse(chosenGreatHouse);

                    DoSendHouseAck(requestingPlayer.ClientID, chosenGreatHouse.ToString());
                    Log.Information("The player with the session id: " + sessionID + " chose the great house " + chosenGreatHouse.ToString());

                    // check, whether the other player already got confirmation
                    if (!firstPlayerGotGreatHousesAndGotRequestAck)
                    {
                        firstPlayerGotGreatHousesAndGotRequestAck = true;
                    }
                    else
                    {
                        // first player already has great house, so start the game
                        Party.GetInstance().Start();
                        List<Player> listPlayer = Party.GetInstance().GetActivePlayers();
                        foreach (Player player in listPlayer)
                        {
                            player.UsedGreatHouse.City = player.City;
                        }
                    }
                }
                else
                {
                    Log.Error("The player with the session id: " + sessionID + " requested " + chosenGreatHouse.ToString() + ", but the server didn't offered this greathouse");

                    // resend house offer:
                    DoSendHouseOffer(requestingPlayer.ClientID, requestingPlayer.OfferedGreatHouses);

                    // send a strike
                    DoSendStrike(requestingPlayer.ClientID, msg);
                }

            }
            else
            {
                Log.Error("There is no player with the session id: " + sessionID);
            }

        }

        /// <summary>
        /// executed, when a player wants to move his character along a path while it's his turn
        /// </summary>
        /// <param name="msg">contains informations about the player, the character he wants to move and the path he wants to move his character along</param>
        /// <exception cref="NotImplementedException"></exception>
        public override void OnMovementRequestMessage(MovementRequestMessage msg)
        {
            //get the player who wants to move his character
            Player activePlayer = Party.GetInstance().GetPlayerByClientID(msg.clientID);

            if (activePlayer == null)
            {
                throw new NullReferenceException($"Requested player with {msg.clientID} isn't known!");
            }

            //get the character which should be moved
            Character movingCharacter = GetCharacterByIdFromPlayer(activePlayer, msg.characterID);

            if (movingCharacter == null)
            {
                DoSendError(005, $"Moving character is null", activePlayer.SessionID);
                return;
            }

            var path = msg.specs.path;
            var newPath = new List<Position>();
            foreach (var position in path)
            {
                var party = Party.GetInstance();
                if (movingCharacter.MPcurrent > 0) //check if Character has enough Movement Points
                {
                    if (position.x >= 0 && position.x < party.map.MAP_WIDTH && position.y >= 0 && position.y < party.map.MAP_HEIGHT) //check if movement is in bounds of the map
                    {
                        //check if movement is on walkable terrain
                        if (party.map.fields[position.y, position.x].tileType != TileType.MOUNTAINS.ToString() && party.map.fields[position.y, position.x].tileType != TileType.CITY.ToString()) //check needed and not implemented utils
                        {
                            bool movementWasAllowed = false;

                            if (party.map.fields[position.y, position.x].IsCharacterStayingOnThisField)  //if the mapfield is occupied by a character they swap positions
                            {
                                Character passiveCharacter = party.map.fields[position.y, position.x].GetCharacterStayingOnThisField(party.map.GetCharactersOnMap());
                                if (passiveCharacter != null)
                                {
                                    passiveCharacter.Movement(passiveCharacter.CurrentMapfield, movingCharacter.CurrentMapfield);
                                    DoSendMovementDemand(msg.clientID, passiveCharacter.CharacterId, new List<Position> { new Position(movingCharacter.CurrentMapfield.XCoordinate, movingCharacter.CurrentMapfield.ZCoordinate) });
                                    movementWasAllowed = movingCharacter.Movement(movingCharacter.CurrentMapfield, party.map.fields[position.y, position.x]);
                                }
                            }
                            else
                            {
                                movementWasAllowed = movingCharacter.Movement(movingCharacter.CurrentMapfield, party.map.fields[position.y, position.x]); //move character 1 field along its path
                            }
                            if (movementWasAllowed)
                            {
                                newPath.Add(position);
                                if (party.map.fields[position.y, position.x].tileType == TileType.FLAT_SAND.ToString() || party.map.fields[position.y, position.x].tileType == TileType.DUNE.ToString())
                                {
                                    if(movingCharacter.SteppedOnSandfield)
                                    {
                                        movingCharacter.SetLoud();
                                    }
                                    else
                                    {
                                        movingCharacter.SteppedOnSandfield = true;
                                    }
                                }
                                TryDeliverSpiceToCity(activePlayer, movingCharacter);
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            if (newPath.Count != 0)
            {
                DoSendMovementDemand(msg.clientID, msg.characterID, newPath);
                DoSendChangeCharacterStatsDemand(msg.clientID, msg.characterID, new CharacterStatistics(movingCharacter));

                // TODO: movement request was invalid, so end the game!!!!!!
            }
            if (movingCharacter.MPcurrent <= 0 && movingCharacter.APcurrent <= 0) // all ap and mp were used in this characterTrait -> end turn
            {
                Party.GetInstance().RoundHandler.GetCharacterTraitPhase().SendRequestForNextCharacter();
            }
        }

        /// <summary>
        /// Check if character is on neighborfield of the city. If yes deliver all spice to the city.
        /// </summary>
        /// <param name="activePlayer">The active player</param>
        /// <param name="movingCharacter">The moving character</param>
        private void TryDeliverSpiceToCity(Player activePlayer, Character movingCharacter)
        {
            foreach (var mapfield in Party.GetInstance().map.GetNeighborFields(movingCharacter.CurrentMapfield))
            {
                if (mapfield.IsCityField && mapfield.clientID == activePlayer.ClientID)
                {
                    activePlayer.statistics.AddToHouseSpiceStorage(movingCharacter.inventoryUsed);
                    movingCharacter.inventoryUsed = 0;
                    DoChangePlayerSpiceDemand(activePlayer.ClientID, activePlayer.statistics.HouseSpiceStorage);
                }
            }
        }

        /// <summary>
        /// Gets a character by its id from a player
        /// </summary>
        /// <param name="player">The player from where to get the character</param>
        /// <param name="characterID">The id of the character</param>
        /// <returns>Return the found character</returns>
        private Character GetCharacterByIdFromPlayer(Player player, int characterID)
        {
            foreach (var character in player.UsedGreatHouse.GetCharactersAlive())
            {
                if (character.CharacterId == characterID)
                {
                    return character;
                }
            }
            return null;
        }

        /// <summary>
        /// executed, when a player want to do a action with his character while it's his turn
        /// </summary>
        /// <param name="msg">contains informations about the player, the character he wants to do a action with and the action he wants his character to do</param>
        public override void OnActionRequestMessage(ActionRequestMessage msg)
        {
            //request from client to run an action

            //get the player who wants to do the action
            Player activePlayer = Party.GetInstance().GetPlayerByClientID(msg.clientID);
            Player enemyPlayer = Party.GetInstance().GetActivePlayers().Find(c => c.ClientID != activePlayer.ClientID);

            if (activePlayer == null)
            {
                throw new NullReferenceException($"Requested player with {msg.clientID} isn't known!");
            }
            if (enemyPlayer == null)
            {
                throw new NullReferenceException($"Enemy player not found!");
            }

            //get the characters which are involved in the action
            Character actionCharacter = GetCharacterByIdFromPlayer(activePlayer, msg.characterID);
            Character targetCharacter = null;
            //get the target character from enemy player if the target character is not an ally
            if (msg.specs.target != null)
            {
                foreach (var character in enemyPlayer.UsedGreatHouse.GetCharactersAlive())
                {
                    if (character.CurrentMapfield.XCoordinate == msg.specs.target.x && character.CurrentMapfield.ZCoordinate == msg.specs.target.y)
                    {
                        targetCharacter = character;
                    }
                }
            }

            if (actionCharacter == null)
            {
                DoSendError(005, "ActionCharacter is null", activePlayer.SessionID);
                return;
            }

            ActionType action;
            if (actionCharacter.APcurrent > 0)
            {
                var map = Party.GetInstance().map;
                var charactersHit = new List<Character>();
                //check which action the player wants to do with his character
                if (!actionCharacter.IsInSandStorm(map))
                {
                    switch (Enum.Parse(typeof(ActionType), msg.action))
                    {
                        case ActionType.ATTACK:
                            action = ActionType.ATTACK;
                            ExecuteAttack(msg, activePlayer, actionCharacter, targetCharacter, action, map, charactersHit);
                            break;
                        case ActionType.COLLECT:
                            action = ActionType.COLLECT;
                            ExecuteCollectSpice(msg, activePlayer, actionCharacter, action);
                            break;
                        case ActionType.KANLY:
                            action = ActionType.KANLY;
                            ExecuteKanlyAction(msg, activePlayer, targetCharacter, actionCharacter, action, map, charactersHit);
                            break;
                        case ActionType.FAMILY_ATOMICS:
                            action = ActionType.FAMILY_ATOMICS;
                            charactersHit = ExecuteAtomics(msg, activePlayer, enemyPlayer, actionCharacter, action, map);
                            break;
                        case ActionType.SPICE_HOARDING:
                            action = ActionType.SPICE_HOARDING;
                            ExecuteSpiceHoarding(msg, activePlayer, actionCharacter, action, map);
                            break;
                        case ActionType.VOICE:
                            action = ActionType.VOICE;
                            ExecuteVoice(msg, activePlayer, targetCharacter, actionCharacter, action, map, charactersHit);
                            break;
                        case ActionType.SWORD_SPIN:
                            action = ActionType.SWORD_SPIN;
                            charactersHit = ExecuteSwordSpin(msg, activePlayer, actionCharacter, action, map);
                            break;
                        default:
                            throw new ArgumentException($"Actiontype {msg.action} not supoorted here.");
                    }
                    UpdateCharacterStatistics(activePlayer, enemyPlayer, actionCharacter, charactersHit);
                    bool hit = false;
                    foreach (var localChar in charactersHit)
                    {
                        if (localChar.healthCurrent <= 0 && localChar.inventoryUsed > 0)
                        {
                            //Drop Spice
                            Map.instance.SpreadSpiceOnFields(localChar.CurrentMapfield, localChar.inventoryUsed);
                            hit = true;
                            localChar.inventoryUsed = 0;
                        }
                    }
                    if(hit)
                    {
                        DoSendMapChangeDemand(MapChangeReasons.ROUND_PHASE);
                    }
                }
                else
                {
                    //Character is in Sandstorm
                    actionCharacter.MPcurrent = 0;
                    actionCharacter.APcurrent = 0;
                }
            }

            if ((actionCharacter.MPcurrent <= 0 && actionCharacter.APcurrent <= 0) || actionCharacter.IsDead())
            {
                Party.GetInstance().RoundHandler.GetCharacterTraitPhase().SendRequestForNextCharacter();
            }
        }

        /// <summary>
        /// Sends the changed character statistics to the client.
        /// </summary>
        /// <param name="activePlayer">The active player</param>
        /// <param name="enemyPlayer">The enemy player</param>
        /// <param name="actionCharacter">The action character</param>
        /// <param name="charactersHit">The list of hittet characters</param>
        private void UpdateCharacterStatistics(Player activePlayer, Player enemyPlayer, Character actionCharacter, List<Character> charactersHit)
        {
            foreach (var character in charactersHit)
            {
                if (activePlayer.UsedGreatHouse == character.greatHouse)
                {
                    DoSendChangeCharacterStatsDemand(activePlayer.ClientID, character.CharacterId, new CharacterStatistics(character));

                }
                else if (enemyPlayer.UsedGreatHouse == character.greatHouse)
                {
                    DoSendChangeCharacterStatsDemand(enemyPlayer.ClientID, character.CharacterId, new CharacterStatistics(character));
                }
            }
            DoSendChangeCharacterStatsDemand(activePlayer.ClientID, actionCharacter.CharacterId, new CharacterStatistics(actionCharacter));
        }

        /// <summary>
        /// executed if the player wants to transfer spice from one character to another
        /// </summary>
        /// <param name="msg">contains information about the player who wants to transfer spice, the character who already has the spice, the character who should get the spice and the amount of spice he wants to transfer</param>
        public override void OnTransferRequestMessage(TransferRequestMessage msg)
        {
            //get the player who wants to do the transfer
            Player activePlayer = Party.GetInstance().GetPlayerByClientID(msg.clientID);

            if (activePlayer == null)
            {
                throw new NullReferenceException($"Requested player with {msg.clientID} isn't known!");
            }

            //get the characters which are involved in the transfer
            Character activeCharacter = GetCharacterByIdFromPlayer(activePlayer, msg.characterID);
            Character targetCharacter = GetCharacterByIdFromPlayer(activePlayer, msg.targetID);

            if (activeCharacter == null)
            {
                DoSendError(005, "Active character is null", activePlayer.SessionID);
                return;
            }
            if (targetCharacter == null)
            {
                DoSendError(005, "TargetCharacter is null", activePlayer.SessionID);
                return;
            }

            if (!activeCharacter.IsInSandStorm(Map.instance) && !targetCharacter.IsInSandStorm(Map.instance))
            {
                bool targetCharacterIsOnNeighborfield = false;
                foreach (var mapfield in Party.GetInstance().map.GetNeighborFields(activeCharacter.CurrentMapfield))
                {
                    if (mapfield.IsCharacterStayingOnThisField && mapfield.Character.CharacterId == targetCharacter.CharacterId)
                    {
                        targetCharacterIsOnNeighborfield = true;
                    }
                }

                if (targetCharacterIsOnNeighborfield && targetCharacter.greatHouse == activeCharacter.greatHouse)
                {
                    activeCharacter.GiftSpice(targetCharacter, msg.amount);
                    DoSendTransferDemand(msg.clientID, msg.characterID, msg.targetID);

                    DoSendChangeCharacterStatsDemand(msg.clientID, msg.characterID, new CharacterStatistics(activeCharacter));
                    DoSendChangeCharacterStatsDemand(msg.clientID, msg.targetID, new CharacterStatistics(targetCharacter));
                    TryDeliverSpiceToCity(activePlayer, targetCharacter);
                }

                if (activeCharacter.MPcurrent <= 0 && activeCharacter.APcurrent <= 0)
                {
                    Party.GetInstance().RoundHandler.GetCharacterTraitPhase().SendRequestForNextCharacter();
                }
            }
        }

        /// <summary>
        /// End turn of a character and heal this character if he hasn't moved
        /// </summary>
        /// <param name="msg">EndTurnMessage with clientID and characterID</param>
        public override void OnEndTurnRequestMessage(EndTurnRequestMessage msg)
        {
            foreach (var player in Party.GetInstance().GetActivePlayers())
            {
                if (player.ClientID == msg.clientID)
                {
                    foreach (var character in player.UsedGreatHouse.Characters)
                    {
                        if (character.CharacterId == msg.characterID && character.MPcurrent == character.MPmax)
                        {
                            character.HealIfHasntMoved();
                            DoSendChangeCharacterStatsDemand(msg.clientID, msg.characterID, new CharacterStatistics(character));
                        }
                    }
                }
            }
            Party.GetInstance().RoundHandler.GetCharacterTraitPhase().SendRequestForNextCharacter();
        }

        /// <summary>
        /// Sends the current gamestate to the client who requested for this
        /// </summary>
        /// <param name="msg"></param>
        public override void OnGameStateRequestMessage(GameStateRequestMessage msg)
        {

            if (Party.GetInstance().AreTwoPlayersRegistred())
            {
                Console.WriteLine("Beginning parsing GameStateRequest");

                //Requirement complete game state

                //First Step: Get active Players
                var activePlayersList = Party.GetInstance().GetActivePlayers();
                int[] activePlayerIds = new int[2] { activePlayersList.ElementAt(0).ClientID, activePlayersList.ElementAt(1).ClientID };
                List<Character> allCharacters = Party.GetInstance().GetAllCharacters();
                List<Character> aliveCharacters = allCharacters.Where(value => value.healthCurrent > 0 && !value.killedBySandworm).ToList<Character>();
                int historyOffset = 0;
                if (Party.GetInstance().RoundHandler.IsOverlengthMechanismActive)
                {
                    historyOffset += 1;
                }
                string[] history = new string[1 + aliveCharacters.Count + activePlayersList.Count + historyOffset + 1];

                MapChangeDemandMessage mapChangeDemandMessage = new MapChangeDemandMessage(MapChangeReasons.ROUND_PHASE, Party.GetInstance().map.fields, new Position(Party.GetInstance().RoundHandler.SandstormPhase.EyeOfStorm.XCoordinate, Party.GetInstance().RoundHandler.SandstormPhase.EyeOfStorm.ZCoordinate));


                history[0] = MessageConverter.FromMessage(mapChangeDemandMessage); //Append Map´_Change Message
                int clientID = 0;
                //Create Spawn of Characters
                for (int i = 0; i < aliveCharacters.Count; i++)
                {
                    var currentChar = allCharacters.ElementAt(i);
                    clientID = 0;
                    if (activePlayersList.ElementAt(0).UsedGreatHouse.Characters.Contains(currentChar))
                    {
                        clientID = activePlayersList.ElementAt(0).ClientID;
                    }
                    else if (activePlayersList.ElementAt(1).UsedGreatHouse.Characters.Contains(currentChar))
                    {
                        clientID = activePlayersList.ElementAt(1).ClientID;
                    }

                    history[1 + i] = MessageConverter.FromMessage(new SpawnCharacterDemandMessage(currentChar.greatHouse.City.clientID, currentChar.CharacterId, currentChar.CharacterName, new Position(currentChar.CurrentMapfield.XCoordinate, currentChar.CurrentMapfield.ZCoordinate), currentChar));

                }

                int offset = 1 + aliveCharacters.Count;
                foreach (var player in activePlayersList)
                {
                    history[offset] = MessageConverter.FromMessage(new ChangePlayerSpiceDemandMessage(player.ClientID, player.statistics.HouseSpiceStorage));
                    offset++;
                }
                if (Party.GetInstance().RoundHandler.IsOverlengthMechanismActive)
                {
                    history[offset] = MessageConverter.FromMessage(new EndGameMessage());
                    offset++;
                }

                clientID = 0;
                if (activePlayersList.ElementAt(0).UsedGreatHouse.Characters.Contains(Party.GetInstance().RoundHandler.GetCharacterTraitPhase().GetCurrentTurnCharacter()))
                {
                    clientID = activePlayersList.ElementAt(0).ClientID;
                }
                else if (activePlayersList.ElementAt(1).UsedGreatHouse.Characters.Contains(Party.GetInstance().RoundHandler.GetCharacterTraitPhase().GetCurrentTurnCharacter()))
                {
                    clientID = activePlayersList.ElementAt(1).ClientID;
                }

                history[offset] = MessageConverter.FromMessage(new TurnDemandMessage(clientID, Party.GetInstance().RoundHandler.GetCharacterTraitPhase().GetCurrentTurnCharacter().CharacterId));

                DoSendGameState(msg.clientID, activePlayerIds, history);
                Console.WriteLine("Sent GameState");
            }
            else
            {
                DoSendError(005, "Can't send gamestate because party is not running", msg.clientID.ToString());
            }
        }

        /// <summary>
        /// Request from the client to pause or unpause the current running game
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="sessionID"></param>
        public override void OnPauseGameRequestMessage(PauseGameRequestMessage msg, string sessionID)
        {
            // determine the client, who send this message
            Client requestingClient = Party.GetInstance().GetClientBySessionID(sessionID);

            // check, whether the requesting client is an ai, so reject the request
            if (requestingClient.IsActivePlayer && !requestingClient.IsAI)
            {
                int clientID = requestingClient.ClientID;

                // check, whether it is a "pause request" or a "resumption request"
                if (msg.pause)
                {
                    if (Party.GetInstance().RoundHandler.PauseGame(clientID))
                    {
                        Console.WriteLine("Session was paused");
                        Log.Information($"The party was paused by the client with the ID {clientID}.");
                        DoSendGamePauseDemand(true);
                    }
                    else
                    {
                        Log.Warning($"The pause request by the client with the ID {clientID} failed.");
                    }
                }
                else
                {
                    if (Party.GetInstance().RoundHandler.ContinueGame(clientID))
                    {
                        Console.WriteLine("Session was unpaused");
                        Log.Information($"The party was resumed by the client with the ID {clientID}.");
                        DoSendGamePauseDemand(false);
                    }
                    else
                    {
                        Log.Warning($"The resumption request by the client with the ID {clientID} failed.");
                    }
                }
            }
            else
            {
                Log.Warning("The requsting client is either an ai or a spectator and they are not allowed to request a pause!");
            }
        }

        /// <summary>
        /// sends a GAME_PAUSE_DEMAND message, so inform all clients whether the party was paused or will be resumed
        /// <param name="pause">Boolean whether the game was paused or unpaused</param>
        /// </summary>
        public void DoSendGamePauseDemand(bool pause)
        {
            // fetch the pause request and all it's information
            PauseRequest pauseRequest = Party.GetInstance().RoundHandler.PauseRequest;

            PausGameDemandMessage gamePauseDemandMessage = new PausGameDemandMessage(pauseRequest.ClientID, pause);
            NetworkController.HandleSendingMessage(gamePauseDemandMessage);
        }

        /// <summary>
        /// sends a UNPAUSE_GAME_OFFER message, so inform all clients, that the maximal pause time is exceeded and every player now
        /// can resume the game
        /// </summary>
        public void DoSendUnpauseGameOffer()
        {
            // fetch the pause request and all it's information
            PauseRequest pauseRequest = Party.GetInstance().RoundHandler.PauseRequest;

            UnpauseGameOfferMessage unpauseGameOfferMessage = new UnpauseGameOfferMessage(pauseRequest.ClientID);
            NetworkController.HandleSendingMessage(unpauseGameOfferMessage);
        }


        /// <summary>
        /// /// The server is sending a JoinAcceptedMessage if the join was successful
        /// </summary>
        /// <param name="clientSecret">Unique identifikator for the client, which is just known between the affected parties</param>
        public override void DoAcceptJoin(string clientSecret, int clientID, string sessionID)
        {
            JoinAcceptedMessage joinAcceptedMessage = new JoinAcceptedMessage(clientSecret, clientID);
            NetworkController.HandleSendingMessage(joinAcceptedMessage, sessionID);
            Log.Information("Join request of " + clientID + " was accepted");
        }

        /// <summary>
        /// sends the an error message to the client
        /// </summary>
        /// <param name="errorCode">the error code (see "Standardisierungsdokument")</param>
        /// <param name="errorDescription">a further description of the error</param>
        /// <param name="sessionID">the session id of the client, the message need to be send to</param>
        public override void DoSendError(int errorCode, string errorDescription, string sessionID)
        {
            ErrorMessage errorMessage = new ErrorMessage(errorCode, errorDescription);
            NetworkController.HandleSendingMessage(errorMessage, sessionID);
            Log.Debug("An error (code = " + errorCode + " ) occured: " + errorDescription);
        }

        /// <summary>
        /// sends the game configuration message
        /// </summary>
        /// <remarks>
        /// Therefore the server fetches
        /// <list type="bullet">
        /// <item>the configuration classes (scenario and party configuration)</item>
        /// <item>the client ids of both players and their cities (positions)</item>
        /// <item>the eye of the storm</item>
        /// </list>
        /// </remarks>
        public override void DoSendGameConfig()
        {
            // get the scenario loaded by the server
            List<List<string>> scenario = ScenarioConfiguration.GetInstance().scenario;

            // get the party configuration loaded by the server
            string partyConfiguration = JsonConvert.SerializeObject(PartyConfiguration.GetInstance());

            // get the client ids and their city positions
            int client0ID = Party.GetInstance().GetActivePlayers()[0].ClientID;
            int client1ID = Party.GetInstance().GetActivePlayers()[1].ClientID;

            // get cities of both players
            City cityPlayer0 = Party.GetInstance().GetActivePlayers()[0].City;
            City cityPlayer1 = Party.GetInstance().GetActivePlayers()[1].City;

            PlayerInfo[] playerInfo = new PlayerInfo[2];
            playerInfo[0] = new PlayerInfo(client0ID, Party.GetInstance().GetActivePlayers()[0].ClientName, cityPlayer0.XCoordinate, cityPlayer0.ZCoordinate);
            playerInfo[1] = new PlayerInfo(client1ID, Party.GetInstance().GetActivePlayers()[1].ClientName, cityPlayer1.XCoordinate, cityPlayer1.ZCoordinate);

            // get the eye of the storm
            MapField stormEyeField = Party.GetInstance().RoundHandler.SandstormPhase.EyeOfStorm;
            Position stormEye = new Position(stormEyeField.XCoordinate, stormEyeField.ZCoordinate);

            GameConfigMessage gameConfigMessage = new GameConfigMessage(scenario, PartyConfiguration.GetInstance(), playerInfo, stormEye); ;
            NetworkController.HandleSendingMessage(gameConfigMessage);
        }

        /// <summary>
        /// Sends the game configuration to a joined spectator
        /// </summary>
        /// <param name="spectatorID">ClientID of the spectator</param>
        public void DoSendGameConfigToSpectator(string spectatorID)
        {
            // get the scenario loaded by the server
            List<List<string>> scenario = ScenarioConfiguration.GetInstance().scenario;

            // get the client ids and their city positions
            int client0ID = Party.GetInstance().GetActivePlayers()[0].ClientID;
            int client1ID = Party.GetInstance().GetActivePlayers()[1].ClientID;

            // get cities of both players
            City cityPlayer0 = Party.GetInstance().GetActivePlayers()[0].City;
            City cityPlayer1 = Party.GetInstance().GetActivePlayers()[1].City;

            PlayerInfo[] playerInfo = new PlayerInfo[2];
            playerInfo[0] = new PlayerInfo(client0ID,"", cityPlayer0.XCoordinate, cityPlayer0.ZCoordinate);
            playerInfo[1] = new PlayerInfo(client1ID,"", cityPlayer1.XCoordinate, cityPlayer1.ZCoordinate);

            // get the eye of the storm
            MapField stormEyeField = Party.GetInstance().RoundHandler.SandstormPhase.EyeOfStorm;
            Position stormEye = new Position(stormEyeField.XCoordinate, stormEyeField.ZCoordinate);

            GameConfigMessage gameConfigMessage = new GameConfigMessage(scenario, PartyConfiguration.GetInstance(), playerInfo, stormEye);
            NetworkController.HandleSendingMessage(gameConfigMessage, spectatorID);
        }

        /// <summary>
        /// Sends the houseOffer to the client
        /// </summary>
        /// <param name="clientID">ID of the client</param>
        /// <param name="houses">The offered houses as an array</param>
        public override void DoSendHouseOffer(int clientID, GreatHouseType[] houses)
        {
            GreatHouse[] greatHouses = { GreatHouseFactory.CreateNewGreatHouse(houses[0]), GreatHouseFactory.CreateNewGreatHouse(houses[1]) };
            HouseOfferMessage houseOfferMessage = new HouseOfferMessage(clientID, greatHouses);
            NetworkController.HandleSendingMessage(houseOfferMessage);
        }

        /// <summary>
        /// Sends a ACK-Message to the client if the houseRequest succeed
        /// </summary>
        /// <param name="clientID">ID of the client</param>
        /// <param name="houseName">Name of the selected house</param>
        public override void DoSendHouseAck(int clientID, string houseName)
        {
            HouseAcknowledgementMessage houseACKMessage = new HouseAcknowledgementMessage(clientID, houseName);
            NetworkController.HandleSendingMessage(houseACKMessage);
        }

        /// <summary>
        /// Sends a demand message message to the client with the ID of the character whos turn it is
        /// </summary>
        /// <param name="clientID">ID of the client</param>
        /// <param name="characterID">ID of the character whos turn it is</param>
        public override void DoSendTurnDemand(int clientID, int characterID)
        {
            TurnDemandMessage turnDemandMessage = new TurnDemandMessage(clientID, characterID);
            NetworkController.HandleSendingMessage(turnDemandMessage);
        }

        /// <summary>
        /// Sends a demand message if a movement turn is over
        /// </summary>
        /// <param name="clientID">ID of the client</param>
        /// <param name="characterID">ID of the character who moved</param>
        /// <param name="path">The moved path of the character</param>
        public override void DoSendMovementDemand(int clientID, int characterID, List<Position> path)
        {
            MovementDemandMessage movementDemandMessage = new MovementDemandMessage(clientID, characterID, new Specs(null, path));
            NetworkController.HandleSendingMessage(movementDemandMessage);
        }

        /// <summary>
        /// Sends a demand message if a action turn is over
        /// </summary>
        /// <param name="clientID">ID of the client who executed the action</param>
        /// <param name="characterID">ID of the character who executed the action</param>
        /// <param name="action">The type of the executed action</param>
        /// <param name="target">The target of the executed action</param>
        public override void DoSendActionDemand(int clientID, int characterID, ActionType action, Position target)
        {
            ActionDemandMessage actionDemandMessage = new ActionDemandMessage(clientID, characterID, action, target);
            NetworkController.HandleSendingMessage(actionDemandMessage);
        }

        /// <summary>
        /// Sends a demand message if a transfer turn is over
        /// </summary>
        /// <param name="clientID">The id of the client of transferred spice</param>
        /// <param name="characterID">ID of the character of transferred spice</param>
        /// <param name="targetID">ID of the character who received spice</param>
        public override void DoSendTransferDemand(int clientID, int characterID, int targetID)
        {
            TransferDemandMessage transferDemandMessage = new TransferDemandMessage(clientID, characterID, targetID);
            NetworkController.HandleSendingMessage(transferDemandMessage);
        }

        /// <summary>
        /// Sends a message to the client with the updated values of a character
        /// </summary>
        /// <param name="clientID">ID of the client whos character got changed values</param>
        /// <param name="characterID">ID of the character whos values got changed</param>
        /// <param name="stats">The updated statistics of the character</param>
        public override void DoSendChangeCharacterStatsDemand(int clientID, int characterID, CharacterStatistics stats)
        {
            ChangeCharacterStatisticsDemandMessage changeCharacterStatisticsDemandMessage = new ChangeCharacterStatisticsDemandMessage(clientID, characterID, stats);
            NetworkController.HandleSendingMessage(changeCharacterStatisticsDemandMessage);
        }

        public override void DoSendMapChangeDemand(MapChangeReasons mapChangeReasons)
        {
            // get the new map from the party
            MapField[,] newMap = Party.GetInstance().map.fields;

            MapChangeDemandMessage mapChangeDemandMessage = new MapChangeDemandMessage(mapChangeReasons, newMap, new Position(Party.GetInstance().RoundHandler.SandstormPhase.EyeOfStorm.XCoordinate, Party.GetInstance().RoundHandler.SandstormPhase.EyeOfStorm.ZCoordinate));
            NetworkController.HandleSendingMessage(mapChangeDemandMessage);
        }

        public override void DoSendAtomicsUpdateDemand(int clientID, bool shunned, int atomicsLeft)
        {
            AtomicsUpdateDemandMessage atomicsUpdateDemandMessage = new AtomicsUpdateDemandMessage(clientID, shunned, atomicsLeft);
            NetworkController.HandleSendingMessage(atomicsUpdateDemandMessage);
        }

        /// <summary>
        /// Send a demand message to the client for a new spawned character
        /// </summary>
        /// <param name="attributes">The attributes of the character</param>
        public override void DoSpawnCharacterDemand(Character attributes)
        {
            // get both players
            Player player1 = Party.GetInstance().GetActivePlayers()[0];
            Player player2 = Party.GetInstance().GetActivePlayers()[1];

            int clientID = 0;

            // fetch the id of the client, whose character this is
            // therefore, check whether the character is from player 1
            if (player1.UsedGreatHouse.Characters.Contains(attributes))
            {
                clientID = player1.ClientID;
            }
            else if (player2.UsedGreatHouse.Characters.Contains(attributes))
            {
                clientID = player2.ClientID;
            }
            else
            {
                Log.Error("Cannot send SPAWN_CHARACTER_DEMAND, because the spawned character do not belong to any player!");
            }


            // get the position of the character
            Position position = new Position(attributes.CurrentMapfield.XCoordinate, attributes.CurrentMapfield.ZCoordinate);

            SpawnCharacterDemandMessage spawnCharacterDemandMessage = new SpawnCharacterDemandMessage(clientID, attributes.CharacterId, attributes.CharacterName, position, attributes);
            NetworkController.HandleSendingMessage(spawnCharacterDemandMessage);
        }

        /// <summary>
        /// Sends a message to the client with the updated spice value of player
        /// </summary>
        /// <param name="clientID">ID of the player whos spice value got changed</param>
        /// <param name="newSpiceVal">The new spice value of the player</param>
        public override void DoChangePlayerSpiceDemand(int clientID, int newSpiceVal)
        {
            ChangePlayerSpiceDemandMessage changePlayerSpiceDemandMessage = new ChangePlayerSpiceDemandMessage(clientID, newSpiceVal);
            NetworkController.HandleSendingMessage(changePlayerSpiceDemandMessage);
        }

        public override void DoSpawnSandwormDemand(int characterID, MapField mapField)
        {
            int x = mapField.XCoordinate;
            int y = mapField.ZCoordinate;
            Position position = new Position(x, y);

            // determine the client, whose character is targeted
            List<Player> players = Party.GetInstance().GetActivePlayers();
            Player playerWithCharacter = players.Find(player => player.UsedGreatHouse.Characters.Any(character => character.CharacterId == characterID));

            SandwormSpawnDemandMessage sandwormSpawnDemandMessage = new SandwormSpawnDemandMessage(playerWithCharacter.ClientID, characterID, position);
            NetworkController.HandleSendingMessage(sandwormSpawnDemandMessage);
        }

        public override void DoMoveSandwormDemand(List<MapField> list)
        {
            List<Position> path = new List<Position>();
            foreach (MapField mapField in list)
            {
                Position position = new Position(mapField.XCoordinate, mapField.ZCoordinate);
                path.Add(position);
            }
            SandwormMoveDemandMessage sandwormMoveDemandMessage = new SandwormMoveDemandMessage(path);
            NetworkController.HandleSendingMessage(sandwormMoveDemandMessage);
        }

        public override void DoDespawnSandwormDemand()
        {
            SandwormDespawnDemandMessage sandwormDespawnDemandMessage = new SandwormDespawnDemandMessage();
            NetworkController.HandleSendingMessage(sandwormDespawnDemandMessage);
        }

        /// <summary>
        /// This method will be called, when the overlengthmechanism is aktive.
        /// </summary>
        public override void DoEndGame()
        {
            EndGameMessage endGameMessage = new EndGameMessage();
            NetworkController.HandleSendingMessage(endGameMessage);
        }

        /// <summary>
        /// This message will be sent to the clients when the game ends.
        /// </summary>
        /// <param name="winnerID">ID of the winner of the party</param>
        /// <param name="loserID">ID of the loser of the party</param>
        /// <param name="stats">Repräsentation of the statistics of the Game</param>
        public override void DoGameEndMessage(int winnerID, int loserID, Statistics[] stats)
        {
            GameEndMessage gameEndMessage = new GameEndMessage(winnerID, loserID, stats);
            NetworkController.HandleSendingMessage(gameEndMessage);
        }

        public override void DoSendGameState(int clientID, int[] activlyPlayingIDs, String[] history)
        {
            GameStateMessage gameStateMessage = new GameStateMessage(history, activlyPlayingIDs, clientID);
            NetworkController.HandleSendingMessage(gameStateMessage, Party.GetInstance().GetSessionIDbyClientID(clientID));
        }

        /// <summary>
        /// increases the amount of strikes of a client and sends a strike message
        /// </summary>
        /// <param name="clientID">the id of the player, who gets a strike</param>
        /// <param name="wrongMessage">the wrong message, who was send by the client</param>
        public override void DoSendStrike(int clientID, Message wrongMessage)
        {
            string wrongMessageAsString = MessageConverter.FromMessage(wrongMessage);
            Player playerForStrike;
            foreach (var player in Party.GetInstance().GetActivePlayers())
            {
                if (player.ClientID == clientID)
                {
                    playerForStrike = player;
                    playerForStrike.AddStrike();
                    StrikeMessage strikeMessage = new StrikeMessage(clientID, wrongMessageAsString, playerForStrike.AmountOfStrikes);
                    NetworkController.HandleSendingMessage(strikeMessage);
                    return;
                }
            }
            Log.Error($"No player with clientID = {clientID} found.");
        }

        public override void DoGamePauseDemand(int requestedByClientID, bool pause)
        {
            PausGameDemandMessage gamePauseDemandMessage = new PausGameDemandMessage(requestedByClientID, pause);
            NetworkController.HandleSendingMessage(gamePauseDemandMessage);
        }

        public void DoSendHeliDemand(int clientID, int characterID, Position target, bool crash)
        {
            HeliDemandMessage heliDemandMessage = new HeliDemandMessage(clientID, characterID, target, crash);
            NetworkController.HandleSendingMessage(heliDemandMessage);
        }


        public override void OnUnpauseGameOffer(int requestedByClientID)
        {
            UnpauseGameOfferMessage unpauseGameOfferMessage = new UnpauseGameOfferMessage(requestedByClientID);
            NetworkController.HandleSendingMessage(unpauseGameOfferMessage);
        }

        public override void OnHeliRequestMessage(HeliRequestMessage heliRequestMessage)
        {
            Player activePlayer = null;
            foreach (var player in Party.GetInstance().GetActivePlayers())
            {
                if (player.ClientID == heliRequestMessage.clientID)
                {
                    activePlayer = player;
                }
            }

            if (activePlayer == null)
            {
                throw new NullReferenceException($"Requested player with {heliRequestMessage.clientID} isn't known!");
            }

            //get the character which should be moved
            Character portingChar = null;
            foreach (var character in activePlayer.UsedGreatHouse.GetCharactersAlive())
            {
                if (character.CharacterId == heliRequestMessage.characterID)
                {
                    portingChar = character;
                }
            }

            if (portingChar == null)
            {
                DoSendError(005, $"Moving character is null", activePlayer.SessionID);
                return;
            }


            Party party = Party.GetInstance();


            if (heliRequestMessage.target.x >= 0 && heliRequestMessage.target.x < party.map.MAP_WIDTH && heliRequestMessage.target.y >= 0 && heliRequestMessage.target.y < party.map.MAP_HEIGHT) //check if movement is in bounds of the map
            {
                //check if movement is on walkable terrain
                if (party.map.fields[heliRequestMessage.target.y, heliRequestMessage.target.x].tileType.Equals(TileType.HELIPORT.ToString()))
                {
                    
                    Random rdm = new Random();
                    bool crash;
                    MapField targetField;
                    //Only if flying through storm
                    //TODO insert crash-probability from config
                    if (rdm.NextDouble() < PartyConfiguration.GetInstance().crashProbability && Map.instance.HasSandstormOnPath(portingChar.CurrentMapfield, heliRequestMessage.target))
                    {
                        //Crash
                        targetField = Map.instance.GetRandomApproachableField();
                        Map.instance.SpreadSpiceOnFields(targetField, portingChar.inventoryUsed);
                        portingChar.inventoryUsed = 0;
                        crash = true;
                    }
                    else
                    {
                        //no Crash
                        if (!party.map.fields[heliRequestMessage.target.y, heliRequestMessage.target.x].IsCharacterStayingOnThisField)  
                        {
                            targetField = Map.instance.GetMapFieldAtPosition(heliRequestMessage.target.x, heliRequestMessage.target.y);
                        }
                        else
                        {
                            //Character staying on target field. Choose random Neighbour
                            targetField = Map.instance.GetRandomApproachableNeighborField(party.map.fields[heliRequestMessage.target.y, heliRequestMessage.target.x]);
                        }
                        crash = false;
                    }
                    portingChar.CurrentMapfield.DisplaceCharacter(portingChar);
                    portingChar.CurrentMapfield = targetField;
                    targetField.PlaceCharacter(portingChar);

                    DoSendHeliDemand(activePlayer.ClientID, heliRequestMessage.characterID, new Position(targetField.XCoordinate, targetField.ZCoordinate), crash);

                    if(crash)
                    {
                        DoSendMapChangeDemand(MapChangeReasons.ROUND_PHASE);
                        DoSendChangeCharacterStatsDemand(activePlayer.ClientID, portingChar.CharacterId, new CharacterStatistics(portingChar));
                    }

                }
            }
        }

        public override void OnJoinAccepted(JoinAcceptedMessage msg)
        {
            throw new NotImplementedException();
        }

        // the server should not use this method
        public override void OnJoinAcceptedMessage(JoinAcceptedMessage joinAcceptedMessage)

        {
            throw new NotImplementedException();
        }


        public override void DoSendJoin(string clientName) { }

        // the server should not use this method
        public override void OnGameConfigMessage(GameConfigMessage gameConfigMessage)
        {
            throw new NotImplementedException();
        }

        // the server should not use this method
        public override void OnMapChangeDemandMessage(MapChangeDemandMessage mapChangeDemandMessage)
        {
            throw new NotImplementedException();
        }

        // the server should not use this method
        public override void OnStrikeMessage(StrikeMessage strikeMessage)
        {
            throw new NotImplementedException();
        }

        // the server should not use this method
        public override void OnGameEndMessage(GameEndMessage gameEndMessage)
        {
            throw new NotImplementedException();
        }

        // the server should not use this method
        public override void OnGameStateMessage(GameStateMessage gameStateMessage)
        {
            throw new NotImplementedException();
        }

        // the server should not use this method
        public override void OnPauseGameDemandMessage(GamePauseDemandMessage gamePauseDemandMessage)
        {
            throw new NotImplementedException();
        }

        // the server should not use this method
        public override void OnHouseOfferMessage(HouseOfferMessage houseOfferMessage)
        {
            throw new NotImplementedException();
        }

        // the server should not use this method
        public override void OnHouseAcknowledgementMessage(HouseAcknowledgementMessage houseAcknowledgementMessage)
        {
            throw new NotImplementedException();
        }

        // the server should not use this method
        public override void OnTurnDemandMessage(TurnDemandMessage turnDemandMessage)
        {
            throw new NotImplementedException();
        }

        // the server should not use this method
        public override void OnMovementDemandMessage(MovementDemandMessage movementDemandMessage)
        {
            throw new NotImplementedException();
        }

        // the server should not use this method
        public override void OnActionDemandMessage(ActionDemandMessage actionDemandMessage)
        {
            throw new NotImplementedException();
        }

        // the server should not use this method
        public override void OnChangeCharacterStatisticsDemandMessage(ChangeCharacterStatisticsDemandMessage changeCharacterStatisticsDemandMessage)
        {
            throw new NotImplementedException();
        }

        // the server should not use this method
        public override void OnSpawnCharacterDemandMessage(SpawnCharacterDemandMessage spawnCharacterDemandMessage)
        {
            throw new NotImplementedException();
        }

        // the server should not use this method
        public override void OnChangePlayerSpiceDemandMessage(ChangePlayerSpiceDemandMessage changePlayerSpiceDemandMessage)
        {
            throw new NotImplementedException();
        }

        // the server should not use this method
        public override void OnSandwormSpawnDemandMessage(SandwormSpawnDemandMessage sandwormSpawnDemandMessage)
        {
            throw new NotImplementedException();
        }

        // the server should not use this method
        public override void OnSandwormMoveDemandMessage(SandwormMoveDemandMessage sandwormMoveMessage)
        {
            throw new NotImplementedException();
        }

        // the server should not use this method
        public override void OnSandwormDespawnMessage(SandwormDespawnDemandMessage sandwormDespawnDemandMessage)
        {
            throw new NotImplementedException();
        }

        // the server should not use this method
        public override void OnEndGameMessage(EndGameMessage endGameMessage)
        {
            throw new NotImplementedException();
        }

        // the server should not use this method
        public override void OnTransferDemandMessage(TransferDemandMessage transferDemandMessage)
        {
            throw new NotImplementedException();
        }

        // the server should not use this method
        public override void OnAtomicsUpdateDemandMessage(AtomicsUpdateDemandMessage atomicUpdateDemandMessage)
        {
            throw new NotImplementedException();
        }

        public override void OnUnpauseOfferDemand(UnpauseGameOfferMessage unpauseGameOfferMessage)
        {
            throw new NotImplementedException();
        }

        public override void OnHeliDemandMessage(HeliDemandMessage msg)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Executes the Sword spin attack
        /// </summary>
        /// <param name="msg">The ActionRequestMessage</param>
        /// <param name="activePlayer">The active player</param>
        /// <param name="actionCharacter">The character who executes the action</param>
        /// <param name="action">The action type</param>
        /// <param name="map">The current map</param>
        /// <param name="charactersHit">The List of hittet characters</param>
        /// <returns>The list of hittet characters</returns>
        private List<Character> ExecuteSwordSpin(ActionRequestMessage msg, Player activePlayer, Character actionCharacter, ActionType action, Map map)
        {
            var charactersHit = new List<Character>();
            if (actionCharacter.APcurrent == actionCharacter.APmax
                                            && actionCharacter.characterType == Enum.GetName(typeof(CharacterType), CharacterType.FIGHTER))
            {
                DoSendActionDemand(msg.clientID, msg.characterID, action, msg.specs.target);
                charactersHit = actionCharacter.SwordSpin(map);
                foreach (var character in charactersHit)
                {
                    if (character.IsDead())
                    {
                        activePlayer.statistics.AddToEnemiesDefeated(1);
                    }
                }
            }

            return charactersHit;
        }

        /// <summary>
        /// Executes the voice attack
        /// </summary>
        /// <param name="msg">The ActionRequestMessage</param>
        /// <param name="activePlayer">The active player</param>
        /// <param name="targetCharacter">The character who gets attacked</param>
        /// <param name="actionCharacter">The character who executes the action</param>
        /// <param name="action">The action type</param>
        /// <param name="map">The current map</param>
        /// <param name="charactersHit">The list of hittet characters</param>
        private void ExecuteVoice(ActionRequestMessage msg, Player activePlayer, Character targetCharacter, Character actionCharacter, ActionType action, Map map, List<Character> charactersHit)
        {
            if (targetCharacter == null)
            {
                foreach (var character in activePlayer.UsedGreatHouse.GetCharactersAlive())
                {
                    if (msg.specs.target != null
                        && character.CurrentMapfield.XCoordinate == msg.specs.target.x
                        && character.CurrentMapfield.ZCoordinate == msg.specs.target.y)
                    {
                        targetCharacter = character;
                    }
                }
            }
            if (targetCharacter == null)
            {
                DoSendError(005, "Target character is null when voice action is executed!", activePlayer.SessionID);
            }
            if (actionCharacter.APcurrent == actionCharacter.APmax
                && !targetCharacter.IsInSandStorm(map)
                && actionCharacter.characterType == Enum.GetName(typeof(CharacterType), CharacterType.BENE_GESSERIT))
            {
                DoSendActionDemand(msg.clientID, msg.characterID, action, msg.specs.target);
                int InventoryUsedBeforeVoice = actionCharacter.inventoryUsed;
                actionCharacter.Voice(targetCharacter);
                activePlayer.statistics.AddToTotalSpiceCollected(actionCharacter.inventoryUsed - InventoryUsedBeforeVoice);
                charactersHit.Add(targetCharacter);
                TryDeliverSpiceToCity(activePlayer, actionCharacter);
            }
        }

        /// <summary>
        /// Executes the spice hoarding attack
        /// </summary>
        /// <param name="msg">The ActionRequestMessage</param>
        /// <param name="activePlayer">The active player</param>
        /// <param name="actionCharacter">The character who executes the action</param>
        /// <param name="action">The action type</param>
        /// <param name="map">The current map</param>
        private void ExecuteSpiceHoarding(ActionRequestMessage msg, Player activePlayer, Character actionCharacter, ActionType action, Map map)
        {
            if (actionCharacter.APcurrent == actionCharacter.APmax
                                            && actionCharacter.characterType == Enum.GetName(typeof(CharacterType), CharacterType.MENTAT))
            {
                DoSendActionDemand(msg.clientID, msg.characterID, action, msg.specs.target);
                int inventoryUsedBeforeSpiceHoarding = actionCharacter.inventoryUsed;
                actionCharacter.SpiceHoarding(map);
                activePlayer.statistics.AddToTotalSpiceCollected(actionCharacter.inventoryUsed - inventoryUsedBeforeSpiceHoarding);
                DoSendMapChangeDemand(MapChangeReasons.ROUND_PHASE);
                //deliver spice to city if city is neighborfield
                TryDeliverSpiceToCity(activePlayer, actionCharacter);
            }
        }

        /// <summary>
        /// Executes the family atomics action
        /// </summary>
        /// <param name="msg">The ActionRequestMessage</param>
        /// <param name="activePlayer">The active player</param>
        /// <param name="enemyPlayer">The enemy player</param>
        /// <param name="actionCharacter">The action character</param>
        /// <param name="action">The action type</param>
        /// <param name="map">The current map</param>
        /// <param name="charactersHit">The list of hittet characters</param>
        /// <returns>The list of hittet characters</returns>
        private List<Character> ExecuteAtomics(ActionRequestMessage msg, Player activePlayer, Player enemyPlayer, Character actionCharacter, ActionType action, Map map)
        {
            var charactersHit = new List<Character>();
            if (actionCharacter.APcurrent == actionCharacter.APmax
                                            && actionCharacter.characterType == Enum.GetName(typeof(CharacterType), CharacterType.NOBLE))
            {
                DoSendActionDemand(msg.clientID, msg.characterID, action, msg.specs.target);
                //get the mapfield where the active Character aims to
                MapField targetMapField = null;
                foreach (var mapfield in map.fields)
                {
                    if (mapfield.XCoordinate == msg.specs.target.x && mapfield.ZCoordinate == msg.specs.target.y)
                    {
                        targetMapField = mapfield;
                    }
                }
                if (targetMapField == null)
                {
                    DoSendError(005, "TargetMapField is null when FamilyAtomics was executed!", activePlayer.SessionID);
                }

                // if atomic bomb is thrown on sandworm or neighborfield of sandworm then remove the sandworm
                if (Sandworm.GetSandworm() != null)
                {
                    foreach (var mapField in map.GetNeighborFields(targetMapField))
                    {
                        if (Sandworm.GetSandworm().GetCurrentField().Equals(mapField) || Sandworm.GetSandworm().GetCurrentField().Equals(targetMapField))
                        {
                            Sandworm.Despawn(this);
                            break;
                        }
                    }
                }
                bool greathouseConventionBrokenBeforeAtomicBomb = Noble.greatHouseConventionBroken;
                charactersHit = actionCharacter.AtomicBomb(targetMapField, map, Noble.greatHouseConventionBroken, activePlayer.UsedGreatHouse, enemyPlayer.UsedGreatHouse);
                activePlayer.statistics.AddToEnemiesDefeated(charactersHit.Count);
                DoSendMapChangeDemand(MapChangeReasons.FAMILY_ATOMICS);
                if (greathouseConventionBrokenBeforeAtomicBomb != Noble.greatHouseConventionBroken)
                {
                    DoSendAtomicsUpdateDemand(msg.clientID, true, actionCharacter.greatHouse.unusedAtomicBombs);        //character which threw the atomic is shunned now
                }
                else
                {
                    DoSendAtomicsUpdateDemand(msg.clientID, false, actionCharacter.greatHouse.unusedAtomicBombs);       //character which threw the atomic is not shunned
                }
            }

            return charactersHit;
        }

        /// <summary>
        /// Executes the kanly action of a character
        /// </summary>
        /// <param name="msg">The ActionRequestMessage</param>
        /// <param name="activePlayer">The active player</param>
        /// <param name="targetCharacter">The target character</param>
        /// <param name="actionCharacter">The character who executes the action</param>
        /// <param name="action">The action type</param>
        /// <param name="map">The current map</param>
        /// <param name="charactersHit">The list of hittet characters</param>
        private void ExecuteKanlyAction(ActionRequestMessage msg, Player activePlayer, Character targetCharacter, Character actionCharacter, ActionType action, Map map, List<Character> charactersHit)
        {
            Random rnd = new Random();
            double success = rnd.NextDouble();
            if (targetCharacter == null)
            {
                DoSendError(005, "Target character is null when kanly action is executed!", activePlayer.SessionID);
            }
            else if (actionCharacter.APcurrent == actionCharacter.APmax
                && actionCharacter.characterType == Enum.GetName(typeof(CharacterType), CharacterType.NOBLE)
                && targetCharacter.characterType == Enum.GetName(typeof(CharacterType), CharacterType.NOBLE)
                && !targetCharacter.IsInSandStorm(map))
            {
                targetCharacter.SpentAp(targetCharacter.APcurrent);
                if (success < PartyConfiguration.GetInstance().kanlySuccessProbability)
                {
                    DoSendActionDemand(msg.clientID, msg.characterID, action, msg.specs.target);
                    actionCharacter.Kanly(targetCharacter);
                    activePlayer.statistics.AddToEnemiesDefeated(1);
                    charactersHit.Add(targetCharacter);
                }
            }
        }

        /// <summary>
        /// Executes the collect spice action from a character.
        /// </summary>
        /// <param name="msg">The ActionRequestMessage</param>
        /// <param name="activePlayer">The active player</param>
        /// <param name="actionCharacter">The character who executes the action</param>
        /// <param name="action">The action type</param>
        /// <param name="map">The current map</param>
        private void ExecuteCollectSpice(ActionRequestMessage msg, Player activePlayer, Character actionCharacter, ActionType action)
        {
            DoSendActionDemand(msg.clientID, msg.characterID, action, msg.specs.target);
            actionCharacter.CollectSpice();
            if (actionCharacter.CurrentMapfield.tileType == "DUNE" || actionCharacter.CurrentMapfield.tileType == "FLAT_SAND")
            {
                actionCharacter.SetLoud();
            }
            activePlayer.statistics.AddToTotalSpiceCollected(1);
            DoSendMapChangeDemand(MapChangeReasons.ROUND_PHASE);
            TryDeliverSpiceToCity(activePlayer, actionCharacter);
        }

        /// <summary>
        /// This method executes the basic attack in OnActionRequestMessage
        /// </summary>
        /// <param name="msg">The ActionRequestMessage</param>
        /// <param name="activePlayer">The active player</param>
        /// <param name="actionCharacter">The action character</param>
        /// <param name="targetCharacter">The target character</param>
        /// <param name="action">The ActionType</param>
        /// <param name="map">The current map</param>
        /// <param name="charactersHit">The List of hittet characters</param>
        private void ExecuteAttack(ActionRequestMessage msg, Player activePlayer, Character actionCharacter, Character targetCharacter, ActionType action, Map map, List<Character> charactersHit)
        {
            if (targetCharacter != null && !targetCharacter.IsInSandStorm(map))
            {
                DoSendActionDemand(msg.clientID, msg.characterID, action, msg.specs.target);
                actionCharacter.Attack(targetCharacter);
                if (targetCharacter.IsDead()) // if enemy dies from attack, update statistics
                {
                    activePlayer.statistics.AddToEnemiesDefeated(1);
                }
                charactersHit.Add(targetCharacter);
            }
        }
    }
}