using UnityEngine;
using System.Collections;

public class LevelSelect : MonoBehaviour {

	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){

		float widthScale = 0.2f;
		float heightScale = 0.05f;

		//Pause Button
		if (GUI.Button (new Rect (Screen.width * (1 - widthScale)/2, Screen.height * (1 - heightScale - 0.15f)/2, Screen.width * widthScale, Screen.height * heightScale), "Level 1")) 
		{
			Application.LoadLevel ("project");
		}
		if (GUI.Button (new Rect (Screen.width * (1 - widthScale)/2, Screen.height * (1 - heightScale - 0.0f)/2, Screen.width * widthScale, Screen.height * heightScale), "Level 2")) 
		{
			Application.LoadLevel ("project");
		}
		if (GUI.Button (new Rect (Screen.width * (1 - widthScale)/2, Screen.height * (1 - heightScale + 0.15f)/2, Screen.width * widthScale, Screen.height * heightScale), "Level 3")) 
		{
			Application.LoadLevel ("project");
		}
		if (GUI.Button (new Rect (Screen.width * (1 - widthScale)/2, Screen.height * (1 - heightScale + 0.3f)/2, Screen.width * widthScale, Screen.height * heightScale), "Level 4")) 
		{
			Application.LoadLevel ("project");
		}
		if (GUI.Button (new Rect (Screen.width * (1 - widthScale)/2, Screen.height * (1 - heightScale + 0.45f)/2, Screen.width * widthScale, Screen.height * heightScale), "Level 5"))
		{
			Application.LoadLevel ("project");
		}
		if (GUI.Button (new Rect (Screen.width * (1 - widthScale)/2, Screen.height * (1 - heightScale + 0.6f)/2, Screen.width * widthScale, Screen.height * heightScale), "Go Back")) 
		{
			Application.LoadLevel ("");
		}


	}

}
