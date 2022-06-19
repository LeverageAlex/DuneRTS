using System;
using System.Collections.Generic;
using GameData.Configuration;
using GameData.network.controller;
using GameData.network.messages;
using GameData.network.util.enums;
using GameData.network.util.world;
using GameData.network.util.world.greatHouse;
using Newtonsoft.Json;
using Serilog;

namespace AIClient
{
    /// <summary>
    /// Controller for handling all messages send and received by the ai client
    /// </summary>
    /// <remarks>
    /// Therefore this controller inherits the standard message controller and implement
    /// all necessary methods, that are needed for playing a game with the ai client
    /// </remarks>
    public class AIPlayerMessageController : MessageController
    {
        public AIPlayerMessageController()
        {
        }

        /// <summary>
        /// sends a join request with and signalize the server, that this client is an ai and provide the name
        /// </summary>
        /// <param name="clientName">the name of the ai client</param>
        public override void DoSendJoin(string clientName)
        {
            // the ai is an active player and an ai, so set both bool to true
            JoinMessage msg = new JoinMessage(clientName, true, true);
            NetworkController.HandleSendingMessage(msg);
        }

        /// <summary>
        /// sends a house request with a given great house name
        /// </summary>
        /// <param name="houseName">the name of the chosen great house</param>
        public override void DoSendHouseRequest(string houseName)
        {
            HouseRequestMessage msg = new HouseRequestMessage(houseName);
            NetworkController.HandleSendingMessage(msg);
        }

        /// <summary>
        /// called, when the server send a JOINACCEPTED message, so the join request was accepted. 
        /// </summary>
        /// <remarks>
        /// So set the client id and secret received with this message and prepare the game.
        /// </remarks>
        /// <param name="msg">the received JOINACCEPTED message</param>
        public override void OnJoinAccepted(JoinAcceptedMessage msg)
        {
            Log.Information($"The join request was accepted. \n The ai client now has the id {msg.clientID}!");

            // set the id and secret
            Party.GetInstance().ClientID = msg.clientID;
            Party.GetInstance().ClientSecret = msg.clientSecret;
        }

        /// <summary>
        /// called, when an error occur during the join process, e. g. if the join message was wrong or if there are
        /// already two active players
        /// </summary>
        /// <param name="msg">the received ERROR message</param>
        public override void OnErrorMessage(ErrorMessage msg)
        {
            Log.Error($"An error occured during joining: {msg.ErrorDescription}");

            switch (msg.ErrorCode)
            {
                case 1:
                    Log.Debug("The join message had a bad format, so send it again");
                    DoSendJoin(Programm.Configuration.Name);
                    break;
                case 3:
                    Log.Debug("There are already two players registred, so terminate this client!");
                    Environment.Exit(0);
                    break;
                default:
                    Log.Debug($"The error code was {msg.ErrorCode} a could not be proccessed!");
                    break;
            }
        }

        /// <summary>
        /// called, when the server send the game config message after both players joined the game and successfully chose
        /// a great house
        /// </summary>
        /// <remarks>
        /// In this callback the following things are done:
        /// <list type="bullet">
        /// <item>processing and creating a party configuration</item>
        /// <item>create a map, the client is playing on</item>
        /// <item>set the position of the storm eye on the map</item>
        /// </list>
        /// </remarks>
        /// <param name="gameConfigMessage">the received GAMECFG message</param>
        public override void OnGameConfigMessage(GameConfigMessage gameConfigMessage)
        {
            Log.Debug("Received the game configuration from the server");

            // load the party configuration and create a new party configuration class
            PartyConfiguration partyConfiguration = JsonConvert.DeserializeObject<PartyConfiguration>(gameConfigMessage.party.refr);
            PartyConfiguration.SetInstance(partyConfiguration);

            //Initialization for greatHouses in GameData project
            GameData.Configuration.Configuration.InitializeConfigurations();

            // Initialization for the character configurations in GameData project
            GameData.Configuration.Configuration.InitializeCharacterConfiguration(
                PartyConfiguration.GetInstance().noble,
                PartyConfiguration.GetInstance().mentat,
                PartyConfiguration.GetInstance().beneGesserit,
                PartyConfiguration.GetInstance().fighter);

            // set the map of the ai client
            int mapWidth = gameConfigMessage.scenario[0].Count;
            int mapHeight = gameConfigMessage.scenario.Count;
            Party.GetInstance().Map = new Map(mapWidth, mapHeight, gameConfigMessage.scenario);

            Party.GetInstance().Map.PositionOfEyeOfStorm = gameConfigMessage.stormEye;

            // TODO: process the cityToClient information

        }

