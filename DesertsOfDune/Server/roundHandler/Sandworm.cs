using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.messages;
using GameData.network.util.world;
using GameData.network.util.world.character;
using GameData.network.util.world.mapField;
using Server;
using Server.Configuration;

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


        public void Execute(MapField[,] mapFields, List<Character> loudCharacters)
        {
            //should be called:
            Spawn(PartyConfiguration.GetInstance().sandWormSpeed, PartyConfiguration.GetInstance().sandWormSpawnDistance, mapFields);
            //Party.GetInstance().serverMessageController.DoSpawnSandwormDemand();

            Character target = ChooseTargetCharacter(loudCharacters);

            Graph graph = Graph.DetermineSandWormGraph(mapFields);
            currentField = new MapField(false, false, 0, null);
            this.currentField.XCoordinate = 0;
            this.currentField.ZCoordinate = 0;
            List<MapField> list =  MoveSandworm(target, graph);
            // TODO: call ServerMessageController
           // ServerMessageController.DoMoveSandwormDemand(list);
        }

        /// <summary>
        /// This method spawns a sandworm if no sandworm exists
        /// </summary>
        /// <param name="sandWormSpeed">defines the speed the sandworm has</param>
        /// <param name="sandWormSpawnDistance">defines the minimal spawn distance from characters</param>
        /// <returns></returns>
        public static SandWorm Spawn(int sandWormSpeed, int sandWormSpawnDistance, MapField[,] map)
        {
            if (sandWorm == null)
            {
                sandWorm = new SandWorm(sandWormSpeed, sandWormSpawnDistance, map);
               // Party.GetInstance().serverMessageController.DoSpawnSandwormDemand(0, targetCharacter.CharacterId, sandWorm.currentField);
            }
            return sandWorm;
        }

        /// <summary>
        /// private Constructor of the class Sandworm supports singleton pattern
        /// </summary>
        /// <param name="sandWormSpeed">defines the speed the sandworm has</param>
        /// <param name="sandWormSpawnDistance">defines the minimal spawn distance from characters</param>
        private SandWorm(int sandWormSpeed, int sandWormSpawnDistance, MapField[,] map)
        {
            this.sandWormSpeed = sandWormSpeed;
            this.mapFields = map;
            this.currentField = DetermineField(sandWormSpawnDistance);
        }

        /// <summary>
        /// This method determines the spawn position of the sandworm
        /// </summary>
        /// <param name="sandWormSpawnDistance">the minimum distance the sandworm should be spawned in.</param>
        /// <returns>the spawn position of the SandWorm</returns>
        private static MapField DetermineField(int sandWormSpawnDistance)
        {
            // todo determine sand field that is in specified distance to all characters
            for(int i = 0; i < sandWormSpawnDistance; i++)
            {
                for(int j = 0; j < sandWormSpawnDistance; j++)
                {

                }
            }
            return null;
        }

        /// <summary>
        /// This method handles the Sandworm movement.
        /// </summary>
        /// <returns>true, if sandworm was moved</returns>
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
        /// <param name="target"></param>
        /// <param name="graph"></param>
        /// <returns></returns>
        public MapField MoveSandWormByOneField(Character target, Graph graph)
        {
            int startVertex = Graph.ConvertArrayIndexToVertex(this.currentField.XCoordinate, this.currentField.ZCoordinate, mapFields);
            int targetVertex = Graph.ConvertArrayIndexToVertex(target.CurrentMapfield.XCoordinate, target.CurrentMapfield.ZCoordinate, mapFields);
            Console.WriteLine("startVertex: " + startVertex);
            Console.WriteLine("targetVertex: " + targetVertex);
            int[] parent = DijkstrasAlgorithm.Dijkstra(graph.Node, startVertex);
            int nextVertex = DijkstrasAlgorithm.GetFirstStep(parent, targetVertex);
            if (nextVertex == startVertex)
            {
                nextVertex = targetVertex;
            } 
            Console.WriteLine("nextVertex: " + nextVertex);
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
            //TODO: call ServerMessageController.DoDespawnSandwormDemand()
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
