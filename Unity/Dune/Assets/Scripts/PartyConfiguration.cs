using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyConfiguration : MonoBehaviour
{

    public static readonly CharacterBaseValue Noble;
    public static readonly CharacterBaseValue Mentat;
    public static readonly CharacterBaseValue BeneGesserit;
    public static readonly CharacterBaseValue Fighter;


    /*Inits our troops with values
 * Currently uses values from Lastenheft
 * Will only be called once
*/
    static PartyConfiguration()
    {
        Noble = new CharacterBaseValue(100, 10, 2, 2, 20, 5);
        Mentat = new CharacterBaseValue(75, 10, 2, 3, 10, 10);
        BeneGesserit = new CharacterBaseValue(150, 20, 3, 2, 20, 5);
        Fighter = new CharacterBaseValue(200, 20, 2, 2, 40, 3);
        Debug.Log("Party configuration successfully initalised!");
    }




    public class CharacterBaseValue
    {
        public int HP, HealHP, MP, AP, AD, spiceInv;

        public CharacterBaseValue(int HP, int HealHP, int MP, int AP, int AD, int spiceInv)
        {
            this.HP = HP;
            this.HealHP = HealHP;
            this.MP = MP;
            this.AP = AP;
            this.AD = AD;
            this.spiceInv = spiceInv;
        }
    }
}
