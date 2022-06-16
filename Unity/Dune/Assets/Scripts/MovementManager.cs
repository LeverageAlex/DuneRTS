using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameData.network.util.world;
using Serilog;

/**
 * This manages all Characters and Moveables, that shall be moved.
 * - Helps creating the movement path of chars
 * - Moves Characters according to path
 */
[Serializable]
    public class MovementManager : MonoBehaviour
    {

        public static MovementManager instance;
        // private Character selectedChar;
        private LinkedList<Character> updateCharacters;
        private LinkedList<MoveAbles> OtherMoveAbles;
        [SerializeField]
        private List<Position> selCharPath;

        // public static bool charSelected { get { return instance.selectedChar != null; } }
        public static bool isAnimating { get { return instance.updateCharacters.Count != 0; } }

        /**
         * This class shall manage the movement of the characters
         */

        // Start is called before the first frame update
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                updateCharacters = new LinkedList<Character>();
                selCharPath = new List<Position>();
                OtherMoveAbles = new LinkedList<MoveAbles>();
            }
            else Debug.Log("MovementManager Error. Instance of updateCharacters already exist.");


        }

        // Update is called once per frame
        void Update()
        {


            for (var cluster = updateCharacters.First; cluster != null;)
            {
                var next = cluster.Next;
                if (!cluster.Value.calledUpdate())
                {
                    MapManager.instance.getNodeFromPos(cluster.Value.X, cluster.Value.Z).ResetColor();
                    updateCharacters.Remove(cluster);
                }
                cluster = next;
            }

            //Every other Object to move, who is not a Character
            for (var cluster = OtherMoveAbles.First; cluster != null;)
            {
                var next = cluster.Next;
                if (!cluster.Value.calledUpdate())
                {
                    OtherMoveAbles.Remove(cluster);
                }
                cluster = next;
            }

        //Test code
        //Starts animation on key b

        if (Input.GetKeyDown(KeyCode.Return) && selCharPath.Count > 0)
            {
            // AnimateChar(character, MovementManager.instance.getSelCharPath);
            RequestMovement();
            }

        }


        /*  public void selectCharacter(Character character)
          {
              selectedChar = character;
          }*/

        //Ignores all other functions within class
        public void addCharacterToAnimate(Character character, List<Vector3> pathing)
        {
            updateCharacters.AddLast(character);
            CharacterTurnHandler.instance.GetSelectedCharacter().SetWalkPath(pathing);
        }

    public void addOtherToAnimate(MoveAbles moveAble)
    {
        OtherMoveAbles.AddLast(moveAble);
    }


    public void unselectCharacter()
        {
            selCharPath.Clear();
        }

        public void AddWaypoint(Position vec)
        {
            if (IsWaypointAttachable(vec.x, vec.y))
            {
                selCharPath.Add(vec);
            }
            else
            {
                Debug.Log("Can not extend Path, due too low MP or Field out of range");
            }
        }


        /*
         * Will check whether the MP limit is reached and if point is in range.
         * Currently deactivated for easier testing, but should be activated later
         */
        public bool IsWaypointAttachable(int x, int z)
        {
            if (selCharPath.Count == 0) { // distinction needed at the first node to select
                return selCharPath.Count < CharacterTurnHandler.instance.GetSelectedCharacter().MP && MapManager.instance.isNodeNeighbour(CharacterTurnHandler.instance.GetSelectedCharacter().X,
                    CharacterTurnHandler.instance.GetSelectedCharacter().Z, x, z);
            }
            else
            {
                return selCharPath.Count < CharacterTurnHandler.instance.GetSelectedCharacter().MP && MapManager.instance.isNodeNeighbour(selCharPath[selCharPath.Count-1].x, selCharPath[selCharPath.Count-1].y,
                   x, z);
            }

           // return true;
        }

        //public void AnimateSelectedChar()
        //{
            // if (!isAnimating)
            // {
           /* if (selCharPath.Count > 0)
            {
                Character selectedChar = CharacterTurnHandler.instance.GetSelectedCharacter();
                updateCharacters.AddLast(selectedChar);

                // PlayerController.doRequestMovement(ClientID, CharacterId, path); 
               // PlayerMessageController.DoMovementRequest(1234, selectedChar.GetInstanceID(), selCharPath);
                
                // Message = PlayerController.OnMovement((Movement)Message)

                // Sollte erst ausgeführt werden, wenn die aktion ausgeführt werden darf.
                selectedChar.SetWalkPath(selCharPath);
                selectedChar.ReduceMP(selCharPath.Count);
                selCharPath = new List<Position>();
                CharacterTurnHandler.instance.ResetSelection();
            AudioController.instance.Play("CharWalk");
            }*/
        //}

    private LinkedList<Vector3> convertVector(List<Position> path) {
        LinkedList<Vector3> newPos = new LinkedList<Vector3>();

        Vector3 tmp;
        foreach(Position p in path)
        {
            
            tmp = new Vector3(p.x, 0, p.y);
            newPos.AddLast(tmp);
        }

        return newPos;
    }

    public void AnimateChar(Character character, List<Position> path) {
        if (path.Count > 0)
        {
            List<Vector3> newPath = new List<Vector3>(path.Count);

            Vector3 tmp;
            foreach (Position p in path)
            {

                tmp = new Vector3(p.x, character.BaseY + MapManager.instance.getNodeFromPos(p.x, p.y).charHeightOffset, p.y);
                newPath.Add(tmp);
            }


            updateCharacters.AddLast(character);
            character.SetWalkPath(newPath);
            AudioController.instance.Play("CharWalk");
        }
    }


    public List<Position> getSelCharPath()
    {
        return selCharPath;
    }


    public void RequestMovement()
    {
        //   Log.Debug("Button getriggert. Sende Nachricht");
        //    SessionHandler.messageController.DoRequestHouse("ATREIDES");
        //if (!Mode.debugMode)
        //{
        Log.Debug("DoRequestMovement: for characterID " + CharacterTurnHandler.instance.GetSelectedCharacter().CharacterId);
        SessionHandler.messageController.DoRequestMovement(SessionHandler.clientId, CharacterTurnHandler.instance.GetSelectedCharacter().CharacterId, selCharPath);
        // }
        //else
        //{
        //     AnimateChar(CharacterTurnHandler.instance.GetSelectedCharacter(), selCharPath);
        //    selCharPath.Clear();
        // }
    }


    
}