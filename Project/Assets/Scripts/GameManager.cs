using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/* Core game manager */

public class GameManager : MonoBehaviour {
	
	/* Game components */
	public static GameManager instance;
	
	/* Prefabs needed for game */

	//for maps
	public GameObject Grass0TilePrefab;
	public GameObject Grass1TilePrefab;
	public GameObject Grass2TilePrefab;
	public GameObject Grass3TilePrefab;
	public GameObject MountainGrass1TilePrefab;
	public GameObject MountainGrass2TilePrefab;
	public GameObject MountainGrass3TilePrefab;
	public GameObject MountainSand1TilePrefab;
	public GameObject MountainSand2TilePrefab;
	public GameObject MountainSand3TilePrefab;
	public GameObject Sand0TilePrefab;
	public GameObject Sand1TilePrefab;
	public GameObject Sand2TilePrefab;
	public GameObject Sand3TilePrefab;
	public GameObject Sand4TilePrefab;
	public GameObject Water1TilePrefab;
	public GameObject Water2TilePrefab;
	public GameObject Water3TilePrefab;
	public GameObject Water4TilePrefab;
	public GameObject TreeGrass1TilePrefab;
	public GameObject TreeGrass2TilePrefab;
	public GameObject TreeGrass3TilePrefab;
	public GameObject TreeSand1TilePrefab;
	public GameObject TreeSand2TilePrefab;
	public GameObject TreeSand3TilePrefab;


	public GameObject AIPrefab;
	public GameObject tile;
	public GameObject jetPrefab; 
	public GameObject soldierPrefab;
	public GameObject AlienTroopPrefab;
	public GameObject alienShipPrefab;
	public GameObject berserkerPrefab;
	public GameObject tankPrefab;
	public GameObject PlayerPrefab;
	
	/* Character components */
	GameObject player;
	public int playerCount;
	public int aiCount;
	public List <GameObject> playerList;
	public List <GameObject> humanList;
	public List <GameObject> aiList;
	private const float PLAYER_HEIGHT = -1.0f; 	//Used to spawn game objects 1.5 above the map so they are not in collision
	private const float AI_HEIGHT = -0.25f;
	public int currentPlayerIndex;//Iterates throught the player list
	public int currentAIIndex; //Iterates throught the AI list
	
	/* Map components */
	Tile grid;
	public List <List<GameObject>> map;
	public Transform mapTransform;
	public int mapSize = 11; //The size of the map i 
	
	/* Panel components */
	public bool IsPause; 
	public int scores;	
	
	
	void Awake(){
		
		instance = this;

		loadMapFromCsv ();
	}
	
	void Start () {
		
		playerList = new List<GameObject> ();
		aiList = new List<GameObject> ();
		currentPlayerIndex = 0;
		currentAIIndex = 0;
		playerCount = 0;
		aiCount = 0;
		//generateMap (); //Generate map
		//spawnPlayers(); //spawn players to be controlled by users
		//spawnAI(); //Spawn AI opponent for the player
		spawnUnitsFromCsv ();

		IsPause = true;
		scores = 0;
	}
	
	/* Delete player character when it's dead
	 * helper function 
	 */
	public void deleteChar(int charCount){
		
		Destroy(playerList[currentPlayerIndex]);
		playerList.RemoveAt(currentPlayerIndex);
		charCount -=1;
	}
	
	void Update () {
		
		// winner detect
		if(playerCount <= 0){
			print ("You lose");
			Application.LoadLevel("Victory");
		}
		if(aiCount <= 0){
			print ("You win");
			Application.LoadLevel("Defeat");
		}
		
		if( playerList.Count > 0){
			
			getHumanTurn(); 		// TODO: For the future
			
			// TODO: Testing code
			GameObject temp = playerList [currentPlayerIndex];
			
			if(temp.GetComponent<UserPlayer>() != null){
				UserPlayer player  = temp.GetComponent<UserPlayer>();
				player.TurnUpdate ();
				
				if (player.HP <= 0){
					deleteChar(playerCount);
				}
			}
			else if(temp.GetComponent<AiPlayer>() != null){
				AiPlayer ai  = temp.GetComponent<AiPlayer>();
				ai.TurnUpdate ();
				
				if (ai.HP <= 0){
					deleteChar(aiCount);
				}
			}
		}
	}
	
	/*
	 * Finds the current player player's type 
	 * (Ex. Tank, Soldier, Medic,etc)
	 */
	// TODO: Can possibly refactor code in the future
	public void getHumanTurn(){
		
		GameObject temp = playerList [currentPlayerIndex];
		
		if(temp.GetComponent<Tank>() != null){ //Check what script is attached to prefab
			Tank tank  = temp.GetComponent<Tank>();
			
			if (tank.HP <= 0){ //Check if player is not dead
				deleteChar(playerCount);
			}
			tank.TurnUpdate ();			
		}
		else if(temp.GetComponent<Soldier>() != null){
			Soldier soldier  = temp.GetComponent<Soldier>();
			if (soldier.HP <= 0){
				deleteChar(playerCount);
			}
			soldier.TurnUpdate ();
		}
		else if(temp.GetComponent<Medic>() != null){
			Medic medic  = temp.GetComponent<Medic>();
			
			if (medic.HP <= 0){
				deleteChar(playerCount);
			}
			medic.TurnUpdate ();			
		}
		else if(temp.GetComponent<Specialist>() != null){
			Specialist spec  = temp.GetComponent<Specialist>();
			
			if (spec.HP <= 0){
				deleteChar(playerCount);
			}
			spec.TurnUpdate ();		
		}
		else if(temp.GetComponent<Helicopter>() != null){
			Helicopter heli  = temp.GetComponent<Helicopter>();
			
			if (heli.HP <= 0){
				deleteChar(playerCount);
			}	
			heli.TurnUpdate ();			
		}
		else if(temp.GetComponent<Jet>() != null){
			Jet jet  = temp.GetComponent<Jet>();
			
			if (jet.HP <= 0){
				deleteChar(playerCount);
			}	
			jet.TurnUpdate ();
		}
		else{
			getAlienTurn();
		}
	}
	
