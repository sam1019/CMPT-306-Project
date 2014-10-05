using UnityEngine;
using System.Collections;

//This class acts a interface class that inherits from MonoBehaviour
public class Player : MonoBehaviour {
	
	public Vector3 destination;
	public float moveSpeed = 10.0f;
	
	void Awake () {
		destination = transform.position;
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public virtual void TurnUpdate () {
		
	}
}
