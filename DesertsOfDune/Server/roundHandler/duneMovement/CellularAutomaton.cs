using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine;
using GameData.network.messages;
using GameData.network.util.enums;
using GameData.network.util.world;
using GameData.network.util.world.mapField;
using Serilog;
using Server.Configuration;

namespace Server.roundHandler.duneMovementHandler
{
    /// <summary>
    /// Represents a cellular automaton based on the principle of Conway's Game of Life
    /// </summary>
    public class CellularAutomaton
    {
        /// <summary>
        /// list of numbers of neighbors, which are necessary for a cell to spawn
        /// </summary>
        /// <example>
        /// born = {2, 3} means, that a cell need 2 or 3 neighbor cells, that are alive, to spawn
        /// </example>
        private List<int> born;


        /// <summary>
        /// list of numbers of neighbors, which are necessary for a cell to survive
        /// </summary>
        /// <example>
        /// born = {2, 3} means, that a cell need 2 or 3 neighbor cells, that are alive, to survive
        /// </example>
        private List<int> survive;

        private Map map;

        /// <summary>
        /// create a new cellular automaton with a given transition rule
        /// </summary>
        /// <param name="map">the map, the cellular automaton work on and manipulate</param>
        public CellularAutomaton(Map map)
        {
            this.map = map;
            ParseTransitionRule(PartyConfiguration.GetInstance().cellularAutomaton);
        }

        /// <summary>
        /// parses a string, representing the transition rule as described in requirements document, and extract the list of numbers of neighbor cells
        /// needed for spawning or surving
        /// </summary>
        /// <param name="transitionRule"></param>
        private void ParseTransitionRule(string transitionRule)
        {
            // ignore B and S, so only use substring starting at index = 1
            string bornRule = transitionRule.Split("/")[0].Substring(1);
            string surviveRule = transitionRule.Split("/")[1].Substring(1);

            born = Array.ConvertAll(bornRule.ToCharArray(), c => (int)Char.GetNumericValue(c)).ToList();
            survive = Array.ConvertAll(surviveRule.ToCharArray(), c => (int)Char.GetNumericValue(c)).ToList();
        }

        /// <summary>
        /// counts the number of neighbor cells of a given cell, that are alive, so of type "DUNE", "CITY", "MOUNTAIN"
        /// </summary>
        /// <param name="field">the cell, whose alive neighbors should be counted</param>
        /// <returns>the number of alive neighbor cells</returns>
        private int GetNumberOfAliveNeighborCells(MapField field)
        {
            int numberOfAliveNeighborCells = 0;

            List<MapField> neighbors = map.GetNeighborFields(field);
            foreach (MapField neighbor in neighbors)
            {
                if (CellIsAlive(neighbor))
                {
                    numberOfAliveNeighborCells++;
                }
            }

            return numberOfAliveNeighborCells;

        }

        /// <summary>
        /// determines, whether a given cell (= field on the map) is "alive" or "dead".
        /// A cell is "alive", if it has the type "DUNE", "CITY", "MOUNTAIN".
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private bool CellIsAlive(MapField field)
        {
            return field.tileType.Equals(TileType.DUNE.ToString()) || field.tileType.Equals(TileType.MOUNTAINS.ToString()) || field.tileType.Equals(TileType.CITY.ToString());
        }

        /// <summary>
        /// calculate the next generation of cells of the map, so bring the cellular automaton into the next state (based on the transition rule)
        /// </summary>
        public void NextIteration()
        {
            bool wasMapChanged = false;

            for (int x = 0; x < map.MAP_WIDTH; x++)
            {
                for (int y = 0; y < map.MAP_HEIGHT; y++)
                {
                    MapField cell = map.GetMapFieldAtPosition(x, y);

                    // checks, if the cell is a desert field, which can changed and is not constant.
                    if (this.map.IsMapFieldADesertField(cell))
                    {
                        // get the number of alive cells
                        int numberOfAliveNeighborCells = GetNumberOfAliveNeighborCells(cell);

                        if (born.Contains(numberOfAliveNeighborCells))
                        {
                            // cell will be born, so will be a dune
                            MapField newDune = new Dune(cell.hasSpice, cell.isInSandstorm, cell.stormEye);
                            if (cell.Character != null)
                            {

                                newDune.PlaceCharacter(cell.Character);
                            }

                            map.SetMapFieldAtPosition(newDune, x, y);
                            wasMapChanged = true;
                            continue;
                        }

                        if (survive.Contains(numberOfAliveNeighborCells))
                        {
                            // cell survives, so nothing changes
                            continue;
                        }
                        
                        // cell will die, so will be a flat sand
                        MapField newFlatSand = new FlatSand(cell.hasSpice, cell.isInSandstorm, cell.stormEye);
                        if (cell.Character != null)
                        {
                            newFlatSand.PlaceCharacter(cell.Character);
                        }

                        map.SetMapFieldAtPosition(newFlatSand, x, y);
                        wasMapChanged = true;
                    }
                }
            }

            // if the map was changed in this iteration, send the map change message
            if (wasMapChanged)
            {
                Party.GetInstance().messageController.DoSendMapChangeDemand(MapChangeReasons.DUNEWALKING);
            }
        }

    }
}
