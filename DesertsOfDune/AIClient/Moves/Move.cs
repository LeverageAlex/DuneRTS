using System;
namespace AIClient.Moves
{
    /// <summary>
    /// Represents the general idea of a "move", so either the demand to end the turn, do an action or move in any direction
    /// </summary>
    public abstract class Move
    {
        public MoveTypes Type { get; private set; }

        public Move(MoveTypes type)
        {
            this.Type = type;
        }
    }
}
