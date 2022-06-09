using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * This Component synchronizes the Text and Slider values
 */
public class UpdateSlider : MonoBehaviour
{
    // Start is called before the first frame update
    private InputField InputField;
    private Slider Slider;
    //private Text Text;
    public Text Text;


    public int Value { get => _value; set { _value = value;  UpdateUI(); } }
    public int ValueMax { get => _valueMax; set { _valueMax = value; UpdateUI(); } }
    public int ValueMin { get => _valueMin; set { _valueMin = value; UpdateUI(); } }

    private int _value = 0;
    private int _valueMax = 100;
    private int _valueMin = 0;


    public void Init() {
        Slider = this.transform.Find("Slider").GetComponent<Slider>();
        Slider.wholeNumbers = true;
        InputField = this.transform.Find("InputField").GetComponent<InputField>();
        Text = this.transform.Find("InputField").Find("Text").GetComponent<Text>();
    }

    void UpdateUI() {
        Slider.minValue = ValueMin;
        Slider.maxValue = ValueMax;
        Slider.value = Value;
        InputField.text = Value.ToString();
    }



    // Update is called once per frame
    void Update()
    {
        // if slider or input text changes, update UI
        if (Slider.value != Value)
            Value = (int)Slider.value;
        else {
            int textValue;
            if (int.TryParse(Text.text, out textValue)) {
                if (textValue != Value)
                    Value = textValue;
            }
        }        
    }
}