        /// <summary>
        /// called, when the server sends two great houses to choose between
        /// </summary>
        /// <remarks>
        /// If the house offer was for this client, randomly choose one great house and send the decicision back.
        /// </remarks>
        /// <param name="houseOfferMessage">received HOUSE_OFFER message containing the two great houses</param>
        /// TODO: check, whether it is better to not choose randomly
        public override void OnHouseOfferMessage(HouseOfferMessage houseOfferMessage)
        {
            // check, whether the message was for this client
            if (houseOfferMessage.clientID == Party.GetInstance().ClientID)
            {
                GreatHouse firstChoice = houseOfferMessage.houses[0];
                GreatHouse secondChoice = houseOfferMessage.houses[1];

                Log.Information($"The server offered the following two great houses: {firstChoice.houseName} and {secondChoice.houseName}.");

                // choose great house randomly
                Random random = new Random();
                if (random.NextDouble() < 0.5)
                {
                    // choose first choice
                    DoSendHouseRequest(firstChoice.houseName);
                    Log.Debug($"Chose the great house {firstChoice.houseName}");
                }
                else
                {
                    // choose second choice
                    DoSendHouseRequest(secondChoice.houseName);
                    Log.Debug($"Chose the great house {secondChoice.houseName}");
                }
            }
        }

        /// <summary>
        /// called, when the server acknowledged the chosen great house and set the name of the great house
        /// </summary>
        /// <param name="houseAcknowledgementMessage"></param>
        public override void OnHouseAcknowledgementMessage(HouseAcknowledgementMessage houseAcknowledgementMessage)
        {
            Log.Information($"The great house for this client is {houseAcknowledgementMessage.houseName}");

            Party.GetInstance().AssignedGreatHouse = houseAcknowledgementMessage.houseName;
        }

        /// <summary>
        /// called, when the server sends a message about the new map
        /// </summary>
        /// <remarks>
        /// changes the map in the party depending on the new map provided by this message and display the new map with the reason in the console
        /// </remarks>
        /// <param name="mapChangeDemandMessage">the received MAP_CHANGE_DEMAND message</param>
        public override void OnMapChangeDemandMessage(MapChangeDemandMessage mapChangeDemandMessage)
        {
            // update the map in the party
            Map map = Party.GetInstance().Map;
            map.fields = map.GetNewMapFromMessage(mapChangeDemandMessage.newMap);


            Party.GetInstance().Map.PositionOfEyeOfStorm = mapChangeDemandMessage.stormEye;

            // print new map to console
            Log.Debug($"The map changed because of {mapChangeDemandMessage.changeReason}. The new map is: \n");
            Party.GetInstance().Map.DrawMapToConsole();
        }

        /// <summary>
        /// called, when the server sends a SPAWN_CHARACTER_DEMAND message
        /// </summary>
        /// <remarks>
        /// spawn a new character, so create a new character and add it to the list of alive characters in the party
        /// </remarks>
        /// <param name="spawnCharacterDemandMessage">the received SPAWN_CHARACTER_DEMAND message</param>
        public override void OnSpawnCharacterDemandMessage(SpawnCharacterDemandMessage spawnCharacterDemandMessage)
        {
            // check, if the character spawn is for this client
            if (spawnCharacterDemandMessage.clientID == Party.GetInstance().ClientID)
            {
                // create a new character depending on the values
                CharacterType type = (CharacterType)Enum.Parse(typeof(CharacterType), spawnCharacterDemandMessage.attributes.characterType);
                Character newCharacter = new Character(spawnCharacterDemandMessage.characterID, spawnCharacterDemandMessage.characterName, type, spawnCharacterDemandMessage.attributes.healthMax, spawnCharacterDemandMessage.attributes.healthCurrent, spawnCharacterDemandMessage.attributes.healingHP, spawnCharacterDemandMessage.attributes.MPmax, spawnCharacterDemandMessage.attributes.MPcurrent, spawnCharacterDemandMessage.attributes.APmax, spawnCharacterDemandMessage.attributes.APcurrent, spawnCharacterDemandMessage.attributes.attackDamage, spawnCharacterDemandMessage.attributes.inventorySize, spawnCharacterDemandMessage.attributes.inventoryUsed, spawnCharacterDemandMessage.attributes.killedBySandworm, spawnCharacterDemandMessage.attributes.isLoud);

                MapField characterMapField = Party.GetInstance().Map.GetMapFieldAtPosition(spawnCharacterDemandMessage.position.x, spawnCharacterDemandMessage.position.y);
                characterMapField.PlaceCharacter(newCharacter);

                newCharacter.CurrentMapfield = characterMapField;

                // add new character to list of alive characters
                Party.GetInstance().AddAliveCharacter(newCharacter);
            }
        }

