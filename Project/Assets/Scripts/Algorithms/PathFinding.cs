using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinding : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void doPathFinding(Player playerPosition) {
		int x = playerPosition.gridPosition.x;
		int y = playerPosition.gridPosition.y;
		int range = playerPosition.movementRange;

		List <List<GameObject>> tempMap = GameManager.instance.map;

		// search up
		// search right
		// search down
		// search left
	}
}
