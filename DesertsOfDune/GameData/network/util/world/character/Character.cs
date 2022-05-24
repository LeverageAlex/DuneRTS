using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.messages;
using Newtonsoft.Json;
using GameData.network.util.enums;

namespace GameData.network.util.world
{
    /// <summary>
    /// This is the base class for all Characterstypes to inherit from
    /// </summary>
    public class Character
    {
        [JsonProperty]
        protected string characterType;
        [JsonProperty]
        protected int healthMax;
        [JsonProperty]
        protected int healthCurrent;
        [JsonProperty]
        protected int healingHP;
        [JsonProperty]
        protected int MPmax;
        [JsonProperty]
        public int MPcurrent { get; }
        [JsonProperty]
        protected int APmax;
        [JsonProperty]
        protected int APcurrent;
        [JsonProperty]
        protected int attackDamage;
        [JsonProperty]
        protected int inventorySize;
        [JsonProperty]
        protected int inventoryUsed;
        [JsonProperty]
        protected bool killedBySandworm;
        [JsonProperty]
        protected bool isLoud;
        [JsonIgnore]
        protected MapField currentMapfield;
        [JsonIgnore]
        public MapField CurrentMapfield { get{return currentMapfield; }set{ currentMapfield = value;}}
        [JsonIgnore]
        public int CharacterId { get; }

        private GreatHouse greatHouse;
        [JsonIgnore]
        public bool KilledBySandworm
        {get { return killedBySandworm; } set { killedBySandworm = value; } }
        
        [JsonIgnore]
        public HouseCharacter HouseCharacter { get; }


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
            this.characterType = Enum.GetName(characterType.GetType(), characterType);
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
        /// create a new character (initially for adding to the great house)
        /// TODO: set parameters, set other values
        /// </summary>
        /// <param name="type"></param>
        /// <param name="maxHP"></param>
        /// <param name="maxMP"></param>
        /// <param name="maxAP"></param>
        /// <param name="damage"></param>
        /// <param name="inventorySize"></param>
        /// <param name="healingHP"></param>
        public Character(CharacterType type, int maxHP, int maxMP, int maxAP, int damage, int inventorySize, int healingHP)
        {
            this.characterType = Enum.GetName(characterType.GetType(), characterType);
            this.healthMax = maxHP;
            this.MPmax = maxMP;
            this.APmax = maxAP;
            this.attackDamage = damage;
            this.inventorySize = inventorySize;
            this.healingHP = healingHP;
        }

        /// <summary>
        /// This method sets the character data to default values.
        /// </summary>
        public void ResetData()
        {
            // this method should be extended by different character types
        }

        /// <summary>
        /// This method decreases the hp of the character by a specific amount.
        /// </summary>
        /// <param name="hp">the healthpoints to decrease the current healthpoints by.</param>
        /// <returns>true, if decreasing the hp was possible</returns>
        public bool DecreaseHP(int hp)
        {
            if (healthCurrent > hp)
            {
                healthCurrent = healthCurrent - hp;
                return true;
            } else
            {
                return false;
            }
        }

        /// <summary>
        /// this method heals the character if is has not moved this round.
        /// </summary>
        /// <returns>true, if character was healed</returns>
        public bool HealIfHasntMoved()
        {
            // TODO: add if moved this round clause
            if (healthCurrent + healingHP < healthMax)
            {
                this.healthCurrent = healthCurrent + healingHP;
                return true;
            } else
            {
                this.healthCurrent = healthMax;
                return false;
            }
        }

        /// <summary>
        /// this method is used to spent movementpoints
        /// </summary>
        /// <param name="mp">the movement points to substract</param>
        /// <returns>true, if substraction was possible</returns>
        public bool SpentMP(int mp)
        {
            if (mp <= MPcurrent)
            {
                this.MPcurrent -= mp;
                return true;
            }
            return false;
        }

        /// <summary>
        /// this method reduces the player abilty points by a certain amount
        /// </summary>
        /// <param name="ap">the amount of ability points to reduce by</param>
        /// <returns>true, if ap reduction was possible</returns>
        public bool SpentAp(int ap)
        {
            if (APcurrent >= ap)
            {
                this.APcurrent -= ap;
                return true;
            }
            return false;
        }

        /// <summary>
        /// this method resets the movement and action points
        /// </summary>
        /// <returns>true</returns>
        public bool resetMPandAp()
        {
            this.MPcurrent = MPmax;
            this.APcurrent = APmax;
            return true;
        }

        /// <summary>
        /// this method is retuns true, if the character is dead.
        /// </summary>
        /// <returns>true, if healthpoints are equal to zero</returns>
        public bool IsDead()
        {
            if (healthCurrent == 0)
            {
                return true;
            }
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
        /// This method is used to set a character to loud
        /// </summary>
        public void SetLoud()
        {
            isLoud = true;
        }

        /// <summary>
        /// this method tells weather the character stands next to his city.
        /// </summary>
        /// <returns>true, if the character stands next to his city</returns>
        public bool StandingNextToCityField()
        {
            if (Math.Abs(this.greatHouse.City.XCoordinate - this.currentMapfield.XCoordinate) <= 1)
            {
                if (Math.Abs(this.greatHouse.City.ZCoordinate - this.currentMapfield.ZCoordinate) <= 1)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// This method handles the movement of a character
        /// </summary>
        /// <param name="startField">the start field of the character</param>
        /// <param name="goalField">the goal field of the character</param>
        /// <returns></returns>
        public bool Movement(MapField startField,MapField goalField)
        {
            // TODO: implement logic
            int dist = Math.Abs(startField.XCoordinate - goalField.XCoordinate) + Math.Abs(startField.ZCoordinate - goalField.ZCoordinate);
            if (dist > 2)
            {
                return false;
            }
            currentMapfield = goalField;
            return true;
        }

        /// <summary>
        /// this method contains the logic for a atack from one character to a nother character.
        /// </summary>
        /// <param name="target">the character targeted by the atack</param>
        /// <returns>true, if atack was possible</returns>
        public bool Atack(Character target)
        {
            int dist = Math.Abs(target.currentMapfield.XCoordinate - currentMapfield.XCoordinate) + Math.Abs(target.currentMapfield.ZCoordinate - currentMapfield.ZCoordinate);
            if (APcurrent > 0 && dist <= 2 && target.greatHouse != greatHouse)
            {
                APcurrent--;
                target.DecreaseHP(attackDamage);
                return true;
            }
            return false;
        }

        /// <summary>
        /// This method holds the logic for the action collect spice
        /// </summary>
        /// <returns>true, if the action was possible else false</returns>
        public bool CollectSpice()
        {
            if (APcurrent > 0 && currentMapfield.HasSpice && inventoryUsed < inventorySize)
            {
                APcurrent--;
                inventoryUsed++;
                return true;
            }
            return false;
        }

        /// <summary>
        /// This method holds the logic for converting spice from a player to his city.
        /// </summary>
        /// <returns>true, if action was possible</returns>
        public bool GiftSpice(Character targetCharacter, int amount)
        {
            targetCharacter.inventoryUsed += amount;
            return false;
        }
    }
}
