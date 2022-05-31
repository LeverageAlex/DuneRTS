using System;
using GameData.network.util.world;

namespace GameData.Configuration
{
    public static class Configuration
    {
        /// <summary>
        /// 
        /// </summary>
        public static void InitializeConfigurations()
        {
            //initialize greatHouses with houseCharacters
            GreatHouseConfiguration characterConfiguration = new GreatHouseConfiguration();
        }

        public static void InitializeCharacterConfiguration(CharacterProperties noble, CharacterProperties mentat, CharacterProperties beneGesserit, CharacterProperties fighter)
        {
            CharacterConfiguration.Noble = noble;
            CharacterConfiguration.Mentat = mentat;
            CharacterConfiguration.BeneGesserit = beneGesserit;
            CharacterConfiguration.Fighter = fighter;
        }
    }
}
