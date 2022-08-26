using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class ManageDataModel : MonoBehaviour
{
    PartieConfigModel DataModel;

    List<CharacterValuesModel> characterValues = new List<CharacterValuesModel>();

    //List<int> propertieValues = new List<int>();
    int[] propertieValues;

    UpdateSlider[] valueManager;

    CharacterValues[] characterValuesManager;

    // Start is called before the first frame update
    void Start()
    {
        characterValuesManager = GetComponentInChildren<ManageCharacterValues>().Init();
        valueManager = GetComponentInChildren<PopulateProperties>().Init();

        
        // If the matchconfigeditor was opened by 'load Config'
        if (!PlayerPrefs.GetString("matchConfigPath").Equals("")) {
            // load config Data
            StreamReader r = new StreamReader(PlayerPrefs.GetString("matchConfigPath"));
            string jsonString = r.ReadToEnd();
            Debug.Log("JSON String: " + jsonString);
            PartieObject m = JsonConvert.DeserializeObject<PartieObject>(jsonString);


            // set Values
            //UpdateSlider[] allSliders = FindObjectsOfType<UpdateSlider>();

            GameObject.Find("InputFieldBorn").GetComponent<UnityEngine.UI.InputField>().text = m.cellularAutomaton.Substring(1, 1);
            GameObject.Find("InputFieldSurvive").GetComponent<UnityEngine.UI.InputField>().text = m.cellularAutomaton.Substring(4);


            valueManager[0].Value = m.numbOfRounds;
            valueManager[1].Value = m.actionTimeUserClient;
            valueManager[2].Value = m.actionTimeAiClient;
            valueManager[3].Value = (int)Mathf.Round(m.highGroundBonusRatio*100);
            valueManager[4].Value = (int)Mathf.Round(m.lowerGroundMalusRatio*100);
            valueManager[5].Value = (int)Mathf.Round(m.kanlySuccessProbability*100);
            valueManager[6].Value = m.spiceMinimum;
            valueManager[7].Value = m.sandWormSpeed;
            valueManager[8].Value = m.sandWormSpawnDistance;
            valueManager[9].Value = (int)Mathf.Round(m.cloneProbability*100);
            valueManager[10].Value = m.minPauseTime;
            valueManager[11].Value = m.maxStrikes;
            valueManager[12].Value = (int)Mathf.Round(m.crashProbability*100);


            characterValuesManager[3].InvSize = m.fighter.inventorySize;
            characterValuesManager[3].AP = m.fighter.maxAP;
            characterValuesManager[3].MP = m.fighter.maxMP;
            characterValuesManager[3].Healing = m.fighter.healingHP;
            characterValuesManager[3].Damage = m.fighter.damage;
            characterValuesManager[3].Health = m.fighter.maxHP;

            characterValuesManager[2].InvSize = m.beneGesserit.inventorySize;
            characterValuesManager[2].AP = m.beneGesserit.maxAP;
            characterValuesManager[2].MP = m.beneGesserit.maxMP;
            characterValuesManager[2].Healing = m.beneGesserit.healingHP;
            characterValuesManager[2].Damage = m.beneGesserit.damage;
            characterValuesManager[2].Health = m.beneGesserit.maxHP;

            characterValuesManager[1].InvSize = m.mentat.inventorySize;
            characterValuesManager[1].AP = m.mentat.maxAP;
            characterValuesManager[1].MP = m.mentat.maxMP;
            characterValuesManager[1].Healing = m.mentat.healingHP;
            characterValuesManager[1].Damage = m.mentat.damage;
            characterValuesManager[1].Health = m.mentat.maxHP;

            characterValuesManager[0].InvSize = m.noble.inventorySize;
            characterValuesManager[0].AP = m.noble.maxAP;
            characterValuesManager[0].MP = m.noble.maxMP;
            characterValuesManager[0].Healing = m.noble.healingHP;
            characterValuesManager[0].Damage = m.noble.damage;
            characterValuesManager[0].Health = m.noble.maxHP;

            
            PlayerPrefs.SetString("matchConfigPath", "");
        }
    }

    
    public PartieConfigModel getDataModel() {
        foreach (CharacterValues go in characterValuesManager) {
            characterValues.Add(new CharacterValuesModel(go.Health, go.Healing, go.MP, go.AP, go.Damage, go.InvSize));
        }


        //foreach (UpdateSlider go in valueManager) {
        //    propertieValues.Add(go.Value);
            //Debug.Log(i + ": " + go.Value);
        //}

        propertieValues = new int[valueManager.Length];
        for (int i = 0; i < propertieValues.Length; i++) {
            propertieValues[i] = valueManager[i].Value;
        }
        

        float born = Mathf.Clamp(float.Parse(GameObject.Find("InputFieldBorn").GetComponent<UnityEngine.UI.InputField>().text), 0f, 8f);
        float survive = Mathf.Clamp(float.Parse(GameObject.Find("InputFieldSurvive").GetComponent<UnityEngine.UI.InputField>().text), 0f, 8f);
        //string cellularAutomaton = "S" + survive + "/B" + born;
        string cellularAutomaton = "B" + born + "/S" + survive;

        
        //                                 noble                mentant             BeneGesit           Fighter             
        DataModel = new PartieConfigModel(characterValues[0], characterValues[1], characterValues[2], characterValues[3]
            , propertieValues[0], propertieValues[1], propertieValues[2], (float)propertieValues[3]/100
            , (float)propertieValues[4]/100, (float)propertieValues[5] / 100, propertieValues[6]
            , cellularAutomaton, propertieValues[7], propertieValues[8], (float)propertieValues[9]/100
            , propertieValues[10], propertieValues[11], (float)propertieValues[12]/100);

        //Debug.Log("Low GRound Malus for research: " + propertieValues[7]);
        //Debug.Log("(float)propertieValues[7]/100: " + (float)propertieValues[7]/100);
        //Debug.Log("(float)propertieValues[7]: " + (float)propertieValues[7]);
        //Debug.Log("propertieValues[7]/100: " + propertieValues[7]/100);
        //Debug.Log("(float)(propertieValues[7]/100): " + (float)(propertieValues[7]/100));

        //CharacterValuesModel none = new CharacterValuesModel(0, 0, 0, 0, 0, 0);
        //DataModel = new PartieConfigModel(none, none, none, none, 0,0,0,0,0,0,0,"",0,0,0,0,0);


        return DataModel;

    }


}
