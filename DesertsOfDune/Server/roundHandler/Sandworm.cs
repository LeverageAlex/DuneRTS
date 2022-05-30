using System;
using System.Collections.Generic;
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
    /// Handles the sandworm in usual "mode"
    /// </summary>
    public class UsualSandWormPhase : SandwormPhase
    {
        private MapField currentField;
        private int sandWormSpeed;
        private static UsualSandWormPhase sandWorm;
        private MapField[,] mapFields;
        private Character targetCharacter;

        /// <summary>
        /// Public constructor just used to creat a sandworm to call the Execute method for the first time.
        /// This Constructor should not be used for other usecases.
        /// </summary>
        public UsualSandWormPhase()
        {

        }

        public UsualSandWormPhase Execute(MapField[,] mapFields, List<Character> characters)
        {
            if (sandWorm == null && CheckLoudness(characters))
            {
                UsualSandWormPhase sandWorm = Spawn(PartyConfiguration.GetInstance().sandWormSpeed, PartyConfiguration.GetInstance().sandWormSpawnDistance, mapFields, characters);
                sandWorm.targetCharacter = ChooseTargetCharacter(characters);
                return sandWorm;
                // end round
            } else if (sandWorm != null)
            {
                Graph graph = Graph.DetermineSandWormGraph(mapFields);
                List<MapField> list =  MoveSandworm(targetCharacter, graph);
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
        /// This method spawns a sandworm if no sandworm exists
        /// </summary>
        /// <param name="sandWormSpeed">defines the speed the sandworm has</param>
        /// <param name="sandWormSpawnDistance">defines the minimal spawn distance from characters</param>
        /// <returns></returns>
        public static UsualSandWormPhase Spawn(int sandWormSpeed, int sandWormSpawnDistance, MapField[,] map, List<Character> characters)
        {
            if (sandWorm == null)
            {
                sandWorm = new UsualSandWormPhase(sandWormSpeed, sandWormSpawnDistance, map, characters);
               // Party.GetInstance().serverMessageController.DoSpawnSandwormDemand(0, targetCharacter.CharacterId, sandWorm.currentField);
            }
            return sandWorm;
        }

        /// <summary>
        /// private Constructor of the class Sandworm supports singleton pattern
        /// </summary>
        /// <param name="sandWormSpeed">defines the speed the sandworm has</param>
        /// <param name="sandWormSpawnDistance">defines the minimal spawn distance from characters</param>
        private UsualSandWormPhase(int sandWormSpeed, int sandWormSpawnDistance, MapField[,] map, List<Character> characters)
        {
            // set sandwormspeed to constant value for testing
            this.sandWormSpeed = 2;
            //this.sandWormSpeed = sandWormSpeed;
            this.mapFields = map;
            this.currentField = DetermineField(sandWormSpawnDistance, map, characters);
        }

        /// <summary>
        /// This method determines the spawn position of the sandworm
        /// </summary>
        /// <param name="sandWormSpawnDistance">the minimum distance the sandworm should be spawned in.</param>
        /// <returns>the spawn position of the SandWorm</returns>
        private static MapField DetermineField(int sandWormSpawnDistance, MapField[,] map, List<Character> characters)
        {
            // next line just for testing as long es party config does not set this parameter right
            sandWormSpawnDistance = 1;
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
        /// This method determines the minimalDistance for the Sandworm to all characters on the map from a specific field
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
            for(int i = 0; i < sandWormSpeed; i++)
            {
                MapField mapfield = MoveSandWormByOneField(target, graph);
                mapFields.Add(mapfield);
                if (currentField.Character != null)
                {
                    currentField.Character.KilledBySandworm = true;
                    currentField.Character = null;
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
            int startVertex = Graph.ConvertArrayIndexToVertex(this.currentField.XCoordinate, this.currentField.ZCoordinate, mapFields);
            int targetVertex = Graph.ConvertArrayIndexToVertex(target.CurrentMapfield.XCoordinate, target.CurrentMapfield.ZCoordinate, mapFields);
            int[] parent = DijkstrasAlgorithm.Dijkstra(graph.Node, startVertex);
            int nextVertex = DijkstrasAlgorithm.GetFirstStep(parent, targetVertex);
            if (nextVertex == startVertex)
            {
                nextVertex = targetVertex;
            } 
            int indexX = Graph.ConvertVertexToXArrayIndex(nextVertex, mapFields);
            int indexZ = Graph.ConvertVertexToZArrayIndex(nextVertex, mapFields);
            this.currentField = mapFields[indexX, indexZ];
            this.currentField.XCoordinate = indexX;
            this.currentField.ZCoordinate = indexZ;
            return mapFields[indexX, indexZ];
        }

        /// <summary>
        /// Removes the sandWorm instance from the map.
        /// </summary>
        public static void Despawn()
        {
            //TODO: call Party.GetInstance().serverMessageController.DoDespawnSandwormDemand();
            sandWorm = null;
            // implement end of Sandworm Round
        }

        /// <summary>
        /// This method checks if there is a loud Character on the Map
        /// </summary>
        /// <returns>true, if a loud character exists.</returns>
        public bool CheckLoudness(List<Character> characters)
        {
            foreach(Character character in characters)
            {
                if(character.IsLoud())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// This method chooses the target character.
        /// </summary>
        /// <param name="characters">the characters that are marked as loud</param>
        /// <returns>the targeted character</returns>
        public Character ChooseTargetCharacter(List<Character> characters)
        {
            List<Character> loudCharacters = new List<Character>();
            foreach(Character character in characters)
            {
                if (character.IsLoud())
                {
                    loudCharacters.Add(character);
                }
            }
            if(loudCharacters.Count == 0)
            {
                return null;
            }
            if (loudCharacters.Count == 1)
            {
                return loudCharacters[0];
            }
            Random random = new Random();
            int index = random.Next(0, loudCharacters.Count);
            return loudCharacters[index];
        }
    }
}
