using System;
using System.Collections.Generic;
using GameData.network.messages;
using GameData.network.util.enums;
using GameData.network.util.world;
using GameData.network.util.world.mapField;
using Server;
using Server.roundHandler;

namespace GameData.server.roundHandler
{

    /// <summary>
    /// Handles the spice blow and offer the check, whether is should be executed
    /// </summary>
    public class SpiceBlow : IGamePhase
    {

        private readonly Map _map;

        /// <summary>
        /// creates a new spice blow object
        /// </summary>
        /// <param name="map">the map of the game</param>
        public SpiceBlow(Map map)
        {
            this._map = map;
        }

        /// <summary>
        /// changes the selected random field and it's neightbors (of type desert field) to to a new random desert field
        /// </summary>
        /// <param name="randomField">the random field</param>
        private void ChangeFieldAndNeighborsRandomly(MapField randomField)
        {
            bool wasMapChanged = false;

            Random random = new Random();
            List<MapField> neighbors = this._map.GetNeighborFields(randomField);

            foreach (MapField neighbor in neighbors)
            {
                if (this._map.IsMapFieldADesertField(neighbor))
                {
                    if (random.NextDouble() < 0.5)
                    {
                        MapField newDune = new Dune(randomField.hasSpice, randomField.isInSandstorm, randomField.stormEye);
                        newDune.PlaceCharacter(randomField.Character);

                        this._map.SetMapFieldAtPosition(newDune, randomField.XCoordinate, randomField.ZCoordinate);
                        wasMapChanged = true;
                    }
                    else
                    {
                        MapField newFlatSand = new FlatSand(randomField.hasSpice, randomField.isInSandstorm, randomField.stormEye);
                        newFlatSand.PlaceCharacter(randomField.Character);

                        this._map.SetMapFieldAtPosition(newFlatSand, randomField.XCoordinate, randomField.ZCoordinate);
                        wasMapChanged = true;
                    }
                }
            }

            // check, whether the map was changed while the spice blow, if so send a map change message
            if (wasMapChanged)
            {
                Party.GetInstance().messageController.DoSendMapChangeDemand(MapChangeReasons.ROUND_PHASE);
            }
        }

        /// <summary>
        /// places spice by chance on the specified fields and its neighbours
        /// </summary>
        /// <param name="randomField">the random field, on which the spice blow happens</param>
        public void PlaceSpiceOnFields(MapField randomField)
        {
            //TODO determine count neigthbors to break loop if necessary
            Random random = new Random();

            int amountOfSpiceToSpread = random.Next(3, 7);

            // spread the spice
            PlaceSpiceOnField(randomField, amountOfSpiceToSpread);
        }

        /// <summary>
        /// recursive places spice on a random, approachable field until all spice is placed
        /// </summary>
        /// <param name="field">the field, the spice should be placed on</param>
        /// <param name="restAmountOfSpiceToPlace">the amount of spice, that is left for placement</param>
        private bool PlaceSpiceOnField(MapField field, int restAmountOfSpiceToPlace)
        {
            // check, whether all spice was placed:
            if (restAmountOfSpiceToPlace == 0)
            {
                return true;
            }
            else
            {
                // get a random, approachble neighbor field
                MapField nextField = this._map.GetRandomApproachableNeighborField(field);

                if (field == null)
                {
                    // return false, because the spice blow couldn't be finished
                    return false;
                }
                else
                {
                    // if there is no spice on this map field, place it and call the method for a approachable neighbor field
                    if (!field.hasSpice)
                    {
                        field.hasSpice = true;
                        return PlaceSpiceOnField(nextField, restAmountOfSpiceToPlace - 1);
                    }
                    else
                    {
                        return PlaceSpiceOnField(nextField, restAmountOfSpiceToPlace);
                    }
                }

            }


        }

        /// <summary>
        /// determines, whether the spice blow is necessary
        /// </summary>
        /// <returns>true, if the spice blow should be done</returns>
        public bool IsSpiceBlowNecessary(int spiceMinimum, int currentSpice)
        {
            return spiceMinimum > currentSpice;
        }

        /// <summary>
        /// executes a spice blow
        /// </summary>
        public void Execute()
        {
            // get a random desert field on the map
            MapField randomField = this._map.GetRandomDesertField();

            // changes the terrain
            ChangeFieldAndNeighborsRandomly(randomField);

            // place the spice
            PlaceSpiceOnFields(randomField);
        }
    }
}
