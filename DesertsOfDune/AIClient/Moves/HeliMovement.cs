using System;
using GameData.network.util.world;

namespace AIClient.Moves
{
    /// <summary>
    /// Represents the "move" to move a character via the helicopter to a other heliport
    /// </summary>
    public class HeliMovement : Movement
    {
        /// <summary>
        /// creates a new movement move and set the delta x and y movement of this move depending on the target field
        /// </summary>
        /// <param name="targetField">the position of the target heliport</param>
        public HeliMovement(Position currentField, Position targetField) : base (MoveTypes.MOVE_WITH_HELI)
        {
            DeltaX = targetField.x - currentField.x;
            DeltaY = targetField.y - currentField.y;
        }
    }
}

