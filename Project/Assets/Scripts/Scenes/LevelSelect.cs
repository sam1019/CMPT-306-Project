using UnityEngine;
using System.Collections;

public class LevelSelect : MonoBehaviour {

	public static LevelSelect instance;

	public string levelMap;

	public string levelUnits;

	public GUISkin buttonGUISkin;
	public Texture level1ButtonTexture;
	public Texture level2ButtonTexture;
	public Texture level3ButtonTexture;
	public Texture level4ButtonTexture;
	public Texture level5ButtonTexture;
	public Texture quitButtonTexture;


	// Use this for initialization
	void Start () {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/*
	 * select level screen buttons
	 */ 
	void OnGUI(){


		GUI.skin = buttonGUISkin;
		float widthScale = 0.45f;
		float heightScale = 0.1f;

		//Select level 1 botton
		if (GUI.Button (new Rect (Screen.width * (1 - widthScale)/2, Screen.height * (1 - heightScale - 0.2f)/2, Screen.width * widthScale, Screen.height * heightScale), level1ButtonTexture)) 
		{
			levelMap = "Map_Level1";		//generate map from csv file
			levelUnits = "Units_Level1";	//spawn units from csv file
			Time.timeScale=1;
			Application.LoadLevel ("project");
		}
		//Select level 2 botton
		if (GUI.Button (new Rect (Screen.width * (1 - widthScale)/2, Screen.height * (1 - heightScale - 0.0f)/2, Screen.width * widthScale, Screen.height * heightScale), level2ButtonTexture)) 
		{
			levelMap = "Map_Level2";		//generate map from csv file
			levelUnits = "Units_Level2";	//spawn units from csv file
			Time.timeScale=1;
			Application.LoadLevel ("project");
		}
		//Select level 3 botton
		if (GUI.Button (new Rect (Screen.width * (1 - widthScale)/2, Screen.height * (1 - heightScale + 0.2f)/2, Screen.width * widthScale, Screen.height * heightScale), level3ButtonTexture)) 
		{
			levelMap = "Map_Level3";		//generate map from csv file
			levelUnits = "Units_Level3";	//spawn units from csv file
			Time.timeScale=1;
			Application.LoadLevel ("project");
		}
		//Select level 4 botton
		if (GUI.Button (new Rect (Screen.width * (1 - widthScale)/2, Screen.height * (1 - heightScale + 0.4f)/2, Screen.width * widthScale, Screen.height * heightScale), level4ButtonTexture)) 
		{
			levelMap = "Map_Level4";		//generate map from csv file
			levelUnits = "Units_Level4";	//spawn units from csv file
			Time.timeScale=1;
			Application.LoadLevel ("project");
		}
		//Select level 5 botton
		if (GUI.Button (new Rect (Screen.width * (1 - widthScale)/2, Screen.height * (1 - heightScale + 0.6f)/2, Screen.width * widthScale, Screen.height * heightScale), level5ButtonTexture))
		{
			levelMap = "Map_Level5";		//generate map from csv file
			levelUnits = "Units_Level5";	//spawn map from csv file
			Time.timeScale=1;
			Application.LoadLevel ("project");
		}
		//Quit application button
		if (GUI.Button (new Rect (Screen.width * (1 - widthScale)/2, Screen.height * (1 - heightScale + 0.8f)/2, Screen.width * widthScale, Screen.height * heightScale), quitButtonTexture)) 
		{
			Application.Quit();	
		}


	}

}
