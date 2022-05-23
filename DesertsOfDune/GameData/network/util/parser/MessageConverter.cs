using System;
using System.Text.RegularExpressions;
using GameData.network.messages;
using Newtonsoft.Json;

namespace GameData.network.util.parser
{
    /// <summary>
    /// Converter of Message-Objects to Json and vice versa.
    /// </summary>
    public static class MessageConverter
    {
        /// <summary>
        /// converts a Message-Object to a JSON-String, depending on the type of the message
        /// </summary>
        /// <param name="message">the Message-Object</param>
        /// <returns>the parsed JSON-String of the Object or "null" if the parsing was not possible (possible reasons: message has invalid type)</returns>
        /// TODO: change the default behaviour and do not return null, but throw a "ParsingMessageToJSONStringNotPossible"-Exception
        static public String FromMessage(Message message)
        {
            switch (message.getMessageType())
            {
                case "DEBUG":
                    DebugMessage debugMessage = (DebugMessage)message;
                    return JsonConvert.SerializeObject(debugMessage);
                case "CREATE":
                    CreateMessage createMessage = (CreateMessage)message;
                    return JsonConvert.SerializeObject(createMessage);
                case "JOIN":
                    JoinMessage joinMessage = (JoinMessage)message;
                    return JsonConvert.SerializeObject(joinMessage);
                case "JOINACCEPTED":
                    JoinAcceptedMessage joinAcceptedMessage = (JoinAcceptedMessage)message;
                    return JsonConvert.SerializeObject(joinAcceptedMessage);
                case "ACK":
                    AckMessage ackMessage = (AckMessage)message;
                    return JsonConvert.SerializeObject(ackMessage);
                case "ERROR":
                    ErrorMessage errorMessage = (ErrorMessage)message;
                    return JsonConvert.SerializeObject(errorMessage);
                case "GAMECFG":
                    GameConfigMessage gameConfigMessage = (GameConfigMessage)message;
                    return JsonConvert.SerializeObject(gameConfigMessage);
                case "HOUSE_OFFER":
                    HouseOfferMessage houseOfferMessage = (HouseOfferMessage)message;
                    return JsonConvert.SerializeObject(houseOfferMessage);
                case "HOUSE_REQUEST":
                    HouseRequestMessage houseRequestMessage = (HouseRequestMessage)message;
                    return JsonConvert.SerializeObject(houseRequestMessage);
                case "HOUSE_ACKNOWLEDGEMENT":
                    HouseAcknowledgementMessage houseAcknowledgementMessage = (HouseAcknowledgementMessage)message;
                    return JsonConvert.SerializeObject(houseAcknowledgementMessage);
                case "TURN_DEMAND":
                    TurnDemandMessage turnDemandMessage = (TurnDemandMessage)message;
                    return JsonConvert.SerializeObject(turnDemandMessage);
                case "MOVEMENT_REQUEST":
                    MovementRequestMessage movementRequestMessage = (MovementRequestMessage)message;
                    return JsonConvert.SerializeObject(movementRequestMessage);
                case "ACTION_REQUEST":
                    ActionRequestMessage actionRequestMessage = (ActionRequestMessage)message;
                    return JsonConvert.SerializeObject(actionRequestMessage);
                case "TRANSFER_REQUEST":
                    TransferRequestMessage transferReuqestMessage = (TransferRequestMessage)message;
                    return JsonConvert.SerializeObject(transferReuqestMessage);
                case "MOVEMENT_DEMAND":
                    MovementDemandMessage movementMessage = (MovementDemandMessage)message;
                    return JsonConvert.SerializeObject(movementMessage);
                case "ACTION_DEMAND":
                    ActionDemandMessage actionDemandMessage = (ActionDemandMessage)message;
                    return JsonConvert.SerializeObject(actionDemandMessage);
                case "TRANSFER_DEMAND":
                    TransferDemandMessage transferDemandMessage = (TransferDemandMessage)message;
                    return JsonConvert.SerializeObject(transferDemandMessage);
                case "CHARACTER_STAT_CHANGE_DEMAND":
                    ChangeCharacterStatisticsDemandMessage characterStatisticsMessage = (ChangeCharacterStatisticsDemandMessage)message;
                    return JsonConvert.SerializeObject(characterStatisticsMessage);
                case "END_TURN_REQUEST":
                    EndTurnRequestMessage endTurnRequestMessage = (EndTurnRequestMessage)message;
                    return JsonConvert.SerializeObject(endTurnRequestMessage);
                case "MAP_CHANGE_DEMAND":
                    MapChangeDemandMessage mapChangeMessage = (MapChangeDemandMessage)message;
                    return JsonConvert.SerializeObject(mapChangeMessage);
                case "SPAWN_CHARACTER_DEMAND":
                    SpawnCharacterDemandMessage spawnCharacterMessage = (SpawnCharacterDemandMessage)message;
                    return JsonConvert.SerializeObject(spawnCharacterMessage);
                case "CHANGE_PLAYER_SPICE_DEMAND":
                    ChangePlayerSpiceDemandMessage changePlayerSpiceMessage = (ChangePlayerSpiceDemandMessage)message;
                    return JsonConvert.SerializeObject(changePlayerSpiceMessage);
                case "SANDWORM_SPAWN_DEMAND":
                    SandwormSpawnDemandMessage sandwormSpawnMessage = (SandwormSpawnDemandMessage)message;
                    return JsonConvert.SerializeObject(sandwormSpawnMessage);
                case "SANDWORM_MOVE_DEMAND":
                    SandwormMoveDemandMessage sandwormMoveDemandMessage = (SandwormMoveDemandMessage)message;
                    return JsonConvert.SerializeObject(sandwormMoveDemandMessage);
                case "SANDWORM_DESPAWN_DEMAND":
                    SandwormDespawnDemandMessage sandwormDespawnMessage = (SandwormDespawnDemandMessage)message;
                    return JsonConvert.SerializeObject(sandwormDespawnMessage);
                case "ENDGAME":
                    EndGameMessage endGameMessage = (EndGameMessage)message;
                    return JsonConvert.SerializeObject(endGameMessage);
                case "GAME_END":
                    GameEndMessage gameEndMessage = (GameEndMessage)message;
                    return JsonConvert.SerializeObject(gameEndMessage);
                case "GAMESTATE_REQUEST":
                    GameStateRequestMessage gameStateRequestMessage = (GameStateRequestMessage)message;
                    return JsonConvert.SerializeObject(gameStateRequestMessage);
                case "GAMESTATE":
                    GameStateMessage gameStateMessage = (GameStateMessage)message;
                    return JsonConvert.SerializeObject(gameStateMessage);
                case "STRIKE":
                    StrikeMessage strikeMessage = (StrikeMessage)message;
                    return JsonConvert.SerializeObject(strikeMessage);
                case "PAUSE_REQUEST":
                    PauseGameRequestMessage pauseGameRequestMessage = (PauseGameRequestMessage)message;
                    return JsonConvert.SerializeObject(pauseGameRequestMessage);
                case "GAME_PAUSE_DEMAND":
                    GamePauseDemandMessage pauseGameMessage = (GamePauseDemandMessage)message;
                    return JsonConvert.SerializeObject(pauseGameMessage);
                case "UNPAUSE_GAME_OFFER":
                    UnpauseGameOfferMessage unpauseGameOfferMessage = (UnpauseGameOfferMessage)message;
                    return JsonConvert.SerializeObject(unpauseGameOfferMessage);
                default:
                    throw new ArgumentException($"The given message type: {message.getMessageType()} is not implemented");
            }
        }

