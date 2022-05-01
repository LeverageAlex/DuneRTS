using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAbles : MonoBehaviour
{

    private int _x;
    private int _z;
    private float _y;

    public float walkSpeed = 3f;

    private LinkedList<Vector3> walkPath;
    public int X { get { return _x; } }

    public float BaseY { get { return _y; } }
    public int Z { get { return _z; } }
    // Start is called before the first frame update
    void Start()
    {
        _y = transform.position.y - NodeManager.instance.getNodeFromPos(X, Z).charHeightOffset;
        walkPath = new LinkedList<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("c"))
        {
            walkPath.AddLast(new Vector3(7, transform.position.y, 7));
            walkPath.AddLast(new Vector3(3, transform.position.y, 2));
            MovementManager.instance.addOtherToAnimate(this);
        }
    }

    public bool calledUpdate()
    {
        return MoveToPoint();
    }



    public bool MoveToPoint()
    {
        Vector3 dir = walkPath.First.Value - transform.position;
        transform.Translate(dir.normalized * walkSpeed * Time.deltaTime, Space.World);
        if (Vector3.Distance(transform.position, walkPath.First.Value) <= 0.06f)
        {
            walkPath.RemoveFirst();
            NodeManager.instance.placeObjectOnNode(gameObject, (int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.z));

            NodeManager.instance.RemoveObjectOnNode(X, Z);

            _x = (int)Mathf.Round(transform.position.x);
            _z = (int)Mathf.Round(transform.position.z);
            transform.position = new Vector3(X, transform.position.y, Z);

            //E. g. go To next Point
            return walkPath.Count > 0;
        }
        return true;
    }
}
