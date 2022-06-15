using GameData.network.messages;
using GameData.network.util.world;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData.network.controller;
using GameData.network.util.enums;
using System;
using Serilog;

/// <summary>
/// This Class Handles all messages for the Client.
/// </summary>
public class PlayerMessageController : MessageController
{
    public static PlayerMessageController instance;

    public PlayerMessageController()
    {
        if (instance == null) instance = this;
    }

    /// <summary>
    /// this method is responsible for requesting a Join
    /// </summary>
    /// <param name="clientName">the name of the client</param>
    /// <param name="active">weather or not the client is active or a spectator</param>
    /// <param name="isCpu">weather the client is a cpu or not</param>
    public void DoJoin(string clientName, bool active, bool isCpu)
    {
        Log.Debug("Starting Join!");
        SessionHandler.isPlayer = active;
        JoinMessage joinMessage = new JoinMessage(clientName, active, isCpu);
        Log.Debug("Parsing of Message successful");
        NetworkController.HandleSendingMessage(joinMessage);
        Log.Debug("Sent message!");
        //CharacterMgr.handler.
        //NetworkController.HandleSendingMessage(joinMessage);
        //   Debug.Log(CharacterMgr.handler.WebSocket.ToString());
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
        MovementRequestMessage movementRequestMessage = new MovementRequestMessage(clientID, characterID, new Specs(null, path));
        NetworkController.HandleSendingMessage(movementRequestMessage);
    }

