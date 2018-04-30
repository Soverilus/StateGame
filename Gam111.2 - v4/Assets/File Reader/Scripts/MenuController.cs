using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {
	public bool isPaused = false;
	GameObject noPauseMenu;

	void Start(){
		noPauseMenu = GameObject.FindGameObjectWithTag ("NoPause");
	}

	//universal scene loading tool.
	public void GoToScene(string putSceneHere){
		SceneManager.LoadScene (putSceneHere);
	}
	//need to remember to include this in every program
	public void QuitProgram(){
		Application.Quit ();
	}

	//Redundant code:::

	/*
	public void MainMenu(){
		GoToScene ("MainMenu");
	}
	*/

	void Update(){ //pause menu script for further development. WIP code.
		// you can't pause the main menus! at least thanks to this code.
		if ( noPauseMenu == null) {
			if (Input.GetKeyDown (KeyCode.Escape)){
				if (isPaused == false) {
					isPaused = true;
				} 
				else {
					isPaused = false;
				}
			}
		}
	}
}