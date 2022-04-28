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
    public GameObject confirmationPanel;

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
        ConfirmDeactivate();
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
        ConfirmDeactivate();
        ButtonToggles();
    }

    public void ResetAction()
    {
        charState = Actions.EMPTY;
        ConfirmDeactivate();
    }


    //Button call methods
    public void SetCharStateMove()
    {
        //activation by node + press b
        this.charState = Actions.MOVE;
        confirmationPanel.SetActive(true);
    }

    public void SetCharStateAttack()
    {
        //activation by enemyChar
        this.charState = Actions.ATTACK;
        ConfirmDeactivate();
    }

    public void SetCharStateCollectSpice()
    {
        selectedCharacter.Action_CollectSpice();
        this.charState = Actions.COLLECT;
        ConfirmDeactivate();
    }

    public void SetCharStateTransferSpice()
    {
        //activation by otherChar
        this.charState = Actions.TRANSFER;
        ConfirmDeactivate();
    }

    public void SetCharStateKanly()
    {
        //activation by enemyChar
        this.charState = Actions.KANLY;
        ConfirmDeactivate();
    }

    public void SetCharStateVoice()
    {
        //activation by enemyChar
        this.charState = Actions.VOICE;
        ConfirmDeactivate();
    }

    public void SetCharStateSpiceHoarding()
    {
        selectedCharacter.Action_SpiceHoarding();
        this.charState = Actions.SPICE_HOARDING;
        ConfirmDeactivate();
    }

    public void SetCharStateSwordSpin()
    {
        selectedCharacter.Attack_SwordSpin();
        ResetSelection();
        ConfirmDeactivate();
    }
    public void SetCharStateAtomics()
    {
        //activation by node
        this.charState = Actions.FAMILY_ATOMICS;
        ConfirmDeactivate();
    }

    public void confirmAction()
    {
        if(this.charState == Actions.MOVE)
        {
            MovementManager.instance.AnimateSelectedChar();
        }
        ResetSelection();
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

    public void ConfirmDeactivate()
    {
        MovementManager.instance.unselectCharacter();
        NodeManager.instance.ResetNodeColors();
        confirmationPanel.SetActive(false);
    }
}
