using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinding {

	// User
	public const string ALIEN = "alien";
	public const string HUMAN = "human";

	// Action Code
	public const int ATTACK_HIGHLIGHT = 0;
	public const int MOVE_HIGHLIGHT = 1;


	
	private static void tileActionHandler(int x, int y, int actionCode) {
		if (actionCode == ATTACK_HIGHLIGHT) {
			GameManager.instance.map [x] [y].GetComponent<Tile> ().transform.renderer.material.color = Color.red;
		} else if (actionCode == MOVE_HIGHLIGHT) {
			GameManager.instance.map [x] [y].GetComponent<Tile>().transform.renderer.material.color = Color.blue;
		}
	}

	private static bool humanTileAttackCheck (int x, int y) {
		if (GameManager.instance.map [x] [y].GetComponent<Tile> ().occupiedName == "Mountain" ||
			GameManager.instance.map [x] [y].GetComponent<Tile> ().occupiedName == "Water" ||
			GameManager.instance.map [x] [y].GetComponent<Tile> ().occupiedName == "Soldier" ||
			GameManager.instance.map [x] [y].GetComponent<Tile> ().occupiedName == "Jet" ||
			GameManager.instance.map [x] [y].GetComponent<Tile> ().occupiedName == "Tank") {
			return true;
		} else {
			return false;
		}
	}

	private static bool alienTileAttackCheck (int x, int y) {
		if (GameManager.instance.map [x] [y].GetComponent<Tile> ().occupiedName == "Mountain" ||
		    GameManager.instance.map [x] [y].GetComponent<Tile> ().occupiedName == "Water" ||
		    GameManager.instance.map [x] [y].GetComponent<Tile> ().occupiedName == "Berserk" ||
		    GameManager.instance.map [x] [y].GetComponent<Tile> ().occupiedName == "AlienShip" ||
		    GameManager.instance.map [x] [y].GetComponent<Tile> ().occupiedName == "AlienSoldier") {
			return true;
		} else {
			return false;
		}
	}

	private static void pathFindingAlgorithm(int x, int y, int posX, int posY, int range, int action, string user) {
		if(posX < 0 || posY < 0 || posX >= GameManager.instance.mapSize || posY >= GameManager.instance.mapSize) return;
		if (!(posX == x) || !(posY == y)) {
			if (GameManager.instance.map [posX] [posY].GetComponent<Tile> ().isOccupied) {
				if (action == ATTACK_HIGHLIGHT && user == HUMAN){
					if(humanTileAttackCheck(posX, posY)){
						return;
					}
				} else if (action == ATTACK_HIGHLIGHT && user == ALIEN) {
					if(alienTileAttackCheck(posX, posY)){
						return;
					}
				}
			}
		}
		if (range > 0) {
			tileActionHandler(posX, posY, action);
		}
		if (range == 0) {
			tileActionHandler(posX, posY, action);
			return;
		}
		
		// go up
		pathFindingAlgorithm(x, y, posX, posY + 1, range - 1, action, user);
		// go right
		pathFindingAlgorithm(x, y, posX + 1, posY, range - 1, action, user);
		// go down
		pathFindingAlgorithm(x, y, posX, posY - 1, range - 1, action, user);
		// go left
		pathFindingAlgorithm(x, y, posX - 1, posY, range - 1, action, user);
	}


	/* Path Finding
	 * Precondition: posX >= 0, posY >= 0, range >= 0, action != null, map != 0
	 * Postcondition: Highlight the tile according to action code
	 * Para: posX --- Player postion x
	 * 		 posY --- Player postion y
	 * 		 range --- player attack range or move range
	 * 		 action --- ONLY "attack" or "move"
	 */
	public static void doPathFinding(int posX, int posY, int range, int action, string user) {
		int x = posX;
		int y = posY;
		pathFindingAlgorithm (x, y, posX, posY, range, action, user);
	}
}