	/*
	 * Finds the current AI's type 
	 */
	// TODO: Can possibly refactor code in the future
	public void getAlienTurn(){
		
		GameObject temp = playerList [currentPlayerIndex];
		
		if(temp.GetComponent<AlienShip>() != null){ 			//Checks for what script AI has attached
			AlienShip ship  = temp.GetComponent<AlienShip>();	//Get the current player script
			
			if (ship.HP <= 0){	// If AI is dead, remove from game
				deleteChar(aiCount);
			}
			ship.TurnUpdate (); // AI's turn
		}
		else if (temp.GetComponent<AlienSoldier>() != null){
			AlienSoldier alienSoldier  = temp.GetComponent<AlienSoldier>();
			
			if (alienSoldier.HP <= 0){
				deleteChar(aiCount);
			}
			alienSoldier.TurnUpdate ();
		}
		else if (temp.GetComponent<AlienSupport>() != null){
			AlienSupport alienSupport  = temp.GetComponent<AlienSupport>();
			
			if (alienSupport.HP <= 0){
				deleteChar(aiCount);
			}
			alienSupport.TurnUpdate ();
		}
		else if (temp.GetComponent<Berserker>() != null){
			Berserker berserker  = temp.GetComponent<Berserker>();
			
			if (berserker.HP <= 0){
				deleteChar(aiCount);
			}
			berserker.TurnUpdate ();
		}
	}
	
	public void nextTurn() {
		
		/* Iterating through the list, the current index is the current player's turn
		 * When it reaches the length of the list goes back to player at index 0 turn 
		 */
		this.disableHightLight ();
		
		if (playerList.Count ==1) {
			currentPlayerIndex =  0;
		}
		
		if (currentPlayerIndex + 1 < playerList.Count) {
			currentPlayerIndex++;
		} 
		else {
			currentPlayerIndex=0;
		} 
		
	}
	
	/*
	 * Checks if tile selected by player is occupied or not
	 * If occupied player cannot move to tile
	 */
	public bool canPlayerMove(Tile destination){
		
		return (playerList.Count > 0 && !destination.isOccupied);
	}
	
	// set occupied to given tile
	public void setOccupied(Tile destination, Player player){
		
		int x, y;
		
		//Grid postion correlates to the map list indexes
		x = (int) player.gridPosition.x;
		y = (int) player.gridPosition.y;
		
		//Setting the old position to unoccupied
		Tile Maptemp = map [x] [y].GetComponent<Tile>();
		//map [x] [y].GetComponent<Tile> ().isOccupied = false;
		Maptemp.isOccupied = false;
		//Setting new position to occupied
		destination.isOccupied=true;
		destination.occupiedName = player.roleName ();
	}
	
	/* Helper function for MovePlayer() and MoveAlien() */
	private void MoveHelper(Player obj, Tile dest) {
		setOccupied(dest, (Player) obj);													//Sets new tile location occupied
		obj.moveDestination = dest.transform.position + PLAYER_HEIGHT * Vector3.forward;	//Moves player unit
		obj.gridPosition = dest.gridPosition;												//Sets new grid position for unit
	}
	
	// To realize the player movement
	// TODO: Can possibly refactor code in the future
	public void MovePlayer(Tile destination){
		
		if (canPlayerMove(destination)) { // If tile is unoccupied player can move to there

			int i=(int)destination.gridPosition.x;
			int j=(int)destination.gridPosition.y;

			/****Test unit******/
			if(playerList [currentPlayerIndex].GetComponent<UserPlayer>() != null && map[i][j].GetComponent<Tile> ().transform.renderer.material.color == Color.magenta){
				
				UserPlayer Playertemp = playerList [currentPlayerIndex].GetComponent<UserPlayer>();
				MoveHelper(Playertemp, destination);
			}
			else if(playerList [currentPlayerIndex].GetComponent<Tank>() != null && map[i][j].GetComponent<Tile> ().transform.renderer.material.color == Color.magenta){ //Checks for what script is attached to player
				
				Tank tankTemp = playerList [currentPlayerIndex].GetComponent<Tank>();
				MoveHelper(tankTemp, destination);
			}
			else if(playerList [currentPlayerIndex].GetComponent<Soldier>() != null && map[i][j].GetComponent<Tile> ().transform.renderer.material.color == Color.magenta){ //Checks for what script is attached to player
				
				Soldier soldierTemp = playerList [currentPlayerIndex].GetComponent<Soldier>();
				MoveHelper(soldierTemp, destination);
			}
			else if(playerList [currentPlayerIndex].GetComponent<Medic>() != null && map[i][j].GetComponent<Tile> ().transform.renderer.material.color == Color.magenta){ //Checks for what script is attached to player
				
				Medic medicTemp = playerList [currentPlayerIndex].GetComponent<Medic>();
				MoveHelper(medicTemp, destination);
			}
			else if(playerList [currentPlayerIndex].GetComponent<Specialist>() != null && map[i][j].GetComponent<Tile> ().transform.renderer.material.color == Color.magenta){ //Checks for what script is attached to player
				
				Specialist specTemp = playerList [currentPlayerIndex].GetComponent<Specialist>();
				MoveHelper(specTemp, destination);
			}
			else if(playerList [currentPlayerIndex].GetComponent<Jet>() != null && map[i][j].GetComponent<Tile> ().transform.renderer.material.color == Color.magenta){ //Checks for what script is attached to player
				
				Jet jetTemp = playerList [currentPlayerIndex].GetComponent<Jet>();
				MoveHelper(jetTemp, destination);
			}
			else if(playerList [currentPlayerIndex].GetComponent<Helicopter>() != null && map[i][j].GetComponent<Tile> ().transform.renderer.material.color == Color.magenta){ //Checks for what script is attached to player
				
				Helicopter heliTemp = playerList [currentPlayerIndex].GetComponent<Helicopter>();
				MoveHelper(heliTemp, destination);
			}
		}
		else{
			print ("Cannot move there");
		}
	}
	
