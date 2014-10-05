using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour,  GameManagerInteferface {

	public GameObject PlayerPrefab;
	public static GameManager instance;
	public GameObject TilePrefab;
	public GameObject AIPrefab;
	public List <Player> playerList;
	public List<AiPlayer> aiList;
	public List <List<Tile>> map;
	public Transform mapTransform;
	UserPlayer player;
	public int mapSize = 11; //The size of the map i 

	private const float PLAYER_HEIGHT = 1.5f; 	//Used to spawn game objects 1.5 above the map so they are not in collision
	private const float AI_HEIGHT = 1.0f;
	int currentPlayerIndex;//Iterates throught the player list
	int currentAIIndex; //Iterates throught the AI list


	void Awake(){
		instance = this;

	}
	void Start () {
		playerList = new List<Player> ();
		aiList = new List<AiPlayer> ();
		currentPlayerIndex = 0;
		currentAIIndex = 0;
		generateMap (); //Generate map
		spawnPlayers(); //spawn players to be controlled by users
		spawnAI(); //Spawn AI opponent for the player
	}
	

	void Update () {
		playerList[currentPlayerIndex].TurnUpdate();

		/*
		//If player is not dead enable the GUI 
		if (playerList[currentPlayerIndex].HP > 0) {
			playerList[currentPlayerIndex].DisplayHP();
		}
		//If player  is dead delete game object
		else if(playerList[currentPlayerIndex].HP <= 0){
			Destroy(playerList[currentPlayerIndex].gameObject);
		}
		//If AI is dead, delete game object
		else if(AIList[currentPlayerIndex].HP <= 0){
			Destroy(AIList[currentAIIndex].gameObject);
		} */
	}

	public void nextTurn() {

		/*Iterating through the list, the current index is the current player's turn
		 *When it reaches the length of the list goes back to player at index 0 turn 
		 */
		if (currentPlayerIndex + 1 < playerList.Count) {
			currentPlayerIndex++						;
		} else {
			currentPlayerIndex = 0;
		}
	}

	public void AttackPlayer(){
	
	}

	//Moves the player to the selected tile by player
	public void MovePlayer(Tile destination){
		playerList[currentPlayerIndex].destination = destination.transform.position + PLAYER_HEIGHT * Vector3.up;
	}

	//For future implementation that shows all tile range the player can move to
	public void enablePathHighlight(){

	}

	public void disablePathHighlight(){
	
	}

	/* Used to spawn the map 
	 * Map is made up of multiple spawned cubes
	 * The 0,0 coordinate the very center cube
	 */
	public void generateMap(){
		map = new List<List<Tile>>();
		for (int i = 0; i < mapSize; i++) {
			List <Tile> row = new List<Tile>();
			for (int j = 0; j < mapSize; j++) {

				//Cubes spawn around the center cube, whose position is (0,0)
				Tile tile = ((GameObject)Instantiate(TilePrefab, new Vector3(i - Mathf.Floor(mapSize/2),0, -j + Mathf.Floor(mapSize/2))
				                                     , Quaternion.Euler(new Vector3()))).GetComponent<Tile>();

				//Quaternion.Euler(new Vector3()) is used as the default rotation, I found it to work the best
				tile.gridPosition = new Vector2(i, j);

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

	//Spawns players at specific map location, depending on the map
	public void spawnPlayers(){

		//Spawn player and add it to the list
		player = ((GameObject)Instantiate(PlayerPrefab, new Vector3(0 - Mathf.Floor(mapSize/2),1.5f, -0 + Mathf.Floor(mapSize/2)), Quaternion.Euler(new Vector3()))).GetComponent<UserPlayer>();
		
		playerList.Add(player);
		
		player = ((GameObject)Instantiate(PlayerPrefab, new Vector3((mapSize-1) - Mathf.Floor(mapSize/2),PLAYER_HEIGHT, -(mapSize-1) + Mathf.Floor(mapSize/2)), Quaternion.Euler(new Vector3()))).GetComponent<UserPlayer>();
		
		playerList.Add(player);

	}

	//Spawns AI(s) at specific map location, depending on the map
	public void spawnAI(){

		//Spawn AI and add it to list
		AiPlayer aiplayer = ((GameObject)Instantiate(AIPrefab, new Vector3(9 - Mathf.Floor(mapSize/2),AI_HEIGHT, -4 + Mathf.Floor(mapSize/2)), Quaternion.Euler(new Vector3()))).GetComponent<AiPlayer>();
		
		playerList.Add(aiplayer);
	}

}
