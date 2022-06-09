using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitMatchConfig : MonoBehaviour
{
	public string EditorMainMenuSceneName;
	public GameObject Panel;
    
	public void exit() {
		if(Panel != null) {
			Panel.SetActive(true);
		}
	}
	
	public void exitDialog(bool Option) {
		if(Option) {
			SceneManager.LoadScene(EditorMainMenuSceneName);
		} else {
			Panel.SetActive(false);
		}
	}
	
}
