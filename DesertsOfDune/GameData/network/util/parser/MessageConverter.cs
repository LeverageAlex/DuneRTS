using System;
using System.Text.RegularExpressions;
using GameData.network.messages;
using Newtonsoft.Json;
using Serilog;

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
        static public string FromMessage(Message message)
        {
            switch (message.getMessageType())
            {
                case "ACTION_DEMAND":
                    ActionDemandMessage actionDemandMessage = (ActionDemandMessage)message;
                    return JsonConvert.SerializeObject(actionDemandMessage);
                case "ACTION_REQUEST":
                    ActionRequestMessage actionRequestMessage = (ActionRequestMessage)message;
                    return JsonConvert.SerializeObject(actionRequestMessage);
                case "CHANGE_PLAYER_SPICE_DEMAND":
                    ChangePlayerSpiceDemandMessage changePlayerSpiceMessage = (ChangePlayerSpiceDemandMessage)message;
                    return JsonConvert.SerializeObject(changePlayerSpiceMessage);
                case "CHARACTER_STAT_CHANGE_DEMAND":
                    ChangeCharacterStatisticsDemandMessage characterStatisticsMessage = (ChangeCharacterStatisticsDemandMessage)message;
                    return JsonConvert.SerializeObject(characterStatisticsMessage);
                case "CREATE":
                    CreateMessage createMessage = (CreateMessage)message;
                    return JsonConvert.SerializeObject(createMessage);
                case "DEBUG":
                    DebugMessage debugMessage = (DebugMessage)message;
                    return JsonConvert.SerializeObject(debugMessage);
                case "ENDGAME":
                    EndGameMessage endGameMessage = (EndGameMessage)message;
                    return JsonConvert.SerializeObject(endGameMessage);
                case "END_TURN_REQUEST":
                    EndTurnRequestMessage endTurnRequestMessage = (EndTurnRequestMessage)message;
                    return JsonConvert.SerializeObject(endTurnRequestMessage);
                case "GAMECFG":
                    GameConfigMessage gameConfigMessage = (GameConfigMessage)message;
                    return JsonConvert.SerializeObject(gameConfigMessage);
                case "GAME_END":
                    GameEndMessage gameEndMessage = (GameEndMessage)message;
                    return JsonConvert.SerializeObject(gameEndMessage);
                case "GAMESTATE":
                    GameStateMessage gameStateMessage = (GameStateMessage)message;
                    return JsonConvert.SerializeObject(gameStateMessage);
                case "REQUEST_GAMESTATE":
                    GameStateRequestMessage gameStateRequestMessage = (GameStateRequestMessage)message;
                    return JsonConvert.SerializeObject(gameStateRequestMessage);
                case "HOUSE_ACKNOWLEGDEMENT":
                    HouseAcknowledgementMessage houseAcknowledgementMessage = (HouseAcknowledgementMessage)message;
                    return JsonConvert.SerializeObject(houseAcknowledgementMessage);
                case "HOUSE_OFFER":
                    HouseOfferMessage houseOfferMessage = (HouseOfferMessage)message;
                    return JsonConvert.SerializeObject(houseOfferMessage);
                case "HOUSE_REQUEST":
                    HouseRequestMessage houseRequestMessage = (HouseRequestMessage)message;
                    return JsonConvert.SerializeObject(houseRequestMessage);
                case "JOINACCEPTED":
                    JoinAcceptedMessage joinAcceptedMessage = (JoinAcceptedMessage)message;
                    return JsonConvert.SerializeObject(joinAcceptedMessage);
                case "JOIN":
                    JoinMessage joinMessage = (JoinMessage)message;
                    return JsonConvert.SerializeObject(joinMessage);
                case "MAP_CHANGE_DEMAND":
                    MapChangeDemandMessage mapChangeMessage = (MapChangeDemandMessage)message;
                    return JsonConvert.SerializeObject(mapChangeMessage);
                case "MOVEMENT_DEMAND":
                    MovementDemandMessage movementMessage = (MovementDemandMessage)message;
                    return JsonConvert.SerializeObject(movementMessage);
                case "MOVEMENT_REQUEST":
                    MovementRequestMessage movementRequestMessage = (MovementRequestMessage)message;
                    return JsonConvert.SerializeObject(movementRequestMessage);
                case "PAUSE_GAME":
                    PauseGameMessage pauseGameMessage = (PauseGameMessage)message;
                    return JsonConvert.SerializeObject(pauseGameMessage);
                case "PAUSE_REQUEST":
                    PauseGameRequestMessage pauseGameRequestMessage = (PauseGameRequestMessage)message;
                    return JsonConvert.SerializeObject(pauseGameRequestMessage);
                case "SANDWORM_DESPAWN_DEMAND":
                    SandwormDespawnDemandMessage sandwormDespawnMessage = (SandwormDespawnDemandMessage)message;
                    return JsonConvert.SerializeObject(sandwormDespawnMessage);
                case "SANDWORM_MOVE_DEMAND":
                    SandwormMoveDemandMessage sandwormMoveDemandMessage = (SandwormMoveDemandMessage)message;
                    return JsonConvert.SerializeObject(sandwormMoveDemandMessage);
                case "SANDWORM_SPAWN_DEMAND":
                    SandwormSpawnDemandMessage sandwormSpawnMessage = (SandwormSpawnDemandMessage)message;
                    return JsonConvert.SerializeObject(sandwormSpawnMessage);
                case "SPAWN_CHARACTER_DEMAND":
                    SpawnCharacterDemandMessage spawnCharacterMessage = (SpawnCharacterDemandMessage)message;
                    return JsonConvert.SerializeObject(spawnCharacterMessage);
                case "STRIKE":
                    StrikeMessage strikeMessage = (StrikeMessage)message;
                    return JsonConvert.SerializeObject(strikeMessage);
                case "TURN_REQUEST":
                    TurnRequestMessage turnRequestMessage = (TurnRequestMessage)message;
                    return JsonConvert.SerializeObject(turnRequestMessage);
                case "TURN_DEMAND":
                    TurnDemandMessage turnDemandMessage = (TurnDemandMessage)message;
                    return JsonConvert.SerializeObject(turnDemandMessage);
                default:
                    return null;
            }
        }

        /// <summary>
        /// converts a JSON-String to a Message-Object
        /// </summary>
        /// <param name="message">the message as a JSON-String, which should be converted to the fitting Message-Object</param>
        /// <returns>the Message object, which is "equivalent to the JSON-String or null if JSON-String could not be reassambled to a Message-Object
        /// (possible reasons: invalid JSON syntax, not a message string, not expected data in JSON-String)</returns>
        /// TODO: change the default behaviour and do not return null, but throw a "ParsingJSONStringToMessageObjectNotPossible"-Exception
        static public Message ToMessage(string message)
        {
            // pattern for searching and getting the type of the message (needed for finding the fitting Message-Object for deserialization)
            string pattern = "{\"type\":\"([A-Z]*_*[A-Z]*_*[A-Z]*_*[A-Z]*)";
            Regex regex = new Regex(pattern);
            MatchCollection matchedContent = regex.Matches(message);

            // ignore the "type:" and so on and only extract to type
            string messageType = matchedContent[0].Value.Substring(9);

            Log.Debug("The message <" + message + "> is of type: " + messageType);

            // deserialize JSON-String depending on the message type found in the message
            switch (messageType)
            {
                case "ACTION_DEMAND":
                    return JsonConvert.DeserializeObject<ActionDemandMessage>(message);
                case "ACTION_REQUEST":
                    return JsonConvert.DeserializeObject<ActionRequestMessage>(message);
                case "CHARACTER_STAT_CHANGE_DEMAND":
                      return JsonConvert.DeserializeObject<ChangeCharacterStatisticsDemandMessage>(message);
                case "CHANGE_PLAYER_SPICE_DEMAND":
                      return JsonConvert.DeserializeObject<ChangePlayerSpiceDemandMessage>(message);
                case "CREATE":
                    return JsonConvert.DeserializeObject<CreateMessage>(message);
                case "DEBUG":
                    return JsonConvert.DeserializeObject<DebugMessage>(message);
                case "ENDGAME":
                    return JsonConvert.DeserializeObject<EndGameMessage>(message);
                case "END_TURN_REQUEST":
                    return JsonConvert.DeserializeObject<EndTurnRequestMessage>(message);
                case "GAMECFG":
                    return JsonConvert.DeserializeObject<GameConfigMessage>(message);
                case "GAME_END":
                    return JsonConvert.DeserializeObject<GameEndMessage>(message);
                case "GAMESTATE":
                    return JsonConvert.DeserializeObject<GameStateMessage>(message);
                case "REQUEST_GAMESTATE":
                    return JsonConvert.DeserializeObject<GameStateRequestMessage>(message);
                case "HOUSE_ACKNOWLEGDEMENT":
                    return JsonConvert.DeserializeObject<HouseAcknowledgementMessage>(message);
                case "HOUSE_OFFER":
                    return JsonConvert.DeserializeObject<HouseOfferMessage>(message);
                case "HOUSE_REQUEST":
                    return JsonConvert.DeserializeObject<HouseRequestMessage>(message);
                case "JOINACCEPTED":
                    return JsonConvert.DeserializeObject<JoinAcceptedMessage>(message);
                case "JOIN":
                    return JsonConvert.DeserializeObject<JoinMessage>(message);
                case "MAP_CHANGE_DEMAND":
                    return JsonConvert.DeserializeObject<MapChangeDemandMessage>(message);
                case "MOVEMENT_DEMAND":
                    return JsonConvert.DeserializeObject<MovementDemandMessage>(message);
                case "MOVEMENT_REQUEST":
                    return JsonConvert.DeserializeObject<MovementRequestMessage>(message);
                case "PAUSE_GAME":
                    return JsonConvert.DeserializeObject<PauseGameMessage>(message);
                case "PAUSE_REQUEST":
                    return JsonConvert.DeserializeObject<PauseGameRequestMessage>(message);
                case "SANDWORM_DESPAWN_DEMAND":
                    return JsonConvert.DeserializeObject<SandwormDespawnDemandMessage>(message);
                case "SANDWORM_MOVE_DEMAND":
                    return JsonConvert.DeserializeObject<SandwormMoveDemandMessage>(message);
                case "SANDWORM_SPAWN_DEMAND":
                    return JsonConvert.DeserializeObject<SandwormSpawnDemandMessage>(message);
                case "SPAWN_CHARACTER_DEMAND":
                    return JsonConvert.DeserializeObject<SpawnCharacterDemandMessage>(message);
                case "STRIKE":
                    return JsonConvert.DeserializeObject<StrikeMessage>(message);
                case "TURN_DEMAND":
                    return JsonConvert.DeserializeObject<TurnDemandMessage>(message);
                case "TURN_REQUEST":
                    return JsonConvert.DeserializeObject<TurnRequestMessage>(message);
                default:
                    return null;
            }
        }
    }
}
