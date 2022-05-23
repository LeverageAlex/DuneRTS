using System;
using GameData.network.messages;
using GameData.network.util.world;

namespace GameData.Configuration
{
    public class GreatHouseConfiguration
    {
        public static HouseCharacter[] HouseCharactersAtreides { get; set; }
        public static HouseCharacter[] HouseCharactersCorrino { get; set; }
        public static HouseCharacter[] HouseCharactersHarkonnen { get; set; }
        public static HouseCharacter[] HouseCharactersOrdos { get; set; }
        public static HouseCharacter[] HouseCharactersRichese { get; set; }
        public static HouseCharacter[] HouseCharactersVernius { get; set; }

        /// <summary>
        /// creates a new great house configuration object and initiliaze, that all great house character lists are created
        /// </summary>
        public GreatHouseConfiguration()
        {
            GetHouseCharactersAtreides();
            GetHouseCharactersCorrino();
            GetHouseCharactersHarkonnen();
            GetHouseCharactersOrdos();
            GetHouseCharactersRichese();
            GetHouseCharactersVernius();
        }

        /// <summary>
        /// Gets a list of all characters (only name and type), who are contained in "Atreides"
        /// </summary>
        private void GetHouseCharactersAtreides()
        {
            HouseCharactersAtreides = new HouseCharacter[GreatHouse.AMOUNT_OF_CHARACTERS_PER_GREAT_HOUSE];
            HouseCharactersAtreides[0] = new HouseCharacter("Duke Leto Atreides", CharacterType.NOBEL.ToString());
            HouseCharactersAtreides[1] = new HouseCharacter("Paul Atreides", CharacterType.NOBEL.ToString());
            HouseCharactersAtreides[2] = new HouseCharacter("Lady Jessica", CharacterType.BENEGESSERIT.ToString());
            HouseCharactersAtreides[3] = new HouseCharacter("Thufir Hawat", CharacterType.MENTAT.ToString());
            HouseCharactersAtreides[4] = new HouseCharacter("Gurney Halleck", CharacterType.FIGHTHER.ToString());
            HouseCharactersAtreides[5] = new HouseCharacter("Space Pug, Duke Letos tapferer Mopshund", CharacterType.FIGHTHER.ToString());
        }

        /// <summary>
        /// Gets a list of all characters (only name and type), who are contained in "Corrino"
        /// </summary>
        private void GetHouseCharactersCorrino()
        {
            HouseCharactersCorrino = new HouseCharacter[GreatHouse.AMOUNT_OF_CHARACTERS_PER_GREAT_HOUSE];
            HouseCharactersCorrino[0] = new HouseCharacter("Emperor Shaddam IV Corrino", CharacterType.NOBEL.ToString());
            HouseCharactersCorrino[1] = new HouseCharacter("Princess Irulan Corrino", CharacterType.BENEGESSERIT.ToString());
            HouseCharactersCorrino[2] = new HouseCharacter("Count Hasimir Fenring", CharacterType.MENTAT.ToString());
            HouseCharactersCorrino[3] = new HouseCharacter("Lady Margot Fenring", CharacterType.BENEGESSERIT.ToString());
            HouseCharactersCorrino[4] = new HouseCharacter("Reverend Mother Gaius Helen Mohiam", CharacterType.BENEGESSERIT.ToString());
            HouseCharactersCorrino[5] = new HouseCharacter("Captain Aramsham", CharacterType.FIGHTHER.ToString());
        }

