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

    public void SwitchToInGameMenu()
    {
        InGameMenu.SetActive(true);
        InGameUI.SetActive(false);
        OptionsMenu.SetActive(false);
    }

    public void SwitchToInGameUI()
    {
        InGameMenu.SetActive(false);
        InGameUI.SetActive(true);
        OptionsMenu.SetActive(false);
    }

    public void SwitchToOptionsMenu()
    {
        InGameMenu.SetActive(false);
        InGameUI.SetActive(false);
        OptionsMenu.SetActive(true);
    }

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
        InGameMenu.SetActive(false);
        InGameUI.SetActive(false);
        OptionsMenu.SetActive(false);
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
        InGameMenu.SetActive(false);
        InGameUI.SetActive(true);
        OptionsMenu.SetActive(false);
        PauseScreenWithButton.SetActive(false);
        PauseScreenNoButton.SetActive(false);

        Time.timeScale = 1f;
    }
}
