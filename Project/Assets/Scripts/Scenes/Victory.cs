using UnityEngine;
using System.Collections;

public class Victory : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		
		float widthScale = 0.2f;
		float heightScale = 0.05f;

		//continue button; based on the current level
		if(LevelSelect.instance.levelMap == "Map_Level1"){
			if (GUI.Button (new Rect (Screen.width * (1 - widthScale)/2, Screen.height * (1 - heightScale - 0.15f)/2, Screen.width * widthScale, Screen.height * heightScale), "Continue: Level 2")) 
			{
				LevelSelect.instance.levelMap = "Map_Level2";
				LevelSelect.instance.levelUnits = "Units_Level2";
				Application.LoadLevel ("project");
			}
		}
		if(LevelSelect.instance.levelMap == "Map_Level2"){
			if (GUI.Button (new Rect (Screen.width * (1 - widthScale)/2, Screen.height * (1 - heightScale - 0.15f)/2, Screen.width * widthScale, Screen.height * heightScale), "Continue: Level 3")) 
			{
				LevelSelect.instance.levelMap = "Map_Level3";
				LevelSelect.instance.levelUnits = "Units_Level3";
				Application.LoadLevel ("project");
			}
		}
		if(LevelSelect.instance.levelMap == "Map_Level3"){
			if (GUI.Button (new Rect (Screen.width * (1 - widthScale)/2, Screen.height * (1 - heightScale - 0.15f)/2, Screen.width * widthScale, Screen.height * heightScale), "Continue: Level 4")) 
			{
				LevelSelect.instance.levelMap = "Map_Level4";
				LevelSelect.instance.levelUnits = "Units_Level4";
				Application.LoadLevel ("project");
			}
		}
		if(LevelSelect.instance.levelMap == "Map_Level4"){
			if (GUI.Button (new Rect (Screen.width * (1 - widthScale)/2, Screen.height * (1 - heightScale - 0.15f)/2, Screen.width * widthScale, Screen.height * heightScale), "Continue: Level 5")) 
			{
				LevelSelect.instance.levelMap = "Map_Level5";
				LevelSelect.instance.levelUnits = "Units_Level5";
				Application.LoadLevel ("project");
			}
		}
		//can go back and quit
		if (GUI.Button (new Rect (Screen.width * (1 - widthScale)/2, Screen.height * (1 - heightScale + 0.60f)/2, Screen.width * widthScale, Screen.height * heightScale), "Go Back")) 
		{
			Application.LoadLevel ("LevelSelectScene");
		}

	}
}
