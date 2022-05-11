using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * - stores in an state-machine the current state of selected char in turn
 * - manages visibility available actions for character
 */
public class CharacterTurnHandler : MonoBehaviour
{

    public static CharacterTurnHandler instance;

    public static bool CharSelected { get { return instance.selectedCharacter != null; } }

    private Character selectedCharacter;
    //  private Character secondCharacter;
    private Actions charState;
    public Actions CharState { get { return charState; } }

    [Header("Actions:")]
    public GameObject characterAttacksPanel;
    public GameObject confirmationPanel;
    public GameObject kanlyButton, voiceButton, swordSpinButton, atomicsButton, spiceHoardingButton;
    

    [Header("Stats:")]
    public GameObject playerStatsPanel;
    public GameObject PlayerText, SpiceText, CharacterText ,HPText, APText, MPText, SpiceInventoryText; 

    private MapManager nodeManager;

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
        nodeManager = MapManager.instance;
        // ButtonToggles();
        //  ConfirmDeactivate();
    }


    public void SelectCharacter(Character character)
    {
        selectedCharacter = character;
        ButtonToggles();
        selectedCharacter.DrawStats();
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
        Debug.Log("transfer spice");
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
        EndTurn();
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
        if (this.charState == Actions.MOVE)
        {
            MovementManager.instance.AnimateSelectedChar();
        }
        ResetSelection();
    }

    public static void EndTurn()
    {
        PlayerController.DoEndTurnRequest(1234,12);
        Debug.Log("Ended Turn!");
        instance.ResetSelection();
    }

    //Button activation/deactivation
    public void ButtonToggles()
    {
        if (selectedCharacter == null)
        {
            //basics
            characterAttacksPanel.SetActive(false);
            

            PlayerText.SetActive(true);
            SpiceText.SetActive(true);
            CharacterText.SetActive(false);
            HPText.SetActive(false);
            MPText.SetActive(false);
            APText.SetActive(false);
            SpiceInventoryText.SetActive(false);

            return;
        }
        else
        {
            characterAttacksPanel.SetActive(true);
           

            PlayerText.SetActive(false);
            SpiceText.SetActive(false);
            CharacterText.SetActive(true);
            HPText.SetActive(true);
            MPText.SetActive(true);
            APText.SetActive(true);
            SpiceInventoryText.SetActive(true);
        }

        if(!selectedCharacter.isEligibleForSpecialAction())
        {
            //special
            atomicsButton.SetActive(false);
            swordSpinButton.SetActive(false);
            kanlyButton.SetActive(false);
            voiceButton.SetActive(false);
            spiceHoardingButton.SetActive(false);
            return;
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
        confirmationPanel.SetActive(false);
    }
}
