using System;
namespace AIClient.Moves
{
    /// <summary>
    /// Represents the action of transfering spice
    /// </summary>
    public class TransferSpice : Action
    {
        public int CharacterId { get; }

        public int AmountOfSpice { get; }

        public TransferSpice(int characterId, int amountOfSpice) : base (MoveTypes.TRANSFER_SPICE)
        {
            CharacterId = characterId;
            AmountOfSpice = amountOfSpice;
        }
    }
}
