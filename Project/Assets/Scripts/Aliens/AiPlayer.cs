using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AiPlayer : Player {

	// ============= Variables =============
		
	public List<Player> targets = new List<Player> ();
	public List<Player> ableToBeKilledTargets = new List<Player> ();

	// Decision tree code
	public int ATTACK = 0;
	public int KILL_ONE = 1;
	public int CHOOSE_HIGH_HP = 2;
	public int ATTACK_MOST_DEMAGE = 3;
	public int MOVE_TO_PLAYER = 4;
	public int MOVE_TO_ENEMY = 5;

	// speed constant to slow down the ai animation (higher -> slower)
	public int SPEED_CONSTANT = 1;

	private bool isPlayerInAttackRange = false;
	private bool existPlayerBeKilled = false;

	public bool isDecisionMade = false;
	public bool moveToAttack = false;

	// attack target
	public Player target = null;

	public bool decisionExecuted = false;
	public bool hasDecreasePlayerCount = false;
	public bool noNeedToMove = false;

	public int preferenceTileX = -1;
	public int preferenceTileY = -1;

	public int decisionTreeReturnedCode = -1;

	public int HighHPforAbleToKillIndex = -1;
	public int MostDamageTargetIndex = -1;


	// ============= Class Methods =============

	public  void Start () {}

	// Update is called once per frame
	public override void Update () {}



	public override void TurnUpdate (){
		base.TurnUpdate ();
	}



	/*
	 * helper function for anyPlayerInAttackRange()
	 */ 
	private bool anyPlayerInAttackRangeHelper(Tile targetTile){

		foreach (GameObject p in GameManager.instance.playerList) { //Checks for enemy class on tile target

			if(p.GetComponent<Tank>() != null){
				Tank temp = p.GetComponent<Tank>(); //Gets enemy script
				if (temp.gridPosition == targetTile.gridPosition) { //Checks if tile selected contains enemy
					targets.Add(temp);
					Debug.Log("Tank is added into target list");
					return true;
				}
			}
			else if(p.GetComponent<Jet>() != null){ //Checks for enemy class on tile target
				Jet temp = p.GetComponent<Jet>();	//Gets enemy script	
				if (temp.gridPosition == targetTile.gridPosition) { //Checks if tile selected contains enemy
					targets.Add(temp);
					Debug.Log("Jet is added into target list");
					return true;
				}
			}
			else if(p.GetComponent<Soldier>() != null){ //Checks for enemy class on tile target
				Soldier temp = p.GetComponent<Soldier>();	//Gets enemy script	
				if (temp.gridPosition == targetTile.gridPosition) { //Checks if tile selected contains enemy
					targets.Add(temp);
					Debug.Log("Soldier is added into target list");
					return true;
				}
			}
		}
		return false;
	}



	// find if there exists a target in the AI attackable range (attack range + move range)
	private void anyPlayerInAttackRange() {

		List<Tile> listTiles = PathFinding.pathFindingReturnList ((int)this.gridPosition.x, (int)this.gridPosition.y, this.attackRange + this.movementRange);

		for (int i = 0; i < listTiles.Count; i++) {
			if(anyPlayerInAttackRangeHelper(listTiles[i])){
				isPlayerInAttackRange = true;
			}
			//listTiles[i].transform.renderer.material.color = Color.red;
		}
	}



	// find if there exists a target can be killed in this round
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



	/*
	 * Decision tree, return a dicison code
	 */ 
	public int decisionTree() {
		anyPlayerInAttackRange ();
		if (isPlayerInAttackRange) {
			Debug.Log("isPlayerInAttackRange == true");
			if(targets.Count == 1) {
				Debug.Log("targets.Count == 1");
				return ATTACK;
			} else {
				Debug.Log("targets.Count != 1");
				targetCanbeKilledInThisRound();
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
					return MOVE_TO_PLAYER;
				} else {
					Debug.Log("GameManager.instance.aiList.Count > 1");
					return MOVE_TO_PLAYER;
				}
			}
		}
	}



	/*
	 * Preferenc algorithm
	 */ 
	private float calculatePreference (int x, int y, Player target, int K) {
		return (this.HP - target.HP) + 
			(1.0f / Mathf.Sqrt((float)Mathf.Pow(target.gridPosition.x - x, 2) + 
			(float)Mathf.Pow(target.gridPosition.y - y, 2))) + 
			this.attackBonus * K;
	}



	/*
	 * Find the highest preference tile within movement tiles
	 */ 
	public void findHighestPreferenceTile(int lookAhead) {

		// initial the highest preference for comparing
		float highestPreference = -90000.0f;

		// get list by pathfinding
		List<Tile> tempList = PathFinding.pathFindingReturnList ((int)this.gridPosition.x, (int)this.gridPosition.y, this.movementRange);

		for (int i = 0; i < tempList.Count; i++) {

			if(!tempList[i].isOccupied) {

				float tempPreference = 0;
				foreach (GameObject p in GameManager.instance.playerList) { //Checks for enemy class on tile target
					
					if(p.GetComponent<Tank>() != null){
						Tank temp = p.GetComponent<Tank>(); //Gets enemy script
						tempPreference = tempPreference + calculatePreference ((int)tempList[i].gridPosition.x, (int)tempList[i].gridPosition.y, temp, 1);
					}
					else if(p.GetComponent<Jet>() != null){ //Checks for enemy class on tile target
						Jet temp = p.GetComponent<Jet>();	//Gets enemy script	
						tempPreference = tempPreference + calculatePreference ((int)tempList[i].gridPosition.x, (int)tempList[i].gridPosition.y, temp, 1);
					}
					else if(p.GetComponent<Soldier>() != null){ //Checks for enemy class on tile target
						Soldier temp = p.GetComponent<Soldier>();	//Gets enemy script			
						tempPreference = tempPreference + calculatePreference ((int)tempList[i].gridPosition.x, (int)tempList[i].gridPosition.y, temp, 1);
					}
				}

				// highlight movement tile
				tempList[i].transform.renderer.material.color = Color.blue;

				//tempPreference = tempPreference + findHighestPreferenceTileWithLoodAhead(lookAhead, tempList[i], this.movementRange);

				if(tempPreference > highestPreference) {
					highestPreference = tempPreference;
					preferenceTileX = (int)tempList[i].gridPosition.x;
					preferenceTileY = (int)tempList[i].gridPosition.y;
				}
			}
		}
	}
	


	/*
	 * TODO: AI Look Ahead, shut down since it's seriously affecting the performance
	 */
	/*
	public float findHighestPreferenceTileWithLoodAhead(int lookAhead, Tile tile, int moveRange) {

		int tileX = (int)tile.gridPosition.x;
		int tileY = (int)tile.gridPosition.y;
		/* Iterate through the map to highlight the tiles that in its move range
		 * different part of the map has different methods to choose tiles
		 */
	/*
		if (lookAhead == -1) return 0;

		if (lookAhead == 0) {

			float total = 0;

			if (tileX >= tileY) {
				for (int i = 0; i < GameManager.instance.mapSize; i++) {
					for (int j = 0; j < GameManager.instance.mapSize; j++) {
						
						if( i + j <= tileX + tileY + moveRange && i + j >= tileX + tileY - moveRange 
						   && i<= tileX + moveRange && i >= tileX - moveRange &&  j<= tileY + moveRange && j >= tileY - moveRange
						   && (i - j) <= Mathf.Abs(tileX - tileY) + moveRange &&  (i - j) >= Mathf.Abs(tileX - tileY) - moveRange){
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
							total = total + tempPreference;
						}
					}
				}
			}
			else if(tileX < tileY){
				for (int i = 0; i < GameManager.instance.mapSize; i++) {
					for (int j = 0; j < GameManager.instance.mapSize; j++) {
						
						if( i + j <= tileX + tileY + moveRange && i + j >= tileX + tileY - moveRange 
						   && i<= tileX + moveRange && i >= tileX - moveRange &&  j<= tileY + moveRange && j >= tileY - moveRange
						   && (i - j) <= (tileX - tileY + moveRange) &&  (i - j) >= (tileX - tileY - moveRange)){
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
							total = total + tempPreference;
						}
					}
				}
			}

			return total;
		}

		else 
		
		{
			float total2 = 0;
			if (tileX >= tileY) {
				for (int i = 0; i < GameManager.instance.mapSize; i++) {
					for (int j = 0; j < GameManager.instance.mapSize; j++) {
						
						if( i + j <= tileX + tileY + moveRange && i + j >= tileX + tileY - moveRange 
						   && i<= tileX + moveRange && i >= tileX - moveRange &&  j<= tileY + moveRange && j >= tileY - moveRange
						   && (i - j) <= Mathf.Abs(tileX - tileY) + moveRange &&  (i - j) >= Mathf.Abs(tileX - tileY) - moveRange){

							float tempPreference = 0.0f;

							foreach (GameObject p in GameManager.instance.playerList) { //Checks for enemy class on tile target
								
								if(p.GetComponent<Tank>() != null){
									Tank temp = p.GetComponent<Tank>(); //Gets enemy script
									tempPreference = calculatePreference (j, i, temp, 1);
								}
								else if(p.GetComponent<Jet>() != null){ //Checks for enemy class on tile target
									Jet temp = p.GetComponent<Jet>();	//Gets enemy script	
									tempPreference = calculatePreference (j, i, temp, 1);
								}
								else if(p.GetComponent<Soldier>() != null){ //Checks for enemy class on tile target
									Soldier temp = p.GetComponent<Soldier>();	//Gets enemy script			
									tempPreference = calculatePreference (j, i, temp, 1);
								}
							}
							tempPreference = tempPreference + findHighestPreferenceTileWithLoodAhead(lookAhead - 1, GameManager.instance.map [i] [j].GetComponent<Tile> (), moveRange);
							total2 = total2 + tempPreference;
						}
					}
				}
			}
			else if(tileX < tileY){
				for (int i = 0; i < GameManager.instance.mapSize; i++) {
					for (int j = 0; j < GameManager.instance.mapSize; j++) {
						
						if( i + j <= tileX + tileY + moveRange && i + j >= tileX + tileY - moveRange 
						   && i<= tileX + moveRange && i >= tileX - moveRange &&  j<= tileY + moveRange && j >= tileY - moveRange
						   && (i - j) <= (tileX - tileY + moveRange) &&  (i - j) >= (tileX - tileY - moveRange)){

							float tempPreference = 0.0f;
							
							foreach (GameObject p in GameManager.instance.playerList) { //Checks for enemy class on tile target
								
								if(p.GetComponent<Tank>() != null){
									Tank temp = p.GetComponent<Tank>(); //Gets enemy script
									tempPreference = calculatePreference (j, i, temp, 1);
								}
								else if(p.GetComponent<Jet>() != null){ //Checks for enemy class on tile target
									Jet temp = p.GetComponent<Jet>();	//Gets enemy script	
									tempPreference = calculatePreference (j, i, temp, 1);
								}
								else if(p.GetComponent<Soldier>() != null){ //Checks for enemy class on tile target
									Soldier temp = p.GetComponent<Soldier>();	//Gets enemy script			
									tempPreference = calculatePreference (j, i, temp, 1);
								}
							}
							tempPreference = tempPreference + findHighestPreferenceTileWithLoodAhead(lookAhead - 1, GameManager.instance.map [i] [j].GetComponent<Tile> (), moveRange);
							total2 = total2 + tempPreference;
						}
					}
				}
			}

			return total2;
		}
	}
	*/



	/*
	 * Move AI character to attackable range to attack target
	 */ 
	public void moveToAttackableRangeAction() {

		int targetX = (int) target.gridPosition.x;
		int targetY = (int) target.gridPosition.y;

		Debug.Log ("Target X: " + targetX);
		Debug.Log ("Target Y: " + targetY);

		Tile destTile = null;
		List<Tile> temp = new List<Tile> ();

		List<Tile> attackTiles = new List<Tile>(); 
		List<Tile> moveTiles = new List<Tile>();


		// generate two list using pathfinding to find out the tiles which are attackable and not out of move range
		attackTiles = PathFinding.pathFindingReturnList ((int)target.gridPosition.x, (int)target.gridPosition.y, this.attackRange);
		moveTiles = PathFinding.pathFindingReturnList ((int)this.gridPosition.x, (int)this.gridPosition.y, this.movementRange);

		Debug.Log (attackTiles.Count);
		Debug.Log (moveTiles.Count);

		// Highlight the move range tiles
		for (int i = 0; i < moveTiles.Count; i ++) {
			if(!moveTiles[i].isOccupied) {
				moveTiles[i].transform.renderer.material.color = Color.blue;
			}
		}

		// find the common tiles which means AI can move there to attack
		for (int i = 0; i < attackTiles.Count; i ++) {

			if (!attackTiles[i].isOccupied) {

				for (int j = 0; j < moveTiles.Count; j++) {

					if ((int)attackTiles[i].gridPosition.x == (int)moveTiles[j].gridPosition.x && (int)attackTiles[i].gridPosition.y == (int)moveTiles[j].gridPosition.y) {
						temp.Add(moveTiles[j]);
					}
				}
			}
		}


		if (temp.Count == 0) {
			// ai is near by target, no need to move
			destTile = GameManager.instance.map [(int)this.gridPosition.x] [(int)this.gridPosition.y].GetComponent<Tile> ();
		} else {
			// randomly pick one tile to move there
			int randIndex = Random.Range (0, temp.Count);
			destTile = temp[randIndex];
		}

		// 
		if (destTile == null) {
			Debug.LogError("Can't find an attackable tile");
			GameManager.instance.nextTurn();
		}

		this.moveToAttack = true;

		if (destTile.Equals(GameManager.instance.map [(int)this.gridPosition.x] [(int)this.gridPosition.y].GetComponent<Tile> ())) {
			Debug.LogWarning("Same Tile");
			noNeedToMove = true;
			return;
		} else {
			GameManager.instance.moveAlien (destTile);
		}
	}



	/*
	 * Set destination for moveAilien() in GameManager
	 */ 
	public void moveToHighPrefenceAction() {

		// no need to move since ai can attack directly
		if(preferenceTileX == -1 || preferenceTileY == -1) {
			noNeedToMove = true;
			return;
		}

		Tile destTile = GameManager.instance.map [preferenceTileX] [preferenceTileY].GetComponent<Tile> ();

		GameManager.instance.moveAlien (destTile);
	}



	/*
	 * Attack the target which local target variable refers to
	 */ 
	public void doAttack(){

		Debug.Log("Doing Attack");
		Debug.Log ("target x: " + target.gridPosition.x);
		Debug.Log ("target y: " + target.gridPosition.y);

		// showing animation
		rocketInstance = Instantiate(rocketPrefab, transform.position, transform.rotation) as GameObject;
		rocketInstance.GetComponent<rocket>().moveDestination = target.transform.position;

		// damage algorithm
		target.HP = target.HP - this.baseDamage * (K / (K + target.baseDefense));
	}



	/*
	 * Find the highest hp target
	 */ 
	public void findHighHPforAbleToKill() {

		int tempHighestHP = (int) ableToBeKilledTargets[0].GetComponent<Player>().HP;
		HighHPforAbleToKillIndex = 0;

		for (int i = 1; i < ableToBeKilledTargets.Count; i ++) {
			if(tempHighestHP <= ableToBeKilledTargets[i].GetComponent<Player>().HP) {

				tempHighestHP = (int) ableToBeKilledTargets[i].GetComponent<Player>().HP;
				HighHPforAbleToKillIndex = i;
			}
		}
	}



	/*
	 * Find the most damage target 
	 */ 
	public void findMostDamageTarget() {

		int tempHighestDamage = (int) targets[0].GetComponent<Player>().baseDamage;
		MostDamageTargetIndex = 0;

		for (int i = 1; i < targets.Count; i ++) {
			if(tempHighestDamage <= targets[i].GetComponent<Player>().baseDamage) {

				tempHighestDamage = (int) targets[i].GetComponent<Player>().baseDamage;
				MostDamageTargetIndex = i;
			}
		}
	}



	/*
	 * Reset all the variables for next decision make
	 */ 
	public void resetAiPlayer() {

		targets.Clear();
		ableToBeKilledTargets.Clear ();
		isPlayerInAttackRange = false;
		existPlayerBeKilled = false;
		isDecisionMade = false;
		moveToAttack = false;
		target = null;
		decisionExecuted = false;
		preferenceTileX = -1; 
		preferenceTileY = -1;
		hasDecreasePlayerCount = false;
		noNeedToMove = false;
		decisionTreeReturnedCode = -1;
		HighHPforAbleToKillIndex = -1;
		MostDamageTargetIndex = -1;

		Debug.Log ("Targets reset");
	}
}