	//Can possibly refactor code in the future
	/*
	 * Moves AI units
	 */
	public void moveAlien(Tile destination){
		
		if (canPlayerMove(destination)) { //Checks if player can move to tile
			
			if(playerList [currentPlayerIndex].GetComponent<AlienSoldier>() != null){
				//Debug.Log ("AlienSoldier moveAlien() called");
				AlienSoldier alienTemp = playerList [currentPlayerIndex].GetComponent<AlienSoldier>(); //Checks if script is attached to player

				MoveHelper(alienTemp, destination);
			}
		}
		if (canPlayerMove(destination)) {
			
			if(playerList [currentPlayerIndex].GetComponent<AlienSupport>() != null){  //Checks if script is attached to player
				AlienSupport alienSupTemp = playerList [currentPlayerIndex].GetComponent<AlienSupport>();
				MoveHelper(alienSupTemp, destination);
			}
		}
		if (canPlayerMove(destination)) {
			
			if(playerList [currentPlayerIndex].GetComponent<AlienShip>() != null){  //Checks if script is attached to player
				AlienShip alienShipTemp = playerList [currentPlayerIndex].GetComponent<AlienShip>();
				MoveHelper(alienShipTemp, destination);
			}
		}
		if (canPlayerMove(destination)) {
			
			if(playerList [currentPlayerIndex].GetComponent<AlienSoldier>() != null){  //Checks if script is attached to player
				AlienSoldier alienTemp = playerList [currentPlayerIndex].GetComponent<AlienSoldier>();
				MoveHelper(alienTemp, destination);
			}
		}
		if (canPlayerMove(destination)) {
			
			if(playerList [currentPlayerIndex].GetComponent<Berserker>() != null){  //Checks if script is attached to player
				Berserker berserkerTemp = playerList [currentPlayerIndex].GetComponent<Berserker>();
				MoveHelper(berserkerTemp, destination);
			}
		}
	}		
	
	
	/*
	 * Checks what script is attached to the current player's turn  
	 * Then calls the attack function of that player's class
	 * Attacks the unit on the target tile
	 */
	// TODO: Can possibly refactor in the future
	public void whatPlayerClassIsAttacking(Tile targetTile){
		
		/******Test player unit**********/
		if (playerList [currentPlayerIndex].GetComponent<UserPlayer> () != null) { //Checks if script is attached to player
			UserPlayer temp = playerList [currentPlayerIndex].GetComponent<UserPlayer>(); //Get the script of the player
			temp.tempAttack(targetTile); //Calls the attack function of the class				
		}	
		else if (playerList [currentPlayerIndex].GetComponent<Soldier> () != null) { //Checks if script is attached to player
			
			Soldier temp = playerList [currentPlayerIndex].GetComponent<Soldier>();//Get the script of the player
			temp.getEnemyToAttack(targetTile);	//Calls the attack function of the class			 			
		}
		else if (playerList [currentPlayerIndex].GetComponent<Medic> () != null) {//Checks if script is attached to player
			
			Medic temp = playerList [currentPlayerIndex].GetComponent<Medic>();//Get the script of the player
			temp.getEnemyToAttack(targetTile); //Calls the attack function of the class								
		}
		else if (playerList [currentPlayerIndex].GetComponent<Specialist> () != null) {//Checks if script is attached to player
			
			Specialist temp = playerList [currentPlayerIndex].GetComponent<Specialist>();
			temp.getEnemyToAttack(targetTile);	//Calls the attack function of the class							
		}
		else if (playerList [currentPlayerIndex].GetComponent<Tank> () != null) {//Checks if script is attached to player
			
			Tank temp = playerList [currentPlayerIndex].GetComponent<Tank>();//Get the script of the player
			temp.getEnemyToAttack(targetTile);						
		}
		else if (playerList [currentPlayerIndex].GetComponent<Jet> () != null) {//Checks if script is attached to player
			
			Jet temp = playerList [currentPlayerIndex].GetComponent<Jet>();//Get the script of the player
			temp.getEnemyToAttack(targetTile);	//Calls the attack function of the class							
		}
		else if (playerList [currentPlayerIndex].GetComponent<Helicopter> () != null) {//Checks if script is attached to player
			
			Helicopter temp = playerList [currentPlayerIndex].GetComponent<Helicopter>();//Get the script of the player
			temp.getEnemyToAttack(targetTile);	//Calls the attack function of the class						
		}
		else{
			Debug.Log("Something went wrong");
		}
		
	}
	
