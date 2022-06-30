using System;
using System.Collections.Generic;
using System.Linq;
using AIClient.Moves;
using GameData.network.messages;
using GameData.network.util.world;

namespace AIClient
{
    /// <summary>
    /// Represents the world, so the map and the characters, the ai client is playing on.
    /// </summary>
    /// <remarks>
    /// This class is used for storing the map changes coming from server and determine all possible moves or simulate moves in the world
    /// </remarks>
    public class World
    {
        /// <summary>
        /// the map, the ai client is playing on
        /// </summary>
        public Map Map { get; set; }

        public List<Character> AliveCharacters { get; private set; }

        public List<Character> AliveEnemies { get; private set; }

        /// <summary>
        /// the assigned (and chosen) great house for this client
        /// </summary>
        public string AssignedGreatHouse { get; set; }

        public Position CurrentPositionOfSandworm { get; set; }

        public static List<MoveTypes> AllMoveTypes { get; private set; }

        public World()
        {
            this.AliveCharacters = new List<Character>();
            this.AliveEnemies = new List<Character>();
            AllMoveTypes = ((MoveTypes[])Enum.GetValues(typeof(MoveTypes))).ToList();
        }

        /// <summary>
        /// adds a new character to the alive characters of the ai client
        /// </summary>
        /// <param name="character">the character to add to the list</param>
        /// <returns>true, if the character could be added, because there are less than six alive characters and otherwise false</returns>
        public bool AddAliveCharacter(Character character)
        {
            if (AliveCharacters.Count >= 6)
            {
                return false;
            }
            else
            {
                AliveCharacters.Add(character);
                return true;
            }
        }

        public bool AddAliveEnemy(Character character)
        {
            if (AliveEnemies.Count >= 6)
            {
                return false;
            }
            else
            {
                AliveEnemies.Add(character);
                return true;
            }
        }

        /// <summary>
        /// retrieves all possible moves for a certain character from a list of all moves,
        /// so it checks for every existing move, if the character can do this move
        /// </summary>
        /// <param name="character">the character, whose possible moves should be determined</param>
        /// <returns>a list of possible moves, this character can do</returns>
        public List<Move> GetAvailableMoves(Character character)
        {
            List<Move> possibleMoves = new List<Move>();

            // a character can always end its turn
            possibleMoves.Add(new EndTurn());

            // check, in which direction the character can move, if the character has enough movement points

            if (character.MPcurrent > 0)
            {
                List<Movement> possibleMovementDirections = GetPossibleMovementMoves(character);
                foreach (Movement direction in possibleMovementDirections)
                {
                    possibleMoves.Add(direction);
                }
            }


            // check, whether the character can move with the heli

            if (character.MPcurrent > 0)
            {
                List<MapField> heliports = Party.GetInstance().World.Map.GetHeliPortsOnMap();

                Position currentPosition = new Position(character.CurrentMapfield.XCoordinate, character.CurrentMapfield.ZCoordinate);

                // check, character is standing on a heliport
                // TODO: only move, when character is activly moving on this field
                if (heliports.Contains(character.CurrentMapfield))
                {
                    foreach (MapField targetField in heliports)
                    {
                        if (targetField != character.CurrentMapfield)
                        {
                            possibleMoves.Add(new HeliMovement(currentPosition, new Position(targetField.XCoordinate, targetField.ZCoordinate)));
                        }
                    }
                }

            }

            // check, which actions the character can do and if it has enough action points

            if (CanAttack(character))
            {
                foreach (Character neighborCharacter in GetCharacterOfEnemyNeighbors(character))
                {
                    Position neighborPosition = new Position(neighborCharacter.CurrentMapfield.XCoordinate, neighborCharacter.CurrentMapfield.ZCoordinate);
                    possibleMoves.Add(new Moves.Action(MoveTypes.ATTACK, neighborPosition));
                }
            }

            if (CanCollectSpice(character))
            {
                possibleMoves.Add(new Moves.Action(MoveTypes.COLLECT_SPICE));
            }

            for (int i = 1; i <= character.inventoryUsed; i++)
            {
                if (CanTransferSpice(character, i))
                {
                    foreach (Character neighborCharacter in GetCharacterOfFriendlyNeighbors(character))
                    {

                        possibleMoves.Add(new TransferSpice(neighborCharacter.CharacterId, i));
                    }
                }
            }

            if (CanDoKanly(character))
            {
                foreach (Character neighborCharacter in GetCharacterOfEnemyNeighbors(character))
                {
                    if (neighborCharacter.characterType.Equals(CharacterType.NOBLE.ToString()))
                    {
                        Position neighborPosition = new Position(neighborCharacter.CurrentMapfield.XCoordinate, neighborCharacter.CurrentMapfield.ZCoordinate);
                        possibleMoves.Add(new Moves.Action(MoveTypes.KANLY, neighborPosition));
                    }
                }
            }

            if (CanDoFamilyAtomics(character))
            {
                // TODO: do not use random field as target of the bomb, but a strategic target
                Random random = new Random();
                Position targetPosition = new Position(random.Next(Party.GetInstance().World.Map.MAP_WIDTH), random.Next(Party.GetInstance().World.Map.MAP_HEIGHT));
                possibleMoves.Add(new Moves.Action(MoveTypes.FAMILY_ATOMICS, targetPosition));
            }

            if (CanDoSpiceHoarding(character))
            {
                possibleMoves.Add(new Moves.Action(MoveTypes.SPICE_HOARDING));
            }

            if (CanDoVoice(character))
            {
                foreach(Character neighborCharacter in GetCharacterOfEnemyNeighbors(character))
                {
                    Position neighborPosition = new Position(neighborCharacter.CurrentMapfield.XCoordinate, neighborCharacter.CurrentMapfield.ZCoordinate);
                    possibleMoves.Add(new Moves.Action(MoveTypes.VOICE, neighborPosition));
                }

                foreach (Character neighborCharacter in GetCharacterOfFriendlyNeighbors(character))
                {
                    Position neighborPosition = new Position(neighborCharacter.CurrentMapfield.XCoordinate, neighborCharacter.CurrentMapfield.ZCoordinate);
                    possibleMoves.Add(new Moves.Action(MoveTypes.VOICE, neighborPosition));
                }
            }

            if (CanDoSwordSpin(character))
            {
                possibleMoves.Add(new Moves.Action(MoveTypes.SWORD_SPIN));
            }

            return possibleMoves;
        }

