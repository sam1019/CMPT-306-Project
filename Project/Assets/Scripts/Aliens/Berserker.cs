using UnityEngine;
using System.Collections;

public class Berserker : Player {

	// Use this for initialization
	public const string className = "Bezerker";
	public const float baseDamage = 10.0f;
	public const float baseDefense = 90.0f;
	public const float attackHitRate = 1.0f;
	public const float defenseReduceRate = 0.02f;
	public int movementRange = 3;
	public float HP;

	void Start () {

		HP = 200.0f;
		movementRange = 5;

	}
	
	// Update is called once per frame
	public override void Update () {
		
	}
	public override void TurnUpdate (){
		GameManager.instance.nextTurn();
		base.TurnUpdate ();
	}
	public virtual void TurnOnGUI(){
	}
	
	//Display HP
	public void OnGUI(){
		Vector3 location = Camera.main.WorldToScreenPoint (transform.position)+ Vector3.up*30+ Vector3.left*15;
		GUI.TextArea(new Rect(location.x, Screen.height - location.y, 30, 20), HP.ToString());
	}
}
