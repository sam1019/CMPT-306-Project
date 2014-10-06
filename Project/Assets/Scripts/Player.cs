using UnityEngine;
using System.Collections;

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
