using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
using GameData.network.messages;


/**
 * Functional representation of Character transforms:
 * - Provides Data to visualize
 * - Attack and other action-functions
 * - selection of characters
 * - Toggle Animations (without moving the object)
 */
[Serializable]
public class Character : MonoBehaviour
{

    public string charName;
    public float walkSpeed = 3f;

    public GameObject healthBar;
    public Image healthBarImage;

    //[SerializeField] this is used to serialize private fields in json format
    private int characterId;

    CharacterTurnHandler turnHandler;

    public CharTypeEnum characterType;
    public HouseEnum house = HouseEnum.VERNIUS;
    //TODO
    //Wird nur während des erstellen von noch nicht selbstgenerierten Leveln benötigt (da so im UnityEditor gewählt werden kann).
    //Sollte später durch einfach durch eine direkte Referenz ersetzt
    //Es fehlt die Zuordnung zum Haus

    private int HP;  //Health Points
    private int healingHP;
    private int BaseAP;
    private int _AP;  //Attack Points
    private int _MP; //Movement Points
    private int AD;  //Attack-Damage
    private int spiceInv;

    private int _x;
    private int _z;
    private float _y;

    public int X { get { return _x; } }

    public float BaseY { get { return _y; } }
    public int Z { get { return _z; } }

    public int MP { get { return _MP; } }

    public int AP { get { return _AP; } }

    public int CharacterId { get { return characterId; } }


    private bool isLoud;
    private bool isSwallowed;

    private List<Vector3> walkPath;


    private MapManager nodeManager;
    public AudioController audioManager;

    public GameObject emblemLogo;
    public GameObject charModel;
    public enum charSexEnum
    {
        MALE, FEMALE
    }
    public charSexEnum charSex = charSexEnum.MALE;

    private Animator charAnim;
    Quaternion emblem_rotation;
    Quaternion healthBar_rotation;

    private string animation_idle;
    private string animation_attack;
    private string animation_pickUpSpice;
    private string animation_walk;
    private string animation_voice;
    private string animation_kanly;
    private string animation_swordSpin;
    private string animation_spiceHoarding;
    private string animation_transferSpice;
    private string animation_damage;
    private string animation_death;

    public GameObject swordObject;

    private int BaseHP;


    // public Transform t;








    // Start is called before the first frame update
    void Start()
    {
        nodeManager = MapManager.instance;
        turnHandler = CharacterTurnHandler.instance;

        _x = (int)Mathf.Round(transform.position.x);
        _z = (int)Mathf.Round(transform.position.z);
        _y = transform.position.y - nodeManager.getNodeFromPos(X, Z).charHeightOffset;

        //SampleCode only
        BaseHP = 100;
        initCharacter();

        //Update Nodes references on start (only needed because of editor)
        //gameManager.getNodeFromPos((int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.z)).placeObjectOnNode(gameObject);
        nodeManager.placeObjectOnNode(gameObject, (int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.z));
        //Debug.Log("HP " + HP + ", AP " + AP);
        //Debug.Log("Object name: " + gameObject.name);
        BaseAP = _AP;
        emblem_rotation = emblemLogo.transform.rotation;
        healthBar_rotation = healthBar.transform.rotation;

        charAnim = charModel.GetComponent<Animator>();
        initAnimations();
        audioManager = AudioController.instance;
    }

    public void initAnimations()
    {
        if (charSex == charSexEnum.MALE) {
            animation_idle = "Male Sword Stance";
            animation_attack = "Male Attack 1";
            animation_pickUpSpice = "Male Attack 3";
            animation_walk = "Male_Walk";
            animation_swordSpin = "";
            animation_kanly = "Male Attack 3";
            animation_spiceHoarding = "Male Sword Roll";
            animation_transferSpice = "Male Attack 2";
            animation_damage = "Male Damage Light";
            animation_death = "Male Die";
        }
        else
        {
            animation_idle = "Female Sword Stance";
            animation_attack = "Female Sword Attack 2";
            animation_pickUpSpice = "Female Sword Attack 3";
            animation_walk = "Female Sword Walk";
            animation_voice = "Female Sword Attack 3";
            animation_transferSpice = "Female Sword Attack 3";
            animation_damage = "Female Damage Light";
            animation_death = "Female Die";
        }
        




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
            UpdateCharStats(100, 100, 2, 5, false, false);
        } else if(characterType == CharTypeEnum.FIGHTER)
        {
            UpdateCharStats(200, 100, 3, 3, false, false);

        } else if(characterType== CharTypeEnum.MENTANT)
        {
            UpdateCharStats(75, 100, 2, 10, false, false);
        } else if(characterType==CharTypeEnum.BENEGESSERIT)
        {
            UpdateCharStats(150, 100, 2, 5, false, false);
        }

