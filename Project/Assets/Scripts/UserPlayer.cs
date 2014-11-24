using UnityEngine;
using System.Collections;

public class UserPlayer : Player {
	
	// charactor properties
	public string playerName = "Guardian";
	public int attackRange = 1;
	public int movementRange= 5;
	public float HP=100.0f;
	public float attackHitRate = 0.9f;
	public float defenseReduceRate = 0.2f;
	public bool isAttacking =false;
	//public int damageBase = 5;
	//public float damageRollSides = 6;
	public bool isHit;
	public bool isDefend;
	
	//A 2D Rectangle defined by x, y position and width, height.
	private float bottonWidth;
	private float buttonWidth;

	
	void Awake(){

		//Setting the destination to it's spawn
		moveDestination = transform.position;
	}
	
	// Use this for initialization
	void Start () { }
	
	// Update is called once per frame 
	public override void Update () {

		// Basic charactor color is blue
		// When a charactor is chosen, it's color will turn to cyan
		// When a charactor die, it will turn to black and destroy, check Player.cs Script Update()
		if(GameManager.instance.playerList.Count > 0 && this.HP > 0){

			if (GameManager.instance.playerList[GameManager.instance.currentPlayerIndex].GetComponent<UserPlayer>() == this 
			    && GameManager.instance.playerList.Count > 0) {

				transform.renderer.material.color = Color.cyan;
			}
			//Otherwise charactor is blue
			else {
				transform.renderer.material.color = Color.blue;
			}
		}
		if (this.HP <= 0) {
			transform.renderer.material.color = Color.black;		
		}
		base.Update();
	}

	public override void TurnUpdate(){
		
		//Moving the player to destination
		if (Vector3.Distance(moveDestination, transform.position) > 0.1f) {
			transform.position += (moveDestination - transform.position).normalized * moveSpeed * Time.deltaTime;
			
			//Used to check if the player has reached it's destination, if so next turn
			if (Vector3.Distance(moveDestination, transform.position) <= 0.1f) {
				transform.position = moveDestination;
				GameManager.instance.nextTurn();
			}
			base.TurnUpdate ();
		}
	}

	public override void TurnOnGUI () {

		float buttonHeight = 50;
		float buttonWidth = 100;

		// move button
		Rect buttonRect = new Rect(0, Screen.height - buttonHeight * 3, buttonWidth, buttonHeight);
		if (GUI.Button(buttonRect, "Move")) {
			if (!moving) {
				moving = true;
				isAttacking = false;
			} else {
				moving = false;
				isAttacking = false;
			}
		}
		
		//attack button
		buttonRect = new Rect(0, Screen.height - buttonHeight * 2, buttonWidth, buttonHeight);
		
		if (GUI.Button(buttonRect, "Attack")) {
			if (!isAttacking) {
				moving = false;
				isAttacking = true;
			} else {
				moving = false;
				isAttacking = false;
			}
		}
		
		//end turn button
		buttonRect = new Rect(0, Screen.height - buttonHeight * 1, buttonWidth, buttonHeight);		
		
		if (GUI.Button(buttonRect, "End Turn")) {
			actionPoints = 2;
			moving = false;
			isAttacking = false;			
			GameManager.instance.nextTurn();
		}
		base.TurnOnGUI ();
	}

	// Hit rate
	public bool Hit(){

		if(Random.Range(0,10000).CompareTo(attackHitRate*10000)<=0){
			isHit=true;
		}
		else{
			isHit=false;
		}
		return isHit;
	}
	
	// HP is decrease after every hit
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

	/*
	 * Gets current player's grid position
	 */ 
	public Tile getGridPosition(){

		int x = (int) this.gridPosition.x;
		int y = (int)this.gridPosition.y;
		Tile tile = GameManager.instance.map [x] [y].GetComponent<Tile> ();
		return tile;
	}

	/*
	 *Checks if target is within attacking range of player 
	 */
	public bool isTargetInRange(UserPlayer playerTemp, AiPlayer target){

		int playerX, playerY, targetX, targetY;
		playerX = (int)playerTemp.gridPosition.x;
		playerY = (int)playerTemp.gridPosition.y;
		targetX = (int)target.gridPosition.x;
		targetY = (int)target.gridPosition.y;
		print (playerX+ " " + playerY);
		print (targetX+ " " + targetY);
		return(playerX >= targetX - attackRange && playerX <= targetX + attackRange &&
		       playerY >= targetY - attackRange && playerY <= targetY + attackRange);
	}

	public void tempAttack(Tile tile){

		AiPlayer target = null;

		foreach (GameObject p in GameManager.instance.playerList) {

			if(p.GetComponent<AiPlayer>() != null){
				AiPlayer temp = p.GetComponent<AiPlayer>();

				if (temp.gridPosition == tile.gridPosition) { //Checks if tile selected contains enemy
					target = temp;
				}
				else{
					print("Target not found");
				}
			}
		}
		
		if (target != null) {
			UserPlayer playerTemp = GameManager.instance.playerList[GameManager.instance.currentPlayerIndex].GetComponent<UserPlayer>();

			if(isTargetInRange(playerTemp,target)){
				playerTemp.actionPoints--;
				playerTemp.attacking = false;			
				
				//attack logic
				//roll to hit
				bool hit = Random.Range(0.0f, 1.0f) <= playerTemp.attackHitRate;
				
				if (hit) {
					//damage logic
					int amountOfDamage = (int)Mathf.Floor(10 + Random.Range(0, 6));
					target.HP -= amountOfDamage;
				} else {
					print ("Missed, you suck");
				}
				GameManager.instance.nextTurn();
			} else {
				Debug.Log ("Target is out of range!");
			}
		}
	}

	public override string roleName(){

		return playerName;
	}
	
	//Display HP
	public void OnGUI(){

		Vector3 location = Camera.main.WorldToScreenPoint (transform.position)+ Vector3.up*30+ Vector3.left*15;
		GUI.TextArea(new Rect(location.x, Screen.height - location.y, 30, 20), HP.ToString());
	}
}

