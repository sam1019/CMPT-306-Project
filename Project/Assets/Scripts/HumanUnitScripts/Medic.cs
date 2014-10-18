using UnityEngine;
using System.Collections;

public class Medic : Player {

	public const string className = "Medic";
	public const float baseDamage = 7.5f;
	public const float baseDefense = 10.0f;
	public int movementRange=3;
	public float HP;
	public bool isAttacking =false;
	public int attackRange = 1;
	private float bottonWidth;
	private float buttonWidth;
	
	public float attackHitRate = 0.75f;
	public float defenseReduceRate = 0.2f;
	
	void Start () {
		HP = 90.0f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void attack(Tile range){
		
	}
}
