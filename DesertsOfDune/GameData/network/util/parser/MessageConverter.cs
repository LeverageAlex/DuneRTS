using System;
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
        /// <returns>the parsed json string</returns>
        static public String FromMessage(Message message)
        {
            switch (message.getMessageType())
            {
                case "ACTION":
                    // TODO: ActionMessage does not work atm.
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
                    // TODO: ActionMessage does not work atm.
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
            }
            return "";
        }

        /// <summary>
        /// This method converts a json String to a Message object
        /// </summary>
        /// <param name="message">the json string to be converted</param>
        /// <returns>the Message object to be created.</returns>
        static public Message ToMessage(String message)
        {
            
            EndTurnRequestMessage m = JsonConvert.DeserializeObject<EndTurnRequestMessage>(message);
            Console.WriteLine(m.ToString());
            return m;
        }
    }
}
