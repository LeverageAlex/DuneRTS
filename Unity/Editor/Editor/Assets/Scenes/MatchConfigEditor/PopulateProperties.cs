using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Populate the properties panel on the right side
 */
public class PopulateProperties : MonoBehaviour
{

    [System.Serializable]
    public struct PropertyProperty {
        public string Title;
        public string Unit;
        public int DefaultValue;
        public int MinValue;
        public int MaxValue;
    }

    public PropertyProperty[] properties;

    // The relative hight every element takes from the whole panel
    //0,9 / amount of Properties
    private float relativeHeight;


    public void Init() {
        relativeHeight = 0.91f / properties.Length;

        // First entry (Game Of Life)
        SetAnchors(transform.Find("GameOfLife").gameObject, 0);

        // Second enty (Prototype)
        GameObject prototype = transform.Find("NumberOfRounds").gameObject;
        prototype.transform.Find("Input").GetComponent<UpdateSlider>().Init();
        SetValues(prototype, properties[0]);
        SetAnchors(prototype, 1);

        // List of all other properties (copies of the first)
        for (int i = 1; i < properties.Length; i++) {
            GameObject newInput = Instantiate(prototype, this.transform);
            newInput.transform.Find("Input").GetComponent<UpdateSlider>().Init();
            SetValues(newInput, properties[i]);
            SetAnchors(newInput, i + 1);
        }
    }

    void SetValues(GameObject go, PropertyProperty property) {
        go.name = property.Title.Replace(" ", "");
        go.transform.Find("Title").GetComponent<Text>().text = property.Title;
        UpdateSlider valueManager = go.GetComponentInChildren<UpdateSlider>();
        valueManager.ValueMin = property.MinValue;
        valueManager.ValueMax = property.MaxValue;
        valueManager.Value = property.DefaultValue;
        //Debug.Log("NUmber of Rounds Value: " + property.DefaultValue);
        go.transform.Find("Input").Find("Unit").GetComponent<Text>().text = property.Unit;
    }
    void SetAnchors(GameObject go, int position) {
        go.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1 - relativeHeight * position);
        go.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1 - relativeHeight * (position + 1));
    }
}
