using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour {
	public static GameManager instance;
	
	/*Prefabs needed for game*/
	public GameObject TilePrefab;
	public GameObject AIPrefab;
	public GameObject tile;
	public GameObject jetPrefab; 
	public GameObject soldierPrefab;
	public GameObject AlienTroopPrefab;
	public GameObject alienShipPrefab;
	public GameObject berserkerPrefab;
	public GameObject tankPrefab;
	public GameObject PlayerPrefab;
	
	public List <GameObject> playerList;
	public List <List<GameObject>> map;
	public List <GameObject> aiList;
	public Transform mapTransform;
	
		
	GameObject player;
	Tile grid;
	public int mapSize = 11; //The size of the map i 
	
	public int playerCount;
	public int aiCount;
	
	private const float PLAYER_HEIGHT = -1.0f; 	//Used to spawn game objects 1.5 above the map so they are not in collision
	private const float AI_HEIGHT = -0.25f;
	public int currentPlayerIndex;//Iterates throught the player list
	public int currentAIIndex; //Iterates throught the AI list
	
	//Use for Panel
	public bool IsPause; 
	public int scores;	

	void Awake(){
		instance = this;
	}

	void Start () {
		playerList = new List<GameObject> ();
		aiList = new List<GameObject> ();
		currentPlayerIndex = 0;
		currentAIIndex = 0;
		playerCount = 0;
		aiCount = 0;
		generateMap (); //Generate map
		spawnPlayers(); //spawn players to be controlled by users
		spawnAI(); //Spawn AI opponent for the player

		IsPause = true;
		scores = 0;
	}


	void Update () {
		if(playerCount <= 0){
			print ("You lose");
			Application.Quit(); //Temp game over
		}
		if(aiCount <= 0){
			print ("You win");
			Application.Quit(); //Temp game over
		}

		if( playerList.Count > 0){
			
			getHumanTurn(); //For the future

			GameObject temp = playerList [currentPlayerIndex];
			if(temp.GetComponent<UserPlayer>() != null){
				UserPlayer player  = temp.GetComponent<UserPlayer>();
				player.TurnUpdate ();
				
				if (player.HP <= 0){
					Destroy(playerList[currentPlayerIndex]);
					playerList.RemoveAt(currentPlayerIndex);
					playerCount -=1;
				}
				
			}
			else if(temp.GetComponent<AiPlayer>() != null){
				AiPlayer ai  = temp.GetComponent<AiPlayer>();
				ai.TurnUpdate ();
				
				if (ai.HP <= 0){
					Destroy(playerList[currentPlayerIndex]);
					playerList.RemoveAt(currentPlayerIndex);
					playerCount -=1;
				}
				
			}
		}

	}

	public void deletePlayer(){
		Destroy(playerList[currentPlayerIndex]);
		playerList.RemoveAt(currentPlayerIndex);
		playerCount -=1;
	}

	public void deleteAI(){
		Destroy(playerList[currentPlayerIndex]);
		playerList.RemoveAt(currentPlayerIndex);
		aiCount -=1;
	}

	/*
	 * Finds the current player player's type 
	 * (Ex. Tank, Soldier, Medic,etc)
	 */
	//Can possibly refactor code in the future
	public void getHumanTurn(){
		GameObject temp = playerList [currentPlayerIndex];
		if(temp.GetComponent<Tank>() != null){ //Check what script is attached to prefab
			Tank tank  = temp.GetComponent<Tank>();
			if (tank.HP <= 0){ //Check if player is not dead
				deletePlayer();
			}
			tank.TurnUpdate ();			
			
		}
		else if(temp.GetComponent<Soldier>() != null){
			Soldier soldier  = temp.GetComponent<Soldier>();
			if (soldier.HP <= 0){
				deletePlayer();
			}
			soldier.TurnUpdate ();	
						
		}

		else if(temp.GetComponent<Medic>() != null){
			Medic medic  = temp.GetComponent<Medic>();
			if (medic.HP <= 0){
				deletePlayer();
			}
			medic.TurnUpdate ();			

		}
		else if(temp.GetComponent<Specialist>() != null){
			Specialist spec  = temp.GetComponent<Specialist>();
			if (spec.HP <= 0){
				deletePlayer();
			}
			spec.TurnUpdate ();		
				
		}
		else if(temp.GetComponent<Helicopter>() != null){
			Helicopter heli  = temp.GetComponent<Helicopter>();
			if (heli.HP <= 0){
				deletePlayer();
			}	
			heli.TurnUpdate ();			

		}
		else if(temp.GetComponent<Jet>() != null){
			Jet jet  = temp.GetComponent<Jet>();
			if (jet.HP <= 0){
				deletePlayer();
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
	//Can possibly refactor code in the future
	public void getAlienTurn(){
		GameObject temp = playerList [currentPlayerIndex];
		
		if(temp.GetComponent<AlienShip>() != null){ //Checks for what script AI has attached
			AlienShip ship  = temp.GetComponent<AlienShip>(); //Get the current player script
			if (ship.HP <= 0){ //If AI is dead, remove from game
				deleteAI(); 
			}
			ship.TurnUpdate (); //AI's turn
			
		}
		
		else if (temp.GetComponent<AlienSoldier>() != null){
			AlienSoldier alienSoldier  = temp.GetComponent<AlienSoldier>();
			if (alienSoldier.HP <= 0){
				deleteAI();
			}
			alienSoldier.TurnUpdate ();
			
		}
		
		else if (temp.GetComponent<AlienSupport>() != null){
			AlienSupport alienSupport  = temp.GetComponent<AlienSupport>();
			if (alienSupport.HP <= 0){
				deleteAI();
			}
			alienSupport.TurnUpdate ();
			
		}
		else if (temp.GetComponent<Berserker>() != null){
			Berserker berserker  = temp.GetComponent<Berserker>();
			if (berserker.HP <= 0){
				deleteAI();
			}
			berserker.TurnUpdate ();
			
		}
	}
	public void nextTurn() {

		/*Iterating through the list, the current index is the current player's turn
		 *When it reaches the length of the list goes back to player at index 0 turn 
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
	
	public void setOccupied(Tile destination, Player player){
		int x, y;
		
		//Grid postion correlates to the map list indexes
		x = (int) player.gridPosition.x;
		y = (int) player.gridPosition.y;
		
		//Setting the old position to unoccupied
		Tile Maptemp = map [x] [y].GetComponent<Tile>();
		map [x] [y].GetComponent<Tile> ().isOccupied = false;
		Maptemp.isOccupied = false;
		//Setting new position to occupied
		destination.isOccupied=true;
	}

	//To realize the player movement
	//Can possibly refactor code in the future
	public void MovePlayer(Tile destination){
		if (canPlayerMove(destination)) { // If tile is unoccupied player can move to there
			
			/****Test unit******/
			if(playerList [currentPlayerIndex].GetComponent<UserPlayer>() != null){
				UserPlayer Playertemp = playerList [currentPlayerIndex].GetComponent<UserPlayer>();
				setOccupied(destination, (Player) Playertemp);
				Playertemp.moveDestination = destination.transform.position + PLAYER_HEIGHT * Vector3.forward;
				Playertemp.gridPosition = destination.gridPosition;
				
				
			}
			else if(playerList [currentPlayerIndex].GetComponent<Tank>() != null){ //Checks for what script is attached to player
				Tank tankTemp = playerList [currentPlayerIndex].GetComponent<Tank>();
				setOccupied(destination, (Player) tankTemp); //Sets new tile location occupied
				tankTemp.moveDestination = destination.transform.position + PLAYER_HEIGHT * Vector3.forward; //Moves player unit
				tankTemp.gridPosition = destination.gridPosition; //Sets new grid position for unit
				
			}
			
			else if(playerList [currentPlayerIndex].GetComponent<Soldier>() != null){ //Checks for what script is attached to player
				Soldier soldierTemp = playerList [currentPlayerIndex].GetComponent<Soldier>();
				setOccupied(destination, (Player) soldierTemp);  //Sets new tile location occupied
				soldierTemp.moveDestination = destination.transform.position + PLAYER_HEIGHT * Vector3.forward; //Moves player unit
				soldierTemp.gridPosition = destination.gridPosition; //Sets new grid position for unit
				
			}
			
			else if(playerList [currentPlayerIndex].GetComponent<Medic>() != null){ //Checks for what script is attached to player
				Medic medicTemp = playerList [currentPlayerIndex].GetComponent<Medic>();
				setOccupied(destination, (Player) medicTemp);   //Sets new tile location occupied
				medicTemp.moveDestination = destination.transform.position + PLAYER_HEIGHT * Vector3.forward; //Moves player unit
				medicTemp.gridPosition = destination.gridPosition; //Sets new grid position for unit
				
			}
			else if(playerList [currentPlayerIndex].GetComponent<Specialist>() != null){ //Checks for what script is attached to player
				Specialist specTemp = playerList [currentPlayerIndex].GetComponent<Specialist>();
				setOccupied(destination, (Player) specTemp); //Sets new tile location occupied
				specTemp.moveDestination = destination.transform.position + PLAYER_HEIGHT * Vector3.forward; //Moves player unit
				specTemp.gridPosition = destination.gridPosition; //Sets new grid position for unit
				
			}
			else if(playerList [currentPlayerIndex].GetComponent<Jet>() != null){ //Checks for what script is attached to player
				Jet jetTemp = playerList [currentPlayerIndex].GetComponent<Jet>();
				setOccupied(destination, (Player) jetTemp); //Sets new tile location occupied
				jetTemp.moveDestination = destination.transform.position + PLAYER_HEIGHT * Vector3.forward; //Moves player unit
				jetTemp.gridPosition = destination.gridPosition; //Sets new grid position for unit
				
			}
			else if(playerList [currentPlayerIndex].GetComponent<Helicopter>() != null){ //Checks for what script is attached to player
				Helicopter heliTemp = playerList [currentPlayerIndex].GetComponent<Helicopter>();
				setOccupied(destination, (Player) heliTemp);  //Sets new tile location occupied
				heliTemp.moveDestination = destination.transform.position + PLAYER_HEIGHT * Vector3.forward; //Moves player unit
				heliTemp.gridPosition = destination.gridPosition; //Sets new grid position for unit
				
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
				AlienSoldier alienTemp = playerList [currentPlayerIndex].GetComponent<AlienSoldier>(); //Checks if script is attached to player
				setOccupied(destination, (Player) alienTemp); //Sets destination tile as occupied
				alienTemp.moveDestination = destination.transform.position + PLAYER_HEIGHT * Vector3.forward; //Moves player to new tile
				alienTemp.gridPosition = destination.gridPosition; //Sets the new grid postion of unit 
				
			}
		}
		if (canPlayerMove(destination)) {
			if(playerList [currentPlayerIndex].GetComponent<AlienSupport>() != null){  //Checks if script is attached to player
				AlienSupport alienSupTemp = playerList [currentPlayerIndex].GetComponent<AlienSupport>();
				setOccupied(destination, (Player) alienSupTemp); //Sets destination tile as occupied
				alienSupTemp.moveDestination = destination.transform.position + PLAYER_HEIGHT * Vector3.forward; //Moves player to new tile
				alienSupTemp.gridPosition = destination.gridPosition; //Sets the new grid postion of unit 
				
			}
		}
		if (canPlayerMove(destination)) {
			if(playerList [currentPlayerIndex].GetComponent<AlienShip>() != null){  //Checks if script is attached to player
				AlienShip alienShipTemp = playerList [currentPlayerIndex].GetComponent<AlienShip>();
				setOccupied(destination, (Player) alienShipTemp); //Sets destination tile as occupied
				alienShipTemp.moveDestination = destination.transform.position + PLAYER_HEIGHT * Vector3.forward; //Moves player to new tile
				alienShipTemp.gridPosition = destination.gridPosition; //Sets the new grid postion of unit 
				
			}
		}
		if (canPlayerMove(destination)) {
			if(playerList [currentPlayerIndex].GetComponent<AlienSoldier>() != null){  //Checks if script is attached to player
				AlienSoldier alienTemp = playerList [currentPlayerIndex].GetComponent<AlienSoldier>();
				setOccupied(destination, (Player) alienTemp); //Sets destination tile as occupied
				alienTemp.moveDestination = destination.transform.position + PLAYER_HEIGHT * Vector3.forward; //Moves player to new tile
				alienTemp.gridPosition = destination.gridPosition; //Sets the new grid postion of unit 
				
			}
		}
		if (canPlayerMove(destination)) {
			if(playerList [currentPlayerIndex].GetComponent<Berserker>() != null){  //Checks if script is attached to player
				Berserker berserkerTemp = playerList [currentPlayerIndex].GetComponent<Berserker>();
				setOccupied(destination, (Player) berserkerTemp); //Sets destination tile as occupied
				berserkerTemp.moveDestination = destination.transform.position + PLAYER_HEIGHT * Vector3.forward; //Moves player to new tile
				berserkerTemp.gridPosition = destination.gridPosition; //Sets the new grid postion of unit 
				
			}
		}
	}		


	/*
	 * Checks what script is attached to the current player's turn  
	 * Then calls the attack function of that player's class
	 * Attacks the unit on the target tile
	 */
	//Can possibly refactor in the future
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


	//Null exception here, will fix later
	//When click move botton, the available range for player will Highlighted
	public void enableAttackHighlight(int originLocationX, int originLocationY, int range){

		if(originLocationX-range> 0 && originLocationY-range> 0 && originLocationX+range < this.mapSize && originLocationY+range<this.mapSize){
			this.map [originLocationX] [originLocationY-range].GetComponent<Tile> ().transform.renderer.material.color = Color.red;
			this.map [originLocationX] [originLocationY+range].GetComponent<Tile> ().transform.renderer.material.color = Color.red;

			this.map [originLocationX-range] [originLocationY].GetComponent<Tile> ().transform.renderer.material.color = Color.red;
			this.map [originLocationX+range] [originLocationY].GetComponent<Tile> ().transform.renderer.material.color = Color.red;

			this.map [originLocationX-range] [originLocationY-range].GetComponent<Tile> ().transform.renderer.material.color = Color.red;
			this.map [originLocationX+range] [originLocationY+range].GetComponent<Tile> ().transform.renderer.material.color = Color.red;

			this.map [originLocationX-range] [originLocationY+range].GetComponent<Tile> ().transform.renderer.material.color = Color.red;
			this.map [originLocationX+range] [originLocationY-range].GetComponent<Tile> ().transform.renderer.material.color = Color.red;

		}
	}

	//When click move botton, the available range for player will Highlighted
	public void enableMoveHighlight(int originLocationX, int originLocationY, int range){

		/* Iterate through the map to highlight the tiles that in its move range
		 * different part of the map has different methods to choose tiles
		 */

		if (originLocationX >= originLocationY) {
			for (int i = 0; i < mapSize; i++) {
				for (int j = 0; j < mapSize; j++) {
					if( i + j <= originLocationX + originLocationY + range && i + j >= originLocationX + originLocationY - range 
					   && i<= originLocationX + range && i >= originLocationX - range &&  j<= originLocationY + range && j >= originLocationY - range
					   && (i - j) <= Mathf.Abs(originLocationX - originLocationY) + range &&  (i - j) >= Mathf.Abs(originLocationX - originLocationY) - range){
						this.map [i] [j].GetComponent<Tile> ().transform.renderer.material.color = Color.blue;
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
						this.map [i] [j].GetComponent<Tile> ().transform.renderer.material.color = Color.blue;
					}
				}
			}
		}

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
				tile = Instantiate(TilePrefab, new Vector2(i - Mathf.Floor(mapSize/2), -j + Mathf.Floor(mapSize/2)),Quaternion.identity) as GameObject;
				Tile maptemp = tile.GetComponent<Tile>();
				maptemp.gridPosition = new Vector2(i,j);

				row.Add (tile);
			}
			//Add each cube to a list
			map.Add(row);
		}
	}

	//For future implementation
	//Reads from xml file and generates a map
	public void loadMapFromXml() {
	}

	/*
	 * Spawns players onto the map and increment player count
	 * Currently spawned at fixed locations
	 */
	public void spawnPlayers(){
		//Spawn first player and add it to the list
		/********************Spawning tank*****************************/
		GameObject tank;
		
		tank = Instantiate(tankPrefab, new Vector3(0 - Mathf.Floor(mapSize/2), -0 + Mathf.Floor(mapSize/2), PLAYER_HEIGHT),Quaternion.identity) as GameObject;
		playerList.Add(tank); //Add to player list
		Tank tankTemp = tank.GetComponent<Tank> ();
		tankTemp.gridPosition = new Vector2 (0, 0); //Setting grid position to their fixed spawn location
		playerCount += 1; //Increment player count
		
		/********************Spawning Jet*****************************/
		GameObject jet;
		jet = Instantiate(jetPrefab, new Vector3(-6, -6,PLAYER_HEIGHT),Quaternion.identity) as GameObject;
		playerList.Add(jet); //Add to player list
		Jet jetTemp=jet.GetComponent<Jet> ();
		jetTemp.gridPosition = new Vector2 (0, 12); //Setting grid position to their fixed spawn location
		playerCount += 1; //Increment player count
		
		/********************Spawning solider*****************************/
		GameObject soldier;
		soldier = Instantiate(soldierPrefab, new Vector3(-6, -5,PLAYER_HEIGHT),Quaternion.identity) as GameObject;
		playerList.Add(soldier); //Add to player list
		Soldier soldierTemp=soldier.GetComponent<Soldier> ();
		soldierTemp.gridPosition = new Vector2 (0, 11); //Setting grid position to their fixed spawn location
		playerCount += 1; //Increment player count
		
		
	}
	/*
	 * Spawns AI onto the map and increment AI count
	 * Currently spawned at fixed locations
	 */
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
	//Added panel from here for genearl game control
	void OnGUI()
	{	

		//Move Button
		whoToTurnOnGui ();
		
		//Pause Button
		if (GUI.Button (new Rect (100, 10, 80, 20), "Pause")) // make the GUI button with name "pause"
		{if (IsPause)											// if its pause, timeScale = 0
			{Time.timeScale=0;
				IsPause =false;	
				//set the pause to false again
				//need to enable something actions here
			}
			else{
				Time.timeScale=1;
				IsPause=true;
			}
		}

		//Save botton
		if (GUI.Button (new Rect (100, 30, 80, 20), "Save"))// this function to save the game
		{
			PlayerPrefs.SetInt("save player",currentPlayerIndex);
			PlayerPrefs.Save();
		}

		//Here is the GUI for outputing score, now do nothing yet.
		GUI.Label (new Rect (Screen.width - 100, 10, 100, 50), "Score:" + scores.ToString ());// we will add the player's scores here
		GUI.Label (new Rect (Screen.width - 100, 30, 100, 50), "Lives:" + scores.ToString ());// we will add the lives here depending on the player, by passing variable from player attack

		
	}	
}
