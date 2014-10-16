using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
	
	public Vector2 gridPosition = Vector2.zero;
	public bool isOccupied = false;
	//
	//Preformed attributes for future implement
	//
	//public GameObject visual;
	//public int movementCost = 1;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnMouseEnter() {
		//When hovering over tile it changes the color
		transform.renderer.material.color = Color.green;

		//Use for debugging
		//Debug.Log("Mouse position is (" + gridPosition.x + "," + gridPosition.y +")");
	}
	
	void OnMouseExit() {
		//When NOT hovering over tile it changes the color
		transform.renderer.material.color = Color.white;
	}
	
	
	void OnMouseDown() {
		if (GameManager.instance.playerList[GameManager.instance.currentPlayerIndex].GetComponent<UserPlayer>().moving) {
			GameManager.instance.MovePlayer(this);
			transform.renderer.material.color = Color.yellow;
		} 
		else if (GameManager.instance.playerList[GameManager.instance.currentPlayerIndex].GetComponent<UserPlayer>().attacking) {
			GameManager.instance.whatPlayerClassIsAttacking(this);
		} 
		else {
//			impassible = impassible ? false : true;
//			if (impassible) {
//				visual.transform.renderer.materials[0].color = new Color(.5f, .5f, 0.0f);
//			} else {
//				visual.transform.renderer.materials[0].color = Color.white;
//			}
		}
		
	}
}
