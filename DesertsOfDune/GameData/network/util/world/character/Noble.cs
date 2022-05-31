﻿using GameData.Configuration;
using GameData.network.messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.util.world.character
{
    /// <summary>
    /// This Class represents the character type Nobel
    /// </summary>
    public class Noble : Character
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
        public Noble(int healthMax, int healthCurrent, int healingHP, int MPmax, int MPcurrent, int APmax, int APcurrent, int attackDamage, int inventorySize, int inventoryUsed, bool killedBySandworm, bool isLoud) : base(CharacterType.NOBLE, healthMax, healthCurrent, healingHP,  MPmax, MPcurrent, APmax, APcurrent, attackDamage, inventorySize, inventoryUsed, killedBySandworm, isLoud)
        {

        }

        /// <summary>
        /// creates a new nobel 
        /// </summary>
        public Noble() : base(CharacterType.NOBLE, CharacterConfiguration.Noble.maxHP, CharacterConfiguration.Noble.maxMP, CharacterConfiguration.Noble.maxAP, CharacterConfiguration.Noble.damage, CharacterConfiguration.Noble.inventorySize, CharacterConfiguration.Noble.healingHP) {
        }

        /// <summary>
        /// This method resets the data of the character
        /// </summary>
        override
        public void ResetData()
        {
            this.characterType = Enum.GetName(characterType.GetType(), characterType);
            this.healthMax = CharacterConfiguration.Noble.maxHP;
            this.healthCurrent = CharacterConfiguration.Noble.maxHP;
            this.healingHP = CharacterConfiguration.Noble.healingHP;
            this.MPmax = CharacterConfiguration.Noble.maxMP;
            this.MPcurrent = CharacterConfiguration.Noble.maxMP;
            this.APmax = CharacterConfiguration.Noble.maxAP;
            this.APcurrent = CharacterConfiguration.Noble.maxAP;
            this.attackDamage = CharacterConfiguration.Noble.damage;
            this.inventorySize = CharacterConfiguration.Noble.inventorySize;
            this.inventoryUsed = 0;
            this.killedBySandworm = false;
            this.isLoud = false;
        }

        /// <summary>
        /// This method represents the action Kanly for the Character type Nobel
        /// </summary>
        /// <param name="target">the Nobel that is targeted by the atack</param>
        /// <returns>true, if action was successful</returns>
        override
        public bool Kanly(Character target)
        {
            // TODO: implement logic.
            return false;
        }

        /// <summary>
        /// This method represents the action FamilyAtomic
        /// </summary>
        /// <param name="target">The target Field for the Atack</param>
        /// <returns>true, if the action was successful</returns>
        override
        public bool AtomicBomb(MapField target)
        {
            // TODO: implement logic.
            return false;
        }
    }
}