using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AiPlayer : Player {

	/*
	 * Variables
	 */

	public float HP = 100.0f;

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

		GameManager.instance.nextTurn();
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
		if (isPlayerInAttackRange) {
			if(targets.Count == 1) {
				return ATTACK;
			} else {
				if(existPlayerBeKilled) {
					if(ableToBeKilledTargets.Count == 1) {
						return KILL_ONE;
					} else {
						return CHOOSE_HIGH_HP;
					}
				} else {
					return ATTACK_MOST_DEMAGE;
				}
			}

		} else {
			if(this.HP > this.baseHP * 0.7) {
				return MOVE_TO_PLAYER;
			} else {
				if(GameManager.instance.aiList.Count <= 1) {
					return MOVE_TO_ENEMY;
				} else {
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
						Debug.Log(tempPreference);
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

	// Search for targets on the map
	private void searchTargets() {
		/*List <List<GameObject>> tempMap = GameManager.instance.map;
		for (int y = 0; y < GameManager.instance.mapSize; y ++) {
			for (int x = 0; x < GameManager.instance.mapSize; x++) {
				if(tempMap [x] [y].GetComponent<Tile> ().isOccupied == true) {
					targets.Add(tempMap [x] [y].GetComponent<Tile> ());
				}
			}
		}*/

	}

	public override void TurnOnGUI(){}
	
	//Display HP
	public void OnGUI(){

		Vector3 location = Camera.main.WorldToScreenPoint (transform.position)+ Vector3.up*30+ Vector3.left*15;
		GUI.TextArea(new Rect(location.x, Screen.height - location.y, 30, 20), HP.ToString());
	}
}
