using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GameData.network.controller;
using Serilog;
using System.Threading;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;

    [Header("Menus:")]
    public GameObject MainMenu;
    public GameObject OptionsMenu;
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
        Log.Debug("DemandPlayGame");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
       // if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            Log.Debug("Active Scene != 1");
            SceneManager.LoadScene(1);
            Log.Debug("End of Loading Scene");
        }
        //else
        {
       //     Log.Debug("DemandPlayGame Else");
         //   InGameMenuManager.instance.DemandAcceptRejoin();
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
        
        for (int i = 0; i < 3; i++)
        {
            SessionHandler.CreateNetworkModule(serverIP, int.Parse(serverPort));
            if (SessionHandler.clientconhandler.ConnectionIsAlive())
            {
                Debug.Log("Successfuly established connection. Now starting join");
                Log.Debug("Successfuly established connection. Now starting join");
                break;
            }
            else
            {
                Debug.Log("Error on establishing connection... Reconnecting.");
                SessionHandler.CloseNetworkModule();
                Thread.Sleep(250);
                //SessionHandler.CreateNetworkModule(serverIP, int.Parse(serverPort));
            }
        }
        if (SessionHandler.clientconhandler.ConnectionIsAlive())
        {
            SessionHandler.messageController.DoJoin(name, active, false);
        }

    }


    /// <summary>
    /// this method is called by the SERVER to accept a join and send the clienSecret
    /// </summary>
    /// <param name="clientSecret"></param>
    public void DemandJoinAccept()
    {

            DemandPlayGame();

        Log.Debug("DemandJoinAccept: " + clientSecret);

    }


    /// <summary>
    /// this method is a HELPER-METHOD to change the .isActive trade of the menus
    /// </summary>
    /// <param name="menuToActivate">maybe null</param>
    private void ActivateMenu(GameObject menuToActivate)
    {
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(false);
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
    /// this method is called by a BUTTON to switch to the JoinGameMenu
    /// </summary>
    public void SwitchToJoinGameMenu()
    {
        ActivateMenu(JoinGameMenu);
    }
}
