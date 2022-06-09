using GameData.network.messages;
using GameData.network.util.world;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData.network.controller;
using GameData.network.util.enums;

/// <summary>
/// This Class Handles all messages for the Client.
/// </summary>
public class PlayerMessageController : MessageController
{

    /// <summary>
    /// this method is responsible for requesting a Join
    /// </summary>
    /// <param name="clientName">the name of the client</param>
    /// <param name="active">weather or not the client is active or a spectator</param>
    /// <param name="isCpu">weather the client is a cpu or not</param>
    public void DoJoin(string clientName, bool active, bool isCpu)
    {
        JoinMessage joinMessage = new JoinMessage(clientName, active, isCpu);
        NetworkController.HandleSendingMessage(joinMessage);

    }

    /// <summary>
    /// This method is responsible for requesting the game state
    /// </summary>
    /// <param name="clientID"></param>
    public void DoRequestGameState(int clientID)
    {
        GameStateRequestMessage gameStateRequestMessage = new GameStateRequestMessage(clientID);
        NetworkController.HandleSendingMessage(gameStateRequestMessage);
    }

    /// <summary>
    /// This method is responsible for requesting a specific house
    /// </summary>
    /// <param name="houseName">the name of the house to be requested</param>
    public void DoRequestHouse(string houseName)
    {
        HouseRequestMessage houseRequestMessage = new HouseRequestMessage(houseName);
        NetworkController.HandleSendingMessage(houseRequestMessage);
    }

    /// <summary>
    /// This method is responsible for requesting the movement of a Character
    /// </summary>
    /// <param name="clientID">the id of the requesting client</param>
    /// <param name="characterID">the id of the character the movement is requested for</param>
    /// <param name="path">the requested path for the character</param>
    public void DoRequestMovement(int clientID, int characterID, List<Position> path)
    {

        MovementRequestMessage movementRequestMessage = new MovementRequestMessage(clientID, characterID, path);
        NetworkController.HandleSendingMessage(movementRequestMessage);
    }

    /// <summary>
    /// This method is responsible for requesting a action of a Character
    /// </summary>
    /// <param name="clientID">the id of the requesting client</param>
    /// <param name="characterID">the id of the character the action is requested for</param>
    /// <param name="action">the action that is requested</param>
    /// <param name="target">the target of the action</param>
    /// <param name="targetID">the id of the targetCharacter</param>
    public void DoRequestAction(int clientID, int characterID, ActionType action, Position target, int targetID)
    {
        ActionDemandMessage actionDemandMessage = new ActionDemandMessage(clientID, characterID, action, target);
        NetworkController.HandleSendingMessage(actionDemandMessage);
    }

    /// <summary>
    /// This method is responsible for requesting the end of a Turn.
    /// </summary>
    /// <param name="clientID">the id of the client</param>
    /// <param name="characterID">the id of the character</param>
    public void DoRequestEndTurn(int clientID, int characterID)
    {
        EndTurnRequestMessage endTurnRequestMessage = new EndTurnRequestMessage(clientID, characterID);
        NetworkController.HandleSendingMessage(endTurnRequestMessage);
    }

    /// <summary>
    /// This method handles the HouseOfferMessage
    /// </summary>
    /// <param name="joinAcceptedMessage">this message represents the join acceptance of the Server</param>
    /// <returns></returns>
    public override Message OnJoinAcceptedMessage(JoinAcceptedMessage joinAcceptedMessage)
    {
        // TODO: implement logic
        return null;
    }

    /// <summary>
    /// This method handles the GameConfigMessage
    /// </summary>
    /// <param name="gameConfigMessage">this message represents the game configuration of the Server</param>
    /// <returns></returns>
    public override Message OnGameConfigMessage(GameConfigMessage gameConfigMessage)
    {
        // TODO: implement logic
        return null;
    }

