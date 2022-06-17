using System;
using GameData.network.messages;
using Serilog;
using GameData.network.util.parser;
using GameData.network.util.world;
using System.Collections.Generic;
using GameData.network.util.enums;

namespace GameData.network.controller
{
    /// <summary>
    /// Base class of all message controllers
    /// </summary>
    /// <remarks>
    /// The message controller implement the minimal functionality of any message controller.
    /// In this case, every message controller must deal with debug messages. \n
    /// In general the purpose of a message controller is to create the messages from the "context"
    /// and provide callbacks, that execute code and interact with the "context", if a certain message is received.
    /// So the message controller is kind of the interface from network (and outside world) with the inner "context", which
    /// is e. g. the server or the spectator client. \n
    /// Therefore is has do<Message>() and on<Message> methods for every message type, the context need to implement.
    /// The do-Methods always get information and create a message from this information and tell the network controller to send it
    /// and the on-Methods are always triggered by the network controller (respectively connection handler) and parse as well as
    /// process a message and so effect the "context". 
    /// </remarks>
    public abstract class MessageController
    {
        /// <summary>
        /// parent network controller, that contains this message controller (ref. needed so give the message, which need
        /// to be send, to the fitting network controller)
        /// </summary>
        public NetworkController NetworkController { get; set; }

        /// <summary>
        /// creates a new message controller
        /// </summary>
        protected MessageController()
        {
            //TODO: remove contructor?
        }

        /// <summary>
        /// processes a debug message, so log this information (to console)
        /// </summary>
        /// <remarks>
        /// is triggered, if the network controller receives a message of the type "DEBUG" and forward it to this callback
        /// </remarks>
        /// <param name="message">the incoming debug message from network</param>
        /// TODO: check, what the purpose of the debug message is and adjust the implementation
        public void OnDebugMessage(DebugMessage message)
        {
            // extract the information from the debug message
            int code = message.code;
            string explanation = message.explanation;

            // log the information to console
            Log.Debug("Received debug message with code: " + code + " because of: " + explanation);
        }

        /// <summary>
        /// create debug message and trigger the network controller to send this message
        /// </summary>
        /// <param name="code">code number for categorization of the event</param>
        /// <param name="explanation">further explanation of the event / code number</param>
        /// <example>
        /// For instance, the variables could have the following values for describing a debug message
        /// <code>
        /// code = 404
        /// explanation = service is not avaible
        /// </code>
        /// </example>
        public void DoDebug(int code, string explanation)
        {
            // create Debug message
            DebugMessage message = new DebugMessage(code, explanation);

            // send message
            NetworkController.HandleSendingMessage(message);
        }

        public abstract void OnJoinMessage(JoinMessage msg, string sessionID);

        public abstract void OnJoinAccepted(JoinAcceptedMessage msg);

        public abstract void OnRejoinMessage(RejoinMessage msg, string sessionID);

        public abstract void OnHouseRequestMessage(HouseRequestMessage msg, string sessionID);

        /// <summary>
        /// executed, when a player wants to move his character along a path while it's his turn
        /// </summary>
        /// <param name="msg">contains informations about the player, the character he wants to move and the path he wants to move his character along</param>
        /// <exception cref="NotImplementedException"></exception>
        public abstract void OnMovementRequestMessage(MovementRequestMessage msg);

        /// <summary>
        /// executed, when a player want to do a action with his character while it's his turn
        /// </summary>
        /// <param name="msg">contains informations about the player, the character he wants to do a action with and the action he wants his character to do</param>
        public abstract void OnActionRequestMessage(ActionRequestMessage msg);

        public abstract void OnTransferRequestMessage(TransferRequestMessage msg);

        public abstract void OnEndTurnRequestMessage(EndTurnRequestMessage msg);

        public abstract void OnGameStateRequestMessage(GameStateRequestMessage msg);

     //   public abstract void OnPauseGameRequestMessage(PauseGameRequestMessage msg);

        public abstract void DoSendJoin(string clientName);

        public abstract void OnJoinAcceptedMessage(JoinAcceptedMessage joinAcceptedMessage);

        public abstract void OnGameConfigMessage(GameConfigMessage gameConfigMessage);

        public abstract void OnMapChangeDemandMessage(MapChangeDemandMessage mapChangeDemandMessage);

        public abstract void OnStrikeMessage(StrikeMessage strikeMessage);

        public abstract void OnGameEndMessage(GameEndMessage gameEndMessage);

        public abstract void OnGameStateMessage(GameStateMessage gameStateMessage);

