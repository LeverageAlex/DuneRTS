using System;
using System.Collections.Generic;
using System.Text;
using GameData.network.messages;
using GameData.network.util.enums;
using GameData.network.util.world;
using GameData.network.util.world.mapField;
using Server;
using Server.roundHandler;

namespace GameData.server.roundHandler
{
    /// <summary>
    /// Represents the game phase, which handles the sandstorm
    /// </summary>
    public class SandstormPhase : IGamePhase
    {
        /// <summary>
        /// the central field of the storm
        /// </summary>
        public MapField EyeOfStorm { get; set; }

        private Map map;

        /// <summary>
        /// create a new sandworm phase object and set the eye of the storm to a random start field
        /// </summary>
        /// <param name="map"></param>
        public SandstormPhase(Map map)
        {
            this.map = map;
            this.EyeOfStorm = GetRandomStartField();

            ChangeStatusOfMapFields();
        }

        /// <summary>
        /// moves the eye of the storm to a random neighbor field
        /// </summary>
        private void MoveStormToRandomNeighborField()
        {
            List<MapField> neighbors = this.map.GetNeighborFields(EyeOfStorm);

            // chose a random neighbor
            Random random = new Random();
            int neighborIndex = random.Next(neighbors.Count);

            EyeOfStorm = neighbors[neighborIndex];
            this.map.PositionOfEyeOfStorm = new Position(EyeOfStorm.XCoordinate, EyeOfStorm.ZCoordinate);
        }

        /// <summary>
        /// updates the "isInSandstorm"-Status of every map field
        /// </summary>
        private void ChangeStatusOfMapFields()
        {
            List<MapField> neighbors = this.map.GetNeighborFields(EyeOfStorm);

            for (int x = 0; x < this.map.MAP_WIDTH; x++)
            {
                for (int y = 0; y < this.map.MAP_HEIGHT; y++)
                {
                    MapField field = this.map.GetMapFieldAtPosition(x, y);
                    if (field.Equals(EyeOfStorm) || neighbors.Contains(field))
                    {
                        field.isInSandstorm = true;
                    }
                    else
                    {
                        field.isInSandstorm = false;
                    }
                }
            }
        }

        /// <summary>
        /// select a random field on the map as the start field for the eye of the sandstorm
        /// </summary>
        /// <returns></returns>
        private MapField GetRandomStartField()
        {
            Random random = new Random();
            int randomX = random.Next(this.map.MAP_WIDTH);
            int randomY = random.Next(this.map.MAP_HEIGHT);

            this.map.PositionOfEyeOfStorm = new Position(this.map.GetMapFieldAtPosition(randomX, randomY).XCoordinate, this.map.GetMapFieldAtPosition(randomX, randomY).ZCoordinate);

            return this.map.GetMapFieldAtPosition(randomX, randomY);
        }

        /// <summary>
        /// change the desert fields ("FLAT_SAND" and "DUNE") in the storm (all neighbor fields of the eye of storm and the eye itself)
        /// randomly to low or high elevation ("FLAT_SAND" and "DUNE")
        /// </summary>
        private void RandomlyChangeDesertFieldsInStorm()
        {
            bool wasMapChanged = false;

            List<MapField> mapFieldsInStorm = this.map.GetNeighborFields(EyeOfStorm);

            // change the evelation of the eye of the storm, if it is a desert field
            if (this.map.IsMapFieldADesertField(EyeOfStorm))
            {
                ChangeDesertField(EyeOfStorm);
                wasMapChanged = true;
            }

            // change the evelation of the neighbor fields of the eye of the storm, if they are a desert field

            foreach (MapField field in mapFieldsInStorm)
            {
                if (this.map.IsMapFieldADesertField(field))
                {
                    ChangeDesertField(field);
                    wasMapChanged = true;
                }
            }

            // check, whether the was changed after the movement of the sandstorm and send map change message if so
            if (wasMapChanged)
            {
                Party.GetInstance().messageController.DoSendMapChangeDemand(MapChangeReasons.SANDSTORM);
            }
        }

        /// <summary>
        /// randomly change a given desert field to "DUNE" or "FLAT_SAND"
        /// </summary>
        /// <param name="field">the deserts field to change</param>
        private void ChangeDesertField(MapField field)
        {
            Random random = new Random();

            if (random.NextDouble() < 0.5)
            {
                MapField newDune = new Dune(field.HasSpice, field.isInSandstorm, field.stormEye);
                newDune.Character = field.Character;

                map.SetMapFieldAtPosition(newDune, field.XCoordinate, field.ZCoordinate);
            }
            else
            {
                MapField newFlatSand = new FlatSand(field.HasSpice, field.isInSandstorm, field.stormEye);
                newFlatSand.Character = field.Character;

                map.SetMapFieldAtPosition(newFlatSand, field.XCoordinate, field.ZCoordinate);
            }

        }


        public void Execute()
        {
            MoveStormToRandomNeighborField();
            ChangeStatusOfMapFields();
            RandomlyChangeDesertFieldsInStorm();
        }
    }
}
