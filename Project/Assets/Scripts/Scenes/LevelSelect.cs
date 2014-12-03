using UnityEngine;
using System.Collections;

public class LevelSelect : MonoBehaviour {

	public static LevelSelect instance;

	public string levelMap;

	public string levelUnits;

	// Use this for initialization
	void Start () {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){

		float widthScale = 0.14f;
		float heightScale = 0.05f;

		//Pause Button
		if (GUI.Button (new Rect (Screen.width * (1 - widthScale)/2, Screen.height * (1 - heightScale - 0.15f)/2, Screen.width * widthScale, Screen.height * heightScale), "Level 1")) 
		{
			levelMap = "Map_Level1";
			levelUnits = "Units_Level1";
			Application.LoadLevel ("project");
		}
		if (GUI.Button (new Rect (Screen.width * (1 - widthScale)/2, Screen.height * (1 - heightScale - 0.0f)/2, Screen.width * widthScale, Screen.height * heightScale), "Level 2")) 
		{
			levelMap = "Map_Level2";
			levelUnits = "Units_Level2";
			Application.LoadLevel ("project");
		}
		if (GUI.Button (new Rect (Screen.width * (1 - widthScale)/2, Screen.height * (1 - heightScale + 0.15f)/2, Screen.width * widthScale, Screen.height * heightScale), "Level 3")) 
		{
			levelMap = "Map_Level3";
			levelUnits = "Units_Level3";
			Application.LoadLevel ("project");
		}
		if (GUI.Button (new Rect (Screen.width * (1 - widthScale)/2, Screen.height * (1 - heightScale + 0.3f)/2, Screen.width * widthScale, Screen.height * heightScale), "Level 4")) 
		{
			levelMap = "Map_Level4";
			levelUnits = "Units_Level4";
			Application.LoadLevel ("project");
		}
		if (GUI.Button (new Rect (Screen.width * (1 - widthScale)/2, Screen.height * (1 - heightScale + 0.45f)/2, Screen.width * widthScale, Screen.height * heightScale), "Level 5"))
		{
			levelMap = "Map_Level5";
			levelUnits = "Units_Level5";
			Application.LoadLevel ("project");
		}
		if (GUI.Button (new Rect (Screen.width * (1 - widthScale)/2, Screen.height * (1 - heightScale + 0.6f)/2, Screen.width * widthScale, Screen.height * heightScale), "Quit")) 
		{
			Application.Quit();
		}


	}

}
