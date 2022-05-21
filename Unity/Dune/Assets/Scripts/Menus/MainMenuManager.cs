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

    [Header("HouseSelection:")]
    public Toggle option1;
    public Toggle option2;
    public GameObject confirmButton;

    /// <summary>
    /// this method is called by a BUTTON to switch to the MainMenu
    /// </summary>
    public void SwitchToMainMenu()
    {
        MainMenu.SetActive(true);
        HouseSelectionMenu.SetActive(false);
        OptionsMenu.SetActive(false);
    }

    /// <summary>
    /// this method is called by a BUTTON to switch to the OptionsMenu
    /// </summary>
    public void SwitchToOptionsMenu()
    {
        MainMenu.SetActive(false);
        HouseSelectionMenu.SetActive(false);
        OptionsMenu.SetActive(true);
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

        MainMenu.SetActive(false);
        HouseSelectionMenu.SetActive(true);
        OptionsMenu.SetActive(false);
    }

    //THIS METHOD IS TEMPORARY AND ONLY MENT FOR THE BUTTON ACTIVATION OF THE HOUSE SELECTEION ToDo delete
    public void StartHouseSelection()
    {
        SetOptionText(1, "option1");
        SetOptionText(2, "option2");

        MainMenu.SetActive(false);
        HouseSelectionMenu.SetActive(true);
        OptionsMenu.SetActive(false);
    }

    /// <summary>
    /// this method is called by the SERVER to end the HouseSelection when house gets acknowkledged
    /// </summary>
    public void EndHouseSelection()
    {
        MainMenu.SetActive(true);
        HouseSelectionMenu.SetActive(false);
        OptionsMenu.SetActive(false);
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
}
