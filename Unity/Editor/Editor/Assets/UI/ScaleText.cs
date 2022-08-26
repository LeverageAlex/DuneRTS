using UnityEngine;
using UnityEngine.UI;

public class ScaleText : MonoBehaviour
{
    Text title;
    RectTransform rect;
    // Start is called before the first frame update
    void Start()
    {
        title = this.GetComponent<Text>();
        rect = this.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        float height = rect.rect.height;
        title.fontSize = (int)height;
    }
}
