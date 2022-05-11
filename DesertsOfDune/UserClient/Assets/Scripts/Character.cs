using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Character : MonoBehaviour
{
    public string charName;
    public float walkSpeed = 3f;


    private int characterId;

    CharacterTurnHandler turnHandler;

    public CharTypeEnum characterType;
    //TODO
    //Wird nur während des erstellen von noch nicht selbstgenerierten Leveln benötigt (da so im UnityEditor gewählt werden kann).
    //Sollte später durch einfach durch eine direkte Referenz ersetzt
    //Es fehlt die Zuordnung zum Haus

    private int HP;  //Health Points
    private int healingHP;
    private int MP;  //Movement Points
    private int AP;  //Attack Points
    private int AD;  //Attack-Damage
    private int spiceInv;

    private int _x;
    private int _z;
    private float _y;

    public int X { get { return _x; } }

    public float BaseY { get { return _y; } }
    public int Z { get { return _z; } }


    private bool isLoud;
    private bool isSwallowed;

    private LinkedList<Vector3> walkPath;


    private NodeManager nodeManager;
    // public Transform t;








    // Start is called before the first frame update
    void Start()
    {
        nodeManager = NodeManager.instance;
        turnHandler = CharacterTurnHandler.instance;

        _x = (int)Mathf.Round(transform.position.x);
        _z = (int)Mathf.Round(transform.position.z);
        _y = transform.position.y;

        //SampleCode only
        initCharacter();

        //Update Nodes references on start (only needed because of editor)
        //gameManager.getNodeFromPos((int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.z)).placeObjectOnNode(gameObject);
        nodeManager.placeObjectOnNode(gameObject, (int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.z));
        //Debug.Log("HP " + HP + ", AP " + AP);
        //Debug.Log("Object name: " + gameObject.name);
    }

    /*
     * Will be called within movementManager.
     */
    public bool calledUpdate()
    {
        return MoveToPoint();

        // moveToPoint(t);
    }


    public void initCharacter()
    {
       if(characterType == CharTypeEnum.NOBLE)
        {
            UpdateCharStats(100, 10, 2, 2, 20, 5, false, false);
        } else if(characterType == CharTypeEnum.FIGHTER)
        {
            UpdateCharStats(200, 20, 2, 2, 40, 3, false, false);

        } else if(characterType== CharTypeEnum.MENTANT)
        {
            UpdateCharStats(75, 10, 2, 3, 10, 10, false, false);
        } else if(characterType==CharTypeEnum.BENEGESSERIT)
        {
            UpdateCharStats(150, 20, 3, 2, 20, 5, false, false);
        }
    }


   


    public void UpdateCharStats(int HP, int HealHP, int MP, int AP, int AD, int spiceInv, bool isLoud, bool isSwallowed)
    {
        this.HP = HP;
        this.healingHP = HealHP;
        this.MP = MP;
        this.AP = AP;
        this.AD = AD;
        this.spiceInv = spiceInv;
        this.isLoud = isLoud;
        this.isSwallowed = isSwallowed;

        Debug.Log("Updated " + gameObject.name + "s stats.");

    }

    /*
     * Moves Character to a specific point with very little speed. Needs to be called over and over again to finish animation
     * @return: True, if Character is still moving, False if last wayPoint has been reached
     */
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
            NodeManager.instance.placeObjectOnNode(gameObject, _x, _z);

            //E. g. go To next Point
            return walkPath.Count > 0;
        }
        return true;
    }

    public void SetWalkPath(LinkedList<Vector3> way)
    {
        walkPath = way;
    }

    public void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        selectChar();

    }

    public void selectChar()
    {

        if (!CharacterTurnHandler.CharSelected)  //To ADD: Check whether Character is allowed to move
        {
            turnHandler.SelectCharacter(this);
            // Debug.Log("Node set Character!");
            Debug.Log("Select new Character");
        }
        else if (turnHandler.CharState == CharacterTurnHandler.Actions.ATTACK)
        {
            turnHandler.GetSelectedCharacter().Attack_Basic(this);
        }
        else if (turnHandler.CharState == CharacterTurnHandler.Actions.KANLY)
        {
            turnHandler.GetSelectedCharacter().Attack_Kanly(this);
        }
        else if (turnHandler.CharState == CharacterTurnHandler.Actions.VOICE)
        {
            turnHandler.GetSelectedCharacter().Action_Voice(this);
        }
        else if (turnHandler.CharState == CharacterTurnHandler.Actions.TRANSFER)
        {
            turnHandler.GetSelectedCharacter().Action_TransferSpice(this);
        }


    }


    public bool Attack_Basic(Character character)
    {
        //secondCharacter = character;
        Node selectedNode = nodeManager.getNodeFromPos(turnHandler.GetSelectedCharacter().X, turnHandler.GetSelectedCharacter().Z);
        Node secondNode = nodeManager.getNodeFromPos(character.X, character.Z);


        if (nodeManager.isNodeNeighbour(selectedNode, secondNode))
        {
            //TODO execute attack
            Debug.Log("Attack");

            //reset 
            // secondCharacter = null;
            turnHandler.ResetSelection();
            return true;
        }
        else
        {
            turnHandler.ResetAction();
            Debug.Log("illegal Attack");
            return false;
        }
    }

    public bool Action_CollectSpice()
    {
        if (nodeManager.IsSpiceOn(X, Z))
        {
            nodeManager.CollectSpice(X, Z);
            Debug.Log("Collected Spice!");
        }
        else
        {
            Debug.Log("No Spice to collect!");
            return false;
        }
        turnHandler.ResetSelection();
        return true;
    }

    public bool Action_TransferSpice(Character character)
    {
        Node selectedNode = nodeManager.getNodeFromPos(turnHandler.GetSelectedCharacter().X, turnHandler.GetSelectedCharacter().Z);
        Node secondNode = nodeManager.getNodeFromPos(character.X, character.Z);

        if (nodeManager.isNodeNeighbour(selectedNode, secondNode))
        {
            //TODO execute attack
            Debug.Log("Transfer!");

            //reset 
            // secondCharacter = null;
            turnHandler.ResetSelection();
            return true;
        }
        else
        {
            turnHandler.ResetAction();
            Debug.Log("illegal Transfer!");
            return false;
        }
    }


    public bool Attack_SwordSpin()
    {
        //secondCharacter = character;
        if (characterType == CharTypeEnum.FIGHTER)
        {
            //Node selectedNode = nodeManager.getNodeFromPos(turnHandler.GetSelectedCharacter().X, turnHandler.GetSelectedCharacter().Z);
            Debug.Log("Attack_SwordSpin");
            turnHandler.ResetSelection();
            //TODO: Send Attack to Server
            return true;
        }
        else
        {
            turnHandler.ResetAction();
            Debug.Log("illegal Attack_SwordSpin");
            return false;
        }



    }

    /*
    * @param target node
    */
    public bool Attack_Atomic(Node node)
    {
        if (characterType == CharTypeEnum.NOBLE)
        {
            //Check, if there are atomics left in House

            Debug.Log("Atomic explosion at x: " + node.X.ToString() + ", z: " + node.Z.ToString());
            turnHandler.ResetSelection();

            return true;
        }
        else
        {
            turnHandler.ResetAction();
            Debug.Log("Illegal Atomic!");
            return false;
        }
    }

    /*
    * @param target of action
    */
    public bool Attack_Kanly(Character character)
    {

        if (characterType == CharTypeEnum.NOBLE && character.GetCharType() == CharTypeEnum.NOBLE)
        {
            Node selectedNode = nodeManager.getNodeFromPos(turnHandler.GetSelectedCharacter().X, turnHandler.GetSelectedCharacter().Z);
            Node secondNode = nodeManager.getNodeFromPos(character.X, character.Z);
            if (nodeManager.isNodeNeighbour(selectedNode, secondNode))
            {
                Debug.Log("Kanly fight!");
                turnHandler.ResetSelection();
                return true;
            }
            else
            {
                turnHandler.ResetAction();
                Debug.Log("Enemy too far away!");
                return false;
            }
        }
        else
        {
            turnHandler.ResetAction();
            Debug.Log("Illegal Kanly!");
            return false;
        }
    }

    public bool Action_SpiceHoarding()
    {
        //TODO Vorraussetzung zum aufsammeln prüfen?
        if (characterType == CharTypeEnum.MENTANT)
        {

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (nodeManager.IsSpiceOn(X + i, Z + j))
                    {
                        nodeManager.CollectSpice(X + i, Z + j);
                        Debug.Log("Collected Spice!");
                    }
                }
            }

            //TODO Call spice-hoarding Socket-Message and animate (Vorschlag wäre den Unity-Animator zu benutzen und dann mit einer Coroutine nach Ablauf der Animationszeit die Stats zu aktualisieren)
            turnHandler.ResetSelection();
            return true;
        }
        else
        {
            turnHandler.ResetAction();
            Debug.Log("No SpiceHoarding!");
            return false;
        }
    }

    /*
     * @param Target of Action
     */
    public bool Action_Voice(Character character)
    {
        if (characterType == CharTypeEnum.BENEGESSERIT)
        {

            Node selectedNode = nodeManager.getNodeFromPos(turnHandler.GetSelectedCharacter().X, turnHandler.GetSelectedCharacter().Z);
            Node secondNode = nodeManager.getNodeFromPos(character.X, character.Z);
            if (nodeManager.isNodeNeighbour(selectedNode, secondNode))
            {
                Debug.Log("Voice!");
                turnHandler.ResetSelection();
                return true;
            }
            else
            {
                Debug.Log("Enemy too far away!");
                turnHandler.ResetAction();
                return false;
            }
        }
        else
        {
            Debug.Log("Illegal voice!");
            turnHandler.ResetAction();
            return false;
        }
    }

    public CharTypeEnum GetCharType()
    {
        return characterType;
    }




}
