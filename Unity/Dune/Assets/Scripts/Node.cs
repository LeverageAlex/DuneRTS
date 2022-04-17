using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Node : MonoBehaviour
{

    private Renderer rend;
    private int _X, _Z;

    public int X { get { return _X; } }
    public int Z { get { return _Z; } }

    


    public enum HeightLevel
    {
        high, low
    }

    private Color startColor;
    public Color hoverColor = Color.red;

    bool accessible;



    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;

        _X = (int)transform.position.x;
        _Z = (int)transform.position.z;

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnMouseEnter()
    {
        //triggers if there is an Object above the Node
      /*  if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }*/

        rend.material.color = hoverColor;
    }

    private void OnMouseDown()
    {
        SelectNode();
    }


    void OnMouseExit()
    {
        if(rend.material.color == hoverColor)
        rend.material.color = startColor;
    }


    /*
     * Selects the Character on Node in MovementManager, if there is one
     */
    public void SelectNode()
    {
        if (!MovementManager.charSelected && GameManager.instance.getObjectOnNode(this) != null)  //To ADD: Check whether Character is allowed to move
        {
            //Select Character
            Character localChar = (Character)GameManager.instance.getObjectOnNode(this).GetComponent(typeof(Character));
            MovementManager.instance.selectCharacter(localChar);
           // Debug.Log("Node set Character!");
        }
        else if(MovementManager.charSelected && GameManager.instance.getObjectOnNode(this) == null)
        {
            //create Path to walk for Character
            //Change MaterialColor to green
            Vector3 point = new Vector3();
            point.x = transform.position.x;
            point.y = MovementManager.instance.getSelectedChar().transform.position.y;
            point.z = transform.position.z;
            MovementManager.instance.AddWaypoint(point);
            rend.material.color = Color.green;
      //      Debug.Log("MovementManager changed ");
        }

    }





}
