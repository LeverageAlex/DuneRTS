using System;
using System.Collections.Generic;
using GameData.network.controller;
using GameData.network.messages;
using GameData.network.util.parser;
using GameData.network.util.world;

namespace Server
{
    public class ServerMessageController : MessageController
    {

        public ServerMessageController()
        {
        }

        public void OnCreateMessage(CreateMessage msg)
        {
            throw new NotImplementedException("not implemented");
            //Client sendet CreateMsg wenn er eine neue Partie erstellen möchte

            //Party party = new Party();

            //string lobbyCode
            //int cpuCount
        }

        public void OnJoinMessage(JoinMessage msg)
        {
            throw new NotImplementedException("not implemented");
            //Server erwartet Registrierung von Client
            //wird auch beim reconnect verwendet

            //string clientName
            //string connectionCode
            //bool active
        }

        public void OnHouseRequestMessage(HouseRequestMessage msg)
        {
            throw new NotImplementedException("not implemented");
        }

        public void OnMovementRequestMessage(MovementRequestMessage msg)
        {
            throw new NotImplementedException("not implemented");
        }

        public void OnActionRequestMessage(ActionRequestMessage msg)
        {
            throw new NotImplementedException("not implemented");
        }

        public void OnEndTurnRequestMessage(EndTurnRequestMessage msg)
        {
            throw new NotImplementedException("not implemented");
        }

        public void OnGameStateRequestMessage(GameStateRequestMessage msg)
        {
            throw new NotImplementedException("not implemented");
        }

        public void OnPauseGameRequestMessage(PauseGameRequestMessage msg)
        {
            throw new NotImplementedException("not implemented");
        }



        /// <summary>
        /// /// The server is sending a JoinAcceptedMessage if the join was successful
        /// TODO: what is sending back if a exception is thrown?
        /// </summary>
        /// <param name="clientSecret">Unique identifikator for the client, which is just known between the affected parties</param>
        public void DoAcceptJoin(string clientSecret)
        {
            JoinAcceptedMessage joinAcceptedMessage = new JoinAcceptedMessage(clientSecret);
            controller.HandleSendingMessage(joinAcceptedMessage);
        }

        public void DoSendGameConfig(List<string[]> scenario, string party, string[] houseOffer)
        {
            GameConfigMessage gameConfigMessage = new GameConfigMessage(scenario, party, houseOffer);
            controller.HandleSendingMessage(gameConfigMessage);
        }

        public void DoSendHouseOffer(int clientID, GreatHouse[] houses)
        {
            HouseOfferMessage houseOfferMessage = new HouseOfferMessage(clientID, houses);
            controller.HandleSendingMessage(houseOfferMessage);
        }

        public void DoSendHouseAck(int clientID, string houseName)
        {
            HouseAcknowledgementMessage houseACKMessage = new HouseAcknowledgementMessage(clientID, houseName);
            controller.HandleSendingMessage(houseACKMessage);
        }

        public void DoSendTurnDemand(int clientID, int characterID)
        {
            TurnDemandMessage turnDemandMessage = new TurnDemandMessage(clientID, characterID);
            controller.HandleSendingMessage(turnDemandMessage);
        }

        public void DoSendStrike(int clientID, string wrongMessage, int count)
        {
            StrikeMessage strikeMessage = new StrikeMessage(clientID, wrongMessage, count);
            controller.HandleSendingMessage(strikeMessage);
        }

        public void DoSendMovementDemand(int clientID, int characterID, List<Position> path)
        {
            MovementDemandMessage movementDemandMessage = new MovementDemandMessage(clientID, characterID, path);
            controller.HandleSendingMessage(movementDemandMessage);
        }

        public void DoSendActionDemand(int clientID, int characterID, ActionType action, Position target, int targetID)
        {
            ActionDemandMessage actionDemandMessage = new ActionDemandMessage(clientID, characterID, action, target, targetID);
            controller.HandleSendingMessage(actionDemandMessage);
        }

        public void DoSendChangeCharacterStatsDemand(int clientID, int characterID, CharacterStatistics stats)
        {
            ChangeCharacterStatisticsDemandMessage changeCharacterStatisticsDemandMessage = new ChangeCharacterStatisticsDemandMessage(clientID, characterID, stats);
            controller.HandleSendingMessage(changeCharacterStatisticsDemandMessage);
        }

        public void DoSendMapChangeDemand(MapChangeReasons mapChangeReasons, MapField[][] newMap)
        {
            MapChangeDemandMessage mapChangeDemandMessage = new MapChangeDemandMessage(mapChangeReasons, newMap);
            controller.HandleSendingMessage(mapChangeDemandMessage);
        }

        public void DoSpawnCharacterDemand(int clientID, int characterID, string characterName, Position pos, Character attributes, CharacterType characterType)
        {
            SpawnCharacterDemandMessage spawnCharacterDemandMessage = new SpawnCharacterDemandMessage(clientID, characterID, characterName, pos, attributes, characterType);
            controller.HandleSendingMessage(spawnCharacterDemandMessage);
        }

        public void DoChangePlayerSpiceDemand(int clientID, int newSpiceVal)
        {
            ChangePlayerSpiceDemandMessage changePlayerSpiceDemandMessage = new ChangePlayerSpiceDemandMessage(clientID, newSpiceVal);
            controller.HandleSendingMessage(changePlayerSpiceDemandMessage);
        }

        public void DoSpawnSandwormDemand(int clientID, int characterID, Position pos)
        {
            SandwormSpawnDemandMessage sandwormSpawnDemandMessage = new SandwormSpawnDemandMessage(clientID, characterID, pos);
            controller.HandleSendingMessage(sandwormSpawnDemandMessage);
        }

        public void DoMoveSandwormDemand(List<Position> path)
        {
            SandwormMoveDemandMessage sandwormMoveDemandMessage = new SandwormMoveDemandMessage(path);
            controller.HandleSendingMessage(sandwormMoveDemandMessage);
        }

        public void DoDespawnSandwormDemand()
        {
            SandwormDespawnDemandMessage sandwormDespawnDemandMessage = new SandwormDespawnDemandMessage();
            controller.HandleSendingMessage(sandwormDespawnDemandMessage);
        }

        public void DoEndGame(int winnerID, int loserID, Statistics stats)
        {
            GameEndMessage gameEndMessage = new GameEndMessage(winnerID, loserID, stats);
            controller.HandleSendingMessage(gameEndMessage);
        }

        public void DoSendGameState(int clientID, String[] history)
        {
            GameStateMessage gameStateMessage = new GameStateMessage(history, clientID);
            controller.HandleSendingMessage(gameStateMessage);
        }

        public void DoPauseGame(int requestedByClientID, bool pause)
        {
            PauseGameMessage pauseGameMessage = new PauseGameMessage(requestedByClientID, pause);
            controller.HandleSendingMessage(pauseGameMessage);
        }
    }
}