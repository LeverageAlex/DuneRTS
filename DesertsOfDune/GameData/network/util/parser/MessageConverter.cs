using System;
using System.Text.RegularExpressions;
using GameData.network.messages;
using Newtonsoft.Json;

namespace GameData.network.util.parser
{
    /// <summary>
    /// This class is responsible for converting Message Objects to Json and vice versa.
    /// </summary>
    public static class MessageConverter
    {
        /// <summary>
        /// This method converts a Message Object to a Json string
        /// </summary>
        /// <param name="message">the message object</param>
        /// <returns>the parsed json string if this was not possible return null</returns>
        static public String FromMessage(Message message)
        {
                switch (message.getMessageType())
                {
                    case "TRANSFER_REQUEST":
                        TransferRequestMessage transferRequestMessage = (TransferRequestMessage)message;
                        return JsonConvert.SerializeObject(transferRequestMessage);
                    case "TRANSFER_DEMAND":
                        TransferDemandMessage transferDemandMessage = (TransferDemandMessage)message;
                        return JsonConvert.SerializeObject(transferDemandMessage);
                    case "GAME_PAUSE_DEMAND":
                        PausGameDemandMessage pausGameDemandMessage = (PausGameDemandMessage)message;
                        return JsonConvert.SerializeObject(pausGameDemandMessage);
                    case "ACK":
                        AckMessage ackMessage = (AckMessage)message;
                        return JsonConvert.SerializeObject(ackMessage);
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
                    case "GAMESTATE_REQUEST":
                        GameStateRequestMessage gameStateRequestMessage = (GameStateRequestMessage)message;
                        return JsonConvert.SerializeObject(gameStateRequestMessage);
                    case "HOUSE_ACKNOWLEDGEMENT":
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
                    case "TURN_DEMAND":
                        TurnDemandMessage turnDemandMessage = (TurnDemandMessage)message;
                        return JsonConvert.SerializeObject(turnDemandMessage);
                    default:
                        return null;
                }
        }

        /// <summary>
        /// This method converts a json String to a Message object
        /// </summary>
        /// <param name="message">the json string to be converted</param>
        /// <returns>the Message object to be created. If string does not resamble a message return null</returns>
        static public Message ToMessage(String message)
        {
            string pattern = "{\"type\":\"([A-Z]*_*[A-Z]*_*[A-Z]*_*[A-Z]*)";
            Regex rg = new Regex(pattern);
            MatchCollection matchedContent = rg.Matches(message);
            string messageType = matchedContent[0].Value.Substring(9);
            Console.WriteLine("the match: " + messageType);
            switch (messageType)
            {
                case "TRANSFER_REQUEST":
                    return JsonConvert.DeserializeObject<TransferRequestMessage>(message);
                case "TRANSFER_DEMAND":
                    return JsonConvert.DeserializeObject<TransferDemandMessage>(message);
                case "GAME_PAUSE_DEMAND":
                    return JsonConvert.DeserializeObject<PausGameDemandMessage>(message);
                case "ACK":
                    return JsonConvert.DeserializeObject<AckMessage>(message);
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
                case "GAMESTATE_REQUEST":
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
                default:
                    throw new ArgumentException($"The given message type: {messageType} is not implemented");
            }
        }
    }
}
