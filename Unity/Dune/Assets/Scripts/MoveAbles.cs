using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * This class can be bound to Objects in order to make them moveable along a given Path. Works in connection with the MovementManager
 */
public class MoveAbles : MonoBehaviour
{

    private int _x;
    private int _z;
    private float _y;

    public float walkSpeed = 3f;
    public int rotationOffset = 90;

    private LinkedList<Vector3> walkPath;
    public int X { get { return _x; } }

    public float BaseY { get { return _y; } }
    public int Z { get { return _z; } }
    // Start is called before the first frame update
    void Start()
    {
        _y = transform.position.y - MapManager.instance.getNodeFromPos(X, Z).charHeightOffset;
        if(walkPath == null)
        walkPath = new LinkedList<Vector3>();
    }


    public bool calledUpdate()
    {
        return MoveToPoint();
    }

    public void WalkAlongPath(LinkedList<Vector3> walkPath)
    {
        this.walkPath = new LinkedList<Vector3>(walkPath);
        MovementManager.instance.addOtherToAnimate(this);
    }



    /*
     * Will be called every frame to move towards points in walkpath
     * @return: whether movement is finished or needs to be recalled again
     */
    public bool MoveToPoint()
    {
        Vector3 dir = walkPath.First.Value - transform.position;
        //Rotate Object towards movement direction
        transform.rotation = Quaternion.LookRotation(dir);
        transform.Rotate(Vector3.right, rotationOffset);

        transform.Translate(dir.normalized * walkSpeed * Time.deltaTime, Space.World);
        if (Vector3.Distance(transform.position, walkPath.First.Value) <= 0.06f)
        {
            walkPath.RemoveFirst();
            //NodeManager.instance.placeObjectOnNode(gameObject, (int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.z));

            //NodeManager.instance.RemoveObjectOnNode(X, Z);

            _x = (int)Mathf.Round(transform.position.x);
            _z = (int)Mathf.Round(transform.position.z);
            transform.position = new Vector3(X, transform.position.y, Z);

            //E. g. go To next Point
            return walkPath.Count > 0;
        }
        return true;
    }
}
