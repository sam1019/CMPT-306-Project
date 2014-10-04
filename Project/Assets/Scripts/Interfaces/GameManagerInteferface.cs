using UnityEngine;
using System.Collections;

public interface GameManagerInteferface {
	void AttackPlayer ();
	void MovePlayer ();
	void enablePathHighlight();
	void disablePathHighlight();
	void generateMap ();
	void loadMapFromXml () ;
	void spawnPlayers ();
	void spawnAI ();
}