    /// <summary>
    /// This method is responsible for requesting a action of a Character
    /// </summary>
    /// <param name="clientID">the id of the requesting client</param>
    /// <param name="characterID">the id of the character the action is requested for</param>
    /// <param name="action">the action that is requested</param>
    /// <param name="target">the target of the action</param>
    public void DoRequestAction(int clientID, int characterID, ActionType action, Position target)
    {
        ActionRequestMessage actionRequestMessage = new ActionRequestMessage(clientID, characterID, action, new Specs(target, null));
        NetworkController.HandleSendingMessage(actionRequestMessage);
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
    public override void OnJoinAccepted(JoinAcceptedMessage joinAcceptedMessage)
    {
        Log.Debug("Join Accepted!");
        // TODO: implement logic
        if (SessionHandler.isPlayer)
        {
            SessionHandler.clientId = joinAcceptedMessage.clientID;
            Debug.Log("Set client id to: " + SessionHandler.clientId);
        }
        else
        {
            SessionHandler.viewerId = joinAcceptedMessage.clientID;
            Debug.Log("Joined as viewer and set id to: " + SessionHandler.clientId);
        }
        SessionHandler.clientSecret = joinAcceptedMessage.clientSecret;

        IEnumerator demandPlaygame()
        {
            MainMenuManager.instance.DemandJoinAccept();
            yield return null;
        }
        Log.Debug("Got to ThreadDispatcher");
        UnityMainThreadDispatcher.Instance().Enqueue(demandPlaygame());

    }

    /// <summary>
    /// This method handles the GameConfigMessage
    /// </summary>
    /// <param name="gameConfigMessage">this message represents the game configuration of the Server</param>
    /// <returns></returns>
    public override void OnGameConfigMessage(GameConfigMessage gameConfigMessage)
    {
        // TODO: implement logic
        Log.Debug("Preparing Debug OnGameConfig");
        IEnumerator buildMap()
        {

            Debug.Log("Start Building map!");
            //Second list contains z size
            MapManager.instance.setMapSize(gameConfigMessage.scenario.Count, gameConfigMessage.scenario[0].Count);

            for (int x = 0; x < gameConfigMessage.scenario.Count; x++)
            {
                for (int z = 0; z < gameConfigMessage.scenario[0].Count; z++)
                {
                    Debug.Log("PreLoop Built x: " + x + " and z: " + z);
                    if (gameConfigMessage.stormEye != null && MapManager.instance.isNodeNeighbour(x, z, gameConfigMessage.stormEye.x, gameConfigMessage.stormEye.y))
                    {
                        //Node is in Sandstorm
                        MapManager.instance.UpdateBoard(x, z, false, MapManager.instance.StringtoNodeEnum(gameConfigMessage.scenario[x][z]), true);
                    }
                    else
                    {
                        MapManager.instance.UpdateBoard(x, z, false, MapManager.instance.StringtoNodeEnum(gameConfigMessage.scenario[x][z]), false);
                    }
                    Debug.Log("Built x: " + x + " and z: " + z);
                }
            }

            Debug.Log("Built Map!");
            if (gameConfigMessage.stormEye != null)
            {
                MapManager.instance.getNodeFromPos(gameConfigMessage.stormEye.x, gameConfigMessage.stormEye.y).SetSandstorm(true);
                MapManager.instance.SetStormEye(gameConfigMessage.stormEye.x, gameConfigMessage.stormEye.y);
            }
            Debug.Log("Checkpoint");
            Debug.Log("Pre Crash" + gameConfigMessage.cityToClient[0]);

            if (gameConfigMessage.cityToClient[0].clientID == SessionHandler.clientId)
            {
                SessionHandler.enemyClientId = gameConfigMessage.cityToClient[1].clientID;
            }
            else
            {
                SessionHandler.enemyClientId = gameConfigMessage.cityToClient[0].clientID;
            }
            Debug.Log("Soweit Clean");
            MapManager.instance.getNodeFromPos(gameConfigMessage.cityToClient[0].x, gameConfigMessage.cityToClient[0].y).cityOwnerId = gameConfigMessage.cityToClient[0].clientID;
            MapManager.instance.getNodeFromPos(gameConfigMessage.cityToClient[1].x, gameConfigMessage.cityToClient[1].y).cityOwnerId = gameConfigMessage.cityToClient[1].clientID;
            yield return null;
        }
        UnityMainThreadDispatcher.Instance().Enqueue(buildMap());
    }

    /// <summary>
    /// This method handles the MapChangeDemandMessage
    /// Method currently not updatet, wait for implementation of message
    /// </summary>
    /// <param name="mapChangeDemandMessage">this message represents the map change demanded by the server</param>
    /// <returns></returns>
    public override void OnMapChangeDemandMessage(MapChangeDemandMessage mapChangeDemandMessage)
    {

        mapChangeDemandMessage.
        for (int x = 0; x < mapChangeDemandMessage.newMap.GetLength(0); x++)
        {
            for (int z = 0; z < mapChangeDemandMessage.newMap.GetLength(1); z++)
            {
                Debug.Log("PreLoop Built x: " + x + " and z: " + z);
                if (gameConfigMessage.stormEye != null && MapManager.instance.isNodeNeighbour(x, z, gameConfigMessage.stormEye.x, gameConfigMessage.stormEye.y))
                {
                    
                    //Node is in Sandstorm
                    MapManager.instance.UpdateBoard(x, z, false, MapManager.instance.StringtoNodeEnum(gameConfigMessage.scenario[x][z]), true);
                }
                else
                {
                    MapManager.instance.UpdateBoard(x, z, false, MapManager.instance.StringtoNodeEnum(gameConfigMessage.scenario[x][z]), false);
                }
                Debug.Log("Built x: " + x + " and z: " + z);
            }
        }
    }

    /// <summary>
    /// This method handles the StrikeMessage
    /// </summary>
    /// <param name="strikeMessage">this message represents the reaction of the Server to a faulty behaviour of the client</param>
    /// <returns></returns>
    public override void OnStrikeMessage(StrikeMessage strikeMessage)
    {
        GUIHandler.BroadcastGameMessage(strikeMessage.wrongMessage);
    }

    /// <summary>
    /// This method handles the GameEndMessage
    /// </summary>
    /// <param name="gameEndMessage">this message represents the end of the game triggered by the Server</param>
    /// <returns></returns>
    public override void OnGameEndMessage(GameEndMessage gameEndMessage)
    {
        // TODO: implement logic

    }

    /// <summary>
    /// This method handles the GameStateMessage
    /// </summary>
    /// <param name="gameStateMessage">this message represents the game state the Server holds</param>
    /// <returns></returns>
    public override void OnGameStateMessage(GameStateMessage gameStateMessage)
    {
        //WE WILL NEVER NEED THIS
        //Do not implement
    }

    /// <summary>
    /// this method handles the GamePauseDemandMessage
    /// </summary>
    /// <param name="gamePauseDemandMessage">this message represents the game pause demand of the Server</param>
    /// <returns></returns>
    public override void OnPauseGameDemandMessage(GamePauseDemandMessage gamePauseDemandMessage)
    {
        if (gamePauseDemandMessage.pause)
        {
            InGameMenuManager.getInstance().DemandPauseGame(SessionHandler.clientId != gamePauseDemandMessage.requestedByClientID);
        }
        else
        {
            InGameMenuManager.getInstance().RequestUnpauseGame();
        }
    }

    /// <summary>
    /// This method handles the HouseOfferMessage
    /// Method currently not updatet, wait for implementation of message
    /// </summary>
    /// <param name="houseOfferMessage">this message represents the houseoffer of the Server</param>
    /// <returns></returns>
    public override void OnHouseOfferMessage(HouseOfferMessage houseOfferMessage)
    {
        //Log.Debug("Entered OnHouseOffer");
        // Debug.Log("Received clientId: " + houseOfferMessage.clientID + "; expected: " + SessionHandler.clientId);
        if (SessionHandler.clientId == houseOfferMessage.clientID)
        {
            Log.Debug("Entered House Offer Method");
            // TODO: implement logic
            if (UnityMainThreadDispatcher.Instance() == null) Log.Debug("Alles schrott hier");
            IEnumerator houseOff()
            {
                Debug.Log("IEnumerator");
                if (InGameMenuManager.getInstance() == null) Debug.Log("IngameMenuManager is null!");
                InGameMenuManager.getInstance().DemandStartHouseSelection(houseOfferMessage.houses[0].houseName, houseOfferMessage.houses[1].houseName);
                Debug.Log("log changed window!");
                yield return null;
            }

            UnityMainThreadDispatcher.Instance().Enqueue(houseOff());
            Log.Debug("Leaving House Offer");
        }
    }

    /// <summary>
    /// This method handles the HouseAcknowledgementMessage
    /// </summary>
    /// <param name="houseAcknowledgementMessage">this message represents the houseAcknowlegement of the Server</param>
    /// <returns></returns>
    public override void OnHouseAcknowledgementMessage(HouseAcknowledgementMessage houseAcknowledgementMessage)
    {
        // TODO: implement logic

        HouseEnum house;
        switch (houseAcknowledgementMessage.houseName)
        {
            case "CORRINO":
                house = HouseEnum.CORRINO;
                break;
            case "ATREIDES":
                house = HouseEnum.ATREIDES;
                break;
            case "HARKONNEN":
                house = HouseEnum.HARKONNEN;
                break;
            case "ORDOS":
                house = HouseEnum.ORDOS;
                break;
            case "RICHESE":
                house = HouseEnum.RICHESE;
                break;
            default:
                house = HouseEnum.VERNIUS;
                break;
        }

        if (houseAcknowledgementMessage.clientID == SessionHandler.clientId)
        {
            IEnumerator houseAckn()
            {
                InGameMenuManager.getInstance().DemandEndHouseSelection();

                yield return null;
            }
            CharacterMgr.instance.SetPlayerHouse(house);
            UnityMainThreadDispatcher.Instance().Enqueue(houseAckn());
        }
        else
        {
            CharacterMgr.instance.SetEnemyHouse(house);
        }
    }

    /// <summary>
    /// this method handles the TransferDemandMessage
    /// </summary>
    /// <param name="transferDemandMessage">this message represents the transfer demand of the Server</param>
    /// <returns></returns>
    public override void OnTransferDemandMessage(TransferDemandMessage transferDemandMessage)
    {
        Character c1 = CharacterMgr.instance.getCharScriptByID(transferDemandMessage.characterID);
        Character c2 = CharacterMgr.instance.getCharScriptByID(transferDemandMessage.targetID);
        c1.Action_TransferSpiceExecution(c2);
    }

    /// <summary>
    /// This method handles the TurnRequestMessage
    /// </summary>
    /// <param name="turnDemandMessage">this message represents the TurnRequest of the Server</param>
    /// <returns></returns>
    public override void OnTurnDemandMessage(TurnDemandMessage turnDemandMessage)
    {
        // TODO: implement logic
        CharacterTurnHandler.instance.SetTurnChar(CharacterMgr.instance.getCharScriptByID(turnDemandMessage.clientID));
    }

    /// <summary>
    /// This method handles the MovementDemandMessage
    /// </summary>
    /// <param name="movementDemandMessage">This message represents the movement demand of the Server</param>
    /// <returns></returns>
    public override void OnMovementDemandMessage(MovementDemandMessage movementDemandMessage)
    {
        // TODO: implement logic
        MovementManager.instance.AnimateChar(CharacterMgr.instance.getCharScriptByID(movementDemandMessage.characterID), movementDemandMessage.specs.path);
    }

    /// <summary>
    /// This method handles the ActionDemandMessage
    /// </summary>
    /// <param name="actionDemandMessage">This message represents the action demand of the Server</param>
    /// <returns></returns>
    public override void OnActionDemandMessage(ActionDemandMessage actionDemandMessage)
    {
        // TODO: implement logic
        Character character = CharacterMgr.instance.getCharScriptByID(actionDemandMessage.characterID);
        Character enemy;
        switch (actionDemandMessage.action)
        {
            case "ATTACK":
                enemy = MapManager.instance.GetCharOnNode(actionDemandMessage.specs.target.x, actionDemandMessage.specs.target.y);
                character.Attack_BasicExecution(enemy);
                break;
            case "COLLECT":
                character.Action_CollectSpiceExecution();
                break;
            case "KANLY":
                enemy = MapManager.instance.GetCharOnNode(actionDemandMessage.specs.target.x, actionDemandMessage.specs.target.y);
                character.Attack_KanlyExecution(enemy);
                break;
            case "FAMILY_ATOMICS":
                character.Attack_AtomicExecution(MapManager.instance.getNodeFromPos(actionDemandMessage.specs.target.x, actionDemandMessage.specs.target.y));
                break;
            case "SPICE_HOARDING":
                character.Action_SpiceHoardingExecution();
                break;
            case "VOICE":
                enemy = MapManager.instance.GetCharOnNode(actionDemandMessage.specs.target.x, actionDemandMessage.specs.target.y);
                character.Action_VoiceExecution(enemy);
                break;
            case "SWORDSPIN":
                character.Attack_SwordSpinExecution();
                break;
        }
    }

    /// <summary>
    /// This method handles the ChangeCharacterStatisticsDemandMessage
    /// </summary>
    /// <param name="changeCharacterStatisticsDemandMessage">this message represents the demand to change the character statistics of the server</param>
    /// <returns></returns>
    public override void OnChangeCharacterStatisticsDemandMessage(ChangeCharacterStatisticsDemandMessage changeCharacterStatisticsDemandMessage)
    {
        //All fields currently private
        //changeCharacterStatisticsDemandMessage.stats.
        // TODO: implement logic
        Character character = CharacterMgr.instance.getCharScriptByID(changeCharacterStatisticsDemandMessage.characterID);
        character.UpdateCharStats(changeCharacterStatisticsDemandMessage.stats.HP, changeCharacterStatisticsDemandMessage.stats.MP, changeCharacterStatisticsDemandMessage.stats.AP, changeCharacterStatisticsDemandMessage.stats.spice, changeCharacterStatisticsDemandMessage.stats.isLoud, changeCharacterStatisticsDemandMessage.stats.isSwallowed);
    }


    /// <summary>
    /// Method currently not updatet, wait for implementation of message
    /// This method handles the SpawnCharacterDemandMessage
    /// </summary>
    /// <param name="spawnCharacterDemandMessage"></param>
    /// <returns></returns>
    public override void OnSpawnCharacterDemandMessage(SpawnCharacterDemandMessage spawnCharacterDemandMessage)
    {
        // TODO: implement logic
        //spawnCharacterDemandMessage.
        Log.Debug("Trigged OnSpawnCharacterDemandMessage in Client. Starting Main Function...");
        IEnumerator spawnCharacters()
        {
            Debug.Log("Started IEnumerator");
            Log.Debug("Ienumerator startup");
            Debug.Log("Character Attributes: " + (spawnCharacterDemandMessage.attributes == null));
            CharTypeEnum type;
            switch (spawnCharacterDemandMessage.attributes.characterType)
            {
                case "FIGHTER":
                    type = CharTypeEnum.FIGHTER;
                    break;
                case "NOBLE":
                    type = CharTypeEnum.NOBLE;
                    break;
                case "MENTAT":
                    type = CharTypeEnum.MENTANT;
                    break;
                default:
                    type = CharTypeEnum.BENEGESSERIT;
                    break;
            }
            Debug.Log("Selection successful");

            CharacterMgr.instance.spawnCharacter(spawnCharacterDemandMessage.clientID, spawnCharacterDemandMessage.characterID, type, spawnCharacterDemandMessage.position.x, spawnCharacterDemandMessage.position.y, spawnCharacterDemandMessage.attributes.healthCurrent, spawnCharacterDemandMessage.attributes.MPcurrent, spawnCharacterDemandMessage.attributes.APcurrent, spawnCharacterDemandMessage.attributes.APmax, spawnCharacterDemandMessage.attributes.inventoryUsed, spawnCharacterDemandMessage.attributes.KilledBySandworm, spawnCharacterDemandMessage.attributes.IsLoud());
            yield return null;
        }
        UnityMainThreadDispatcher.Instance().Enqueue(spawnCharacters());
    }

    /// <summary>
    /// This method handles the ChangePlayerSpiceDemandMessage
    /// </summary>
    /// <param name="changePlayerSpiceDemandMessage"></param>
    /// <returns></returns>
    public override void OnChangePlayerSpiceDemandMessage(ChangePlayerSpiceDemandMessage changePlayerSpiceDemandMessage)
    {
        // TODO: implement logic
        if (SessionHandler.clientId == changePlayerSpiceDemandMessage.clientID)
        {
            GUIHandler.UpdatePlayerSpice(changePlayerSpiceDemandMessage.newSpiceValue);
        }
        else
        {
            GUIHandler.UpdateEnemySpice(changePlayerSpiceDemandMessage.newSpiceValue);
        }
    }

    /// <summary>
    /// This method handles the SandwormSpawnDemandMessage
    /// </summary>
    /// <param name="sandwormSpawnDemandMessage"></param>
    /// <returns></returns>
    public override void OnSandwormSpawnDemandMessage(SandwormSpawnDemandMessage sandwormSpawnDemandMessage)
    {
        // TODO: implement logic
        CharacterMgr.instance.SpawnSandworm(sandwormSpawnDemandMessage.position.x, sandwormSpawnDemandMessage.position.y);
    }

    /// <summary>
    /// This method handles the SandwormMoveDemandMessage
    /// </summary>
    /// <param name="sandwormMoveMessage"></param>
    /// <returns></returns>
    public override void OnSandwormMoveDemandMessage(SandwormMoveDemandMessage sandwormMoveMessage)
    {
        // TODO: implement logic
        CharacterMgr.instance.SandwormMove(sandwormMoveMessage.path);
    }

    /// <summary>
    /// This method handles the SandwormDespawnDemandMessage
    /// </summary>
    /// <param name="sandwormDespawnDemandMessage"></param>
    /// <returns></returns>
    public override void OnSandwormDespawnMessage(SandwormDespawnDemandMessage sandwormDespawnDemandMessage)
    {
        // TODO: implement logic
        CharacterMgr.instance.DespawnSandworm();
    }

    /// <summary>
    /// This method handles the EndGameMessage
    /// </summary>
    /// <param name="endGameMessage"></param>
    /// <returns></returns>
    public override void OnEndGameMessage(EndGameMessage endGameMessage)
    {
        // TODO: implement logic
    }

    /// <summary>
    /// This method handles the AtomicsUpdateDemandMessage
    /// </summary>
    /// <param name="atomicUpdateDemandMessage">this message represents the demand to update the atomic by the server</param>
    public override void OnAtomicsUpdateDemandMessage(AtomicsUpdateDemandMessage atomicUpdateDemandMessage)
    {
        // TODO: implement logic
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

    public override void DoSendJoin(string clientName)
    {
        throw new NotImplementedException();
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

    public override void OnJoinAcceptedMessage(JoinAcceptedMessage joinAcceptedMessage)
    {
        throw new NotImplementedException();
    }

    public override void DoSendMapChangeDemand(MapChangeReasons mapChangeReasons)
    {
        throw new NotImplementedException();
    }
}
