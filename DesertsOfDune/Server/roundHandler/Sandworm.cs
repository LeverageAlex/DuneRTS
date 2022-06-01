using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using GameData.graph;
using GameData.network.messages;
using GameData.network.util;
using GameData.network.util.enums;
using GameData.network.util.world;
using GameData.network.util.world.character;
using GameData.network.util.world.mapField;
using GameData.Pathfinder;
using Server;
using Server.Configuration;

namespace GameData.server.roundHandler
{
    /// <summary>
    /// Represents a sandworm
    /// </summary>
    public class Sandworm
    {
        private readonly Map _map;
        private MapField _currentField;

        private readonly int _sandWormSpeed;
        private readonly int _sandWormSpawnDistance;

        private static Sandworm _sandWormSingleton;

        private Character _targetCharacter;

        /// <summary>
        /// Public constructor just used to creat a sandworm to call the Execute method for the first time.
        /// This Constructor should not be used for other usecases.
        /// </summary>
        public Sandworm()
        {

        }

        /// <summary>
        /// spawns a sandworm, if no sandworm already exists (implementing the singleton pattern)
        /// </summary>
        /// <param name="sandWormSpeed">defines the speed the sandworm has</param>
        /// <param name="sandWormSpawnDistance">defines the minimal spawn distance from characters</param>
        /// <param name="map">the map, the sandworm is living on</param>
        /// <returns>a reference to a sandworm</returns>
        public static Sandworm Spawn(int sandWormSpeed, int sandWormSpawnDistance, Map map, List<Character> characters)
        {
            if (_sandWormSingleton == null)
            {
                _sandWormSingleton = new Sandworm(sandWormSpeed, sandWormSpawnDistance, map, characters);


            }
            return _sandWormSingleton;
        }

        /// <summary>
        /// private constructor of the class Sandworm supports singleton pattern
        /// </summary>
        /// <param name="sandWormSpeed">defines the speed the sandworm has</param>
        /// <param name="sandWormSpawnDistance">defines the minimal spawn distance from characters</param>
        /// <param name="map">the map, the sandworm is living on</param>
        private Sandworm(int sandWormSpeed, int sandWormSpawnDistance, Map map, List<Character> characters)
        {
            this._sandWormSpeed = sandWormSpeed;
            this._sandWormSpawnDistance = sandWormSpawnDistance;
            this._map = map;
            this._targetCharacter = ChooseTargetCharacter(characters);
            this._currentField = DetermineField(characters);

            // get the target character
            Party.GetInstance().messageController.DoSpawnSandwormDemand(this._targetCharacter.CharacterId, this._currentField);
        }

        /// <summary>
        /// determines the spawn position of the sandworm
        /// </summary>
        /// <remarks>
        /// Be attentive, because this method uses a while (true)-loop, so it is possible, that it never returns</remarks>
        /// <returns>the spawn position of the SandWorm</returns>
        private MapField DetermineField(List<Character> characters)
        {
            MapField positionOfTargetedCharacter = this._targetCharacter.CurrentMapfield;

            while (true)
            {
                // get random desert field on map
                MapField sandwormField = this._map.GetRandomDesertField();

                if (sandwormField.IsCharacterStayingOnThisField)
                {
                    // field is not free
                    continue;
                }

                if (this._map.GetNeighborFields(positionOfTargetedCharacter).Contains(sandwormField))
                {
                    // field is a neighbor field of the character
                    continue;
                }

                if (sandwormField.DistanceTo(positionOfTargetedCharacter) > this._sandWormSpawnDistance)
                {
                    // field is to far away
                    continue;
                }

                return sandwormField;
            }
        }


        /// <summary>
        /// calculates the path to the targeted character (from current position)
        /// </summary>
        /// <returns></returns>
        public Queue<MapField> CalculatePathToTarget()
        {
            MapField targetField = this._targetCharacter.CurrentMapfield;
            AStarPathfinder pathfinder = new AStarPathfinder();
            SandwormGraph graph = new SandwormGraph(_map);

            return pathfinder.GetShortestPath(_currentField, targetField, graph);
        }

