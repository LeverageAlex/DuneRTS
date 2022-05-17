namespace GameData.network.messages
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
        CHARACTER_STAT_CHANGE_DEMAND,
        MAP_CHANGE,
        SPAWN_CHARACTER,
        CHANGE_PLAYER_SPICE_DEMAND,
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
}
