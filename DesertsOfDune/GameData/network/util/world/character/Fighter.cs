using GameData.Configuration;
using GameData.network.messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.util.world.character
{
    public class Fighter : Character
    {
        /// <summary>
        /// This is the Constructor of the Class Fighter
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
        public Fighter(int healthMax, int healthCurrent, int healingHP, int MPmax, int MPcurrent, int APmax, int APcurrent, int attackDamage, int inventorySize, int inventoryUsed, bool killedBySandworm, bool isLoud) : base(CharacterType.FIGHTER, healthMax, healthCurrent, healingHP, MPmax, MPcurrent, APmax, APcurrent, attackDamage, inventorySize, inventoryUsed, killedBySandworm, isLoud)
        {

        }

        /// <summary>
        /// creates a new fighter 
        /// </summary>
        /// <param name="name">the name of the figther</param>
        public Fighter(string name) : base(CharacterType.FIGHTER, CharacterConfiguration.Fighter.maxHP, CharacterConfiguration.Fighter.maxMP, CharacterConfiguration.Fighter.maxAP, CharacterConfiguration.Fighter.damage, CharacterConfiguration.Fighter.inventorySize, CharacterConfiguration.Fighter.healingHP, name)
        {
        }

        /// <summary>
        /// This method resets the data of the character
        /// </summary>
        public override void ResetData()
        {
            this.characterType = Enum.GetName(typeof(CharacterType), CharacterType.FIGHTER);
            this.healthMax = CharacterConfiguration.Fighter.maxHP;
            this.healthCurrent = CharacterConfiguration.Fighter.maxHP;
            this.HealingHP = CharacterConfiguration.Fighter.healingHP;
            this.MPmax = CharacterConfiguration.Fighter.maxMP;
            this.MPcurrent = CharacterConfiguration.Fighter.maxMP;
            this.APmax = CharacterConfiguration.Fighter.maxAP;
            this.APcurrent = CharacterConfiguration.Fighter.maxAP;
            this.attackDamage = CharacterConfiguration.Fighter.damage;
            this.inventorySize = CharacterConfiguration.Fighter.inventorySize;
            this.inventoryUsed = 0;
            this.killedBySandworm = false;
            this.isLoud = false;

        }

        /// <summary>
        /// This method represents the action SwordSpin of the character type fighter
        /// </summary>
        /// <returns>characters hit by the sword spin</returns>
        
        public override List<Character> SwordSpin(Map map)
        {
            List<Character> charactersHit = new List<Character>();
            if(this.APcurrent == this.APmax)
            {
                List<MapField> NeighborFields = map.GetNeighborFields(this.CurrentMapfield);
                foreach (var mapfield in NeighborFields)
                {
                    if (mapfield.IsCharacterStayingOnThisField)
                    {
                        if(mapfield.Character.greatHouse != this.greatHouse && !mapfield.Character.IsInSandStorm(map))
                        {
                            Attack(mapfield.Character);
                            charactersHit.Add(mapfield.Character);
                        }

                    }
                }
                SpentAp(APmax);
            }
            return charactersHit;
        }
    }
}
