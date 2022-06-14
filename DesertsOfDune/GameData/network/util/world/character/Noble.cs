using GameData.Configuration;
using GameData.network.messages;
using GameData.network.util.enums;
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
            int dist = Math.Abs(target.CurrentMapfield.XCoordinate - currentMapfield.XCoordinate) + Math.Abs(target.CurrentMapfield.ZCoordinate - currentMapfield.ZCoordinate);
            if (dist <= 2 && target.greatHouse != this.greatHouse && this.APcurrent == this.APmax)
            {
                target.DecreaseHP(target.healthCurrent);
                SpentAp(APmax);
                return true;
            }
            return false;
        }

        /// <summary>
        /// This method represents the action FamilyAtomic
        /// </summary>
        /// <param name="target">The target Field for the Atack</param>
        /// <returns>true, if the action was successful</returns>
        override
        public bool AtomicBomb(MapField target, Map map, bool greatHouseConventionBroken, GreatHouse activePlayerGreatHouse, GreatHouse passivePlayerGreatHouse)
        {
            if(this.APcurrent == this.APmax && this.greatHouse.unusedAtomicBombs > 0)
            {
                foreach (var mapfield in map.GetNeighborFields(target))
                {
                    switch (Enum.Parse(typeof(TileType), mapfield.tileType))
                    {
                        case TileType.DUNE:
                            mapfield.tileType = Enum.GetName(typeof(TileType), TileType.FLAT_SAND);
                            break;
                        case TileType.MOUNTAINS:
                            mapfield.tileType = Enum.GetName(typeof(TileType), TileType.PLATEAU);
                            break;
                    }
                    if (mapfield.IsCharacterStayingOnThisField)
                    {
                        mapfield.Character.DecreaseHP(mapfield.Character.healthCurrent);
                        if(!greatHouseConventionBroken)
                        {
                            //TODO: implement supportive Characters from other Houses against the Great House who used the atomic bomb
                            List < GreatHouse > remainingGreatHouses = new List<GreatHouse>();
                            if(activePlayerGreatHouse.houseName != "CORRINO" && passivePlayerGreatHouse.houseName != "CORRINO"){
                                remainingGreatHouses.Add(new Corrino());
                            }
                            if (activePlayerGreatHouse.houseName != "ATREIDES" && passivePlayerGreatHouse.houseName != "ATREIDES"){
                                remainingGreatHouses.Add(new Atreides());
                            }
                            if (activePlayerGreatHouse.houseName != "HARKONNEN" && passivePlayerGreatHouse.houseName != "HARKONNEN"){
                                remainingGreatHouses.Add(new Harkonnen());
                            }
                            if (activePlayerGreatHouse.houseName != "ORDOS" && passivePlayerGreatHouse.houseName != "ORDOS"){
                                remainingGreatHouses.Add(new Ordos());
                            }
                            if (activePlayerGreatHouse.houseName != "RICHESE" && passivePlayerGreatHouse.houseName != "RICHESE")
                            {
                                remainingGreatHouses.Add(new Richese());
                            }
                            if (activePlayerGreatHouse.houseName != "VERNIUS" && passivePlayerGreatHouse.houseName != "VERNIUS")
                            {
                                remainingGreatHouses.Add(new Vernius());
                            }
                            Random rnd = new Random();
                            foreach (var greatHouse in remainingGreatHouses)
                            {
                                int randomCharacterIndex = rnd.Next(greatHouse.Characters.Count);
                                passivePlayerGreatHouse.Characters.Add(greatHouse.Characters[randomCharacterIndex]);
                            }
                        }
                    }
                    //remove Sandworm
                    if (mapfield.HasSpice)
                    {
                        mapfield.HasSpice = false;
                    }
                }
                SpentAp(APmax);
                this.greatHouse.unusedAtomicBombs--;
                return true;
            }
            return false;
        }
    }
}
