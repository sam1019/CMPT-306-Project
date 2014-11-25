using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinding {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private static void pathFindingAlgorithm(int x, int y, int posX, int posY, int range, string action) {
		if(posX < 0 || posY < 0 || posX >= GameManager.instance.mapSize || posY >= GameManager.instance.mapSize) return;
		if (posX == x && posY == y) {
		} else {
			if (GameManager.instance.map [posX] [posY].GetComponent<Tile> ().isOccupied) {
				return;
			}
		}
		if (range > 0) {
			// check if it's out of border
			if(action == "attack") {
				GameManager.instance.map [posX] [posY].GetComponent<Tile> ().transform.renderer.material.color = Color.red;
			} else if(action == "move") {
				GameManager.instance.map[posX][posY].GetComponent<Tile>().transform.renderer.material.color = Color.blue;
			} else {
				Debug.Log("Invalid action code for PathFinding");
			}
		}
		if (range == 0) {
			// check if it's out of border
			if(action == "attack") {
				GameManager.instance.map [posX] [posY].GetComponent<Tile> ().transform.renderer.material.color = Color.red;
			} else if(action == "move") {
				GameManager.instance.map[posX][posY].GetComponent<Tile>().transform.renderer.material.color = Color.blue;
			} else {
				Debug.Log("Invalid action code for PathFinding");
			}
			return;
		}
		
		// go up
		pathFindingAlgorithm(x, y, posX, posY + 1, range - 1, action);
		// go right
		pathFindingAlgorithm(x, y, posX + 1, posY, range - 1, action);
		// go down
		pathFindingAlgorithm(x, y, posX, posY - 1, range - 1, action);
		// go left
		pathFindingAlgorithm(x, y, posX - 1, posY, range - 1, action);
	}
	
	/* Path Finding
	 * Precondition: posX >= 0, posY >= 0, range >= 0, action != null, map != 0
	 * Postcondition: Highlight the tile according to action code
	 * Para: posX --- Player postion x
	 * 		 posY --- Player postion y
	 * 		 range --- player attack range or move range
	 * 		 action --- ONLY "attack" or "move"
	 */
	public static void doPathFinding(int posX, int posY, int range, string action) {
		int x = posX;
		int y = posY;
		pathFindingAlgorithm (x, y, posX, posY, range, action);
	}
}
