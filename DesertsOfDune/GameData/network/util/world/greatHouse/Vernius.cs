using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.messages;

namespace GameData.network.util.world
{
    /// <summary>
    /// This class represents the GreatHouse Vernius
    /// </summary>
    public class Vernius : GreatHouse
    {
        public Vernius() : base("VERNIUS", "VIOLETT", GetHouseCharacters())
        {
        }

        private static HouseCharacter[] GetHouseCharacters()
        {
            HouseCharacter[] characters = new HouseCharacter[GreatHouse.AMOUNT_OF_CHARACTERS_PER_GREAT_HOUSE];
            characters[0] = new HouseCharacter("Earl Dominic Vernius", CharacterType.NOBEL.ToString());
            characters[1] = new HouseCharacter("Lady Shando Vernius", CharacterType.NOBEL.ToString());
            characters[2] = new HouseCharacter("Kailea Vernius", CharacterType.NOBEL.ToString());
            characters[3] = new HouseCharacter("Tessia Vernius", CharacterType.BENEGESSERIT.ToString());
            characters[4] = new HouseCharacter("Rhombur Vernius", CharacterType.FIGHTHER.ToString());
            characters[5] = new HouseCharacter("Bronso Vernius", CharacterType.MENTAT.ToString());

            return characters;
        }
    }
}
}
