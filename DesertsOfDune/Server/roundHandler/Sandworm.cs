using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.messages;
using GameData.network.util.world;
using GameData.network.util.world.character;
using GameData.network.util.world.mapField;

namespace GameData.server.roundHandler
{
    /// <summary>
    /// This Class is responsible for handling Sandworm in normal mode.
    /// </summary>
    public class SandWorm : SandwormPhase
    {
        private bool sandwormExists;
        private Character targetCharacter;
        private MapField[,] fields;
        private MapField currentField;
        private int sandWormSpeed;
        private static SandWorm sandWorm;
        private MapField[,] mapFields;


        public void Execute()
        {
            mapFields = new MapField[5, 5];
            mapFields[0, 0] = new FlatSand(false, false, null);
            for(int i = 0; i < 5; i++)
            {
                for(int j = 0; j < 5; j++)
                {
                    mapFields[i, j] = new FlatSand(false, false, null);
                }
            }

            Graph graph = Graph.DetermineSandWormGraph(mapFields);
            Nobel targetCharacter = new Nobel(1, 2, 3, 4, 5, 6, 7, 8, 9, 7, false, false);
            MoveSandWormByOneField(targetCharacter, graph);
        }

        /// <summary>
        /// This method spawns a sandworm if no sandworm exists
        /// </summary>
        /// <param name="sandWormSpeed">defines the speed the sandworm has</param>
        /// <param name="sandWormSpawnDistance">defines the minimal spawn distance from characters</param>
        /// <returns></returns>
        public static SandWorm Spawn(int sandWormSpeed, int sandWormSpawnDistance)
        {
            if (sandWorm == null)
            {
                sandWorm = new SandWorm(sandWormSpeed, sandWormSpawnDistance);
                return sandWorm;
            }
            return sandWorm;
        }

        /// <summary>
        /// private Constructor of the class Sandworm supports singleton pattern
        /// </summary>
        /// <param name="sandWormSpeed">defines the speed the sandworm has</param>
        /// <param name="sandWormSpawnDistance">defines the minimal spawn distance from characters</param>
        private SandWorm(int sandWormSpeed, int sandWormSpawnDistance)
        {
            this.sandWormSpeed = sandWormSpeed;
            this.currentField = DetermindField(sandWormSpawnDistance);
        }

        /// <summary>
        /// This method determines the spawn position of the sandworm
        /// </summary>
        /// <param name="sandWormSpawnDistance">the minimum distance the sandworm should be spawned in.</param>
        /// <returns>the spawn position of the SandWorm</returns>
        private MapField DetermindField(int sandWormSpawnDistance)
        {
            // todo determine sand field that is in specified distance to all characters
            return null;
        }

        /// <summary>
        /// This method handles the Sandworm movement.
        /// </summary>
        /// <returns>true, if sandworm was moved</returns>
        public void MoveSandworm(Character target, Graph graph)
        {
            // TODO: implement pathfinding
            for(int i = 0; i < sandWormSpeed; i++)
            {
                MoveSandWormByOneField(target, graph);
                if (currentField.Character != null)
                {
                    currentField.Character.KilledBySandworm = true;
                    currentField.Character = null;
                    Disapear();
                }
            }
        }

        /// <summary>
        /// This method moves the sandworm one field towards his target
        /// </summary>
        public void MoveSandWormByOneField(Character target, Graph graph)
        {
            int xCoordinateTarget = 3;//target.CurrentMapfield.XCoordinate;
            int zCoordinateTarget = 3; // target.CurrentMapfield.ZCoordinate;
            int xCoordinateCurrent = 1;// currentField.XCoordinate;
            int zCoordinateCurrent = 2; // currentField.ZCoordinate;
            int startVertex = Graph.ConvertArrayIndexToVertex(xCoordinateCurrent, zCoordinateCurrent, mapFields);
            int targetVertex = Graph.ConvertArrayIndexToVertex(xCoordinateTarget, zCoordinateTarget, mapFields);
            int[] parent = DijkstrasAlgorithm.Dijkstra(graph.Node, startVertex);
            int nextVertex = DijkstrasAlgorithm.GetFirstStep(parent, targetVertex);
            Console.WriteLine("nextVertex: " + nextVertex);
            int indexX = Graph.ConvertVertexToXArrayIndex(5, mapFields);
            int indexZ = Graph.ConvertVertexToZArrayIndex(5, mapFields);
            currentField = mapFields[indexX, indexZ];

        }

        /// <summary>
        /// Removes the sandWorm instance from the map.
        /// </summary>
        public static void Disapear()
        {
            sandWorm = null;
            // implement end of Sandworm Round
        }


        /// <summary>
        /// This method checks the path of the sandworm.
        /// </summary>
        /// <returns>true, if</returns>
        public bool DetermineSandwormPath()
        {
            // TODO: implement logic
            return false;
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
        /// <param name="loudCharacters">the characters that are marked as loud</param>
        /// <returns>the targeted character</returns>
        public Character ChooseTargetCharacter(List<Character> loudCharacters)
        {
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
