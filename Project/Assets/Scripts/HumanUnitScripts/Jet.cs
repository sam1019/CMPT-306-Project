using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Jet : Player {

	// character properties
	public const string className = "Jet";
	public bool isAttacking =false;
	public float defenseReduceRate = 0.2f;
	private Animator anim;
	public List<Tile> path;


	void Start () {
		this.baseDamage = 25.0f;
		this.baseDefense = 15.0f;
		this.HP = 120.0f;
		this.baseHP = 120.0f;
		this.movementRange = 5;
		this.attackRange = 2;
		this.attackHitRate = 0.8f;
		anim = gameObject.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	public override void Update () {

		// Basic charactor color is blue
		// When a charactor is chosen, it's color will turn to cyan
		// When a charactor die, it will turn to black and destroy, check Player.cs Script Update()
		if(GameManager.instance.playerList.Count > 0 && this.HP > 0){

			if (GameManager.instance.playerList[GameManager.instance.currentPlayerIndex].GetComponent<Jet>() == this 
			    && GameManager.instance.playerList.Count > 0) {

				anim.SetBool("focus", true); //when in its turn, play animation
				transform.renderer.material.color = Color.Lerp(Color.white,Color.cyan, Time.time%2);
				transform.renderer.material.color = Color.Lerp(Color.cyan,Color.white, Time.time%2);
			}
			//Otherwise charactor is white
			else {
				anim.SetBool("focus", false);//when out its turn, play idle animation
				transform.renderer.material.color = Color.white;
			}
		}
		if (HP <= 0) {
			GameManager.instance.playerCount --;
			Destroy(this.gameObject, 1);		
		}
		base.Update();
	}

	public override void TurnUpdate(){
		
		//Moving the player to destination
		if (Vector3.Distance(moveDestination, transform.position) > 0.1f) {

			//For future development, move from tile to tile
//			path = PathFinding.getTilesPath((int)transform.position.x, (int)transform.position.y, (int)moveDestination.x, (int)moveDestination.y, this.attackRange, 1, "HUMAN");
//			int i = 1;
//			foreach(Tile t in path){
//				Debug.Log ("Tile" + i + t.gridPosition.x + "," + t.gridPosition.y);
//				i ++;
//			}

			transform.position += (moveDestination - transform.position).normalized * moveSpeed * Time.deltaTime;
			
			//Used to check if the player has reached it's destination, if so next turn
			if (Vector3.Distance(moveDestination, transform.position) <= 0.1f) {
				transform.position = moveDestination;
				//GameManager.instance.nextTurn();
				moveTurn  = true;
				GameManager.instance.disableHightLight();
				if(moveTurn&&attackTurn){
					moveTurn = false;
					attackTurn = false;

					GameManager.instance.nextTurn();
				}
			}

			base.TurnUpdate ();
		}
	}


	/*
	 * Gets current player's grid position
	 */ 
	// TODO: Used for testing, may not need for future
	public Tile getGridPosition(){

		int x = (int) this.gridPosition.x;
		int y = (int)this.gridPosition.y;
		Tile tile = GameManager.instance.map [x] [y].GetComponent<Tile> ();
		return tile;
	}
	
	/*
	 * Finds the enemy's class on the selected tile to attack
	 */
	public void getEnemyToAttack(Tile tile){
		if (!attackTurn) {
			foreach (GameObject p in GameManager.instance.playerList) { //Checks for enemy class on tile target
				
				if(p.GetComponent<AlienShip>() != null){
					AlienShip target = null;
					AlienShip temp = p.GetComponent<AlienShip>(); //Gets enemy script
					
					if (temp.gridPosition == tile.gridPosition) { //Checks if tile selected contains enemy
						target = temp;
						JetAttack.attackAlienShip(target); //Attacks the specific enemy unit
						audio.Play ();
					}
				}
				else if(p.GetComponent<AlienSoldier>() != null){ //Checks for enemy class on tile target
					AlienSoldier target = null;
					AlienSoldier temp = p.GetComponent<AlienSoldier>();	//Gets enemy script		
					
					if (temp.gridPosition == tile.gridPosition) { //Checks if tile selected contains enemy
						target = temp;
						JetAttack.attackAlienSoldier(target); //Attacks the specific enemy unit
						audio.Play ();
					}
				}
				else if(p.GetComponent<AlienSupport>() != null){ //Checks for enemy class on tile target
					AlienSupport target = null;
					AlienSupport temp = p.GetComponent<AlienSupport>();	//Gets enemy script			
					
					if (temp.gridPosition == tile.gridPosition) { //Checks if tile selected contains enemy
						target = temp;
						JetAttack.attackAlienSupport(target); //Attacks the specific enemy unit
						audio.Play ();
					}
				}
				else if(p.GetComponent<Berserker>() != null){ //Checks for enemy class on tile target
					Berserker target = null;
					Berserker temp = p.GetComponent<Berserker>(); //Gets enemy script	
					
					if (temp.gridPosition == tile.gridPosition) { //Checks if tile selected contains enemy
						target = temp;
						JetAttack.attackAlienBerserker(target); //Attacks the specific enemy unit
						audio.Play ();
					}
				}
				/**********TEST class************/
				else if(p.GetComponent<AiPlayer>() != null){ //Checks for enemy class on tile target
					AiPlayer target = null;
					AiPlayer temp = p.GetComponent<AiPlayer>();	 //Gets enemy script
					
					if (temp.gridPosition == tile.gridPosition) { //Checks if tile selected contains enemy
						target = temp;
						JetAttack.attackAIPlayer(target); //Attacks the specific enemy unit
					}
				}
			}
			attackTurn = true;
			if(moveTurn&&attackTurn){
				moveTurn = false;
				attackTurn = false;
				
				GameManager.instance.nextTurn();
			}
		}
	}

	//Return the class name of unit
	public override string roleName(){

		return className;
	}

	public override void TurnOnGUI(){

		float buttonHeight = 50;
		float buttonWidth = 100;

		// move button
		Rect buttonRect = new Rect(0, Screen.height - buttonHeight * 3, buttonWidth, buttonHeight);
		if(!moveTurn){
			if (GUI.Button(buttonRect, "Move")) {
				//if not moving, first disable all Highlight 
				//enable Move Highlight
				moving = false;
				if (!moving) {
					GameManager.instance.disableHightLight();
					moving = true;
					isAttacking = false;
					//Enables highlight path hightlight
					GameManager.instance.enableMoveHighlight((int)this.gridPosition.x, (int)this.gridPosition.y, this.movementRange);
				} 
				//otherwise disable all Highlight
				else {
					moving = false;
					isAttacking = false;
					GameManager.instance.disableHightLight();
				}
			}
		}

		if(!attackTurn){
			//attack button
			buttonRect = new Rect(0, Screen.height - buttonHeight * 2, buttonWidth, buttonHeight);
			
			if (GUI.Button(buttonRect, "Attack")) {
				//if not attacking, first disable all Highlight 
				//enable Attack Highlight
				isAttacking = false;
				if (!isAttacking) {
					GameManager.instance.disableHightLight();
					moving = false;
					isAttacking = true;
					//Enables attack hightlight range
					GameManager.instance.enableAttackHighlight((int)this.gridPosition.x, (int)this.gridPosition.y, this.attackRange);
				}
				//otherwise disable all Highlight
				else {
					moving = false;
					isAttacking = false;
					GameManager.instance.disableHightLight();
				}
			}
		}
		
		//end turn button
		buttonRect = new Rect(0, Screen.height - buttonHeight * 1, buttonWidth, buttonHeight);		
		
		if (GUI.Button(buttonRect, "End Turn")) {
			moving = false;
			isAttacking = false;
			moveTurn = false;
			attackTurn =false;
			GameManager.instance.nextTurn();
		}
		base.TurnOnGUI ();
	}
	

}