	// highlight a tile according to x and y for attacking range
	private void highlightAttackedTile(int x, int y) {
		this.map [x] [y].GetComponent<Tile> ().transform.renderer.material.color = Color.red;
	}

	// highlight a tile according to x and y for move range
	private void highlightMoveTile(int x, int y) {
		this.map [x] [y].GetComponent<Tile> ().transform.renderer.material.color = Color.blue;
	}
	

	// When click move botton, the available range for player will Highlighted
	public void enableAttackHighlight(int originLocationX, int originLocationY, int range){
		
		/* Iterate through the map to highlight the tiles that in its attack range
		 * different part of the map has different methods to choose tiles
		 */
		/*
		if (originLocationX >= originLocationY) {
			for (int i = 0; i < mapSize; i++) {
				for (int j = 0; j < mapSize; j++) {
					if (i + j <= originLocationX + originLocationY + range && i + j >= originLocationX + originLocationY - range 
						&& i <= originLocationX + range && i >= originLocationX - range && j <= originLocationY + range && j >= originLocationY - range
						&& (i - j) <= Mathf.Abs (originLocationX - originLocationY) + range && (i - j) >= Mathf.Abs (originLocationX - originLocationY) - range) {
							highlightAttackedTile (i, j);
						}
				}
			}
		} else if (originLocationX < originLocationY) {
			for (int i = 0; i < mapSize; i++) {
				for (int j = 0; j < mapSize; j++) {
					if (i + j <= originLocationX + originLocationY + range && i + j >= originLocationX + originLocationY - range 
						&& i <= originLocationX + range && i >= originLocationX - range && j <= originLocationY + range && j >= originLocationY - range
						&& (i - j) <= (originLocationX - originLocationY + range) && (i - j) >= (originLocationX - originLocationY - range)) {
							highlightAttackedTile (i, j);
						}
				}
			}
		}
		*/
		//Debug.Log (originLocationX);
		//Debug.Log (originLocationY);
		//Debug.Log (range);
		PathFinding.doPathFinding (originLocationX, originLocationY, range, PathFinding.ATTACK_HIGHLIGHT, PathFinding.HUMAN);
	}
	
	// TODO: need fully implementation
	// When click move botton, the available range for player will Highlighted
	public void enableMoveHighlight(int originLocationX, int originLocationY, int range){
		
		/* Iterate through the map to highlight the tiles that in its move range
		 * different part of the map has different methods to choose tiles
		 */
		/*
		if (originLocationX >= originLocationY) {
			for (int i = 0; i < mapSize; i++) {
				for (int j = 0; j < mapSize; j++) {
					
					if( i + j <= originLocationX + originLocationY + range && i + j >= originLocationX + originLocationY - range 
					   && i<= originLocationX + range && i >= originLocationX - range &&  j<= originLocationY + range && j >= originLocationY - range
					   && (i - j) <= Mathf.Abs(originLocationX - originLocationY) + range &&  (i - j) >= Mathf.Abs(originLocationX - originLocationY) - range){
						highlightMoveTile(i,j);
					}
				}
			}
		}
		else if(originLocationX < originLocationY){
			for (int i = 0; i < mapSize; i++) {
				for (int j = 0; j < mapSize; j++) {
					
					if( i + j <= originLocationX + originLocationY + range && i + j >= originLocationX + originLocationY - range 
					   && i<= originLocationX + range && i >= originLocationX - range &&  j<= originLocationY + range && j >= originLocationY - range
					   && (i - j) <= (originLocationX - originLocationY + range) &&  (i - j) >= (originLocationX - originLocationY - range)){
						highlightMoveTile(i,j);
					}
				}
			}
		}
		*/
		//Debug.Log (originLocationX);
		//Debug.Log (originLocationY);
		//Debug.Log (range);
		PathFinding.doPathFinding (originLocationX, originLocationY, range, PathFinding.MOVE_HIGHLIGHT, PathFinding.HUMAN);
	}
	
	
	//After the movement, highlight path will be disabled
	public void disableHightLight(){
		
		for (int i = 0; i < mapSize; i++) {
			for (int j = 0; j < mapSize; j++) {
				this.map [i] [j].GetComponent<Tile> ().transform.renderer.material.color = Color.white;
			}
		}
	}
	
	/* Used to spawn the map 
	 * Map is made up of multiple spawned cubes
	 * The 0,0 coordinate the very center cube
	 */
	public void generateMap(){
		
		map = new List<List<GameObject>>();
		
		for (int i = 0; i < mapSize; i++) {
			List <GameObject> row = new List<GameObject>();
			
			for (int j = 0; j < mapSize; j++) {
				//Tiles spawn around the center tile
				tile = Instantiate(Grass0TilePrefab, new Vector2(i - Mathf.Floor(mapSize/2), -j + Mathf.Floor(mapSize/2)),Quaternion.identity) as GameObject;
				Tile maptemp = tile.GetComponent<Tile>();
				maptemp.gridPosition = new Vector2(i,j);
				
				row.Add (tile);
			}
			//Add each cube to a list
			map.Add(row);
		}
	}

