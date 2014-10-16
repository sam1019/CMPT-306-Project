﻿using UnityEngine;
using System.Collections;

public interface GameManagerInteferface {
	void AttackPlayer (Tile destination);
	void MovePlayer (Tile destination);
	void enablePathHighlight();
	void disablePathHighlight();
	void generateMap ();
	void loadMapFromXml () ;
	void spawnPlayers ();
	void spawnAI ();
}
