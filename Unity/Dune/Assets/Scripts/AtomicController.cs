using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomicController : MonoBehaviour
{
    public GameObject explosionPrefab;
    public GameObject routePrefab;
    public Follow follow;
    private int _x;
    private int _y;
    private int _z;

    public float flightHeight = 7f;

    private int targetX;
    private int targetZ;


    // Start is called before the first frame update
    void Start()
    {
        _x = (int) Mathf.Round(transform.position.x);
        _y = (int)Mathf.Round(transform.position.y);
        _z = (int)Mathf.Round(transform.position.z);

        GameObject route = Instantiate(routePrefab, new Vector3(0, 0, 0), Quaternion.identity);

        route.transform.GetChild(0).SetPositionAndRotation(new Vector3(_x, 0.5f, _z), Quaternion.identity);
        route.transform.GetChild(1).SetPositionAndRotation(new Vector3(_x, flightHeight, _z), Quaternion.identity);
        route.transform.GetChild(2).SetPositionAndRotation(new Vector3(targetX, flightHeight, targetZ), Quaternion.identity);
        route.transform.GetChild(3).SetPositionAndRotation(new Vector3(targetX, 0.5f, targetZ), Quaternion.identity);


        follow.startRush(route.transform);


        Destroy(this, 15f);

    }

    public void Destruction()
    {
        GameObject expl = Instantiate(explosionPrefab, new Vector3(targetX, 0.5f, targetZ), Quaternion.identity);
        Debug.Log("Atomic explosion at x: " + targetX.ToString() + ", z: " + targetZ.ToString());
        Destroy(expl, 3.5f);
        Destroy(gameObject);
    }

    public void SetTargetPos(int x, int z)
    {
        targetX = x;
        targetZ = z;
    }
}
