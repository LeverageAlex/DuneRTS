using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.messages;

namespace GameData.network.util.world
{
    /// <summary>
    /// This class represents the GreatHouse Corrino
    /// </summary>
    public class Corrino : GreatHouse
    {
        public Corrino() : base("CORRINO", "GOLD", GetHouseCharacters())
        {
        }

        private static HouseCharacter[] GetHouseCharacters()
        {
            HouseCharacter[] characters = new HouseCharacter[GreatHouse.AMOUNT_OF_CHARACTERS_PER_GREAT_HOUSE];
            characters[0] = new HouseCharacter("Emperor Shaddam IV Corrino", CharacterType.NOBEL.ToString());
            characters[1] = new HouseCharacter("Princess Irulan Corrino", CharacterType.BENEGESSERIT.ToString());
            characters[2] = new HouseCharacter("Count Hasimir Fenring", CharacterType.MENTAT.ToString());
            characters[3] = new HouseCharacter("Lady Margot Fenring", CharacterType.BENEGESSERIT.ToString());
            characters[4] = new HouseCharacter("Reverend Mother Gaius Helen Mohiam", CharacterType.BENEGESSERIT.ToString());
            characters[5] = new HouseCharacter("Captain Aramsham", CharacterType.FIGHTHER.ToString());

            return characters;
        }
    }
}