        /// <summary>
        /// converts a JSON-String to a Message-Object
        /// </summary>
        /// <param name="message">the message as a JSON-String, which should be converted to the fitting Message-Object</param>
        /// <returns>the Message object, which is "equivalent to the JSON-String or null if JSON-String could not be reassambled to a Message-Object
        /// (possible reasons: invalid JSON syntax, not a message string, not expected data in JSON-String)</returns>
        /// TODO: change the default behaviour and do not return null, but throw a "ParsingJSONStringToMessageObjectNotPossible"-Exception
        static public Message ToMessage(String message)
        {
            string pattern = "{\"type\":\"([A-Z]*_*[A-Z]*_*[A-Z]*_*[A-Z]*)";
            Regex rg = new Regex(pattern);
            MatchCollection matchedContent = rg.Matches(message);
            string messageType = matchedContent[0].Value.Substring(9);
            Console.WriteLine("the match: " + messageType);
            switch (messageType)
            {
                case "DEBUG":
                    return JsonConvert.DeserializeObject<DebugMessage>(message);
                case "CREATE":
                    return JsonConvert.DeserializeObject<CreateMessage>(message);
                case "JOIN":
                    return JsonConvert.DeserializeObject<JoinMessage>(message);
                case "JOINACCEPTED":
                    return JsonConvert.DeserializeObject<JoinAcceptedMessage>(message);
                case "ACK":
                    return JsonConvert.DeserializeObject<AckMessage>(message);
                case "ERROR":
                    return JsonConvert.DeserializeObject<ErrorMessage>(message);
                case "GAMECFG":
                    return JsonConvert.DeserializeObject<GameConfigMessage>(message);
                case "HOUSE_OFFER":
                    return JsonConvert.DeserializeObject<HouseOfferMessage>(message);
                case "HOUSE_REQUEST":
                    return JsonConvert.DeserializeObject<HouseRequestMessage>(message);
                case "HOUSE_ACKNOWLEDGEMENT":
                    return JsonConvert.DeserializeObject<HouseAcknowledgementMessage>(message);
                case "TURN_DEMAND":
                    return JsonConvert.DeserializeObject<TurnDemandMessage>(message);
                case "MOVEMENT_REQUEST":
                    return JsonConvert.DeserializeObject<MovementRequestMessage>(message);
                case "ACTION_REQUEST":
                    return JsonConvert.DeserializeObject<ActionRequestMessage>(message);
                case "TRANSFER_REQUEST":
                    return JsonConvert.DeserializeObject<TransferRequestMessage>(message);
                case "MOVEMENT_DEMAND":
                    return JsonConvert.DeserializeObject<MovementDemandMessage>(message);
                case "ACTION_DEMAND":
                    return JsonConvert.DeserializeObject<ActionDemandMessage>(message);
                case "TRANSFER_DEMAND":
                    return JsonConvert.DeserializeObject<TransferDemandMessage>(message);
                case "CHARACTER_STAT_CHANGE_DEMAND":
                    return JsonConvert.DeserializeObject<ChangeCharacterStatisticsDemandMessage>(message);
                case "END_TURN_REQUEST":
                    return JsonConvert.DeserializeObject<EndTurnRequestMessage>(message);
                case "MAP_CHANGE_DEMAND":
                    return JsonConvert.DeserializeObject<MapChangeDemandMessage>(message);
                case "SPAWN_CHARACTER_DEMAND":
                    return JsonConvert.DeserializeObject<SpawnCharacterDemandMessage>(message);
                case "CHANGE_PLAYER_SPICE_DEMAND":
                    return JsonConvert.DeserializeObject<ChangePlayerSpiceDemandMessage>(message);
                case "SANDWORM_SPAWN_DEMAND":
                    return JsonConvert.DeserializeObject<SandwormSpawnDemandMessage>(message);
                case "SANDWORM_MOVE_DEMAND":
                    return JsonConvert.DeserializeObject<SandwormMoveDemandMessage>(message);
                case "SANDWORM_DESPAWN_DEMAND":
                    return JsonConvert.DeserializeObject<SandwormDespawnDemandMessage>(message);
                case "ENDGAME":
                    return JsonConvert.DeserializeObject<EndGameMessage>(message);
                case "GAME_END":
                    return JsonConvert.DeserializeObject<GameEndMessage>(message);
                case "REQUEST_GAMESTATE":
                    return JsonConvert.DeserializeObject<GameStateRequestMessage>(message);
                case "GAMESTATE":
                    return JsonConvert.DeserializeObject<GameStateMessage>(message);
                case "STRIKE":
                    return JsonConvert.DeserializeObject<StrikeMessage>(message);
                case "PAUSE_REQUEST":
                    return JsonConvert.DeserializeObject<PauseGameRequestMessage>(message);
                case "GAME_PAUSE_DEMAND":
                    return JsonConvert.DeserializeObject<GamePauseDemandMessage>(message);
                case "UNPAUSE_GAME_OFFER":
                    return JsonConvert.DeserializeObject<UnpauseGameOfferMessage>(message);
                default:
                    throw new ArgumentException($"The given message type: {messageType} is not implemented");
            }
        }
    }
}
