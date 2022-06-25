using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.messages;
using Newtonsoft.Json;
using GameData.network.util.enums;
using System.Numerics;
using GameData.Configuration;

namespace GameData.network.util.world
{
    /// <summary>
    /// This is the base class for all Characterstypes to inherit from
    /// </summary>
    public class Character
    {
        [JsonProperty(Order = -2)]
        public string characterType { get; set; }
        [JsonProperty(Order = -1)]
        public int healthMax { get; set; }
        [JsonProperty(Order = 0)]
        public int healthCurrent { get; set; }
        [JsonProperty(Order = 1)]
        public int healingHP { get; private set; }
        [JsonProperty(Order = 2)]
        public int MPmax;
        [JsonProperty(Order = 3)]
        public int MPcurrent { get; set; }
        [JsonProperty(Order = 4)]
        public int APmax { get; set; }
        [JsonProperty(Order = 5)]
        public int APcurrent { get; set; }
        [JsonProperty(Order = 6)]
        public int attackDamage { get; set; }
        [JsonProperty(Order = 7)]
        public int inventorySize { get; set; }
        [JsonProperty(Order = 8)]
        public int inventoryUsed { get; set; }
        [JsonProperty(Order = 9)]
        public bool killedBySandworm { get; set; }
        [JsonProperty(Order = 10)]
        public bool isLoud { get; set; }
        // [JsonIgnore]
        // protected MapField currentMapfield;
        [JsonIgnore]
        public MapField CurrentMapfield { get; set; }
        [JsonIgnore]
        public int CharacterId { get; set; }
        [JsonIgnore]
        public GreatHouse greatHouse { get; set; }
        [JsonIgnore]
        public bool KilledBySandworm
        { get { return killedBySandworm; } set { killedBySandworm = value; } }

        [JsonIgnore]
        public int HealingHP
        { get { return healingHP; } set { healingHP = value; } }

        [JsonIgnore]
        public string CharacterName { get; set; }

        [JsonIgnore]
        public bool Shunned { get; set; }

        /// <summary>
        /// This list is used for the characters which should be add after greatHouseHouseConvention gets broken for the first time.
        /// </summary>
        [JsonIgnore]
        public static List<Character> CharactersToAddAfterAtomics { get; set; }


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
        [JsonConstructor]
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

        public Character(int characterId, string characterName, CharacterType characterType, int healthMax, int healthCurrent, int healingHP, int MPmax, int MPcurrent, int APmax, int APcurrent, int attackDamage, int inventorySize, int inventoryUsed, bool killedBySandworm, bool isLoud)
        {
            this.CharacterId = characterId;
            this.CharacterName = characterName;
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
            this.Shunned = false;
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
        /// <param name="name">the name of the character</param>
        protected Character(CharacterType type, int maxHP, int maxMP, int maxAP, int damage, int inventorySize, int healingHP, string name)
        {
            this.characterType = Enum.GetName(typeof(CharacterType), type);
            this.healthMax = maxHP;
            this.healthCurrent = maxHP;
            this.healingHP = healingHP;
            this.MPmax = maxMP;
            this.MPcurrent = maxMP;
            this.APmax = maxAP;
            this.APcurrent = maxAP;
            this.attackDamage = damage;
            this.inventorySize = inventorySize;
            this.CharacterName = name;

            // set meaningful default values for the remaining attributes
            this.inventoryUsed = 0;
            this.killedBySandworm = false;
            this.isLoud = false;

            Random random = new Random();
            this.CharacterId = random.Next(int.MaxValue);
            this.Shunned = false;
        }

        /// <summary>
        /// This method sets the character data to default values in the in the respective inherited classes.
        /// </summary>
        public virtual void ResetData()
        {

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
                healthCurrent -= hp;
                return true;
            }
            else
            {
                Console.WriteLine("Achtung! Es wurde die sehr gefährliche (fehleranfählige!) Methode DecreaseHP aufgerufen. Falls irgendwelche Seiteneffekte was Felder/Positionen betrifft hier nachschauen");
                healthCurrent = 0;
                CurrentMapfield.DisplaceCharacter(this);  //Super strange, because no CharacterStatsUpdate is sent bevor update
                return false;
            }
        }

