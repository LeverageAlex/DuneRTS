using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GameData.network.controller;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;

    [Header("Menus:")]
    public GameObject MainMenu;
    public GameObject OptionsMenu;
    public GameObject PlayOptionsMenu;
    public GameObject JoinGameMenu;

    [Header("Create/Join Game:")]
    public InputField nameInput;
    public Toggle playerToggle;
    public Toggle viewerToggle;
    public InputField serverIPInput;
    public InputField serverPortInput;

    private string clientSecret;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        ActivateMenu(MainMenu);
    }

    /// <summary>
    /// this method is called by a BUTTON to play the game
    /// </summary>
    public void DemandPlayGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            InGameMenuManager.instance.DemandAcceptRejoin();
        }
    }

    /// <summary>
    /// this method is called by a BUTTON to quit the application
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// this method is called by a BUTTON to join a game
    /// </summary>
    public void RequestJoinGame()
    {
        string name = nameInput.text;
        bool active = playerToggle.isOn;
        string serverIP = serverIPInput.text;
        string serverPort = serverPortInput.text;

        //TODO validate inputs

        Debug.Log("Join: " + name + " " + serverIP + " " + serverPort + " " + active);

        //TODO send JOIN message to server with given IP and given port
        if (!Mode.debugMode)
        {
            ConnectionEstablisher.CreateNetworkModule(serverIP, int.Parse(serverPort));
            ConnectionEstablisher.messageController.DoJoin(name, active, false);


        }
        else
        {

            DemandPlayGame();//TODO delete, just temporary for testing
        }
    }


    /// <summary>
    /// this method is called by the SERVER to accept a join and send the clienSecret
    /// </summary>
    /// <param name="clientSecret"></param>
    public void DemandJoinAccept(string clientSecret)
    {
        DemandPlayGame();
    }


    /// <summary>
    /// this method is a HELPER-METHOD to change the .isActive trade of the menus
    /// </summary>
    /// <param name="menuToActivate">maybe null</param>
    private void ActivateMenu(GameObject menuToActivate)
    {
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        PlayOptionsMenu.SetActive(false);
        JoinGameMenu.SetActive(false);

        if (menuToActivate != null)
        {
            menuToActivate.SetActive(true);
        }
    }


    //BUTTON SWITCH MENU METHODS ------------

    /// <summary>
    /// this method is called by a BUTTON to switch to the MainMenu
    /// </summary>
    public void SwitchToMainMenu()
    {
        ActivateMenu(MainMenu);
    }

    /// <summary>
    /// this method is called by a BUTTON to switch to the OptionsMenu
    /// </summary>
    public void SwitchToOptionsMenu()
    {
        ActivateMenu(OptionsMenu);
    }

    /// <summary>
    /// this method is called by a BUTTON to switch to the PlayOptionsMenu
    /// </summary>
    public void SwitchToPlayOptionsMenu()
    {
        ActivateMenu(PlayOptionsMenu);
    }

    /// <summary>
    /// this method is called by a BUTTON to switch to the JoinGameMenu
    /// </summary>
    public void SwitchToJoinGameMenu()
    {
        ActivateMenu(JoinGameMenu);
    }
}
