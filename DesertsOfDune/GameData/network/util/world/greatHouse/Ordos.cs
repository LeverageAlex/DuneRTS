using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.messages;

namespace GameData.network.util.world
{
    /// <summary>
    /// This class represents the GreatHouse Ordos
    /// </summary>
    public class Ordos : GreatHouse
    {
        public Ordos() : base("ORDOS", "BLAU", GetHouseCharacters())
        {
        }

        private static HouseCharacter[] GetHouseCharacters()
        {
            HouseCharacter[] characters = new HouseCharacter[GreatHouse.AMOUNT_OF_CHARACTERS_PER_GREAT_HOUSE];
            characters[0] = new HouseCharacter("Executrix", CharacterType.NOBEL.ToString());
            characters[1] = new HouseCharacter("The Speaker", CharacterType.NOBEL.ToString());
            characters[2] = new HouseCharacter("Ammon", CharacterType.MENTAT.ToString());
            characters[3] = new HouseCharacter("Edric", CharacterType.MENTAT.ToString());
            characters[4] = new HouseCharacter("Roma Atani", CharacterType.MENTAT.ToString());
            characters[5] = new HouseCharacter("Robot", CharacterType.FIGHTHER.ToString());

            return characters;
        }
    }
}
}
