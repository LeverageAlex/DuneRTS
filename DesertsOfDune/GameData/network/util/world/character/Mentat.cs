using GameData.Configuration;
using GameData.network.messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameData.network.util.world.character
{
    /// <summary>
    /// This Class represents the character type Mentat
    /// </summary>
    public class Mentat : Character
    {
        /// <summary>
        /// This is the Constructor of the Class Mentat
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
        public Mentat(int healthMax, int healthCurrent, int healingHP, int MPmax, int MPcurrent, int APmax, int APcurrent, int attackDamage, int inventorySize, int inventoryUsed, bool killedBySandworm, bool isLoud) : base(CharacterType.MENTAT, healthMax, healthCurrent, healingHP, MPmax, MPcurrent, APmax, APcurrent, attackDamage, inventorySize, inventoryUsed, killedBySandworm, isLoud)
        {

        }

        /// <summary>
        /// creates a new mentat 
        /// </summary>
        /// <param name="name">the name of the mentat</param>
        public Mentat(string name) : base(CharacterType.MENTAT, CharacterConfiguration.Mentat.maxHP, CharacterConfiguration.Mentat.maxMP, CharacterConfiguration.Mentat.maxAP, CharacterConfiguration.Mentat.damage, CharacterConfiguration.Mentat.inventorySize, CharacterConfiguration.Mentat.healingHP, name)
        {
        }

        /// <summary>
        /// This method resets the data of the character
        /// </summary>
        public override void ResetData()
        {
            this.characterType = Enum.GetName(typeof(CharacterType), CharacterType.NOBLE);
            this.healthMax = CharacterConfiguration.Mentat.maxHP;
            this.healthCurrent = CharacterConfiguration.Mentat.maxHP;
            this.HealingHP = CharacterConfiguration.Mentat.healingHP;
            this.MPmax = CharacterConfiguration.Mentat.maxMP;
            this.MPcurrent = CharacterConfiguration.Mentat.maxMP;
            this.APmax = CharacterConfiguration.Mentat.maxAP;
            this.APcurrent = CharacterConfiguration.Mentat.maxAP;
            this.attackDamage = CharacterConfiguration.Mentat.damage;
            this.inventorySize = CharacterConfiguration.Mentat.inventorySize;
            this.inventoryUsed = 0;
            this.killedBySandworm = false;
            this.isLoud = false;
        }

        /// <summary>
        /// This method represents the action SpiceHoarding of the characterType Mentat
        /// </summary>
        /// <returns>true, if action was successful</returns>
        public override bool SpiceHoarding(Map map)
        {
            int inventoryFree = this.inventorySize - this.inventoryUsed;
            List<MapField> NeighborFields = map.GetNeighborFields(this.CurrentMapfield);
            NeighborFields.Add(this.CurrentMapfield);
            Random rnd = new Random();
            if (this.APcurrent == this.APmax && inventoryFree > 0)
            {
                MapField spiceField;
                while (inventoryFree > 0 && NeighborFields.Count > 0)
                {
                    spiceField = NeighborFields[rnd.Next(NeighborFields.Count)];
                    if (spiceField.hasSpice)
                    {
                        inventoryUsed++;
                        spiceField.hasSpice = false;
                        inventoryFree--;
                        if (spiceField.tileType == "DUNE" || spiceField.tileType == "FLAT_SAND")
                        {
                            this.SetLoud();
                        }
                    }
                    NeighborFields.Remove(spiceField);
                }
                SpentAp(APmax);
                
                return true;
            }
            return false;
        }
    }


}
