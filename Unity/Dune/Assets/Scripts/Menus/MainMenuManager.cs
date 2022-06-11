using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
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

    void Start()
    {
        ActivateMenu(MainMenu);
    }

    /// <summary>
    /// this method is called by a BUTTON to play the game
    /// </summary>
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
    public void JoinGame()
    {
        string name = nameInput.text;
        bool active = playerToggle.isOn;
        string serverIP = serverIPInput.text;
        string serverPort = serverPortInput.text;

        //TODO validate inputs

        Debug.Log("Join: " + name + " " + serverIP + " " + serverPort + " " + active);

        //TODO send JOIN message to server with given IP and given port

        PlayGame();//TODO delete, just temporary for testing
    }

    /// <summary>
    /// this method ist called by a BUTTON to rejoin the game
    /// </summary>
    public void RejoinGame()
    {
        Debug.Log("Rejoin: " + clientSecret);
        //TODO send REJOIN with clientSecret
    }

    /// <summary>
    /// this method is called by the SERVER to accept a join and send the clienSecret
    /// </summary>
    /// <param name="clientSecret"></param>
    public void JoinAccept(string clientSecret)
    {
        this.clientSecret = clientSecret;
        PlayGame();
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
