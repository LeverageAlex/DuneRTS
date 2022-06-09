using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class ManageDataModel : MonoBehaviour
{
    PartieConfigModel DataModel;

    List<CharacterValuesModel> characterValues = new List<CharacterValuesModel>();

    List<int> propertieValues = new List<int>();

    UpdateSlider[] valueManager;

    CharacterValues[] characterValuesManager;

    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<ManageCharacterValues>().Init();
        GetComponentInChildren<PopulateProperties>().Init();

        
        valueManager = GameObject.FindObjectsOfType<UpdateSlider>();
        characterValuesManager = GameObject.FindObjectsOfType<CharacterValues>();

        // If the matchconfigeditor was opened by 'load Config'
        if (!PlayerPrefs.GetString("matchConfigPath").Equals("")) {
            // load config Data
            StreamReader r = new StreamReader(PlayerPrefs.GetString("matchConfigPath"));
            string jsonString = r.ReadToEnd();
            Debug.Log("JSON String: " + jsonString);
            PartieObject m = JsonConvert.DeserializeObject<PartieObject>(jsonString);


            // set Values
            UpdateSlider[] allSliders = FindObjectsOfType<UpdateSlider>();

            GameObject.Find("InputFieldBorn").GetComponent<UnityEngine.UI.InputField>().text = m.cellularAutomaton.Substring(1, 1);
            GameObject.Find("InputFieldSurvive").GetComponent<UnityEngine.UI.InputField>().text = m.cellularAutomaton.Substring(4);


            /*
            allSliders[0].Value = m.minPauseTime;
            allSliders[1].Value = (int)(m.cloneProbability * 100);
            allSliders[2].Value = m.sandWormSpawnDistance;
            allSliders[3].Value = m.sandWormSpeed;
            allSliders[4].Value = m.spiceMinimum;
            allSliders[5].Value = (int)(m.kanlySuccessProbability * 100);
            allSliders[6].Value = (int)(m.lowerGroundMalusRatio * 100);
            allSliders[7].Value = (int)(m.highGroundBonusRatio*100);
            allSliders[8].Value = (int)(m.actionTime*100);
            allSliders[32].Value = m.numbOfRounds;
            


            allSliders[9].Value = m.fighter.inventorySize;
            allSliders[10].Value = m.fighter.maxAP;
            allSliders[11].Value = m.fighter.maxMP;
            allSliders[12].Value = m.fighter.healingHP;
            allSliders[13].Value = m.fighter.damage;
            allSliders[29].Value = m.fighter.maxHP;

            allSliders[14].Value = m.beneGesserit.inventorySize;
            allSliders[15].Value = m.beneGesserit.maxAP;
            allSliders[16].Value = m.beneGesserit.maxMP;
            allSliders[17].Value = m.beneGesserit.healingHP;
            allSliders[18].Value = m.beneGesserit.damage;
            allSliders[30].Value = m.beneGesserit.maxHP;

            allSliders[19].Value = m.mentat.inventorySize;
            allSliders[20].Value = m.mentat.maxAP;
            allSliders[21].Value = m.mentat.maxMP;
            allSliders[22].Value = m.mentat.healingHP;
            allSliders[23].Value = m.mentat.damage;
            allSliders[31].Value = m.mentat.maxHP;

            allSliders[24].Value = m.noble.inventorySize;
            allSliders[25].Value = m.noble.maxAP;
            allSliders[26].Value = m.noble.maxMP;
            allSliders[27].Value = m.noble.healingHP;
            allSliders[28].Value = m.noble.damage;
            allSliders[33].Value = m.noble.maxHP;
            */





            allSliders[34].Value = m.numbOfRounds;
            allSliders[10].Value = m.actionTimeUserClient;
            allSliders[9].Value = m.actionTimeAiClient;
            allSliders[8].Value = (int)(m.highGroundBonusRatio*100);
            allSliders[7].Value = (int)(m.lowerGroundMalusRatio*100);
            allSliders[6].Value = (int)(m.kanlySuccessProbability*100);
            allSliders[5].Value = m.spiceMinimum;
            allSliders[4].Value = m.sandWormSpeed;
            allSliders[3].Value = m.sandWormSpawnDistance;
            allSliders[2].Value = (int)(m.cloneProbability*100);
            allSliders[1].Value = m.minPauseTime;
            allSliders[0].Value = m.maxStrikes;


            allSliders[11].Value = m.fighter.inventorySize;
            allSliders[12].Value = m.fighter.maxAP;
            allSliders[13].Value = m.fighter.maxMP;
            allSliders[14].Value = m.fighter.healingHP;
            allSliders[15].Value = m.fighter.damage;
            allSliders[31].Value = m.fighter.maxHP;

            allSliders[16].Value = m.beneGesserit.inventorySize;
            allSliders[17].Value = m.beneGesserit.maxAP;
            allSliders[18].Value = m.beneGesserit.maxMP;
            allSliders[19].Value = m.beneGesserit.healingHP;
            allSliders[20].Value = m.beneGesserit.damage;
            allSliders[32].Value = m.beneGesserit.maxHP;

            allSliders[21].Value = m.mentat.inventorySize;
            allSliders[22].Value = m.mentat.maxAP;
            allSliders[23].Value = m.mentat.maxMP;
            allSliders[24].Value = m.mentat.healingHP;
            allSliders[25].Value = m.mentat.damage;
            allSliders[33].Value = m.mentat.maxHP;

            allSliders[26].Value = m.noble.inventorySize;
            allSliders[27].Value = m.noble.maxAP;
            allSliders[28].Value = m.noble.maxMP;
            allSliders[29].Value = m.noble.healingHP;
            allSliders[30].Value = m.noble.damage;
            allSliders[35].Value = m.noble.maxHP;



            PlayerPrefs.SetString("matchConfigPath", "");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public PartieConfigModel getDataModel() {
        foreach (CharacterValues go in characterValuesManager) {
            characterValues.Add(new CharacterValuesModel(go.Health, go.Healing, go.MP, go.AP, go.Damage, go.InvSize));
        }

        int i = 0;
        foreach (UpdateSlider go in valueManager) {
            propertieValues.Add(go.Value);
            //Debug.Log(i + ": " + go.Value);
            i++;
        }

        float born = Mathf.Clamp(float.Parse(GameObject.Find("InputFieldBorn").GetComponent<UnityEngine.UI.InputField>().text), 0f, 8f);
        float survive = Mathf.Clamp(float.Parse(GameObject.Find("InputFieldSurvive").GetComponent<UnityEngine.UI.InputField>().text), 0f, 8f);
        //string cellularAutomaton = "S" + survive + "/B" + born;
        string cellularAutomaton = "B" + born + "/S" + survive;

        
        //                                 noble                mentant             BeneGesit           Fighter             
        DataModel = new PartieConfigModel(characterValues[3], characterValues[2], characterValues[1], characterValues[0], propertieValues[34], propertieValues[10], propertieValues[9], (float)propertieValues[8]/100, (float)propertieValues[7]/100, (float)propertieValues[6] / 100, propertieValues[5], cellularAutomaton, propertieValues[4], propertieValues[3], (float)propertieValues[2]/100, propertieValues[1], propertieValues[0]);

        Debug.Log("Low GRound Malus for research: " + propertieValues[7]);
        Debug.Log("(float)propertieValues[7]/100: " + (float)propertieValues[7]/100);
        Debug.Log("(float)propertieValues[7]: " + (float)propertieValues[7]);
        Debug.Log("propertieValues[7]/100: " + propertieValues[7]/100);
        Debug.Log("(float)(propertieValues[7]/100): " + (float)(propertieValues[7]/100));

        //CharacterValuesModel none = new CharacterValuesModel(0, 0, 0, 0, 0, 0);
        //DataModel = new PartieConfigModel(none, none, none, none, 0,0,0,0,0,0,0,"",0,0,0,0,0);


        return DataModel;

    }


}
