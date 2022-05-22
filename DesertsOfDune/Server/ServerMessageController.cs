using System;
using System.Collections.Generic;
using GameData.network.controller;
using GameData.network.messages;
using GameData.network.util.world;
using Server.Clients;

namespace Server
{
    public class ServerMessageController : MessageController
    {
        private readonly Dictionary<string, Party> _parties; //Dictionary of all created parties

        public ServerMessageController()
        {
            _parties = new Dictionary<string, Party>();
        }

        /// <summary>
        /// Client sends a CreateMessage if he wants to create a new party.
        /// Therefore the client has to send a lobby for a unique identification of the party
        /// and the cpuCount which specifies how many AIPlayer participate.
        /// </summary>
        /// <param name="msg">CreateMessage with the info of lobbyCode and cpuCount</param>
        public void OnCreateMessage(CreateMessage msg)
        {
            //TODO: msg.Spectate?
            _parties.Add(msg.lobbyCode, new Party(msg.lobbyCode));
            //Console.WriteLine("- Party created");

            //send back ack or error message
        }

        /// <summary>
        /// Client requests to join a party with a clintName and a flag if he is player or spectator.
        /// To join to the party, the connectionCode from the JoinMessage has to be equal to the lobbyCode of the created party.
        /// </summary>
        /// <param name="msg">JoinMessage with the value clientName, connectionCode and active flag if he is a player.</param>
        public void OnJoinMessage(JoinMessage msg)
        {
            var clientName = msg.clientName;

            foreach (var party in _parties)
            {
                if(party.Key == msg.connectionCode)
                {
                    if (!msg.isCpu && msg.active) //client is a HumanPlayer
                    {
                        //distinction between AI and Human
                        var player = new Player(clientName, party.Key);
                        party.Value.AddPlayer(player);
                        //Console.WriteLine($"- Player {clientName} joined"); //test
                    }
                    else if(msg.isCpu && msg.active) //client is AIPlayer
                    {
                    }
                    else //client is spectator
                    {

                    }
                }
                else
                {
                    //errorMessage
                }
            }
        }

        public void OnHouseRequestMessage(HouseRequestMessage msg)
        {
            throw new NotImplementedException("not implemented");

            //receiving of chosen house of Player

            //string houseName

            //create  great house
        }

        public void OnMovementRequestMessage(MovementRequestMessage msg)
        {
            throw new NotImplementedException("not implemented");

            //request from client to move a character

            //int clientID
            //int characterID
            //Specs
            //path
        }

        public void OnActionRequestMessage(ActionRequestMessage msg)
        {
            throw new NotImplementedException("not implemented");

            //request from client to run an action

            //int clientID
            //int characterID
            //Action
            //Specs
            //target
            //int targetID
        }

        public void OnTransferRequestMessage(TransferReuqestMessage msg)
        {
            throw new NotImplementedException("not implemented");
        }

        public void OnEndTurnRequestMessage(EndTurnRequestMessage msg)
        {
            throw new NotImplementedException("not implemented");

            //End move phase prematurely

            //int clientID
            //int characterID
        }

        public void OnGameStateRequestMessage(GameStateRequestMessage msg)
        {
            throw new NotImplementedException("not implemented");

            //Requirement complete game state

            //int clientID
        }

        public void OnPauseGameRequestMessage(PauseGameRequestMessage msg)
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
        public void DoAcceptJoin(string clientSecret, int clientID)
        {
            JoinAcceptedMessage joinAcceptedMessage = new JoinAcceptedMessage(clientSecret, clientID);
            NetworkController.HandleSendingMessage(joinAcceptedMessage);
            Console.WriteLine("- Join accepted");
        }

        public void DoSendAck()
        {
            AckMessage ackMessage = new AckMessage();
            NetworkController.HandleSendingMessage(ackMessage);
        }

        public void DoSendError(int errorCode, string errorDescription)
        {
            ErrorMessage errorMessage = new ErrorMessage(errorCode, errorDescription);
            NetworkController.HandleSendingMessage(errorMessage);
        }

        public void DoSendGameConfig(List<string[]> scenario, string party, int client0ID, int client1ID)
        {
            GameConfigMessage gameConfigMessage = new GameConfigMessage(scenario, party, client0ID, client1ID);
            NetworkController.HandleSendingMessage(gameConfigMessage);
        }

        public void DoSendHouseOffer(int clientID, GreatHouse[] houses)
        {
            HouseOfferMessage houseOfferMessage = new HouseOfferMessage(clientID, houses);
            NetworkController.HandleSendingMessage(houseOfferMessage);
        }

        public void DoSendHouseAck(int clientID, string houseName)
        {
            HouseAcknowledgementMessage houseACKMessage = new HouseAcknowledgementMessage(clientID, houseName);
            NetworkController.HandleSendingMessage(houseACKMessage);
        }

