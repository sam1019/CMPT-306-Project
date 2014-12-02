using UnityEngine;
using System.Collections;

public class Defeat : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		
		float widthScale = 0.2f;
		float heightScale = 0.05f;

		//try again on current level
		if (GUI.Button (new Rect (25.0f+Screen.width * (1 - widthScale)/2, Screen.height * (1 - heightScale + 0.45f)/2, Screen.width * widthScale/1.5f, Screen.height * heightScale), "Try Again!")) 
		{
			Application.LoadLevel ("project");
		}


		// can go back and quit
		if (GUI.Button (new Rect (25.0f+Screen.width * (1 - widthScale)/2, Screen.height * (1 - heightScale + 0.60f)/2, Screen.width * widthScale/1.5f, Screen.height * heightScale), "Go Back")) 
		{
			Application.LoadLevel ("LevelSelectScene");
		}
		
	}
}