	// Reads from CSV file and generates a map
	public void loadMapFromCsv() {
		string[,] mapDate = CSVReader.read (LevelSelect.instance.levelMap);
		map = new List<List<GameObject>>();
		
		for (int i = 0; i < mapSize; i++) {
			List <GameObject> row = new List<GameObject>();
			
			for (int j = 0; j < mapSize; j++) {
				//Tiles spawn around the center tile
				if(mapDate[i,j] == "Grass0"){
					tile = Instantiate(Grass0TilePrefab, new Vector2(i - Mathf.Floor(mapSize/2), -j + Mathf.Floor(mapSize/2)),Quaternion.identity) as GameObject;
				}else if(mapDate[i,j] == "Grass1"){
					tile = Instantiate(Grass1TilePrefab, new Vector2(i - Mathf.Floor(mapSize/2), -j + Mathf.Floor(mapSize/2)),Quaternion.identity) as GameObject;
				}else if(mapDate[i,j] == "Grass2"){
					tile = Instantiate(Grass2TilePrefab, new Vector2(i - Mathf.Floor(mapSize/2), -j + Mathf.Floor(mapSize/2)),Quaternion.identity) as GameObject;
				}else if(mapDate[i,j] == "Grass3"){
					tile = Instantiate(Grass3TilePrefab, new Vector2(i - Mathf.Floor(mapSize/2), -j + Mathf.Floor(mapSize/2)),Quaternion.identity) as GameObject;
				}else if(mapDate[i,j] == "Water1"){
					tile = Instantiate(Water1TilePrefab, new Vector2(i - Mathf.Floor(mapSize/2), -j + Mathf.Floor(mapSize/2)),Quaternion.identity) as GameObject;
					tile.GetComponent<Tile>().isOccupied = true; // this tile is occupied initially, cannot move to this tile
					tile.GetComponent<Tile>().occupiedName = "Water";
				}else if(mapDate[i,j] == "Water2"){
					tile = Instantiate(Water2TilePrefab, new Vector2(i - Mathf.Floor(mapSize/2), -j + Mathf.Floor(mapSize/2)),Quaternion.identity) as GameObject;
					tile.GetComponent<Tile>().isOccupied = true; // this tile is occupied initially, cannot move to this tile
					tile.GetComponent<Tile>().occupiedName = "Water";
				}else if(mapDate[i,j] == "Water3"){
					tile = Instantiate(Water3TilePrefab, new Vector2(i - Mathf.Floor(mapSize/2), -j + Mathf.Floor(mapSize/2)),Quaternion.identity) as GameObject;
					tile.GetComponent<Tile>().isOccupied = true; // this tile is occupied initially, cannot move to this tile
					tile.GetComponent<Tile>().occupiedName = "Water";
				}else if(mapDate[i,j] == "Water4"){
					tile = Instantiate(Water4TilePrefab, new Vector2(i - Mathf.Floor(mapSize/2), -j + Mathf.Floor(mapSize/2)),Quaternion.identity) as GameObject;
					tile.GetComponent<Tile>().isOccupied = true; // this tile is occupied initially, cannot move to this tile
					tile.GetComponent<Tile>().occupiedName = "Water";
				}else if(mapDate[i,j] == "MountainSand1"){
					tile = Instantiate(MountainSand1TilePrefab, new Vector2(i - Mathf.Floor(mapSize/2), -j + Mathf.Floor(mapSize/2)),Quaternion.identity) as GameObject;
					tile.GetComponent<Tile>().isOccupied = true; // this tile is occupied initially, cannot move to this tile
					tile.GetComponent<Tile>().occupiedName = "Mountain";
				}else if(mapDate[i,j] == "MountainSand2"){
					tile = Instantiate(MountainSand2TilePrefab, new Vector2(i - Mathf.Floor(mapSize/2), -j + Mathf.Floor(mapSize/2)),Quaternion.identity) as GameObject;
					tile.GetComponent<Tile>().isOccupied = true; // this tile is occupied initially, cannot move to this tile
					tile.GetComponent<Tile>().occupiedName = "Mountain";
				}else if(mapDate[i,j] == "MountainSand3"){
					tile = Instantiate(MountainSand3TilePrefab, new Vector2(i - Mathf.Floor(mapSize/2), -j + Mathf.Floor(mapSize/2)),Quaternion.identity) as GameObject;
					tile.GetComponent<Tile>().isOccupied = true; // this tile is occupied initially, cannot move to this tile
					tile.GetComponent<Tile>().occupiedName = "Mountain";
				}else if(mapDate[i,j] == "MountainGrass1"){
					tile = Instantiate(MountainGrass1TilePrefab, new Vector2(i - Mathf.Floor(mapSize/2), -j + Mathf.Floor(mapSize/2)),Quaternion.identity) as GameObject;
					tile.GetComponent<Tile>().isOccupied = true; // this tile is occupied initially, cannot move to this tile
					tile.GetComponent<Tile>().occupiedName = "Mountain";
				}else if(mapDate[i,j] == "MountainGrass2"){
					tile = Instantiate(MountainGrass2TilePrefab, new Vector2(i - Mathf.Floor(mapSize/2), -j + Mathf.Floor(mapSize/2)),Quaternion.identity) as GameObject;
					tile.GetComponent<Tile>().isOccupied = true; // this tile is occupied initially, cannot move to this tile
					tile.GetComponent<Tile>().occupiedName = "Mountain";
				}else if(mapDate[i,j] == "MountainGrass3"){
					tile = Instantiate(MountainGrass3TilePrefab, new Vector2(i - Mathf.Floor(mapSize/2), -j + Mathf.Floor(mapSize/2)),Quaternion.identity) as GameObject;
					tile.GetComponent<Tile>().isOccupied = true; // this tile is occupied initially, cannot move to this tile
					tile.GetComponent<Tile>().occupiedName = "Mountain";
				}else if(mapDate[i,j] == "TreeSand1"){
					tile = Instantiate(TreeSand1TilePrefab, new Vector2(i - Mathf.Floor(mapSize/2), -j + Mathf.Floor(mapSize/2)),Quaternion.identity) as GameObject;
				}else if(mapDate[i,j] == "TreeSand2"){
					tile = Instantiate(TreeSand2TilePrefab, new Vector2(i - Mathf.Floor(mapSize/2), -j + Mathf.Floor(mapSize/2)),Quaternion.identity) as GameObject;
				}else if(mapDate[i,j] == "TreeSand3"){
					tile = Instantiate(TreeSand3TilePrefab, new Vector2(i - Mathf.Floor(mapSize/2), -j + Mathf.Floor(mapSize/2)),Quaternion.identity) as GameObject;
				}else if(mapDate[i,j] == "TreeGrass1"){
					tile = Instantiate(TreeGrass1TilePrefab, new Vector2(i - Mathf.Floor(mapSize/2), -j + Mathf.Floor(mapSize/2)),Quaternion.identity) as GameObject;
				}else if(mapDate[i,j] == "TreeGrass2"){
					tile = Instantiate(TreeGrass2TilePrefab, new Vector2(i - Mathf.Floor(mapSize/2), -j + Mathf.Floor(mapSize/2)),Quaternion.identity) as GameObject;
				}else if(mapDate[i,j] == "TreeGrass3"){
					tile = Instantiate(TreeGrass3TilePrefab, new Vector2(i - Mathf.Floor(mapSize/2), -j + Mathf.Floor(mapSize/2)),Quaternion.identity) as GameObject;
				}else if(mapDate[i,j] == "Sand0"){
					tile = Instantiate(Sand0TilePrefab, new Vector2(i - Mathf.Floor(mapSize/2), -j + Mathf.Floor(mapSize/2)),Quaternion.identity) as GameObject;
				}else if(mapDate[i,j] == "Sand1"){
					tile = Instantiate(Sand1TilePrefab, new Vector2(i - Mathf.Floor(mapSize/2), -j + Mathf.Floor(mapSize/2)),Quaternion.identity) as GameObject;
				}else if(mapDate[i,j] == "Sand2"){
					tile = Instantiate(Sand2TilePrefab, new Vector2(i - Mathf.Floor(mapSize/2), -j + Mathf.Floor(mapSize/2)),Quaternion.identity) as GameObject;
				}else if(mapDate[i,j] == "Sand3"){
					tile = Instantiate(Sand3TilePrefab, new Vector2(i - Mathf.Floor(mapSize/2), -j + Mathf.Floor(mapSize/2)),Quaternion.identity) as GameObject;
				}else if(mapDate[i,j] == "Sand4"){
					tile = Instantiate(Sand4TilePrefab, new Vector2(i - Mathf.Floor(mapSize/2), -j + Mathf.Floor(mapSize/2)),Quaternion.identity) as GameObject;
				}else{
					tile = Instantiate(Grass1TilePrefab, new Vector2(i - Mathf.Floor(mapSize/2), -j + Mathf.Floor(mapSize/2)),Quaternion.identity) as GameObject;
				};
				Tile maptemp = tile.GetComponent<Tile>();
				maptemp.gridPosition = new Vector2(i,j);
				
				row.Add (tile);
			}
			//Add each cube to a list
			map.Add(row);
		}
	}

