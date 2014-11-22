using UnityEngine;
using System.Collections;

public class Berserker : Player {

	// character properties
	public const string className = "Bezerker";
	public const float baseDamage = 10.0f;
	public const float baseDefense = 90.0f;
	public const float attackHitRate = 1.0f;
	public const float defenseReduceRate = 0.02f;
	public int movementRange;
	public float HP;

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
		moveDestination = new Vector3 (-3.0f, -3.0f, -1.0f);
		if (Vector3.Distance(moveDestination, transform.position) > 0.1f){
			transform.position += (moveDestination - transform.position).normalized * moveSpeed * Time.deltaTime;
			if (Vector3.Distance(moveDestination, transform.position) <= 0.1f) {
				transform.position = moveDestination;
				GameManager.instance.nextTurn();
			}
			base.TurnUpdate();
		}
	}

	public override void TurnOnGUI(){}
	
	//Display HP
	public void OnGUI(){

		Vector3 location = Camera.main.WorldToScreenPoint (transform.position)+ Vector3.up*30+ Vector3.left*15;
		GUI.TextArea(new Rect(location.x, Screen.height - location.y, 30, 20), HP.ToString());
	}
}
