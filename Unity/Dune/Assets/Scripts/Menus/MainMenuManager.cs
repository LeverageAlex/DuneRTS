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
    public GameObject CreateJoinGameMenu;

    [Header("Create/Join Game:")]
    public GameObject createGameText;
    public GameObject joinGameText;
    public GameObject cpuCountPanel;
    public GameObject createButton;
    public GameObject joinButton;
    public InputField nameInput;
    public Toggle playerToggle;
    public Toggle viewerToggle;
    public InputField lobbyCodeInput;
    public Dropdown cpuCountDropdown;
    //public Dropdown scenarioDropdown;
    //public Dropdown matchDropdown;

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
    /// this mehtod is called by a BUTTON to create a game
    /// </summary>
    public void CreateGame()
    {
        string lobbyCode = lobbyCodeInput.text;
        int cpuCount = cpuCountDropdown.value;

        string name = nameInput.text;
        bool active = playerToggle.isOn;

        //TODO validate inputs

        Debug.Log("Create: " + lobbyCode + " " + cpuCount);

        //TODO send CREATE message to server

        JoinGame(name, lobbyCode, active);
    }

    /// <summary>
    /// this method is called by a BUTTON to join a game
    /// </summary>
    public void JoinGame()
    {
        string name = nameInput.text;
        string lobbyCode = lobbyCodeInput.text;
        bool active = playerToggle.isOn;

        //TODO validate inputs

        JoinGame(name, lobbyCode, active);
    }

    /// <summary>
    /// this method is called to join/rejoin a game
    /// it gets called as a followup from CreateGame or the button-called JoinGame-Method or by rejoining after connection loss
    /// </summary>
    /// <param name="name"></param>
    /// <param name="connectionCode"></param>
    /// <param name="active"></param>
    private void JoinGame(string name, string connectionCode, bool active)
    {
        Debug.Log("Join: " + name + " " + connectionCode + " " + active);

        //TODO send JOIN message to server

        PlayGame();//TODO delete, just temporary for testing
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
        CreateJoinGameMenu.SetActive(false);

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
    /// this method is called by a BUTTON to switch to the CreateGameMenu
    /// </summary>
    public void SwitchToCreatGameMenu()
    {
        ActivateMenu(CreateJoinGameMenu);
        createGameText.SetActive(true);
        joinGameText.SetActive(false);
        cpuCountPanel.SetActive(true);
        createButton.SetActive(true);
        joinButton.SetActive(false);
    }

    /// <summary>
    /// this method is called by a BUTTON to switch to the JoinGameMenu
    /// </summary>
    public void SwitchToJoinGameMenu()
    {
        ActivateMenu(CreateJoinGameMenu);
        createGameText.SetActive(false);
        joinGameText.SetActive(true);
        cpuCountPanel.SetActive(false);
        createButton.SetActive(false);
        joinButton.SetActive(true);
    }
}
