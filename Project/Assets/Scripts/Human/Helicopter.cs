using UnityEngine;
using System.Collections;

public class Helicopter : Player {
	
	public const string className = "Helicopter";
	public const float baseDamage = 20.0f;
	public const float baseDefense = 20.0f;
	public int movementRange = 4;
	public float HP;
	
	public int attackRange = 2;
	private float bottonWidth;
	private float buttonWidth;
	
	public float attackHitRate = 0.8f;
	public float defenseReduceRate = 0.2f;
	
	void Start () {
		HP = 120.0f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
