using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour,  GameManagerInteferface {

	public GameObject PlayerPrefab;
	public static GameManager instance;
	public GameObject TilePrefab;
	public GameObject AIPrefab;
	public List <Player> playerList = new List<Player>();
	public List <AI> AIList = new List<AI> ();
	public List <List<Tile>> map = new List<List<Tile>> ();
	public Transform mapTransform;
	public int mapSize = 11; //The size of the map i 

	private const float PLAYER_HEIGHT = 1.5f; 	//Used to spawn game objects 1.5 above the map so they are not in collision
	private const float AI_HEIGHT = 1.0f;
	public int currentPlayerIndex = 0;//Iterates throught the player list
	public int currentAIIndex =0; //Iterates throught the AI list


	void Awake(){
		instance = this;
	
	}
	void Start () {
		generateMap (); //Generate map
		spawnPlayers(); //spawn players to be controlled by users
		spawnAI(); //Spawn AI opponent for the player
	}
	

	void Update () {
		//playerList [currentPlayerIndex].TurnUpdate ();
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
	public void MovePlayer(Tile destination){
		playerList [currentPlayerIndex].destination = destination.transform.position + PLAYER_HEIGHT * Vector3.up;
	}
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

				tile.gridPosition = new Vector2(i, j);

				row.Add (tile);
			}
			//Add each cube to a list
			map.Add(row);
		}
	}
	public void loadMapFromXml() {
	}

	public void spawnPlayers(){
		Player player;
		//Spawn player and add it to the list
		player = ((GameObject)Instantiate(PlayerPrefab, new Vector3(0 - Mathf.Floor(mapSize/2),PLAYER_HEIGHT, -0+Mathf.Floor(mapSize/2))
		                                     , Quaternion.Euler(new Vector3()))).GetComponent<Player>();

		playerList.Add (player);

		//Spawn player and add it to the list
		player = ((GameObject)Instantiate(PlayerPrefab, new Vector3((mapSize-1) - Mathf.Floor(mapSize/2),PLAYER_HEIGHT, -(mapSize-1) + Mathf.Floor(mapSize/2)), Quaternion.Euler(new Vector3()))).GetComponent<Player>();
		
		playerList.Add (player);
		Player AI;
		AI = ((GameObject)Instantiate(AIPrefab, new Vector3((mapSize-1) - Mathf.Floor(mapSize/2),AI_HEIGHT, mapSize-1 - Mathf.Floor(mapSize/2)), Quaternion.Euler(new Vector3()))).GetComponent<AI>();
		
		playerList.Add (AI);

	}
	public void spawnAI(){
	
	}

}
