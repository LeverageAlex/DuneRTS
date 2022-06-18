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
using GameData.server.roundHandler;
using GameData.network.util.world.mapField;
using GameData.network.util.world.character;
using Server.roundHandler;
using System.Threading;

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
                //  DoSendGameConfig();
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
                if (client.ClientSecret == msg.ClientSecret)
                {
                    client.SessionID = sessionID; //new sessionID for the rejoined client
                    if (client.IsActivePlayer)
                    {
                        // TODO: send current game stats or gamestats what player has missed
                    }
                    rejoinSuccessful = true;
                    DoAcceptJoin(client.ClientSecret, client.ClientID, client.SessionID);
                    Log.Information($"Rejoin of client: {client.ClientName} was successful.");
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
                        List<Player> listPlayer =  Party.GetInstance().GetActivePlayers();
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

            if (activePlayer == null)
            {
                throw new NullReferenceException($"Requested player with {msg.clientID} isn't known!");
            }

            //get the character which should be moved
            Character movingCharacter = null;
            foreach (var character in activePlayer.UsedGreatHouse.GetCharactersAlive())
            {
                if (character.CharacterId == msg.characterID)
                {
                    movingCharacter = character;
                }
            }

            if (movingCharacter == null)
            {
                DoSendError(005, $"Moving character is null", activePlayer.SessionID);
                return;
            }

            var path = msg.specs.path;
            bool alreadySteppedOnSandField = false;
            var newPath = new List<Position>();
            foreach (var position in path)
            {
                var party = Party.GetInstance();
                if (movingCharacter.MPcurrent > 0 && !movingCharacter.IsInSandStorm(Map.instance)) //check if Character has enough Movement Points
                {
                    if (position.x >= 0 && position.x < party.map.MAP_WIDTH && position.y >= 0 && position.y < party.map.MAP_HEIGHT) //check if movement is in bounds of the map
                    {
                        //check if movement is on walkable terrain
                        if (party.map.fields[position.y, position.x].tileType != TileType.MOUNTAINS.ToString() && party.map.fields[position.y, position.x].tileType != TileType.CITY.ToString()) //check needed and not implemented utils
                        {
                            if (party.map.fields[position.y, position.x].IsCharacterStayingOnThisField)  //if the mapfield is occupied by a character they swap positions
                            {
                                Character passiveCharacter = party.map.fields[position.y, position.x].GetCharacterStayingOnThisField(party.map.GetCharactersOnMap());
                                passiveCharacter.Movement(passiveCharacter.CurrentMapfield, movingCharacter.CurrentMapfield);
                                DoSendMovementDemand(msg.clientID, passiveCharacter.CharacterId, new List<Position> { new Position(movingCharacter.CurrentMapfield.XCoordinate, movingCharacter.CurrentMapfield.ZCoordinate) });
                                movingCharacter.Movement(movingCharacter.CurrentMapfield, party.map.fields[position.y, position.x]);
                            }
                            else
                            {
                                movingCharacter.Movement(movingCharacter.CurrentMapfield, party.map.fields[position.y, position.x]); //move character 1 field along its path
                            }
                            //path.Add(position);
                            newPath.Add(position);
                            if (party.map.fields[position.y, position.x].tileType == TileType.FLAT_SAND.ToString() || party.map.fields[position.y, position.x].tileType == TileType.DUNE.ToString())
                            {
                                if (alreadySteppedOnSandField)
                                {
                                    movingCharacter.SetLoud();
                                }
                                else
                                {
                                    alreadySteppedOnSandField = true;
                                }
                            }
                            //deliver spice to city if city is neighborfield
                            foreach (var mapfield in party.map.GetNeighborFields(movingCharacter.CurrentMapfield))
                            {
                                if (mapfield.IsCityField && mapfield.clientID == activePlayer.ClientID)
                                {
                                    activePlayer.statistics.AddToHouseSpiceStorage(movingCharacter.inventoryUsed);
                                    movingCharacter.inventoryUsed = 0;
                                    DoChangePlayerSpiceDemand(activePlayer.ClientID, activePlayer.statistics.HouseSpiceStorage);
                                }
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

            DoSendMovementDemand(msg.clientID, msg.characterID, newPath);
            DoSendChangeCharacterStatsDemand(msg.clientID, msg.characterID, new CharacterStatistics(movingCharacter));

            if (movingCharacter.MPcurrent <= 0 && movingCharacter.APcurrent <= 0)
            {
              //  CharacterTraitPhase.StopAndResetTimer();
                Party.GetInstance().RoundHandler.GetCharacterTraitPhase().SendRequestForNextCharacter();
            }
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
                if (player.ClientID != msg.clientID)
                {
                    enemyPlayer = player;
                }
            }

            if (activePlayer == null)
            {
                throw new NullReferenceException($"Requested player with {msg.clientID} isn't known!");
            }
            if (enemyPlayer == null)
            {
                throw new NullReferenceException($"Enemy player not found!");
            }

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
                if (character.CurrentMapfield.XCoordinate == msg.specs.target.x && character.CurrentMapfield.ZCoordinate == msg.specs.target.y)
                {
                    targetCharacter = character;
                    friendlyFire = true; //characters can not attack their allys
                }
            }
            //get the target character from enemy player if the target character is not an ally
            if (targetCharacter == null)
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

            //set Attack as standard enum and change it if needed
            ActionType action = ActionType.ATTACK;

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
                            if (!friendlyFire && !targetCharacter.IsInSandStorm(map))
                            {
                                actionCharacter.Attack(targetCharacter);
                                if (targetCharacter.IsDead()) // if enemy dies from attack, update statistics
                                {
                                    activePlayer.statistics.AddToEnemiesDefeated(1);
                                }
                                charactersHit.Add(targetCharacter);
                            }
                            break;
                        case ActionType.COLLECT:
                            action = ActionType.COLLECT;
                            actionCharacter.CollectSpice();
                            activePlayer.statistics.AddToTotalSpiceCollected(1);
                            DoSendMapChangeDemand(MapChangeReasons.ROUND_PHASE);
                            //deliver spice to city if city is neighborfield
                            foreach (var mapfield in map.GetNeighborFields(actionCharacter.CurrentMapfield))
                            {
                                if (mapfield.IsCityField && mapfield.clientID == activePlayer.ClientID)
                                {
                                    activePlayer.statistics.AddToHouseSpiceStorage(actionCharacter.inventoryUsed);
                                    actionCharacter.inventoryUsed = 0;
                                    DoChangePlayerSpiceDemand(activePlayer.ClientID, activePlayer.statistics.HouseSpiceStorage);
                                }
                            }
                            break;
                        //check in every special action if the character is from the right character type to do the special aciton and check if his ap is full
                        case ActionType.KANLY:
                            Random rnd = new Random();
                            int success = rnd.Next(100);
                            action = ActionType.KANLY;
                            if (targetCharacter == null)
                            {
                                DoSendError(005, "Target character is null when kanly action is executed!", activePlayer.SessionID);
                            }
                            else if (actionCharacter.APcurrent == actionCharacter.APmax
                                && actionCharacter.characterType == Enum.GetName(typeof(CharacterType), CharacterType.NOBLE)
                                && targetCharacter.characterType == Enum.GetName(typeof(CharacterType), CharacterType.NOBLE)
                                && !friendlyFire
                                && !targetCharacter.IsInSandStorm(map)
                                && success < PartyConfiguration.GetInstance().kanlySuccessProbability * 100)
                            {
                                actionCharacter.Kanly(targetCharacter);
                                activePlayer.statistics.AddToEnemiesDefeated(1);
                                charactersHit.Add(targetCharacter);
                            } 
                        break;
                    case ActionType.FAMILY_ATOMICS:
                        action = ActionType.FAMILY_ATOMICS;
                            if (actionCharacter.APcurrent == actionCharacter.APmax
                                && actionCharacter.characterType == Enum.GetName(typeof(CharacterType), CharacterType.NOBLE))
                            {
                                //get the mapfield where the active Character aims to
                                MapField targetMapField = null;
                                foreach (var mapfield in map.fields)
                                {
                                    if (mapfield.XCoordinate == msg.specs.target.x && mapfield.ZCoordinate == msg.specs.target.y)
                                    {
                                        targetMapField = mapfield;
                                    }
                                }
                                if(targetMapField == null)
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
                            if(greathouseConventionBrokenBeforeAtomicBomb != Noble.greatHouseConventionBroken)
                            {
                                    int charactersAmount = enemyPlayer.UsedGreatHouse.Characters.Count;
                                    for (int i = charactersAmount - 1; i > charactersAmount - 5; i--)
                                    {
                                        DoSpawnCharacterDemand(enemyPlayer.UsedGreatHouse.Characters[i]);
                                    }
                                DoSendAtomicsUpdateDemand(msg.clientID, true, actionCharacter.greatHouse.unusedAtomicBombs);
                            }
                            else
                            {
                                DoSendAtomicsUpdateDemand(msg.clientID, false, actionCharacter.greatHouse.unusedAtomicBombs);
                            }
                        }
                        break;
                    case ActionType.SPICE_HOARDING:
                        action = ActionType.SPICE_HOARDING;
                            if (actionCharacter.APcurrent == actionCharacter.APmax
                                && actionCharacter.characterType == Enum.GetName(typeof(CharacterType), CharacterType.MENTAT))
                            {
                                int inventoryUsedBeforeSpiceHoarding = actionCharacter.inventoryUsed;
                                actionCharacter.SpiceHoarding(map);
                                activePlayer.statistics.AddToTotalSpiceCollected(actionCharacter.inventoryUsed - inventoryUsedBeforeSpiceHoarding);
                                DoSendMapChangeDemand(MapChangeReasons.ROUND_PHASE);
                                //deliver spice to city if city is neighborfield
                                foreach (var mapfield in map.GetNeighborFields(actionCharacter.CurrentMapfield))
                                {
                                    if (mapfield.IsCityField && mapfield.clientID == activePlayer.ClientID)
                                    {
                                        activePlayer.statistics.AddToHouseSpiceStorage(actionCharacter.inventoryUsed);
                                        actionCharacter.inventoryUsed = 0;
                                        DoChangePlayerSpiceDemand(activePlayer.ClientID, activePlayer.statistics.HouseSpiceStorage);
                                    }
                                }
                            }
                            break;
                        //check in every special action if the character is from the right character type to do the special aciton and check if his ap is full
                        case ActionType.VOICE:
                            action = ActionType.VOICE;
                            if (actionCharacter.APcurrent == actionCharacter.APmax
                                && !targetCharacter.IsInSandStorm(map)
                                && actionCharacter.characterType == Enum.GetName(typeof(CharacterType), CharacterType.BENE_GESSERIT))
                            {
                                if (targetCharacter == null)
                                {
                                    DoSendError(005, "Target character is null when voice action is executed!", activePlayer.SessionID);
                                }
                                int InventoryUsedBeforeVoice = actionCharacter.inventoryUsed;
                                actionCharacter.Voice(targetCharacter);
                                activePlayer.statistics.AddToTotalSpiceCollected(actionCharacter.inventoryUsed - InventoryUsedBeforeVoice);
                                charactersHit.Add(targetCharacter);
                                //deliver spice to city if city is neighborfield
                                foreach (var mapfield in map.GetNeighborFields(actionCharacter.CurrentMapfield))
                                {
                                    if (mapfield.IsCityField && mapfield.clientID == activePlayer.ClientID)
                                    {
                                        activePlayer.statistics.AddToHouseSpiceStorage(actionCharacter.inventoryUsed);
                                        actionCharacter.inventoryUsed = 0;
                                        DoChangePlayerSpiceDemand(activePlayer.ClientID, activePlayer.statistics.HouseSpiceStorage);
                                    }
                                }
                            }
                            break;
                        case ActionType.SWORD_SPIN:
                            action = ActionType.SWORD_SPIN;
                            if (actionCharacter.APcurrent == actionCharacter.APmax
                                && actionCharacter.characterType == Enum.GetName(typeof(CharacterType), CharacterType.FIGHTER))
                            {
                                charactersHit = actionCharacter.SwordSpin(map);
                                foreach (var character in charactersHit)
                                {
                                    if (character.IsDead())
                                    {
                                        activePlayer.statistics.AddToEnemiesDefeated(1);
                                    }
                                }
                            }
                            break;
                        default:
                            throw new ArgumentException($"Actiontype {msg.action} not supoorted here.");
                    }
                    DoSendActionDemand(msg.clientID, msg.characterID, action, msg.specs.target);
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
                } else
                {
                    //Character is in Sandstorm
                    actionCharacter.MPcurrent = 0;
                    actionCharacter.APcurrent = 0;
                }
            }

            if ((actionCharacter.MPcurrent <= 0 && actionCharacter.APcurrent <= 0) || actionCharacter.IsDead())
            {
          //      CharacterTraitPhase.StopAndResetTimer();
                Party.GetInstance().RoundHandler.GetCharacterTraitPhase().SendRequestForNextCharacter();
            }
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

            if (activePlayer == null)
            {
                throw new NullReferenceException($"Requested player with {msg.clientID} isn't known!");
            }

                //get the characters which are involved in the transfer
                Character activeCharacter = null;
            Character targetCharacter = null;
            foreach (var character in activePlayer.UsedGreatHouse.Characters)
            {
                if (character.CharacterId == msg.characterID)
                {
                    activeCharacter = character;
                    activeCharacter = character;
                }
                if (character.CharacterId == msg.targetID)
                {
                    targetCharacter = character;
                }
            }

            if (!activeCharacter.IsInSandStorm(Map.instance) && !targetCharacter.IsInSandStorm(Map.instance))
            {
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

                bool targetCharacterIsOnNeighborfield = false;
                foreach (var mapfield in Party.GetInstance().map.GetNeighborFields(activeCharacter.CurrentMapfield))
                {
                    if (mapfield.IsCharacterStayingOnThisField && mapfield.Character.CharacterId == targetCharacter.CharacterId)
                    {
                        targetCharacterIsOnNeighborfield = true;
                    }
                }

                if (targetCharacterIsOnNeighborfield && !targetCharacter.IsInSandStorm(Party.GetInstance().map) && targetCharacter.greatHouse == activeCharacter.greatHouse)
                {
                    activeCharacter.GiftSpice(targetCharacter, msg.amount);
                    DoSendTransferDemand(msg.clientID, msg.characterID, msg.targetID);

                    DoSendChangeCharacterStatsDemand(msg.clientID, msg.characterID, new CharacterStatistics(activeCharacter));
                    DoSendChangeCharacterStatsDemand(msg.clientID, msg.targetID, new CharacterStatistics(targetCharacter));
                    //deliver spice to city if city is neighborfield
                    foreach (var mapfield in Party.GetInstance().map.GetNeighborFields(targetCharacter.CurrentMapfield))
                    {
                        if (mapfield.IsCityField && mapfield.clientID == activePlayer.ClientID)
                        {
                            activePlayer.statistics.AddToHouseSpiceStorage(targetCharacter.inventoryUsed);
                            targetCharacter.inventoryUsed = 0;
                            DoChangePlayerSpiceDemand(activePlayer.ClientID, activePlayer.statistics.HouseSpiceStorage);
                        }
                    }
                }

                if (activeCharacter.MPcurrent <= 0 && activeCharacter.APcurrent <= 0)
                {
                    //CharacterTraitPhase.StopAndResetTimer();
                    Party.GetInstance().RoundHandler.GetCharacterTraitPhase().SendRequestForNextCharacter();
                }
            }
        }

        /// <summary>
        /// End turn of a character and heal this character if he hasn't moved
        /// </summary>
        /// <param name="msg"></param>
        public override void OnEndTurnRequestMessage(EndTurnRequestMessage msg)
        {
            //CharacterTraitPhase.StopAndResetTimer();
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

        public override void OnGameStateRequestMessage(GameStateRequestMessage msg)
        {
            throw new NotImplementedException("not implemented");

            //Requirement complete game state

            //int clientID
        }

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
            PartyReference partyReference = new PartyReference(partyConfiguration);

            // get the client ids and their city positions
            int client0ID = Party.GetInstance().GetActivePlayers()[0].ClientID;
            int client1ID = Party.GetInstance().GetActivePlayers()[1].ClientID;

            // get cities of both players
            City cityPlayer0 = Party.GetInstance().GetActivePlayers()[0].City;
            City cityPlayer1 = Party.GetInstance().GetActivePlayers()[1].City;

            CityToClient[] cityToClientArray = new CityToClient[2];
            cityToClientArray[0] = new CityToClient(client0ID, cityPlayer0.XCoordinate, cityPlayer0.ZCoordinate);
            cityToClientArray[1] = new CityToClient(client1ID, cityPlayer1.XCoordinate, cityPlayer1.ZCoordinate);

            // get the eye of the storm
            MapField stormEyeField = Party.GetInstance().RoundHandler.SandstormPhase.EyeOfStorm;
            Position stormEye = new Position(stormEyeField.XCoordinate, stormEyeField.ZCoordinate);

            GameConfigMessage gameConfigMessage = new GameConfigMessage(scenario, partyReference, cityToClientArray, stormEye);
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
            MovementDemandMessage movementDemandMessage = new MovementDemandMessage(clientID, characterID, new Specs(null, path));
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
    }
}