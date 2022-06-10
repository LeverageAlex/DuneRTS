using System;
using System.Collections.Generic;
using GameData.network.controller;
using GameData.network.messages;
using GameData.network.util.enums;
using GameData.network.util.world;

namespace AIClient
{
    public class AIPlayerMessageController : MessageController
    {
        public AIPlayerMessageController()
        {
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

        public override void DoEndGame()
        {
            throw new NotImplementedException();
        }

        public override void DoGameEndMessage(int winnerID, int loserID, Statistics stats)
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

        public override void DoSendMapChangeDemand(MapChangeReasons mapChangeReasons, MapField[,] newMap)
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

        public override void OnActionRequestMessage(ActionRequestMessage msg)
        {
            throw new NotImplementedException();
        }

        public override void OnEndTurnRequestMessage(EndTurnRequestMessage msg)
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

        public override void OnMovementRequestMessage(MovementRequestMessage msg)
        {
            throw new NotImplementedException();
        }

        public override void OnPauseGameRequestMessage(PauseGameRequestMessage msg)
        {
            throw new NotImplementedException();
        }

        public override void OnRejoinMessage(RejoinMessage msg, string sessionID)
        {
            throw new NotImplementedException();
        }

        public override void OnTransferRequestMessage(TransferRequestMessage msg)
        {
            throw new NotImplementedException();
        }

        public override void OnUnpauseGameOffer(int requestedByClientID)
        {
            throw new NotImplementedException();
        }
    }
}
