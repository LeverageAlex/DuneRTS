using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PartyConfiguration;

public class Character : MonoBehaviour
{
    public string charName;
    public float walkSpeed = 3f;


    private int characterId;
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

    private bool isLoud;
    private bool isSwallowed;

    public bool move;
    private LinkedList<Vector3> walkPath;
// public Transform t;








    // Start is called before the first frame update
    void Start()
    {

        GameManager gameManager = GameManager.instance;
        //SampleCode only
        CharacterBaseValue type = GetTypeByString(gameObject.name);
        initCharacter(type);


        //Update Nodes references on start (only needed because of editor)
        //gameManager.getNodeFromPos((int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.z)).placeObjectOnNode(gameObject);
        gameManager.placeObjectOnNode(gameObject, (int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.z));
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


    public void initCharacter(CharacterBaseValue characterBaseValue)
    {
        HP = characterBaseValue.HP;
        MP = characterBaseValue.MP;
        AP = characterBaseValue.AP;
        AD = characterBaseValue.AD;
        spiceInv = characterBaseValue.spiceInv;
        healingHP = characterBaseValue.HealHP;

        isLoud = false;
        isSwallowed = false;
    }


    //To be deleted
    CharacterBaseValue GetTypeByString(string charType)
    {
        switch(charType)
        {
            case "Noble":
                return PartyConfiguration.Noble;
            case "BeneGesserit":
                return PartyConfiguration.BeneGesserit;
            case "Mentat":
                return PartyConfiguration.Mentat;
            case "Fighter":
                return PartyConfiguration.Fighter;
            default:
                Debug.Log("Error in Character-Script. String did not Match");
                return null;
        }
    }


    public void UpdateCharStats(int HP, int HealHP, int MP, int AP, int AD, int spiceInv, bool isLoud, bool isSwallowed)
    {
        this.HP = HP;
        this.healingHP=HealHP;
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

        if (Vector3.Distance(transform.position, walkPath.First.Value) <= 0.2f)
        {
            walkPath.RemoveFirst();
            //E. g. go To next Point
            return walkPath.Count > 0;
        }
        return true;
    }

    public void SetWalkPath(LinkedList<Vector3> way)
    {
        walkPath = way;
    }


}
