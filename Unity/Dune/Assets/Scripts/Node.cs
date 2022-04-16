using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Node : MonoBehaviour
{

    private Renderer rend;
    private GameObject objectOnNode;

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


    void OnMouseExit()
    {
        rend.material.color = startColor;
    }

    public GameObject getObjectOnNode()
    {
        return objectOnNode;
    }


    public bool placeObjectOnNode(GameObject obj)
    {
        if (isPlaceAble())
        {
            objectOnNode = obj;
            return true;
        }

        else return false;
    }


    /*
 * Might be calling the "Shared-Logic", if such thing will exist
 */
    public bool isPlaceAble()
    {
    if (objectOnNode == null) return true;
    return false;
    }

}
