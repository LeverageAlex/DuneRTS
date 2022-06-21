using System;
namespace AIClient.Moves
{
    /// <summary>
    /// Represents the move of ending a turn prematurely 
    /// </summary>
    public class EndTurn : Move
    {
        public EndTurn() : base (MoveTypes.END_TURN)
        {
        }
    }
}
