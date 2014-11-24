using UnityEngine;
using System.Collections;

public class HelicopterAttack : MonoBehaviour {
	
	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}
	
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
			Helicopter heli = GameManager.instance.playerList[GameManager.instance.currentPlayerIndex].GetComponent<Helicopter>();
			
			if(isTargetInRange((int)heli.gridPosition.x, (int)heli.gridPosition.y,
			                   (int)target.gridPosition.x,(int)target.gridPosition.y, heli.attackRange)){
				
				heli.actionPoints--;
				heli.attacking = false;			
				
				//attack logic
				//roll to hit
				bool hit = Random.Range(0.0f, 1.0f) <= heli.attackHitRate;
				
				if (hit) {
					//damage logic
					int amountOfDamage = (int)Mathf.Floor(10 + Random.Range(0, 6)); //Arbitrary numbers for now
					target.HP -= amountOfDamage;
				} else {
					print ("Missed");
				}
				
				//GameManager.instance.nextTurn();
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
			Helicopter heli = GameManager.instance.playerList[GameManager.instance.currentPlayerIndex].GetComponent<Helicopter>();
			
			if(isTargetInRange((int)heli.gridPosition.x, (int)heli.gridPosition.y,
			                   (int)target.gridPosition.x,(int)target.gridPosition.y, heli.attackRange)){
				
				heli.actionPoints--; //Decrement action points
				heli.attacking = false;			
				
				//attack logic
				//roll to hit
				bool hit = Random.Range(0.0f, 1.0f) <= heli.attackHitRate;
				
				if (hit) {
					//damage logic
					//In future damage dealt will take in account for target's defense
					int amountOfDamage = (int)Mathf.Floor(10 + Random.Range(0, 6));
					target.HP -= amountOfDamage;
				} else {
					print ("Missed");
				}
				
				//GameManager.instance.nextTurn(); //After attacking end turn
			} else {
				Debug.Log ("Target is out of range!");
			}
		}
	}
	
	/*
	 * If target is alien support it will attack correct enemy 
	 */
	public static void attackAlienSupport(AlienSupport target){
		
		if (target != null) {
			Helicopter heli = GameManager.instance.playerList[GameManager.instance.currentPlayerIndex].GetComponent<Helicopter>();
			
			if(isTargetInRange((int)heli.gridPosition.x, (int)heli.gridPosition.y,
			                   (int)target.gridPosition.x,(int)target.gridPosition.y, heli.attackRange)){
				heli.actionPoints--; //Decrement action points
				heli.attacking = false;			
				
				//attack logic
				//roll to hit
				bool hit = Random.Range(0.0f, 1.0f) <= heli.attackHitRate;
				
				if (hit) {
					//damage logic
					//In future damage dealt will take in account for target's defense
					int amountOfDamage = (int)Mathf.Floor(10 + Random.Range(0, 6));
					
					target.HP -= amountOfDamage;
				} else {
					print ("Missed");
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
			Helicopter heli = GameManager.instance.playerList[GameManager.instance.currentPlayerIndex].GetComponent<Helicopter>();
			if(isTargetInRange((int)heli.gridPosition.x, (int)heli.gridPosition.y,
			                   (int)target.gridPosition.x,(int)target.gridPosition.y, heli.attackRange)){
				heli.actionPoints--; //Decrement action points
				
				heli.attacking = false;			
				
				//attack logic
				//roll to hit
				bool hit = Random.Range(0.0f, 1.0f) <= heli.attackHitRate;
				
				if (hit) {
					//damage logic
					//In future damage dealt will take in account for target's defense
					int amountOfDamage = (int)Mathf.Floor(10 + Random.Range(0, 6));
					
					target.HP -= amountOfDamage;
				} else {
					print ("Missed");
				}
				
				//GameManager.instance.nextTurn(); //After attacking end turn
			} else {
				Debug.Log ("Target is out of range!");
			}
		}
	}
	public static void attackAIPlayer(AiPlayer target){
		if (target != null) {
			Helicopter heli = GameManager.instance.playerList[GameManager.instance.currentPlayerIndex].GetComponent<Helicopter>();
			if(isTargetInRange((int)heli.gridPosition.x, (int)heli.gridPosition.y,
			                   (int)target.gridPosition.x,(int)target.gridPosition.y, heli.attackRange)){
				heli.actionPoints--; //Decrement action points
				
				heli.attacking = false;			
				
				//attack logic
				//roll to hit
				bool hit = Random.Range(0.0f, 1.0f) <= heli.attackHitRate;
				
				if (hit) {
					//damage logic
					//In future damage dealt will take in account for target's defense
					int amountOfDamage = (int)Mathf.Floor(10 + Random.Range(0, 6));
					
					target.HP -= amountOfDamage;
				} else {
					print ("Missed");
				}
				
				//GameManager.instance.nextTurn(); //After attacking end turn
			} else {
				Debug.Log ("Target is out of range!");
			}
		}
	}
}
