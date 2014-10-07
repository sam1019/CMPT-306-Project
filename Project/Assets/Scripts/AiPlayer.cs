using UnityEngine;
using System.Collections;


public class AiPlayer : Player {
	
	// Use this for initialization
	public  void Start () {
		
	}
	
	// Update is called once per frame
	public  void Update () {
		if (this.HP <= 0) {
			this.transform.renderer.material.color = Color.black;
		} 
	}
	public override void TurnUpdate (){
		GameManager.instance.nextTurn();
		base.TurnUpdate ();
	}
	
}
