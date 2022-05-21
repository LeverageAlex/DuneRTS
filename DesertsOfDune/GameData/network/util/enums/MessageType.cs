﻿namespace GameData.network.messages
{
    /// <summary>
    /// This enum describes all the Messagetypes that exist.
    /// </summary>
    public enum MessageType
    {
        DEBUG,
        CREATE,
        JOIN,
        JOINACCEPTED,
        ACK,
        ERROR,
        GAMECFG,
        HOUSE_OFFER,
        HOUSE_REQUEST,
        HOUSE_ACKNOWLEDGEMENT,
        TURN_DEMAND,
        MOVEMENT_REQUEST,
        ACTION_REQUEST,
        TRANSFER_REQUEST,
        MOVEMENT_DEMAND,
        ACTION_DEMAND,
        TRANSFER_DEMAND,
        CHARACTER_STAT_CHANGE_DEMAND,
        END_TURN_REQUEST,
        MAP_CHANGE_DEMAND,
        SPAWN_CHARACTER_DEMAND,
        CHANGE_PLAYER_SPICE_DEMAND,
        SANDWORM_SPAWN_DEMAND,
        SANDWORM_MOVE_DEMAND,
        SANDWORM_DESPAWN_DEMAND,
        ENDGAME,
        GAME_END,
        GAMESTATE_REQUEST,
        GAMESTATE,
        STRIKE,
        PAUSE_REQUEST,
        GAME_PAUSE_DEMAND,
        UNPAUSE_GAME_OFFER
    }
}
