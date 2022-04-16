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


    void OnMouseExit()
    {
        rend.material.color = startColor;
    }





}
