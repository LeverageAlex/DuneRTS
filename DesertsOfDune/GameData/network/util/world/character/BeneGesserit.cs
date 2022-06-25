using GameData.Configuration;
using GameData.network.messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.util.world.character
{
    /// <summary>
    /// This Class represents the character type BeneGesserit
    /// </summary>
    public class BeneGesserit : Character
    {

        /// <summary>
        /// This is the Constructor of the Class BeneGesserit
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
        public BeneGesserit(int healthMax, int healthCurrent, int healingHP, int MPmax, int MPcurrent, int APmax, int APcurrent, int attackDamage, int inventorySize, int inventoryUsed, bool killedBySandworm, bool isLoud) : base(CharacterType.BENE_GESSERIT, healthMax, healthCurrent, healingHP, MPmax, MPcurrent, APmax, APcurrent, attackDamage, inventorySize, inventoryUsed, killedBySandworm, isLoud)
        {

        }

        /// <summary>
        /// creates a new bene gesserit
        /// </summary>
        /// <param name="name">the name of the bene gesserit</param>
        public BeneGesserit(string name) : base(CharacterType.BENE_GESSERIT, CharacterConfiguration.BeneGesserit.maxHP, CharacterConfiguration.BeneGesserit.maxMP, CharacterConfiguration.BeneGesserit.maxAP, CharacterConfiguration.BeneGesserit.damage, CharacterConfiguration.BeneGesserit.inventorySize, CharacterConfiguration.BeneGesserit.healingHP, name)
        {
        }

        /// <summary>
        /// This method resets the data of the character
        /// </summary>
        public override void ResetData()
        {
            this.characterType = Enum.GetName(typeof(CharacterType), CharacterType.BENE_GESSERIT);
            this.healthMax = CharacterConfiguration.BeneGesserit.maxHP;
            this.healthCurrent = CharacterConfiguration.BeneGesserit.maxHP;
            this.HealingHP = CharacterConfiguration.BeneGesserit.healingHP;
            this.MPmax = CharacterConfiguration.BeneGesserit.maxMP;
            this.MPcurrent = CharacterConfiguration.BeneGesserit.maxMP;
            this.APmax = CharacterConfiguration.BeneGesserit.maxAP;
            this.APcurrent = CharacterConfiguration.BeneGesserit.maxAP;
            this.attackDamage = CharacterConfiguration.BeneGesserit.damage;
            this.inventorySize = CharacterConfiguration.BeneGesserit.inventorySize;
            this.inventoryUsed = 0;
            this.killedBySandworm = false;
            this.isLoud = false;
        }

        /// <summary>
        /// This method represents the action Voice for the Character type BeneGesserit
        /// </summary>
        /// <param name="target">the character targeted by the action</param>
        /// <returns>true, if the action was successful</returns>
        override
        public bool Voice(Character target)
        {
            int dist = Math.Abs(target.CurrentMapfield.XCoordinate - CurrentMapfield.XCoordinate) + Math.Abs(target.CurrentMapfield.ZCoordinate - CurrentMapfield.ZCoordinate);
            int inventoryFree = this.inventorySize - this.inventoryUsed;
            int spiceGift = 0;
            if (dist <= 2 
                && this.APcurrent == this.APmax
                && inventoryFree > 0
                && target.inventoryUsed > 0)
            {
               if(inventoryFree >= target.inventoryUsed)
                {
                    spiceGift = target.inventoryUsed;
                }
                else
                {
                    spiceGift = inventoryFree;
                }
                target.GiftSpice(this, spiceGift);
                SpentAp(APmax);
                return true;
            }
            return false;
        }
    }
}