    /// <summary>
    /// This method handles the MapChangeDemandMessage
    /// </summary>
    /// <param name="mapChangeDemandMessage">this message represents the map change demanded by the server</param>
    /// <returns></returns>
    public override Message OnMapChangeDemandMessage(MapChangeDemandMessage mapChangeDemandMessage)
    {
        // TODO: implement logic
        return null;
    }

    /// <summary>
    /// This method handles the StrikeMessage
    /// </summary>
    /// <param name="strikeMessage">this message represents the reaction of the Server to a faulty behaviour of the client</param>
    /// <returns></returns>
    public override Message OnStrikeMessage(StrikeMessage strikeMessage)
    {
        // TODO: implement logic
        return null;
    }

    /// <summary>
    /// This method handles the GameEndMessage
    /// </summary>
    /// <param name="gameEndMessage">this message represents the end of the game triggered by the Server</param>
    /// <returns></returns>
    public override Message OnGameEndMessage(GameEndMessage gameEndMessage)
    {
        // TODO: implement logic
        return null;
    }

    /// <summary>
    /// This method handles the GameStateMessage
    /// </summary>
    /// <param name="gameStateMessage">this message represents the game state the Server holds</param>
    /// <returns></returns>
    public override Message OnGameStateMessage(GameStateMessage gameStateMessage)
    {
        // TODO: implement logic
        return null;
    }

    /// <summary>
    /// this method handles the GamePauseDemandMessage
    /// </summary>
    /// <param name="gamePauseDemandMessage">this message represents the game pause demand of the Server</param>
    /// <returns></returns>
    public override Message OnPauseGameDemandMessage(GamePauseDemandMessage gamePauseDemandMessage)
    {
        // TODO: implement logic
        return null;
    }

    /// <summary>
    /// This method handles the HouseOfferMessage
    /// </summary>
    /// <param name="houseOfferMessage">this message represents the houseoffer of the Server</param>
    /// <returns></returns>
    public override Message OnHouseOfferMessage(HouseOfferMessage houseOfferMessage)
    {
        // TODO: implement logic
        return null;
    }

    /// <summary>
    /// This method handles the HouseAcknowledgementMessage
    /// </summary>
    /// <param name="houseAcknowledgementMessage">this message represents the houseAcknowlegement of the Server</param>
    /// <returns></returns>
    public override Message OnHouseAcknowledgementMessage(HouseAcknowledgementMessage houseAcknowledgementMessage)
    {
        // TODO: implement logic
        return null;
    }

    /// <summary>
    /// this method handles the TransferDemandMessage
    /// </summary>
    /// <param name="transferDemandMessage">this message represents the transfer demand of the Server</param>
    /// <returns></returns>
    public override Message OnTransferDemandMessage(TransferDemandMessage transferDemandMessage)
    {
        // TODO: implement logic
        return null;
    }

    /// <summary>
    /// This method handles the TurnRequestMessage
    /// </summary>
    /// <param name="turnDemandMessage">this message represents the TurnRequest of the Server</param>
    /// <returns></returns>
    public override Message OnTurnDemandMessage(TurnDemandMessage turnDemandMessage)
    {
        // TODO: implement logic
        return null;
    }

    /// <summary>
    /// This method handles the MovementDemandMessage
    /// </summary>
    /// <param name="movementDemandMessage">This message represents the movement demand of the Server</param>
    /// <returns></returns>
    public override Message OnMovementDemandMessage(MovementDemandMessage movementDemandMessage)
    {
        // TODO: implement logic
        return null;
    }

    /// <summary>
    /// This method handles the ActionDemandMessage
    /// </summary>
    /// <param name="actionDemandMessage">This message represents the action demand of the Server</param>
    /// <returns></returns>
    public override Message OnActionDemandMessage(ActionDemandMessage actionDemandMessage)
    {
        // TODO: implement logic
        return null;
    }

