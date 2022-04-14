using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PartyConfiguration;

public class Character : MonoBehaviour
{
    public string name;
    //TODO
    //Wird nur während des erstellen von noch nicht selbstgenerierten Leveln benötigt (da so im UnityEditor gewählt werden kann).
    //Sollte später durch einfach durch eine direkte Referenz ersetzt
    public string charType = "Noble";
    //Es fehlt die Zuordnung zum Haus

    private int HP;  //Health Points
    private int healingHP;
    private int MP;  //Movement Points
    private int AP;  //Attack Points
    private int AD;  //Attack-Damage
    private int spiceInv;

    private bool noisy;




    



    // Start is called before the first frame update
    void Start()
    {
        //SampleCode only

        initCharacter(PartyConfiguration.Noble);
        Debug.Log("HP " + HP + ", AP " + AP);
    }

  
    public void initCharacter(CharacterBaseValue characterBaseValue)
    {
        HP = characterBaseValue.HP;
        MP = characterBaseValue.MP;
        AP = characterBaseValue.AP;
        AD = characterBaseValue.AD;
        spiceInv = characterBaseValue.spiceInv;
        healingHP = characterBaseValue.HealHP;
    }

    CharacterBaseValue getTypeByString(string charType)
    {
        switch(charType)
        {
            case "Noble":
                return PartyConfiguration.Noble;
                break;
            case "BenneGesserit":
                return PartyConfiguration.BeneGesserit;
            case "Mentat":
                return PartyConfiguration.Mentat;
            case "Fighter":
                return PartyConfiguration.Fighter;
            default:
                Debug.Log("Error in Character-Script. String did not Match");
                return null;
        }
    }


}
