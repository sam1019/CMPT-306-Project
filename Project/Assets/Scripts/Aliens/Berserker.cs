using UnityEngine;
using System.Collections;

public class Berserker : MonoBehaviour {

	// Use this for initialization
	public string className;
	public float baseDamage;
	public float baseDefense;
	public int movementRange;
	public float HP;

	void Start () {
		HP = 200.0f;
		className = "Bezerker";
		baseDamage = 10.0f;
		baseDefense = 90.0f;
		movementRange = 2;

	}
	
	// Update is called once per frame
	void Update () {
		if (this.HP <= 0) {
			this.transform.renderer.material.color = Color.black;
			Destroy(this.transform.gameObject);
		}
	}
}
