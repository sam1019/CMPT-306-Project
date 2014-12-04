﻿using UnityEngine;
using System.Collections;

public class Soldier : Player {

	// character properties
	public const string className = "Soldier";
	public bool isAttacking =false;
	public float defenseReduceRate = 0.2f;
	private Animator anim;


	void Start () {
		this.HP = 100.0f;
		this.baseHP = 100.0f;
		this.baseDamage = 15.0f;
		this.baseDefense = 15.0f;
		this.movementRange = 6;
		this.attackRange = 4;
		this.attackHitRate = 0.95f;
		anim = gameObject.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	public override void Update () {

		//Basic charactor color is white
		//When a charactor is chosen, it's color will turn to cyan
		//When a charactor die, it will turn to black and destroy, check Player.cs Script Update()
		if(GameManager.instance.playerList.Count > 0 && this.HP > 0){

			if (GameManager.instance.playerList[GameManager.instance.currentPlayerIndex].GetComponent<Soldier>() == this 
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
		base.Update();
	}
	
	public override void TurnUpdate(){
		
		//Moving the player to destination
		if (Vector3.Distance(moveDestination, transform.position) > 0.1f) {
			SendMessage("Play","soldierMove");
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
			rocketInstance = Instantiate(rocketPrefab, transform.position, transform.rotation) as GameObject;
			rocketInstance.GetComponent<rocket>().moveDestination = tile.transform.position;
			foreach (GameObject p in GameManager.instance.playerList) { //Checks for enemy class on tile target

				SendMessage("Play","soldierHit");	//play soldier attack audio
				if(p.GetComponent<AlienShip>() != null){
					AlienShip target = null;
					AlienShip temp = p.GetComponent<AlienShip>(); //Gets enemy script
					
					if (temp.gridPosition == tile.gridPosition) { //Checks if tile selected contains enemy
						target = temp;
						SoldierAttack.attackAlienShip(target); //Attacks the specific enemy unit
					}
				}
				else if(p.GetComponent<AlienSoldier>() != null){ //Checks for enemy class on tile target
					AlienSoldier target = null;
					AlienSoldier temp = p.GetComponent<AlienSoldier>();	 //Gets enemy script	

					if (temp.gridPosition == tile.gridPosition) { //Checks if tile selected contains enemy
						target = temp;
						SoldierAttack.attackAlienSoldier(target); //Attacks the specific enemy unit
					}
				}
				else if(p.GetComponent<Berserker>() != null){ //Checks for enemy class on tile target
					Berserker target = null;
					Berserker temp = p.GetComponent<Berserker>(); //Gets enemy script	 	

					if (temp.gridPosition == tile.gridPosition) { //Checks if tile selected contains enemy
						target = temp;
						SoldierAttack.attackAlienBerserker(target); //Attacks the specific enemy unit
					}
				}
				/**********TEST class************/
				else if(p.GetComponent<AiPlayer>() != null){ //Checks for enemy class on tile target
					AiPlayer target = null;
					AiPlayer temp = p.GetComponent<AiPlayer>();	//Gets enemy script	 

					if (temp.gridPosition == tile.gridPosition) { //Checks if tile selected contains enemy
						target = temp;
						SoldierAttack.attackAIPlayer(target); //Attacks the specific enemy unit
						audio.Play ();
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

		GUI.skin = TurnGUISkin;
		float buttonHeight = Screen.height / 3;
		float buttonWidth = Screen.width / 4;
		float pauseHeight = Screen.height / 3;

		GUI.Box(new Rect (0, pauseHeight, buttonWidth, Screen.height-pauseHeight),
		        "GAME INFO\n"+"Charactor: "+roleName()+"\nHP: "+(int)this.HP+"\nBase Damage: "+this.baseDamage+"\nDefence: "+this.baseDefense
		        +"\nAttackHitRate: "+this.attackHitRate+"\nMovement Range: "+this.movementRange+"\nAttack Range: "+this.attackRange,"Box");

		// move button
		Rect buttonRect = new Rect(Screen.width - buttonWidth, Screen.height - buttonHeight * 3, buttonWidth, buttonHeight);
		if(!moveTurn){
			if (GUI.Button(buttonRect, MoveButtonTexture)) {
				//if not moving, first disable all Highlight 
				//enable Move Highlight
				moving = false;
				if (!moving){
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
			buttonRect = new Rect(Screen.width - buttonWidth, Screen.height - buttonHeight * 2, buttonWidth, buttonHeight);
			
			if (GUI.Button(buttonRect, AttackButtonTexture)) {
				//if not attacking, first disable all Highlight 
				//enable Attack Highlig
				isAttacking = false;
				if (!isAttacking)  {
					GameManager.instance.disableHightLight();
					moving = false;
					isAttacking = true;
					//Enables attack hightlight range
					print (this.attackRange);
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
		buttonRect = new Rect(Screen.width - buttonWidth, Screen.height - buttonHeight * 1, buttonWidth, buttonHeight);		
		
		if (GUI.Button(buttonRect, EndTurnButtonTexture)) { 
			moving = false;
			isAttacking = false;
			moveTurn = false;
			attackTurn =false;
			GameManager.instance.nextTurn();
		}
		base.TurnOnGUI ();
	}

}
