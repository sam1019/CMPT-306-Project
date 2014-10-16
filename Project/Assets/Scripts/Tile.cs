using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
	
	public Vector2 gridPosition = Vector2.zero;
	public bool isOccupied = false;
	
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

		GameManager.instance.MovePlayer(this);
		GameManager.instance.whatPlayerClassIsAttacking (this);
		
	}
}
