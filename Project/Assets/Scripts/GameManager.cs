using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour {
	public GameObject tankPrefab;
	public GameObject PlayerPrefab;
	public static GameManager instance;
	public GameObject TilePrefab;
	public GameObject AIPrefab;
	public GameObject tile;
	public GameObject jetPrefab;
	public GameObject soldierPrefab;
	public List <GameObject> playerList;
	public List <List<GameObject>> map;
	public List <GameObject> aiList;
	public Transform mapTransform;
	public GameObject AlienTroopPrefab;
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

	//Can possibly refactor code in the future
	public void getHumanTurn(){
		GameObject temp = playerList [currentPlayerIndex];
		if(temp.GetComponent<Tank>() != null){
			Tank tank  = temp.GetComponent<Tank>();
			if (tank.HP <= 0){
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
	//Can possibly refactor code in the future
	public void getAlienTurn(){
		GameObject temp = playerList [currentPlayerIndex];

		if(temp.GetComponent<AlienShip>() != null){
			AlienShip ship  = temp.GetComponent<AlienShip>();
			if (ship.HP <= 0){
				deleteAI();
			}
			ship.TurnUpdate ();

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
		if (canPlayerMove(destination)) {
			if(playerList [currentPlayerIndex].GetComponent<UserPlayer>() != null){
				UserPlayer Playertemp = playerList [currentPlayerIndex].GetComponent<UserPlayer>();
				setOccupied(destination, (Player) Playertemp);
				Playertemp.moveDestination = destination.transform.position + PLAYER_HEIGHT * Vector3.forward;
				Playertemp.gridPosition = destination.gridPosition;

		
			}
			else if(playerList [currentPlayerIndex].GetComponent<Tank>() != null){
				Tank tankTemp = playerList [currentPlayerIndex].GetComponent<Tank>();
			//	setOccupied(destination, (Player) tankTemp); //Redundant cast? Have not tested
				tankTemp.moveDestination = destination.transform.position + PLAYER_HEIGHT * Vector3.forward;
				tankTemp.gridPosition = destination.gridPosition;
				
			}

			else if(playerList [currentPlayerIndex].GetComponent<Soldier>() != null){
				Soldier soldierTemp = playerList [currentPlayerIndex].GetComponent<Soldier>();
				setOccupied(destination, (Player) soldierTemp); //Redundant cast? Have not tested
				soldierTemp.moveDestination = destination.transform.position + PLAYER_HEIGHT * Vector3.forward;
				soldierTemp.gridPosition = destination.gridPosition;
				
			}

			else if(playerList [currentPlayerIndex].GetComponent<Medic>() != null){
				Medic medicTemp = playerList [currentPlayerIndex].GetComponent<Medic>();
				setOccupied(destination, (Player) medicTemp); //Redundant cast? Have not tested
				medicTemp.moveDestination = destination.transform.position + PLAYER_HEIGHT * Vector3.forward;
				medicTemp.gridPosition = destination.gridPosition;
				
			}
			else if(playerList [currentPlayerIndex].GetComponent<Specialist>() != null){
				Specialist specTemp = playerList [currentPlayerIndex].GetComponent<Specialist>();
				setOccupied(destination, (Player) specTemp); //Redundant cast? Have not tested
				specTemp.moveDestination = destination.transform.position + PLAYER_HEIGHT * Vector3.forward;
				specTemp.gridPosition = destination.gridPosition;
				
			}
			else if(playerList [currentPlayerIndex].GetComponent<Jet>() != null){
				Jet jetTemp = playerList [currentPlayerIndex].GetComponent<Jet>();
				setOccupied(destination, (Player) jetTemp); //Redundant cast? Have not tested
				jetTemp.moveDestination = destination.transform.position + PLAYER_HEIGHT * Vector3.forward;
				jetTemp.gridPosition = destination.gridPosition;
				
			}
			else if(playerList [currentPlayerIndex].GetComponent<Helicopter>() != null){
				Helicopter heliTemp = playerList [currentPlayerIndex].GetComponent<Helicopter>();
				setOccupied(destination, (Player) heliTemp); //Redundant cast? Have not tested
				heliTemp.moveDestination = destination.transform.position + PLAYER_HEIGHT * Vector3.forward;
				heliTemp.gridPosition = destination.gridPosition;
				
			}
		}
		else{
			print ("Cannot move there");
		}
	}

	//Can possibly refactor code in the future
	public void moveAlien(Tile destination){
		if (canPlayerMove(destination)) {
			if(playerList [currentPlayerIndex].GetComponent<AlienSoldier>() != null){
				AlienSoldier alienTemp = playerList [currentPlayerIndex].GetComponent<AlienSoldier>();
				setOccupied(destination, (Player) alienTemp);
				alienTemp.moveDestination = destination.transform.position + PLAYER_HEIGHT * Vector3.forward;
				alienTemp.gridPosition = destination.gridPosition;
				
			}
		}
		if (canPlayerMove(destination)) {
			if(playerList [currentPlayerIndex].GetComponent<AlienSupport>() != null){
				AlienSupport alienSupTemp = playerList [currentPlayerIndex].GetComponent<AlienSupport>();
				setOccupied(destination, (Player) alienSupTemp);
				alienSupTemp.moveDestination = destination.transform.position + PLAYER_HEIGHT * Vector3.forward;
				alienSupTemp.gridPosition = destination.gridPosition;
				
			}
		}
		if (canPlayerMove(destination)) {
			if(playerList [currentPlayerIndex].GetComponent<AlienShip>() != null){
				AlienShip alienShipTemp = playerList [currentPlayerIndex].GetComponent<AlienShip>();
				setOccupied(destination, (Player) alienShipTemp);
				alienShipTemp.moveDestination = destination.transform.position + PLAYER_HEIGHT * Vector3.forward;
				alienShipTemp.gridPosition = destination.gridPosition;
								
			}
		}
		if (canPlayerMove(destination)) {
			if(playerList [currentPlayerIndex].GetComponent<AlienSoldier>() != null){
				AlienSoldier alienTemp = playerList [currentPlayerIndex].GetComponent<AlienSoldier>();
				setOccupied(destination, (Player) alienTemp);
				alienTemp.moveDestination = destination.transform.position + PLAYER_HEIGHT * Vector3.forward;
				alienTemp.gridPosition = destination.gridPosition;
								
			}
		}
		if (canPlayerMove(destination)) {
			if(playerList [currentPlayerIndex].GetComponent<Berserker>() != null){
				Berserker berserkerTemp = playerList [currentPlayerIndex].GetComponent<Berserker>();
				setOccupied(destination, (Player) berserkerTemp);
				berserkerTemp.moveDestination = destination.transform.position + PLAYER_HEIGHT * Vector3.forward;
				berserkerTemp.gridPosition = destination.gridPosition;
								
			}
		}
	}


	public void whatPlayerClassIsAttacking(Tile targetTile){
		if (playerList [currentPlayerIndex].GetComponent<UserPlayer> () != null) {
			UserPlayer temp = playerList [currentPlayerIndex].GetComponent<UserPlayer>();
			temp.tempAttack(targetTile);						
		}	
		else if (playerList [currentPlayerIndex].GetComponent<Soldier> () != null) {
			Soldier temp = playerList [currentPlayerIndex].GetComponent<Soldier>();
			temp.getEnemyToAttack(targetTile);						
		}
		else if (playerList [currentPlayerIndex].GetComponent<Medic> () != null) {
			Medic temp = playerList [currentPlayerIndex].GetComponent<Medic>();
			temp.getEnemyToAttack(targetTile);						
		}
		else if (playerList [currentPlayerIndex].GetComponent<Specialist> () != null) {
			Specialist temp = playerList [currentPlayerIndex].GetComponent<Specialist>();
			temp.getEnemyToAttack(targetTile);						
		}
		else if (playerList [currentPlayerIndex].GetComponent<Tank> () != null) {
			Tank temp = playerList [currentPlayerIndex].GetComponent<Tank>();
			temp.getEnemyToAttack(targetTile);						
		}
		else if (playerList [currentPlayerIndex].GetComponent<Jet> () != null) {
			Jet temp = playerList [currentPlayerIndex].GetComponent<Jet>();
			temp.getEnemyToAttack(targetTile);						
		}
		else if (playerList [currentPlayerIndex].GetComponent<Helicopter> () != null) {
			Helicopter temp = playerList [currentPlayerIndex].GetComponent<Helicopter>();
			temp.getEnemyToAttack(targetTile);						
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

	List<Vector2> highLightTileVector;
	//Null exception here, will fix later
	//When click move botton, the available path for player will Highlighted
	public void enableMoveHighlight(int originLocationX, int originLocationY, int range){
		int startX = originLocationX - range;
		if (startX <= 0) {
			startX = originLocationX;
		}
		int startY = originLocationY - range; 
		if (startY <= 0) {
			startY = originLocationY;
		}
		while (startX<(range*2)+1) {
			this.map [startX] [originLocationY-range].GetComponent<Tile> ().transform.renderer.material.color = Color.blue;
			startX+=1;

		}
		this.map [originLocationX] [originLocationY-range].GetComponent<Tile> ().transform.renderer.material.color = Color.blue;
		this.map [originLocationX] [originLocationY+range].GetComponent<Tile> ().transform.renderer.material.color = Color.blue;
		
		this.map [originLocationX-range] [originLocationY].GetComponent<Tile> ().transform.renderer.material.color = Color.blue;
		this.map [originLocationX+range] [originLocationY].GetComponent<Tile> ().transform.renderer.material.color = Color.blue;
		
		this.map [originLocationX-range] [originLocationY-range].GetComponent<Tile> ().transform.renderer.material.color = Color.blue;
		this.map [originLocationX+range] [originLocationY+range].GetComponent<Tile> ().transform.renderer.material.color = Color.blue;
		
		this.map [originLocationX-range] [originLocationY+range].GetComponent<Tile> ().transform.renderer.material.color = Color.blue;
		this.map [originLocationX+range] [originLocationY-range].GetComponent<Tile> ().transform.renderer.material.color = Color.blue;
	}

	//Wait for Lujie Duan's enableMoveHighlight method
	//After the movement, hightlight paht set to be disable
//	public void disableMoveHightLight(){
//		int count = highLightTileVector.Count;
//		foreach(Vector2 element in highLightTileVector) {
//			this.map[(int)element.x][(int)element.y].GetComponent<Tile>().transform.renderer.material.color = Color.clear;
//		}
//	}

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

	public void spawnPlayers(){
		//Spawn first player and add it to the list
		/********************Spawning tank*****************************/
		GameObject tank;

		tank = Instantiate(tankPrefab, new Vector3(0 - Mathf.Floor(mapSize/2), -0 + Mathf.Floor(mapSize/2), PLAYER_HEIGHT),Quaternion.identity) as GameObject;
		playerList.Add(tank);
		Tank tankTemp = tank.GetComponent<Tank> ();
		tankTemp.gridPosition = new Vector2 (0, 0);
		playerCount += 1;

		/********************Spawning Jet*****************************/
		GameObject jet;
		jet = Instantiate(jetPrefab, new Vector3(-6, -6,PLAYER_HEIGHT),Quaternion.identity) as GameObject;
		playerList.Add(jet);
		playerCount += 1;

		/********************Spawning solider*****************************/
		GameObject soldier;
		soldier = Instantiate(soldierPrefab, new Vector3(-6, -5,PLAYER_HEIGHT),Quaternion.identity) as GameObject;
		playerList.Add(soldier);
		playerCount += 1;


	}
	public void spawnAI(){

		GameObject aiplayer = Instantiate(AlienTroopPrefab, new Vector3(6 - Mathf.Floor(mapSize/2), -6 + Mathf.Floor(mapSize/2), PLAYER_HEIGHT),Quaternion.identity) as GameObject;
		playerList.Add(aiplayer);
		aiCount += 1;
		aiList.Add(aiplayer);
		AlienSoldier temp = aiplayer.GetComponent<AlienSoldier> ();
		temp.gridPosition = new Vector2 (6, 6);

	}
	public void whoToTurnOnGui(){
		if(playerList[currentPlayerIndex].GetComponent<UserPlayer>()!=null){
			if (playerList[currentPlayerIndex].GetComponent<UserPlayer>().HP > 0){
				playerList[currentPlayerIndex].GetComponent<UserPlayer>().TurnOnGUI();
				
				
			}
		}
		else if(playerList[currentPlayerIndex].GetComponent<Tank>()!=null){
			if (playerList[currentPlayerIndex].GetComponent<Tank>().HP > 0){
				playerList[currentPlayerIndex].GetComponent<Tank>().TurnOnGUI();
				
				
			}
		}
		else if(playerList[currentPlayerIndex].GetComponent<Jet>()!=null){
			if (playerList[currentPlayerIndex].GetComponent<Jet>().HP > 0){
				playerList[currentPlayerIndex].GetComponent<Jet>().TurnOnGUI();
				
				
			}
		}
		else if(playerList[currentPlayerIndex].GetComponent<Soldier>()!=null){
			if (playerList[currentPlayerIndex].GetComponent<Soldier>().HP > 0){
				playerList[currentPlayerIndex].GetComponent<Soldier>().TurnOnGUI();
				
				
			}
		}
		else if(playerList[currentPlayerIndex].GetComponent<Medic>()!=null){
			if (playerList[currentPlayerIndex].GetComponent<Medic>().HP > 0){
				playerList[currentPlayerIndex].GetComponent<Medic>().TurnOnGUI();
				
				
			}
		}
		else if(playerList[currentPlayerIndex].GetComponent<Specialist>()!=null){
			if (playerList[currentPlayerIndex].GetComponent<Specialist>().HP > 0){
				playerList[currentPlayerIndex].GetComponent<Specialist>().TurnOnGUI();
				
				
			}
		}
		else if(playerList[currentPlayerIndex].GetComponent<Helicopter>()!=null){
			if (playerList[currentPlayerIndex].GetComponent<Helicopter>().HP > 0){
				playerList[currentPlayerIndex].GetComponent<Helicopter>().TurnOnGUI();
				
				
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