        /// <summary>
        /// This method handles the Sandworm movement.
        /// </summary>
        /// <param name="target">the target character of the SandWorm</param>
        /// <param name="graph">the graph used to move the sandworm</param>
        /// <returns>the mapth of the SandWorm</returns>
        public List<MapField> MoveSandworm(Character target, SandwormGraph graph)
        {
            List<MapField> mapFields = new List<MapField>();
            for (int i = 0; i < _sandWormSpeed; i++)
            {
                MapField mapfield = MoveSandWormByOneField(target, graph);
                mapFields.Add(mapfield);
                if (_currentField.Character != null)
                {
                    _currentField.Character.KilledBySandworm = true;
                    _currentField.Character = null;
                    // TODO: sandworm despawns and sandworm phase ends.
                    Despawn();
                    return mapFields;
                }
                if (mapfield.XCoordinate == target.CurrentMapfield.XCoordinate && mapfield.ZCoordinate == target.CurrentMapfield.ZCoordinate)
                {
                    return mapFields;
                }
            }
            return mapFields;
        }

        /// <summary>
        /// moves the sandworm along a path
        /// </summary>
        /// <param name="path">the path, on which the sandworm moves (but not complete path, but only a part depending on the speed)</param>
        public void MoveSandWorm(Queue<MapField> path)
        {
            // check, whether the targeted character moves on a plateau, so disappear

            if (_targetCharacter.CurrentMapfield.TileType.Equals(TileType.PLATEAU.ToString())){
                Despawn();
            }
            // check, if there is a path for sandworm to move along
            else if (path.Count == 0)
            {
                // do not move, but disappear and appear on a random desert field
                _currentField = this._map.GetRandomDesertField();
            }
            else
            {
                List<MapField> movedPath = new List<MapField>();
                for (int i = 0; i < _sandWormSpeed; i++)
                {
                    MapField nextField = path.Dequeue();
                    movedPath.Add(nextField);
                    bool needToDisappear = MoveSandwormByOneField(nextField);

                    if (needToDisappear)
                    {
                        Despawn();
                        break;
                    }
                }

                Party.GetInstance().messageController.DoMoveSandwormDemand(movedPath);
            }


        }

        /// <summary>
        /// moves the sandworm by one field to next field and update the old and new field.
        /// Furthermore, if there is a character on the field, the sandworm swallow it.
        /// </summary>
        /// <param name="nextField">the field, the sandworm need to move on</param>
        /// <returns>true, if the sandworm moved to a field with a character and need to disappear</returns>
        public bool MoveSandwormByOneField(MapField nextField)
        {
            _currentField = new FlatSand(_currentField.HasSpice, _currentField.isInSandstorm, _currentField.stormEye);
            _currentField.IsApproachable = true;
            _currentField = nextField;
            _currentField.IsApproachable = false;

            if (_currentField.IsCharacterStayingOnThisField)
            {
                _currentField.Character.KilledBySandworm = true;
                _currentField.Character = null;

                return true;
            }

            return false;
        }

        /// <summary>
        /// removes the sandWorm instance, so let the sandworm disappear
        /// </summary>
        public static void Despawn()
        {
            _sandWormSingleton = null;

            Party.GetInstance().messageController.DoDespawnSandwormDemand();
        }

        /// <summary>
        /// chooses a target character from all loud characters
        /// </summary>
        /// <param name="characters">the characters, that are marked as loud</param>
        /// <returns>the targeted character or null, if there is no loud character</returns>
        /// TODO: do not return null
        public Character ChooseTargetCharacter(List<Character> characters)
        {
            List<Character> loudCharacters = (List<Character>)characters.Where(character => character.IsLoud());

            if (loudCharacters.Count == 0)
            {
                return null;
            }

            // select a random loud character
            Random random = new Random();
            int index = random.Next(loudCharacters.Count);
            return loudCharacters[index];
        }
    }
}
