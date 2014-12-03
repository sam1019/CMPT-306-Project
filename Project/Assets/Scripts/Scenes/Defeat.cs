using UnityEngine;
using System.Collections;

public class Defeat : MonoBehaviour {


	public GUISkin buttonGUISkin;
	public Texture tryAgainButtonTexture;
	public Texture goBackButtonTexture;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		
		GUI.skin = buttonGUISkin;
		float widthScale = 0.45f;
		float heightScale = 0.1f;

		//try again on current level
		if (GUI.Button (new Rect (25.0f+Screen.width * (1 - widthScale)/2, Screen.height * (1 - heightScale + 0.4f)/2, Screen.width * widthScale/1.5f, Screen.height * heightScale), tryAgainButtonTexture)) 
		{
			Application.LoadLevel ("project");
		}


		// can go back and quit
		if (GUI.Button (new Rect (25.0f+Screen.width * (1 - widthScale)/2, Screen.height * (1 - heightScale + 0.60f)/2, Screen.width * widthScale/1.5f, Screen.height * heightScale), goBackButtonTexture)) 
		{
			Application.LoadLevel ("LevelSelectScene");
		}
		
	}
}
