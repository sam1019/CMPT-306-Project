using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour,  GameManagerInteferface {

	public GameObject Players;
	public static GameManager instance;
	public GameObject AI;
	public GameObject map;
	public List <Player> playerList = new List<Player>();
	public List <AI> AIList = new List<AI> ();
	public Transform mapTransform;
	private int mapiSize; //The size of the map i 

	public int currentPlayerIndex = 0;//Iterates throught the player list
	public int currentAIIndex =0; //Iterates throught the AI list


	void Awake(){
		instance = this;
		mapTransform.transform.FindChild ("MAP NAME HERE");
	}
	void Start () {
		generateMap (); //Generate map
		spawnPlayers(); //spawn players to be controlled by users
		spawnAI(); //Spawn AI opponent for the player
	}
	

	void Update () {
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
		}
	}
	public void AttackPlayer(){
	
	}
	public void MovePlayer(){

	}
	public void enablePathHighlight(){

	}

	public void disablePathHighlight(){
	
	}
	public void generateMap(){
		loadMapFromXml();
	}
	public void loadMapFromXml() {
	}

	public void spawnPlayers(){
	
	}
	public void spawnAI(){
	
	}

}