	/*
	 * Spawns units onto the map from csv and increment player count
	 */
	public void spawnUnitsFromCsv(){
		string[,] UnitsDate = CSVReader.read (LevelSelect.instance.levelUnits);
	
		for (int i = 0; i < mapSize; i++) {
			for (int j = 0; j < mapSize; j++) {
				if(UnitsDate[i,j] == "Tank"){
					/********************Spawning tank*****************************/
					GameObject tank;
					tank = Instantiate(tankPrefab, new Vector3(i - 6, 6 - j, PLAYER_HEIGHT),Quaternion.identity) as GameObject;
					playerList.Add(tank); //Add to player list
					humanList.Add(tank); //Add to Human list
					Tank tankTemp = tank.GetComponent<Tank> ();
					tankTemp.gridPosition = new Vector2 (i, j); //Setting grid position to their fixed spawn location
					playerCount += 1; //Increment player count
					setOccupied(map[i][j].GetComponent<Tile>(), tankTemp);
				}else if(UnitsDate[i,j] == "Jet"){
					/********************Spawning Jet*****************************/
					GameObject jet;
					jet = Instantiate(jetPrefab, new Vector3(i - 6, 6 - j,PLAYER_HEIGHT),Quaternion.identity) as GameObject;
					playerList.Add(jet); //Add to player list
					humanList.Add(jet); //Add to Human list
					Jet jetTemp=jet.GetComponent<Jet> ();
					jetTemp.gridPosition = new Vector2 (i, j); //Setting grid position to their fixed spawn location
					playerCount += 1; //Increment player count
					setOccupied(map[i][j].GetComponent<Tile>(), jetTemp);
				}else if(UnitsDate[i,j] == "Soldier"){ 
					/********************Spawning solider*****************************/
					GameObject soldier;
					soldier = Instantiate(soldierPrefab, new Vector3(i - 6, 6 - j,PLAYER_HEIGHT),Quaternion.identity) as GameObject;
					playerList.Add(soldier); //Add to player list
					humanList.Add(soldier); //Add to Human list
					Soldier soldierTemp=soldier.GetComponent<Soldier> ();
					soldierTemp.gridPosition = new Vector2 (i, j); //Setting grid position to their fixed spawn location
					playerCount += 1; //Increment player count
					setOccupied(map[i][j].GetComponent<Tile>(), soldierTemp);
				}else if(UnitsDate[i,j] == "AlienSoldier"){ 
					/********************Spawning alien solider*****************************/
					GameObject aiplayer = Instantiate(AlienTroopPrefab, new Vector3(i - 6, 6 - j, PLAYER_HEIGHT),Quaternion.identity) as GameObject;
					playerList.Add(aiplayer); //Add to playerlist
					aiCount += 1; //Increment AI count
					aiList.Add(aiplayer); //Add to ailist
					AlienSoldier temp = aiplayer.GetComponent<AlienSoldier> ();
					setOccupied(map[i][j].GetComponent<Tile>(), temp);
					temp.gridPosition = new Vector2 (i, j); //Set the grid postion to the fixed spawn point
				}else if(UnitsDate[i,j] == "AlienShip"){ 
					/********************Spawning alien ship*****************************/
					GameObject ship = Instantiate(alienShipPrefab, new Vector3(i - 6, 6 - j, PLAYER_HEIGHT),Quaternion.identity) as GameObject;
					playerList.Add(ship); //Add to playerlist
					aiCount += 1; //Increment AI count
					aiList.Add(ship); //Add to ailist
					AlienShip shipTemp = ship.GetComponent<AlienShip> ();
					shipTemp.gridPosition = new Vector2 (i, j); //Set the grid postion to the fixed spawn point
					setOccupied(map[i][j].GetComponent<Tile>(), shipTemp);
				}else if(UnitsDate[i,j] == "Berserk"){ 
					/*******************Spawning alien berserker*****************************/
					
					GameObject berserk = Instantiate(berserkerPrefab, new Vector3(i - 6, 6 - j, PLAYER_HEIGHT),Quaternion.identity) as GameObject;
					playerList.Add(berserk); //Add to playerlist
					aiCount += 1; //Increment AI count
					aiList.Add(berserk); //Add to ailist
					Berserker berserkTemp = berserk.GetComponent<Berserker> ();
					berserkTemp.gridPosition = new Vector2 (i, j); //Set the grid postion to the fixed spawn point 
					setOccupied(map[i][j].GetComponent<Tile>(), berserkTemp);
				}else{ 
					
				}
			}
		}
	}