        public void DoSendTurnDemand(int clientID, int characterID)
        {
            TurnDemandMessage turnDemandMessage = new TurnDemandMessage(clientID, characterID);
            NetworkController.HandleSendingMessage(turnDemandMessage);
        }

        public void DoSendMovementDemand(int clientID, int characterID, List<Position> path)
        {
            MovementDemandMessage movementDemandMessage = new MovementDemandMessage(clientID, characterID, path);
            NetworkController.HandleSendingMessage(movementDemandMessage);
        }

        public void DoSendActionDemand(int clientID, int characterID, ActionType action, Position target)
        {
            ActionDemandMessage actionDemandMessage = new ActionDemandMessage(clientID, characterID, action, target);
            NetworkController.HandleSendingMessage(actionDemandMessage);
        }

        public void DoSendTransferDemand(int clientID, int characterID, int targetID)
        {
            TransferDemandMessage transferDemandMessage = new TransferDemandMessage(clientID, characterID, targetID);
            NetworkController.HandleSendingMessage(transferDemandMessage);
        }

        public void DoSendChangeCharacterStatsDemand(int clientID, int characterID, CharacterStatistics stats)
        {
            ChangeCharacterStatisticsDemandMessage changeCharacterStatisticsDemandMessage = new ChangeCharacterStatisticsDemandMessage(clientID, characterID, stats);
            NetworkController.HandleSendingMessage(changeCharacterStatisticsDemandMessage);
        }

        public void DoSendMapChangeDemand(MapChangeReasons mapChangeReasons, MapField[,] newMap)
        {
            MapChangeDemandMessage mapChangeDemandMessage = new MapChangeDemandMessage(mapChangeReasons, newMap);
            NetworkController.HandleSendingMessage(mapChangeDemandMessage);
        }

        public void DoSpawnCharacterDemand(int clientID, int characterID, string characterName, Position pos, Character attributes)
        {
            SpawnCharacterDemandMessage spawnCharacterDemandMessage = new SpawnCharacterDemandMessage(clientID, characterID, characterName, pos, attributes);
            NetworkController.HandleSendingMessage(spawnCharacterDemandMessage);
        }

        public void DoChangePlayerSpiceDemand(int clientID, int newSpiceVal)
        {
            ChangePlayerSpiceDemandMessage changePlayerSpiceDemandMessage = new ChangePlayerSpiceDemandMessage(clientID, newSpiceVal);
            NetworkController.HandleSendingMessage(changePlayerSpiceDemandMessage);
        }

        public void DoSpawnSandwormDemand(int clientID, int characterID, Position pos)
        {
            SandwormSpawnDemandMessage sandwormSpawnDemandMessage = new SandwormSpawnDemandMessage(clientID, characterID, pos);
            NetworkController.HandleSendingMessage(sandwormSpawnDemandMessage);
        }

        public void DoMoveSandwormDemand(List<Position> path)
        {
            SandwormMoveDemandMessage sandwormMoveDemandMessage = new SandwormMoveDemandMessage(path);
            NetworkController.HandleSendingMessage(sandwormMoveDemandMessage);
        }

        public void DoDespawnSandwormDemand()
        {
            SandwormDespawnDemandMessage sandwormDespawnDemandMessage = new SandwormDespawnDemandMessage();
            NetworkController.HandleSendingMessage(sandwormDespawnDemandMessage);
        }

        public void DoEndGame()
        {
            EndGameMessage endGameMessage = new EndGameMessage();
            NetworkController.HandleSendingMessage(endGameMessage);
        }

        public void DoGameEndMessage(int winnerID, int loserID, Statistics stats)
        {
            GameEndMessage gameEndMessage = new GameEndMessage(winnerID, loserID, stats);
            NetworkController.HandleSendingMessage(gameEndMessage);
        }

        public void DoSendGameState(int clientID, int[] activlyPlayingIDs, String[] history)
        {
            GameStateMessage gameStateMessage = new GameStateMessage(history, activlyPlayingIDs, clientID);
            NetworkController.HandleSendingMessage(gameStateMessage);
        }

        public void DoSendStrike(int clientID, string wrongMessage, int count)
        {
            StrikeMessage strikeMessage = new StrikeMessage(clientID, wrongMessage, count);
            NetworkController.HandleSendingMessage(strikeMessage);
        }

        public void DoGamePauseDemand(int requestedByClientID, bool pause)
        {
            PausGameDemandMessage gamePauseDemandMessage = new PausGameDemandMessage(requestedByClientID, pause);
            NetworkController.HandleSendingMessage(gamePauseDemandMessage);
        }

        public void OnUnpauseGameOffer(int requestedByClientID)
        {
            UnpauseGameOfferMessage unpauseGameOfferMessage = new UnpauseGameOfferMessage(requestedByClientID);
            NetworkController.HandleSendingMessage(unpauseGameOfferMessage);
        }
    }
}