        /// <summary>
        /// Gets a list of all characters (only name and type), who are contained in "Harkonnen"
        /// </summary>
        private void GetHouseCharactersHarkonnen()
        {
            HouseCharactersHarkonnen = new HouseCharacter[GreatHouse.AMOUNT_OF_CHARACTERS_PER_GREAT_HOUSE];
            HouseCharactersHarkonnen[0] = new HouseCharacter("Baron Vladimir Harkonnen", CharacterType.NOBEL.ToString());
            HouseCharactersHarkonnen[1] = new HouseCharacter("Count Glossu Beast Rabban", CharacterType.FIGHTHER.ToString());
            HouseCharactersHarkonnen[2] = new HouseCharacter("Feyd-Rautha Rabban", CharacterType.FIGHTHER.ToString());
            HouseCharactersHarkonnen[3] = new HouseCharacter("Piter De Vries", CharacterType.MENTAT.ToString());
            HouseCharactersHarkonnen[4] = new HouseCharacter("Iakin Nefud", CharacterType.FIGHTHER.ToString());
            HouseCharactersHarkonnen[5] = new HouseCharacter("Pet Spider", CharacterType.FIGHTHER.ToString());
        }

        /// <summary>
        /// Gets a list of all characters (only name and type), who are contained in "Ordos"
        /// </summary>
        private void GetHouseCharactersOrdos()
        {
            HouseCharactersOrdos = new HouseCharacter[GreatHouse.AMOUNT_OF_CHARACTERS_PER_GREAT_HOUSE];
            HouseCharactersOrdos[0] = new HouseCharacter("Executrix", CharacterType.NOBEL.ToString());
            HouseCharactersOrdos[1] = new HouseCharacter("The Speaker", CharacterType.NOBEL.ToString());
            HouseCharactersOrdos[2] = new HouseCharacter("Ammon", CharacterType.MENTAT.ToString());
            HouseCharactersOrdos[3] = new HouseCharacter("Edric", CharacterType.MENTAT.ToString());
            HouseCharactersOrdos[4] = new HouseCharacter("Roma Atani", CharacterType.MENTAT.ToString());
            HouseCharactersOrdos[5] = new HouseCharacter("Robot", CharacterType.FIGHTHER.ToString());
        }

        /// <summary>
        /// Gets a list of all characters (only name and type), who are contained in "Richese"
        /// </summary>
        private void GetHouseCharactersRichese()
        {
            HouseCharactersRichese = new HouseCharacter[GreatHouse.AMOUNT_OF_CHARACTERS_PER_GREAT_HOUSE];
            HouseCharactersRichese[0] = new HouseCharacter("Count Ilban Richese", CharacterType.NOBEL.ToString());
            HouseCharactersRichese[1] = new HouseCharacter("Helena Richese", CharacterType.NOBEL.ToString());
            HouseCharactersRichese[2] = new HouseCharacter("Haloa Rund", CharacterType.MENTAT.ToString());
            HouseCharactersRichese[3] = new HouseCharacter("Flinto Kinnis", CharacterType.MENTAT.ToString());
            HouseCharactersRichese[4] = new HouseCharacter("Tenu Chobyn", CharacterType.MENTAT.ToString());
            HouseCharactersRichese[5] = new HouseCharacter("Yresk", CharacterType.FIGHTHER.ToString());
        }

        /// <summary>
        /// Gets a list of all characters (only name and type), who are contained in "Vernius"
        /// </summary>
        private void GetHouseCharactersVernius()
        {
            HouseCharactersVernius = new HouseCharacter[GreatHouse.AMOUNT_OF_CHARACTERS_PER_GREAT_HOUSE];
            HouseCharactersVernius[0] = new HouseCharacter("Earl Dominic Vernius", CharacterType.NOBEL.ToString());
            HouseCharactersVernius[1] = new HouseCharacter("Lady Shando Vernius", CharacterType.NOBEL.ToString());
            HouseCharactersVernius[2] = new HouseCharacter("Kailea Vernius", CharacterType.NOBEL.ToString());
            HouseCharactersVernius[3] = new HouseCharacter("Tessia Vernius", CharacterType.BENEGESSERIT.ToString());
            HouseCharactersVernius[4] = new HouseCharacter("Rhombur Vernius", CharacterType.FIGHTHER.ToString());
            HouseCharactersVernius[5] = new HouseCharacter("Bronso Vernius", CharacterType.MENTAT.ToString());
        }
    }


}
