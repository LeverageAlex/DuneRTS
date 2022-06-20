using System;
using GameData.network.messages;
using Serilog;

namespace AIClient.Moves
{
    /// <summary>
    /// Represents the "move" to move a character in a direction
    /// </summary>
    public class Action : Move
    {
        public ActionType actionType;

        public Action(MoveTypes type) : base(type)
        {
            switch(type){
                case MoveTypes.ATTACK:
                    actionType = ActionType.ATTACK;
                    break;
                case MoveTypes.COLLECT_SPICE:
                    actionType = ActionType.COLLECT;
                    break;
                case MoveTypes.TRANSFER_SPICE:
                    actionType = ActionType.TRANSFER;
                    break;
                case MoveTypes.KANLY:
                    actionType = ActionType.KANLY;
                    break;
                case MoveTypes.FAMILY_ATOMICS:
                    actionType = ActionType.FAMILY_ATOMICS;
                    break;
                case MoveTypes.SPICE_HOARDING:
                    actionType = ActionType.SPICE_HOARDING;
                    break;
                case MoveTypes.VOICE:
                    actionType = ActionType.VOICE;
                    break;
                case MoveTypes.SWORD_SPIN:
                    actionType = ActionType.SWORD_SPIN;
                    break;
                default:
                    Log.Error($"The given MoveType {type} is invalid as a character action!");
                    break;
            }
        }
    }
}
