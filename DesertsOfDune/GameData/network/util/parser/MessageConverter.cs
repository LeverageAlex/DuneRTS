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
            switch (message.GetMessageType())
            {
                case MessageType.DEBUG:
                    DebugMessage debugMessage = (DebugMessage)message;
                    return JsonConvert.SerializeObject(debugMessage);
                case MessageType.JOIN:
                    JoinMessage joinMessage = (JoinMessage)message;
                    return JsonConvert.SerializeObject(joinMessage);
                case MessageType.REJOIN:
                    RejoinMessage rejoinMessage = (RejoinMessage)message;
                    return JsonConvert.SerializeObject(rejoinMessage);
                case MessageType.JOINACCEPTED:
                    JoinAcceptedMessage joinAcceptedMessage = (JoinAcceptedMessage)message;
                    return JsonConvert.SerializeObject(joinAcceptedMessage);
                case MessageType.ERROR:
                    ErrorMessage errorMessage = (ErrorMessage)message;
                    return JsonConvert.SerializeObject(errorMessage);
                case MessageType.GAMECFG:
                    GameConfigMessage gameConfigMessage = (GameConfigMessage)message;
                    return JsonConvert.SerializeObject(gameConfigMessage);
                case MessageType.HOUSE_OFFER:
                    HouseOfferMessage houseOfferMessage = (HouseOfferMessage)message;
                    return JsonConvert.SerializeObject(houseOfferMessage);
                case MessageType.HOUSE_REQUEST:
                    HouseRequestMessage houseRequestMessage = (HouseRequestMessage)message;
                    return JsonConvert.SerializeObject(houseRequestMessage);
                case MessageType.HOUSE_ACKNOWLEDGEMENT:
                    HouseAcknowledgementMessage houseAcknowledgementMessage = (HouseAcknowledgementMessage)message;
                    return JsonConvert.SerializeObject(houseAcknowledgementMessage);
                case MessageType.TURN_DEMAND:
                    TurnDemandMessage turnDemandMessage = (TurnDemandMessage)message;
                    return JsonConvert.SerializeObject(turnDemandMessage);
                case MessageType.MOVEMENT_REQUEST:
                    MovementRequestMessage movementRequestMessage = (MovementRequestMessage)message;
                    return JsonConvert.SerializeObject(movementRequestMessage);
                case MessageType.ACTION_REQUEST:
                    ActionRequestMessage actionRequestMessage = (ActionRequestMessage)message;
                    return JsonConvert.SerializeObject(actionRequestMessage);
                case MessageType.TRANSFER_REQUEST:
                    TransferRequestMessage transferReuqestMessage = (TransferRequestMessage)message;
                    return JsonConvert.SerializeObject(transferReuqestMessage);
                case MessageType.MOVEMENT_DEMAND:
                    MovementDemandMessage movementMessage = (MovementDemandMessage)message;
                    return JsonConvert.SerializeObject(movementMessage);
                case MessageType.ACTION_DEMAND:
                    ActionDemandMessage actionDemandMessage = (ActionDemandMessage)message;
                    return JsonConvert.SerializeObject(actionDemandMessage);
                case MessageType.TRANSFER_DEMAND:
                    TransferDemandMessage transferDemandMessage = (TransferDemandMessage)message;
                    return JsonConvert.SerializeObject(transferDemandMessage);
                case MessageType.CHARACTER_STAT_CHANGE_DEMAND:
                    ChangeCharacterStatisticsDemandMessage characterStatisticsMessage = (ChangeCharacterStatisticsDemandMessage)message;
                    return JsonConvert.SerializeObject(characterStatisticsMessage);
                case MessageType.END_TURN_REQUEST:
                    EndTurnRequestMessage endTurnRequestMessage = (EndTurnRequestMessage)message;
                    return JsonConvert.SerializeObject(endTurnRequestMessage);
                case MessageType.MAP_CHANGE_DEMAND:
                    MapChangeDemandMessage mapChangeMessage = (MapChangeDemandMessage)message;
                    return JsonConvert.SerializeObject(mapChangeMessage, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Include });
                case MessageType.ATOMICS_UPDATE_DEMAND:
                    AtomicsUpdateDemandMessage atomicsUpdateDemandMessage = (AtomicsUpdateDemandMessage)message;
                    return JsonConvert.SerializeObject(atomicsUpdateDemandMessage);
                case MessageType.SPAWN_CHARACTER_DEMAND:
                    SpawnCharacterDemandMessage spawnCharacterMessage = (SpawnCharacterDemandMessage)message;
                    return JsonConvert.SerializeObject(spawnCharacterMessage);
                case MessageType.CHANGE_PLAYER_SPICE_DEMAND:
                    ChangePlayerSpiceDemandMessage changePlayerSpiceMessage = (ChangePlayerSpiceDemandMessage)message;
                    return JsonConvert.SerializeObject(changePlayerSpiceMessage);
                case MessageType.SANDWORM_SPAWN_DEMAND:
                    SandwormSpawnDemandMessage sandwormSpawnMessage = (SandwormSpawnDemandMessage)message;
                    return JsonConvert.SerializeObject(sandwormSpawnMessage);
                case MessageType.SANDWORM_MOVE_DEMAND:
                    SandwormMoveDemandMessage sandwormMoveDemandMessage = (SandwormMoveDemandMessage)message;
                    return JsonConvert.SerializeObject(sandwormMoveDemandMessage);
                case MessageType.SANDWORM_DESPAWN_DEMAND:
                    SandwormDespawnDemandMessage sandwormDespawnMessage = (SandwormDespawnDemandMessage)message;
                    return JsonConvert.SerializeObject(sandwormDespawnMessage);
                case MessageType.ENDGAME:
                    EndGameMessage endGameMessage = (EndGameMessage)message;
                    return JsonConvert.SerializeObject(endGameMessage);
                case MessageType.GAME_END:
                    GameEndMessage gameEndMessage = (GameEndMessage)message;
                    return JsonConvert.SerializeObject(gameEndMessage);
                case MessageType.GAMESTATE_REQUEST:
                    GameStateRequestMessage gameStateRequestMessage = (GameStateRequestMessage)message;
                    return JsonConvert.SerializeObject(gameStateRequestMessage);
                case MessageType.GAMESTATE:
                    GameStateMessage gameStateMessage = (GameStateMessage)message;
                    return JsonConvert.SerializeObject(gameStateMessage);
                case MessageType.STRIKE:
                    StrikeMessage strikeMessage = (StrikeMessage)message;
                    return JsonConvert.SerializeObject(strikeMessage);
                case MessageType.PAUSE_REQUEST:
                    PauseGameRequestMessage pauseGameRequestMessage = (PauseGameRequestMessage)message;
                    return JsonConvert.SerializeObject(pauseGameRequestMessage);
                case MessageType.GAME_PAUSE_DEMAND:
                    PausGameDemandMessage pauseGameMessage = (PausGameDemandMessage)message;
                    return JsonConvert.SerializeObject(pauseGameMessage);
                case MessageType.UNPAUSE_GAME_OFFER:
                    UnpauseGameOfferMessage unpauseGameOfferMessage = (UnpauseGameOfferMessage)message;
                    return JsonConvert.SerializeObject(unpauseGameOfferMessage);
                default:
                    throw new ArgumentException($"The given message type: {message.GetMessageType()} is not implemented");
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

            MessageType msgType = (MessageType)Enum.Parse(typeof(MessageType), messageType);

            Console.WriteLine("the match: " + messageType);
            switch (msgType)
            {
                case MessageType.DEBUG:
                    return JsonConvert.DeserializeObject<DebugMessage>(message);
                case MessageType.JOIN:
                    return JsonConvert.DeserializeObject<JoinMessage>(message);
                case MessageType.REJOIN:
                    return JsonConvert.DeserializeObject<RejoinMessage>(message);
                case MessageType.JOINACCEPTED:
                    return JsonConvert.DeserializeObject<JoinAcceptedMessage>(message);
                case MessageType.ERROR:
                    return JsonConvert.DeserializeObject<ErrorMessage>(message);
                case MessageType.GAMECFG:
                    return JsonConvert.DeserializeObject<GameConfigMessage>(message);
                case MessageType.HOUSE_OFFER:
                   // return  //TODO: finish deserialization of HouseOfferMessage
                    return JsonConvert.DeserializeObject<HouseOfferMessage>(message);
                case MessageType.HOUSE_REQUEST:
                    return JsonConvert.DeserializeObject<HouseRequestMessage>(message);
                case MessageType.HOUSE_ACKNOWLEDGEMENT:
                    return JsonConvert.DeserializeObject<HouseAcknowledgementMessage>(message);
                case MessageType.TURN_DEMAND:
                    return JsonConvert.DeserializeObject<TurnDemandMessage>(message);
                case MessageType.MOVEMENT_REQUEST:
                    return JsonConvert.DeserializeObject<MovementRequestMessage>(message);
                case MessageType.ACTION_REQUEST:
                    return JsonConvert.DeserializeObject<ActionRequestMessage>(message);
                case MessageType.TRANSFER_REQUEST:
                    return JsonConvert.DeserializeObject<TransferRequestMessage>(message);
                case MessageType.MOVEMENT_DEMAND:
                    return JsonConvert.DeserializeObject<MovementDemandMessage>(message);
                case MessageType.ACTION_DEMAND:
                    return JsonConvert.DeserializeObject<ActionDemandMessage>(message);
                case MessageType.TRANSFER_DEMAND:
                    return JsonConvert.DeserializeObject<TransferDemandMessage>(message);
                case MessageType.CHARACTER_STAT_CHANGE_DEMAND:
                    return JsonConvert.DeserializeObject<ChangeCharacterStatisticsDemandMessage>(message);
                case MessageType.END_TURN_REQUEST:
                    return JsonConvert.DeserializeObject<EndTurnRequestMessage>(message);
                case MessageType.MAP_CHANGE_DEMAND:
                    return JsonConvert.DeserializeObject<MapChangeDemandMessage>(message);
                case MessageType.ATOMICS_UPDATE_DEMAND:
                    return JsonConvert.DeserializeObject<AtomicsUpdateDemandMessage>(message);
                case MessageType.SPAWN_CHARACTER_DEMAND:
                    return JsonConvert.DeserializeObject<SpawnCharacterDemandMessage>(message);
                case MessageType.CHANGE_PLAYER_SPICE_DEMAND:
                    return JsonConvert.DeserializeObject<ChangePlayerSpiceDemandMessage>(message);
                case MessageType.SANDWORM_SPAWN_DEMAND:
                    return JsonConvert.DeserializeObject<SandwormSpawnDemandMessage>(message);
                case MessageType.SANDWORM_MOVE_DEMAND:
                    return JsonConvert.DeserializeObject<SandwormMoveDemandMessage>(message);
                case MessageType.SANDWORM_DESPAWN_DEMAND:
                    return JsonConvert.DeserializeObject<SandwormDespawnDemandMessage>(message);
                case MessageType.ENDGAME:
                    return JsonConvert.DeserializeObject<EndGameMessage>(message);
                case MessageType.GAME_END:
                    return JsonConvert.DeserializeObject<GameEndMessage>(message);
                case MessageType.GAMESTATE_REQUEST:
                    return JsonConvert.DeserializeObject<GameStateRequestMessage>(message);
                case MessageType.GAMESTATE:
                    return JsonConvert.DeserializeObject<GameStateMessage>(message);
                case MessageType.STRIKE:
                    return JsonConvert.DeserializeObject<StrikeMessage>(message);
                case MessageType.PAUSE_REQUEST:
                    return JsonConvert.DeserializeObject<PauseGameRequestMessage>(message);
                case MessageType.GAME_PAUSE_DEMAND:
                    return JsonConvert.DeserializeObject<GamePauseDemandMessage>(message);
                case MessageType.UNPAUSE_GAME_OFFER:
                    return JsonConvert.DeserializeObject<UnpauseGameOfferMessage>(message);
                default:
                    throw new ArgumentException($"The given message type: {messageType} is not implemented");
            }
        }
    }
}
