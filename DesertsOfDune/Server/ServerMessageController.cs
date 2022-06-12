using System;
using System.Collections.Generic;
using GameData.network.controller;
using GameData.network.messages;
using GameData.network.util.world;
using Server.Clients;
using Serilog;
using Server.ClientManagement.Clients;
using GameData.network.util.enums;
using GameData.network.util.world.greatHouse;
using System.Linq;
using GameData.network.util.parser;
using Server.Configuration;
using Newtonsoft.Json;

namespace Server
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
        /// TODO: handle reconnect
        public override void OnJoinMessage(JoinMessage msg, string sessionID)
        {
            Client client;

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
            if (Party.GetInstance().AreTwoPlayersRegistred())
            {
                Party.GetInstance().PrepareGame();
                DoSendGameConfig();
            }
        }

        /// <summary>
        /// If a client loose connection to the server, he can rejoin to the game with the clientSecret from the JoinAcceptedMessage.
        /// </summary>
        /// <param name="msg">RejoinMessage with clientSecretParameter.</param>
        public override void OnRejoinMessage(RejoinMessage msg, string sessionID)
        {
            var connectedClients = Party.GetInstance().GetConnectedClients();
            bool rejoinSuccessful = false;
            foreach (var client in connectedClients)
            {
                if (client.ClientSecret == msg.ClientSecret)
                {
                    client.SessionID = sessionID; //new sessionID for the rejoined client
                    if (client.IsActivePlayer)
                    {
                        // TODO: send current game stats or gamestats what player has missed
                    }
                    rejoinSuccessful = true;
                    Log.Information($"Rejoin of client: {client.ClientName} was successful.");
                }
            }
            if (!rejoinSuccessful)
            {
                //disconnect the client
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
            //request from client to move a character

            //get the player who wants to move his character
            Player activePlayer = null;
            foreach (var player in Party.GetInstance().GetActivePlayers())
            {
                if (player.ClientID == msg.clientID)
                {
                    activePlayer = player;
                }
            }
            /**if(activePlayer == null)
            {
                DoSendError(005, $"No Player with clientID = {msg.clientID} known.", sessionID);
                return;
            }*/

            //get the character which should be moved
            Character movingCharacter = null;
            foreach (var character in activePlayer.UsedGreatHouse.GetCharactersAlive())
            {
                if (character.CharacterId == msg.characterID)
                {
                    movingCharacter = character;
                }
            }
            /**if(movingCharacter == null)
            {
                DoSendError(005, $"Moving character is null", sessionID);
                return;
            }*/

            //List<Position> path = new List<Position>();
            List<Position> path = msg.specs.path;
            foreach (var position in path)
            {
                var party = Party.GetInstance();
                //check if Character has enough Movement Points
                if (movingCharacter.MPcurrent > 0)
                {
                    //check if movement is in bounds of the map
                    if (position.x >= 0 && position.x < party.map.MAP_WIDTH && position.y >= 0 && position.y < party.map.MAP_HEIGHT)
                    {
                        //check if movement is on walkable terrain
                        if (party.map.fields[position.x, position.y].tileType != "Mountain" && party.map.fields[position.x, position.y].tileType != "City") //check needed and not implemented utils
                        {
                            movingCharacter.Movement(movingCharacter.CurrentMapfield, party.map.fields[position.x, position.y]); //move character 1 field along its path
                            path.Add(position);
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
            DoSendMovementDemand(msg.clientID, msg.characterID, path);
        }

        /// <summary>
        /// executed, when a player want to do a action with his character while it's his turn
        /// </summary>
        /// <param name="msg">contains informations about the player, the character he wants to do a action with and the action he wants his character to do</param>
        public override void OnActionRequestMessage(ActionRequestMessage msg)
        {

            //request from client to run an action

            //get the player who wants to do the action
            Player activePlayer = null;
            Player enemyPlayer = null;
            foreach (var player in Party.GetInstance().GetActivePlayers())
            {
                if (player.ClientID == msg.clientID)
                {
                    activePlayer = player;
                }
                if (player.ClientID == msg.clientID)
                {
                    enemyPlayer = player;
                }

            }

            /**if(activePlayer == null)
            {
                DoSendError(005, $"No Player with clientID = {msg.clientID} known.", sessionID);
                return;
            }*/

            //get the characters which are involved in the action
            Character actionCharacter = null;
            Character targetCharacter = null;
            bool friendlyFire = false;
            foreach (var character in activePlayer.UsedGreatHouse.GetCharactersAlive())
            {
                if (character.CharacterId == msg.characterID)
                {
                    actionCharacter = character;
                }
                if (character.CurrentMapfield.stormEye == msg.specs.target)
                {
                    targetCharacter = character;
                    friendlyFire = true;            //characters can not attack their allys
                }
            }
            //get the target character from enemy player if the target character is not an ally
            if (targetCharacter == null)
            {
                foreach (var character in enemyPlayer.UsedGreatHouse.GetCharactersAlive())
                {
                    if (character.CurrentMapfield.stormEye == msg.specs.target)
                    {
                        targetCharacter = character;
                    }
                }
            }

            /**if (actionCharacter == null)
            {
                DoSendError(005, "ActionCharacter is null", sessionID)
            }*/

            //set Attack as standard enum and change it if needed
            ActionType action = ActionType.ATTACK;

            if (actionCharacter.APcurrent > 0)
            {
                //check which action the player wants to do with his character
                switch (Enum.Parse(typeof(ActionType), msg.action))
                {
                    case ActionType.ATTACK:
                        action = ActionType.ATTACK;
                        if (!friendlyFire)
                        {
                            actionCharacter.Atack(targetCharacter);
                        }
                        break;
                    case ActionType.COLLECT:
                        action = ActionType.COLLECT;
                        actionCharacter.CollectSpice();
                        break;
                    //check in every special action if the character is from the right character type to do the special aciton and check if his ap is full
                    case ActionType.KANLY:
                        action = ActionType.KANLY;
                        if (actionCharacter.APcurrent == actionCharacter.APmax
                            && actionCharacter.characterType == Enum.GetName(typeof(CharacterType), CharacterType.NOBLE)
                            && targetCharacter.characterType == Enum.GetName(typeof(CharacterType), CharacterType.NOBLE)
                            && !friendlyFire)
                        {
                            actionCharacter.Kanly(targetCharacter);
                        }
                        break;
                    case ActionType.FAMILY_ATOMICS:
                        action = ActionType.FAMILY_ATOMICS;
                        if (actionCharacter.APcurrent == actionCharacter.APmax
                            && actionCharacter.characterType == Enum.GetName(typeof(CharacterType), CharacterType.NOBLE))
                        {
                            //get the mapfield where the active Character aims to
                            MapField targetMapField = null;
                            foreach (var mapfield in Party.GetInstance().map.fields)
                            {
                                if (mapfield.stormEye == msg.specs.target)
                                {
                                    targetMapField = mapfield;
                                }
                            }

                            actionCharacter.AtomicBomb(targetMapField, Party.GetInstance().map);
                        }
                        break;
                    case ActionType.SPICE_HORDING:
                        action = ActionType.SPICE_HORDING;
                        if (actionCharacter.APcurrent == actionCharacter.APmax
                            && actionCharacter.characterType == Enum.GetName(typeof(CharacterType), CharacterType.MENTAT))
                        {
                            actionCharacter.SpiceHoarding(Party.GetInstance().map);
                        }
                        break;
                    case ActionType.VOICE:
                        action = ActionType.VOICE;
                        if (actionCharacter.APcurrent == actionCharacter.APmax
                            && actionCharacter.characterType == Enum.GetName(typeof(CharacterType), CharacterType.BENEGESSERIT))
                        {
                            actionCharacter.Voice(targetCharacter);
                        }
                        break;
                    case ActionType.SWORD_SPIN:
                        action = ActionType.SWORD_SPIN;
                        if (actionCharacter.APcurrent == actionCharacter.APmax
                            && actionCharacter.characterType == Enum.GetName(typeof(CharacterType), CharacterType.FIGHTER))
                        {
                            actionCharacter.SwordSpin(Party.GetInstance().map);
                        }
                        break;
                    default:
                        throw new ArgumentException($"Actiontype {msg.action} not supoorted here.");
                }
            }
            DoSendActionDemand(msg.clientID, msg.characterID, action, msg.specs.target);
        }

        /// <summary>
        /// executed if the player wants to transfer spice from one character to another
        /// </summary>
        /// <param name="msg">contains information about the player who wants to transfer spice, the character who already has the spice, the character who should get the spice and the amount of spice he wants to transfer</param>
        public override void OnTransferRequestMessage(TransferRequestMessage msg)
        {
            //get the player who wants to do the transfer
            Player activePlayer = null;
            foreach (var player in Party.GetInstance().GetActivePlayers())
            {
                if (player.ClientID == msg.clientID)
                {
                    activePlayer = player;
                }
            }

            //get the characters which are involved in the transfer
            Character activeCharacter = null;
            Character targetCharacter = null;
            foreach (var character in activePlayer.UsedGreatHouse.Characters)
            {
                if (character.CharacterId == msg.characterID)
                {
                    activeCharacter = character;
                }
                if (character.CharacterId == msg.targetID)
                {
                    targetCharacter = character;
                }
            }

            //get the postion of the characters and check if they stand next to each other
            int activeCharacterX = activeCharacter.CurrentMapfield.stormEye.x;
            int activeCharacterY = activeCharacter.CurrentMapfield.stormEye.y;
            int targetCharacterX = targetCharacter.CurrentMapfield.stormEye.x;
            int targetCharacterY = targetCharacter.CurrentMapfield.stormEye.y;
            if ((activeCharacterX == targetCharacterX && (activeCharacterY == (targetCharacterY + 1)) || (activeCharacterY == targetCharacterY - 1)) || (activeCharacterY == targetCharacterY && ((activeCharacterX == targetCharacterX + 1) || (activeCharacterX == targetCharacterX - 1))))
            {
                activeCharacter.GiftSpice(targetCharacter, msg.amount);
                DoSendTransferDemand(msg.clientID, msg.characterID, msg.targetID);
            }
        }

        public override void OnEndTurnRequestMessage(EndTurnRequestMessage msg)
        {
            throw new NotImplementedException("not implemented");

            //End move phase prematurely

            //int clientID
            //int characterID
        }

        public override void OnGameStateRequestMessage(GameStateRequestMessage msg)
        {
            throw new NotImplementedException("not implemented");

            //Requirement complete game state

            //int clientID
        }

        public override void OnPauseGameRequestMessage(PauseGameRequestMessage msg)
        {
            throw new NotImplementedException("not implemented");

            //request for pause from client

            //bool pause
        }



        /// <summary>
        /// /// The server is sending a JoinAcceptedMessage if the join was successful
        /// TODO: what is sending back if a exception is thrown?
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

        public override void DoSendGameConfig()
        {
            int client0ID = Party.GetInstance().GetActivePlayers()[0].ClientID;
            int client1ID = Party.GetInstance().GetActivePlayers()[1].ClientID;

            List<List<string>> scenario = ScenarioConfiguration.GetInstance().scenario;
            string partyConfiguration = JsonConvert.SerializeObject(PartyConfiguration.GetInstance());
            PartyReference partyReference = new PartyReference(partyConfiguration);

            GameConfigMessage gameConfigMessage = new GameConfigMessage(scenario, partyReference, null, null);
            NetworkController.HandleSendingMessage(gameConfigMessage);
        }

        public override void DoSendHouseOffer(int clientID, GreatHouseType[] houses)
        {
            GreatHouse[] greatHouses = { GreatHouseFactory.CreateNewGreatHouse(houses[0]), GreatHouseFactory.CreateNewGreatHouse(houses[1]) };
            HouseOfferMessage houseOfferMessage = new HouseOfferMessage(clientID, greatHouses);
            NetworkController.HandleSendingMessage(houseOfferMessage);
        }

        public override void DoSendHouseAck(int clientID, string houseName)
        {
            HouseAcknowledgementMessage houseACKMessage = new HouseAcknowledgementMessage(clientID, houseName);
            NetworkController.HandleSendingMessage(houseACKMessage);
        }

        public override void DoSendTurnDemand(int clientID, int characterID)
        {
            TurnDemandMessage turnDemandMessage = new TurnDemandMessage(clientID, characterID);
            NetworkController.HandleSendingMessage(turnDemandMessage);
        }

        public override void DoSendMovementDemand(int clientID, int characterID, List<Position> path)
        {
            MovementDemandMessage movementDemandMessage = new MovementDemandMessage(clientID, characterID, path);
            NetworkController.HandleSendingMessage(movementDemandMessage);
        }

        public override void DoSendActionDemand(int clientID, int characterID, ActionType action, Position target)
        {
            ActionDemandMessage actionDemandMessage = new ActionDemandMessage(clientID, characterID, action, target);
            NetworkController.HandleSendingMessage(actionDemandMessage);
        }

        public override void DoSendTransferDemand(int clientID, int characterID, int targetID)
        {
            TransferDemandMessage transferDemandMessage = new TransferDemandMessage(clientID, characterID, targetID);
            NetworkController.HandleSendingMessage(transferDemandMessage);
        }

        public override void DoSendChangeCharacterStatsDemand(int clientID, int characterID, CharacterStatistics stats)
        {
            ChangeCharacterStatisticsDemandMessage changeCharacterStatisticsDemandMessage = new ChangeCharacterStatisticsDemandMessage(clientID, characterID, stats);
            NetworkController.HandleSendingMessage(changeCharacterStatisticsDemandMessage);
        }

        public override void DoSendMapChangeDemand(MapChangeReasons mapChangeReasons, MapField[,] newMap)
        {
            MapChangeDemandMessage mapChangeDemandMessage = new MapChangeDemandMessage(mapChangeReasons, newMap);
            NetworkController.HandleSendingMessage(mapChangeDemandMessage);
        }

        public override void DoSendAtomicsUpdateDemand(int clientID, bool shunned, int atomicsLeft)
        {
            AtomicsUpdateDemandMessage atomicsUpdateDemandMessage = new AtomicsUpdateDemandMessage(clientID, shunned, atomicsLeft);
            NetworkController.HandleSendingMessage(atomicsUpdateDemandMessage);
        }

        public override void DoSpawnCharacterDemand(Character attributes)
        {
            string characterName = attributes.HouseCharacter.characterName;
            Position pos = new Position(attributes.CurrentMapfield.XCoordinate, attributes.CurrentMapfield.ZCoordinate);

            // todo set clientid reasonable.
            SpawnCharacterDemandMessage spawnCharacterDemandMessage = new SpawnCharacterDemandMessage(1234, attributes.CharacterId, characterName, pos, attributes);
            NetworkController.HandleSendingMessage(spawnCharacterDemandMessage);
        }

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
        public override void DoGameEndMessage(int winnerID, int loserID, Statistics stats)
        {
            GameEndMessage gameEndMessage = new GameEndMessage(winnerID, loserID, stats);
            NetworkController.HandleSendingMessage(gameEndMessage);
        }

        public override void DoSendGameState(int clientID, int[] activlyPlayingIDs, String[] history)
        {
            GameStateMessage gameStateMessage = new GameStateMessage(history, activlyPlayingIDs, clientID);
            NetworkController.HandleSendingMessage(gameStateMessage);
        }

        /// <summary>
        /// increases the amount of strikes of a client and sends a strike message
        /// </summary>
        /// <param name="clientID">the id of the player, who gets a strike</param>
        /// <param name="wrongMessage">the wrong message, who was send by the client</param>
        /**public override void DoSendStrike(Player player, Message wrongMessage)
        {
            string wrongMessageAsString = MessageConverter.FromMessage(wrongMessage);
            player.AddStrike();
            StrikeMessage strikeMessage = new StrikeMessage(player.ClientID, wrongMessageAsString, player.AmountOfStrikes);
            NetworkController.HandleSendingMessage(strikeMessage);
        }*/
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

        public override void OnUnpauseGameOffer(int requestedByClientID)
        {
            UnpauseGameOfferMessage unpauseGameOfferMessage = new UnpauseGameOfferMessage(requestedByClientID);
            NetworkController.HandleSendingMessage(unpauseGameOfferMessage);
        }

        public override void OnJoinAccepted(JoinAcceptedMessage msg) { }

        // the server should not use this method
        public override Message OnJoinAcceptedMessage(JoinAcceptedMessage joinAcceptedMessage)

        {
            throw new NotImplementedException();
        }


        public override void DoSendJoin(string clientName) { }

        // the server should not use this method
        public override Message OnGameConfigMessage(GameConfigMessage gameConfigMessage)
        {
            throw new NotImplementedException();
        }

        // the server should not use this method
        public override Message OnMapChangeDemandMessage(MapChangeDemandMessage mapChangeDemandMessage)
        {
            throw new NotImplementedException();
        }

        // the server should not use this method
        public override Message OnStrikeMessage(StrikeMessage strikeMessage)
        {
            throw new NotImplementedException();
        }

        // the server should not use this method
        public override Message OnGameEndMessage(GameEndMessage gameEndMessage)
        {
            throw new NotImplementedException();
        }

        // the server should not use this method
        public override Message OnGameStateMessage(GameStateMessage gameStateMessage)
        {
            throw new NotImplementedException();
        }

        // the server should not use this method
        public override Message OnPauseGameDemandMessage(GamePauseDemandMessage gamePauseDemandMessage)
        {
            throw new NotImplementedException();
        }

        // the server should not use this method
        public override Message OnHouseOfferMessage(HouseOfferMessage houseOfferMessage)
        {
            throw new NotImplementedException();
        }

        // the server should not use this method
        public override Message OnHouseAcknowledgementMessage(HouseAcknowledgementMessage houseAcknowledgementMessage)
        {
            throw new NotImplementedException();
        }

        // the server should not use this method
        public override Message OnTurnDemandMessage(TurnDemandMessage turnDemandMessage)
        {
            throw new NotImplementedException();
        }

        // the server should not use this method
        public override Message OnMovementDemandMessage(MovementDemandMessage movementDemandMessage)
        {
            throw new NotImplementedException();
        }

        // the server should not use this method
        public override Message OnActionDemandMessage(ActionDemandMessage actionDemandMessage)
        {
            throw new NotImplementedException();
        }

        // the server should not use this method
        public override Message OnChangeCharacterStatisticsDemandMessage(ChangeCharacterStatisticsDemandMessage changeCharacterStatisticsDemandMessage)
        {
            throw new NotImplementedException();
        }

        // the server should not use this method
        public override Message OnSpawnCharacterDemandMessage(SpawnCharacterDemandMessage spawnCharacterDemandMessage)
        {
            throw new NotImplementedException();
        }

        // the server should not use this method
        public override Message OnChangePlayerSpiceDemandMessage(ChangePlayerSpiceDemandMessage changePlayerSpiceDemandMessage)
        {
            throw new NotImplementedException();
        }

        // the server should not use this method
        public override Message OnSandwormSpawnDemandMessage(SandwormSpawnDemandMessage sandwormSpawnDemandMessage)
        {
            throw new NotImplementedException();
        }

        // the server should not use this method
        public override Message OnSandwormMoveDemandMessage(SandwormMoveDemandMessage sandwormMoveMessage)
        {
            throw new NotImplementedException();
        }

        // the server should not use this method
        public override Message OnSandwormDespawnMessage(SandwormDespawnDemandMessage sandwormDespawnDemandMessage)
        {
            throw new NotImplementedException();
        }

        // the server should not use this method
        public override Message OnEndGameMessage(EndGameMessage endGameMessage)
        {
            throw new NotImplementedException();
        }

        // the server should not use this method
        public override Message OnTransferDemandMessage(TransferDemandMessage transferDemandMessage)
        {
            throw new NotImplementedException();
        }
    }
}