﻿using UnityEngine;
using System.Collections;

public class Medic : Player {

	public const string className = "Medic";
	public const float baseDamage = 7.5f;
	public const float baseDefense = 10.0f;
	public int movementRange=3;
	public float HP;
	public bool isAttacking =false;
	public int attackRange = 1;
	private float bottonWidth;
	private float buttonWidth;
	
	public float attackHitRate = 0.75f;
	public float defenseReduceRate = 0.2f;
	
	void Start () {
		HP = 90.0f;
	}
	
	// Update is called once per frame
	public override void Update () {
		//Basic charactor color is blue
		//When a charactor is chosen, it's color will turn to cyan
		//When a charactor die, it will turn to black and destroy, check Player.cs Script Update()
		if(GameManager.instance.playerList.Count > 0 && this.HP > 0){
			if (GameManager.instance.playerList[GameManager.instance.currentPlayerIndex].GetComponent<Medic>() == this && GameManager.instance.playerList.Count > 0) {
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
			base.TurnUpdate ();
		}
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
		foreach (GameObject p in GameManager.instance.playerList) { //Checks for enemy class on tile target
			if(p.GetComponent<AlienShip>() != null){
				AlienShip target = null;
				AlienShip temp = p.GetComponent<AlienShip>(); //Gets enemy script
				
				if (temp.gridPosition == tile.gridPosition) { //Checks if tile selected contains enemy
					target = temp;
					MedicAttack.attackAlienShip(target); //Attacks the specific enemy unit
				}
			}
			else if(p.GetComponent<AlienSoldier>() != null){  //Checks for enemy class on tile target
				AlienSoldier target = null;
				AlienSoldier temp = p.GetComponent<AlienSoldier>(); //Gets enemy script				
				if (temp.gridPosition == tile.gridPosition) { //Checks if tile selected contains enemy
					target = temp;
					MedicAttack.attackAlienSoldier(target); //Attacks the specific enemy unit
				}
			}
			else if(p.GetComponent<AlienSupport>() != null){ //Checks for enemy class on tile target
				AlienSupport target = null;
				AlienSupport temp = p.GetComponent<AlienSupport>();//Gets enemy script				
				if (temp.gridPosition == tile.gridPosition) { //Checks if tile selected contains enemy
					target = temp;
					MedicAttack.attackAlienSupport(target); //Attacks the specific enemy unit
				}
			}
			else if(p.GetComponent<Berserker>() != null){ //Checks for enemy class on tile target
				Berserker target = null;
				Berserker temp = p.GetComponent<Berserker>(); //Gets enemy script			
				if (temp.gridPosition == tile.gridPosition) { //Checks if tile selected contains enemy
					target = temp;
					MedicAttack.attackAlienBerserker(target); //Attacks the specific enemy unit
				}
			}
			/**********TEST class************/
			else if(p.GetComponent<AiPlayer>() != null){ //Checks for enemy class on tile target
				AiPlayer target = null;
				AiPlayer temp = p.GetComponent<AiPlayer>(); //Gets enemy script		
				if (temp.gridPosition == tile.gridPosition) { //Checks if tile selected contains enemy
					target = temp;
					MedicAttack.attackAIPlayer(target); //Attacks the specific enemy unit
				}
			}
			
		}
		
	}

	//Return the class name of unit
	public override string roleName(){
		return className;
	}
	public virtual void TurnOnGUI(){
		float buttonHeight = 50;
		float buttonWidth = 100;
		
		Rect buttonRect = new Rect(0, Screen.height - buttonHeight * 3, buttonWidth, buttonHeight);
		if (GUI.Button(buttonRect, "Move")) {
			//if not moving, first disable all Highlight 
			//enable Move Highlight
			if (!moving) {
				GameManager.instance.disableHightLight();
				moving = true;
				isAttacking = false;
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
			//enable Attack Highlight
			if (!isAttacking) {
				GameManager.instance.disableHightLight();
				moving = false;
				isAttacking = true;
				
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
			//actionPoints = 2;
			moving = false;
			isAttacking = false;			
			GameManager.instance.nextTurn();
		}
		base.TurnOnGUI ();
	}
	
	//Display HP
	public void OnGUI(){
		Vector3 location = Camera.main.WorldToScreenPoint (transform.position)+ Vector3.up*30+ Vector3.left*15;
		GUI.TextArea(new Rect(location.x, Screen.height - location.y, 30, 20), HP.ToString());
	}
}
