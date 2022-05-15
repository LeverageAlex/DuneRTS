using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HouseSelectMenu : MonoBehaviour
{
    public Toggle option1;
    public Toggle option2;
    public GameObject confirmButton;
    public GameObject[] menusToToggle;

    public void StartHouseSelection(string houseName1, string houseName2)
    {
        SetOptionText(1, houseName1);
        SetOptionText(2, houseName2);

        this.gameObject.SetActive(true);
        foreach(GameObject menu in menusToToggle)
        {
            menu.SetActive(false);
        }
    }

    //THIS METHOD IS TEMPORARY AND ONLY MENT FOR THE BUTTON ACTIVATION OF THE HOUSE SELECTEION ToDo delete
    public void StartHouseSelection()
    {
        SetOptionText(1, "option1");
        SetOptionText(2, "option1");

        this.gameObject.SetActive(true);
        foreach (GameObject menu in menusToToggle)
        {
            menu.SetActive(false);
        }
    }

    private void SetOptionText(int index, string houseName)
    {
        if(index == 1)
        {
            option1.GetComponentInChildren<Text>().text = houseName;
        } 
        else if(index == 2)
        {
            option2.GetComponentInChildren<Text>().text = houseName;
        }  
    }

    public void SelectOption()
    {
        if (option1.isOn)
        {
            Debug.Log(option1.GetComponentInChildren<Text>().text + " was selected!");
            //TODO send message to server

            EndHouseSelection();
        } 
        else if(option2.isOn)
        {
            Debug.Log(option2.GetComponentInChildren<Text>().text + " was selected!");
            //TODO send message to server

            EndHouseSelection();
        }
    }

    private void EndHouseSelection()
    {
        this.gameObject.SetActive(false);
        foreach (GameObject menu in menusToToggle)
        {
            menu.SetActive(true);
        }
    }

    public void ToggleChange()
    {
        if (!option1.isOn && !option2.isOn)
        {
            confirmButton.SetActive(false);
        } else
        {
            confirmButton.SetActive(true);
        }
    }
}
