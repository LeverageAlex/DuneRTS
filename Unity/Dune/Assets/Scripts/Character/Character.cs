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
    public int characterId { get; set; }

    CharacterTurnHandler turnHandler;

    public CharTypeEnum characterType;
    public HouseEnum house = HouseEnum.VERNIUS;

    private int HP;  //Health Points
    private int BaseAP;
    private int _AP;  //Attack Points
    private int _MP; //Movement Points
    private int spiceInv;

    private bool isDead = false;
    public static int semaphoreWalk { get; set; }

    private int _x;
    private int _z;
    private float _y;

    public int X { get { return _x; } }

    public float BaseY { get { return _y; } }
    public int Z { get { return _z; } }

    public int MP { get { return _MP; } }

    public int AP { get { return _AP; } }

    //public int CharacterId { get { return characterId; } }


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
    private string animation_spiceHoarding;
    private string animation_transferSpice;
    private string animation_damage;
    private string animation_death;

    public GameObject swordObject;

    private int BaseHP;


    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    void Start()
    {
        nodeManager = MapManager.instance;
        turnHandler = CharacterTurnHandler.instance;

        _x = (int)Mathf.Round(transform.position.x);
        _z = (int)Mathf.Round(transform.position.z);
        _y = transform.position.y - nodeManager.getNodeFromPos(X, Z).charHeightOffset;
        nodeManager.placeObjectOnNode(gameObject, (int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.z));
        BaseAP = _AP;
        emblem_rotation = emblemLogo.transform.rotation;
        healthBar_rotation = healthBar.transform.rotation;

        charAnim = charModel.GetComponent<Animator>();
        initAnimations();
        audioManager = AudioController.instance;
        SetMatColorToHouse();
    }


    /// <summary>
    /// Sets the animation-names. Needed because of the distinct animation-names of models.
    /// </summary>
    public void initAnimations()
    {
        if (charSex == charSexEnum.MALE) {
            animation_idle = "Male Sword Stance";
            animation_attack = "Male Attack 1";
            animation_pickUpSpice = "Male Attack 3";
            animation_walk = "Male_Walk";
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

    /// <summary>
    /// Will be called within movementManager to move the character by some pixels
    /// </summary>
    /// <returns></returns>
    public bool calledUpdate()
    {
        return MoveToPoint();

    }



    /// <summary>
    /// Triggers death-animation and removes character from dictionary.
    /// </summary>
    public void KillCharacter()
    {
        isDead = true;
        CharacterMgr.instance.removeCharacter(characterId);
        charAnim.Play(animation_death);
        Destroy(gameObject, 1.5f);
    }

   

    /// <summary>
    /// Updates the stats of the character. If hp <= 0 or isSwallowed == true then calls KillCharacter()
    /// </summary>
    /// <param name="HP"></param>
    /// <param name="MP"></param>
    /// <param name="AP"></param>
    /// <param name="spiceInv"></param>
    /// <param name="isLoud"></param>
    /// <param name="isSwallowed"></param>
    public void UpdateCharStats(int HP, int MP, int AP, int spiceInv, bool isLoud, bool isSwallowed)
    {
        if (HP > 0 && !isSwallowed)
        {
            this.HP = HP;
            this._AP = AP;
            this._MP = MP;
            this.spiceInv = spiceInv;
            this.isLoud = isLoud;
            this.isSwallowed = isSwallowed;

            healthBarImage.fillAmount = (float)HP / BaseHP;

            if (turnHandler != null && turnHandler.GetSelectedCharacter() == this)
            {
                DrawStats();
            }

            Debug.Log("Updated " + gameObject.name + "s stats.");
        }
        else
        {
            //Killing Character
            Debug.Log("Removed Character at x: " + this.X + ", Z: " + this.Z);
            KillCharacter();
            }
    }

    /// <summary>
    /// Moves Character to a specific point with very little speed. Needs to be called over and over again to finish animation.
    /// </summary>
    /// <returns>True, if Character is still moving, False if last wayPoint has been reached</returns>
    public bool MoveToPoint()
    {
        Vector3 dir = walkPath[0] - transform.position;
        transform.Translate(dir.normalized * walkSpeed * Time.deltaTime, Space.World);
        charAnim.Play(animation_walk);
        RotateTowardsVector(dir);
        turnHandler.updateSelectionArrow();
        if (Vector3.Distance(transform.position, walkPath[0]) <= 0.06f)
        {

            //The character reached a new point in walkPath. Now remove the point from list and update position of character.
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
                //there is no path to go, so stop animation and return false.
                SetAnimationToIdle();
                semaphoreWalk--;
                //Needed for check if eligible for Heliport
                turnHandler.HeliportCheck();
                if (semaphoreWalk == 0)
                {
                    audioManager.StopPlaying("CharWalk");
                }
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Sets the path to walk.
    /// </summary>
    /// <param name="way"></param>
    public void SetWalkPath(List<Vector3> way)
    {
        walkPath = way;
    }

    /// <summary>
    /// Triggers the selection of a character, if there is no other object over the object like GUI etc.
    /// </summary>
    public void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        selectChar();

    }

    /// <summary>
    /// Triggered if mouse enters character. If Current turnState is Move, mark current node.
    /// </summary>
   public void OnMouseEnter()
    {
        if(turnHandler.CharState == CharacterTurnHandler.Actions.MOVE)
        {
            MapManager.instance.getNodeFromPos(X, Z).Colorize(true);
        }
    }


    /// <summary>
    /// Called on exit of mouse. Resets the color of field.
    /// </summary>
    public void OnMouseExit()
    {
        if (turnHandler.CharState == CharacterTurnHandler.Actions.MOVE)
        {
            MapManager.instance.getNodeFromPos(X, Z).Colorize(false);
        }
    }

    /// <summary>
    /// Called if there is a mouseClick on Character.
    /// Responds in a appropriate manner according to the current state, e. g. passes the character itself as target of an action.
    /// </summary>
    public void selectChar()
    {

       if (turnHandler.CharState == CharacterTurnHandler.Actions.ATTACK)
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
            Debug.Log("Transfer Character: " + this.characterId);
            turnHandler.GetSelectedCharacter().Action_TransferSpiceTrigger(this);

        }
        else if (turnHandler.CharState == CharacterTurnHandler.Actions.FAMILY_ATOMICS)
        {
            CharacterTurnHandler.instance.GetSelectedCharacter().Attack_AtomicTrigger(nodeManager.getNodeFromPos(X, Z));
        }
       else if(turnHandler.CharState == CharacterTurnHandler.Actions.MOVE)
        {
            MapManager.instance.getNodeFromPos(X, Z).SelectNode();
        }
       else if(turnHandler.CharState == CharacterTurnHandler.Actions.HELIPORT)
        {
            MapManager.instance.getNodeFromPos(X, Z).SelectNode();
        }


        }


    /// <summary>
    /// Sends BasicAttack-Message Request to Server
    /// </summary>
    /// <param name="character">target character</param>
    /// <returns></returns>
    public bool Attack_BasicTrigger(Character character)
    {
        Node selectedNode = nodeManager.getNodeFromPos(turnHandler.GetSelectedCharacter().X, turnHandler.GetSelectedCharacter().Z);
        Node secondNode = nodeManager.getNodeFromPos(character.X, character.Z);
        

        if (nodeManager.isNodeNeighbour(selectedNode, secondNode) && !character.IsMemberOfHouse(house))
        {

            if (Mode.debugMode)
            {
                Attack_BasicExecution(character);
            }
            else
            {
                SessionHandler.messageController.DoRequestAction(SessionHandler.clientId, characterId, ActionType.ATTACK, new GameData.network.util.world.Position(character.X, character.Z));
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


    /// <summary>
    /// Plays attack and damage animation.
    /// </summary>
    /// <param name="character">target character</param>
    public void Attack_BasicExecution(Character character)
    {
        turnHandler.ResetAction();
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
        if (Mode.debugMode)
        {
            turnHandler.ResetSelection();
        }

    }

    /// <summary>
    /// Sends CollectSpice-Message Request to Server and audio
    /// </summary>
    /// <returns>whether there was spice on the field to collect.</returns>
    public bool Action_CollectSpiceTrigger()
    {
        if (nodeManager.IsSpiceOn(X, Z))
        {
            if (Mode.debugMode)
            {
                Action_CollectSpiceExecution();
            }
            else {
                SessionHandler.messageController.DoRequestAction(SessionHandler.clientId, characterId, ActionType.COLLECT, new GameData.network.util.world.Position(X, Z));
            }

        }
        else
        {
            Debug.Log("No Spice to collect!");
            return false;
        }
        //reset 
        if (Mode.debugMode)
        {
            turnHandler.ResetSelection();
        }
        return true;
    }

    /// <summary>
    /// Plays collection of spice animation and audio
    /// </summary>
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


    /// <summary>
    /// Sends ActionTransfer-Message request to Server
    /// </summary>
    /// <param name="character"></param>
    /// <returns>whether the character to transfer to belongs to the own house</returns>
    public bool Action_TransferSpiceTrigger(Character character)
    {
        Node selectedNode = nodeManager.getNodeFromPos(turnHandler.GetSelectedCharacter().X, turnHandler.GetSelectedCharacter().Z);
        Node secondNode = nodeManager.getNodeFromPos(character.X, character.Z);

        if (nodeManager.isNodeNeighbour(selectedNode, secondNode) && character.IsMemberOfHouse(house))
        {
            InGameMenuManager.getInstance().DemandSpiceAmount(this, character, this.spiceInv);
            return true;
        }
        else
        {
            turnHandler.ResetAction();
            Debug.Log("illegal Transfer!");
            return false;
        }
    }

    /// <summary>
    /// Resets player action
    /// </summary>
    public void CancleRequstTransferSpice()
    {
        turnHandler.ResetAction();
        Debug.Log("cancled Transfer!");
    }

    /// <summary>
    /// finaly sends TransferSpice
    /// </summary>
    /// <param name="character"></param>
    /// <param name="spiceAmount"></param>
    public void TriggerRequestTransferSpice(Character character, int spiceAmount)
    {
        if (Mode.debugMode)
        {
            Action_TransferSpiceExecution(character);
        }
        else
        {
            Debug.Log("action transferSpice: " + SessionHandler.clientId);
            SessionHandler.messageController.DoRequestTransfer(SessionHandler.clientId, characterId, character.characterId, spiceAmount);
        }      
    }

    /// <summary>
    /// Plays TransferSpice animation and audio
    /// </summary>
    /// <param name="character"></param>
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
        if (Mode.debugMode)
        {
            turnHandler.ResetSelection();
        }
    }


    /// <summary>
    /// Sends SwordSpin-Message request to Server
    /// </summary>
    /// <returns></returns>
    public bool Attack_SwordSpinTrigger()
    {
        if (characterType == CharTypeEnum.FIGHTER)
        {

                SessionHandler.messageController.DoRequestAction(SessionHandler.clientId, characterId, ActionType.SWORD_SPIN, new GameData.network.util.world.Position(X, Z));

            return true;
        }
        else
        {
            turnHandler.ResetAction();
            Debug.Log("illegal Attack_SwordSpin");
            return false;
        }



    }

    /// <summary>
    /// Plays SwordSpin animation and audio
    /// </summary>
    public void Attack_SwordSpinExecution()
    {
        Debug.Log("Attack_SwordSpin");

        charAnim.Play(animation_spiceHoarding);
        audioManager.Play("SpiceHoarding");
    }


    /// <summary>
    /// Sends AtomicAction-Message request to Server
    /// </summary>
    /// <param name="node">target node</param>
    /// <returns></returns>
    public bool Attack_AtomicTrigger(Node node)
    {
        if (characterType == CharTypeEnum.NOBLE)
        { 

            
            if (Mode.debugMode)
            {
                Attack_AtomicExecution(node);
            }
            else
            {
                SessionHandler.messageController.DoRequestAction(SessionHandler.clientId, characterId, ActionType.FAMILY_ATOMICS, new GameData.network.util.world.Position(node.X, node.Z));
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


    /// <summary>
    /// Creates the atomic-Object from prefab, which will fly towards the given target node from the current selected node. It uses a bezier-curve to calculate flying-curve.
    /// </summary>
    /// <param name="node">target node, where the atomic is exploding on.</param>
    public void Attack_AtomicExecution(Node node)
    {
        GameObject atomicInst = Instantiate(CharacterMgr.instance.atomicPrefab, new Vector3(X, 0.5f, Z), Quaternion.identity);
        ((AtomicController)atomicInst.GetComponent(typeof(AtomicController))).SetTargetPos(node.X, node.Z);
        audioManager.Play("AtomicFly");
        Debug.Log("Created Atomic");
       
        if (Mode.debugMode)
        {
            if (_AP <= 0) CharacterTurnHandler.EndTurn();
        }

        if (Mode.debugMode)
        {
            turnHandler.ResetSelection();
        }
    }

    /// <summary>
    /// Sends Kanly-Message request to Server
    /// </summary>
    /// <param name="character">target of action</param>
    /// <returns></returns>
    public bool Attack_KanlyTrigger(Character character)
    {

        if (characterType == CharTypeEnum.NOBLE && character.GetCharType() == CharTypeEnum.NOBLE && !character.IsMemberOfHouse(house))
        {
            Node selectedNode = nodeManager.getNodeFromPos(X, Z);
            Node secondNode = nodeManager.getNodeFromPos(character.X, character.Z);
            if (nodeManager.isNodeNeighbour(selectedNode, secondNode))
            {
                
                if (Mode.debugMode)
                {
                    Attack_KanlyExecution(character);
                }
                else {
                    SessionHandler.messageController.DoRequestAction(SessionHandler.clientId, characterId, ActionType.KANLY, new GameData.network.util.world.Position(character.X, character.Z));
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

    /// <summary>
    /// Plays Kanly animation and audio
    /// </summary>
    /// <param name="character"></param>
    public void Attack_KanlyExecution(Character character)
    {
        Debug.Log("Kanly fight!");
        Vector3 dir = character.transform.position - transform.position;
        RotateTowardsVector(dir);
        charAnim.Play(animation_kanly);
        StartCoroutine(character.PlayDamageAnimation(this));
        if (Mode.debugMode)
        {
            if (_AP <= 0) CharacterTurnHandler.EndTurn();
        }
        
        if (Mode.debugMode)
        {
            turnHandler.ResetSelection();
        }
    }


    /// <summary>
    /// Sends Action_SpiceHoarding-Message request to Server
    /// </summary>
    /// <returns></returns>
    public bool Action_SpiceHoardingTrigger()
    {
        if (characterType == CharTypeEnum.MENTANT)
        {


            if (Mode.debugMode)
            {
                Action_SpiceHoardingExecution();
            }
            else {
                SessionHandler.messageController.DoRequestAction(SessionHandler.clientId, characterId, ActionType.SPICE_HOARDING, new GameData.network.util.world.Position(X, Z));
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

    /// <summary>
    /// Plays SpiceHoarding animation and audio
    /// </summary>
    public void Action_SpiceHoardingExecution()
    {
        charAnim.Play(animation_spiceHoarding);
        audioManager.Play("SpiceHoarding");

        
        if (Mode.debugMode)
        {
            CharacterTurnHandler.EndTurn();
        }

        if (Mode.debugMode)
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
            turnHandler.ResetSelection();
        }
    }

    /// <summary>
    /// Sends VoiceAction-Message to server.
    /// </summary>
    /// <param name="character">target of Action</param>
    /// <returns>whether neighbour could be attacked</returns>
    public bool Action_VoiceTrigger(Character character)
    {
        if (characterType == CharTypeEnum.BENEGESSERIT)
        {

            Node selectedNode = nodeManager.getNodeFromPos(turnHandler.GetSelectedCharacter().X, turnHandler.GetSelectedCharacter().Z);
            Node secondNode = nodeManager.getNodeFromPos(character.X, character.Z);
            if (nodeManager.isNodeNeighbour(selectedNode, secondNode))
            {


                if (Mode.debugMode)
                {
                    Action_VoiceExecution(character);
                }
                else
                {
                    SessionHandler.messageController.DoRequestAction(SessionHandler.clientId, characterId, ActionType.VOICE, new GameData.network.util.world.Position(character.X, character.Z));
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

    /// <summary>
    /// Plays Voice animation and audio.
    /// </summary>
    /// <param name="character"></param>
    public void Action_VoiceExecution(Character character)
    {
        Vector3 dir = character.transform.position - transform.position;
        RotateTowardsVector(dir);
        charAnim.Play(animation_voice);
        Debug.Log("Voice!");


        if (Mode.debugMode)
        {
            turnHandler.ResetSelection();
            CharacterTurnHandler.EndTurn();
        }
       
    }
    /// <summary>
    /// Sends heliport request to server
    /// </summary>
    /// <param name="targetNode"></param>
    public void Action_HeliportTrigger(Node targetNode)
    {
        SessionHandler.messageController.DoRequestHeliport(SessionHandler.clientId, characterId, new GameData.network.util.world.Position(targetNode.X, targetNode.Z));
        turnHandler.ResetAction();
    }


    /// <summary>
    /// Coroutine used for async activation and deactivation of sword
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Sets house-emblem according to house enum.
    /// </summary>
    public void SetMatColorToHouse()
    {
        Sprite img = null;
        switch(house)
        {
            case HouseEnum.CORRINO: //Gold
                img = CharacterMgr.instance.emblemCorinno;
                break;
            case HouseEnum.ATREIDES:
                img = CharacterMgr.instance.emblemAtreides;
                break;
            case HouseEnum.HARKONNEN:
                img = CharacterMgr.instance.emblemHarkonnen;
                break;
            case HouseEnum.ORDOS:
                img = CharacterMgr.instance.emblemOrdos;
                break;
            case HouseEnum.RICHESE: //Silver
                img = CharacterMgr.instance.emblemRichese;
                break;
            case HouseEnum.VERNIUS:
                img = CharacterMgr.instance.emblemVernius;
                break;
        }
        ((Image)(emblemLogo.GetComponent(typeof(Image)))).sprite = img;
    }

    /// <summary>
    /// </summary>
    /// <param name="houseEnum"></param>
    /// <returns>true if character is member of house</returns>
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
        if (!isDead)
        {
            Vector3 dir = character.transform.position - transform.position;
            yield return new WaitForSeconds(0.25f);
            if (!isDead)
            {
                RotateTowardsVector(dir);
                charAnim.Play(animation_damage);
            }
        }
    }

    /// <summary>
    /// Is used to update and set the values of the Char-Stats HUD
    /// </summary>
    public void DrawStats()
    {
        GUIHandler.UpdateHP(HP);
        GUIHandler.UpdateAP(_AP);
        GUIHandler.UpdateMP(_MP);
        GUIHandler.UpdateCharSpice(spiceInv);
    }


    /// <summary>
    /// Rotates character towards a Vector3.
    /// </summary>
    /// <param name="dir"></param>
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

    /// <summary>
    /// Sets maxAP
    /// </summary>
    /// <param name="maxAP"></param>
    public void setMaxAP(int maxAP)
    {
        BaseAP = maxAP;
    }

    /// <summary>
    /// Sets maxHP
    /// </summary>
    /// <param name="maxHP"></param>
    public void setMaxHP(int maxHP)
    {
        BaseHP = maxHP;
    }

    /// <summary>
    /// Sets the characters transform the the given position. The heigh is calculated from the node height.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    public void ReplaceCharacterOnPosition(int x, int z)
    {
        _x = x;
        _z = z;
        this.transform.position = new Vector3(x, BaseY + MapManager.instance.getNodeFromPos(x, z).charHeightOffset, z);

    }
}
