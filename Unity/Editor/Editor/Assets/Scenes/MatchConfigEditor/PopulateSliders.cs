using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PopulateSliders : MonoBehaviour
{
    public Sprite[] FillSprites;
    private enum SliderColor {
        RED = 0, BLUE, GREEN, YELLOW, LIGHT_RED, LIGHT_BLUE, LIGHT_GREEN, LIGHT_YELLOW
    }
    struct SliderProperties {
        public string Title;
        public Vector2 AnchorMin;
        public Vector2 AnchorMax;
        public SliderColor Color;
    }

    private SliderProperties[] properties = { 
            new SliderProperties { Title = "Health",            AnchorMin = new Vector2(0, 0.66f),      AnchorMax = new Vector2(0.5f, 1),   Color = SliderColor.RED},
            new SliderProperties { Title = "Damage",            AnchorMin = new Vector2(0, 0.33f),      AnchorMax = new Vector2(0.5f, 0.66f),   Color = SliderColor.YELLOW},
            new SliderProperties { Title = "Healing",           AnchorMin = new Vector2(0, 0.00f),      AnchorMax = new Vector2(0.5f, 0.33f),   Color = SliderColor.LIGHT_RED},
            new SliderProperties { Title = "Movement Points",   AnchorMin = new Vector2(0.5f, 0.66f),   AnchorMax = new Vector2(1, 1),      Color = SliderColor.GREEN},
            new SliderProperties { Title = "Action Points",     AnchorMin = new Vector2(0.5f, 0.33f),   AnchorMax = new Vector2(1, 0.66f),      Color = SliderColor.BLUE},
            new SliderProperties { Title = "Inventory Size",    AnchorMin = new Vector2(0.5f, 0.00f),   AnchorMax = new Vector2(1, 0.33f),      Color = SliderColor.LIGHT_GREEN},
    };

    public void InitSliders() {
        // Get Prototype Slider use the health one
        GameObject prototypeInput = transform.Find("Health").gameObject;

        // Copy it 5 times and fill in the changed properties
        foreach (var property in properties.Skip(1)) {
            GameObject nextSlider = GameObject.Instantiate(prototypeInput, this.transform);
            nextSlider.GetComponentInChildren<Text>().text = property.Title;
            nextSlider.GetComponent<RectTransform>().anchorMax = property.AnchorMax;
            nextSlider.GetComponent<RectTransform>().anchorMin = property.AnchorMin;
            Transform fill = nextSlider.transform.Find("Input").Find("Slider").Find("Fill Area").Find("Fill");
            fill.GetComponentInChildren<Image>().sprite = FillSprites[(int)property.Color];
            nextSlider.name = property.Title.Replace(" ", "");
        }

        foreach (Transform child in transform) {
            UpdateSlider updslider = child.Find("Input").GetComponent<UpdateSlider>();
            if (updslider != null) updslider.Init();
        }
            
    }
}
