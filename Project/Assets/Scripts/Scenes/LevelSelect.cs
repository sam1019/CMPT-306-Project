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

	void OnGUI(){


		GUI.skin = buttonGUISkin;
		float widthScale = 0.45f;
		float heightScale = 0.1f;

		//Pause Button
		if (GUI.Button (new Rect (Screen.width * (1 - widthScale)/2, Screen.height * (1 - heightScale - 0.2f)/2, Screen.width * widthScale, Screen.height * heightScale), level1ButtonTexture)) 
		{
			levelMap = "Map_Level1";
			levelUnits = "Units_Level1";
			Application.LoadLevel ("project");
		}
		if (GUI.Button (new Rect (Screen.width * (1 - widthScale)/2, Screen.height * (1 - heightScale - 0.0f)/2, Screen.width * widthScale, Screen.height * heightScale), level2ButtonTexture)) 
		{
			levelMap = "Map_Level2";
			levelUnits = "Units_Level2";
			Application.LoadLevel ("project");
		}
		if (GUI.Button (new Rect (Screen.width * (1 - widthScale)/2, Screen.height * (1 - heightScale + 0.2f)/2, Screen.width * widthScale, Screen.height * heightScale), level3ButtonTexture)) 
		{
			levelMap = "Map_Level3";
			levelUnits = "Units_Level3";
			Application.LoadLevel ("project");
		}
		if (GUI.Button (new Rect (Screen.width * (1 - widthScale)/2, Screen.height * (1 - heightScale + 0.4f)/2, Screen.width * widthScale, Screen.height * heightScale), level4ButtonTexture)) 
		{
			levelMap = "Map_Level4";
			levelUnits = "Units_Level4";
			Application.LoadLevel ("project");
		}
		if (GUI.Button (new Rect (Screen.width * (1 - widthScale)/2, Screen.height * (1 - heightScale + 0.6f)/2, Screen.width * widthScale, Screen.height * heightScale), level5ButtonTexture))
		{
			levelMap = "Map_Level5";
			levelUnits = "Units_Level5";
			Application.LoadLevel ("project");
		}
		if (GUI.Button (new Rect (Screen.width * (1 - widthScale)/2, Screen.height * (1 - heightScale + 0.8f)/2, Screen.width * widthScale, Screen.height * heightScale), quitButtonTexture)) 
		{
			Application.Quit();
		}


	}

}
