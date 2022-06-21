using System;
using GameData.network.messages;
using GameData.network.util.world;
using Serilog;

namespace AIClient.Moves
{
    /// <summary>
    /// Represents the "move" to move a character in a direction
    /// </summary>
    public class Action : Move
    {
        public ActionType ActionType { get; }

        public Position Target { get; }


        /// <summary>
        /// create a new 'action' move
        /// </summary>
        /// <param name="type">the type of the action</param>
        /// <param name="target">the position of the target of target (no always used, so sometimes is null)</param>
        public Action(MoveTypes type, Position target) : base(type)
        {
            switch(type){
                case MoveTypes.ATTACK:
                    ActionType = ActionType.ATTACK;
                    break;
                case MoveTypes.COLLECT_SPICE:
                    ActionType = ActionType.COLLECT;
                    break;
                case MoveTypes.TRANSFER_SPICE:
                    ActionType = ActionType.TRANSFER;
                    break;
                case MoveTypes.KANLY:
                    ActionType = ActionType.KANLY;
                    break;
                case MoveTypes.FAMILY_ATOMICS:
                    ActionType = ActionType.FAMILY_ATOMICS;
                    break;
                case MoveTypes.SPICE_HOARDING:
                    ActionType = ActionType.SPICE_HOARDING;
                    break;
                case MoveTypes.VOICE:
                    ActionType = ActionType.VOICE;
                    break;
                case MoveTypes.SWORD_SPIN:
                    ActionType = ActionType.SWORD_SPIN;
                    break;
                default:
                    Log.Error($"The given MoveType {type} is invalid as a character action!");
                    break;
            }

            this.Target = target;
        }

        /// <summary>
        /// create a new 'action' move, so e. g. a spice hoaring
        /// </summary>
        /// <param name="type">the type of the action</param>
        public Action(MoveTypes type) : this(type, null)
        {
            
        }
    }
}
