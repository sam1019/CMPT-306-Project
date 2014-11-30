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

	private bool isPlayerInAttackRange = false;
	private bool existPlayerBeKilled = false;

	public bool isDecisionMade = false;
	public bool moveToAttack = false;

	public Player target = null;

	public bool decisionExecuted = false;

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

	private bool anyPlayerInAttackRangeHelper(Tile targetTile){

		foreach (GameObject p in GameManager.instance.playerList) { //Checks for enemy class on tile target

			if(p.GetComponent<Tank>() != null){
				Tank temp = p.GetComponent<Tank>(); //Gets enemy script
				Debug.Log("Tank is in Attack Range");
				if (temp.gridPosition == targetTile.gridPosition) { //Checks if tile selected contains enemy
					targets.Add(temp);
					Debug.Log("Tank is added into target list");
					return true;
				}
			}
			else if(p.GetComponent<Jet>() != null){ //Checks for enemy class on tile target
				Jet temp = p.GetComponent<Jet>();	//Gets enemy script	
				Debug.Log("Jet is in Attack Range");
				if (temp.gridPosition == targetTile.gridPosition) { //Checks if tile selected contains enemy
					targets.Add(temp);
					Debug.Log("Jet is added into target list");
					return true;
				}
			}
			else if(p.GetComponent<Soldier>() != null){ //Checks for enemy class on tile target
				Soldier temp = p.GetComponent<Soldier>();	//Gets enemy script	
				Debug.Log("Soldier is in Attack Range");
				if (temp.gridPosition == targetTile.gridPosition) { //Checks if tile selected contains enemy
					targets.Add(temp);
					Debug.Log("Soldier is added into target list");
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

		//PathFinding.doPathFinding ((int)this.gridPosition.x, (int)this.gridPosition.y, this.attackRange, PathFinding.ATTACK_HIGHLIGHT, PathFinding.ALIEN);
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

	public void moveToAttackableRangeAction() {

		int targetX = (int) target.gridPosition.x;
		int targetY = (int) target.gridPosition.y;
		Debug.Log ("Target X: " + targetX);
		Debug.Log ("Target Y: " + targetY);

		Tile destTile = null;
		List<Tile> temp = new List<Tile> ();


		bool hasFoundTile = false;

		/* Iterate through the map to highlight the tiles that in its move range
		 * different part of the map has different methods to choose tiles
		 */
		//int x = (int) GameManager.instance.map [1] [1].transform.position.x;
		if (targetX >= targetY) {
			for (int i = 0; i < GameManager.instance.mapSize; i++) {
				for (int j = 0; j < GameManager.instance.mapSize; j++) {
					
					if( i + j <= targetX + targetY + this.attackRange && i + j >= targetX + targetY - this.attackRange 
					   && i<= targetX + this.attackRange && i >= targetX - this.attackRange &&  j<= targetY + this.attackRange && j >= targetY - this.attackRange
					   && (i - j) <= Mathf.Abs(targetX - targetY) + this.attackRange &&  (i - j) >= Mathf.Abs(targetX - targetY) - this.attackRange){

						if(!hasFoundTile && !GameManager.instance.map [i] [j].GetComponent<Tile> ().isOccupied) {
							temp.Add(GameManager.instance.map [i] [j].GetComponent<Tile> ());
							hasFoundTile = true;
						}
					}
				}
			}
		}
		else if(targetX < targetY){
			for (int i = 0; i < GameManager.instance.mapSize; i++) {
				for (int j = 0; j < GameManager.instance.mapSize; j++) {
					
					if( i + j <= targetX + targetY + this.attackRange && i + j >= targetX + targetY - this.attackRange 
					   && i<= targetX + this.attackRange && i >= targetX - this.attackRange &&  j<= targetY + this.attackRange && j >= targetY - this.attackRange
					   && (i - j) <= (targetX - targetY + this.attackRange) &&  (i - j) >= (targetX - targetY - this.attackRange)){

						if(!hasFoundTile && !GameManager.instance.map [i] [j].GetComponent<Tile> ().isOccupied) {
							temp.Add(GameManager.instance.map [i] [j].GetComponent<Tile> ());
							hasFoundTile = true;
						}
					}
				}
			}
		}

		int randIndex = Random.Range (0, temp.Count);

		destTile = temp[randIndex].GetComponent<Tile>();

		if (destTile == null) {
			Debug.Log("Can't find an attackable tile");
			return;
		}

		this.moveToAttack = true;

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

	public void doAttack(){
		Debug.Log("Doing Attack");
		target.HP = target.HP - this.baseDamage * (K / (K + target.baseDefense));
		if (target.HP <= 0) {
			print ("HELLO");
			GameManager.instance.playerCount-=1;
			Destroy(target.gameObject, 1);
		}
	}

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
	
	public void resetAiPlayer() {
		Debug.Log ("Targets reset");
		targets.Clear();
		ableToBeKilledTargets.Clear ();
		isPlayerInAttackRange = false;
		existPlayerBeKilled = false;
		
		isDecisionMade = false;
		moveToAttack = false;
		
		Player target = null;
		
		decisionExecuted = false;
		
		preferenceTileX = -1; 
		preferenceTileY = -1;
		
		decisionTreeReturnedCode = -1;
		
		HighHPforAbleToKillIndex = -1;
		MostDamageTargetIndex = -1;
	}
}
