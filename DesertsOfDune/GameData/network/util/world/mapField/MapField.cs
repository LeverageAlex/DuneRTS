using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using GameData.network.util.enums;
using Newtonsoft.Json;

namespace GameData.network.util.world
{
    /// <summary>
    /// This class resambles one field on the playingfield.
    /// </summary>
    public class MapField
    {
        [JsonProperty]
        private string tileType;
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        private int clientID;
        [JsonProperty]
        private bool hasSpice;
        [JsonIgnore]
        public bool HasSpice
        {
            get { return hasSpice; }
            set { hasSpice = value; }
        }
        [JsonProperty]
        public bool isInSandstorm { get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Position stormEye { get; }
        private Elevation elevation;
        [JsonIgnore]
        public Elevation Elevation
        {
            get { return elevation; }
            set { elevation = value; }
        }
        private bool isAprochable;

        [JsonIgnore]
        public bool IsAproachable
        {
            set { isAprochable = value; }
            get { return isAprochable; }
        }
        private int xCoordinate;
        [JsonIgnore]
        public int XCoordinate { get { return xCoordinate; } set { xCoordinate = value; } }

        private int zCoordinate;
        [JsonIgnore]
        public int ZCoordinate { get { return zCoordinate; } set { zCoordinate = value; } }

        protected bool IsCityField;
        [JsonIgnore]
        public string TileType { get { return tileType; } set { tileType = value; } }

        private Character character;
        [JsonIgnore]
        public Character Character { get { return character; } set { character = value; } }

        /// <summary>
        /// Constructor of the class MapField for a City MapField
        /// </summary>
        /// <param name="tt">the type of field</param>
        /// <param name="hasSpice">true, if it countains spice</param>
        /// <param name="isInSandstorm">true, if there is a sandstorm on the field.</param>
        /// <param name="clientID">the id of the client (only to be set for city tiles)</param>
        /// <param name="stormEye">the center position of the storm can be null</param>
        public MapField(bool hasSpice, bool isInSandstorm, int clientID, Position stormEye)
        {
            this.tileType = Enum.GetName(typeof(TileType),enums.TileType.CITY);
            this.hasSpice = hasSpice;
            this.isInSandstorm = isInSandstorm;
            this.clientID = clientID;
            this.stormEye = stormEye;
        }

        /// <summary>
        /// Constructof of the Class MapField for Fields that are not the city
        /// </summary>
        /// <param name="tt">the type of the MapField</param>
        /// <param name="hasSpice">true, if the MapField has spice on it</param>
        /// <param name="isInSandstorm">true, if there is a sandstorm on the MapField</param>
        /// <param name="stormEye">the Position of the Sandstorm can also be null</param>
        public MapField(TileType tt, Elevation heightLevel, bool hasSpice, bool isInSandstorm, Position stormEye)
        {
            this.tileType = Enum.GetName(typeof(TileType), tt);
            this.elevation = heightLevel;
            this.hasSpice = hasSpice;
            this.isInSandstorm = isInSandstorm;
            this.stormEye = stormEye;
        }

        public MapField(string tileType, int x, int y)
        {
            this.tileType = tileType;
            this.XCoordinate = x;
            this.ZCoordinate = y;
        }

        /// <summary>
        /// This method changes the elevation of the map field
        /// </summary>
        /// <param name="elevation">the elevation to set the field to</param>
        public void ChangeElevation(Elevation elevation)
        {
            this.elevation = elevation;
        }
    }
}
