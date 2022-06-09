using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// This class is attached to the SizeDialog which is deactivated if a configuration is loaded and no
/// new congiguration should be generated.
/// </summary>

public class MapCreator : MonoBehaviour {

    public Button createButton;
    public InputField x;
    public InputField y;

    public MapEditor editor;

    void Start() {

        // If a scene should be loaded
        if (!PlayerPrefs.GetString("scenarioOpenPath").Equals("")) {
            editor.CreateMap(1, 1);
            gameObject.SetActive(false);
        }

        // If a new scene should be generated

        // default map size
        x.text = "8";
        y.text = "8";

        Button btn = createButton.GetComponent<Button>();
        btn.onClick.AddListener(CreateMap);

        x.onValidateInput += delegate (string input, int charIndex, char addedChar) { return x.text.Length < 4 && addedChar >= '0' && addedChar <= '9' ? addedChar : '\0'; };
        y.onValidateInput += delegate (string input, int charIndex, char addedChar) { return y.text.Length < 4 && addedChar >= '0' && addedChar <= '9' ? addedChar : '\0'; };
    }


    void CreateMap() {
        Debug.Log("Creating map....");

        // set map size for editor
        int.TryParse(x.text, out int sizeX);
        int.TryParse(y.text, out int sizeY);

        // hide dialog if valid map parameters
        if (editor.CreateMap(sizeX, sizeY)) {
            gameObject.SetActive(false);
        }
        // reset input fields
        else {
            x.text = "";
            y.text = "";
        }
    }
}
