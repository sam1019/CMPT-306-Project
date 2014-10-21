using UnityEngine;
using System.Collections;


public class AiPlayer : Player {

	public float HP = 100.0f;

	public  void Start () {}
	
	// Update is called once per frame
	public override void Update () {}

	public override void TurnUpdate (){

		GameManager.instance.nextTurn();
		base.TurnUpdate ();
	}
	public virtual void TurnOnGUI(){}
	
	//Display HP
	public void OnGUI(){

		Vector3 location = Camera.main.WorldToScreenPoint (transform.position)+ Vector3.up*30+ Vector3.left*15;
		GUI.TextArea(new Rect(location.x, Screen.height - location.y, 30, 20), HP.ToString());
	}
}