        /// <summary>
        /// called, when the server send a message, which indicates, that one character has new values / statistics
        /// </summary>
        /// <remarks>
        /// checks, whether the character belongs to this clients and if so, update the values with the given values
        /// </remarks>
        /// <param name="changeCharacterStatisticsDemandMessage">the received CHARACTER_STAT_CHANGE_DEMAND message</param>
        public override void OnChangeCharacterStatisticsDemandMessage(ChangeCharacterStatisticsDemandMessage changeCharacterStatisticsDemandMessage)
        {
            // check, if the character statistics change is for this client
            if (changeCharacterStatisticsDemandMessage.clientID == Party.GetInstance().ClientID)
            {
                // find the character with the given id
                foreach (Character character in Party.GetInstance().AliveCharacters)
                {
                    if (character.CharacterId == changeCharacterStatisticsDemandMessage.characterID)
                    {
                        // change values of this character
                        character.healthCurrent = changeCharacterStatisticsDemandMessage.stats.HP;
                        character.MPcurrent = changeCharacterStatisticsDemandMessage.stats.MP;
                        character.APcurrent = changeCharacterStatisticsDemandMessage.stats.AP;
                        character.inventoryUsed = changeCharacterStatisticsDemandMessage.stats.spice;
                        character.KilledBySandworm = changeCharacterStatisticsDemandMessage.stats.isSwallowed;
                        character.isLoud = changeCharacterStatisticsDemandMessage.stats.isLoud;
                    }
                }
            }
        }

        /// <summary>
        /// called, when the server sends a message, that contains the new spice value of a city / client
        /// </summary>
        /// <remarks>
        /// checks, whether this message is for this client and if so, set the new value and print it to the console
        /// </remarks>
        /// <param name="changePlayerSpiceDemandMessage">the received CHANGE_PLAYER_SPICE_DEMAND message</param>
        public override void OnChangePlayerSpiceDemandMessage(ChangePlayerSpiceDemandMessage changePlayerSpiceDemandMessage)
        {
            // check, if the spice change is for this client
            if (changePlayerSpiceDemandMessage.clientID == Party.GetInstance().ClientID)
            {
                Party.GetInstance().CitySpice = changePlayerSpiceDemandMessage.newSpiceValue;
                Log.Information($"This city has now a spice amount of {changePlayerSpiceDemandMessage.newSpiceValue}!");
            }
        }

        /// <summary>
        /// called, when the server send the message, that a new sandworm spawned
        /// </summary>
        /// <remarks>
        /// print the information to the console and set the map field of the sandworm to not approachable
        /// </remarks>
        /// <param name="sandwormSpawnDemandMessage">the received SANDWORM_SPAWN_DEMAND message</param>
        public override void OnSandwormSpawnDemandMessage(SandwormSpawnDemandMessage sandwormSpawnDemandMessage)
        {
            Log.Debug($"A new sandworm spawned at ({sandwormSpawnDemandMessage.position.x},{sandwormSpawnDemandMessage.position.y}).");

            // set the map field as not approachable
            Party.GetInstance().CurrentPositionOfSandworm = sandwormSpawnDemandMessage.position;
            Party.GetInstance().Map.GetMapFieldAtPosition(sandwormSpawnDemandMessage.position.x, sandwormSpawnDemandMessage.position.y).IsApproachable = false;
        }

