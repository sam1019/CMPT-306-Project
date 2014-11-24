using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* This class acts as an interface class
 * Root character class
 */
public class Player : MonoBehaviour {
	
	public Vector3 moveDestination;
	public float moveSpeed = 10.0f;
	public Vector2 gridPosition = Vector2.zero;
	//public float HP;

	//boolean to judge moveing and attack condition
	public bool moving= false;
	public bool attacking=false;
	public int actionPoints = 2;

	public int movementRange;
	public int attackRange;

	public float HP;
	public float baseHP;
	public float baseDamage;
	public float baseDefense;
	public float attackBonus;
	public int K = 100; // reference on Google Drive; it's used for culculating demage	
	
	public bool moveTurn = false;
	public bool attackTurn = false;
	//For movement in GameManager.MovePlayer()
	public List<Vector3> positionQueue = new List<Vector3>();

	void Awake () {
		moveDestination = transform.position;
	}

	public virtual void Update () {}
	
	//Each charactor in each turn can do 2 actions in total
	// virtual keyword allows child classes to override the method
	public virtual void TurnUpdate () {
		if (actionPoints <= 0) {
			actionPoints = 2;
			moving = false;
			attacking = false;
			moveTurn = false;
			attackTurn = false;
			GameManager.instance.nextTurn();
		}
	}
	
	// create GUI component
	public virtual void TurnOnGUI(){}

	// get role name
	public virtual string roleName () {
		return null;
	}
}
