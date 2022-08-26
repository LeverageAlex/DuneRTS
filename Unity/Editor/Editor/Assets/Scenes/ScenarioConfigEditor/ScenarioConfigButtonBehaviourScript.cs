using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// This class contains all the methods that are needed to exit the scenario editor scene
/// </summary>
public class ScenarioConfigButtonBehaviourScript : MonoBehaviour
{
    public string EditorMainMenuSceneName;

    public GameObject mapEditor;
    public GameObject Panel;

    public GameObject citiesMissingText;

    private MapModel mapModel;

    /// <summary>
    /// This method starts the process of creating and saving the config file.
    /// The method is triggered by the save button
    /// </summary>
    public void SaveAndExit() {
        mapModel = mapEditor.GetComponent<MapEditor>().mapModel;
        if(getAmountOfCities(mapModel) != 2) {
            citiesMissingText.SetActive(true);
            return;
        }

        if (ConfigJSONLink.JSONFromMapModel(mapModel, null)) {
            SceneManager.LoadScene(EditorMainMenuSceneName);
        }
    }

    /// <summary>
    /// This method opens up a panel that asks the user if he wants to exit without saving the file
    /// </summary>
    public void exit() {
        if (Panel != null) {
            Panel.SetActive(true);
        }
    }

    /// <summary>
    /// This methods defines how the program should react based on the users choice made in the panel
    /// </summary>
    /// <param name="Option">Is true if the user wants to leave without saving the configuration</param>
    public void exitDialog(bool Option) {
        if (Option) {
            SceneManager.LoadScene(EditorMainMenuSceneName);
        }
        else {
            Panel.SetActive(false);
        }
    }


    private int getAmountOfCities(MapModel mapModel) {
        int count = 0;

        for(int x=0; x<mapModel.getDimensionX(); x++) {
            for (int y = 0; y < mapModel.getDimensionY(); y++) {
                if(mapModel.getTileAtPosition(x,y).fieldType == FieldType.CITY) {
                    count++;
                }
            }   
        }
        return count;
    }
}