	/*
	 * Spawns players onto the map and increment player count
	 * Currently spawned at fixed locations
	 */
	// TODO: will refactor the code
	public void spawnPlayers(){
		
		//Spawn first player and add it to the list
		/********************Spawning tank*****************************/
		GameObject tank;
		tank = Instantiate(tankPrefab, new Vector3(4, 4, PLAYER_HEIGHT),Quaternion.identity) as GameObject;
		playerList.Add(tank); //Add to player list
		humanList.Add(tank); //Add to Human list
		Tank tankTemp = tank.GetComponent<Tank> ();
		tankTemp.gridPosition = new Vector2 (10, 2); //Setting grid position to their fixed spawn location
		playerCount += 1; //Increment player count
		
		/********************Spawning Jet*****************************/
		GameObject jet;
		jet = Instantiate(jetPrefab, new Vector3(-6, -6,PLAYER_HEIGHT),Quaternion.identity) as GameObject;
		playerList.Add(jet); //Add to player list
		humanList.Add(jet); //Add to Human list
		Jet jetTemp=jet.GetComponent<Jet> ();
		jetTemp.gridPosition = new Vector2 (0, 12); //Setting grid position to their fixed spawn location
		playerCount += 1; //Increment player count
		
		/********************Spawning solider*****************************/
		GameObject soldier;
		soldier = Instantiate(soldierPrefab, new Vector3(-6, -5,PLAYER_HEIGHT),Quaternion.identity) as GameObject;
		playerList.Add(soldier); //Add to player list
		humanList.Add(soldier); //Add to Human list
		Soldier soldierTemp=soldier.GetComponent<Soldier> ();
		soldierTemp.gridPosition = new Vector2 (0, 11); //Setting grid position to their fixed spawn location
		playerCount += 1; //Increment player count
	}
	
