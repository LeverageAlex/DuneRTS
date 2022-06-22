using System;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace AIClient.Moves
{
    /// <summary>
    /// Represents the "move" to move a character in a direction
    /// </summary>
    public class Movement : Move
    {
        public int DeltaX { get; }
        public int DeltaY { get; }

        /// <summary>
        /// creates a new movement move and set the delta x and y movement of this move depending on the type
        /// </summary>
        /// <param name="type">the movement type respectivly the movement direction</param>
        public Movement(MoveTypes type) : base(type)
        {
            // TODO: check, whether dy = -1 really is moving upwards
            switch (type)
            {
                case MoveTypes.MOVE_LEFT_UP:
                    DeltaX = -1;
                    DeltaY = 1;
                    break;
                case MoveTypes.MOVE_UP:
                    DeltaX = 0;
                    DeltaY = 1;
                    break;
                case MoveTypes.MOVE_RIGHT_UP:
                    DeltaX = 1;
                    DeltaY = 1;
                    break;
                case MoveTypes.MOVE_RIGHT:
                    DeltaX = 1;
                    DeltaY = 0;
                    break;
                case MoveTypes.MOVE_RIGHT_DOWN:
                    DeltaX = 1;
                    DeltaY = -1;
                    break;
                case MoveTypes.MOVE_DOWN:
                    DeltaX = 0;
                    DeltaY = -1;
                    break;
                case MoveTypes.MOVE_LEFT_DOWN:
                    DeltaX = -1;
                    DeltaY = -1;
                    break;
                case MoveTypes.MOVE_LEFT:
                    DeltaX = -1;
                    DeltaY = 0;
                    break;
                default:
                    Log.Error($"The given MoveType {type} is invalid as a movement direction!");
                    break;
            }
        }
    }
}
