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
    private LinkedList<Character> updateCharacters;
    private LinkedList<MoveAbles> OtherMoveAbles;
    [SerializeField]
    private List<Position> selCharPath;

    public static bool isAnimating { get { return instance.updateCharacters.Count != 0; } }


    /// <summary>
    /// This class shall manage the movement of the characters.
    /// Start is called before the first frame update
    /// </summary>
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

    /// <summary>
    /// Update is called once per frame. 
    /// Moves and removes all objects in animation queues.
    /// </summary>
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
            else
            {
                MapManager.instance.ResetNodeColors();
            }
            cluster = next;
        }

        //Every other Object to move, who is not a Character
        for (var cluster = OtherMoveAbles.First; cluster != null;)
        {
            var next = cluster.Next;
            if (cluster.Value == null || !cluster.Value.calledUpdate() )
            {
                UnityMainThreadDispatcher.allowDequeue = true;
                OtherMoveAbles.Remove(cluster);
            }
            cluster = next;
        }


    }

    /// <summary>
    /// Add a MoveAble like Sandworm to the animation-queue.
    /// </summary>
    /// <param name="moveAble"></param>
    public void addOtherToAnimate(MoveAbles moveAble)
    {
        OtherMoveAbles.AddLast(moveAble);
    }


    /// <summary>
    /// Clears the selected Path
    /// </summary>
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


    /// <summary>
    /// Checks whether the MP limit is reached and if point is in range.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns>True if node can be added. False otherwise</returns>
    public bool IsWaypointAttachable(int x, int z)
    {
        if (selCharPath.Count == 0)
        { // distinction needed at the first node to select
            return selCharPath.Count < CharacterTurnHandler.instance.GetSelectedCharacter().MP && MapManager.instance.isNodeNeighbour(CharacterTurnHandler.instance.GetSelectedCharacter().X,
                CharacterTurnHandler.instance.GetSelectedCharacter().Z, x, z);
        }
        else
        {
            return selCharPath.Count < CharacterTurnHandler.instance.GetSelectedCharacter().MP && MapManager.instance.isNodeNeighbour(selCharPath[selCharPath.Count - 1].x, selCharPath[selCharPath.Count - 1].y,
               x, z);
        }

    }

    /// <summary>
    /// Used by PlayerMessageController to add a character to the AnimationMoveQueue.
    /// </summary>
    /// <param name="character"></param>
    /// <param name="path"></param>
    public void AnimateChar(Character character, List<Position> path)
    {
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
            Character.semaphoreWalk++;
            if (Character.semaphoreWalk == 1)
            {
                AudioController.instance.Play("CharWalk");
            }
        }
    }


    /// <summary>
    /// Passes all needed parameters for a movement-request to the PlayerMessageController
    /// </summary>
    public void RequestMovement()
    {
        Log.Debug("DoRequestMovement: for characterID " + CharacterTurnHandler.instance.GetSelectedCharacter().characterId);
        SessionHandler.messageController.DoRequestMovement(SessionHandler.clientId, CharacterTurnHandler.instance.GetSelectedCharacter().characterId, selCharPath);
    }



}