	/*
	 * Spawns AI onto the map and increment AI count
	 * Currently spawned at fixed locations
	 */
	// TODO: will refactor the code
	public void spawnAI(){
		
		/********************Spawning alien solider*****************************/
		GameObject aiplayer = Instantiate(AlienTroopPrefab, new Vector3(6 - Mathf.Floor(mapSize/2), -6 + Mathf.Floor(mapSize/2), PLAYER_HEIGHT),Quaternion.identity) as GameObject;
		playerList.Add(aiplayer); //Add to playerlist
		aiCount += 1; //Increment AI count
		aiList.Add(aiplayer); //Add to ailist
		AlienSoldier temp = aiplayer.GetComponent<AlienSoldier> ();
		temp.gridPosition = new Vector2 (6, 6); //Set the grid postion to the fixed spawn point
		
		/********************Spawning alien ship*****************************/
		GameObject ship = Instantiate(alienShipPrefab, new Vector3(6 - Mathf.Floor(mapSize/2), -6 + Mathf.Floor(mapSize/2)+1, PLAYER_HEIGHT),Quaternion.identity) as GameObject;
		playerList.Add(ship); //Add to playerlist
		aiCount += 1; //Increment AI count
		aiList.Add(ship); //Add to ailist
		AlienShip shipTemp = ship.GetComponent<AlienShip> ();
		shipTemp.gridPosition = new Vector2 (6,5); //Set the grid postion to the fixed spawn point
		
		/*******************Spawning alien berserker*****************************/
		
		GameObject berserk = Instantiate(berserkerPrefab, new Vector3(6+1 - Mathf.Floor(mapSize/2), -6 + Mathf.Floor(mapSize/2), PLAYER_HEIGHT),Quaternion.identity) as GameObject;
		playerList.Add(berserk); //Add to playerlist
		aiCount += 1; //Increment AI count
		aiList.Add(berserk); //Add to ailist
		Berserker berserkTemp = berserk.GetComponent<Berserker> ();
		berserkTemp.gridPosition = new Vector2 (7,6); //Set the grid postion to the fixed spawn point 
	}
	
	/*
	 *Finds the class of the current player unit to disable the button GUI 
	 */
	public void whoToTurnOnGui(){
		
		/*********Test player unit************/
		if(playerList[currentPlayerIndex].GetComponent<UserPlayer>()!=null){ //Checks if script is attached
			
			if (playerList[currentPlayerIndex].GetComponent<UserPlayer>().HP > 0){ //Get the script
				playerList[currentPlayerIndex].GetComponent<UserPlayer>().TurnOnGUI(); //Turn on the GUI for player
				
			}
		}
		else if(playerList[currentPlayerIndex].GetComponent<Tank>()!=null){ //Checks if script is attached
			
			if (playerList[currentPlayerIndex].GetComponent<Tank>().HP > 0){ //Get the script
				playerList[currentPlayerIndex].GetComponent<Tank>().TurnOnGUI();//Turn on the GUI for player
				
			}
		}
		else if(playerList[currentPlayerIndex].GetComponent<Jet>()!=null){//Checks if script is attached
			
			if (playerList[currentPlayerIndex].GetComponent<Jet>().HP > 0){ //Get the script
				playerList[currentPlayerIndex].GetComponent<Jet>().TurnOnGUI(); //Turn on the GUI for player
				
			}
		}
		else if(playerList[currentPlayerIndex].GetComponent<Soldier>()!=null){ //Checks if script is attached
			
			if (playerList[currentPlayerIndex].GetComponent<Soldier>().HP > 0){ //Get the script
				playerList[currentPlayerIndex].GetComponent<Soldier>().TurnOnGUI(); //Turn on the GUI for player
			}
		}
		else if(playerList[currentPlayerIndex].GetComponent<Medic>()!=null){ //Checks if script is attached
			
			if (playerList[currentPlayerIndex].GetComponent<Medic>().HP > 0){ //Get the script
				playerList[currentPlayerIndex].GetComponent<Medic>().TurnOnGUI(); //Turn on the GUI for player
			}
		}
		else if(playerList[currentPlayerIndex].GetComponent<Specialist>()!=null){ //Checks if script is attached
			
			if (playerList[currentPlayerIndex].GetComponent<Specialist>().HP > 0){ //Get the script
				playerList[currentPlayerIndex].GetComponent<Specialist>().TurnOnGUI();
			}
		}
		else if(playerList[currentPlayerIndex].GetComponent<Helicopter>()!=null){ //Checks if script is attached
			
			if (playerList[currentPlayerIndex].GetComponent<Helicopter>().HP > 0){ //Get the script
				playerList[currentPlayerIndex].GetComponent<Helicopter>().TurnOnGUI(); //Turn on the GUI for player
			}
		}
	}
	
	void OnGUI() {	
		float widthScale = 0.08f;

		//Move, Attack, End Turn Button
		whoToTurnOnGui ();
		
		//Restart Button
		if (GUI.Button (new Rect (5, 10,  Screen.width * widthScale, 20), "Restart")) // make the GUI button with name "pause"
		{
			Application.LoadLevel(Application.loadedLevel);
		}

		//Save botton
		if (GUI.Button (new Rect (5, 30,  Screen.width * widthScale, 20), "Exit Game")) // this function to save the game
		{
			Application.LoadLevel ("LevelSelectScene");
		}

		//Save botton
		if (GUI.Button (new Rect (5, 50,  Screen.width * widthScale, 20), "Save")) // this function to save the game
		{
			PlayerPrefs.SetInt("save player",currentPlayerIndex);
			PlayerPrefs.Save();
		}

		//Here is the GUI for outputing score, now do nothing yet.
		// we will add the player's scores here
		//GUI.Label (new Rect (Screen.width - 100, 10, 100, 50), "Score:" + scores.ToString ());
		// we will add the lives here depending on the player, by passing variable from player attack
		//GUI.Label (new Rect (Screen.width - 100, 30, 100, 50), "Lives:" + scores.ToString 
	}	
}
