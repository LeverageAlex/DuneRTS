using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnWorld : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject Nodes;
    public GameObject[] chars;
    void Start()
    {
        Nodes.SetActive(false);
        foreach(var charac in chars) {
            charac.SetActive(false);
        }
        Destroy(gameObject);
    }

}
