using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTurnHandler : MonoBehaviour
{

    public static CharacterTurnHandler instance;

    public static bool CharSelected { get { return instance.selectedCharacter != null; } }

    private Character selectedCharacter;
  //  private Character secondCharacter;
    private Actions charState;
    public Actions CharState { get { return charState; } }
    
    private NodeManager nodeManager;

    public enum Actions
    {
        ATTACK, MOVE, COLLECT, TRANSFER, KANLY, FAMILY_ATOMICS, SPICE_HOARDING, VOICE, SWORD_SPIN, EMPTY
    }

    private void Awake()
    {
        instance = this;
    }

    
    private void Start()
    {
        nodeManager = NodeManager.instance;
    }

    void Update()
    {
        if(Input.GetKey("j"))
        {
            charState = Actions.MOVE;
        }
        else if(Input.GetKey("k"))
        {
            charState = Actions.ATTACK;
        }
        else if(Input.GetKeyDown("l"))
        {
            //charState = Actions.SWORD_SPIN;
            selectedCharacter.Attack_SwordSpin();
            
        }
        else if(Input.GetKey("n"))
        {
            charState = Actions.FAMILY_ATOMICS;
        }
    }

    public void SelectCharacter(Character character)
    {
        selectedCharacter = character;
    }


    public Character GetSelectedCharacter()
    {
        return selectedCharacter;
    }

    public void ResetSelection()
    {
        selectedCharacter = null;
       charState = Actions.EMPTY;
    }

    public void ResetAction()
    {
        charState = Actions.EMPTY;
    }




}
