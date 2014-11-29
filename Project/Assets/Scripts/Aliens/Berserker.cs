using UnityEngine;
using System.Collections;

public class Berserker : AiPlayer {

	// character properties
	public const string className = "Berserker";
	public const float defenseReduceRate = 0.02f;

	// GUI components properties
	private Animator anim;
		
	void Start () {
		this.baseDamage = 10.0f;
		this.baseDefense = 90.0f;
		this.HP = 200.0f;
		this.baseHP = 200.0f;
		this.movementRange = 5;
		this.attackRange = 2;
		this.attackHitRate = 1f;
		anim = gameObject.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	public override void Update () {
		// Basic charactor color is blue
		// When a charactor is chosen, it's color will turn to cyan
		// When a charactor die, it will turn to black and destroy, check Player.cs Script Update()
		if(GameManager.instance.playerList.Count > 0 && this.HP > 0){
			
			if (GameManager.instance.playerList[GameManager.instance.currentPlayerIndex].GetComponent<Berserker>() == this 
			    && GameManager.instance.playerList.Count > 0) {
				
				anim.SetBool("focus", true); //when in its turn, play animation
				transform.renderer.material.color = Color.cyan;
			}
			//Otherwise charactor is blue
			else {
				anim.SetBool("focus", false);//when out its turn, play idle animation
				transform.renderer.material.color = Color.white;
			}
		}
		if (this.HP <= 0) {
			SendMessage("Play","BerserkerDestroy");
			GameManager.instance.playerCount --;
			Destroy(this.gameObject, 1);		
		}
		base.Update();
	}
	
	public override void TurnUpdate (){
		
		if (this.isDecisionMade) {
			// TODO: Debugging code, will be removed later
			//System.Threading.Thread.Sleep(1000);
			
			//print ("HELLO");
			if (Vector3.Distance (moveDestination, transform.position) > 0.1f) {
				//print ("Goodbye");
				transform.position += (moveDestination - transform.position).normalized * moveSpeed * Time.deltaTime;
				
				//Used to check if the player has reached it's destination, if so next turn
				if (Vector3.Distance (moveDestination, transform.position) <= 0.1f) {
					print ("Reached");
					transform.position = moveDestination;
					
					if(this.decisionTreeReturnedCode == this.ATTACK || 
					   this.decisionTreeReturnedCode == this.ATTACK_MOST_DEMAGE || 
					   this.decisionTreeReturnedCode == this.KILL_ONE || 
					   this.decisionTreeReturnedCode == this.CHOOSE_HIGH_HP) {
						
						this.doAttack();
					}
					
					this.decisionExecuted = true;
					GameManager.instance.disableHightLight ();
					if (this.decisionExecuted) {
						this.decisionExecuted = false;
						//attackTurn = false;
						this.isDecisionMade = false;
						this.resetTargets();
						GameManager.instance.nextTurn ();
					}
				}			
				base.TurnUpdate ();
			}
			
		} else {
			if(GameManager.instance.playerCount <= 0) return;
			
			this.decisionTreeReturnedCode = this.decisionTree();
			Debug.Log ("AlienSoldier Decision Tree Code: " + this.decisionTreeReturnedCode);
			
			if (this.decisionTreeReturnedCode == this.ATTACK) {
				//Debug.Log ("Called");
				this.target = this.targets[0].GetComponent<Player>();
				if (this.target != null) {
					this.moveToAttackableRangeAction();
				} else {
					Debug.LogError("AI Attack Target is NULL!!!");
				}
			} else if (this.decisionTreeReturnedCode == this.KILL_ONE) {
				
				this.target = this.ableToBeKilledTargets[0].GetComponent<Player>();
				if (this.target != null) {
					this.moveToAttackableRangeAction();
				} else {
					Debug.LogError("AI Attack Target is NULL!!!");
				}
			} else if (this.decisionTreeReturnedCode == this.CHOOSE_HIGH_HP) {
				this.findHighHPforAbleToKill();
				this.target = this.ableToBeKilledTargets[this.HighHPforAbleToKillIndex].GetComponent<Player>();
				if (this.target != null) {
					this.moveToAttackableRangeAction();
				} else {
					Debug.LogError("AI Attack Target is NULL!!!");
				}
			} else if (this.decisionTreeReturnedCode == this.ATTACK_MOST_DEMAGE) {
				this.findMostDamageTarget();
				this.target = this.targets[this.MostDamageTargetIndex].GetComponent<Player>();
				if (this.target != null) {
					this.moveToAttackableRangeAction();
				} else {
					Debug.LogError("AI Attack Target is NULL!!!");
				}
			} else if (this.decisionTreeReturnedCode == this.MOVE_TO_PLAYER) {
				
				this.findHighestPreferenceTile ();
				//Debug.Log ("Preference Tile X: " + this.preferenceTileX);
				//Debug.Log ("Preference Tile Y: " + this.preferenceTileY);
				this.moveToHighPrefenceAction();
			}
			
			this.isDecisionMade = true;
		}
	}
	
	/*
	 * Gets current player's grid position
	 */ 
	// TODO: Used for testing, may not need for future
	public Tile getGridPosition(){
		
		int x = (int) this.gridPosition.x;
		int y = (int)this.gridPosition.y;
		Tile tile = GameManager.instance.map [x] [y].GetComponent<Tile> ();
		return tile;
	}
	
	
	//Return the class name of unit
	public override string roleName(){
		
		return className;
	}


}