        /// <summary>
        /// gets all possible directions, a given character can move to
        /// </summary>
        /// <param name="character">the character, whose possible movement directions should be determined</param>
        /// <returns>a list of all directions, the character can move to</returns>
        private List<Movement> GetPossibleMovementMoves(Character character)
        {
            List<Movement> possibleDirections = new List<Movement>();

            Position currentPosition = new Position(character.CurrentMapfield.XCoordinate, character.CurrentMapfield.ZCoordinate);

            // check all possible movement directions:

            // moving left upwards
            Movement moveLeftUp = new Movement(MoveTypes.MOVE_LEFT_UP);
            if (MovementIsValid(moveLeftUp, currentPosition))
            {
                possibleDirections.Add(moveLeftUp);
            }

            // moving upwards
            Movement moveUp = new Movement(MoveTypes.MOVE_UP);
            if (MovementIsValid(moveUp, currentPosition))
            {
                possibleDirections.Add(moveUp);
            }

            // moving right upwards
            Movement moveRightUp = new Movement(MoveTypes.MOVE_RIGHT_UP);
            if (MovementIsValid(moveRightUp, currentPosition))
            {
                possibleDirections.Add(moveRightUp);
            }

            // moving rightwards
            Movement moveRight = new Movement(MoveTypes.MOVE_RIGHT);
            if (MovementIsValid(moveRight, currentPosition))
            {
                possibleDirections.Add(moveRight);
            }

            // moving right downwards
            Movement moveRightDownw = new Movement(MoveTypes.MOVE_RIGHT_DOWN);
            if (MovementIsValid(moveRightDownw, currentPosition))
            {
                possibleDirections.Add(moveRightDownw);
            }

            // moving downwards
            Movement moveDown = new Movement(MoveTypes.MOVE_DOWN);
            if (MovementIsValid(moveDown, currentPosition))
            {
                possibleDirections.Add(moveDown);
            }
            // moving left downwards
            Movement moveLeftDown = new Movement(MoveTypes.MOVE_LEFT_DOWN);
            if (MovementIsValid(moveLeftDown, currentPosition))
            {
                possibleDirections.Add(moveLeftDown);
            }

            // moving leftwards
            Movement moveLeft = new Movement(MoveTypes.MOVE_LEFT);
            if (MovementIsValid(moveLeft, currentPosition))
            {
                possibleDirections.Add(moveLeft);
            }

            return possibleDirections;
        }

