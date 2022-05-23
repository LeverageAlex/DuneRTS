using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.messages;

namespace GameData.network.util.world
{
    /// <summary>
    /// This class represents the GreatHouse Harkonnen
    /// </summary>
    public class Harkonnen : GreatHouse
    {
        public Harkonnen() : base("HARKONNEN", "ROT", GetHouseCharacters())
        {
        }

        private static HouseCharacter[] GetHouseCharacters()
        {
            HouseCharacter[] characters = new HouseCharacter[GreatHouse.AMOUNT_OF_CHARACTERS_PER_GREAT_HOUSE];
            characters[0] = new HouseCharacter("Baron Vladimir Harkonnen", CharacterType.NOBEL.ToString());
            characters[1] = new HouseCharacter("Count Glossu Beast Rabban", CharacterType.FIGHTHER.ToString());
            characters[2] = new HouseCharacter("Feyd-Rautha Rabban", CharacterType.FIGHTHER.ToString());
            characters[3] = new HouseCharacter("Piter De Vries", CharacterType.MENTAT.ToString());
            characters[4] = new HouseCharacter("Iakin Nefud", CharacterType.FIGHTHER.ToString());
            characters[5] = new HouseCharacter("Pet Spider", CharacterType.FIGHTHER.ToString());

            return characters;
        }
    }
}
}
