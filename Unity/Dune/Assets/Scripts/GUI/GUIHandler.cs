using System.Collections;
using UnityEngine.UI;
using UnityEngine;


/**
 * This class handles the HUDs were data is shown, so no Buttons, just stats
 * 
 */
public class GUIHandler : MonoBehaviour
{

    public static GUIHandler instance;

    public Text playerSpice;
    public Text enemySpice;
    public Text HPText;
    public Text MPText;
    public Text APText;
    public Text SpiceInv;

    public GameObject gameMessage;
    public Text gameMessageText;

    private void Start()
    {
        instance = this;
        UpdatePlayerSpice(0);
        UpdateEnemySpice(0);
        BroadcastGameMessage("5 Gegen Willi");
    }


    public static void UpdatePlayerSpice(int spiceText)
    {
        instance.playerSpice.text = "Spice: " + spiceText.ToString();

    }
    public static void UpdateEnemySpice(int spiceText)
    {
        instance.enemySpice.text = "Spice: " + spiceText.ToString();

    }




    public static void UpdateHP(int hpText)
    {
        instance.HPText.text = "HP: " + hpText.ToString();

    }

    public static void UpdateMP(int mpText)
    {
        instance.MPText.text = "MP: " + mpText.ToString();

    }

    public static void UpdateAP(int apText)
    {
        instance.APText.text = "AP: " + apText.ToString();

    }

    public static void UpdateCharSpice(int spiceText)
    {
        instance.SpiceInv.text = "Inventory: " + spiceText.ToString();

    }

    public static void BroadcastGameMessage(string msg)
    {
        instance.gameMessageText.text = msg;
        instance.gameMessage.SetActive(true);
        instance.StartCoroutine(instance.deactivateGameMessage());

    }

    public IEnumerator deactivateGameMessage()
    {
        string startText = instance.gameMessageText.text;
        yield return new WaitForSeconds(5);
        if (startText.Equals(instance.gameMessageText.text))
        {
            instance.gameMessage.SetActive(false);
        }
    }


}
