using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.messages;

namespace GameData.network.util.world
{
    /// <summary>
    /// This class represents the GreatHouse Richese
    /// </summary>
    public class Richese : GreatHouse
    {
        public Richese() : base("RICHESE", "SILBER", GetHouseCharacters())
        {
        }

        private static HouseCharacter[] GetHouseCharacters()
        {
            HouseCharacter[] characters = new HouseCharacter[GreatHouse.AMOUNT_OF_CHARACTERS_PER_GREAT_HOUSE];
            characters[0] = new HouseCharacter("Count Ilban Richese", CharacterType.NOBEL.ToString());
            characters[1] = new HouseCharacter("Helena Richese", CharacterType.NOBEL.ToString());
            characters[2] = new HouseCharacter("Haloa Rund", CharacterType.MENTAT.ToString());
            characters[3] = new HouseCharacter("Flinto Kinnis", CharacterType.MENTAT.ToString());
            characters[4] = new HouseCharacter("Tenu Chobyn", CharacterType.MENTAT.ToString());
            characters[5] = new HouseCharacter("Yresk", CharacterType.FIGHTHER.ToString());

            return characters;
        }
    }
}
}
