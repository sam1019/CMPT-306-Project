using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour,  GameManagerInteferface {

	public GameObject PlayerPrefab;
	public static GameManager instance;
	public GameObject TilePrefab;
	public GameObject AIPrefab;
	public GameObject tile;
	public List <GameObject> playerList;
	public List <List<GameObject>> map;
	public Transform mapTransform;
	GameObject player;
	Tile grid;
	public int mapSize = 11; //The size of the map i 


	private const float PLAYER_HEIGHT = -0.25f; 	//Used to spawn game objects 1.5 above the map so they are not in collision
	private const float AI_HEIGHT = -0.25f;
	public int currentPlayerIndex;//Iterates throught the player list
	public int currentAIIndex; //Iterates throught the AI list
	

	void Awake(){
		instance = this;

	
	}
	void Start () {
		playerList = new List<GameObject> ();
		int currentPlayerIndex = 0;
		generateMap (); //Generate map
		spawnPlayers(); //spawn players to be controlled by users
		spawnAI(); //Spawn AI opponent for the player
	}
	

	void Update () {
		if (playerList [currentPlayerIndex] == null) {
			print ("NULL");		
		}
		GameObject temp = playerList [currentPlayerIndex];
		UserPlayer user  = temp.GetComponent<UserPlayer>();
		user.TurnUpdate ();
//		UserPlayer user = (UserPlayer) playerList [currentPlayerIndex].GetComponent<UserPlayer> ();
//		user.TurnUpdate ();


//		playerList[currentPlayerIndex].TurnUpdate();


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
//		playerList[currentPlayerIndex].moveDestination = destination.transform.position + PLAYER_HEIGHT * Vector3.up;
//        playerList[currentPlayerIndex].gridPosition = destination.gridPosition;
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
		map = new List<List<GameObject>>();
	
		for (int i = 0; i < mapSize; i++) {
			List <GameObject> row = new List<GameObject>();
			for (int j = 0; j < mapSize; j++) {

				//Tiles spawn around the center tile
				tile = Instantiate(TilePrefab, new Vector2(i - Mathf.Floor(mapSize/2), -j + Mathf.Floor(mapSize/2)),Quaternion.identity) as GameObject;
			
				//tile. = new Vector2(i, j);


				//tile.gridPosition = new Vector2(i,j);

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

		//Spawn player and add it to the list
		player = Instantiate(PlayerPrefab, new Vector3(0 - Mathf.Floor(mapSize/2), -0 + Mathf.Floor(mapSize/2), PLAYER_HEIGHT),Quaternion.identity) as GameObject;
		
		playerList.Add(player);
		
		player = Instantiate(PlayerPrefab, new Vector3(12 - Mathf.Floor(mapSize/2), -12 + Mathf.Floor(mapSize/2),PLAYER_HEIGHT),Quaternion.identity) as GameObject;
		playerList.Add(player);

		print (player.transform.position.x);
		print (player.transform.position.y);



		//Spawn AI and add it to list

		//AiPlayer aiplayer = ((GameObject)Instantiate(AIPrefab, new Vector3(9 - Mathf.Floor(mapSize/2),AI_HEIGHT, -4 + Mathf.Floor(mapSize/2)), Quaternion.Euler(new Vector3()))).GetComponent<AiPlayer>();

		//AiPlayer aiplayer = ((GameObject)Instantiate(AIPrefab, new Vector3(9 - Mathf.Floor(mapSize/2), -4 + Mathf.Floor(mapSize/2), AI_HEIGHT), Quaternion.Euler(new Vector3()))).GetComponent<AiPlayer>();

	}
	public void spawnAI(){
		GameObject aiplayer = Instantiate(AIPrefab, new Vector3(6 - Mathf.Floor(mapSize/2), -6 + Mathf.Floor(mapSize/2), PLAYER_HEIGHT),Quaternion.identity) as GameObject;
		
		playerList.Add(aiplayer);
	}

}
