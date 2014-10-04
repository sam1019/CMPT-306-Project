using UnityEngine;
using System.Collections;

/*Generic Player*/
public class Player : MonoBehaviour, PlayerInterface {

	public int HP;

	public Vector2 gridPosition;
	public Vector3 destination;

	public int moveSpeed;
	public int movementRange;
	public int attackRange;

	public bool moving;
	public bool attacking;
	
	public int baseDamage;

	public float defenseReduction;
	public float damageRollSides;
	
	//RNG element to make game interesting
	public float attackChance;

	//How many moves the players can make before the turn is over
	public int actionPoints;

	// virtual keyword allows child classes to override the method

	// Use this for initialization
	public virtual void Start () {
	
	}
	
	// Update is called once per frame
	public virtual void Update () {
	
	}

	//Explicit definition of interface method
	// virtual keyword allows child classes to override the method
	public virtual void DisplayHP(){

	}

	// virtual keyword allows child classes to override the method
	public virtual void TurnUpdate(){

	}

	
}
