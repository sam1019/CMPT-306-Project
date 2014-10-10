using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This class acts a interface class that inherits from MonoBehaviour
public class Player : MonoBehaviour {
	
	public Vector3 moveDestination;
	public float moveSpeed = 10.0f;
	
	public Vector2 gridPosition = Vector2.zero;
	
	//charactor movement range & attack range in each actoin
	public int movementRange= 5;
	public int attackRange = 1;
	
	//boolean to judge moveing and attack condition
	public bool moving= false;
	public bool attacking=false;
	
	public float HP=100.0f;
	public float attackHitRate = 0.8f;
	public float defenseReduceRate = 0.2f;
	//public int damageBase = 5;
	//public float damageRollSides = 6;
	public int actionPoints = 1;
	
	
	//For movement in GameManager.MovePlayer()
	public List<Vector3> positionQueue = new List<Vector3>();
	
	
	
	void Awake () {
		moveDestination = transform.position;
	}
	
	// Use this for initialization
	void Start () {
		
	}

	// virtual keyword allows child classes to override the method
	public virtual void Update () {
		//Checks if player is dead or not
//		if (this.HP <= 0) {
//			//Returns a rotation that rotates z degrees around the z axis, 
//			//x degrees around the x axis, and y degrees around the y axis (in that order).
//			//transform.rotation = Quaternion.Euler(new Vector2(90,0));
//			this.transform.renderer.material.color = Color.black;
//			//Delay a second after the play die
//			Destroy(this.transform.gameObject, 1.0f);
//		} 
	}
	
	//Each charactor in each turn can do 2 actions in total
	// virtual keyword allows child classes to override the method
	public virtual void TurnUpdate () {
		if (actionPoints <= 0) {
			actionPoints = 1;
			moving = false;
			attacking = false;			
			GameManager.instance.nextTurn();
		}
	}
	
	public virtual void TurnOnGUI(){
	}

	//Display HP
	public void OnGUI(){
		Vector3 location = Camera.main.WorldToScreenPoint (transform.position)+ Vector3.up*30+ Vector3.left*15;
		GUI.TextArea(new Rect(location.x, Screen.height - location.y, 30, 20), HP.ToString());
	}

	public virtual string roleName (){
		return null;
	}
	
	public void GetPositionX(){
	}
	
	public void GetPositioinY(){
	}
	
	public void isMoved(){
		//return null;
	}
	
	
}
