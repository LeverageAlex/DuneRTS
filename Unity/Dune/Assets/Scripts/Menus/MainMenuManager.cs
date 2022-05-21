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
    public GameObject HouseSelectionMenu;
    public GameObject PlayOptionsMenu;
    public GameObject CreateJoinGameMenu;

    [Header("HouseSelection:")]
    public Toggle option1;
    public Toggle option2;
    public GameObject confirmButton;

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
    /// this method is called by the SERVER to start the HouseSelcetion with two options
    /// </summary>
    /// <param name="houseName1"></param>
    /// <param name="houseName2"></param>
    public void StartHouseSelection(string houseName1, string houseName2)
    {
        SetOptionText(1, houseName1);
        SetOptionText(2, houseName2);

        ActivateMenu(HouseSelectionMenu);
    }

    //THIS METHOD IS TEMPORARY AND ONLY MENT FOR THE BUTTON ACTIVATION OF THE HOUSE SELECTEION ToDo delete
    public void StartHouseSelection()
    {
        StartHouseSelection("option 1", "option 2");
    }

    /// <summary>
    /// this method is called by the SERVER to end the HouseSelection when house gets acknowkledged
    /// </summary>
    public void EndHouseSelection()
    {
        ActivateMenu(MainMenu);
    }

    /// <summary>
    /// this method is called by a BUTTON to select one of the two house options
    /// </summary>
    public void SelectOption()
    {
        if (option1.isOn)
        {
            Debug.Log(option1.GetComponentInChildren<Text>().text + " was selected!");
            //TODO send message to server

            EndHouseSelection();//TODO trigger by server instead
        }
        else if (option2.isOn)
        {
            Debug.Log(option2.GetComponentInChildren<Text>().text + " was selected!");
            //TODO send message to server

            EndHouseSelection();//TODO trigger by server instead
        }
    }

    /// <summary>
    /// this method is called by a TOGGLE to toggle the confirmButton.isActive
    /// </summary>
    public void ToggleChange()
    {
        if (!option1.isOn && !option2.isOn)
        {
            confirmButton.SetActive(false);
        }
        else
        {
            confirmButton.SetActive(true);
        }
    }

    /// <summary>
    /// this method is a HELPER-METHOD to set the textFields of the house options
    /// </summary>
    /// <param name="index">first or second option</param>
    /// <param name="houseName">name</param>
    private void SetOptionText(int index, string houseName)
    {
        if (index == 1)
        {
            option1.GetComponentInChildren<Text>().text = houseName;
        }
        else if (index == 2)
        {
            option2.GetComponentInChildren<Text>().text = houseName;
        }
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
    }

    /// <summary>
    /// this method is called by the SERVER to accept a join and send the clienSecret
    /// </summary>
    /// <param name="clientSecret"></param>
    public void JoinAccept(string clientSecret)
    {
        this.clientSecret = clientSecret;
    }

    /// <summary>
    /// this method is a HELPER-METHOD to change the .isActive trade of the menus
    /// </summary>
    /// <param name="menuToActivate">maybe null</param>
    private void ActivateMenu(GameObject menuToActivate)
    {
        MainMenu.SetActive(false);
        HouseSelectionMenu.SetActive(false);
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
