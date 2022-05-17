using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.messages
{
    /// <summary>
    /// This class holds all Enums in the folder util.
    /// </summary>
    public class Enums
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
            PAUSE_GAME,
            END_TURN_REQUEST,
            GAME_END
        }

        /// <summary>
        /// This enum describes the type of Client.
        /// </summary>
        public enum ClientType
        {
            PLAYER,
            AI,
            SPECTATOR
        }
        /// <summary>
        /// This enum describes the type of Parser that needs to be used.
        /// </summary>
        public enum ParserType
        {
            SERVER,
            AI
        }
        /// <summary>
        /// This enum describes the reasons for a map change.
        /// </summary>
        public enum MapChangeReasons
        {
            SANDSTORM,
            DUNEWALKING,
            ENDGAME
        }
        /// <summary>
        /// This enum describes the types of actions that can be executed by a character.
        /// </summary>
        public enum ActionType
        {
            ATTACK,
            COLLECT,
            TRANSFER,
            KANLY,
            FAMILY_ATOMICS,
            SPICE_HORDING,
            VOICE,
            SWORD_SPIN
        }

        /// <summary>
        /// This enum descibes the different types of characters.
        /// </summary>
        public enum CharacterType
        {
            NOBEL,
            MENTAT,
            BENEGESSERIT,
            FIGHTER
        }
    }
}
