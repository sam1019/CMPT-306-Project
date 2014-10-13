using UnityEngine;
using System.Collections;

public class Specialist : Player {

	public const string className = "Specialist";
	public const float baseDamage = 75.0f;
	public const float baseDefense = 10.0f;
	public int movementRange=5;
	public float HP;
	
	public int attackRange = 3;
	private float bottonWidth;
	private float buttonWidth;
	
	public float attackHitRate = 0.95f;
	public float defenseReduceRate = 0.2f;
	
	void Start () {
		HP = 75.0f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
