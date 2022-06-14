using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using GameData.network.util.world.mapField;
using GameData.network.messages;
using WebSocketSharp;
using GameData.network.util.world.character;
using GameData.network.util.enums;

namespace GameData.network.util.world
{
    /// <summary>
    /// Base class for the Great Houses
    /// </summary>
    public class GreatHouse
    {
        [JsonProperty(Order = 1)]
        public string houseName { get; }
        [JsonProperty (Order = 2)]
        private string houseColor;
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, Order = 3)]
        private bool illegalAtomicUsage;
        [JsonProperty (Order = 4)]
        public HouseCharacter[] houseCharacters { get; set; }
        [JsonIgnore]
        public City City { get; }
        [JsonIgnore]
        public List<Character> Characters { get; }
        [JsonIgnore]
        public int unusedAtomicBombs { get; set; }

        public static readonly int AMOUNT_OF_CHARACTERS_PER_GREAT_HOUSE = 6;

        /// <summary>
        /// Constructor of the Class GreatHouse
        /// </summary>
        /// <param name="houseName">the name of the Greathouse</param>
        /// <param name="houseColor">the color of the house</param>
        /// <param name="houseCharacters">the characters of the house</param>

        [JsonConstructor]
        public GreatHouse(string houseName, string houseColor, HouseCharacter[] houseCharacters)
        {
            this.houseName = houseName;
            this.houseColor = houseColor;
            this.illegalAtomicUsage = false;
            this.houseCharacters = houseCharacters;
            this.Characters = GetCharactersForHouse();
            this.unusedAtomicBombs = 3;
        }

        /// <summary>
        /// This constructor is used for testing perpeces only in order to not use the method getcharactersforhouse.
        /// </summary>
        /// <param name="houseName">the name of the Greathouse</param>
        /// <param name="houseColor">the color of the house</param>
        /// <param name="houseCharacters">the characters of the house</param>
        /// <param name="characters">the characters of the House</param>
        protected GreatHouse(string houseName, string houseColor, HouseCharacter[] houseCharacters, List<Character> characters)
        {
            this.houseName = houseName;
            this.houseColor = houseColor;
            this.illegalAtomicUsage = false;
            this.houseCharacters = houseCharacters;
            this.Characters = characters;
            this.unusedAtomicBombs = 3;
        }

        /// <summary>
        /// Getter for field illegalAtomicUsage
        /// </summary>
        /// <returns>true, if house used illegalAtomicUsage</returns>
        public bool GetIllegalAtomicUsage()
        {
            return illegalAtomicUsage;
        }

        /// <summary>
        /// creates a list of the character objects based on the house characters list
        /// </summary>
        /// <remarks>
        /// The house characters list only contains the types and names of the characters and not further properties,
        /// so there should be created another list of characters (with complete properties and behaviour)
        /// </remarks>
        /// <returns>a list of characters with complete, initial properties</returns>
        private List<Character> GetCharactersForHouse()
        {
            List<Character> characters = new List<Character>();

            foreach (HouseCharacter houseCharacter in this.houseCharacters)
            {
                Character newCharacter = null;

                switch ((CharacterType) Enum.Parse(typeof(CharacterType), houseCharacter.characterClass)) {
                    case CharacterType.NOBLE:
                        newCharacter = new Noble();
                        break;
                    case CharacterType.BENEGESSERIT:
                        newCharacter = new BeneGesserit();
                        break;
                    case CharacterType.MENTAT:
                        newCharacter = new Mentat();
                        break;
                    case CharacterType.FIGHTER:
                        newCharacter = new Fighter();
                        break;
                    default:
                        // TODO: print error or throw exception, if type of character is not valid
                        break;
                }

                characters.Add(newCharacter);
            }

            return characters;
        }

        public List<Character> GetCharactersAlive()
        {
            var charactersAlive = new List<Character>();
            foreach (var character in this.Characters)
            {
                if (!character.IsDead())
                {
                    charactersAlive.Add(character);
                }
            }
            return charactersAlive;
        }
    }
}
