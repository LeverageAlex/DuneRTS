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

    // Start is called before the first frame update
    void Start()
    {
        
    }


    /*
     * To be filled after open question regarding standardDocument has ben resolved
  /*  public bool spawnEnemy()
    {
        if (!characterDict.ContainsKey(characterID))
            return false;

    }*/

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


}
