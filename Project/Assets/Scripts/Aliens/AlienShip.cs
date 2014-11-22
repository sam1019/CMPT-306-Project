using UnityEngine;
using System.Collections;

public class AlienShip : Player {

	// character properties
	public const string className = "Alien Ship";
	public const float baseDamage = 30.0f;
	public const float baseDefense = 10.0f;
	public const int attackRange = 2;
	public const float attackHitRate = 0.8f;
	public const float defenseReduceRate = 0.2f;
	public int movementRange;
	public float HP;

	// GUI component properties
	private Animator anim;


	void Start () {

		HP = 90.0f;
		movementRange = 4;
		anim = gameObject.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	public override void Update () {}

	public override void TurnUpdate (){
//		Tile distination = new Tile (5, -5, 0);
		moveDestination = new Vector3 (5.0f, 5.0f, -1.0f);
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
