using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour {
	
	public Vector2 gridPosition = Vector2.zero;
	public bool isOccupied = false;
	public List<Tile> neighbors = new List<Tile>();
	public GameObject visual;
    private Color tempColorRecord;
    
	//
	//Preformed attributes for future implement
	//
	//public GameObject visual;
	//public int movementCost = 1;
	//
	
	// Use this for initialization
	void Start () {
		neighbors = new List<Tile>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnMouseEnter() {
        //First record the origianl tile color
		//When hovering over tile it changes the color to become green
		tempColorRecord = transform.renderer.material.color;
		transform.renderer.material.color = Color.green;

		//Use for debugging
		//Debug.Log("Mouse position is (" + gridPosition.x + "," + gridPosition.y +")");
	}
	
	void OnMouseExit() {
		//When NOT hovering over tile it changes to the original color
		transform.renderer.material.color = tempColorRecord;
	}
	public bool isPlayerMoving(){
		if (GameManager.instance.playerList [GameManager.instance.currentPlayerIndex].GetComponent<UserPlayer> () != null) {
			return GameManager.instance.playerList [GameManager.instance.currentPlayerIndex].GetComponent<UserPlayer> ().moving;
		}
		else if (GameManager.instance.playerList [GameManager.instance.currentPlayerIndex].GetComponent<Soldier> () != null) {
			return GameManager.instance.playerList [GameManager.instance.currentPlayerIndex].GetComponent<Soldier> ().moving;
		}
		else if (GameManager.instance.playerList [GameManager.instance.currentPlayerIndex].GetComponent<Medic> () != null) {
			return GameManager.instance.playerList [GameManager.instance.currentPlayerIndex].GetComponent<Medic> ().moving;
		}
		else if (GameManager.instance.playerList [GameManager.instance.currentPlayerIndex].GetComponent<Tank> () != null) {
			return GameManager.instance.playerList [GameManager.instance.currentPlayerIndex].GetComponent<Tank> ().moving;
		}
		else if (GameManager.instance.playerList [GameManager.instance.currentPlayerIndex].GetComponent<Helicopter> () != null) {
			return GameManager.instance.playerList [GameManager.instance.currentPlayerIndex].GetComponent<Helicopter> ().moving;
		}
		else if (GameManager.instance.playerList [GameManager.instance.currentPlayerIndex].GetComponent<Jet> () != null) {
			return GameManager.instance.playerList [GameManager.instance.currentPlayerIndex].GetComponent<Jet> ().moving;
		}
		else{
			return false;
		}
	}
	public bool isPlayerAttacking(){
		if (GameManager.instance.playerList [GameManager.instance.currentPlayerIndex].GetComponent<UserPlayer> () != null) {
			return GameManager.instance.playerList [GameManager.instance.currentPlayerIndex].GetComponent<UserPlayer> ().isAttacking;
		}
		else if (GameManager.instance.playerList [GameManager.instance.currentPlayerIndex].GetComponent<Soldier> () != null) {
			return GameManager.instance.playerList [GameManager.instance.currentPlayerIndex].GetComponent<Soldier> ().isAttacking;
		}
		else if (GameManager.instance.playerList [GameManager.instance.currentPlayerIndex].GetComponent<Medic> () != null) {
			return GameManager.instance.playerList [GameManager.instance.currentPlayerIndex].GetComponent<Medic> ().isAttacking;
		}
		else if (GameManager.instance.playerList [GameManager.instance.currentPlayerIndex].GetComponent<Tank> () != null) {
			return GameManager.instance.playerList [GameManager.instance.currentPlayerIndex].GetComponent<Tank> ().isAttacking;
		}
		else if (GameManager.instance.playerList [GameManager.instance.currentPlayerIndex].GetComponent<Helicopter> () != null) {
			return GameManager.instance.playerList [GameManager.instance.currentPlayerIndex].GetComponent<Helicopter> ().isAttacking;
		}
		else if (GameManager.instance.playerList [GameManager.instance.currentPlayerIndex].GetComponent<Jet> () != null) {
			return GameManager.instance.playerList [GameManager.instance.currentPlayerIndex].GetComponent<Jet> ().isAttacking;
		}
		else{
			return false;
		}
	}
	
	void OnMouseDown() {
		if (isPlayerMoving()) {
			GameManager.instance.MovePlayer(this);
			transform.renderer.material.color = Color.yellow;
		} 
		else if (isPlayerAttacking()) {
			GameManager.instance.whatPlayerClassIsAttacking(this);
		} 
		/*
		else {
			impassible = impassible ? false : true;
			if (impassible) {
				visual.transform.renderer.materials[0].color = new Color(.5f, .5f, 0.0f);
			} else {
				visual.transform.renderer.materials[0].color = Color.white;
			}
		}*/
		
	}
}
