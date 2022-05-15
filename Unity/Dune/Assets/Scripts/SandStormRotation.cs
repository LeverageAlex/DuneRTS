using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandStormRotation : MonoBehaviour
{
    public GameObject pointToRotate;
    public float rotationSpeed = 100;

    // Update is called once per frame
    void Update()
    {
        pointToRotate.transform.Rotate(new Vector3(0, rotationSpeed*Time.deltaTime, 0), Space.World);
    }
}
