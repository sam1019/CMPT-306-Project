using UnityEngine;
using System.Collections;

public class Soldier : Player {

	public const string className = "Soldier";
	public const float baseDamage = 15.0f;
	public const float baseDefense = 15.0f;
	public int movementRange=3;
	public float HP;
	public bool isAttacking =false;
	public int attackRange = 1;
	private float bottonWidth;
	private float buttonWidth;
	
	public float attackHitRate = 0.75f;
	public float defenseReduceRate = 0.2f;
	
	void Start () {
		HP = 100.0f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void attack(Tile range){
		
	}
}
