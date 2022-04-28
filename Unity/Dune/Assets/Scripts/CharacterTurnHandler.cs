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

    public GameObject kanlyButton, voiceButton, swordSpinButton, atomicsButton, spiceHoardingButton;
    public GameObject characterAttacksPanel;

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
        ButtonToggles();
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
            ResetSelection();
            
        }
        else if(Input.GetKey("n"))
        {
            charState = Actions.FAMILY_ATOMICS;
        }
    }

    

    public void SelectCharacter(Character character)
    {
        selectedCharacter = character;
        ButtonToggles();
    }


    public Character GetSelectedCharacter()
    {
        return selectedCharacter;
    }

    public void ResetSelection()
    {
        selectedCharacter = null;
        charState = Actions.EMPTY;
        ButtonToggles();
    }

    public void ResetAction()
    {
        charState = Actions.EMPTY;
    }


    //Button call methods
    public void SetCharStateMove()
    {
        this.charState = Actions.MOVE;
    }

    public void SetCharStateAttack()
    {
        this.charState = Actions.ATTACK;
    }

    public void SetCharStateSwordSpin()
    {
        selectedCharacter.Attack_SwordSpin();
        ResetSelection();
    }
    public void SetCharStateAtomics()
    {
        this.charState = Actions.FAMILY_ATOMICS;
    }
    
    //Button activation/deactivation
    public void ButtonToggles()
    {
        if(selectedCharacter == null)
        {
            //basics
            characterAttacksPanel.SetActive(false);
            //special
            atomicsButton.SetActive(false);
            swordSpinButton.SetActive(false);
            kanlyButton.SetActive(false);
            voiceButton.SetActive(false);
            spiceHoardingButton.SetActive(false);
            return;
        } 
        else
        {
            characterAttacksPanel.SetActive(true);
        }

        switch (selectedCharacter.characterType)
        {
            case CharTypeEnum.FIGHTER:
                atomicsButton.SetActive(false);
                swordSpinButton.SetActive(true);
                kanlyButton.SetActive(false);
                voiceButton.SetActive(false);
                spiceHoardingButton.SetActive(false);
                break;
            case CharTypeEnum.NOBLE:
                atomicsButton.SetActive(true);
                swordSpinButton.SetActive(false);
                kanlyButton.SetActive(true);
                voiceButton.SetActive(false);
                spiceHoardingButton.SetActive(false);
                break;
            case CharTypeEnum.MENTANT:
                atomicsButton.SetActive(false);
                swordSpinButton.SetActive(false);
                kanlyButton.SetActive(false);
                voiceButton.SetActive(false);
                spiceHoardingButton.SetActive(true);
                break;
            case CharTypeEnum.BENEGESSERIT:
                atomicsButton.SetActive(false);
                swordSpinButton.SetActive(false);
                kanlyButton.SetActive(false);
                voiceButton.SetActive(true);
                spiceHoardingButton.SetActive(false);
                break;
            default:
                atomicsButton.SetActive(false);
                swordSpinButton.SetActive(false);
                kanlyButton.SetActive(false);
                voiceButton.SetActive(false);
                spiceHoardingButton.SetActive(false);
                break;
        }
    }
}
