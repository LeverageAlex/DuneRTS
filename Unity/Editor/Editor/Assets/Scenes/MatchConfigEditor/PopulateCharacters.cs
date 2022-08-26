using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateCharacters : MonoBehaviour
{
    [System.Serializable]
    public struct CharacterProperties {
        public string Title;
        public Sprite Thumbnail;
    }

    public CharacterProperties[] properties;

    private CharacterValues[] allCharacters { get; set; }


    public CharacterValues[] Init() {
        

        // Copy Noble 4 times
        GameObject prototype = transform.Find("Noble").gameObject;
        for (int i = 1; i < 4; i++) {
            GameObject newCharacter = Instantiate(prototype, this.transform);
            newCharacter.name = properties[i].Title;
            newCharacter.transform.Find("Main").gameObject.GetComponentInChildren<Text>().text = properties[i].Title;
            newCharacter.transform.Find("Main").gameObject.GetComponentInChildren<Image>().sprite = properties[i].Thumbnail;
            newCharacter.GetComponent<RectTransform>().anchorMax = new Vector2(1, (1 - 0.25f * i));
            newCharacter.GetComponent<RectTransform>().anchorMin = new Vector2(0, (0.75f - 0.25f * i));

            //transform.GetComponent<CharacterValues>().Init();
            //allCharacters[i] = newCharacter.GetComponent<CharacterValues>();
            //allCharacters[i].Init();
        }


        // Initialize every Character
        allCharacters = new CharacterValues[properties.Length];
        int iterator = 0;
        foreach (Transform t in transform) {
            allCharacters[iterator] = t.GetComponent<CharacterValues>();
            allCharacters[iterator].Init();
            iterator++;
        }
        
        return allCharacters;
    }
}
