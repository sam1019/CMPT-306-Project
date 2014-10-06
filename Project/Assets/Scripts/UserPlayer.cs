using UnityEngine;
using System.Collections;

/*Generic Player*/
public class UserPlayer : Player {

<<<<<<< HEAD
	//Role Name
	public string playerName = "Guardian";
	
	//A 2D Rectangle defined by x, y position and width, height.
	private float bottonWidth;
	private float buttonWidth;
	
=======



>>>>>>> FETCH_HEAD
	void Awake(){
		//Setting the destination to it's spawn
		moveDestination = transform.position;
	}
	// virtual keyword allows child classes to override the method

	// Use this for initialization
	public  void Start () {
	
	}
	
	// Update is called once per frame
	public override void Update () {
		//basic charactor color is blue
		//When a charactor is chosen, it's color will turn to yellow
		if (GameManager.instance.playerList[GameManager.instance.currentPlayerIndex] == this) {
			transform.renderer.material.color = Color.yellow;
		}
		//Other unchosed charactor is blue
		else {
			transform.renderer.material.color = Color.blue;
		}
		base.Update();
	}
	

	// virtual keyword allows child classes to override the method
	public override void TurnUpdate(){

		//Moving the player to destination
		if (Vector3.Distance(moveDestination, transform.position) > 0.1f) {
			transform.position += (moveDestination - transform.position).normalized * moveSpeed * Time.deltaTime;

			//Used to check if the player has reached it's destination, if so next turn
			if (Vector3.Distance(moveDestination, transform.position) <= 0.1f) {
				transform.position = moveDestination;
				GameManager.instance.nextTurn();
			}
		}
		base.TurnUpdate ();
	}


	public bool isHit;
	public bool isDefend;

	//Hit rate
	public bool Hit(){
		if(Random.Range(0,10000).CompareTo(attackHitRate*10000)<=0){
			isHit=true;
		}
		else{
			isHit=false;
		}
		return isHit;
	}

	//HP is decrease after every hit
	public float HPChange (){
		//if hit, do damage; otherwise no damage
		if(isHit==true){
			if(isDefend==false){
				HP=HP-10.0f;
			}
			else{
				HP=HP-10.0f*defenseReduceRate;
			}
		}
		return HP;
	}

	public string roleName(){
		return playerName;
	}
}