        public abstract void OnPauseGameDemandMessage(GamePauseDemandMessage gamePauseDemandMessage);

        public abstract void OnPauseGameRequestMessage(PauseGameRequestMessage msg, string sessionID);

        public abstract void OnHouseOfferMessage(HouseOfferMessage houseOfferMessage);

        public abstract void OnHouseAcknowledgementMessage(HouseAcknowledgementMessage houseAcknowledgementMessage);

        public abstract void OnTurnDemandMessage(TurnDemandMessage turnDemandMessage);

        public abstract void OnMovementDemandMessage(MovementDemandMessage movementDemandMessage);

        public abstract void OnActionDemandMessage(ActionDemandMessage actionDemandMessage);

        public abstract void OnChangeCharacterStatisticsDemandMessage(ChangeCharacterStatisticsDemandMessage changeCharacterStatisticsDemandMessage);

        public abstract void OnSpawnCharacterDemandMessage(SpawnCharacterDemandMessage spawnCharacterDemandMessage);

        public abstract void OnChangePlayerSpiceDemandMessage(ChangePlayerSpiceDemandMessage changePlayerSpiceDemandMessage);

        public abstract void OnSandwormSpawnDemandMessage(SandwormSpawnDemandMessage sandwormSpawnDemandMessage);

        public abstract void OnSandwormMoveDemandMessage(SandwormMoveDemandMessage sandwormMoveMessage);

        public abstract void OnSandwormDespawnMessage(SandwormDespawnDemandMessage sandwormDespawnDemandMessage);

        public abstract void OnEndGameMessage(EndGameMessage endGameMessage);

        public abstract void OnTransferDemandMessage(TransferDemandMessage transferDemandMessage);

        public abstract void OnAtomicsUpdateDemandMessage(AtomicsUpdateDemandMessage atomicUpdateDemandMessage);


        public abstract void DoAcceptJoin(string clientSecret, int clientID, string sessionID);

        /// <summary>
        /// sends the an error message to the client
        /// </summary>
        /// <param name="errorCode">the error code (see "Standardisierungsdokument")</param>
        /// <param name="errorDescription">a further description of the error</param>
        /// <param name="sessionID">the session id of the client, the message need to be send to</param>
        public abstract void DoSendError(int errorCode, string errorDescription, string sessionID);

        public abstract void DoSendGameConfig();

        public abstract void DoSendHouseOffer(int clientID, GreatHouseType[] houses);

        public abstract void DoSendHouseAck(int clientID, string houseName);

        public abstract void DoSendTurnDemand(int clientID, int characterID);

        public abstract void DoSendMovementDemand(int clientID, int characterID, List<Position> path);

        public abstract void DoSendActionDemand(int clientID, int characterID, ActionType action, Position target);

        public abstract void DoSendTransferDemand(int clientID, int characterID, int targetID);

        public abstract void DoSendChangeCharacterStatsDemand(int clientID, int characterID, CharacterStatistics stats);

        public abstract void DoSendMapChangeDemand(MapChangeReasons mapChangeReasons);

        public abstract void DoSendAtomicsUpdateDemand(int clientID, bool shunned, int atomicsLeft);

        public abstract void DoSpawnCharacterDemand(Character attributes);

        public abstract void DoChangePlayerSpiceDemand(int clientID, int newSpiceVal);

        public abstract void DoSpawnSandwormDemand(int characterID, MapField mapField);

        public abstract void DoMoveSandwormDemand(List<MapField> list);

        public abstract void DoDespawnSandwormDemand();

        /// <summary>
        /// This method will be called, when the overlengthmechanism is aktive.
        /// </summary>
        public abstract void DoEndGame();

        /// <summary>
        /// This message will be sent to the clients when the game ends.
        /// </summary>
        /// <param name="winnerID">ID of the winner of the party</param>
        /// <param name="loserID">ID of the loser of the party</param>
        /// <param name="stats">Repräsentation of the statistics of the Game</param>
        public abstract void DoGameEndMessage(int winnerID, int loserID, Statistics stats);

        public abstract void DoSendGameState(int clientID, int[] activlyPlayingIDs, String[] history);

        /// <summary>
        /// increases the amount of strikes of a client and sends a strike message
        /// </summary>
        /// <param name="player">the player, who gets a strike</param>
        /// <param name="wrongMessage">the wrong message, who was send by the client</param>
        public abstract void DoSendStrike(int clientID, Message wrongMessage);

        public abstract void DoGamePauseDemand(int requestedByClientID, bool pause);

        public abstract void OnUnpauseGameOffer(int requestdByClient);
    }
}
