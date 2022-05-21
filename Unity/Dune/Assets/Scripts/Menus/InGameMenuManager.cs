using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenuManager : MonoBehaviour
{
    public GameObject InGameMenu;
    public GameObject InGameUI;
    public GameObject OptionsMenu;
    public GameObject PauseScreenWithButton;
    public GameObject PauseScreenNoButton;

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    /// <summary>
    /// this method is called by a button to send a pause request
    /// </summary>
    public void PauseGame()
    {
        PauseScreenWithButton.SetActive(true);

        //TODO send message to server for a pause request
        ForcedPauseGame();//TODO delete
    }

    /// <summary>
    /// this method is called by a server message to pause the game
    /// </summary>
    public void ForcedPauseGame()
    {
        ActivateMenu(null);//deactivate all menus

        if (!PauseScreenWithButton.activeSelf)
        {
            PauseScreenNoButton.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    /// <summary>
    /// this method is called by a button to send an unpause request
    /// </summary>
    public void UnpauseGame()
    {
        //TODO send message to server for a unpause request
        ForcedUnpauseGame();//TODO delete
    }

    /// <summary>
    /// this method is called by a server message to unpause the game
    /// </summary>
    public void ForcedUnpauseGame()
    {
        ActivateMenu(InGameUI);

        PauseScreenWithButton.SetActive(false);
        PauseScreenNoButton.SetActive(false);

        Time.timeScale = 1f;
    }

    /// <summary>
    /// this method is a HELPER-METHOD to change the .isActive trade of the menus
    /// </summary>
    /// <param name="menuToActivate">maybe null</param>
    private void ActivateMenu(GameObject menuToActivate)
    {
        InGameMenu.SetActive(false);
        InGameUI.SetActive(false);
        OptionsMenu.SetActive(false);

        if(menuToActivate != null)
        {
            menuToActivate.SetActive(true);
        }
    }

    //BUTTON SWITCH MENU METHODS ------------

    /// <summary>
    /// this method is called by a BUTTON to switch to the MainMenu
    /// </summary>
    public void SwitchToInGameMenu()
    {
        ActivateMenu(InGameMenu);
    }

    /// <summary>
    /// this method is called by a BUTTON to switch to the InGameUi
    /// </summary>
    public void SwitchToInGameUI()
    {
        ActivateMenu(InGameUI);
    }

    /// <summary>
    /// this method is called by a BUTTON to switch to the OptionsMenu
    /// </summary>
    public void SwitchToOptionsMenu()
    {
        ActivateMenu(OptionsMenu);
    }
}
