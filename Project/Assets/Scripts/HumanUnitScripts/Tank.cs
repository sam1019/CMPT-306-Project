﻿using UnityEngine;
using System.Collections;

public class Tank : Player {
	
	// Use this for initialization
	public const string className = "Tank";
	public const float baseDamage = 45.0f;
	public const float baseDefense = 40.0f;
	public int movementRange=2;
	public float HP;
	
	public int attackRange = 1;
	private float bottonWidth;
	private float buttonWidth;
	
	public float attackHitRate = 0.8f;
	public float defenseReduceRate = 0.2f;
	public bool isAttacking =false;

	private Animator anim;

	void Awake(){
		//Setting the destination to it's spawn
		moveDestination = transform.position;
	}
	
	void Start () {
		HP = 150.0f;
		anim = gameObject.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	public override void Update () {
		//Basic charactor color is blue
		//When a charactor is chosen, it's color will turn to cyan
		//When a charactor die, it will turn to black and destroy, check Player.cs Script Update()
		if(GameManager.instance.playerList.Count > 0 && this.HP > 0){
			if (GameManager.instance.playerList[GameManager.instance.currentPlayerIndex].GetComponent<Tank>() == this && GameManager.instance.playerList.Count > 0) {
				anim.SetBool("focus", true); //when in its turn, play animation
				transform.renderer.material.color = Color.cyan;

			}
			//Otherwise charactor is blue
			else {
				transform.renderer.material.color = Color.blue;
				anim.SetBool("focus", false);//when out its turn, play idle animation
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
			moving=false;
			//Used to check if the player has reached it's destination, if so next turn
			if (Vector3.Distance(moveDestination, transform.position) <= 0.1f) {
				transform.position = moveDestination;// + Vector3.back;
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
	
	
	public void getEnemyToAttack(Tile tile){
		foreach (GameObject p in GameManager.instance.playerList) {
			if(p.GetComponent<AlienShip>() != null){
				AlienShip target = null;
				AlienShip temp = p.GetComponent<AlienShip>();
				
				if (temp.gridPosition == tile.gridPosition) { //Checks if tile selected contains enemy
					target = temp;
					TankAttack.attackAlienShip(target);
				}
			}
			else if(p.GetComponent<AlienSoldier>() != null){
				AlienSoldier target = null;
				AlienSoldier temp = p.GetComponent<AlienSoldier>();				
				if (temp.gridPosition == tile.gridPosition) { //Checks if tile selected contains enemy
					target = temp;
					TankAttack.attackAlienSoldier(target);
				}
			}
			else if(p.GetComponent<AlienSupport>() != null){
				AlienSupport target = null;
				AlienSupport temp = p.GetComponent<AlienSupport>();				
				if (temp.gridPosition == tile.gridPosition) { //Checks if tile selected contains enemy
					target = temp;
					TankAttack.attackAlienSupport(target);
				}
			}
			else if(p.GetComponent<Berserker>() != null){
				Berserker target = null;
				Berserker temp = p.GetComponent<Berserker>();				
				if (temp.gridPosition == tile.gridPosition) { //Checks if tile selected contains enemy
					target = temp;
					TankAttack.attackAlienBerserker(target);
				}
			}
			/**********TEST class************/
			else if(p.GetComponent<AiPlayer>() != null){
				AiPlayer target = null;
				AiPlayer temp = p.GetComponent<AiPlayer>();				
				if (temp.gridPosition == tile.gridPosition) { //Checks if tile selected contains enemy
					target = temp;
					TankAttack.attackAIPlayer(target);
				}
			}
			
		}
		
	}
	
	public override string roleName(){
		return className;
	}
	public virtual void TurnOnGUI(){
		float buttonHeight = 50;
		float buttonWidth = 100;

		//move button
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