        /// <summary>
        /// this method heals the character if is has not moved this round.
        /// </summary>
        /// <returns>true, if character was healed</returns>
        public bool HealIfHasntMoved()
        {
            if (healthCurrent + healingHP < healthMax)
            {
                this.healthCurrent = healthCurrent + healingHP;
                return true;
            }
            else
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
            if (healthCurrent <= 0)
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
        /// This method is used to set a character to silent
        /// </summary>
        public void SetSilent()
        {
            isLoud = false;
        }

        /// <summary>
        /// this method tells weather the character stands next to his city.
        /// </summary>
        /// <returns>true, if the character stands next to his city</returns>
        public bool StandingNextToCityField()
        {
            if (Math.Abs(this.greatHouse.City.XCoordinate - this.CurrentMapfield.XCoordinate) <= 1)
            {
                if (Math.Abs(this.greatHouse.City.ZCoordinate - this.CurrentMapfield.ZCoordinate) <= 1)
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
        public bool Movement(MapField startField, MapField goalField)
        {
            double dist = Math.Sqrt(Math.Pow(Math.Abs(startField.XCoordinate - goalField.XCoordinate), 2) + Math.Pow(Math.Abs(startField.ZCoordinate - goalField.ZCoordinate), 2));
            if (dist >= 2 || dist == 0)
            {
                return false;
            }
            SpentMP(1);
            CurrentMapfield = goalField;
            startField.DisplaceCharacter(this);
            goalField.PlaceCharacter(this);
            return true;
        }

        public void Teleport(MapField targetField)
        {
            CurrentMapfield.DisplaceCharacter(this);
            CurrentMapfield = targetField;
        }

        /// <summary>
        /// this method contains the logic for a atack from one character to a nother character.
        /// </summary>
        /// <param name="target">the character targeted by the atack</param>
        /// <returns>true, if attack was possible</returns>
        public bool Attack(Character target)
        {
            int dist = Math.Abs(target.CurrentMapfield.XCoordinate - CurrentMapfield.XCoordinate) + Math.Abs(target.CurrentMapfield.ZCoordinate - CurrentMapfield.ZCoordinate);
            if (APcurrent > 0 && dist <= 2 && target.greatHouse != greatHouse)
            {
                SpentAp(1);
                if (CurrentMapfield.Elevation == target.CurrentMapfield.Elevation)
                {
                    target.DecreaseHP(attackDamage);
                }
                else if (CurrentMapfield.Elevation == Elevation.high && target.CurrentMapfield.Elevation == Elevation.low)
                {
                    target.DecreaseHP((int)Math.Round(attackDamage * PartyConfiguration.GetInstance().highGroundBonusRatio));
                }
                else if (CurrentMapfield.Elevation == Elevation.low && target.CurrentMapfield.Elevation == Elevation.high)
                {
                    target.DecreaseHP((int)Math.Round(attackDamage * PartyConfiguration.GetInstance().lowerGroundMalusRatio));
                }
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
            if (APcurrent > 0 && CurrentMapfield.hasSpice && inventoryUsed < inventorySize)
            {
                SpentAp(1);
                inventoryUsed++;
                CurrentMapfield.hasSpice = false;
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
            if (this.inventoryUsed - amount >= 0)
            {
                SpentAp(1);
                targetCharacter.inventoryUsed += amount;
                this.inventoryUsed -= amount;
                return true;
            }
            return false;
        }

        /// <summary>
        /// This method is needed to get overridden by the Noble class to perform the Kanly special aciton
        /// </summary>
        /// <param name="targetCharacter"> this is the Character who gets Attacked by the active noble character; needs to be a noble </param>
        /// <returns>true if kanly was successful</returns>
        virtual
        public bool Kanly(Character targetCharacter)
        {
            //Do nothing because only Nobles can perform this move
            return false;
        }

        /// <summary>
        /// This method is needed to get overriden by the Nobel class to perform the AtomicBomb special action
        /// </summary>
        /// <param name="target"> this is the Field on the Map where the active Noble character aims its atomic bomb to </param>
        /// <returns>characters hit by the atomic bomb</returns>
        virtual
        public List<Character> AtomicBomb(MapField target, Map map, bool greatHouseConventionBroken, GreatHouse activePlayerGreatHouse, GreatHouse passivePlayerGreatHouse)
        {
            //Do nothing because only Nobles can perform this move
            return new List<Character>();
        }

        /// <summary>
        /// This method is needed to get overridden by the Mentat class to perform the SpiceHoarding special action
        /// </summary>
        /// <returns>true, if spice hoarding was successful</returns>
        virtual
        public bool SpiceHoarding(Map map)
        {
            //Do nothing beacuse only Mentats can perform this move
            return false;
        }

        /// <summary>
        /// This method is needed to get overridden by the BeneGesserit class to perform the Voice special action
        /// </summary>
        /// <param name="target"> this is the character who gets focused by the active Bene Gesserit character </param>
        /// <returns>true, if voice was successful</returns>
        virtual
        public bool Voice(Character target)
        {
            //Do nothing because only BeneGesserits can perform this move
            return false;
        }

        /// <summary>
        /// This method is needed to get overridden by the Fighter class to perform the SwordSpin special attack
        /// </summary>
        /// <returns>charactes hit by the sword spin</returns>
        virtual
        public List<Character> SwordSpin(Map map)
        {
            //Do nothing because only Fighters can perform this move
            return new List<Character>();
        }

        /// <summary>
        /// This method is used to check if the Character is staying in the sandstorm
        /// </summary>
        /// <param name="map">The map where the player is</param>
        /// <returns>true if the characters stands in the sandstorm</returns>
        public bool IsInSandStorm(Map map)
        {
            if (CurrentMapfield.GetMapFieldPosition().x == map.PositionOfEyeOfStorm.x && CurrentMapfield.GetMapFieldPosition().y == map.PositionOfEyeOfStorm.y)
            {
                return true;
            }
            foreach (var mapfield in map.GetNeighborFields(CurrentMapfield))
            {
                if (mapfield.GetMapFieldPosition().x == map.PositionOfEyeOfStorm.x && mapfield.GetMapFieldPosition().y == map.PositionOfEyeOfStorm.y)
                {
                    return true;
                }
            }
            return false;
        }
    }
}