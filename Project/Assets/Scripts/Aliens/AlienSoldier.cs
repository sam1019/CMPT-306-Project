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
	
	void Start () {
		HP = 100.0f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
