using UnityEngine;
using System.Collections;

public class AlienSoldier : Player {

	// character properties
	public const string className = "Alien Soldier";
	public const float baseDamage = 18.5f;
	public const float baseDefense = 18.5f;
	public const int attackRange = 1;
	public const float attackHitRate = 0.8f;
	public const float defenseReduceRate = 0.2f;
	public int movementRange;

	// GUI components properties
	private Animator anim;


	void Start () {

		HP = 100.0f;
		anim = gameObject.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	public override void Update () {}

	public override void TurnUpdate (){

		GameManager.instance.nextTurn();
		base.TurnUpdate ();
	}

	public override void TurnOnGUI(){}

}
