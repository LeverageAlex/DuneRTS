using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spicy : MonoBehaviour
{
    public GameObject spicy;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("l") && Input.GetKey(KeyCode.LeftControl))
        {
            GameObject s = (GameObject)Instantiate(spicy);
            s.transform.position = new Vector3(Random.value * 10f, 25, Random.value * 10f);
            Destroy(s, 10);
        }
    }
}
