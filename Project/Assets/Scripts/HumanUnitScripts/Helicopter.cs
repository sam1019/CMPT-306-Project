﻿using UnityEngine;
using System.Collections;

public class Helicopter : Player {

	// character properties
	public const string className = "Helicopter";
	public const float baseDamage = 20.0f;
	public const float baseDefense = 20.0f;
	public bool isAttacking =false;
	public int attackRange = 2;
	public float attackHitRate = 0.8f;
	public float defenseReduceRate = 0.2f;
	public int movementRange;
	public float HP;


	void Start () {

		HP = 120.0f;
		movementRange = 4;
	}
	
	// Update is called once per frame
	public override void Update () {

		// Basic charactor color is blue
		// When a charactor is chosen, it's color will turn to cyan
		// When a charactor die, it will turn to black and destroy, check Player.cs Script Update()
		if(GameManager.instance.playerList.Count > 0 && this.HP > 0){

			if (GameManager.instance.playerList[GameManager.instance.currentPlayerIndex].GetComponent<Helicopter>() == this 
			    && GameManager.instance.playerList.Count > 0) {

				transform.renderer.material.color = Color.cyan;
			}
			// Otherwise charactor is blue
			else {
				transform.renderer.material.color = Color.white;
			}
		}

//		if (this.HP <= 0) {
//			transform.renderer.material.color = Color.black;		
//		}
		base.Update();
	}
	
	public override void TurnUpdate(){
		
		//Moving the player to destination
		if (Vector3.Distance(moveDestination, transform.position) > 0.1f) {
			transform.position += (moveDestination - transform.position).normalized * moveSpeed * Time.deltaTime;
			
			//Used to check if the player has reached it's destination, if so next turn
			if (Vector3.Distance(moveDestination, transform.position) <= 0.1f) {
				transform.position = moveDestination;// + Vector3.back;
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
	//Used for testing, may not need for future
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
			foreach (GameObject p in GameManager.instance.playerList) {
				if(p.GetComponent<AlienShip>() != null){ //Checks for enemy class on tile target
					AlienShip target = null;
					AlienShip temp = p.GetComponent<AlienShip>(); //Gets enemy script				
					
					if (temp.gridPosition == tile.gridPosition) { //Checks if tile selected contains enemy
						target = temp;
						HelicopterAttack.attackAlienShip(target); //Attacks the specific enemy unit
					}
				}
				else if(p.GetComponent<AlienSoldier>() != null){ //Checks for enemy class on tile target
					AlienSoldier target = null;
					AlienSoldier temp = p.GetComponent<AlienSoldier>();	 //Gets enemy script							
					if (temp.gridPosition == tile.gridPosition) { //Checks if tile selected contains enemy
						target = temp;
						HelicopterAttack.attackAlienSoldier(target); //Attacks the specific enemy unit
					}
				}
				else if(p.GetComponent<AlienSupport>() != null){  //Checks for enemy class on tile target
					AlienSupport target = null;
					AlienSupport temp = p.GetComponent<AlienSupport>();	 //Gets enemy script							
					if (temp.gridPosition == tile.gridPosition) { //Checks if tile selected contains enemy
						target = temp;
						HelicopterAttack.attackAlienSupport(target); //Attacks the specific enemy unit
					}
				}
				else if(p.GetComponent<Berserker>() != null){ //Checks for enemy class on tile target
					Berserker target = null;
					Berserker temp = p.GetComponent<Berserker>(); //Gets enemy script								
					if (temp.gridPosition == tile.gridPosition) { //Checks if tile selected contains enemy
						target = temp;
						HelicopterAttack.attackAlienBerserker(target); //Attacks the specific enemy unit
					}
				}
				/**********TEST class************/
				else if(p.GetComponent<AiPlayer>() != null){  //Checks for enemy class on tile target
					AiPlayer target = null;
					AiPlayer temp = p.GetComponent<AiPlayer>(); //Gets enemy script				
					if (temp.gridPosition == tile.gridPosition) { //Checks if tile selected contains enemy
						target = temp;
						HelicopterAttack.attackAIPlayer(target); //Attacks the specific enemy unit
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
	
	public override string roleName(){

		return className;
	}

	public override void TurnOnGUI(){

		float buttonHeight = 50;
		float buttonWidth = 100;

		// move button
		Rect buttonRect = new Rect(0, Screen.height - buttonHeight * 3, buttonWidth, buttonHeight);
		if (GUI.Button(buttonRect, "Move")) {
			//if not moving, first disable all Highlight 
			//enable Move Highlight
			moving = false;
			if ((!moving)&&(!moveTurn)){
				GameManager.instance.disableHightLight();
				moving = true;
				isAttacking = false;
				//Enable path highlight
				GameManager.instance.enableMoveHighlight((int)this.gridPosition.x, (int)this.gridPosition.y, this.movementRange);
			} 
			//otherwise disable all Highlight
			else {
				moving = false;
				isAttacking = false;
				GameManager.instance.disableHightLight();
			}
		}
		
		//attack button
		buttonRect = new Rect(0, Screen.height - buttonHeight * 2, buttonWidth, buttonHeight);
		
		if (GUI.Button(buttonRect, "Attack")) {
			//if not attacking, first disable all Highlight 
			//enable Attack Highligh
			isAttacking = false;
			if ((!isAttacking)&&(!attackTurn)) {
				GameManager.instance.disableHightLight();
				moving = false;
				isAttacking = true;
				//Enable hightlight for attack range
				GameManager.instance.enableAttackHighlight((int)this.gridPosition.x, (int)this.gridPosition.y, this.attackRange);
			} 
			//otherwise disable all Highlight
			else {
				moving = false;
				isAttacking = false;
				GameManager.instance.disableHightLight();
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