        /// <summary>
        /// called, when the server send the message, that a new sandworm moved
        /// </summary>
        /// <remarks>
        /// print the information to the console and update the position of the sandworm and that this field is not approachable
        /// </remarks>
        /// <param name="sandwormMoveMessage">the received SANDWORM_MOVE_DEMAND message</param>
        public override void OnSandwormMoveDemandMessage(SandwormMoveDemandMessage sandwormMoveMessage)
        {
            Party.GetInstance().Map.GetMapFieldAtPosition(Party.GetInstance().CurrentPositionOfSandworm.x, Party.GetInstance().CurrentPositionOfSandworm.y).IsApproachable = true;

            Position targetPosition = sandwormMoveMessage.path[sandwormMoveMessage.path.Count - 1];
            Log.Debug($"The sandworm moved to ({targetPosition.x},{targetPosition.y}).");

            // set the map field as not approachable
            Party.GetInstance().CurrentPositionOfSandworm = targetPosition;
            Party.GetInstance().Map.GetMapFieldAtPosition(targetPosition.x, targetPosition.y).IsApproachable = false;

        }

        /// <summary>
        /// called, when the server send the message, that a new sandworm despawned
        /// </summary>
        /// <remarks>
        /// print the information to the console and set the map field of the sandworm to approachable
        /// </remarks>
        /// <param name="sandwormDespawnDemandMessage">the received SANDWORM_DESPAWN_DEMAND message</param>
        public override void OnSandwormDespawnMessage(SandwormDespawnDemandMessage sandwormDespawnDemandMessage)
        {
            Log.Debug("The sandworm despawned.");

            // set the map field as not approachable
            Party.GetInstance().Map.GetMapFieldAtPosition(Party.GetInstance().CurrentPositionOfSandworm.x, Party.GetInstance().CurrentPositionOfSandworm.y).IsApproachable = true;

        }

        /// <summary>
        /// called, when the server sends a pause / resume message
        /// </summary>
        /// <remarks>
        /// informs the client, that the game was paused / resumed and print this to the console
        /// </remarks>
        /// <param name="gamePauseDemandMessage">the received GAME_PAUSE_DEMAND message</param>
        public override void OnPauseGameDemandMessage(GamePauseDemandMessage gamePauseDemandMessage)
        {
            if (gamePauseDemandMessage.pause)
            {
                Log.Information($"The game was paused by the client with the id {gamePauseDemandMessage.requestedByClientID}.");
            }
            else
            {
                Log.Information($"The game was resumed by the client with the id {gamePauseDemandMessage.requestedByClientID}.");
            }
        }


        /// <summary>
        /// called, when the server sends a resume offer
        /// </summary>
        /// <remarks>
        /// informs the client, that the game can be resumed and print this to the console.
        /// Because the ai client is not allowed to resume a game, no resume demand is send!
        /// </remarks>
        /// <param name="unpauseGameOfferMessage">the received UNPAUSE_GAME_OFFER message</param>
        public override void OnUnpauseOfferDemand(UnpauseGameOfferMessage unpauseGameOfferMessage)
        {
            Log.Debug($"The pause requested by the client with the id {unpauseGameOfferMessage.requestedByClientID} can be resumed by any client (except for ai clients).");
        }



        public override void OnActionDemandMessage(ActionDemandMessage actionDemandMessage)
        {
            throw new NotImplementedException();
        }

        public override void OnActionRequestMessage(ActionRequestMessage msg)
        {
            throw new NotImplementedException();
        }


        public override void OnAtomicsUpdateDemandMessage(AtomicsUpdateDemandMessage atomicUpdateDemandMessage)

        {
            throw new NotImplementedException();
        }



        public override void OnEndGameMessage(EndGameMessage endGameMessage)

        {
            throw new NotImplementedException();
        }

        public override void OnEndTurnRequestMessage(EndTurnRequestMessage msg)
        {
            throw new NotImplementedException();
        }

        public override void OnGameEndMessage(GameEndMessage gameEndMessage)

        {
            throw new NotImplementedException();
        }

        public override void OnGameStateMessage(GameStateMessage gameStateMessage)
        {
            throw new NotImplementedException();
        }


        public override void OnGameStateRequestMessage(GameStateRequestMessage msg)
        {
            throw new NotImplementedException();
        }