    /// <summary>
    /// This method handles the ChangeCharacterStatisticsDemandMessage
    /// </summary>
    /// <param name="changeCharacterStatisticsDemandMessage">this message represents the demand to change the character statistics of the server</param>
    /// <returns></returns>
    public override Message OnChangeCharacterStatisticsDemandMessage(ChangeCharacterStatisticsDemandMessage changeCharacterStatisticsDemandMessage)
    {
        // TODO: implement logic
        return null;
    }


    /// <summary>
    /// This method handles the SpawnCharacterDemandMessage
    /// </summary>
    /// <param name="spawnCharacterDemandMessage"></param>
    /// <returns></returns>
    public override Message OnSpawnCharacterDemandMessage(SpawnCharacterDemandMessage spawnCharacterDemandMessage)
    {
        // TODO: implement logic
        return null;
    }

    /// <summary>
    /// This method handles the ChangePlayerSpiceDemandMessage
    /// </summary>
    /// <param name="changePlayerSpiceDemandMessage"></param>
    /// <returns></returns>
    public override Message OnChangePlayerSpiceDemandMessage(ChangePlayerSpiceDemandMessage changePlayerSpiceDemandMessage)
    {
        // TODO: implement logic
        return null;
    }

    /// <summary>
    /// This method handles the SandwormSpawnDemandMessage
    /// </summary>
    /// <param name="sandwormSpawnDemandMessage"></param>
    /// <returns></returns>
    public override Message OnSandwormSpawnDemandMessage(SandwormSpawnDemandMessage sandwormSpawnDemandMessage)
    {
        // TODO: implement logic
        return null;
    }

    /// <summary>
    /// This method handles the SandwormMoveDemandMessage
    /// </summary>
    /// <param name="sandwormMoveMessage"></param>
    /// <returns></returns>
    public override Message OnSandwormMoveDemandMessage(SandwormMoveDemandMessage sandwormMoveMessage)
    {
        // TODO: implement logic
        return null;
    }

    /// <summary>
    /// This method handles the SandwormDespawnDemandMessage
    /// </summary>
    /// <param name="sandwormDespawnDemandMessage"></param>
    /// <returns></returns>
    public override Message OnSandwormDespawnMessage(SandwormDespawnDemandMessage sandwormDespawnDemandMessage)
    {
        // TODO: implement logic
        return null;
    }

    /// <summary>
    /// This method handles the EndGameMessage
    /// </summary>
    /// <param name="endGameMessage"></param>
    /// <returns></returns>
    public override Message OnEndGameMessage(EndGameMessage endGameMessage)
    {
        // TODO: implement logic
        return null;
    }

    // This method should not be called by the client.
    public override void OnJoinMessage(JoinMessage msg, string sessionID)
    {
        throw new System.NotImplementedException();
    }

    // This method should not be called by the client.
    public override void OnRejoinMessage(RejoinMessage msg, string sessionID)
    {
        throw new System.NotImplementedException();
    }

    // This method should not be called by the client.
    public override void OnHouseRequestMessage(HouseRequestMessage msg, string sessionID)
    {
        throw new System.NotImplementedException();
    }

    // This method should not be called by the client.
    public override void OnMovementRequestMessage(MovementRequestMessage msg)
    {
        throw new System.NotImplementedException();
    }

    // This method should not be called by the client.
    public override void OnActionRequestMessage(ActionRequestMessage msg)
    {
        throw new System.NotImplementedException();
    }

    // This method should not be called by the client.
    public override void OnTransferRequestMessage(TransferRequestMessage msg)
    {
        throw new System.NotImplementedException();
    }

    // This method should not be called by the client.
    public override void OnEndTurnRequestMessage(EndTurnRequestMessage msg)
    {
        throw new System.NotImplementedException();
    }

    // This method should not be called by the client.
    public override void OnGameStateRequestMessage(GameStateRequestMessage msg)
    {
        throw new System.NotImplementedException();
    }

