using UnityEngine;
using System.Collections;

public class AlienSoldier : Player {
	
	public const string className = "Alien Soldier";
	public const float baseDamage = 18.5f;
	public const float baseDefense = 18.5f;
	public int movementRange = 3;
	public float HP;
	
	public int attackRange = 1;
	private float bottonWidth;
	private float buttonWidth;
	
	public float attackHitRate = 0.8f;
	public float defenseReduceRate = 0.2f;

	private Animator anim;
	
	void Start () {
		HP = 100.0f;
		anim = gameObject.GetComponent<Animator> ();
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
