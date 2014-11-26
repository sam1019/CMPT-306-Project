using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AiPlayer : Player {

	/*
	 * Variables
	 */
	

	private List<Tile> moveTiles = new List<Tile>();
	private List<Player> targets = new List<Player> ();
	private List<Player> ableToBeKilledTargets = new List<Player> ();
	private List<List<GameObject>> stateTree;

	// Decision tree code
	private const int ATTACK = 0;
	private const int KILL_ONE = 1;
	private const int CHOOSE_HIGH_HP = 2;
	private const int ATTACK_MOST_DEMAGE = 3;
	private const int MOVE_TO_PLAYER = 4;
	private const int MOVE_TO_ENEMY = 5;

	private bool isPlayerInAttackRange = false;
	private bool existPlayerBeKilled = false;

	public bool isDecisionMade = false;

	public int preferenceTileX, preferenceTileY;
	
	/*
	 * Class Methods
	 */

	public  void Start () {
		stateTree = new List<List<GameObject>> ();
	}

	// Update is called once per frame
	public override void Update () {}

	public override void TurnUpdate (){
		base.TurnUpdate ();
	}

	private bool anyPlayerInAttackRangeHelper(Tile targetTile){

		foreach (GameObject p in GameManager.instance.playerList) { //Checks for enemy class on tile target

			if(p.GetComponent<Tank>() != null){
				Tank temp = p.GetComponent<Tank>(); //Gets enemy script
				if (temp.gridPosition == targetTile.gridPosition) { //Checks if tile selected contains enemy
					targets.Add(temp);
					return true;
				}
			}
			else if(p.GetComponent<Jet>() != null){ //Checks for enemy class on tile target
				Jet temp = p.GetComponent<Jet>();	//Gets enemy script	
				if (temp.gridPosition == targetTile.gridPosition) { //Checks if tile selected contains enemy
					targets.Add(temp);
					return true;
				}
			}
			else if(p.GetComponent<Soldier>() != null){ //Checks for enemy class on tile target
				Soldier temp = p.GetComponent<Soldier>();	//Gets enemy script			
				if (temp.gridPosition == targetTile.gridPosition) { //Checks if tile selected contains enemy
					targets.Add(temp);
					return true;
				}
			}
		}
		return false;
	}

	private void anyPlayerInAttackRange() {
		/* Iterate through the map to highlight the tiles that in its move range
		 * different part of the map has different methods to choose tiles
		 */
		if (this.gridPosition.x >= this.gridPosition.y) {
			for (int i = 0; i < GameManager.instance.mapSize; i++) {
				for (int j = 0; j < GameManager.instance.mapSize; j++) {
					
					if( i + j <= this.gridPosition.x + this.gridPosition.y + this.attackRange && i + j >= this.gridPosition.x + this.gridPosition.y - this.attackRange 
					   && i<= this.gridPosition.x + this.attackRange && i >= this.gridPosition.x - this.attackRange &&  j<= this.gridPosition.y + this.attackRange && j >= this.gridPosition.y - this.attackRange
					   && (i - j) <= Mathf.Abs(this.gridPosition.x - this.gridPosition.y) + this.attackRange &&  (i - j) >= Mathf.Abs(this.gridPosition.x - this.gridPosition.y) - this.attackRange){
						if(anyPlayerInAttackRangeHelper(GameManager.instance.map [i] [j].GetComponent<Tile> ())){
							isPlayerInAttackRange = true;
						}
						GameManager.instance.map [i] [j].GetComponent<Tile> ().transform.renderer.material.color = Color.red;
						//System.Threading.Thread.Sleep(1000);
					}
				}
			}
		}
		else if(this.gridPosition.x < this.gridPosition.y){
			for (int i = 0; i < GameManager.instance.mapSize; i++) {
				for (int j = 0; j < GameManager.instance.mapSize; j++) {
					
					if( i + j <= this.gridPosition.x + this.gridPosition.y + this.attackRange && i + j >= this.gridPosition.x + this.gridPosition.y - this.attackRange 
					   && i<= this.gridPosition.x + this.attackRange && i >= this.gridPosition.x - this.attackRange &&  j<= this.gridPosition.y + this.attackRange && j >= this.gridPosition.y - this.attackRange
					   && (i - j) <= (this.gridPosition.x - this.gridPosition.y + this.attackRange) &&  (i - j) >= (this.gridPosition.x - this.gridPosition.y - this.attackRange)){
						if(anyPlayerInAttackRangeHelper(GameManager.instance.map [i] [j].GetComponent<Tile> ())){
							isPlayerInAttackRange = true;
						}
						GameManager.instance.map [i] [j].GetComponent<Tile> ().transform.renderer.material.color = Color.red;
						//System.Threading.Thread.Sleep(1000);
					}
				}
			}
		}
	}

	private void targetCanbeKilledInThisRound() {
		foreach (Player p in targets) { 
			
			if(p.GetComponent<Tank>() != null){
				Tank temp = p.GetComponent<Tank>();
				if (temp.HP - ((this.K / (this.K + temp.baseDefense)) * this.baseDamage) < 0) {
					ableToBeKilledTargets.Add(temp);
					existPlayerBeKilled = true;
				}
			}
			else if(p.GetComponent<Jet>() != null){ 
				Jet temp = p.GetComponent<Jet>();	
				if (temp.HP - ((this.K / (this.K + temp.baseDefense)) * this.baseDamage) < 0) {
					ableToBeKilledTargets.Add(temp);
					existPlayerBeKilled = true;
				}
			}
			else if(p.GetComponent<Soldier>() != null){ 
				Soldier temp = p.GetComponent<Soldier>();
				if (temp.HP - ((this.K / (this.K + temp.baseDefense)) * this.baseDamage) < 0) {
					ableToBeKilledTargets.Add(temp);
					existPlayerBeKilled = true;
				}
			}
		}
	}

	// Decision tree
	public int decisionTree() {
		anyPlayerInAttackRange ();
		if (isPlayerInAttackRange) {
			Debug.Log("isPlayerInAttackRange == true");
			if(targets.Count == 1) {
				Debug.Log("targets.Count == 1");
				return ATTACK;
			} else {
				Debug.Log("targets.Count != 1");
				if(existPlayerBeKilled) {
					Debug.Log("existPlayerBeKilled == true");
					if(ableToBeKilledTargets.Count == 1) {
						Debug.Log("ableToBeKilledTargets.Count == 1");
						return KILL_ONE;
					} else {
						Debug.Log("ableToBeKilledTargets.Count != 1");
						return CHOOSE_HIGH_HP;
					}
				} else {
					Debug.Log("existPlayerBeKilled == false");
					return ATTACK_MOST_DEMAGE;
				}
			}

		} else {
			Debug.Log("isPlayerInAttackRange == false");
			if(this.HP > this.baseHP * 0.7) {
				Debug.Log("HPHigh == true");
				return MOVE_TO_PLAYER;
			} else {
				Debug.Log("HPHigh == false");
				if(GameManager.instance.aiList.Count <= 1) {
					Debug.Log("GameManager.instance.aiList.Count <= 1");
					return MOVE_TO_ENEMY;
				} else {
					Debug.Log("GameManager.instance.aiList.Count > 1");
					return MOVE_TO_PLAYER;
				}
			}
		}
	}

	private float calculatePreference (int x, int y, Player target, int K) {
		return (this.HP - target.HP) + 
			(1.0f / Mathf.Sqrt((float)Mathf.Pow(target.gridPosition.x - x, 2) + 
			(float)Mathf.Pow(target.gridPosition.y - y, 2))) + 
			this.attackBonus * K;
	}

	public void findHighestPreferenceTile() {
		/* Iterate through the map to highlight the tiles that in its move range
		 * different part of the map has different methods to choose tiles
		 */
		float highestPreference = -1000.0f;
		if (this.gridPosition.x >= this.gridPosition.y) {
			for (int i = 0; i < GameManager.instance.mapSize; i++) {
				for (int j = 0; j < GameManager.instance.mapSize; j++) {
					
					if( i + j <= this.gridPosition.x + this.gridPosition.y + this.movementRange && i + j >= this.gridPosition.x + this.gridPosition.y - this.movementRange 
					   && i<= this.gridPosition.x + this.movementRange && i >= this.gridPosition.x - this.movementRange &&  j<= this.gridPosition.y + this.movementRange && j >= this.gridPosition.y - this.movementRange
					   && (i - j) <= Mathf.Abs(this.gridPosition.x - this.gridPosition.y) + this.movementRange &&  (i - j) >= Mathf.Abs(this.gridPosition.x - this.gridPosition.y) - this.movementRange){
						float tempPreference = 0;
						foreach (GameObject p in GameManager.instance.playerList) { //Checks for enemy class on tile target
							
							if(p.GetComponent<Tank>() != null){
								Tank temp = p.GetComponent<Tank>(); //Gets enemy script
								tempPreference = tempPreference + calculatePreference (j, i, temp, 1);
							}
							else if(p.GetComponent<Jet>() != null){ //Checks for enemy class on tile target
								Jet temp = p.GetComponent<Jet>();	//Gets enemy script	
								tempPreference = tempPreference + calculatePreference (j, i, temp, 1);
							}
							else if(p.GetComponent<Soldier>() != null){ //Checks for enemy class on tile target
								Soldier temp = p.GetComponent<Soldier>();	//Gets enemy script			
								tempPreference = tempPreference + calculatePreference (j, i, temp, 1);
							}
						}
						//Debug.Log(tempPreference);
						if(tempPreference > highestPreference) {
							highestPreference = tempPreference;
							preferenceTileX = j;
							preferenceTileY = i;
						}
					}
				}
			}
		}
		else if(this.gridPosition.x < this.gridPosition.y){
			for (int i = 0; i < GameManager.instance.mapSize; i++) {
				for (int j = 0; j < GameManager.instance.mapSize; j++) {
					
					if( i + j <= this.gridPosition.x + this.gridPosition.y + this.movementRange && i + j >= this.gridPosition.x + this.gridPosition.y - this.movementRange 
					   && i<= this.gridPosition.x + this.movementRange && i >= this.gridPosition.x - this.movementRange &&  j<= this.gridPosition.y + this.movementRange && j >= this.gridPosition.y - this.movementRange
					   && (i - j) <= (this.gridPosition.x - this.gridPosition.y + this.movementRange) &&  (i - j) >= (this.gridPosition.x - this.gridPosition.y - this.movementRange)){
						float tempPreference = 0;
						foreach (GameObject p in GameManager.instance.playerList) { //Checks for enemy class on tile target
							
							if(p.GetComponent<Tank>() != null){
								Tank temp = p.GetComponent<Tank>(); //Gets enemy script
								tempPreference = tempPreference + calculatePreference (j, i, temp, 1);
							}
							else if(p.GetComponent<Jet>() != null){ //Checks for enemy class on tile target
								Jet temp = p.GetComponent<Jet>();	//Gets enemy script	
								tempPreference = tempPreference + calculatePreference (j, i, temp, 1);
							}
							else if(p.GetComponent<Soldier>() != null){ //Checks for enemy class on tile target
								Soldier temp = p.GetComponent<Soldier>();	//Gets enemy script			
								tempPreference = tempPreference + calculatePreference (j, i, temp, 1);
							}
						}
						if(tempPreference > highestPreference) {
							highestPreference = tempPreference;
							preferenceTileX = j;
							preferenceTileY = i;
						}
					}
				}
			}
		}
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
						JetAttack.attackAlienShip(target); //Attacks the specific enemy unit
					}
				}
				else if(p.GetComponent<AlienSoldier>() != null){ //Checks for enemy class on tile target
					AlienSoldier target = null;
					AlienSoldier temp = p.GetComponent<AlienSoldier>();	//Gets enemy script		
					
					if (temp.gridPosition == tile.gridPosition) { //Checks if tile selected contains enemy
						target = temp;
						JetAttack.attackAlienSoldier(target); //Attacks the specific enemy unit
					}
				}
				else if(p.GetComponent<AlienSupport>() != null){ //Checks for enemy class on tile target
					AlienSupport target = null;
					AlienSupport temp = p.GetComponent<AlienSupport>();	//Gets enemy script			
					
					if (temp.gridPosition == tile.gridPosition) { //Checks if tile selected contains enemy
						target = temp;
						JetAttack.attackAlienSupport(target); //Attacks the specific enemy unit
					}
				}
				else if(p.GetComponent<Berserker>() != null){ //Checks for enemy class on tile target
					Berserker target = null;
					Berserker temp = p.GetComponent<Berserker>(); //Gets enemy script	
					
					if (temp.gridPosition == tile.gridPosition) { //Checks if tile selected contains enemy
						target = temp;
						JetAttack.attackAlienBerserker(target); //Attacks the specific enemy unit
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
	}

	public void attackAction0(Player target) {
		// used for checking the available tile when destination tile is occupied
		bool upAvailable = false;
		bool downAvailable = false;
		bool leftAvailable = false;
		bool rightAvailable = false;

		int targetX = (int) target.gridPosition.x;
		int targetY = (int) target.gridPosition.y;
		
		// Check if the destination is out of border
		if (targetY + 1 >= 0 && targetY + 1 < GameManager.instance.mapSize && !GameManager.instance.map [targetX] [targetY + 1].GetComponent<Tile> ().isOccupied) upAvailable = true;
		if (targetY - 1 >= 0 && targetY - 1 < GameManager.instance.mapSize && !GameManager.instance.map [targetX] [targetY - 1].GetComponent<Tile> ().isOccupied) downAvailable = true;
		if (targetX - 1 >= 0 && targetX - 1 < GameManager.instance.mapSize && !GameManager.instance.map [targetX - 1] [targetY].GetComponent<Tile> ().isOccupied) leftAvailable = true;
		if (targetX + 1 >= 0 && targetX + 1 < GameManager.instance.mapSize && !GameManager.instance.map [targetX + 1] [targetY].GetComponent<Tile> ().isOccupied) rightAvailable = true;

		Tile destTile = null;
		if(upAvailable) destTile = GameManager.instance.map [targetX] [targetY + 1].GetComponent<Tile> ();
		if(downAvailable) destTile = GameManager.instance.map [targetX] [targetY - 1].GetComponent<Tile> ();
		if(leftAvailable) destTile = GameManager.instance.map [targetX - 1] [targetY].GetComponent<Tile> ();
		if(rightAvailable) destTile = GameManager.instance.map [targetX + 1] [targetY].GetComponent<Tile> ();

		GameManager.instance.moveAlien (destTile);
	}
	
	public void moveToHighPrefenceAction() {

		// used for checking the available tile when destination tile is occupied
		bool upAvailable = false;
		bool downAvailable = false;
		bool leftAvailable = false;
		bool rightAvailable = false;

		// Check if the destination is out of border
		if (preferenceTileY + 1 >= 0 && preferenceTileY + 1 < GameManager.instance.mapSize && !GameManager.instance.map [preferenceTileX] [preferenceTileY + 1].GetComponent<Tile> ().isOccupied) upAvailable = true;
		if (preferenceTileY - 1 >= 0 && preferenceTileY - 1 < GameManager.instance.mapSize && !GameManager.instance.map [preferenceTileX] [preferenceTileY - 1].GetComponent<Tile> ().isOccupied) downAvailable = true;
		if (preferenceTileX - 1 >= 0 && preferenceTileX - 1 < GameManager.instance.mapSize && !GameManager.instance.map [preferenceTileX - 1] [preferenceTileY].GetComponent<Tile> ().isOccupied) leftAvailable = true;
		if (preferenceTileX + 1 >= 0 && preferenceTileX + 1 < GameManager.instance.mapSize && !GameManager.instance.map [preferenceTileX + 1] [preferenceTileY].GetComponent<Tile> ().isOccupied) rightAvailable = true;
		
		Tile destTile = null;
		
		if (GameManager.instance.map [preferenceTileX] [preferenceTileY].GetComponent<Tile> ().isOccupied) {
			if(upAvailable) destTile = GameManager.instance.map [preferenceTileX] [preferenceTileY + 1].GetComponent<Tile> ();
			if(downAvailable) destTile = GameManager.instance.map [preferenceTileX] [preferenceTileY - 1].GetComponent<Tile> ();
			if(leftAvailable) destTile = GameManager.instance.map [preferenceTileX - 1] [preferenceTileY].GetComponent<Tile> ();
			if(rightAvailable) destTile = GameManager.instance.map [preferenceTileX + 1] [preferenceTileY].GetComponent<Tile> ();
		} else {
			destTile = GameManager.instance.map [preferenceTileX] [preferenceTileY].GetComponent<Tile> ();
		}

		GameManager.instance.moveAlien (destTile);
	}

	public void AttackHelper(Player target){

		target.HP = target.HP - this.baseDamage * (K / (K + target.baseDefense));
		attackTurn = true;
		if (moveTurn&&attackTurn) {
			moveTurn = false;
			attackTurn = false;
			this.isDecisionMade = false;
			GameManager.instance.nextTurn ();
		}
	}
}
