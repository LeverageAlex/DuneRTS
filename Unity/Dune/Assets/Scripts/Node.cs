using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using GameData.network.util.world;

/**
 * Class is the functional representation of Nodes displayed on the board
 * - Checks for mouse-interactions
 * - stores informations bound to node
 */
public class Node : MonoBehaviour
{

    private Renderer rend;
    private int _X, _Z;

    public int X { get { return _X; } }
    public int Z { get { return _Z; } }

    private float offsetSpiceLowY = 0.35f;
    private float offsetSpiceHighY = 0.525f;

    public float charHeightOffset = 0f;

    public HeightLevel heightLvl = HeightLevel.low;
    public NodeTypeEnum _nodeTypeEnum;

    public NodeTypeEnum nodeTypeEnum { get { return _nodeTypeEnum; } }

    private bool marked = false;

    private Color markedPathColor = Color.green;

    private bool isInSandstorm;

    public int cityOwnerId;



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
        if (EventSystem.current.IsPointerOverGameObject()) return;


            rend.material.color = hoverColor;



    }

    private void OnMouseOver()
    {
        if (Input.GetKeyDown("l"))
        {
            if (heightLvl == HeightLevel.high)
                MapManager.instance.SpawnSpiceCrumOn(_X, offsetSpiceHighY, _Z);
            else MapManager.instance.SpawnSpiceCrumOn(_X, offsetSpiceLowY, _Z);
        }
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        SelectNode();
    }


    void OnMouseExit()
    {
        if (rend.material.color == hoverColor && !marked)
            ResetColor();
        else if(marked) this.rend.material.color = markedPathColor;
    }


    /*
     * Selects the Character on Node in MovementManager, if there is one
     */
    public void SelectNode()
    {

        if (accessible && CharacterTurnHandler.instance.CharState == CharacterTurnHandler.Actions.MOVE && CharacterTurnHandler.CharSelected /*&& MapManager.instance.getObjectOnNode(this) == null*/)
        {
            if (MovementManager.instance.IsWaypointAttachable(X, Z))
            {
                this.rend.material.color = markedPathColor;
                marked = true;
            }
            //Vector3 point = new Vector3();
            Position point = new Position(X, Z);
            //point.x = transform.position.x;
            //point.y = CharacterTurnHandler.instance.GetSelectedCharacter().BaseY + charHeightOffset;
           // point.z = transform.position.z;
            MovementManager.instance.AddWaypoint(point);

        }
        else if (CharacterTurnHandler.instance.CharState == CharacterTurnHandler.Actions.FAMILY_ATOMICS && CharacterTurnHandler.CharSelected)
        {
            CharacterTurnHandler.instance.GetSelectedCharacter().Attack_AtomicTrigger(this);
        }
        else if(CharacterTurnHandler.instance.CharState == CharacterTurnHandler.Actions.HELIPORT && nodeTypeEnum == NodeTypeEnum.HELIPORT && CharacterTurnHandler.CharSelected)
        {
            CharacterTurnHandler.instance.GetSelectedCharacter().Action_HeliportTrigger(this);
        }




    }


    public void ResetColor()
    {
        rend.material.color = startColor;
        marked = false;
    }

    public bool IsInSandstorm { get { return isInSandstorm; } }
    public void SetSandstorm(bool storm) {
        isInSandstorm = storm;
    }


}
