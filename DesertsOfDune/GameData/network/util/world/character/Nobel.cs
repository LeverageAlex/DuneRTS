using GameData.network.messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.util.world.character
{
    /// <summary>
    /// This Class represents the character type Nobel
    /// </summary>
    public class Nobel : Character
    {
        /// <summary>
        /// This is the Constructor of the Class Nobel
        /// </summary>
        /// <param name="healthMax">the maximum health of the Character</param>
        /// <param name="healthCurrent">the current healthpoints of the Character</param>
        /// <param name="healingHP">the healingHealthPoints of the Character</param>
        /// <param name="MPmax">the maximum movementpoints of the Character</param>
        /// <param name="MPcurrent">the current MovementPoints of the Character</param>
        /// <param name="APmax">the maximum Ability Points of the Character</param>
        /// <param name="APcurrent">the current Ability Points of the Character</param>
        /// <param name="attackDamage">the atack damage of the Character</param>
        /// <param name="inventorySize">the inventorySize of the Character</param>
        /// <param name="inventoryUsed">the usedup InventorySpace of the Character</param>
        /// <param name="killedBySandworm">true, if the Character was killed by the sandworm</param>
        /// <param name="isLoud">true, if the character is loud</param>
        public Nobel(int healthMax, int healthCurrent, int healingHP, int MPmax, int MPcurrent, int APmax, int APcurrent, int attackDamage, int inventorySize, int inventoryUsed, bool killedBySandworm, bool isLoud) : base(CharacterType.NOBEL, healthMax, healthCurrent, healingHP,  MPmax, MPcurrent, APmax, APcurrent, attackDamage, inventorySize, inventoryUsed, killedBySandworm, isLoud)
        {

        }
        /// <summary>
        /// This method represents the action Kanly for the Character type Nobel
        /// </summary>
        /// <param name="target">the Nobel that is targeted by the atack</param>
        /// <returns>true, if action was successful</returns>
        public bool Kanly(Nobel target)
        {
            // TODO: implement logic.
            return false;
        }

        /// <summary>
        /// This method represents the action FamilyAtomic
        /// </summary>
        /// <param name="target">The target Field for the Atack</param>
        /// <returns>true, if the action was successful</returns>
        public bool AtomicBomb(MapField target)
        {
            // TODO: implement logic.
            return false;
        }
    }
}
