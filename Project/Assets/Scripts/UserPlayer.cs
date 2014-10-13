using UnityEngine;
using System.Collections;


public class UserPlayer : Player {
	
	//Role Name
	public string playerName = "Guardian";
	
	//A 2D Rectangle defined by x, y position and width, height.
	public int attackRange = 1;
	private float bottonWidth;
	private float buttonWidth;
	public int movementRange= 5;
	public float HP=100.0f;
	public float attackHitRate = 0.8f;
	public float defenseReduceRate = 0.2f;
	//public int damageBase = 5;
	//public float damageRollSides = 6;

	
	void Awake(){
		//Setting the destination to it's spawn
		moveDestination = transform.position;
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	public override void Update () {
		//Basic charactor color is blue
		//When a charactor is chosen, it's color will turn to cyan
		//When a charactor die, it will turn to black and destroy, check Player.cs Script Update()
		if(GameManager.instance.playerList.Count > 0 && this.HP > 0){
			if (GameManager.instance.playerList[GameManager.instance.currentPlayerIndex].GetComponent<UserPlayer>() == this && GameManager.instance.playerList.Count > 0) {
				transform.renderer.material.color = Color.cyan;
			}
			//Otherwise charactor is blue
			else {
				transform.renderer.material.color = Color.blue;
			}
		}
		base.Update();
	}
	
	
	// virtual keyword allows child classes to override the method
	public override void TurnUpdate(){
		
		//Moving the player to destination
		if (Vector3.Distance(moveDestination, transform.position) > 0.1f) {
			transform.position += (moveDestination - transform.position).normalized * moveSpeed * Time.deltaTime;
			
			//Used to check if the player has reached it's destination, if so next turn
			if (Vector3.Distance(moveDestination, transform.position) <= 0.1f) {
				transform.position = moveDestination;// + Vector3.back;
				GameManager.instance.nextTurn();
			}
			base.TurnUpdate ();
		}
	}
	
	
	public bool isHit;
	public bool isDefend;
	
	//Hit rate
	public bool Hit(){
		if(Random.Range(0,10000).CompareTo(attackHitRate*10000)<=0){
			isHit=true;
		}
		else{
			isHit=false;
		}
		return isHit;
	}
	
	//HP is decrease after every hit
	public float HPChange (){
		//if hit, do damage; otherwise no damage
		if(isHit==true){
			if(isDefend==false){
				HP=HP-10.0f;
			}
			else{
				HP=HP-10.0f*defenseReduceRate;
			}
		}
		return HP;
	}
	
	public override string roleName(){
		return playerName;
	}
	public virtual void TurnOnGUI(){
	}
	
	//Display HP
	public void OnGUI(){
		Vector3 location = Camera.main.WorldToScreenPoint (transform.position)+ Vector3.up*30+ Vector3.left*15;
		GUI.TextArea(new Rect(location.x, Screen.height - location.y, 30, 20), HP.ToString());
	}
}

