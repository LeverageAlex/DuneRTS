using System;
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
    public GameObject PlayerText, SpiceText, CharacterText ,HPText, APText, MPText, SpiceInventoryText, selectedArrow; 

    private MapManager nodeManager;

    public enum Actions
    {
        ATTACK, MOVE, COLLECT, TRANSFER, KANLY, FAMILY_ATOMICS, SPICE_HOARDING, VOICE, SWORD_SPIN, EMPTY, HELIPORT
    }

    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        nodeManager = MapManager.instance;
        ButtonToggles();
        ConfirmDeactivate();
    }


    public void SelectCharacter(Character character)
    {
        selectedCharacter = character;
        selectedArrow.SetActive(true);
        updateSelectionArrow();
        ButtonToggles();
        selectedCharacter.DrawStats();
    }

    public void SelectAlienCharacter(Character character)
    {
        selectedCharacter = character;
        selectedArrow.SetActive(true);
        updateSelectionArrow();
    }


    public Character GetSelectedCharacter()
    {
        return selectedCharacter;
    }

    public void ResetSelection()
    {
        selectedCharacter = null;
        selectedArrow.SetActive(false);
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
        AudioController.instance.Play("menuSelect");
    }

    public void SetCharStateAttack()
    {
        //activation by enemyChar
        if(charState == Actions.MOVE) nodeManager.ResetNodeColors();
        this.charState = Actions.ATTACK;
        ConfirmDeactivate();
    }

    public void SetCharStateCollectSpice()
    {
        if (charState == Actions.MOVE) nodeManager.ResetNodeColors();
        selectedCharacter.Action_CollectSpiceTrigger();
        this.charState = Actions.COLLECT;
        ConfirmDeactivate();
    }

    public void SetCharStateTransferSpice()
    {
        //activation by otherChar
        if (charState == Actions.MOVE) nodeManager.ResetNodeColors();
        this.charState = Actions.TRANSFER;
        Debug.Log("transfer spice");
        ConfirmDeactivate();
    }

    public void SetCharStateKanly()
    {
        //activation by enemyChar
        if (charState == Actions.MOVE) nodeManager.ResetNodeColors();
        this.charState = Actions.KANLY;
        ConfirmDeactivate();
    }

    public void SetCharStateVoice()
    {
        //activation by enemyChar
        if (charState == Actions.MOVE) nodeManager.ResetNodeColors();
        this.charState = Actions.VOICE;
        ConfirmDeactivate();
    }

    public void SetCharStateSpiceHoarding()
    {
        if (charState == Actions.MOVE) nodeManager.ResetNodeColors();
        selectedCharacter.Action_SpiceHoardingTrigger();
        this.charState = Actions.EMPTY;
        ConfirmDeactivate();
    }

    public void SetCharStateSwordSpin()
    {
        if (charState == Actions.MOVE) nodeManager.ResetNodeColors();
        selectedCharacter.Attack_SwordSpinTrigger();
        EndTurn();
        ConfirmDeactivate();
    }
    public void SetCharStateAtomics()
    {
        if (charState == Actions.MOVE) nodeManager.ResetNodeColors();
        //activation by node
        this.charState = Actions.FAMILY_ATOMICS;
        ConfirmDeactivate();
    }

    public void SetCharStateHeliport()
    {
        if (charState == Actions.MOVE) nodeManager.ResetNodeColors();
        this.charState = Actions.HELIPORT;
        ConfirmDeactivate();
        DisableSelectionBox();
    }


    /// <summary>
    ///Is called when the confirmButton is pressed
    /// </summary>
    public void confirmAction()
    {
        if (this.charState == Actions.MOVE)
        {
            //MovementManager.instance.AnimateSelectedChar();
            MovementManager.instance.RequestMovement();
        }

        charState = Actions.EMPTY;
        ConfirmDeactivate();
    }

    public static void EndTurn()
    {
        //PlayerMessageController.DoEndTurnRequest(1234,12);
        Debug.Log("Ended Turn!");
        if(Mode.debugMode)
        {
            instance.ResetSelection();
        } 
        else
        {
            SessionHandler.messageController.DoRequestEndTurn(SessionHandler.clientId, instance.selectedCharacter.characterId);
        }
        
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
        if (SessionHandler.isPlayer)
        {
            if (!selectedCharacter.isEligibleForSpecialAction())
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
    }

    public void ConfirmDeactivate()
    {
        MovementManager.instance.unselectCharacter();
        confirmationPanel.SetActive(false);
        AudioController.instance.Play("menuSelect");
    }


    public void DisableSelectionBox()
    {
        PlayerText.SetActive(true);
        SpiceText.SetActive(true);
        CharacterText.SetActive(false);
        HPText.SetActive(false);
        MPText.SetActive(false);
        APText.SetActive(false);
        SpiceInventoryText.SetActive(false);
    }


    public void updateSelectionArrow()
    {
        if (selectedCharacter != null)
        {
            selectedArrow.transform.position = selectedCharacter.transform.position;
        }
    }
}
