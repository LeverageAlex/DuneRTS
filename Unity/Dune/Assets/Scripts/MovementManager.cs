using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;



[Serializable]
    public class MovementManager : MonoBehaviour
    {

        public static MovementManager instance;
        // private Character selectedChar;
        private LinkedList<Character> updateCharacters;
        private LinkedList<MoveAbles> OtherMoveAbles;
        [SerializeField]
        private LinkedList<Vector3> selCharPath;

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
                selCharPath = new LinkedList<Vector3>();
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
                    NodeManager.instance.getNodeFromPos(cluster.Value.X, cluster.Value.Z).ResetColor();
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
                AnimateSelectedChar();
            }

        }


        /*  public void selectCharacter(Character character)
          {
              selectedChar = character;
          }*/

        //Ignores all other functions within class
        public void addCharacterToAnimate(Character character, LinkedList<Vector3> pathing)
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

        public void AddWaypoint(Vector3 vec)
        {
            if (IsWaypointAttachable((int)vec.x, (int)vec.z))
            {
                selCharPath.AddLast(vec);
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
                return selCharPath.Count < CharacterTurnHandler.instance.GetSelectedCharacter().MP && NodeManager.instance.isNodeNeighbour(CharacterTurnHandler.instance.GetSelectedCharacter().X,
                    CharacterTurnHandler.instance.GetSelectedCharacter().Z, x, z);
            }
            else
            {
                return selCharPath.Count < CharacterTurnHandler.instance.GetSelectedCharacter().MP && NodeManager.instance.isNodeNeighbour((int)selCharPath.Last.Value.x, (int)selCharPath.Last.Value.z,
                   x, z);
            }

            return true;
        }

        public void AnimateSelectedChar()
        {
            // if (!isAnimating)
            // {
            if (selCharPath.Count > 0)
            {
                Character selectedChar = CharacterTurnHandler.instance.GetSelectedCharacter();
                updateCharacters.AddLast(selectedChar);

                // PlayerController.doRequestMovement(ClientID, CharacterId, path); 
                PlayerController.DoRequestMovement(1234, selectedChar.GetInstanceID(), selCharPath);
                
                // Message = PlayerController.OnMovement((Movement)Message)

                // Sollte erst ausgeführt werden, wenn die aktion ausgeführt werden darf.
                selectedChar.SetWalkPath(selCharPath);
                selectedChar.ReduceMP(selCharPath.Count);
                selCharPath = new LinkedList<Vector3>();
                CharacterTurnHandler.instance.ResetSelection();
            }
        }


    
}