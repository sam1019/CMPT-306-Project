using UnityEngine;
using System.Collections;

public class AlienShip : Player {

	public const string className = "Alien Ship";
	public const float baseDamage = 30.0f;
	public const float baseDefense = 10.0f;
	public int movementRange = 4;
	public float HP;
	
	public int attackRange = 2;
	private float bottonWidth;
	private float buttonWidth;
	
	public float attackHitRate = 0.8f;
	public float defenseReduceRate = 0.2f;
	
	void Start () {
		HP = 90.0f;
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
