using UnityEngine;
using System.Collections;

public class Berserker : Player {

	// Use this for initialization
	public const string className = "Bezerker";
	public const float baseDamage = 10.0f;
	public const float baseDefense = 90.0f;
	public const float attackHitRate = 1.0f;
	public const float defenseReduceRate = 0.02f;
	public int movementRange = 3;
	public float HP;

	void Start () {

		HP = 200.0f;
		movementRange = 5;

	}
	
	// Update is called once per frame
	void Update () {
		if (this.HP <= 0) {
			this.transform.renderer.material.color = Color.black;
			Destroy(this.transform.gameObject);
		}
	}
}
