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
	public List <GameObject> aiList;
	public Transform mapTransform;
	GameObject player;
	Tile grid;
	public int mapSize = 11; //The size of the map i 


	private const float PLAYER_HEIGHT = -1.0f; 	//Used to spawn game objects 1.5 above the map so they are not in collision
	private const float AI_HEIGHT = -0.25f;
	public int currentPlayerIndex;//Iterates throught the player list
	public int currentAIIndex; //Iterates throught the AI list
	

	public bool IsPause; 
	public int scores;	

	void Awake(){
		instance = this;

	
	}
	void Start () {
		playerList = new List<GameObject> ();
		aiList = new List<GameObject> ();
		int currentPlayerIndex = 0;
		generateMap (); //Generate map
		spawnPlayers(); //spawn players to be controlled by users
		spawnAI(); //Spawn AI opponent for the player

		IsPause = true;
		scores = 0;
	}
	

	void Update () {
		if (playerList [currentPlayerIndex] == null) {
			print ("NULL");		
		}
		GameObject temp = playerList [currentPlayerIndex];
		UserPlayer user  = temp.GetComponent<UserPlayer>();
		user.TurnUpdate ();

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
		GameObject temp = playerList [currentPlayerIndex];
		UserPlayer user  = temp.GetComponent<UserPlayer>();
		user.moveDestination = destination.transform.position + PLAYER_HEIGHT * Vector3.up;
		user.gridPosition = destination.gridPosition;
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


	}
	public void spawnAI(){

		GameObject aiplayer = Instantiate(AIPrefab, new Vector3(6 - Mathf.Floor(mapSize/2), -6 + Mathf.Floor(mapSize/2), PLAYER_HEIGHT),Quaternion.identity) as GameObject;
		
		aiList.Add(aiplayer);
	}

	//added panel from here 
	void OnGUI()
	{	
		
		//Pause 
		if (GUI.Button (new Rect (100, 140, 80, 20), "Pause")) // make the GUI button with name "pause"
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
		
		if (GUI.Button (new Rect (100,240, 80, 20), "Save"))// this function to save the game
		{
			PlayerPrefs.SetInt("save player",currentPlayerIndex);
			PlayerPrefs.Save();
		}
		
		if (GUI.Button (new Rect (100,340, 80, 20), "End Turn"))// this function to save the game
		{
			PlayerPrefs.SetInt("End turn",currentPlayerIndex);
			this.nextTurn();
		}
		
		
		//here is the GUI for outputing score, now do nothing yet.
		GUI.Label (new Rect (10, 10, 50, 20), "Score:" + scores.ToString ());// we will add the player's scores here
		GUI.Label (new Rect (10, 30, 50, 20), "Lives:" + scores.ToString ());// we will add the lives here depending on the player, by passing variable from player attack
		
		//here is the GUI to show the next turn
		GUI.Label (new Rect (Screen.width - 100, 0, 100, 50), "End turn:"+ scores.ToString ());

		
	}	
}
