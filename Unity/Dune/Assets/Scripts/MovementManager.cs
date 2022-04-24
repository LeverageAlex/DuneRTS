using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{

    public static MovementManager instance;
   // private Character selectedChar;
    private LinkedList<Character> updateCharacters;
    private LinkedList<Vector3> selCharPath;

   // public static bool charSelected { get { return instance.selectedChar != null; } }
    public static bool isAnimating { get { return instance.updateCharacters.Count != 0; } }

    /**
     * This class shall manage the movement of the characters
     */

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            updateCharacters = new LinkedList<Character>();
            selCharPath = new LinkedList<Vector3>();
        }
        else Debug.Log("MovementManager Error. Instance of updateCharacters already exist.");


    }

    // Update is called once per frame
    void Update()
    {


        for (var cluster = updateCharacters.First;  cluster != null; ) 
        {
            var next = cluster.Next;
            if (!cluster.Value.calledUpdate()) { 
                updateCharacters.Remove(cluster);
            }
            cluster = next;
        }

        //Test code
        //Starts animation on key b
        if (Input.GetKey("b") && selCharPath.Count > 0) {
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


    public void unselectCharacter()
    {
        selCharPath.Clear();
    }

    public void AddWaypoint(Vector3 vec)
    {
        selCharPath.AddLast(vec);
    }

    public void AnimateSelectedChar()
    {
        if (!isAnimating)
        {
            Character selectedChar = CharacterTurnHandler.instance.GetSelectedCharacter();
            updateCharacters.AddLast(selectedChar);
            selectedChar.SetWalkPath(selCharPath);
            selCharPath = new LinkedList<Vector3>();
            CharacterTurnHandler.instance.ResetSelection();
        }
    }

    
}
