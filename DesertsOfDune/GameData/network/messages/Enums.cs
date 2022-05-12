using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.messages
{
    public class Enums
    {
        public enum MessageType
        {
            DEBUG,
            CREATE,
            JOIN,
            JOINACCEPTED,
            GAMECFG,
            HOUSE_OFFER,
            HOUSE_REQUEST,
            HOUSE_ACKNOWLEGDEMENT,
            TURN_REQUEST,
            MOVEMENT_REQUEST,
            ACTION_REQUEST,
            MOVEMENT,
            ACTION,
            CHARACTER_STAT_CHANGE,
            MAP_CHANGE,
            SPAWN_CHARACTER,
            CHANGE_PLAYER_SPICE,
            SANDWORM_SPAWN,
            SANDWORM_MOVE,
            SANDWORM_DESPAWN,
            ENDGAME,
            REQUEST_GAMESTATE,
            GAMESTATE,
            STRIKE,
            PAUSE_REQUEST,
            PAUSE_GAME
        }
    }
}
