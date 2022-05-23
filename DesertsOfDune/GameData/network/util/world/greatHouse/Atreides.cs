using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.messages;

namespace GameData.network.util.world
{
    /// <summary>
    /// This class represents the GreatHouse Atreides
    /// </summary>
    public class Atreides : GreatHouse
    {
        public Atreides() : base("ATREIDES", "GRÜN", GetHouseCharacters())
        {
        }

        private static HouseCharacter[] GetHouseCharacters()
        {
            HouseCharacter[] characters = new HouseCharacter[GreatHouse.AMOUNT_OF_CHARACTERS_PER_GREAT_HOUSE];
            characters[0] = new HouseCharacter("Duke Leto Atreides", CharacterType.NOBEL.ToString());
            characters[1] = new HouseCharacter("Paul Atreides", CharacterType.NOBEL.ToString());
            characters[2] = new HouseCharacter("Lady Jessica", CharacterType.BENEGESSERIT.ToString());
            characters[3] = new HouseCharacter("Thufir Hawat", CharacterType.MENTAT.ToString());
            characters[4] = new HouseCharacter("Gurney Halleck", CharacterType.FIGHTHER.ToString());
            characters[5] = new HouseCharacter("Space Pug, Duke Letos tapferer Mopshund", CharacterType.FIGHTHER.ToString());

            return characters;
        }
    }
}
