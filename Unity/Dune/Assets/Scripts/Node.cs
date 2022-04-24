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

    public bool accessible = true;



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
        ResetColor();
    }


    /*
     * Selects the Character on Node in MovementManager, if there is one
     */
    public void SelectNode()
    {
        if (!accessible) return;

        if(CharacterTurnHandler.instance.CharState == CharacterTurnHandler.Actions.MOVE  && CharacterTurnHandler.CharSelected && NodeManager.instance.getObjectOnNode(this) == null)
        {
            Vector3 point = new Vector3();
            point.x = transform.position.x;
            point.y = CharacterTurnHandler.instance.GetSelectedCharacter().transform.position.y;
            point.z = transform.position.z;
            MovementManager.instance.AddWaypoint(point);
            this.rend.material.color = Color.green;
        } else if(CharacterTurnHandler.instance.CharState == CharacterTurnHandler.Actions.FAMILY_ATOMICS && CharacterTurnHandler.CharSelected)
        {
            CharacterTurnHandler.instance.GetSelectedCharacter().Attack_Atomic(this);
        }
      
 


    }


    public void ResetColor()
    {
        rend.material.color = startColor;
    }


}
