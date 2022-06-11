using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFadeScript : MonoBehaviour
{
    public float showTime = 3f;
    public float fadeTime = 1f;

    private float startTime;

    private Text text;
 
    
    void Start() {
        text = GetComponent<Text>();
        startTime = Time.time;
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
    }

    private void OnEnable() {
        startTime = Time.time;
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
    }

    // Update is called once per frame
    void Update()
    {
        // Deactivate if time is over
        if(startTime+showTime+fadeTime < Time.time) {
            gameObject.SetActive(false);
        }

        // Fade if show time is over
        if(startTime + showTime < Time.time) {
            float alpha = 1 - (Time.time - (startTime + showTime)) / fadeTime;
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
        }
    }
}