        public override void OnHouseRequestMessage(HouseRequestMessage msg, string sessionID)
        {
            throw new NotImplementedException();
        }

        public override void OnJoinMessage(JoinMessage msg, string sessionID)
        {
            throw new NotImplementedException();
        }

        public override void OnMovementDemandMessage(MovementDemandMessage movementDemandMessage)
        {
            throw new NotImplementedException();
        }

        public override void OnMovementRequestMessage(MovementRequestMessage msg)
        {
            throw new NotImplementedException();
        }

        public override void OnPauseGameRequestMessage(PauseGameRequestMessage msg, string sessionID)
        {
            throw new NotImplementedException();
        }

        public override void OnRejoinMessage(RejoinMessage msg, string sessionID)
        {
            throw new NotImplementedException();
        }

        
        public override void OnStrikeMessage(StrikeMessage strikeMessage)
        {
            throw new NotImplementedException();
        }

        public override void OnTransferDemandMessage(TransferDemandMessage transferDemandMessage)

        {
            throw new NotImplementedException();
        }

        public override void OnTransferRequestMessage(TransferRequestMessage msg)
        {
            throw new NotImplementedException();
        }

        public override void OnTurnDemandMessage(TurnDemandMessage turnDemandMessage)
        {
            throw new NotImplementedException();
        }

        public override void OnUnpauseGameOffer(int requestedByClientID)
        {
            throw new NotImplementedException();
        }

        public override void DoAcceptJoin(string clientSecret, int clientID, string sessionID)
        {
            throw new NotImplementedException();
        }

        public override void DoChangePlayerSpiceDemand(int clientID, int newSpiceVal)
        {
            throw new NotImplementedException();
        }

        public override void DoDespawnSandwormDemand()
        {
            throw new NotImplementedException();
        }
        public override void DoGamePauseDemand(int requestedByClientID, bool pause)
        {
            throw new NotImplementedException();
        }

        public override void DoMoveSandwormDemand(List<MapField> list)
        {
            throw new NotImplementedException();
        }

        public override void DoSendActionDemand(int clientID, int characterID, ActionType action, Position target)
        {
            throw new NotImplementedException();
        }

        public override void DoSendAtomicsUpdateDemand(int clientID, bool shunned, int atomicsLeft)
        {
            throw new NotImplementedException();
        }

        public override void DoSendChangeCharacterStatsDemand(int clientID, int characterID, CharacterStatistics stats)
        {
            throw new NotImplementedException();
        }

        public override void DoSendError(int errorCode, string errorDescription, string sessionID)
        {
            throw new NotImplementedException();
        }

        public override void DoSendGameConfig()
        {
            throw new NotImplementedException();
        }

        public override void DoSendGameState(int clientID, int[] activlyPlayingIDs, string[] history)
        {
            throw new NotImplementedException();
        }

        public override void DoSendHouseAck(int clientID, string houseName)
        {
            throw new NotImplementedException();
        }

        public override void DoSendHouseOffer(int clientID, GreatHouseType[] houses)
        {
            throw new NotImplementedException();
        }

        public override void DoSendMapChangeDemand(MapChangeReasons mapChangeReasons)
        {
            throw new NotImplementedException();
        }

        public override void DoSendMovementDemand(int clientID, int characterID, List<Position> path)
        {
            throw new NotImplementedException();
        }

        public override void DoSendStrike(int clientID, Message wrongMessage)
        {
            throw new NotImplementedException();
        }

        public override void DoSendTransferDemand(int clientID, int characterID, int targetID)
        {
            throw new NotImplementedException();
        }

        public override void DoSendTurnDemand(int clientID, int characterID)
        {
            throw new NotImplementedException();
        }

        public override void DoSpawnCharacterDemand(Character attributes)
        {
            throw new NotImplementedException();
        }

        public override void DoSpawnSandwormDemand(int characterID, MapField mapField)
        {
            throw new NotImplementedException();
        }

        public override void DoEndGame()
        {
            throw new NotImplementedException();
        }

        public override void DoGameEndMessage(int winnerID, int loserID, Statistics[] stats)
        {
            throw new NotImplementedException();
        }

        public override void OnJoinAcceptedMessage(JoinAcceptedMessage joinAcceptedMessage)
        {
            throw new NotImplementedException();
        }
    }
}
