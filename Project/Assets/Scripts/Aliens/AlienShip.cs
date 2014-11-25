using UnityEngine;
using System.Collections;

public class AlienShip : Player {

	// character properties
	public const string className = "AlienShip";
	public const float baseDamage = 30.0f;
	public const float baseDefense = 10.0f;
	public const int attackRange = 2;
	public const float attackHitRate = 0.8f;
	public const float defenseReduceRate = 0.2f;

	// GUI component properties
	private Animator anim;


	void Start () {

		HP = 90.0f;
		movementRange = 4;
		anim = gameObject.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	public override void Update () {}

	public override void TurnUpdate (){

		GameManager.instance.nextTurn();
		base.TurnUpdate ();
	}
	public override void TurnOnGUI(){}

	public override string roleName(){
		
		return className;
	}
}
