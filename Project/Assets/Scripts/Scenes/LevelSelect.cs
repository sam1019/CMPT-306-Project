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

		//Pause Button
		if (GUI.Button (new Rect (400, 150, 160, 20), "Level 1")) 
		{
			Application.LoadLevel ("project");
		}
		if (GUI.Button (new Rect (400, 180, 160, 20), "Level 2")) 
		{
			Application.LoadLevel ("project");
		}
		if (GUI.Button (new Rect (400, 210, 160, 20), "Level 3")) 
		{
			Application.LoadLevel ("project");
		}
		if (GUI.Button (new Rect (400, 240, 160, 20), "Level 4")) 
		{
			Application.LoadLevel ("project");
		}
		if (GUI.Button (new Rect (400, 270, 160, 20), "Level 5"))
		{
			Application.LoadLevel ("project");
		}
		if (GUI.Button (new Rect (400, 300, 160, 20), "Go Back")) 
		{
			Application.LoadLevel ("");
		}


	}

}
