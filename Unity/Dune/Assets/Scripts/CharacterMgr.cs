using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMgr : MonoBehaviour
{
    //Connects CharacterID to CharacterObject
    Dictionary<int, GameObject> characterDict = new Dictionary<int, GameObject>();

    public GameObject noblePrefab;
    public GameObject mentatPrefab;
    public GameObject beneGesseritPrefab;
    public GameObject fighterPrefab;


    public enum charType
    {
        NOBLE, MENTANT, BENEGESSERIT, FIGHTER
    }

    // Start is called before the first frame update
    void Start()
    {
        //Example to show Method
        //spawnCharacter(175, "FIGHTER", 15, 15, 4, 10, 4, 4, 3, 6, false, false);
   
    }





    /*
     * To be filled after open question regarding standardDocument has ben resolved
  */  public bool spawnCharacter(int characterID, string type,int x, int z,int HPcurrent, int healingHP, int MPcurrent, int APcurrent, int attackDamage, int inventoryLeft, bool killedBySandworm, bool loud)
    {
        if (characterDict.ContainsKey(characterID))
            return false;



        GameObject newChar = (GameObject) Instantiate(getCharTypeByString(type), new Vector3(x, 0f, z), Quaternion.identity);
        characterDict.Add(characterID, newChar);
        Character localChar = (Character) newChar.GetComponent(typeof(Character));
        localChar.updateCharStats(HPcurrent, healingHP, MPcurrent, APcurrent, attackDamage, inventoryLeft, loud, killedBySandworm);
        return true;
    }

    public bool characterStatChange(int characterID, int HP, int HealHP, int MP, int AP, int AD, int spiceInv, bool isLoud, bool isSwallowed)
    {
        if (!characterDict.ContainsKey(characterID))
            return false;

        Character charScript = getCharScriptByID(characterID);
        charScript.updateCharStats(HP, HealHP, MP, AP, AD, spiceInv, isLoud, isSwallowed);
        return true;
    }

    public Character getCharScriptByID(int characterID)
    {
        return (Character)characterDict[characterID].GetComponent(typeof(Character));
    }


    GameObject getCharTypeByString(string type)
    {
        switch (type)
        {
            case "NOBLE":
                return noblePrefab;
            case "BENEGESSERIT":
                return beneGesseritPrefab;
            case "MENTAT":
                return mentatPrefab;
            case "FIGHTER":
                return fighterPrefab;
            default:
                Debug.Log("Error in CharacterMgr: CharType doesn't exist!");
                return null;
        }
    }


}
