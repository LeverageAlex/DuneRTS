using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using GameData.network.messages;
using GameData.network.util;
using GameData.network.util.world;
using GameData.network.util.world.character;
using GameData.network.util.world.mapField;
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

        public Sandworm Execute(MapField[,] mapFields, List<Character> characters)
        {
            if (_sandWormSingleton == null && CheckLoudness(characters))
            {
                Sandworm sandWorm = Spawn(PartyConfiguration.GetInstance().sandWormSpeed, PartyConfiguration.GetInstance().sandWormSpawnDistance, mapFields, characters);
                sandWorm._targetCharacter = ChooseTargetCharacter(characters);
                return sandWorm;
                // end round
            } else if (_sandWormSingleton != null)
            {
                Graph graph = Graph.DetermineSandWormGraph(mapFields);
                List<MapField> list =  MoveSandworm(_targetCharacter, graph);
                // TODO: call ServerMessageController
                // Party.GetInstance().serverMessageController.DoMoveSandwormDemand(list);
                // end round
                return null;
            } else
            {
                return null;
            }
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
            this._currentField = DetermineField(characters);
            this._targetCharacter = ChooseTargetCharacter(characters);

            // get the target character
            Party.GetInstance().messageController.DoSpawnSandwormDemand(this._targetCharacter.CharacterId, this._currentField);
        }

        /// <summary>
        /// determines the spawn position of the sandworm
        /// </summary>
        /// <returns>the spawn position of the SandWorm</returns>
        private static MapField DetermineField(List<Character> characters)
        { 
           // todo determine sand field that is in specified distance to all characters
            while(true)
            {
                Random random = new Random();
                int indexX = random.Next(0,map.GetLength(0));
                int indexZ = random.Next(0, map.GetLength(1));
                if (map[indexX, indexZ].TileType == "DUNE" || map[indexX, indexZ].TileType == "FLAT" && map[indexX, indexZ].Character == null)
                {
                    if (DetermineMinDistanceToCharacters(characters, map[indexX, indexZ], map) >= sandWormSpawnDistance)
                    {
                        return map[indexX, indexZ];
                    }
                }
            }
        }

        /// <summary>
        /// determines the minimal distance for the Sandworm to all characters on the map from a specific field
        /// </summary>
        /// <param name="characters">all characters on the map</param>
        /// <param name="mapField">the current mapfield used to determine the minimalD distance from</param>
        /// <param name="map">the whole map of the game</param>
        /// <returns></returns>
        private static int DetermineMinDistanceToCharacters(List<Character> characters, MapField mapField, MapField[,] map)
        {
            int minDistance = int.MaxValue;
            Graph graph = Graph.DetermineSandWormGraph(map);
            int startVertex = Graph.ConvertArrayIndexToVertex(mapField.XCoordinate, mapField.ZCoordinate, map);
            int[] distances = DijkstrasAlgorithm.DijkstraDistances(graph.Node, startVertex);
            foreach (Character character in characters)
            {
                int targetVertex = Graph.ConvertArrayIndexToVertex(character.CurrentMapfield.XCoordinate, character.CurrentMapfield.ZCoordinate, map);
                if (distances[targetVertex] < minDistance)
                {
                    minDistance = distances[targetVertex];
                }
            }
            return minDistance;
        }

        /// <summary>
        /// This method handles the Sandworm movement.
        /// </summary>
        /// <param name="target">the target character of the SandWorm</param>
        /// <param name="graph">the graph used to move the sandworm</param>
        /// <returns>the mapth of the SandWorm</returns>
        public List<MapField> MoveSandworm(Character target, Graph graph)
        {
            List<MapField> mapFields = new List<MapField>();
            for(int i = 0; i < _sandWormSpeed; i++)
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
        ///  This method moves the sandworm one field towards his target
        /// </summary>
        /// <param name="target">the targeted character</param>
        /// <param name="graph">the graph used to determine the movement of the sandworm</param>
        /// <returns>the resulting position of the Sandworm</returns>
        public MapField MoveSandWormByOneField(Character target, Graph graph)
        {
            /*
             * todo: Falls der Sandwurm keine nur über Wüstenfelder führende Strecke hat, um zu seiner Zielperson zu gelangen,
                verschwindet er und taucht direkt danach auf einem zufällig gewählten Wüstenfeld wieder auf 
                Seine Rundenphase ist damit beendet
             */
            int startVertex = Graph.ConvertArrayIndexToVertex(this._currentField.XCoordinate, this._currentField.ZCoordinate, mapFields);
            int targetVertex = Graph.ConvertArrayIndexToVertex(target.CurrentMapfield.XCoordinate, target.CurrentMapfield.ZCoordinate, mapFields);
            int[] parent = DijkstrasAlgorithm.Dijkstra(graph.Node, startVertex);
            int nextVertex = DijkstrasAlgorithm.GetFirstStep(parent, targetVertex);
            if (nextVertex == startVertex)
            {
                nextVertex = targetVertex;
            } 
            int indexX = Graph.ConvertVertexToXArrayIndex(nextVertex, mapFields);
            int indexZ = Graph.ConvertVertexToZArrayIndex(nextVertex, mapFields);
            this._currentField = mapFields[indexX, indexZ];
            this._currentField.XCoordinate = indexX;
            this._currentField.ZCoordinate = indexZ;
            return mapFields[indexX, indexZ];
        }

        /// <summary>
        /// removes the sandWorm instance from the map.
        /// </summary>
        public static void Despawn()
        {
            //TODO: call Party.GetInstance().serverMessageController.DoDespawnSandwormDemand();
            _sandWormSingleton = null;
            // implement end of Sandworm Round
        }

        /// <summary>
        /// checks, if there is a loud character on the map
        /// </summary>
        /// <returns>true, if a loud character exists</returns>
        public bool CheckLoudness(List<Character> characters)
        {
            return characters.Any(character => character.IsLoud());
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

            if(loudCharacters.Count == 0)
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
