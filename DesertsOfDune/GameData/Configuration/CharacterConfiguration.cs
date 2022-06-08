using System;
using GameData.network.util.world;

namespace GameData.Configuration
{
    public class CharacterConfiguration
    {
        public static CharacterProperties Noble { get; set; }
        public static CharacterProperties Mentat { get; set; }
        public static CharacterProperties BeneGesserit { get; set; }
        public static CharacterProperties Fighter { get; set; }


        protected CharacterConfiguration()
        {
        }
    }
}
