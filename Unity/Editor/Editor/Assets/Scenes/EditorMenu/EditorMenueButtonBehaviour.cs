using SFB;
using UnityEngine;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Linq;
using System.IO;

/// <summary>
/// This script is attached to the Canvas of the scene.
/// All methods are triggered by onCLick()-events of the buttons of the menue.
/// </summary>
public class EditorMenueButtonBehaviour : MonoBehaviour
{

    public TextAsset scenarioSchema;
    public TextAsset partySchema;
    public GameObject errorWindow;

    /// <summary>
    /// This method should be triggered by the Button that should switch to the party config editor scene
    /// </summary>
    public void newMatchConfig() {
        PlayerPrefs.SetString("matchConfigPath", "");   // Ensure, that a new template is created
        UnityEngine.SceneManagement.SceneManager.LoadScene("MatchConfigEditorScene");
    }


    /// <summary>
    /// This method should be triggered by the button that should switch to the scenario config editor scene
    /// </summary>
    public void newScenarioConfig() {
        PlayerPrefs.SetString("scenarioOpenPath", "");  // Ensure, that a new template is created
        UnityEngine.SceneManagement.SceneManager.LoadScene("ScenarioConfigEditorScene");
    }

    /// <summary>
    /// This method should be triggered by the exit button of the menue
    /// </summary>
    public void quitEditor() {
        Application.Quit(0);
    }


    /// <summary>
    /// This method should be triggered by the open cofiguration button.
    /// It gives the user the ability to choose a config file from the file system and then automatically
    /// opens up the corresponding editor to edit the configuration.
    /// </summary>
    public void LoadAConfiguration() {

        // The user can decide if the type of configuration should be filtert
        var extensions = new[] {
            new ExtensionFilter("All","scenario.json","party.json"),
            new ExtensionFilter("Scenario Configuration", "scenario.json" ),
            new ExtensionFilter("Match Configuration", "party.json" ),
        };
        //Open the file browser
        var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);
        
        // Decide what editor to use
        //Debug.Log($"Found {paths.Length} paths, 0th: {paths[0]}");
        if (paths.Length < 1) {
            Debug.Log("No path provided!");
            return;
        }

        // Load the file to check if it is valid
        string configText = File.ReadAllText(paths[0]);
        if (configText == null) {
            showLoadErrorWindow();
            return;
        }

        // Try to parse the json file
        try {
            JObject config = JObject.Parse(configText);
        

            // Load the corresponding scene and save the filepath of the config file that should be loaded 
            // in the player prefs, so that the editor scenes can load it on their own
            if (paths[0].EndsWith("party.json")) {
            
                JSchema schema = JSchema.Parse(partySchema.text);
                if (!config.IsValid(schema)) {
                    showLoadErrorWindow();
                    return;
                }
                PlayerPrefs.SetString("matchConfigPath", paths[0]);
                UnityEngine.SceneManagement.SceneManager.LoadScene("MatchConfigEditorScene");
            }
            else if (paths[0].EndsWith("scenario.json")) {
            
                JSchema schema = JSchema.Parse(scenarioSchema.text);
                if (!config.IsValid(schema)) {
                    showLoadErrorWindow();
                    return;
                }

                PlayerPrefs.SetString("scenarioOpenPath", paths[0]);
                UnityEngine.SceneManagement.SceneManager.LoadScene("ScenarioConfigEditorScene");
            }
            else {
                Debug.LogError("Opening the configuration file did not work");
            }

        }
        catch {
            showLoadErrorWindow();
            return;
        }
    }

    private void showLoadErrorWindow() {
        errorWindow.SetActive(true);
    }
}