        /// <summary>
        /// checks, if a character at a given position can move into a given direction and still is on the map and the new field is approachable
        /// </summary>
        /// <param name="movement">the movement and so the direction, in which the character want to move</param>
        /// <param name="currentPosition">the position of the character</param>
        /// <returns>true, if a movement in a certain direction is valid (on map + field is approachable)</returns>
        private bool MovementIsValid(Movement movement, Position currentPosition)
        {
            Position newPosition = Position.Move(currentPosition, movement.DeltaX, movement.DeltaY);
            MapField newMapField = Party.GetInstance().World.Map.GetMapFieldAtPosition(newPosition.x, newPosition.y);

            

            if (newMapField != null)
            {
                if (newMapField.isInSandstorm)
                {
                    return false;
                }
                return newMapField.IsApproachable;
            }
            return false;
        }

        /// <summary>
        /// checks, whether a given character can do an attack
        /// </summary>
        /// <param name="character"the character, who wants to attack></param>
        /// <returns>true, if the character can attack (AP > 0 and enemy on neighbor field)</returns>
        private bool CanAttack(Character character)
        {
            // check enough AP
            if (character.APcurrent > 0)
            {
                // check enemy on neighbor field
                List<MapField> neighborFields = Party.GetInstance().World.Map.GetNeighborFields(character.CurrentMapfield);
                foreach (MapField neighbor in neighborFields)
                {
                    if (neighbor.IsCharacterStayingOnThisField)
                    {
                        if (!Party.GetInstance().World.AliveCharacters.Contains(neighbor.Character))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// checks, whether a given character can collect spice
        /// </summary>
        /// <param name="character">the character, who wants to collect spice</param>
        /// <returns>true, if the character can collect spice</returns>
        private bool CanCollectSpice(Character character)
        {
            // check enough AP
            if (character.APcurrent > 0)
            {
                // check spice on the map field of the character
                if (character.CurrentMapfield.hasSpice)
                {
                    // check character has enough inventory space left
                    if (character.inventoryUsed < character.inventorySize)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// checks, whether a given character can transfer spice
        /// </summary>
        /// <param name="character">the character, who wants to transfer spice</param>
        /// <param name="spiceAmount">the amount of spice, the character want to transfer</param>
        /// <returns>true, if the character can transfer spice</returns>
        private bool CanTransferSpice(Character character, int spiceAmount)
        {
            // check enough AP
            if (character.APcurrent > 0)
            {
                // check character has the amount
                if (character.inventoryUsed >= spiceAmount)
                {
                    // check character from own great house is on neighbor field
                    List<MapField> neighborFields = Party.GetInstance().World.Map.GetNeighborFields(character.CurrentMapfield);
                    foreach (MapField neighbor in neighborFields)
                    {
                        if (neighbor.IsCharacterStayingOnThisField)
                        {
                            if (Party.GetInstance().World.AliveCharacters.Contains(neighbor.Character))
                            {
                                // check other character has enough space in inventory
                                if (neighbor.Character.inventorySize - neighbor.Character.inventoryUsed >= spiceAmount)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// checks, whether a given character is a noble and can do the Kanly action
        /// </summary>
        /// <param name="character">the character, who wants to do the Kanly action</param>
        /// <returns>true, if the character can do the Kanly action</returns>
        private bool CanDoKanly(Character character)
        {
            // check, there are no AP already used
            if (character.APcurrent == character.APmax)
            {
                // check, character is a noble
                if (character.characterType.Equals(CharacterType.NOBLE.ToString()))
                {
                    // check enemy is on neighbor field
                    List<MapField> neighborFields = Party.GetInstance().World.Map.GetNeighborFields(character.CurrentMapfield);
                    foreach (MapField neighbor in neighborFields)
                    {
                        if (neighbor.IsCharacterStayingOnThisField)
                        {
                            if (!Party.GetInstance().World.AliveCharacters.Contains(neighbor.Character))
                            {
                                // check other character is also noble
                                if (neighbor.Character.characterType.Equals(CharacterType.NOBLE.ToString()))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }


        /// <summary>
        /// checks, whether a given character is a noble and can do the Family Atomics action
        /// </summary>
        /// <param name="character">the character, who wants to do the Family Atomics action</param>
        /// <returns>true, if the character can do the Family Atomics action</returns>
        private bool CanDoFamilyAtomics(Character character)
        {
            // check, there are no AP already used
            if (character.APcurrent == character.APmax)
            {
                // check, character is a noble
                if (character.characterType.Equals(CharacterType.NOBLE.ToString()))
                {
                    // check, there are atomics left
                    if (Party.GetInstance().UsedFamilyAtomics < 3)
                    {
                        return true;
                    }

                }
            }

            return false;
        }

        /// <summary>
        /// checks, whether a given character is a mentat and can do the Spice Hoarding action
        /// </summary>
        /// <param name="character">the character, who wants to do the Spice Hoarding action</param>
        /// <returns>true, if the character can do the Spice Hoarding action</returns>
        private bool CanDoSpiceHoarding(Character character)
        {
            // check, there are no AP already used
            if (character.APcurrent == character.APmax)
            {
                // check, character is a mentat
                if (character.characterType.Equals(CharacterType.MENTAT.ToString()))
                {
                    int freeSpaceInInventory = character.inventorySize - character.inventoryUsed;

                    if (freeSpaceInInventory > 0)
                    {
                        // check, spice on neighbor fields
                        List<MapField> neighborFields = Party.GetInstance().World.Map.GetNeighborFields(character.CurrentMapfield);
                        foreach (MapField neighbor in neighborFields)
                        {
                            if (neighbor.hasSpice)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// checks, whether a given character is a bene gesserit and can do the Voice action
        /// </summary>
        /// <param name="character">the character, who wants to do the Voice action</param>
        /// <returns>true, if the character can do the Voice action</returns>
        private bool CanDoVoice(Character character)
        {
            // check, there are no AP already used
            if (character.APcurrent == character.APmax)
            {
                // check, character is a bene gesserit
                if (character.characterType.Equals(CharacterType.BENE_GESSERIT.ToString()))
                {
                    int freeSpaceInInventory = character.inventorySize - character.inventoryUsed;

                    if (freeSpaceInInventory > 0)
                    {
                        // check, other character on neighbor field
                        List<MapField> neighborFields = Party.GetInstance().World.Map.GetNeighborFields(character.CurrentMapfield);
                        foreach (MapField neighbor in neighborFields)
                        {
                            if (neighbor.IsCharacterStayingOnThisField)
                            {
                                // check, other character has spice
                                if (neighbor.Character.inventoryUsed > 0)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// checks, whether a given character is a fighter and can do the Sword spin action
        /// </summary>
        /// <param name="character">the character, who wants to do the Sword spin action</param>
        /// <returns>true, if the character can do the Sword spin action</returns>
        private bool CanDoSwordSpin(Character character)
        {
            // check, there are no AP already used
            if (character.APcurrent == character.APmax)
            {
                // check, character is a fighter
                if (character.characterType.Equals(CharacterType.FIGHTER.ToString()))
                {
                    // check enemy on neighbor field
                    List<MapField> neighborFields = Party.GetInstance().World.Map.GetNeighborFields(character.CurrentMapfield);
                    foreach (MapField neighbor in neighborFields)
                    {
                        if (neighbor.IsCharacterStayingOnThisField)
                        {
                            if (!Party.GetInstance().World.AliveCharacters.Contains(neighbor.Character))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }


        /// <summary>
        /// get a list of all characters, that are in the same great house like the given character and are standing to the next field
        /// </summary>
        /// <param name="character">the character whose friendly neighbors should be determined</param>
        /// <returns>a list of all friendly neighbor characters</returns>
        private List<Character> GetCharacterOfFriendlyNeighbors(Character character)
        {
            List<Character> characters = new List<Character>();

            // check enemy on neighbor field
            List<MapField> neighborFields = Party.GetInstance().World.Map.GetNeighborFields(character.CurrentMapfield);
            foreach (MapField neighbor in neighborFields)
            {
                if (neighbor.IsCharacterStayingOnThisField)
                {
                    if (Party.GetInstance().World.AliveCharacters.Contains(neighbor.Character))
                    {
                        characters.Add(neighbor.Character);
                    }
                }
            }

            return characters;
        }

        /// <summary>
        /// get a list of all characters, that are in the other great house like the given character and are standing to the next field
        /// </summary>
        /// <param name="character">the character whose enemy neighbors should be determined</param>
        /// <returns>a list of all enemy neighbor characters</returns>
        private List<Character> GetCharacterOfEnemyNeighbors(Character character)
        {
            List<Character> characters = new List<Character>();

            // check enemy on neighbor field
            List<MapField> neighborFields = Party.GetInstance().World.Map.GetNeighborFields(character.CurrentMapfield);
            foreach (MapField neighbor in neighborFields)
            {
                if (neighbor.IsCharacterStayingOnThisField)
                {
                    if (!Party.GetInstance().World.AliveCharacters.Contains(neighbor.Character))
                    {
                        characters.Add(neighbor.Character);
                    }
                }
            }

            return characters;
        }
    }
}
