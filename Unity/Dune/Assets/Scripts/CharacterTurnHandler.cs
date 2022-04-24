using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTurnHandler : MonoBehaviour
{

    private Character selectedCharacter;
    private Character secondCharacter;
    private Actions charState;
    public Actions CharState { get { return charState; } }

    public enum Actions
    {
        ATTACK, MOVE, COLLECT, TRANSFER, KANLY, FAMILY_ATOMICS, SPICE_HOARDING, VOICE, SWORD_SPIN 
    }

    public void selectCharacter(Character character)
    {
        selectedCharacter = character;
    }

    public void selectSecondCharacter(Character character)
    {
        secondCharacter = character;
    }

}
