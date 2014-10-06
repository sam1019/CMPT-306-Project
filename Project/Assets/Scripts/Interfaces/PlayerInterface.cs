using UnityEngine;
using System.Collections;

public interface PlayerInterface {
	
	void DisplayHP();

	void roleName();

	void GetPositionX();

	void GetPositioinY();

	void isMoved();

	void TurnUpdate();

	void TurnOnGUI ();

	void OnGUI();

}