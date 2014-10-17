using UnityEngine;
using System.Collections;

public class Jet : Player {

	public const string className = "Jet";
	public const float baseDamage = 25.0f;
	public const float baseDefense = 15.0f;
	public int movementRange = 5;
	public float HP;
	
	public int attackRange = 2;
	private float bottonWidth;
	private float buttonWidth;
	
	public float attackHitRate = 0.8f;
	public float defenseReduceRate = 0.2f;
	
	void Start () {
		HP = 120.0f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void getEnemyToAttack(Tile tile){
		foreach (GameObject p in GameManager.instance.playerList) {
			if(p.GetComponent<AlienShip>() != null){
				AlienShip target = null;
				AlienShip temp = p.GetComponent<AlienShip>();
				
				if (temp.gridPosition == tile.gridPosition) { //Checks if tile selected contains enemy
					target = temp;
					JetAttack.attackAlienShip(target);
				}
				else{
					print("Target not found");
				}
			}
			else if(p.GetComponent<AlienSoldier>() != null){
				AlienSoldier target = null;
				AlienSoldier temp = p.GetComponent<AlienSoldier>();				
				if (temp.gridPosition == tile.gridPosition) { //Checks if tile selected contains enemy
					target = temp;
					TankAttack.attackAlienSoldier(target);
				}
				else{
					print("Target not found");
				}
			}
			else if(p.GetComponent<AlienSupport>() != null){
				AlienSupport target = null;
				AlienSupport temp = p.GetComponent<AlienSupport>();				
				if (temp.gridPosition == tile.gridPosition) { //Checks if tile selected contains enemy
					target = temp;
					TankAttack.attackAlienSupport(target);
				}
				else{
					print("Target not found");
				}
			}
			else if(p.GetComponent<Berserker>() != null){
				Berserker target = null;
				Berserker temp = p.GetComponent<Berserker>();				
				if (temp.gridPosition == tile.gridPosition) { //Checks if tile selected contains enemy
					target = temp;
					TankAttack.attackAlienBerserker(target);
				}
				else{
					print("Target not found");
				}
			}
			
		}
		
	}
}
