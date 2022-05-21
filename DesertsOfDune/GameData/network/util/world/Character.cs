using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.messages;
using Newtonsoft.Json;

namespace GameData.network.util.world
{
    /// <summary>
    /// This is the base class for all Characterstypes to inherit from
    /// </summary>
    public class Character
    {
        [JsonProperty]
        CharacterType CharacterType;
        [JsonProperty]
        private int healthMax;
        [JsonProperty]
        private int healthCurrent;
        [JsonProperty]
        private int healingHP;
        [JsonProperty]
        private int MPmax;
        [JsonProperty]
        private int MPcurrent;
        [JsonProperty]
        private int APmax;
        [JsonProperty]
        private int APcurrent;
        [JsonProperty]
        private int attackDamage;
        [JsonProperty]
        private int inventorySize;
        [JsonProperty]
        private int inventoryUsed;
        [JsonProperty]
        private bool killedBySandworm;
        [JsonProperty]
        private bool isLoud;

        //[JsonProperty]
        //private MapField mapField;

        // TODO: add Class Inventory and reference it as a field.

        /// <summary>
        /// This is the Constructor of the Class Character
        /// </summary>
        /// <param name="characterType">the type of the character e.g. NOBEL</param>
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
        public Character(CharacterType characterType, int healthMax, int healthCurrent, int healingHP, int MPmax, int MPcurrent, int APmax, int APcurrent, int attackDamage, int inventorySize, int inventoryUsed, bool killedBySandworm, bool isLoud)
        {
            this.CharacterType = characterType;
            this.healthMax = healthMax;
            this.healthCurrent = healthCurrent;
            this.healingHP = healingHP;
            this.MPmax = MPmax;
            this.MPcurrent = MPcurrent;
            this.APmax = APmax;
            this.APcurrent = APcurrent;
            this.attackDamage = attackDamage;
            this.inventorySize = inventorySize;
            this.inventoryUsed = inventoryUsed;
            this.killedBySandworm = killedBySandworm;
            this.isLoud = isLoud;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hp"></param>
        /// <returns></returns>
        public bool DecreaseHP(float hp)
        {
            // TODO: implement logic
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool HealIfHasntMoved()
        {
            // TODO: implement logic
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mp"></param>
        /// <returns></returns>
        public bool SpentMP(int mp)
        {
            // TODO: implement logic
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ap"></param>
        /// <returns></returns>
        public bool SpentAp(int ap)
        {
            // TODO: implement logic
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool resetMPandAp()
        {
            // TODO: implement logic
            return false;
        }

        /// <summary>
        /// this method is retuns true, if the character is dead.
        /// </summary>
        /// <returns>true, if healthpoints are equal to zero</returns>
        public bool IsDead()
        {
            if(healthCurrent == 0) return true;
            return false;
        }

        /// <summary>
        /// Getter for the fild isLoud
        /// </summary>
        /// <returns>true, if the character is loud</returns>
        public bool IsLoud()
        {
            return isLoud;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool StandingNextToCityField()
        {
            // TODO: implement logic
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startField"></param>
        /// <param name="mapField"></param>
        /// <returns></returns>
        public bool Movement(MapField startField,MapField mapField)
        {
            // TODO: implement logic
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool Atack(Character target)
        {
            // TODO: implement logic
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CollectSpice()
        {
            // TODO: implement logic
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool GiftSpice()
        {
            // TODO: implement logic
            return false;
        }
    }
}