    // This method should not be called by the client.
    public override void OnPauseGameRequestMessage(PauseGameRequestMessage msg)
    {
        throw new System.NotImplementedException();
    }

    // This method should not be called by the client.
    public override void DoAcceptJoin(string clientSecret, int clientID, string sessionID)
    {
        throw new System.NotImplementedException();
    }

    // This method should not be called by the client.
    public override void DoSendError(int errorCode, string errorDescription, string sessionID)
    {
        throw new System.NotImplementedException();
    }

    // This method should not be called by the client.
    public override void DoSendGameConfig()
    {
        throw new System.NotImplementedException();
    }

    // This method should not be called by the client.
    public override void DoSendHouseOffer(int clientID, GreatHouseType[] houses)
    {
        throw new System.NotImplementedException();
    }

    // This method should not be called by the client.
    public override void DoSendHouseAck(int clientID, string houseName)
    {
        throw new System.NotImplementedException();
    }

    // This method should not be called by the client.
    public override void DoSendTurnDemand(int clientID, int characterID)
    {
        throw new System.NotImplementedException();
    }

    // This method should not be called by the client.
    public override void DoSendMovementDemand(int clientID, int characterID, List<Position> path)
    {
        throw new System.NotImplementedException();
    }

    // This method should not be called by the client.
    public override void DoSendActionDemand(int clientID, int characterID, ActionType action, Position target)
    {
        throw new System.NotImplementedException();
    }

    // This method should not be called by the client.
    public override void DoSendTransferDemand(int clientID, int characterID, int targetID)
    {
        throw new System.NotImplementedException();
    }

    // This method should not be called by the client.
    public override void DoSendChangeCharacterStatsDemand(int clientID, int characterID, CharacterStatistics stats)
    {
        throw new System.NotImplementedException();
    }

    // This method should not be called by the client.
    public override void DoSendMapChangeDemand(MapChangeReasons mapChangeReasons, MapField[,] newMap)
    {
        throw new System.NotImplementedException();
    }

    // This method should not be called by the client.
    public override void DoSendAtomicsUpdateDemand(int clientID, bool shunned, int atomicsLeft)
    {
        throw new System.NotImplementedException();
    }

    // This method should not be called by the client.
    public override void DoSpawnCharacterDemand(GameData.network.util.world.Character attributes)
    {
        throw new System.NotImplementedException();
    }

    // This method should not be called by the client.
    public override void DoChangePlayerSpiceDemand(int clientID, int newSpiceVal)
    {
        throw new System.NotImplementedException();
    }

    // This method should not be called by the client.
    public override void DoSpawnSandwormDemand(int characterID, MapField mapField)
    {
        throw new System.NotImplementedException();
    }

    // This method should not be called by the client.
    public override void DoMoveSandwormDemand(List<MapField> list)
    {
        throw new System.NotImplementedException();
    }

    // This method should not be called by the client.
    public override void DoDespawnSandwormDemand()
    {
        throw new System.NotImplementedException();
    }

    // This method should not be called by the client.
    public override void DoEndGame()
    {
        throw new System.NotImplementedException();
    }

    // This method should not be called by the client.
    public override void DoGameEndMessage(int winnerID, int loserID, Statistics stats)
    {
        throw new System.NotImplementedException();
    }

    // This method should not be called by the client.
    public override void DoSendGameState(int clientID, int[] activlyPlayingIDs, string[] history)
    {
        throw new System.NotImplementedException();
    }

    // This method should not be called by the client.
    public override void DoSendStrike(int clientID, Message wrongMessage)
    {
        throw new System.NotImplementedException();
    }

    // This method should not be called by the client.
    public override void DoGamePauseDemand(int requestedByClientID, bool pause)
    {
        throw new System.NotImplementedException();
    }

    // This method should not be called by the client.
    public override void OnUnpauseGameOffer(int requestedByClientID)
    {
        throw new System.NotImplementedException();
    }
}
