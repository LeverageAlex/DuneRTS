using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* Used to:
* - setup bezier curve to target atomic location
* - on destroy
*/
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

    private GameObject route;

    // Start is called before the first frame update
    void Start()
    {
        _x = (int) Mathf.Round(transform.position.x);
        _y = (int)Mathf.Round(transform.position.y);
        _z = (int)Mathf.Round(transform.position.z);


        //Set up Bezier curve points
        route = Instantiate(routePrefab, new Vector3(0, 0, 0), Quaternion.identity);

        route.transform.GetChild(0).SetPositionAndRotation(new Vector3(_x, 0.5f, _z), Quaternion.identity);
        route.transform.GetChild(1).SetPositionAndRotation(new Vector3(_x, flightHeight, _z), Quaternion.identity);
        route.transform.GetChild(2).SetPositionAndRotation(new Vector3(targetX, flightHeight, targetZ), Quaternion.identity);
        route.transform.GetChild(3).SetPositionAndRotation(new Vector3(targetX, 0.5f, targetZ), Quaternion.identity);

        //Start the movement
        follow.startRush(route.transform);


    }

    public void Destruction()
    {
        GameObject expl = Instantiate(explosionPrefab, new Vector3(targetX, 0.5f, targetZ), Quaternion.identity);
        Debug.Log("Atomic explosion at x: " + targetX.ToString() + ", z: " + targetZ.ToString());
        AudioManager.instance.StopPlaying("AtomicFly");
        AudioManager.instance.Play("AtomicExplosion");
        Destroy(expl, 3.5f);
        Destroy(gameObject);
        Destroy(route);
    }

    public void SetTargetPos(int x, int z)
    {
        targetX = x;
        targetZ = z;
    }
}
