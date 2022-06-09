using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveMatch : MonoBehaviour
{
    public string EditorMainMenuSceneName;

    PartieConfigModel partieConfigModel;

    ManageDataModel manageDataModel;

    public void SaveAndExit() {

        manageDataModel = GameObject.FindObjectOfType<ManageDataModel>();        

        partieConfigModel = manageDataModel.getDataModel();

        if (ConfigJSONLink.JSONFromMatchConfig(partieConfigModel)) {
            SceneManager.LoadScene(EditorMainMenuSceneName);
        }

    }
}
