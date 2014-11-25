using UnityEngine;
using System.Collections;

public class Berserker : Player {

	// character properties
	public const string className = "Berserker";
	public const float baseDamage = 10.0f;
	public const float baseDefense = 90.0f;
	public const float attackHitRate = 1.0f;
	public const float defenseReduceRate = 0.02f;
	public int movementRange;

	// GUI components properties
	private Animator anim;


	void Start () {

		HP = 200.0f;
		movementRange = 5;
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
