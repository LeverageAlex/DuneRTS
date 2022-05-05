using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomicController : MonoBehaviour
{
    public GameObject explosionPrefab;
    private int _x;
    private int _y;
    private int _z;

    private int targetX;
    private int targetZ;


    // Start is called before the first frame update
    void Start()
    {
        _x = (int) Mathf.Round(transform.position.x);
        _y = (int)Mathf.Round(transform.position.y);
        _z = (int)Mathf.Round(transform.position.z);


        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject obj = Instantiate(explosionPrefab, new Vector3(targetX, 0.5f, targetZ), Quaternion.identity);
        Debug.Log("Atomic explosion at x: " + targetX.ToString() + ", z: " + targetZ.ToString());
        //ToDo Smooth animation via animator (turn down the light intensity)
        Destroy(obj, 3.5f);
        Destroy(this);
    }

    public void SetTargetPos(int x, int z)
    {
        targetX = x;
        targetZ = z;
    }
}
