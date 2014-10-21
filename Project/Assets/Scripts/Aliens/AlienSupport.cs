using UnityEngine;
using System.Collections;

public class AlienSupport : Player {

	// character properties
	public const string className = "Alien Soldier";
	public const float baseDamage = 0.0f;
	public const float baseDefense = 15.0f;
	public const int attackRange = 0;
	public const float attackHitRate = 0.8f;
	public const float defenseReduceRate = 0.2f;
	public int movementRange;
	public float HP;


	 void Start () {
		HP = 100.0f;
	}
	
	// Update is called once per frame
	public override void Update () {}
}
