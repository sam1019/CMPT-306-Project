using UnityEngine;
using System.Collections;

/*Generic Player*/
public class UserPlayer : Player {




	void Awake(){
		//Setting the destination to it's spawn
		destination = transform.position;
	}
	// virtual keyword allows child classes to override the method

	// Use this for initialization
	public  void Start () {
	
	}
	
	// Update is called once per frame
	public  void Update () {
	
	}
	

	// virtual keyword allows child classes to override the method
	public override void TurnUpdate(){

		//Moving the player to destination
		if (Vector3.Distance(destination, transform.position) > 0.1f) {
			transform.position += (destination - transform.position).normalized * moveSpeed * Time.deltaTime;

			//Used to check if the player has reached it's destination, if so next turn
			if (Vector3.Distance(destination, transform.position) <= 0.1f) {
				transform.position = destination;
				GameManager.instance.nextTurn();
			}
		}
		base.TurnUpdate ();
	}

	
}
