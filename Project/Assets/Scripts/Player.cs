using UnityEngine;
using System.Collections;

/*Generic Player*/
public class Player : MonoBehaviour, PlayerInterface {

	public int HP;

	public Vector2 gridPosition = Vector2.zero;
	public Vector3 destination;

	public int moveSpeed=10;
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


	void Awake(){
		//Setting the destination to it's spawn
		destination = transform.position;
	}
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

		//Moving the player to destination
		if (Vector3.Distance(destination, transform.position) > 0.1f) {
			transform.position += (destination - transform.position).normalized * moveSpeed * Time.deltaTime;

			//Used to check if the player has reached it's destination, if so next turn
			if (Vector3.Distance(destination, transform.position) <= 0.1f) {
				transform.position = destination;
				GameManager.instance.nextTurn();
			}
		}
	}

	
}
