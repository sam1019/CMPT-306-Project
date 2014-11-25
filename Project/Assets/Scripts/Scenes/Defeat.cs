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

		if (GUI.Button (new Rect (Screen.width * (1 - widthScale)/2, Screen.height * (1 - heightScale + 0.45f)/2, Screen.width * widthScale, Screen.height * heightScale), "Try Again!")) 
		{
			Application.LoadLevel ("project");
		}

		if (GUI.Button (new Rect (Screen.width * (1 - widthScale)/2, Screen.height * (1 - heightScale + 0.60f)/2, Screen.width * widthScale, Screen.height * heightScale), "Go Back")) 
		{
			Application.LoadLevel ("LevelSelectScene");
		}
		
	}
}
