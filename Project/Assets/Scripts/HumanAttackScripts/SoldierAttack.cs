using UnityEngine;
using System.Collections;

public class SoldierAttack : MonoBehaviour {

	public static bool isMissed;
	
	// Use this for initialization
	void Start () {
		isMissed=false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	/*
	 *Checks if target is within attacking range of player 
	 */
	public static bool isTargetInRange(int playerX, int playerY, int targetX, int targetY, int attackRange){
		return(playerX >= targetX - attackRange && playerX <= targetX + attackRange &&
		       playerY >= targetY - attackRange && playerY <= targetY + attackRange);
	}
	
	/*
	 * If target is alien soldier it will attack correct enemy 
	 */
	public static void attackAlienSoldier(AlienSoldier target){
		if (target != null) {
			Soldier soldier = GameManager.instance.playerList[GameManager.instance.currentPlayerIndex].GetComponent<Soldier>();
			if(isTargetInRange((int)soldier.gridPosition.x, (int)soldier.gridPosition.y,
			                   (int)target.gridPosition.x,(int)target.gridPosition.y, soldier.attackRange)){
				soldier.actionPoints--;
				
				soldier.attacking = false;			
				
				//attack logic
				//roll to hit
				bool hit = Random.Range(0.0f, 1.0f) <= soldier.attackHitRate;
				
				if (hit) {
					//damage logic
					//In future damage dealt will take in account for target's defense
					int amountOfDamage = (int)Mathf.Floor(10 + Random.Range(0, 6));
					
					target.HP -= amountOfDamage;
				} else {
					print ("Missed");
					isMissed=true;
				}
				
				//GameManager.instance.nextTurn(); //After attacking end turn
			} else {
				Debug.Log ("Target is out of range!");
			}
		}
	}
	
	/*
	 * If target is alien ship it will attack correct enemy 
	 */
	public static void attackAlienShip(AlienShip target){
		if (target != null) {
			Soldier soldier = GameManager.instance.playerList[GameManager.instance.currentPlayerIndex].GetComponent<Soldier>();
			if(isTargetInRange((int)soldier.gridPosition.x, (int)soldier.gridPosition.y,
			                   (int)target.gridPosition.x,(int)target.gridPosition.y, soldier.attackRange)){
				soldier.actionPoints--;
				
				soldier.attacking = false;			
				
				//attack logic
				//roll to hit
				bool hit = Random.Range(0.0f, 1.0f) <= soldier.attackHitRate;
				
				if (hit) {
					//damage logic
					//In future damage dealt will take in account for target's defense
					int amountOfDamage = (int)Mathf.Floor(10 + Random.Range(0, 6));
					
					target.HP -= amountOfDamage;
				} else {
					print ("Missed");
					isMissed=true;
				}
				
				//GameManager.instance.nextTurn(); //After attacking end turn
			} else {
				Debug.Log ("Target is out of range!");
			}
		}
	}

	
	/*
	 * If target is alien berserker it will attack correct enemy 
	 */
	public static void attackAlienBerserker(Berserker target){
		if (target != null) {
			Soldier soldier = GameManager.instance.playerList[GameManager.instance.currentPlayerIndex].GetComponent<Soldier>();
			if(isTargetInRange((int)soldier.gridPosition.x, (int)soldier.gridPosition.y,
			                   (int)target.gridPosition.x,(int)target.gridPosition.y, soldier.attackRange)){
				soldier.actionPoints--;
				
				soldier.attacking = false;			
				
				//attack logic
				//roll to hit
				bool hit = Random.Range(0.0f, 1.0f) <= soldier.attackHitRate;
				
				if (hit) {
					//damage logic
					//In future damage dealt will take in account for target's defense
					int amountOfDamage = (int)Mathf.Floor(10 + Random.Range(0, 6));
					
					target.HP -= amountOfDamage;
				} else {
					print ("Missed");
					isMissed=true;
				}
				
				//GameManager.instance.nextTurn(); //After attacking end turn
			} else {
				Debug.Log ("Target is out of range!");
			}
		}
	}
	
	public static void attackAIPlayer(AiPlayer target){
		if (target != null) {
			Soldier soldier = GameManager.instance.playerList[GameManager.instance.currentPlayerIndex].GetComponent<Soldier>();
			if(isTargetInRange((int)soldier.gridPosition.x, (int)soldier.gridPosition.y,
			                   (int)target.gridPosition.x,(int)target.gridPosition.y, soldier.attackRange)){
				soldier.actionPoints--;
				
				soldier.attacking = false;			
				
				//attack logic
				//roll to hit
				bool hit = Random.Range(0.0f, 1.0f) <= soldier.attackHitRate;
				
				if (hit) {
					//damage logic
					//In future damage dealt will take in account for target's defense
					int amountOfDamage = (int)Mathf.Floor(10 + Random.Range(0, 6));
					
					target.HP -= amountOfDamage;
				} else {
					print ("Missed");
					isMissed=true;
				}
				
				//GameManager.instance.nextTurn(); //After attacking end turn
			} else {
				Debug.Log ("Target is out of range!");
			}
		}
	}
	

	void OnGUI(){
		float widthScale = 0.40f;
		float heightScale = 0.5f;
		//Debug.Log ("In OnGUI");
		if(isMissed){
			Time.timeScale=0;
			Rect windowRect = new Rect (Screen.width * (1 - widthScale)/2, Screen.height * (1 - heightScale)/2, Screen.width * widthScale, Screen.height * heightScale);
			windowRect = GUI.Window(0, windowRect, ShowPopupWindow, "Attack Missed");
		}
	}
	
	/*
	 * Popup Window contain detail
	 */
	void ShowPopupWindow(int WindowID){
		float widthScale = 0.40f;
		float heightScale = 0.5f;
		float widthScale2 = 0.40f;
		float heightScale2 = 0.15f;
		
		//Back to Game button
		if (GUI.Button(new Rect (Screen.width * (1 - widthScale)/2*(1 - widthScale2+0.245f)/2, Screen.height * (1 - heightScale)/2*(1 - heightScale2+1.5f)/2*0.8f, Screen.width * widthScale*widthScale2, Screen.height * heightScale*heightScale2), "Back To Game"))
		{
			isMissed = false;	//Disable popup window
			Time.timeScale=1;		//Game run again
		}
	}
}