        SetMatColorToHouse();
    }


   


    public void UpdateCharStats(int HP, int MP, int AP, int spiceInv, bool isLoud, bool isSwallowed)
    {
        this.HP = HP;
        //this.healingHP = HealHP;
        this._AP = AP;
        this._MP = MP;
        this.spiceInv = spiceInv;
        this.isLoud = isLoud;
        this.isSwallowed = isSwallowed;

        healthBarImage.fillAmount = (float) HP / BaseHP;

        if (turnHandler != null && turnHandler.GetSelectedCharacter() == this)
        {
            DrawStats();
        }

        Debug.Log("Updated " + gameObject.name + "s stats.");
    }

    /*
     * Moves Character to a specific point with very little speed. Needs to be called over and over again to finish animation
     * @return: True, if Character is still moving, False if last wayPoint has been reached
     */
    public bool MoveToPoint()
    {
        Vector3 dir = walkPath[0] - transform.position;
        transform.Translate(dir.normalized * walkSpeed * Time.deltaTime, Space.World);
        charAnim.Play(animation_walk);
        RotateTowardsVector(dir);
        turnHandler.updateSelectionArrow();
        // ReduceMP(1);
        if (Vector3.Distance(transform.position, walkPath[0]) <= 0.06f)
        {
            walkPath.RemoveAt(0);
            MapManager.instance.placeObjectOnNode(gameObject, (int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.z));

            MapManager.instance.RemoveObjectOnNode(X, Z);

            _x = (int)Mathf.Round(transform.position.x);
            _z = (int)Mathf.Round(transform.position.z);
            transform.position = new Vector3(X, transform.position.y, Z);
            MapManager.instance.placeObjectOnNode(gameObject, _x, _z);

            //E. g. go To next Point
            if(walkPath.Count > 0)
            {
                return true;
            }
            else
            {
                SetAnimationToIdle();
                audioManager.StopPlaying("CharWalk");
                return false;
            }
        }
        return true;
    }

    public void SetWalkPath(List<Vector3> way)
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
            turnHandler.GetSelectedCharacter().Attack_BasicTrigger(this);
        }
        else if (turnHandler.CharState == CharacterTurnHandler.Actions.KANLY)
        {
            turnHandler.GetSelectedCharacter().Attack_KanlyTrigger(this);
        }
        else if (turnHandler.CharState == CharacterTurnHandler.Actions.VOICE)
        {
            turnHandler.GetSelectedCharacter().Action_VoiceTrigger(this);
        }
        else if (turnHandler.CharState == CharacterTurnHandler.Actions.TRANSFER)
        {
            turnHandler.GetSelectedCharacter().Action_TransferSpiceTrigger(this);
        }
        else if (turnHandler.CharState == CharacterTurnHandler.Actions.FAMILY_ATOMICS)
        {
            CharacterTurnHandler.instance.GetSelectedCharacter().Attack_AtomicTrigger(nodeManager.getNodeFromPos(X, Z));
        }


        }


    public bool Attack_BasicTrigger(Character character)
    {
        //secondCharacter = character;
        Node selectedNode = nodeManager.getNodeFromPos(turnHandler.GetSelectedCharacter().X, turnHandler.GetSelectedCharacter().Z);
        Node secondNode = nodeManager.getNodeFromPos(character.X, character.Z);
        

        if (nodeManager.isNodeNeighbour(selectedNode, secondNode) && !character.IsMemberOfHouse(house))
        {

            //PlayerMessageController.DoActionRequest(1234, characterId, Enums.ActionType.ATTACK, selectedNode);
            // TODO wait for Server response.
            //TODO execute attack
            if (Mode.debugMode)
            {
                Attack_BasicExecution(character);
            }
            else
            {
                SessionHandler.messageController.DoRequestAction(0, characterId, ActionType.ATTACK, new GameData.network.util.world.Position(X, Z));
            }
            return true;
        }
        else
        {
            turnHandler.ResetAction();
            Debug.Log("illegal Attack");
            return false;
        }
    }

    public void Attack_BasicExecution(Character character)
    {
        Vector3 dir = character.transform.position - transform.position;
        RotateTowardsVector(dir);
        charAnim.Play(animation_attack);
        StartCoroutine(character.PlayDamageAnimation(this));
        audioManager.Play("SwordStab");
        if (Mode.debugMode)
        {
            if (_AP <= 0) CharacterTurnHandler.EndTurn();
        }

        Debug.Log("Attack");

        //reset 
        // secondCharacter = null;
        turnHandler.ResetSelection();

    }

    public bool Action_CollectSpiceTrigger()
    {
        if (nodeManager.IsSpiceOn(X, Z))
        {
            //PlayerMessageController.DoActionRequest(1234, characterId, Enums.ActionType.COLLECT, nodeManager.getNodeFromPos(X, Z));
            // just fill data the node should be available here.
            if (Mode.debugMode)
            {
                Action_CollectSpiceExecution();
            }
            else {
                SessionHandler.messageController.DoRequestAction(0, characterId, ActionType.COLLECT, new GameData.network.util.world.Position(X, Z));
            }

        }
        else
        {
            Debug.Log("No Spice to collect!");
            return false;
        }
        turnHandler.ResetSelection();
        return true;
    }

    public void Action_CollectSpiceExecution()
    {
        StartCoroutine(SwordDeAndActivation());
        charAnim.Play(animation_pickUpSpice);
        nodeManager.CollectSpice(X, Z);
        audioManager.Play("SpicePickup");
        Debug.Log("Collected Spice!");
        if (Mode.debugMode)
        {
            if (_AP <= 0) CharacterTurnHandler.EndTurn();
        }
    }

    public bool Action_TransferSpiceTrigger(Character character)
    {
        Node selectedNode = nodeManager.getNodeFromPos(turnHandler.GetSelectedCharacter().X, turnHandler.GetSelectedCharacter().Z);
        Node secondNode = nodeManager.getNodeFromPos(character.X, character.Z);

        if (nodeManager.isNodeNeighbour(selectedNode, secondNode) && character.IsMemberOfHouse(house))
        {
            //PlayerMessageController.DoActionRequest(1234, characterId, Enums.ActionType.TRANSFER, selectedNode);
            //TODO execute attack
            if (Mode.debugMode)
            {
                Action_TransferSpiceExecution(character);
            }
            else { 
                
            }
            return true;
        }
        else
        {
            turnHandler.ResetAction();
            Debug.Log("illegal Transfer!");
            return false;
        }
    }

    public void Action_TransferSpiceExecution(Character character)
    {
        Debug.Log("Transfer!");
        Vector3 dir = character.transform.position - transform.position;
        RotateTowardsVector(dir);
        StartCoroutine(SwordDeAndActivation());
        charAnim.Play(animation_transferSpice);
        if (Mode.debugMode)
        {
            if (_AP <= 0) CharacterTurnHandler.EndTurn();
        }
        //reset 
        // secondCharacter = null;
        turnHandler.ResetSelection();
    }


    public bool Attack_SwordSpinTrigger()
    {
        //secondCharacter = character;
        if (characterType == CharTypeEnum.FIGHTER)
        {
            //Node selectedNode = nodeManager.getNodeFromPos(turnHandler.GetSelectedCharacter().X, turnHandler.GetSelectedCharacter().Z);

            // just fill data the node has to be a parameter of Atack_SwordSpin

            //PlayerMessageController.DoActionRequest(1234, characterId, Enums.ActionType.SWORD_SPIN, nodeManager.getNodeFromPos(X,Z));
            if (Mode.debugMode)
            {
                Attack_SwordSpinExecution();
            }
            else { 
            
            }
            //TODO: Send Attack to Server
            //TODO: wait for response from server
           
            return true;
        }
        else
        {
            turnHandler.ResetAction();
            Debug.Log("illegal Attack_SwordSpin");
            return false;
        }



    }

    public void Attack_SwordSpinExecution()
    {
        Debug.Log("Attack_SwordSpin");
        turnHandler.ResetSelection();
        charAnim.Play(animation_swordSpin);

        if (Mode.debugMode)
        {
            CharacterTurnHandler.EndTurn();
        }
        
    }

    /*
    * @param target node
    */
    public bool Attack_AtomicTrigger(Node node)
    {
        if (characterType == CharTypeEnum.NOBLE)
        {
            //Check, if there are atomics left in House

            //PlayerMessageController.DoActionRequest(1234, characterId, Enums.ActionType.FAMILY_ATOMICS, node);
            if (Mode.debugMode)
            {
                Attack_AtomicExecution(node);
            }
            else
            {

            }
            return true;
        }
        else
        {
            turnHandler.ResetAction();
            Debug.Log("Illegal Atomic!");
            return false;
        }
    }

    public void Attack_AtomicExecution(Node node)
    {
        GameObject atomicInst = Instantiate(CharacterMgr.instance.atomicPrefab, new Vector3(X, 0.5f, Z), Quaternion.identity);
        ((AtomicController)atomicInst.GetComponent(typeof(AtomicController))).SetTargetPos(node.X, node.Z);
        audioManager.Play("AtomicFly");
        Debug.Log("Created Atomic");
        turnHandler.ResetSelection();
        if (Mode.debugMode)
        {
            if (_AP <= 0) CharacterTurnHandler.EndTurn();
        }
        //CharacterTurnHandler.EndTurn();
    }

    /*
    * @param target of action
    */
    public bool Attack_KanlyTrigger(Character character)
    {

        if (characterType == CharTypeEnum.NOBLE && character.GetCharType() == CharTypeEnum.NOBLE && !character.IsMemberOfHouse(house))
        {
            Node selectedNode = nodeManager.getNodeFromPos(X, Z);
            Node secondNode = nodeManager.getNodeFromPos(character.X, character.Z);
            if (nodeManager.isNodeNeighbour(selectedNode, secondNode))
            {
                //PlayerMessageController.DoActionRequest(1234, characterId, Enums.ActionType.KANLY, secondNode);
                if (Mode.debugMode)
                {
                    Attack_KanlyExecution(character);
                }
                else { 
                
                }
                return true;
            }
            else
            {
                turnHandler.ResetAction();
                Debug.Log("Enemy too far away or no Enemy!");
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

    public void Attack_KanlyExecution(Character character)
    {
        Debug.Log("Kanly fight!");
        Vector3 dir = character.transform.position - transform.position;
        RotateTowardsVector(dir);
        charAnim.Play(animation_kanly);
        StartCoroutine(character.PlayDamageAnimation(this));
        turnHandler.ResetSelection();
        if (_AP <= 0) CharacterTurnHandler.EndTurn();
    }

    public bool Action_SpiceHoardingTrigger()
    {
        //TODO Vorraussetzung zum aufsammeln prüfen?
        if (characterType == CharTypeEnum.MENTANT)
        {

            // just fill data the selected node should be available here.
            //PlayerMessageController.DoActionRequest(1234, characterId, Enums.ActionType.SPICE_HORDING, nodeManager.getNodeFromPos(X, Z));
            if (Mode.debugMode)
            {
                Action_SpiceHoardingExecution();
            }
            else { 
            
            }
            return true;
        }
        else
        {
            turnHandler.ResetAction();
            Debug.Log("No SpiceHoarding!");
            return false;
        }
    }

    public void Action_SpiceHoardingExecution()
    {
        charAnim.Play(animation_spiceHoarding);
        audioManager.Play("SpiceHoarding");

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (nodeManager.IsSpiceOn(X + i, Z + j))
                {
                    nodeManager.CollectSpice(X + i, Z + j);
                    Debug.Log("Collected Spice!");
                    CharacterTurnHandler.EndTurn();
                }
            }
        }

        turnHandler.ResetSelection();
    }

    /*
     * @param Target of Action
     */
    public bool Action_VoiceTrigger(Character character)
    {
        if (characterType == CharTypeEnum.BENEGESSERIT)
        {

            Node selectedNode = nodeManager.getNodeFromPos(turnHandler.GetSelectedCharacter().X, turnHandler.GetSelectedCharacter().Z);
            Node secondNode = nodeManager.getNodeFromPos(character.X, character.Z);
            if (nodeManager.isNodeNeighbour(selectedNode, secondNode))
            {

                //PlayerMessageController.DoActionRequest(1234, characterId, Enums.ActionType.VOICE, selectedNode);
                //TODO: wait for response from server
                if (Mode.debugMode)
                {
                    Action_VoiceExecution(character);
                }
                else
                {

                }
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

    public void Action_VoiceExecution(Character character)
    {
        Vector3 dir = character.transform.position - transform.position;
        RotateTowardsVector(dir);
        charAnim.Play(animation_voice);
        Debug.Log("Voice!");


        turnHandler.ResetSelection();
        CharacterTurnHandler.EndTurn();
    }

    public void Action_HeliportTrigger(Node targetNode)
    {
        Node startNode = nodeManager.getNodeFromPos(X, Z);

    }

    public void Action_HeliportExecution()
    {

    }


    public IEnumerator SwordDeAndActivation()
    {
        swordObject.SetActive(false);
        yield return new WaitForSeconds(1);
        if (charSex == charSexEnum.MALE)
        {
            swordObject.SetActive(true);
        }

    }

    public CharTypeEnum GetCharType()
    {
        return characterType;
    }

    public bool isEligibleForSpecialAction()
    {
        return (BaseAP <= _AP);  
    }

    public bool HasAP()
    {
        return _AP > 0;
    }

    public void SetMatColorToHouse()
    {
       // Color col = Color.gray;
        Sprite img = null;
        switch(house)
        {
            case HouseEnum.CORRINO: //Gold
               // col = new Color(255, 215, 0);
                img = CharacterMgr.instance.emblemCorinno;
                break;
            case HouseEnum.ATREIDES:
              //  col = Color.green;
                img = CharacterMgr.instance.emblemAtreides;
                break;
            case HouseEnum.HARKONNEN:
               // col = Color.red;
                img = CharacterMgr.instance.emblemHarkonnen;
                break;
            case HouseEnum.ORDOS:
                //col = Color.blue;
                img = CharacterMgr.instance.emblemOrdos;
                break;
            case HouseEnum.RICHESE: //Silver
                //col = new Color(192, 192, 192);
                img = CharacterMgr.instance.emblemRichese;
                break;
            case HouseEnum.VERNIUS:
                //col = new Color(128, 0, 128); //Purple
                img = CharacterMgr.instance.emblemVernius;
                break;
        }
        //GetComponent<Renderer>().material.color = col;
        ((Image)(emblemLogo.GetComponent(typeof(Image)))).sprite = img;
    }

    public bool IsMemberOfHouse(HouseEnum houseEnum)
    {
        return houseEnum == house;
    }

    /// <summary>
    /// Plays Damage Animation and rotates against attacker
    /// </summary>
    /// <param name="character">Attacker</param>
    public IEnumerator PlayDamageAnimation(Character character)
    {
        Vector3 dir = character.transform.position - transform.position;
        yield return new WaitForSeconds(0.25f);
        RotateTowardsVector(dir);
        charAnim.Play(animation_damage);
    }

    public void OnDeath()
    {
        charAnim.Play(animation_death);
        Destroy(gameObject, 1f);
    }

    /*
     * Is used to update and set the values of the Char-Stats HUD
     */
    public void DrawStats()
    {
        GUIHandler.UpdateHP(HP);
        GUIHandler.UpdateAP(_AP);
        GUIHandler.UpdateMP(_MP);
        GUIHandler.UpdateCharSpice(spiceInv);
    }

    public void RotateTowardsVector(Vector3 dir)
    {
        transform.rotation = Quaternion.LookRotation(dir);
        emblemLogo.transform.rotation = emblem_rotation;
        healthBar.transform.rotation = healthBar_rotation;
    }
 

    public void SetAnimationToIdle()
    {
        charAnim.Play(animation_idle);
    }

    public void setMaxAP(int maxAP)
    {
        BaseAP = maxAP;
    }

    public void setMaxHP(int maxHP)
    {
        BaseHP = maxHP;
    }

    


}
